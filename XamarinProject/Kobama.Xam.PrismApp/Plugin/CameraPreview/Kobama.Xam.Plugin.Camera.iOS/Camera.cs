﻿// -----------------------------------------------------------------------
// <copyright file="Camera.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
#pragma warning disable SA1300
namespace Kobama.Xam.Plugin.Camera.iOS
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using AVFoundation;
    using CoreFoundation;
    using CoreGraphics;
    using CoreMedia;
    using CoreVideo;
    using Foundation;
    using Kobama.Xam.Plugin.Camera.Options;
    using Kobama.Xam.Plugin.Log;
    using UIKit;

    /// <summary>
    /// Camera Class
    /// </summary>
    /// <seealso cref="Kobama.Xam.Plugin.Camera.ICameraControl" />
    public class Camera : ICameraControl
    {
        private readonly Logger logger = new Logger(nameof(Camera));
        private readonly float minZoom = 1.0f;
        private float maxZoom;
        private AVCaptureVideoPreviewLayer mPreviewLayer;

        private Camera()
        {
        }

        /// <summary>
        /// Occurs when callabck opened.
        /// </summary>
        public event Opened CallabckOpened;

        /// <summary>
        /// Occurs when callback received image.
        /// </summary>
        public event SavedImage CallbackSavedImage;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static Camera Instance { get; } = new Camera();

        /// <summary>
        /// Gets the main device.
        /// </summary>
        /// <value>
        /// The main device.
        /// </value>
        public AVCaptureDevice MainDevice { get; private set; }

        /// <summary>
        /// Gets the capture session.
        /// </summary>
        /// <value>
        /// The capture session.
        /// </value>
        public AVCaptureSession CaptureSession { get; private set; }

        /// <summary>
        /// Gets or sets the input.
        /// </summary>
        /// <value>
        /// The input.
        /// </value>
        public AVCaptureDeviceInput Input { get; set; }

        /// <summary>
        /// Gets the output.
        /// </summary>
        /// <value>
        /// The output.
        /// </value>
        public AVCaptureVideoDataOutput Output { get; private set; }

        /// <summary>
        /// Gets the still output.
        /// </summary>
        /// <value>
        /// The still output.
        /// </value>
        public AVCaptureStillImageOutput StillOutput { get; private set; }

        /// <summary>
        /// Gets or sets the recorder.
        /// </summary>
        /// <value>
        /// The recorder.
        /// </value>
        public OutputRecorder Recorder { get; set; }

        /// <summary>
        /// Gets or sets the queue.
        /// </summary>
        /// <value>
        /// The queue.
        /// </value>
        public DispatchQueue Queue { get; set; }

        /// <summary>
        /// Gets or sets the image mode.
        /// </summary>
        /// <value>
        /// The image mode.
        /// </value>
        public ImageMode ImageMode { get; set; } = ImageMode.EachFrame;

        /// <summary>
        /// Gets or sets the camera view.
        /// </summary>
        /// <value>
        /// The camera view.
        /// </value>
        public UIView CameraView { get; set; }

        /// <summary>
        /// Gets the preview layer.
        /// </summary>
        public AVCaptureVideoPreviewLayer PreviewLayer
        {
            get { return this.mPreviewLayer; }
        }

        /// <summary>
        /// Gets or sets the lens.
        /// </summary>
        /// <value>The lens.</value>
        public CameraLens Lens { get; set; } = CameraLens.Front;

        /// <summary>
        /// Changes the lens.
        /// </summary>
        /// <param name="lens">Lens.</param>
        public void ChangeLens(CameraLens lens)
        {
            if (this.Lens != lens)
            {
                this.CloseCamera();
                this.Lens = lens;
                this.OpenCamera();
            }
        }

        /// <summary>
        /// Gets the fps range list.
        /// </summary>
        /// <returns>
        /// The fps range list.
        /// </returns>
        public List<CameraFpsRange> GetFpsRangeList()
        {
            var list = new List<CameraFpsRange>();

            if (this.MainDevice != null)
            {
                var rangeList = this.MainDevice.ActiveFormat.VideoSupportedFrameRateRanges;

                foreach (var range in rangeList)
                {
                    list.Add(new CameraFpsRange((int)range.MinFrameRate, (int)range.MaxFrameRate));
                }
            }

            return list;
        }

        /// <summary>
        /// Gets the size list.
        /// </summary>
        /// <returns>
        /// The size list.
        /// </returns>
        public List<Size> GetSizeList()
        {
            var list = new List<Size>();
            return list;
        }

        /// <summary>
        /// Ons the destroy.
        /// </summary>
        public void OnDestroy()
        {
            this.Dispose();
        }

        /// <summary>
        /// Ons the pause.
        /// </summary>
        public void OnPause()
        {
            this.CloseCamera();
        }

        /// <summary>
        /// Ons the resume.
        /// </summary>
        public void OnResume()
        {
            this.Initialize();
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            this.CloseCamera();
            this.CaptureSession?.Dispose();
            this.CaptureSession = null;
        }

        /// <summary>
        /// Raiseds the image.
        /// </summary>
        /// <param name="image">Image.</param>
        /// <param name="size">Size.</param>
        public void RaisedImage(byte[] image, Size size)
        {
            this.CallbackSavedImage.Invoke(image, size);
        }

        /// <summary>
        /// Sets the option AE mode.
        /// </summary>
        /// <param name="range">The range.</param>
        public void SetOptionAEMode(CameraFpsRange range)
        {
        }

        /// <summary>
        /// Takes the picture.
        /// </summary>
        public void TakePicture()
        {
            var connection = this.StillOutput.ConnectionFromMediaType(AVMediaType.Video);

            if (connection == null)
            {
                this.logger.Error("Connection is null");
                return;
            }

            this.StillOutput.CaptureStillImageAsynchronously(connection, (sampleBuffer, err) =>
            {
                var imageData = AVCaptureStillImageOutput.JpegStillToNSData(sampleBuffer);
                var uiImage = new UIImage(imageData);
                var cgImage = uiImage.CGImage;
                var newuiImage = new UIImage(cgImage, new nfloat(1.0), this.GetPhotoOrientation());
                if (newuiImage == null)
                {
                    this.logger.CalledMethod($"new UIImage is null");
                }

                uiImage = newuiImage;

                this.logger.CalledMethod($"uiImage orientation:{uiImage.Orientation.ToString()}");

                byte[] bytes = null;
                using (var data = uiImage.AsJPEG())
                {
                    bytes = data.ToArray();
                    this.RaisedImage(bytes, new Size((int)uiImage.Size.Width, (int)uiImage.Size.Height));
                }

                // These codes should be fixed with reference to http://hiro128.hatenablog.jp/entry/2017/09/13/203715
            });
        }

        /// <summary>
        /// Closes the camera.
        /// </summary>
        public void CloseCamera()
        {
            this.logger.CalledMethod();
            if (this.CaptureSession == null)
            {
                return;
            }

            this.CaptureSession.StopRunning();

            if (this.Recorder != null)
            {
                this.Recorder.Dispose();
                this.Recorder = null;
            }

            if (this.Queue != null)
            {
                this.Queue.Dispose();
                this.Queue = null;
            }

            if (this.Output != null)
            {
                this.CaptureSession.RemoveOutput(this.Output);
                this.Output.Dispose();
                this.Output = null;
            }

            if (this.Input != null)
            {
                this.CaptureSession.RemoveInput(this.Input);
                this.Input.Dispose();
                this.Input = null;
            }

            if (this.MainDevice != null)
            {
                this.MainDevice.Dispose();
                this.MainDevice = null;
            }
        }

        /// <summary>
        /// Opens the camera.
        /// </summary>
        public void OpenCamera()
        {
            this.logger.CalledMethod();
            if (this.CaptureSession == null)
            {
                this.CaptureSession = new AVCaptureSession();
                this.mPreviewLayer = new AVCaptureVideoPreviewLayer(this.CaptureSession)
                {
                    Frame = this.CameraView.Bounds,
                    VideoGravity = AVLayerVideoGravity.ResizeAspectFill
                };
            }

            this.Initialize();
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            this.logger.CalledMethod();
            if (this.CaptureSession == null)
            {
                return;
            }

            this.CaptureSession.BeginConfiguration();

            this.CaptureSession.SessionPreset = AVCaptureSession.PresetHigh;

            var cameraPosition = (this.Lens == CameraLens.Front) ? AVCaptureDevicePosition.Front : AVCaptureDevicePosition.Back;
            this.MainDevice = AVCaptureDevice.GetDefaultDevice(AVCaptureDeviceType.BuiltInDuoCamera, AVMediaType.Video, cameraPosition)
                ?? AVCaptureDevice.GetDefaultDevice(AVCaptureDeviceType.BuiltInTrueDepthCamera, AVMediaType.Video, cameraPosition)
                ?? AVCaptureDevice.GetDefaultDevice(AVCaptureDeviceType.BuiltInWideAngleCamera, AVMediaType.Video, cameraPosition);

            if (this.MainDevice == null)
            {
                return;
            }

            this.MainDevice.LockForConfiguration(out NSError device_error);
            if (device_error != null)
            {
                this.logger.Error($"{device_error.LocalizedDescription}");
                this.MainDevice.UnlockForConfiguration();
                return;
            }

            // フレームレート設定
            this.MainDevice.ActiveVideoMinFrameDuration = new CMTime(1, 24);
            this.MainDevice.ActiveVideoMaxFrameDuration = new CMTime(1, 24);

            this.MainDevice.UnlockForConfiguration();

            // max zoom
            this.maxZoom = (float)Math.Min(this.MainDevice.ActiveFormat.VideoMaxZoomFactor, 6);

            // 入力設定
            if (this.Input != null)
            {
                this.CaptureSession.RemoveInput(this.Input);
            }

            this.Input = new AVCaptureDeviceInput(this.MainDevice, out NSError error);
            this.CaptureSession.AddInput(this.Input);

            // 出力設定
            // フレーム処理用
            if (this.Output != null)
            {
                this.CaptureSession.RemoveOutput(this.Output);
                this.Output = null;
            }

            if (this.StillOutput != null)
            {
                this.CaptureSession.RemoveOutput(this.StillOutput);
                this.StillOutput = null;
            }

            if (this.ImageMode == ImageMode.EachFrame)
            {
                this.Output = new AVCaptureVideoDataOutput();
                this.Queue = new DispatchQueue("myQueue");
                this.Output.AlwaysDiscardsLateVideoFrames = true;

                AVCaptureVideoDataOutputSampleBufferDelegate recoder;
                recoder = new OutputRecorder(this);

                this.Output.SetSampleBufferDelegateQueue(recoder, this.Queue);
                var vSettings = new AVVideoSettingsUncompressed
                {
                      PixelFormatType = CVPixelFormatType.CV32BGRA
                };
                this.Output.WeakVideoSettings = vSettings.Dictionary;

                this.CaptureSession.AddOutput(this.Output);
            }
            else
            {
                this.StillOutput = new AVCaptureStillImageOutput();
                this.CaptureSession.AddOutput(this.StillOutput);
            }

            this.CaptureSession.CommitConfiguration();

            this.CaptureSession.StartRunning();
        }

        /// <summary>
        /// Gestures the pinch.
        /// </summary>
        /// <param name="scaleState">State of the scale.</param>
        /// <param name="lastScale">The last scale.</param>
        /// <returns>Scale</returns>
        public nfloat GesturePinch(nfloat scaleState, nfloat lastScale)
        {
            this.MainDevice.LockForConfiguration(out NSError device_error);
            if (device_error != null)
            {
                this.logger.Error($"{device_error.LocalizedDescription}");
                this.MainDevice.UnlockForConfiguration();
                return 1.0f;
            }

            var scale = scaleState + (1 - lastScale);
            var zoom = this.MainDevice.VideoZoomFactor * scale;
            if (zoom > this.maxZoom)
            {
                zoom = this.maxZoom;
            }

            if (zoom < this.minZoom)
            {
                zoom = this.minZoom;
            }

            this.MainDevice.VideoZoomFactor = zoom;
            this.MainDevice.UnlockForConfiguration();
            return scale;
        }

        /// <summary>
        /// Updates the orientation.
        /// </summary>
        public void UpdateOrientation()
        {
            var connection = this.mPreviewLayer.Connection;
            var device = UIDevice.CurrentDevice;

            if (connection != null && device != null)
            {
                var orientation = device.Orientation;

                if (connection.SupportsVideoOrientation)
                {
                    this.logger.Debug("device orientation: " + orientation);
                    switch (orientation)
                    {
                        case UIDeviceOrientation.Portrait:
                            connection.VideoOrientation = AVCaptureVideoOrientation.Portrait;
                            break;
                        case UIDeviceOrientation.LandscapeRight:
                            connection.VideoOrientation = AVCaptureVideoOrientation.LandscapeLeft;
                            break;
                        case UIDeviceOrientation.LandscapeLeft:
                            connection.VideoOrientation = AVCaptureVideoOrientation.LandscapeRight;
                            break;
                        case UIDeviceOrientation.PortraitUpsideDown:
                            connection.VideoOrientation = AVCaptureVideoOrientation.PortraitUpsideDown;
                            break;
                        default:
                            connection.VideoOrientation = AVCaptureVideoOrientation.Portrait;
                            break;
                    }

                    this.logger.CalledMethod($"Orientation: {connection.VideoOrientation.ToString()}");
                    this.mPreviewLayer.Frame = this.CameraView.Bounds;
                }
            }
        }

        /// <summary>
        /// Gets the photo orientation.
        /// </summary>
        /// <returns>The photo orientation.</returns>
        public UIImageOrientation GetPhotoOrientation()
        {
            bool isBack = this.Input.Device.Position == AVCaptureDevicePosition.Back;
            UIImageOrientation orientation;

            var devOrientation = UIDevice.CurrentDevice.Orientation;

            switch (devOrientation)
            {
                case UIDeviceOrientation.Portrait:
                    if (isBack)
                    {
                        orientation = UIImageOrientation.Right;
                    }
                    else
                    {
                        orientation = UIImageOrientation.Right;
                    }

                    break;
                case UIDeviceOrientation.LandscapeRight:
                    if (isBack)
                    {
                        orientation = UIImageOrientation.Down;
                    }
                    else
                    {
                        orientation = UIImageOrientation.Up;
                    }

                    break;
                case UIDeviceOrientation.LandscapeLeft:
                    if (isBack)
                    {
                        orientation = UIImageOrientation.Up;
                    }
                    else
                    {
                        orientation = UIImageOrientation.Down;
                    }

                    break;
                case UIDeviceOrientation.PortraitUpsideDown:
                    if (isBack)
                    {
                        orientation = UIImageOrientation.Left;
                    }
                    else
                    {
                        orientation = UIImageOrientation.Left;
                    }

                    break;
                default:
                    orientation = UIImageOrientation.Right;
                    break;
            }

            return orientation;
        }
    }
}
