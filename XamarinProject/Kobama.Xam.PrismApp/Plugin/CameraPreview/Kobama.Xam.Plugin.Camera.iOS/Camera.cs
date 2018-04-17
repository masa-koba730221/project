// -----------------------------------------------------------------------
//  <copyright file="Camera.cs" company="mkoba">
//      Copyright (c) mkoba. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AVFoundation;
using CoreFoundation;
using CoreMedia;
using CoreVideo;
using Foundation;
using Kobama.Xam.Plugin.Camera.Options;
using Kobama.Xam.Plugin.Log;
using UIKit;

namespace Kobama.Xam.Plugin.Camera.iOS
{
    public class Camera : ICameraControl
    {
        private static readonly Logger Log = new Logger(nameof(Camera));

        public AVCaptureDevice MainDevice { get; private set; }
        public AVCaptureSession CaptureSession { get; private set; }
        public AVCaptureDeviceInput Input { get; set; }
        public AVCaptureVideoDataOutput Output { get; private set; }
        public OutputRecorder Recorder { get; set; }
        public DispatchQueue Queue { get; set; }

        public ImageAvailableMode ImageMode { get; set; } = ImageAvailableMode.EachFrame;

        public event SavedImage CallbackSavedImage;
        public event Opened CallabckOpened;

        private CameraLens mLens = CameraLens.Rear;
        private float MaxZoom;
        private float MinZoom = 1.0f;
        private AVCaptureVideoPreviewLayer mPreviewLayer;

        private static Camera mInstance = new Camera();

        private Camera(){
            
        }

        public void ChangeLens(CameraLens lens)
        {
            if (this.mLens != lens){
                this.CloseCamera();
                this.mLens = lens;
                this.OpenCamera();
            }
        }

        public static Camera Instance {
            get
            {
                return mInstance;
            }
        }

        public UIView CameraView { set; get; }

        public AVCaptureVideoPreviewLayer PreviewLayer 
        {
            get { return this.mPreviewLayer; }
        }

        public List<CameraFpsRange> GetFpsRangeList()
        {
            var list = new List<CameraFpsRange>();

            if(MainDevice != null){
                var rangeList = MainDevice.ActiveFormat.VideoSupportedFrameRateRanges;

                foreach (var range in rangeList)
                {
                    list.Add(new CameraFpsRange((int)range.MinFrameRate, (int)range.MaxFrameRate));
                }
            }

            return list;
        }

        public List<Size> GetSizeList()
        {
            var list = new List<Size>();
            return list;
        }

        public void OnDestroy()
        {
            this.Dispose();
        }

        public void OnPause()
        {
            this.CloseCamera();
        }

        public void OnResume()
        {
            this.Initialize();
        }

        public void Dispose()
        {
            this.CloseCamera();
            this.CaptureSession.Dispose();
            this.CaptureSession = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Kobama.Xam.Plugin.Camera.Droid.Camera2"/> class.
        /// </summary>
        /// <param name="image">Image.</param>
        public void NotifySavedIamage(byte[] image, System.Drawing.Size size)
        {
            this.CallbackSavedImage.Invoke(image, size);
        }

        public void SetOptionAEMode(CameraFpsRange range)
        {
        }

        public void TakePicture()
        {
        }

        public void CloseCamera()
        {
            if(CaptureSession == null )
            {
                return;
            }

            CaptureSession.StopRunning();
            Recorder.Dispose();
            Queue.Dispose();
            if(Output!=null){
                CaptureSession.RemoveOutput(Output);
                Output.Dispose();
                this.Output = null;
            }

            if(Input!=null)
            {
                CaptureSession.RemoveInput(Input);
                Input.Dispose();
                this.Input = null;
            }
            MainDevice.Dispose();
        }

        public void OpenCamera()
        {
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

        public void Initialize()
        {
            if (this.CaptureSession == null)
            {
                return;
            }

            this.CaptureSession.BeginConfiguration();

            this.CaptureSession.SessionPreset = AVCaptureSession.PresetHigh;

            var cameraPosition = (this.mLens == CameraLens.Front) ? AVCaptureDevicePosition.Front : AVCaptureDevicePosition.Back;
            MainDevice = AVCaptureDevice.GetDefaultDevice(AVCaptureDeviceType.BuiltInDuoCamera, AVMediaType.Video, cameraPosition)
                ?? AVCaptureDevice.GetDefaultDevice(AVCaptureDeviceType.BuiltInWideAngleCamera, AVMediaType.Video, cameraPosition)
                ?? AVCaptureDevice.GetDefaultDevice(AVCaptureDeviceType.BuiltInWideAngleCamera, AVMediaType.Video, cameraPosition);

            if (MainDevice == null)
            {
                return;
            }

            NSError device_error;
            MainDevice.LockForConfiguration(out device_error);
            if (device_error != null)
            {
                Console.WriteLine($"Error: {device_error.LocalizedDescription}");
                MainDevice.UnlockForConfiguration();
                return;
            }

            //フレームレート設定
            MainDevice.ActiveVideoMinFrameDuration = new CMTime(1, 24);
            MainDevice.ActiveVideoMaxFrameDuration = new CMTime(1, 24);

            MainDevice.UnlockForConfiguration();

            //max zoom
            MaxZoom = (float)Math.Min(MainDevice.ActiveFormat.VideoMaxZoomFactor, 6);

            //入力設定
            if(Input!=null){
                this.CaptureSession.RemoveInput(this.Input);
            }
            NSError error;
            Input = new AVCaptureDeviceInput(MainDevice, out error);
            CaptureSession.AddInput(Input);

            //出力設定
            //フレーム処理用
            if(this.Output!=null)
            {
                this.CaptureSession.RemoveOutput(this.Output);
            }
            Output = new AVCaptureVideoDataOutput();
            Queue = new DispatchQueue("myQueue");
            Output.AlwaysDiscardsLateVideoFrames = true;
            Recorder = new OutputRecorder(this);
            Output.SetSampleBufferDelegateQueue(Recorder, Queue);
            var vSettings = new AVVideoSettingsUncompressed();
            vSettings.PixelFormatType = CVPixelFormatType.CV32BGRA;
            Output.WeakVideoSettings = vSettings.Dictionary;

            CaptureSession.AddOutput(Output);

            this.CaptureSession.CommitConfiguration();

            CaptureSession.StartRunning();
        }


        public nfloat GesturePinch(nfloat scaleState, nfloat lastScale){
            NSError device_error;
            MainDevice.LockForConfiguration(out device_error);
            if (device_error != null)
            {
                Console.WriteLine($"Error: {device_error.LocalizedDescription}");
                MainDevice.UnlockForConfiguration();
                return 1.0f;
            }
            var scale = scaleState + (1 - lastScale);
            var zoom = MainDevice.VideoZoomFactor * scale;
            if (zoom > MaxZoom) zoom = MaxZoom;
            if (zoom < MinZoom) zoom = MinZoom;
            MainDevice.VideoZoomFactor = zoom;
            MainDevice.UnlockForConfiguration();
            return scale;
        }
    }
}
