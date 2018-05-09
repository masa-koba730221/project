// -----------------------------------------------------------------------
// <copyright file="Camera2.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kobama.Xam.Plugin.Camera.Droid
{
    using System;
    using System.Collections.Generic;
    using Android.App;
    using Android.Content;
    using Android.Graphics;
    using Android.Hardware.Camera2;
    using Android.Hardware.Camera2.Params;
    using Android.Media;
    using Android.OS;
    using Android.Util;
    using Android.Views;
    using Java.IO;
    using Java.Lang;
    using Java.Util;
    using Java.Util.Concurrent;
    using Kobama.Xam.Plugin.Camera.Droid.Listener;
    using Kobama.Xam.Plugin.Camera.Options;
    using Kobama.Xam.Plugin.CameraPreview.Droid;
    using Kobama.Xam.Plugin.Log;
    using Math = Java.Lang.Math;
    using Orientation = Android.Content.Res.Orientation;

    /// <summary>
    /// Camera2 preview.
    /// </summary>
    public class Camera2 : ICameraControl
    {
        /// <summary>
        /// The request camera permission.
        /// </summary>
        public static readonly int REQUEST_CAMERA_PERMISSION = 1;

        /// <summary>
        /// The logger.
        /// </summary>
        private static readonly Logger Log = new Logger(nameof(Camera2));

        /// <summary>
        /// The orientations.
        /// </summary>
        private static readonly SparseIntArray ORIENTATIONS = new SparseIntArray();

        /// <summary>
        /// Max preview width that is guaranteed by Camera2 API
        /// </summary>
        private static readonly int MAX_PREVIEW_WIDTH = 1920;

        /// <summary>
        /// Max preview height that is guaranteed by Camera2 API
        /// </summary>
        private static readonly int MAX_PREVIEW_HEIGHT = 1080;

        /// <summary>
        /// ID of the current
        /// </summary>
        private string mCameraId;

        /// <summary>
        /// Gets or sets CameraCaptureSession for camera preview.
        /// </summary>
        /// <value>The m capture session.</value>
        public CameraCaptureSession mCaptureSession { get; set; }

        /// <summary>
        /// Gets or sets reference to the opened CameraDevice
        /// </summary>
        /// <value>The m camera device.</value>
        public CameraDevice mCameraDevice { get; set; }

        /// <summary>
        /// The size of the camera preview
        /// </summary>
        private Android.Util.Size mPreviewSize;

        /// <summary>
        /// CameraDevice.StateListener is called when a CameraDevice changes its state
        /// </summary>
        private Camera2StateListener mStateCallback;

        /// <summary>
        /// An additional thread for running tasks that shouldn't block the UI.
        /// </summary>
        private HandlerThread mBackgroundThread;

        /// <summary>
        /// ImageReader that handles still image capture.
        /// </summary>
        private ImageReader mImageReader;

        /// <summary>
        /// Gets or sets Handler for running tasks in the background.
        /// </summary>
        public Handler mBackgroundHandler { get; set; }

        /// <summary>
        /// Gets or sets CaptureRequest.Builder for the camera preview
        /// </summary>
        public CaptureRequest.Builder mPreviewRequestBuilder { get; set; }

        public CaptureRequest mPreviewRequest { get; set; }

        /// <summary>
        /// The state of the m.
        /// </summary>
        private CameraState mState = CameraState.STATE_PREVIEW;

        /// <summary>
        /// Gets or sets Semaphore to prevent the app from exiting before closing the camera.
        /// </summary>
        public CameraState State
        {
            get
            {
                return this.mState;
            }

            set
            {
                Log.Debug($"Set State: {this.mState} to {value}");
                this.mState = value;
            }
        }

        /// <summary>
        /// Gets or sets A Semaphore to prevent the app from exiting before closing the camera.
        /// </summary>
        public Semaphore mCameraOpenCloseLock { get; set; } = new Semaphore(1);

        /// <summary>
        /// Gets or sets CameraCaptureSession.CaptureCallback that handles events related to JPEG capture.
        /// </summary>
        public Camera2CaptureListener mCaptureCallback { get; set; }

        /// <summary>
        /// Gets or sets the m lens.
        /// </summary>
        /// <value>The m lens.</value>
        private LensFacing mLens = LensFacing.Back;

        /// <summary>
        /// The m camera characteristics.
        /// </summary>
        private CameraCharacteristics mCameraCharacteristics;

        // Timeout for the pre-capture sequence.
        private const long PRECAPTURE_TIMEOUT_MS = 1000;

        /// <summary>
        /// Timer to use with pre-capture sequence to ensure a timely capture if 3A convergence is taking
        /// too long.
        /// </summary>
        private long mCaptureTimer;

        /// <summary>
        /// Prevents a default instance of the <see cref="Camera2"/> class from being created.
        /// </summary>
        private Camera2()
        {
            Log.CalledMethod();

            // fill ORIENTATIONS list
            ORIENTATIONS.Append((int)SurfaceOrientation.Rotation0, 90);
            ORIENTATIONS.Append((int)SurfaceOrientation.Rotation90, 0);
            ORIENTATIONS.Append((int)SurfaceOrientation.Rotation180, 270);
            ORIENTATIONS.Append((int)SurfaceOrientation.Rotation270, 180);

            this.mStateCallback = new Camera2StateListener(this);
            this.mCaptureCallback = new Camera2CaptureListener(this);
        }

        /// <summary>
        /// The m context.
        /// </summary>
        private Context mContext;

        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        /// <value>The context.</value>
        public Context Context
        {
            get
            {
                return this.mContext;
            }

            set
            {
                this.mContext = value;
            }
        }

        /// <summary>
        /// The m texture view.
        /// </summary>
        private TextureView mTextureView;

        /// <summary>
        /// Gets or sets the texture view.
        /// </summary>
        /// <value>The texture view.</value>
        public TextureView TextureView
        {
            get { return this.mTextureView; }
            set { this.mTextureView = value; }
        }

        /// <summary>
        /// The instance.
        /// </summary>
        private static Camera2 instance = new Camera2();

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static Camera2 Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// Gets or sets the image mode.
        /// </summary>
        /// <value>
        /// The image mode.
        /// </value>
        public ImageAvailableMode ImageMode { get; set; } = ImageAvailableMode.Auto;

        /// <summary>
        /// Chooses the size of the optimal.
        /// </summary>
        /// <returns>The optimal size.</returns>
        /// <param name="choices">Choices.</param>
        /// <param name="textureViewWidth">Texture view width.</param>
        /// <param name="textureViewHeight">Texture view height.</param>
        /// <param name="maxWidth">Max width.</param>
        /// <param name="maxHeight">Max height.</param>
        /// <param name="aspectRatio">Aspect ratio.</param>
        private static Android.Util.Size ChooseOptimalSize(
            Android.Util.Size[] choices,
            int textureViewWidth,
            int textureViewHeight,
            int maxWidth,
            int maxHeight,
            Android.Util.Size aspectRatio)
        {
            Log.CalledMethod();

            // Collect the supported resolutions that are at least as big as the preview Surface
            var bigEnough = new List<Android.Util.Size>();

            // Collect the supported resolutions that are smaller than the preview Surface
            var notBigEnough = new List<Android.Util.Size>();
            int w = aspectRatio.Width;
            int h = aspectRatio.Height;

            for (var i = 0; i < choices.Length; i++)
            {
                Android.Util.Size option = choices[i];
                if ((option.Width <= maxWidth) && (option.Height <= maxHeight) &&
                       option.Height == option.Width * h / w)
                {
                    if (option.Width >= textureViewWidth &&
                        option.Height >= textureViewHeight)
                    {
                        bigEnough.Add(option);
                    }
                    else
                    {
                        notBigEnough.Add(option);
                    }
                }
            }

            // Pick the smallest of those big enough. If there is no one big enough, pick the
            // largest of those not big enough.
            if (bigEnough.Count > 0)
            {
                return (Android.Util.Size)Collections.Min(bigEnough, new CompareSizesByArea());
            }
            else if (notBigEnough.Count > 0)
            {
                return (Android.Util.Size)Collections.Max(notBigEnough, new CompareSizesByArea());
            }
            else
            {
                Log.Error("Couldn't find any suitable preview size");
                return choices[0];
            }
        }

        /// <summary>
        /// Ons the resume.
        /// </summary>
        public void OnResume()
        {
            Log.CalledMethod();

            this.StartBackgroundThread();

            if (this.mTextureView.IsAvailable)
            {
                this.OpenCamera(this.mTextureView.Width, this.mTextureView.Height);
            }
            else
            {
                Log.CalledMethod("TextureView is invalid");
            }
        }

        /// <summary>
        /// Ons the pause.
        /// </summary>
        public void OnPause()
        {
            Log.CalledMethod();
            this.CloseCamera();
            this.StopBackgroundThread();
        }

        /// <summary>
        /// Ons the destroy.
        /// </summary>
        public void OnDestroy()
        {
            this.OnPause();
            this.CallbackSavedImage = null;
        }

        /// <summary>
        /// Changes the lens.
        /// </summary>
        /// <param name="lens">Lens.</param>
        public void ChangeLens(Options.CameraLens lens)
        {
            LensFacing requestLens;

            switch (lens)
            {
                case Options.CameraLens.Rear:
                    requestLens = LensFacing.Back;
                    break;
                case Options.CameraLens.Front:
                    requestLens = LensFacing.Front;
                    break;
                default:
                    Log.Error($"Error Argument:{lens}");
                    return;
            }

            if (this.mLens != requestLens)
            {
                this.CloseCamera();
                this.mLens = requestLens;
                this.OpenCamera(this.mTextureView.Width, this.mTextureView.Height);
            }
        }

        /// <summary>
        /// Gets the camera identifier.
        /// </summary>
        /// <returns>The camera identifier.</returns>
        /// <param name="lens">Lens.</param>
        private string GetCameraId(LensFacing lens)
        {
            var activity = (Activity)this.mContext;
            var manager = (CameraManager)activity.GetSystemService(Context.CameraService);
            try
            {
                for (var i = 0; i < manager.GetCameraIdList().Length; i++)
                {
                    var id = manager.GetCameraIdList()[i];
                    CameraCharacteristics characteristics = manager.GetCameraCharacteristics(id);

                    // We don't use a front facing camera in this sample.
                    var facing = (Integer)characteristics.Get(CameraCharacteristics.LensFacing);
                    if (facing == null || (facing != null && facing != Integer.ValueOf((int)lens)))
                    {
                        continue;
                    }

                    this.mCameraCharacteristics = characteristics;
                    return id;
                }
            }
            catch (CameraAccessException e)
            {
                Log.Error(e.ToString());
            }
            catch (NullPointerException e)
            {
                Log.Error(e.ToString());
            }

            this.mCameraCharacteristics = null;
            return null;
        }

        /// <summary>
        /// Ises the supported flash.
        /// </summary>
        /// <returns><c>true</c>, if supported flash was ised, <c>false</c> otherwise.</returns>
        private bool IsSupportedFlash()
        {
            if (this.mCameraCharacteristics == null)
            {
                return false;
            }

            // Check if the flash is supported.
            bool? available = (bool?)this.mCameraCharacteristics.Get(CameraCharacteristics.FlashInfoAvailable);
            if (available == null)
            {
                return false;
            }
            else if (available == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the image reader.
        /// </summary>
        /// <returns>The image reader.</returns>
        /// <param name="handler">Handler.</param>
        /// <param name="listener">Listener.</param>
        private ImageReader GetImageReader(Handler handler, ImageAvailableListener listener)
        {
            if (this.mCameraCharacteristics == null)
            {
                return null;
            }

            var map = (StreamConfigurationMap)this.mCameraCharacteristics.Get(CameraCharacteristics.ScalerStreamConfigurationMap);
            if (map == null)
            {
                return null;
            }

            var formats = map.GetOutputFormats();
            foreach (var format in formats)
            {
                Log.Debug($"Image output format:{format}");
            }

            // For still image captures, we use the largest available size.
            Android.Util.Size largest;
            var sizeList = map.GetOutputSizes((int)ImageFormatType.Jpeg);
            largest = (Android.Util.Size)Collections.Max(Arrays.AsList(sizeList), new CompareSizesByArea());

            var reader = ImageReader.NewInstance(largest.Width, largest.Height, ImageFormatType.Jpeg, /*maxImages*/1);
            reader.SetOnImageAvailableListener(listener, handler);

            Log.Debug($"ImageReader Size:{largest.Width} ,{largest.Height}");
            return reader;
        }

        private ImageReader GetImageReaderForEachFrame(Handler handler, ImageAvailableListener listener)
        {
            if (this.mCameraCharacteristics == null)
            {
                return null;
            }

            var map = (StreamConfigurationMap)this.mCameraCharacteristics.Get(CameraCharacteristics.ScalerStreamConfigurationMap);
            if (map == null)
            {
                return null;
            }

            var formats = map.GetOutputFormats();

            // For still image captures, we use the largest available size.
            Android.Util.Size largest;
            largest = new Android.Util.Size(640, 480);

            var reader = ImageReader.NewInstance(largest.Width, largest.Height, ImageFormatType.Yuv420888, /*maxImages*/2);
            reader.SetOnImageAvailableListener(listener, handler);

            Log.Debug($"ImageReader Size:{largest.Width} ,{largest.Height}");
            return reader;
        }

        /// <summary>
        /// Gets the window orientation.
        /// </summary>
        /// <returns>The window orientation.</returns>
        private SurfaceOrientation GetWindowOrientation()
        {
            return ((Activity)this.mContext).WindowManager.DefaultDisplay.Rotation;
        }

        /// <inheritdoc/>
        public List<System.Drawing.Size> GetSizeList()
        {
            if (this.mCameraCharacteristics == null)
            {
                return null;
            }

            var map = (StreamConfigurationMap)this.mCameraCharacteristics.Get(CameraCharacteristics.ScalerStreamConfigurationMap);
            if (map == null)
            {
                return null;
            }

            var list = map.GetOutputSizes((int)ImageFormatType.Jpeg);
            var systemList = new List<System.Drawing.Size>();
            foreach (var s in list)
            {
                systemList.Add(new System.Drawing.Size(s.Width, s.Height));
            }

            return systemList;
        }

        /// <summary>
        /// Gets the fps range list.
        /// </summary>
        /// <returns>
        /// The fps range list.
        /// </returns>
        public List<CameraFpsRange> GetFpsRangeList()
        {
            if (this.mCameraCharacteristics == null)
            {
                return null;
            }

            var ranges = this.mCameraCharacteristics.Get(CameraCharacteristics.ControlAeAvailableTargetFpsRanges).ToArray<Range>();
            if (ranges == null)
            {
                return null;
            }

            var list = new List<CameraFpsRange>();
            foreach (var range in ranges)
            {
                var lower = (int)range.Lower;
                var upper = (int)range.Upper;
                list.Add(new CameraFpsRange(lower, upper));
            }

            return list;
        }

        /// <summary>
        /// Gets the AFA vailable mode list.
        /// </summary>
        /// <returns>The AFA vailable mode list.</returns>
        public int[] GetAFAvailableModeList()
        {
            if (this.mCameraCharacteristics == null)
            {
                return null;
            }

            int[] list = null;
            var value = this.mCameraCharacteristics.Get(CameraCharacteristics.ControlAfAvailableModes);
            if (value != null)
            {
                list = value.ToArray<int>();
            }

            return list;
        }

        /// <summary>
        /// Gets the AWBA vailable mode list.
        /// </summary>
        /// <returns>The AWBA vailable mode list.</returns>
        public int[] GetAWBAvailableModeList()
        {
            if (this.mCameraCharacteristics == null)
            {
                return null;
            }

            return this.mCameraCharacteristics.Get(CameraCharacteristics.ControlAwbAvailableModes).ToArray<int>();
        }

        /// <summary>
        /// Gets the lens minimum focus distande.
        /// </summary>
        /// <returns>The lens minimum focus distande.</returns>
        public float? GetLensMinFocusDistande()
        {
            if (this.mCameraCharacteristics == null)
            {
                return null;
            }

            var minFocusDist = (float)this.mCameraCharacteristics.Get(CameraCharacteristics.LensInfoMinimumFocusDistance);

            return minFocusDist;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Kobama.Xam.Plugin.Camera.Droid.Camera2"/> class.
        /// </summary>
        /// <param name="image">Image.</param>
        /// <param name="size">Size</param>
        public void NotifySavedIamage(byte[] image, System.Drawing.Size size)
        {
            if (this.CallbackSavedImage != null)
            {
                this.CallbackSavedImage.Invoke(image, size);
            }
        }

        /// <summary>
        /// Gets the sensor orientation.
        /// </summary>
        /// <returns>The sensor orientation.</returns>
        private int GetSensorOrientation()
        {
            if (this.mCameraCharacteristics == null)
            {
                return 0;
            }

            return (int)this.mCameraCharacteristics.Get(CameraCharacteristics.SensorOrientation);
        }

        /// <summary>
        /// Sets up member variables related to camera.
        /// </summary>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        private void SetUpCameraOutputs(int width, int height)
        {
            Log.CalledMethod($"width:{width} height:{height}");

            var activity = (Activity)this.mContext;
            var manager = (CameraManager)activity.GetSystemService(Context.CameraService);
            try
            {
                var map = (StreamConfigurationMap)this.mCameraCharacteristics.Get(CameraCharacteristics.ScalerStreamConfigurationMap);
                if (map == null)
                {
                    return;
                }

				Android.Util.Size largest = (Android.Util.Size)Collections.Max(Arrays.AsList(map.GetOutputSizes(Class.FromType(typeof(SurfaceTexture)))), new CompareSizesByArea());

                // Find out if we need to swap dimension to get the preview size relative to sensor
                // coordinate.
                var displayRotation = this.GetWindowOrientation();

                // noinspection ConstantConditions
                var sensorOrientation = this.GetSensorOrientation();
                bool swappedDimensions = false;
                switch (displayRotation)
                {
                    case SurfaceOrientation.Rotation0:
                    case SurfaceOrientation.Rotation180:
                        if (sensorOrientation == 90 || sensorOrientation == 270)
                        {
                            swappedDimensions = true;
                        }

                        break;
                    case SurfaceOrientation.Rotation90:
                    case SurfaceOrientation.Rotation270:
                        if (sensorOrientation == 0 || sensorOrientation == 180)
                        {
                            swappedDimensions = true;
                        }

                        break;
                    default:
                        // Log.Error(TAG, "Display rotation is invalid: " + displayRotation);
                        break;
                }

                Android.Graphics.Point displaySize = new Android.Graphics.Point();
                activity.WindowManager.DefaultDisplay.GetSize(displaySize);
                var rotatedPreviewWidth = width;
                var rotatedPreviewHeight = height;
                var maxPreviewWidth = displaySize.X;
                var maxPreviewHeight = displaySize.Y;

                if (swappedDimensions)
                {
                    rotatedPreviewWidth = height;
                    rotatedPreviewHeight = width;
                    maxPreviewWidth = displaySize.Y;
                    maxPreviewHeight = displaySize.X;
                }

                if (maxPreviewWidth > MAX_PREVIEW_WIDTH)
                {
                    maxPreviewWidth = MAX_PREVIEW_WIDTH;
                }

                if (maxPreviewHeight > MAX_PREVIEW_HEIGHT)
                {
                    maxPreviewHeight = MAX_PREVIEW_HEIGHT;
                }

                Log.Debug($"View: {rotatedPreviewWidth},{rotatedPreviewHeight}  MaxSize:{maxPreviewWidth},{maxPreviewHeight} Aspect:{largest.Width},{largest.Height}");
                this.mPreviewSize = ChooseOptimalSize(map.GetOutputSizes(Class.FromType(typeof(SurfaceTexture))), rotatedPreviewWidth, rotatedPreviewHeight, maxPreviewWidth, maxPreviewHeight, largest);
                Log.Debug($"PreviewSize: {this.mPreviewSize.Width},{this.mPreviewSize.Height}");
                return;
            }
            catch (CameraAccessException e)
            {
                Log.Error(e.ToString());
            }
            catch (NullPointerException e)
            {
                Log.Error(e.ToString());
            }
        }

        /// <summary>
        /// Opens the camera specified by {@link Camera2BasicFragment#mCameraId}.
        /// </summary>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public void OpenCamera(int width, int height)
        {
            Log.CalledMethod($"width:{width} heiht:{height}");

            // if (ContextCompat.CheckSelfPermission(Activity, Manifest.Permission.Camera) != Permission.Granted)
            // {
            //    RequestCameraPermission();
            //    return;
            // }
            this.mCameraId = this.GetCameraId(this.mLens);
            this.SetUpCameraOutputs(width, height);
            this.ConfigureTransform(width, height);
            var activity = (Activity)this.mContext;
            var manager = (CameraManager)activity.GetSystemService(Context.CameraService);
            try
            {
                if (!this.mCameraOpenCloseLock.TryAcquire(2500, TimeUnit.Milliseconds))
                {
                    throw new RuntimeException("Time out waiting to lock camera opening.");
                }

                manager.OpenCamera(this.mCameraId, this.mStateCallback, this.mBackgroundHandler);

                this.CallabckOpened.Invoke();
            }
            catch (CameraAccessException e)
            {
                e.PrintStackTrace();
            }
            catch (InterruptedException e)
            {
                throw new RuntimeException("Interrupted while trying to lock camera opening.", e);
            }
        }

        /// <summary>
        /// Closes the current {@link CameraDevice}.
        /// </summary>
        public void CloseCamera()
        {
            Log.CalledMethod();
            try
            {
                this.mCameraOpenCloseLock.Acquire();
                if (this.mCaptureSession != null)
                {
                    this.mCaptureSession.Close();
                    this.mCaptureSession = null;
                }

                if (this.mCameraDevice != null)
                {
                    this.mCameraDevice.Close();
                    this.mCameraDevice = null;
                }

                if (this.mImageReader != null)
                {
                    this.mImageReader.Close();
                    this.mImageReader = null;
                }
            }
            catch (InterruptedException e)
            {
                throw new RuntimeException("Interrupted while trying to lock camera closing.", e);
            }
            finally
            {
                this.mCameraOpenCloseLock.Release();
            }
        }

        /// <summary>
        /// Starts a background thread and its {@link Handler}.
        /// </summary>
        public void StartBackgroundThread()
        {
            if (this.mBackgroundThread == null)
            {
                Log.CalledMethod();
                this.mBackgroundThread = new HandlerThread("CameraBackground");
                this.mBackgroundThread.Start();
                this.mBackgroundHandler = new Handler(this.mBackgroundThread.Looper);
            }
        }

        /// <summary>
        /// Stops the background thread and its {@link Handler}.
        /// </summary>
        public void StopBackgroundThread()
        {
            if (this.mBackgroundThread != null)
            {
                Log.CalledMethod();
                this.mBackgroundThread.QuitSafely();
                try
                {
                    this.mBackgroundThread.Join();
                    this.mBackgroundThread = null;
                    this.mBackgroundHandler = null;
                }
                catch (InterruptedException e)
                {
                    e.PrintStackTrace();
                }
            }
        }

        /// <summary>
        /// Creates a new {@link CameraCaptureSession} for camera preview.
        /// </summary>
        public void CreateCameraPreviewSession()
        {
            Log.CalledMethod();
            try
            {
                SurfaceTexture texture = this.mTextureView.SurfaceTexture;
                if (texture == null)
                {
                    throw new IllegalStateException("texture is null");
                }

                //// We configure the size of default buffer to be the size of camera preview we want.
                Log.CalledMethod($"BufferSize width:{this.mPreviewSize.Width} height:{this.mPreviewSize.Height}");
                texture.SetDefaultBufferSize(this.mPreviewSize.Width, this.mPreviewSize.Height);

                // This is the output Surface we need to start preview.
                var surface = new Surface(texture);

                // We set up a CaptureRequest.Builder with the output Surface.
                this.mPreviewRequestBuilder = this.mCameraDevice.CreateCaptureRequest(CameraTemplate.Preview);

                this.mPreviewRequestBuilder.AddTarget(surface);

                if (this.ImageMode == ImageAvailableMode.EachFrame)
                {
                    this.mImageReader = this.GetImageReaderForEachFrame(this.mBackgroundHandler, new ImageAvailableListener(this));
                    this.mPreviewRequestBuilder.AddTarget(this.mImageReader.Surface);
                }
                else
                {
                    this.mImageReader = this.GetImageReader(this.mBackgroundHandler, new ImageAvailableListener(this));
                }

                // var activity = (Activity)this.mContext;
                // int rotation = (int)activity.WindowManager.DefaultDisplay.Rotation;
                // this.mPreviewRequestBuilder.Set(CaptureRequest.JpegOrientation, this.GetOrientation(rotation));

                // Here, we create a CameraCaptureSession for camera preview.
                var surfaces = new List<Surface>
                {
                    surface,
                    this.mImageReader.Surface
                };
                this.mCameraDevice.CreateCaptureSession(surfaces, new Camera2CaptureSessionCallback(this), null);
            }
            catch (CameraAccessException e)
            {
                e.PrintStackTrace();
            }
        }

        /// <summary>
        /// Cast the specified obj.
        /// </summary>
        /// <returns>The cast.</returns>
        /// <param name="obj">Object.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T Cast<T>(Java.Lang.Object obj) where T : class
        {
            var propertyInfo = obj.GetType().GetProperty("Instance");
            return propertyInfo == null ? null : propertyInfo.GetValue(obj, null) as T;
        }

        /// <summary>
        /// Configures the necessary {@link android.graphics.Matrix}
        /// transformation to `mTextureView`.
        /// This method should be called after the camera preview size is determined in
        /// setUpCameraOutputs and also the size of `mTextureView` is fixed.
        /// </summary>
        /// <param name="viewWidth">View width.</param>
        /// <param name="viewHeight">View height.</param>
        public void ConfigureTransform(int viewWidth, int viewHeight)
        {
            Log.CalledMethod($"viewWidth:{viewWidth} viewHeight:{viewHeight}");

            Activity activity = (Activity)this.mContext;
            if (this.mTextureView == null || this.mPreviewSize == null || activity == null)
            {
                return;
            }

            RectF bufferRect;
            var sensorOrientation = this.GetSensorOrientation();
            if (sensorOrientation == 90 || sensorOrientation == 270)
            {
				bufferRect = new RectF(0, 0, this.mPreviewSize.Height, this.mPreviewSize.Width);
            }
            else
            {
				bufferRect = new RectF(0, 0, this.mPreviewSize.Width, this.mPreviewSize.Height);
            }

            var rotation = (int)this.GetWindowOrientation();
            Matrix matrix = new Matrix();
            RectF viewRect = new RectF(0, 0, viewWidth, viewHeight);
            float centerX = viewRect.CenterX();
            float centerY = viewRect.CenterY();

            bufferRect.Offset(centerX - bufferRect.CenterX(), centerY - bufferRect.CenterY());
            matrix.SetRectToRect(viewRect, bufferRect, Matrix.ScaleToFit.Fill);
            float scale = Math.Max((float)viewHeight / this.mPreviewSize.Height, (float)viewWidth / this.mPreviewSize.Width);
            Log.CalledMethod($"Scale:{scale}");
            matrix.PostScale(scale, scale, centerX, centerY);

            if (rotation == (int)SurfaceOrientation.Rotation90 || rotation == (int)SurfaceOrientation.Rotation270)
            {
                matrix.PostRotate(90 * (rotation - 2), centerX, centerY);
            }
            else if (rotation == (int)SurfaceOrientation.Rotation180)
            {
                matrix.PostRotate(180, centerX, centerY);
            }
            else
            {
                matrix.PostRotate(0, centerX, centerY);
            }

            this.mTextureView.SetTransform(matrix);
        }

        /// <summary>
        /// Initiate a still image capture.
        /// </summary>
        public void TakePicture()
        {
            Log.CalledMethod();
            this.LockFocus();
            this.StartTimerLocked();
        }

        /// <summary>
        /// Unlock the focus. This method should be called when still image capture sequence is
        /// finished.
        /// </summary>
        public void UnlockFocus()
        {
            Log.CalledMethod();
            try
            {
                // Reset the auto-focus trigger
                this.mPreviewRequestBuilder.Set(CaptureRequest.ControlAfTrigger, (int)ControlAFTrigger.Cancel);
                this.SetAutoFlash(this.mPreviewRequestBuilder);
                this.mCaptureSession.Capture(this.mPreviewRequestBuilder.Build(), this.mCaptureCallback, this.mBackgroundHandler);
                this.mPreviewRequestBuilder.Set(CaptureRequest.ControlAfTrigger, (int)ControlAFTrigger.Idle);

                // After this, the camera will go back to the normal state of preview.
                this.State = CameraState.STATE_PREVIEW;
                this.mCaptureSession.SetRepeatingRequest(this.mPreviewRequest, this.mCaptureCallback, this.mBackgroundHandler);
            }
            catch (CameraAccessException e)
            {
                e.PrintStackTrace();
            }
        }

        /// <summary>
        /// Lock the focus as the first step for a still image capture.
        /// </summary>
        private void LockFocus()
        {
            Log.CalledMethod();
            try
            {
                // This is how to tell the camera to lock focus.
                this.mPreviewRequestBuilder.Set(CaptureRequest.ControlAfTrigger, (int)ControlAFTrigger.Start);

                // Tell #mCaptureCallback to wait for the lock.
                this.State = CameraState.STATE_WAITING_LOCK;
                this.mCaptureSession.Capture(this.mPreviewRequestBuilder.Build(), this.mCaptureCallback, this.mBackgroundHandler);
            }
            catch (CameraAccessException e)
            {
                e.PrintStackTrace();
            }
        }

        /// <summary>
        /// Run the precapture sequence for capturing a still image. This method should be called when
        /// we get a response in {@link #mCaptureCallback} from {@link #lockFocus()}.
        /// </summary>
        public void RunPrecaptureSequence()
        {
            Log.CalledMethod();
            try
            {
                // This is how to tell the camera to trigger.
                this.mPreviewRequestBuilder.Set(CaptureRequest.ControlAePrecaptureTrigger, (int)ControlAEPrecaptureTrigger.Start);

                // Tell #mCaptureCallback to wait for the precapture sequence to be set.
                this.State = CameraState.STATE_WAITING_PRECAPTURE;
                this.mCaptureSession.Capture(this.mPreviewRequestBuilder.Build(), this.mCaptureCallback, this.mBackgroundHandler);
            }
            catch (CameraAccessException e)
            {
                e.PrintStackTrace();
            }
        }

        /// <summary>
        /// The still capture builder.
        /// </summary>
        private CaptureRequest.Builder stillCaptureBuilder;

        /// <summary>
        /// Occurs when callback saved image.
        /// </summary>
        public event SavedImage CallbackSavedImage;

        /// <summary>
        /// Occurs when callabck opened.
        /// </summary>
        public event Opened CallabckOpened;

        /// <summary>
        /// Capture a still picture. This method should be called when we get a response in
        /// {@link #mCaptureCallback} from both {@link #lockFocus()}.
        /// </summary>
        public void CaptureStillPicture()
        {
            Log.CalledMethod();
            try
            {
                var activity = (Activity)this.mContext;
                if (activity == null || this.mCameraDevice == null)
                {
                    return;
                }

                // This is the CaptureRequest.Builder that we use to take a picture.
                this.stillCaptureBuilder = this.mCameraDevice.CreateCaptureRequest(CameraTemplate.StillCapture);
                this.stillCaptureBuilder.AddTarget(this.mImageReader.Surface);

                this.Setup3AControlLock(this.stillCaptureBuilder);

                // Orientation
                int rotation = (int)activity.WindowManager.DefaultDisplay.Rotation;
                this.stillCaptureBuilder.Set(CaptureRequest.JpegOrientation, this.GetOrientation(rotation));

                this.mCaptureSession.StopRepeating();
                this.mCaptureSession.Capture(this.stillCaptureBuilder.Build(), new Camera2CaptureStillPictureSessionCallback(this), null);
            }
            catch (CameraAccessException e)
            {
                e.PrintStackTrace();
            }
        }

        /// <summary>
        /// Retrieves the JPEG orientation from the specified screen rotation.
        /// </summary>
        /// <returns>The orientation.</returns>
        /// <param name="rotation">Rotation.</param>
        private int GetOrientation(int rotation)
        {
            Log.CalledMethod();

            // Sensor orientation is 90 for most devices, or 270 for some devices (eg. Nexus 5X)
            // We have to take that into account and rotate JPEG properly.
            // For devices with orientation of 90, we simply return our mapping from ORIENTATIONS.
            // For devices with orientation of 270, we need to rotate the JPEG 180 degrees.
            return (ORIENTATIONS.Get(rotation) + this.GetSensorOrientation() + 270) % 360;
        }

        /// <summary>
        /// Sets the auto flash.
        /// </summary>
        /// <param name="requestBuilder">Request builder.</param>
        public void SetAutoFlash(CaptureRequest.Builder requestBuilder)
        {
            Log.CalledMethod();
            if (this.IsSupportedFlash())
            {
                requestBuilder.Set(CaptureRequest.ControlAeMode, (int)ControlAEMode.OnAutoFlash);
            }
        }

        /// <summary>
        /// Sets the option AE mode.
        /// </summary>
        /// <param name="range">The range.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void SetOptionAEMode(CameraFpsRange range)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Setup3s the AC ontrol lock.
        /// </summary>
        public void Setup3AControlLock(CaptureRequest.Builder builder)
        {
            Log.CalledMethod();

            builder.Set(CaptureRequest.ControlMode, (int)ControlMode.Auto);

            if (this.IsAFRun())
            {
                Log.CalledMethod("In AF Run");

                // If there is a "continuous picture" mode available, use it, otherwise default to AUTO.
                if (this.Contains(this.GetAFAvailableModeList(), (int)ControlAFMode.ContinuousPicture))
                {
                    Log.CalledMethod("Set ContinuousPicture to AF Mode");
                    builder.Set(CaptureRequest.ControlAfMode, (int)ControlAFMode.ContinuousPicture);
                }
                else
                {
                    Log.CalledMethod("Set Auto to AF Mode");
                    builder.Set(CaptureRequest.ControlAfMode, (int)ControlAFMode.Auto);
                }
            }

            if (this.Contains(this.GetAWBAvailableModeList(), (int)ControlAwbMode.Auto))
            {
                Log.CalledMethod("Set Auto to AWB Mode");
                // Allow AWB to run auto-magically if this device supports this
                builder.Set(CaptureRequest.ControlAwbMode, (int)ControlAwbMode.Auto);
            }

            // var list = this.owner.GetFpsRangeList();
            builder.Set(CaptureRequest.ControlAeTargetFpsRange, new Range(15, 15));

            // Flash is automatically enabled when necessary.
            this.SetAutoFlash(builder);
        }

        /// <summary>
        /// Return true if the given array contains the given integer.
        /// </summary>
        /// <returns><c>true</c>, if the array contains the given integer, <c>false</c> otherwise.</returns>
        /// <param name="modes">array to check.</param>
        /// <param name="mode">integer to get for.</param>
        public bool Contains(int[] modes, int mode)
        {
            if (modes == null)
            {
                return false;
            }

            foreach (int i in modes)
            {
                if (i == mode)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Ises the legacy locked.
        /// </summary>
        /// <returns><c>true</c>, if legacy locked was ised, <c>false</c> otherwise.</returns>
        public bool IsLegacyLocked()
        {
            return (int)this.mCameraCharacteristics.Get(CameraCharacteristics.InfoSupportedHardwareLevel) == (int)InfoSupportedHardwareLevel.Legacy;
        }

        /// <summary>
        /// Ises the AF Run.
        /// </summary>
        /// <returns><c>true</c>, if AF was run, <c>false</c> otherwise.</returns>
        public bool IsAFRun()
        {
            var value = this.GetLensMinFocusDistande();

            if (value == null || value == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Start the timer for the pre-capture sequence.
        ///
        /// Call this only with {@link #mCameraStateLock} held.
        /// </summary>
        public void StartTimerLocked()
        {
            mCaptureTimer = SystemClock.ElapsedRealtime();
        }

        /// <summary>
        /// Check if the timer for the pre-capture sequence has been hit.
        ///
        /// Call this only with {@link #mCameraStateLock} held.
        /// </summary>
        /// <returns><c>true</c>, if the timeout occurred, <c>false</c> otherwise.</returns>
        public bool HitTimeoutLocked()
        {
            return (SystemClock.ElapsedRealtime() - mCaptureTimer) > PRECAPTURE_TIMEOUT_MS;
        }
     }
}