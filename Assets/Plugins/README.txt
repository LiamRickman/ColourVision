# Overview

Gfycat Screen Capture SDK allows you to capture in-game screens and upload the results to Gfycat.

Use of Gfycat’s APIs and SDKs requires authentication. Register for an API key at https://developers.gfycat.com/signup/ and include details about your product, so we know about the cool thing you're building. We love to highlight what developers are doing with our technology. If your product is not live when you begin developing with our APIs, update your app name and product URL later and send us a note to api@gfycat.com so we can check it out.

The Gfycat Screen Capture SDK for Unity consists of three classes:
- `Gfycat.ScreenCapture.AccountManager` responsible for Gfycat login/logout
- `Gfycat.ScreenCapture.ViewRecorder` responsible for screen capture
- `Gfycat.ScreenCapture.VideoUploader` responsible for uploading to Gfycat

To initialize the SDK you can either call the `Gfycat.ScreenCapture.AccountManager.Instance.SetClientCredentials` method passing your API key and secret or add the `Gfycat Credentials` component and configure credentials there.

To authenticate a user with Gfycat:

1. Call the `Gfycat.ScreenCapture.AccountManager.Instance.Login()` method and wait for it to complete.
2. Check the `Gfycat.ScreenCapture.AccountManager.Instance.IsLoggedIn` and `Gfycat.ScreenCapture.AccountManager.Instance.Username` properties to get user login state.

To start screen capture, do the following:

1. Create instance of `Gfycat.ScreenCapture.ViewRecorder` using `Create()` method and keep reference to it.
2. Configure capture options.
3. Call `Start()` to start recording.
    - Call `Pause()` to pause recording.
    - Call `Resume()` to resume recording.
4. Call `Finish()` passing `Action` callback to finish recording.
5. You can access the generated video file in the callback passed in (4).

To upload a video to Gfycat, do the following:

1. Ensure that user is authenticated with Gfycat.
2. Create instance of `Gfycat.ScreenCapture.VideoUploader` using `Create()` method and keep the reference to it.
3. Specify the source mp4 file path using `VideoPath` property.
4. Attach handlers to `OnGfycatUploaded` and `OnFailure` events.
5. Call `Upload()` to initiate upload process.
6. Use `GfycatUrl` and `GfyId` properties from your `OnGfycatUploaded` event handler to reference newly created gfycat.

Please refer to `Gfycat.ScreenCapture.Example.cs` file for more details.

# Gfycat.ScreenCapture.AccountManager class

This class provides Gfycat account management functionality.

## Properties

- `bool IsLoggedIn`: user login status
- `string Username`: currently logged in username

## Methods

- `static void SetClientCredentials(string appClientID, string appClientSecret)`: call this function once per application lifetime to specify Gfycat API credentials
- `void Login(Action<bool> completionHandler)`: authenticate a user with Gfycat
- `void Logout();`: clear currently authenticated user

# Gfycat.ScreenCapture.ViewRecorder class

This class is responsbile for providing the screen capture functionality.
Your application must keep this object alive until video capture session is finished or cancelled.

## Properties

- `string OutputPath`: path to mp4 file to be created (readonly)
- `IntPtr SourceView`: source view reference, initialized to main view by default
- `Rect SourceRect`: source rectangle in view coordinate system, defaults to full screen
- `float Scale`: capture scale, defaults to screen scale
- `Int32 FrameRate`: capture frame rate
- `Int32 KeyframeRate`: capture keyframe rate, set to the same value as `FrameRate` for best result
- `float BufferDuration`: maximum capture duration in seconds

## Methods

- `void Start()`: starts video capture session; no properties should be modified after this call
- `void Pause()`: pauses video capture session
- `void Resume()`: resumes paused video capture session
- `void Cancel()`: cancels started video capture session
- `void Finish(Action completionHandler)`: finishes action video capture session and calls `completionHandler` when done

# Gfycat.ScreenCapture.VideoUploader class

This class provides Gfycat video upload functionality.
Your application must keep this object alive until upload completes or fails.

## Properties

- `string VideoPath`: path to mp4 file to be uploaded
- `string Title`: Gfycat title
- `string GfyId`: Gfycat ID of the newly created Gfycat (readonly)
- `string GfycatUrl`: Gfycat URL of the newly created Gfycat (readonly)

# Events

- `Action<string> OnGfycatRegistered`: fired after allocating new Gfycat ID; receives gfyId as a parameter
- `Action<string> OnGfycatUploaded`: fired after uploading media content; receives gfyId as a parameter
- `Action<string> OnFailure`: fired in case of error; receives error message as a parameter

# Methods

- `void Upload(Action<string> completionHandler)`: initiates upload, passes Gfycat ID to the `completionHandler` on success or `null` on failure.
