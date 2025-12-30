'/*
'****************************************************************************************************
'* IBScanUltimateApi.vb
'*
'* DESCRIPTION:
'*     API functions for IBScanUltimate.
'*     http://www.integratedbiometrics.com
'*
'* NOTES:
'*     Copyright (c) Integrated Biometrics, 2009-2017
'*
'* HISTORY:
'*     2012/04/06  1.0.0  Created.
'*     2012/05/29  1.1.0  Added blocking API functions (IBSU_AsyncOpenDevice(), IBSU_BGetImage(), 
'*                            IBSU_BGetInitProgress(), IBSU_BGetClearPlatenAtCapture()).
'*     2012/11/06  1.4.1  Added rolling and extended open API functions (IBSU_BGetRollingInfo(),
'*                            IBSU_OpenDeviceEx()).
'*     2013/03/20  1.6.0  Added API function to support IBScanMatcher integration 
'*                            (IBSU_GetIBSM_ResultImageInfo(), IBSU_GetNFIQScore()).
'*     2013/04/05  1.6.2  Added API function to enable or disable trace log at run-time 
'*                            (IBSU_EnableTraceLog()).
'*     2013/08/03  1.6.9  Reformatted.
'*     2013/10/14  1.7.0  Added API functions to acquire an image from a device (blocking for resultEx),
'*                        deregister a callback function, show (or remove) an overlay object, 
'*                        show (or remove) all overlay objects, add an overlay text, modify an existing
'*                        overlay text, add an overlay line, modify an existing line, add an overlay
'*                        quadrangle, modify an existing quadrangle, add an overlay shape, modify an
'*                        overlay shape, save image to bitmap memory
'*                        (IBSU_BGetImageEx(), IBSU_ReleaseCallbacks(), IBSU_ShowOverlayObject,
'*                         IBSU_ShowAllOverlayObject(), IBSU_RemoveOverlayObject(), IBSU_RemoveAllOverlayObject(),
'*                         IBSU_AddOverlayText(), IBSU_ModifyOverlayText(), IBSU_AddOverlayLine(),
'*                         IBSU_ModifyOverlayLine(), IBSU_AddOverlayQuadrangle(), IBSU_ModifyOverlayQuadrangle(),
'*                         IBSU_AddOverlayShape(), IBSU_ModifyOverlayShape(), IBSU_SaveBitmapMem())
'*     2014/02/25  1.7.1  Changed datatype from string to StringBuilder for IBSU_SetClientDisplayProperty
'*     2014/07/23  1.8.0  Reformatted.
'*                        Added API functions are relelated to WSQ
'*                        (IBSU_WSQEncodeMem, IBSU_WSQEncodeToFile, IBSU_WSQDecodeMem,
'*                         IBSU_WSQDecodeFromFile, IBSU_FreeMemory)
'*     2014/09/17  1.8.1  Added API functions are relelated to JPEG2000 and PNG
'*                        (IBSU_SavePngImage, IBSU_SaveJP2Image)
'*     2015/03/04  1.8.3  Reformatted to support UNICODE for WinCE
'*                        Added API function is relelated to ClientWindow
'*                        (IBSU_RedrawClientWindow)
'*                        Bug Fixed, Added new parameter (pitch) to WSQ functions
'*                        (IBSU_WSQEncodeMem, IBSU_WSQEncodeToFile, IBSU_WSQDecodeMem,
'*                         IBSU_WSQDecodeFromFile)
'*     2015/04/07  1.8.4  Added API function to unload the library manually
'*                        (IBSU_UnloadLibrary)
'*     2015/08/05  1.8.5  Added API function to combine two image into one
'*                        (IBSU_CombineImage)
'*     2015/12/11  1.9.0  Added API function to support Kojak device
'*                        (IBSU_GetOperableBeeper, IBSU_SetBeeper)
'*                        Changed datatype of API function IBSU_GetIBSM_ResultImageInfo() at VB.Net
'*     2017/04/27  1.9.7  Added API function to support improved feature for CombineImage
'*                        (IBSU_CombineImageEx)
'*     2017/06/17  1.9.8  Added API function to support improved feature for CombineImage
'*                        (IBSU_CheckWetFinger, IBSU_BGetRollingInfoEx, IBSU_GetImageWidth,
'*                         IBSU_IsWritableDirectory)
'*     2017/08/22  1.9.9  Added API function to get final image by native for Columbo
'*                        (RESERVED_GetFinalImageByNative)
'*     2018/03/06  2.0.0  Added API function to improve dispaly speed on Embedded System
'*                        (IBSU_GenerateDisplayImage)
'*     2018/04/27  2.0.1  Added API function to improve dispaly speed on Embedded System
'*                        (IBSU_RemoveFingerImage, IBSU_AddFingerImage, IBSU_IsFingerDuplicated,
'*                         IBSU_IsValidFingerGeometry)
'*                        Deprecated API function about IBScanMater(IBSM)
'*                        (IBSU_GetIBSM_ResultImageInfo)
'*     2019/02/19  2.1.0  Added API function
'*                        (IBSU_GetRequiredSDKVersion)
'*     2019/06/21  3.0.0  Added API function
'*                        (IBSU_SetEncryptionKey, RESERVED_GetEncryptedImage, RESERVED_ConvertToDecryptImage)
'*     2020/10/06  3.7.0  Added API function to support new locking feature
'*                        (IBSU_SetCustomerKey, IBSU_GetErrorString)
'*     2021/08/04  3.7.2  Added API function to support new Spoof(PAD) function
'*                        (IBSU_IsSpoofFingerDetected)
'****************************************************************************************************
'*/

Imports System.Runtime.InteropServices
Imports System.Diagnostics
Imports System.Text


Public Class Win32
    <DllImport("user32.dll")> _
    Public Shared Function GetClientRect(ByVal hWnd As IntPtr, ByRef lpRect As DLL.IBSU_RECT) As Integer
    End Function

    <DllImport("kernel32.dll")> _
    Public Shared Sub Beep(ByVal freq As Integer, ByVal duration As Integer)
    End Sub

    <DllImport("user32.dll")> _
    Public Shared Function SetFocus(ByVal hWnd As IntPtr) As Integer
    End Function
End Class

Partial Public Class DLL
    '/*
    '****************************************************************************************************
    '* CALLBACK FUNCTION TYPES
    '****************************************************************************************************
    '*/

    '/*
    '****************************************************************************************************
    '* IBSU_Callback()
    '*
    '* DESCRIPTION:
    '*     Callback for ENUM_IBSU_ESSENTIAL_EVENT_COMMUNICATION_BREAK, called when communication with a
    '*     device is interrupted.
    '*
    '* ARGUMENTS:
    '*     deviceHandle  Device handle.
    '*     pContext      User context.
    '****************************************************************************************************
    '*/
    Public Delegate Sub IBSU_Callback( _
            ByVal deviceHandle As Integer, _
            ByVal pContext As IntPtr)

    '/*
    '****************************************************************************************************
    '* IBSU_CallbackPreviewImage()
    '*
    '* DESCRIPTION:
    '*     Callback for ENUM_IBSU_ESSENTIAL_EVENT_PREVIEW_IMAGE, called when a preview image is available.
    '*
    '* ARGUMENTS:
    '*     deviceHandle  Device handle.
    '*     pContext      User context.
    '*     image         Preview image data.  This structure, including the buffer, is valid only 
    '*                   within the function context.  If required for later use, any data must be 
    '*                   copied to another structure.
    '****************************************************************************************************
    '*/
    Public Delegate Sub IBSU_CallbackPreviewImage( _
            ByVal deviceHandle As Integer, _
            ByVal pContext As IntPtr, _
            ByVal image As IBSU_ImageData)

    '/*
    '****************************************************************************************************
    '* IBSU_CallbackFingerCount()
    '*
    '* DESCRIPTION:
    '*     Callback for ENUM_IBSU_OPTIONAL_EVENT_FINGER_COUNT, called when the finger count changes.
    '*
    '* ARGUMENTS:
    '*     deviceHandle      Device handle.
    '*     pContext          User context.
    '*     fingerCountState  Finger count state.
    '****************************************************************************************************
    '*/
    Public Delegate Sub IBSU_CallbackFingerCount( _
            ByVal deviceHandle As Integer, _
            ByVal pContext As IntPtr, _
            ByVal fingerCountState As IBSU_FingerCountState)

    '/*
    '****************************************************************************************************
    '* IBSU_CallbackFingerQuality()
    '*
    '* DESCRIPTION:
    '*     Callback for ENUM_IBSU_OPTIONAL_EVENT_FINGER_QUALITY, called when a finger quality changes.
    '*
    '* ARGUMENTS:
    '*     deviceHandle       Device handle.
    '*     pContext           User context.
    '*     pQualityArray      Array of finger qualities.
    '*     qualityArrayCount  Number of qualities in array.
    '****************************************************************************************************
    '*/
    Public Delegate Sub IBSU_CallbackFingerQuality( _
            ByVal deviceHandle As Integer, _
            ByVal pContext As IntPtr, _
            ByVal pQualityArray As IntPtr, _
            ByVal qualityArrayCount As Integer)

    '/*
    '****************************************************************************************************
    '* IBSU_CallbackDeviceCount()
    '*
    '* DESCRIPTION:
    '*     Callback for ENUM_IBSU_ESSENTIAL_EVENT_DEVICE_COUNT, called when the number of detected
    '*     devices changes.
    '*
    '* ARGUMENTS:
    '*     detectedDevices  Number of detected devices.
    '*     pContext         User context.
    '****************************************************************************************************
    '*/
    Public Delegate Sub IBSU_CallbackDeviceCount( _
            ByVal detectedDevices As Integer, _
            ByVal pContext As IntPtr)

    '/*
    '****************************************************************************************************
    '* IBSU_CallbackInitProgress()
    '*
    '* DESCRIPTION:
    '*     Callback for ENUM_IBSU_ESSENTIAL_EVENT_INIT_PROGRESS, called when the initialization progress
    '*     changes for a device.
    '*
    '* ARGUMENTS:
    '*     deviceIndex    Zero-based index of device.
    '*     pContext       User context.
    '*     progressValue  Initialization progress, as a percent, between 0 and 100, inclusive.
    '****************************************************************************************************
    '*/
    Public Delegate Sub IBSU_CallbackInitProgress( _
            ByVal deviceIndex As Integer, _
            ByVal pContext As IntPtr, _
            ByVal progressValue As Integer)

    '/*
    '****************************************************************************************************
    '* IBSU_CallbackTakingAcquisition()
    '*
    '* DESCRIPTION:
    '*     Callback for ENUM_IBSU_ESSENTIAL_EVENT_TAKING_ACQUISITION, called for a rolled print
    '*     acquisition when the rolling should begin.
    '*
    '* ARGUMENTS:
    '*     deviceHandle  Device handle.
    '*     pContext      User context.
    '*     imageType     Type of image being acquired.
    '****************************************************************************************************
    '*/
    Public Delegate Sub IBSU_CallbackTakingAcquisition( _
            ByVal deviceHandle As Integer, _
            ByVal pContext As IntPtr, _
            ByVal imageType As IBSU_ImageType)

    '/*
    '****************************************************************************************************
    '* IBSU_CallbackCompleteAcquisition()
    '*
    '* DESCRIPTION:
    '*     Callback for ENUM_IBSU_ESSENTIAL_EVENT_COMPLETE_ACQUISITION, called for a rolled print
    '*     acquisition when the rolling capture has completed.
    '*
    '* ARGUMENTS:
    '*     deviceHandle  Device handle.
    '*     pContext      User context.
    '*     imageType     Type of image being acquired.
    '****************************************************************************************************
    '*/
    Public Delegate Sub IBSU_CallbackCompleteAcquisition( _
            ByVal deviceHandle As Integer, _
            ByVal pContext As IntPtr, _
            ByVal imageType As IBSU_ImageType)

    '/*
    '****************************************************************************************************
    '* IBSU_CallbackResultImage()
    '*
    '* DESCRIPTION:
    '*     Callback for ENUM_IBSU_ESSENTIAL_EVENT_RESULT_IMAGE, called when the result image is 
    '*     available.
    '*
    '* ARGUMENTS:
    '*     deviceHandle          Device handle.
    '*     pContext              User context.
    '*     image                 Data of preview or result image.  The buffer in this structure points to 
    '*                           an internal image buffer; the data should be copied to an application 
    '*                           buffer if desired for future processing.
    '*     imageType             Image type.
    '*     pSplitImageArray      Array of four structures with data of individual finger images split
    '*                           from result image.  The buffers in these structures point to internal
    '*                           image buffers; the data should be copied to application buffers if
    '*                           desired for future processing.
    '*     splitImageArrayCount  Number of finger images split from result images.
    '****************************************************************************************************
    '*/
    Public Delegate Sub IBSU_CallbackResultImage( _
            ByVal deviceHandle As Integer, _
            ByVal pContext As IntPtr, _
            ByVal image As IBSU_ImageData, _
            ByVal imageType As IBSU_ImageType, _
            ByVal pSplitImageArray As IntPtr, _
            ByVal splitImageArrayCount As Integer)

    '/*
    '****************************************************************************************************
    '* IBSU_CallbackResultImageEx()
    '*
    '* DESCRIPTION:
    '*     Callback for ENUM_IBSU_ESSENTIAL_EVENT_RESULT_IMAGE_EX, called when the result image is 
    '*     available, with extended information.
    '*
    '* ARGUMENTS:
    '*     deviceHandle            Device handle.
    '*     pContext                User context.
    '*     imageStatus             Status from result image acquisition.  See error codes in 
    '*                             'IBScanUltimateApi_err'.
    '*     image                   Data of preview or result image.  The buffer in this structure points to 
    '*                             an internal image buffer; the data should be copied to an application 
    '*                             buffer if desired for future processing.
    '*     imageType               Image type.
    '*     detectedFingerCount     Number of detected fingers.
    '*     segmentImageArrayCount  Number of finger images split from result images.
    '*     pSegmentImageArray      Array of structures with data of individual finger images split from
    '*                             result image.  The buffers in these structures point to internal image 
    '*                             buffers; the data should be copied to application buffers if desired 
    '*                             for future processing.
    '*     pSegmentPositionArray   Array of structures with position data for individual fingers split 
    '*                             from result image.
    '****************************************************************************************************
    '*/
    Public Delegate Sub IBSU_CallbackResultImageEx( _
            ByVal deviceHandle As Integer, _
            ByVal pContext As IntPtr, _
            ByVal imageStatus As Integer, _
            ByVal image As IBSU_ImageData, _
            ByVal imageType As IBSU_ImageType, _
            ByVal detectedFingerCount As Integer, _
            ByVal segmentImageArrayCount As Integer, _
            ByVal pSegmentImageArray As IntPtr, _
            ByVal SegmentPositionArray As IntPtr)

    '/*
    '****************************************************************************************************
    '* IBSU_CallbackClearPlatenAtCapture()
    '*
    '* DESCRIPTION:
    '*     Callback for ENUM_IBSU_OPTIONAL_EVENT_CLEAR_PLATEN_AT_CAPTURE, called when the platen was not 
    '*     clear when capture started or has since become clear.
    '*
    '* ARGUMENTS:
    '*     deviceHandle  Device handle.
    '*     pContext      User context.
    '*     platenState   Platen state.
    '****************************************************************************************************
    '*/
    Public Delegate Sub IBSU_CallbackClearPlatenAtCapture( _
            ByVal deviceHandle As Integer, _
            ByVal pContext As IntPtr, _
            ByVal platenState As IBSU_PlatenState)

    '/*
    '****************************************************************************************************
    '* IBSU_CallbackAsyncOpenDevice()
    '*
    '* DESCRIPTION:
    '*     Callback for ENUM_IBSU_ESSENTIAL_EVENT_ASYNC_OPEN_DEVICE, called asynchronous device 
    '*     initialization completes
    '*
    '* ARGUMENTS:
    '*     deviceIndex   Zero-based index of device.
    '*     pContext      User context.
    '*     deviceHandle  Handle for subsequent function calls.
    '*     errorCode     Error that prevented initialization from completing.
    '****************************************************************************************************
    '*/
    Public Delegate Sub IBSU_CallbackAsyncOpenDevice( _
            ByVal deviceIndex As Integer, _
            ByVal pContext As IntPtr, _
            ByVal deviceHandle As Integer, _
            ByVal errorCode As Integer)

    '/*
    '****************************************************************************************************
    '* IBSU_CallbackNotifyMessage()
    '*
    '* DESCRIPTION:
    '*     Callback for ENUM_IBSU_OPTIONAL_EVENT_NOTIFY_MESSAGE, called when a warning message is 
    '*     generated.
    '*
    '* ARGUMENTS:
    '*     deviceHandle   Device handle.
    '*     pContext       User context.
    '*     notifyMessage  Status code as defined in IBScanUltimateApi_err.vb.
    '****************************************************************************************************
    '*/
    Public Delegate Sub IBSU_CallbackNotifyMessage( _
            ByVal deviceHandle As Integer, _
            ByVal pContext As IntPtr, _
            ByVal notifyMessage As Integer)

    '/*
    '****************************************************************************************************
    '* IBSU_CallbackKeyButtons()
    '*
    '* DESCRIPTION:
    '*     Callback for ENUM_IBSU_ESSENTIAL_EVENT_KEYBUTTON, called when the key button of device was chicked.
    '*
    '* ARGUMENTS:
    '*     deviceHandle       Device handle.
    '*     pContext           User context.
    '*     pressedKeyButtons  The key button index which is pressed.
    '****************************************************************************************************
    '*/
    Public Delegate Sub IBSU_CallbackKeyButtons( _
            ByVal deviceHandle As Integer, _
            ByVal pContext As IntPtr, _
            ByVal pressedKeyButtons As Integer)

	'/*
    '****************************************************************************************************
    '* IBSU_CallbackRequiredSDK()
    '*
    '* DESCRIPTION:
    '*     Callback for ENUM_IBSU_ESSENTIAL_EVENT_REQUIRED_SDK, when higher SDK version is requreid during device initialization.
    '* ARGUMENTS:
    '*     deviceIndex			Zero-based index of device.
    '*     pContext				User context.
    '*     pMinSDKVersion        Minimum SDK Version required for running
    '****************************************************************************************************
    '*/
    Public Delegate Sub IBSU_CallbackRequiredSDK( _
            ByVal deviceHandle As Integer, _
            ByVal pContext As IntPtr, _
            ByVal minSDKVersion As String)

    '/*
    '****************************************************************************************************
    '* GLOBAL FUNCTIONS
    '****************************************************************************************************
    '*/

    '/*
    '****************************************************************************************************
    '* IBSU_GetSDKVersion()
    '* 
    '* DESCRIPTION:
    '*     Obtain product and software version information.
    '*
    '* ARGUMENTS:
    '*     pVerinfo  Pointer to structure that will receive SDK version information.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '*/
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_GetSDKVersion( _
            ByRef pVerinfo As IBSU_SdkVersion) As Integer
    End Function
    Public Shared Function _IBSU_GetSDKVersion( _
            ByRef pVerinfo As IBSU_SdkVersion) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_GetSDKVersion(pVerinfo)
        Catch except As Exception
            Trace.WriteLine("IBSU_GetSDKVersion : " & except.Message)
        End Try

        Return nRc
    End Function
    '
    '  ****************************************************************************************************
    '* IBSU_GetDeviceCount()
    '* 
    '* DESCRIPTION:
    '*     Retrieve count of connected IB USB scanner devices.
    '*
    '* ARGUMENTS:
    '*     pDeviceCount  Pointer to variable that will receive count of connected IB USB scanner devices.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_GetDeviceCount( _
            ByRef pDeviceCount As Integer) As Integer
    End Function
    Public Shared Function _IBSU_GetDeviceCount( _
            ByRef pDeviceCount As Integer) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_GetDeviceCount(pDeviceCount)
        Catch except As Exception
            Trace.WriteLine("IBSU_GetDeviceCount : " & except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_GetDeviceDescription()
    '* 
    '* DESCRIPTION:
    '*     Retrieve detailed device information about a particular scanner by its logical index.
    '*
    '* ARGUMENTS:
    '*     deviceIndex  Zero-based index of the scanner.
    '*     pDeviceDesc  Pointer to structure that will receive description of device.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_GetDeviceDescription( _
            ByVal deviceIndex As Integer, _
            ByRef pDeviceDesc As IBSU_DeviceDesc) As Integer
    End Function
    Public Shared Function _IBSU_GetDeviceDescription( _
            ByVal deviceIndex As Integer, _
            ByRef pDeviceDesc As IBSU_DeviceDesc) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_GetDeviceDescription(deviceIndex, pDeviceDesc)
        Catch except As Exception
            Trace.WriteLine("IBSU_GetDeviceDescription : " & except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_RegisterCallbacks()
    '* 
    '* DESCRIPTION:
    '*     Register a callback function for an event type.  These asynchronous notifications enable 
    '*     event-driven management of scanner processes.  For more information about a particular 
    '*     event or the signature of its callback, see the definition of 'IBSU_Events'.
    '*
    '* ARGUMENTS:
    '*     handle             Handle for device associated with this event (if appropriate).
    '*     event              Event for which the callback is being registered.
    '*     pCallbackFunction  Pointer to the callback function.
    '*     pContext           Pointer to user context that will be passed to callback function.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_RegisterCallbacks( _
            ByVal handle As Integer, _
            ByVal events As IBSU_Events, _
            ByVal pEventName As [Delegate], _
            ByVal pContext As IntPtr) As Integer
    End Function
    Public Shared Function _IBSU_RegisterCallbacks( _
            ByVal handle As Integer, _
            ByVal events As IBSU_Events, _
            ByVal pEventName As [Delegate], _
            ByVal pContext As IntPtr) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_RegisterCallbacks(handle, events, pEventName, pContext)
        Catch except As Exception
            Trace.WriteLine("IBSU_RegisterCallbacks : " & except.Message)
        End Try

        Return nRc
    End Function

    '
    ' ****************************************************************************************************
    '* IBSU_OpenDevice()
    '* 
    '* DESCRIPTION:
    '*     Initialize a device and obtain a handle for subsequent function calls.  Any initialized device
    '*     must be released with IBSU_CloseDevice() or IBSU_CloseAllDevice() before shutting down the 
    '*     application.
    '*
    '* ARGUMENTS:
    '*     deviceIndex  Zero-based index of the scanner.
    '*     pHandle      Pointer to variable that will receive device handle for subsequent function calls.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_OpenDevice( _
            ByVal deviceIndex As Integer, _
            ByRef handle As Integer) As Integer
    End Function
    Public Shared Function _IBSU_OpenDevice( _
            ByVal deviceIndex As Integer, _
            ByRef handle As Integer) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_OpenDevice(deviceIndex, handle)
        Catch except As Exception
            Trace.WriteLine("IBSU_OpenDevice : " & except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_CloseDevice()
    '* 
    '* DESCRIPTION:
    '*     Release all resources for a device.
    '*
    '* ARGUMENTS:
    '*     handle  Device handle.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '*         IBSU_ERR_RESOURCE_LOCKED: A callback is still active.
    '*         IBSU_ERR_DEVICE_NOT_INITIALIZED: Device has already been released or is unknown.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_CloseDevice( _
            ByVal handle As Integer) As Integer
    End Function
    Public Shared Function _IBSU_CloseDevice( _
            ByVal handle As Integer) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_CloseDevice(handle)
        Catch except As Exception
            Trace.WriteLine("IBSU_CloseDevice : " & except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_CloseAllDevice()
    '* 
    '* DESCRIPTION:
    '*     Release all resources for all open devices.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '*         IBSU_ERR_RESOURCE_LOCKED: A callback is still active.
    '****************************************************************************************************
    '   
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_CloseAllDevice() As Integer
    End Function
    Public Shared Function _IBSU_CloseAllDevice() As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_CloseAllDevice()
        Catch except As Exception
            Trace.WriteLine("IBSU_CloseAllDevice : " & except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_IsDeviceOpened()
    '* 
    '* DESCRIPTION:
    '*     Check whether a device is open/initialized.
    '*
    '* ARGUMENTS:
    '*     handle  Device handle.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if device is open/initialized.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '*         IBSU_ERR_INVALID_PARAM_VALUE: Handle value is out of range.
    '*         IBSU_ERR_DEVICE_NOT_INITIALIZED: Device has not been initialized.
    '*         IBSU_ERR_DEVICE_IO: Device has been initialized, but there was a communication problem.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_IsDeviceOpened( _
            ByVal handle As Integer) As Integer
    End Function
    Public Shared Function _IBSU_IsDeviceOpened( _
            ByVal handle As Integer) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_IsDeviceOpened(handle)
        Catch except As Exception
            Trace.WriteLine("IBSU_IsDeviceOpened : " & except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_SetProperty()
    '* 
    '* DESCRIPTION:
    '*     Set the value of a property for a device.  For descriptions of properties and values, see 
    '*     definition of 'IBSU_PropertyId'.
    '*
    '* ARGUMENTS:
    '*     handle         Device handle.
    '*     propertyId     Property for which value will be set.
    '*     propertyValue  Value of property to set.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_SetProperty( _
            ByVal handle As Integer, _
            ByVal propertyId As IBSU_PropertyId, _
            ByVal propertyValue As StringBuilder) As Integer
    End Function
    Public Shared Function _IBSU_SetProperty( _
            ByVal handle As Integer, _
            ByVal propertyId As IBSU_PropertyId, _
            ByVal propertyValue As StringBuilder) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_SetProperty(handle, propertyId, propertyValue)
        Catch except As Exception
            Trace.WriteLine("IBSU_SetProperty : " & except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_GetProperty()
    '* 
    '* DESCRIPTION:
    '*     Get the value of a property for a device.  For descriptions of properties and values, see 
    '*     definition of 'IBSU_PropertyId'.
    '*
    '* ARGUMENTS:
    '*     handle          Device handle.
    '*     propertyId      Property for which value will be set.
    '*     propertyValue   Buffer in which value of property will be stored.  This buffer should be 
    '*                     able to hold IBSU_MAX_STR_LEN characters.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_GetProperty( _
            ByVal handle As Integer, _
            ByVal propertyId As IBSU_PropertyId, _
            ByVal propertyValue As StringBuilder) As Integer
    End Function
    Public Shared Function _IBSU_GetProperty( _
            ByVal handle As Integer, _
            ByVal propertyId As IBSU_PropertyId, _
            ByVal propertyValue As StringBuilder) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_GetProperty(handle, propertyId, propertyValue)
        Catch except As Exception
            Trace.WriteLine("IBSU_GetProperty : " & except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_EnableTraceLog()
    '* 
    '* DESCRIPTION:
    '*     Enable or disable trace log at run-time.  The trace log is enabled by default on Windows and
    '*     Android and disabled by default on Linux.
    '*
    '* ARGUMENTS:
    '*     on  Indicates whether trace log should be turned on or off.  TRUE to turn log on; FALSE to 
    '*         turn log off.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_EnableTraceLog( _
            ByVal onoff As Boolean) As Integer
    End Function
    Public Shared Function _IBSU_EnableTraceLog( _
            ByVal onoff As Boolean) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_EnableTraceLog(onoff)
        Catch except As Exception
            Trace.WriteLine("IBSU_EnableTraceLog : " & except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_IsCaptureAvailable()
    '* 
    '* DESCRIPTION:
    '*     Check whether capture mode is supported by a device.
    '*
    '* ARGUMENTS:
    '*     handle           Device handle.
    '*     imageType        Type of capture.
    '*     imageResolution  Resolution of capture.
    '*     pIsAvailable     Pointer to variable that will receive indicator of support.  Will be set to
    '*                      TRUE if mode is supported.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_IsCaptureAvailable( _
            ByVal handle As Integer, _
            ByVal imageType As IBSU_ImageType, _
            ByVal imageResolution As IBSU_ImageResolution, _
            ByRef pIsAvailable As Boolean) As Integer
    End Function
    Public Shared Function _IBSU_IsCaptureAvailable( _
            ByVal handle As Integer, _
            ByVal imageType As IBSU_ImageType, _
            ByVal imageResolution As IBSU_ImageResolution, _
            ByRef pIsAvailable As Boolean) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_IsCaptureAvailable(handle, imageType, imageResolution, pIsAvailable)
        Catch except As Exception
            Trace.WriteLine("IBSU_IsCaptureAvailable : " & except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_BeginCaptureImage()
    '* 
    '* DESCRIPTION:
    '*     Begin acquiring an image from a device.
    '*
    '* ARGUMENTS:
    '*     handle           Device handle.
    '*     imageType        Type of capture.
    '*     imageResolution  Resolution of capture.
    '*     captureOptions   Bit-wise OR of capture options:
    '*                          IBSU_OPTION_AUTO_CONTRAST - automatically adjust contrast to optimal value
    '*                          IBSU_OPTION_AUTO_CAPTURE - complete capture automatically when a good-
    '*                              quality image is available
    '*                          IBSU_OPTION_IGNORE_FINGER_COUNT - ignore finger count when deciding to 
    '*                              complete capture
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '*         IBSU_ERR_CAPTURE_STILL_RUNNING - An acquisition is currently executing and must complete 
    '*             before another capture can be started.
    '*         IBSU_ERR_CAPTURE_INVALID_MODE - Capture mode is not supported by this device. 
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_BeginCaptureImage( _
            ByVal handle As Integer, _
            ByVal imageType As IBSU_ImageType, _
            ByVal imageResolution As IBSU_ImageResolution, _
            ByVal captureOptions As UInteger) As Integer
    End Function
    Public Shared Function _IBSU_BeginCaptureImage( _
            ByVal handle As Integer, _
            ByVal imageType As IBSU_ImageType, _
            ByVal imageResolution As IBSU_ImageResolution, _
            ByVal captureOptions As UInteger) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_BeginCaptureImage(handle, imageType, imageResolution, captureOptions)
        Catch except As Exception
            Trace.WriteLine("IBSU_BeginCaptureImage : " & except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_CancelCaptureImage()
    '* 
    '* DESCRIPTION:
    '*     Abort acquisition on a device.
    '*
    '* ARGUMENTS:
    '*     handle  Device handle.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '*         IBSU_ERR_CAPTURE_NOT_RUNNING - Acquisition is not active. 
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_CancelCaptureImage( _
            ByVal handle As Integer) As Integer
    End Function
    Public Shared Function _IBSU_CancelCaptureImage( _
            ByVal handle As Integer) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_CancelCaptureImage(handle)
        Catch except As Exception
            Trace.WriteLine("IBSU_CancelCaptureImage : " & except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_IsCaptureActive()
    '* 
    '* DESCRIPTION:
    '*     Determine whether acquisition is active for a device.
    '*
    '* ARGUMENTS:
    '*     handle     Device handle.
    '*     pIsActive  Pointer to variable that will receive indicator of activity.  TRUE if a acquisition
    '*                is in progress.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_IsCaptureActive( _
            ByVal handle As Integer, _
            ByRef pIsActive As Boolean) As Integer
    End Function
    Public Shared Function _IBSU_IsCaptureActive( _
            ByVal handle As Integer, _
            ByRef pIsActive As Boolean) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_IsCaptureActive(handle, pIsActive)
        Catch except As Exception
            Trace.WriteLine("IBSU_IsCaptureActive : " & except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_TakeResultImageManually()
    '* 
    '* DESCRIPTION:
    '*     Begin acquiring an image from a device with image gain manually set.
    '*
    '* ARGUMENTS:
    '*     handle  Device handle.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_TakeResultImageManually( _
            ByVal handle As Integer) As Integer
    End Function
    Public Shared Function _IBSU_TakeResultImageManually( _
            ByVal handle As Integer) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_TakeResultImageManually(handle)
        Catch except As Exception
            Trace.WriteLine("IBSU_TakeResultImageManually : " & except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_GetContrast()
    '* 
    '* DESCRIPTION:
    '*     Get the contrast value for a device.
    '*
    '* ARGUMENTS:
    '*     handle          Device handle.
    '*     pContrastValue  Pointer to variable that will receive contrast value.  Value will be between
    '*                     IBSU_MIN_CONTRAST_VALUE and IBSU_MAX_CONTRAST_VALUE, inclusive.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_GetContrast( _
            ByVal handle As Integer, _
            ByRef pContrastValue As Integer) As Integer
    End Function
    Public Shared Function _IBSU_GetContrast( _
            ByVal handle As Integer, _
            ByRef pContrastValue As Integer) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_GetContrast(handle, pContrastValue)
        Catch except As Exception
            Trace.WriteLine("IBSU_GetContrast : " & except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_SetContrast()
    '* 
    '* DESCRIPTION:
    '*     Set the contrast value for a device.
    '*
    '* ARGUMENTS:
    '*     handle         Device handle.
    '*     contrastValue  Contrast value.  Value must be between IBSU_MIN_CONTRAST_VALUE and 
    '*                    IBSU_MAX_CONTRAST_VALUE, inclusive.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_SetContrast( _
            ByVal handle As Integer, _
            ByVal contrastValue As Integer) As Integer
    End Function
    Public Shared Function _IBSU_SetContrast( _
            ByVal handle As Integer, _
            ByVal contrastValue As Integer) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_SetContrast(handle, contrastValue)
        Catch except As Exception
            Trace.WriteLine("IBSU_SetContrast : " & except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_SetLEOperationMode()
    '* 
    '* DESCRIPTION:
    '*     Set the light-emitting (LE) film operation mode for a device.
    '*
    '* ARGUMENTS:
    '*     handle           Device handle.
    '*     leOperationMode  LE film operation mode.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_SetLEOperationMode( _
            ByVal handle As Integer, _
            ByVal leOperationMode As IBSU_LEOperationMode) As Integer
    End Function
    Public Shared Function _IBSU_SetLEOperationMode( _
            ByVal handle As Integer, _
            ByVal leOperationMode As IBSU_LEOperationMode) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_SetLEOperationMode(handle, leOperationMode)
        Catch except As Exception
            Trace.WriteLine("IBSU_SetLEOperationMode : " & except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_GetLEOperationMode()
    '* 
    '* DESCRIPTION:
    '*     Get the light-emitting (LE) film operation mode for a device.
    '*
    '* ARGUMENTS:
    '*     handle            Device handle.
    '*     pLeOperationMode  Pointer to variable that will receive LE film operation mode.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_GetLEOperationMode( _
            ByVal handle As Integer, _
            ByRef leOperationMode As IBSU_LEOperationMode) As Integer
    End Function
    Public Shared Function _IBSU_GetLEOperationMode( _
            ByVal handle As Integer, _
            ByRef leOperationMode As IBSU_LEOperationMode) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_GetLEOperationMode(handle, leOperationMode)
        Catch except As Exception
            Trace.WriteLine("IBSU_GetLEOperationMode : " & except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_IsTouchedFinger()
    '* 
    '* DESCRIPTION:
    '*     Determine whether a finger is on a scanner's surface.  This function queries the proximity 
    '*     detector only integrated into some sensors. 
    '*
    '* ARGUMENTS:
    '*     handle         Device handle.
    '*     pTouchInValue  Pointer to variable that will receive touch input value.  0 if no finger is 
    '*                    touching the surface, 1 if one or more fingers are touching the surface.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_IsTouchedFinger( _
            ByVal handle As Integer, _
            ByRef touchInValue As Integer) As Integer
    End Function
    Public Shared Function _IBSU_IsTouchedFinger( _
            ByVal handle As Integer, _
            ByRef touchInValue As Integer) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_IsTouchedFinger(handle, touchInValue)
        Catch except As Exception
            Trace.WriteLine("IBSU_IsTouchedFinger : " & except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_GetOperableLEDs()
    '* 
    '* DESCRIPTION:
    '*     Get characteristics of operable LEDs on a device. 
    '*
    '* ARGUMENTS:
    '*     handle         Device handle.
    '*     pLedType       Pointer to variable that will receive type of LEDs.
    '*     pLedCount      Pointer to variable that will receive count of LEDs.
    '*     pOperableLEDs  Pointer to variable that will receive bit-pattern of operable LEDs.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_GetOperableLEDs( _
            ByVal handle As Integer, _
            ByRef pLedType As IBSU_LedType, _
            ByRef pLedCount As Integer, _
            ByRef pOperableLEDs As UInteger) As Integer
    End Function
    Public Shared Function _IBSU_GetOperableLEDs( _
            ByVal handle As Integer, _
            ByRef pLedType As IBSU_LedType, _
            ByRef pLedCount As Integer, _
            ByRef pOperableLEDs As UInteger) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_GetOperableLEDs(handle, pLedType, pLedCount, pOperableLEDs)
        Catch except As Exception
            Trace.WriteLine("IBSU_GetOperableLEDs : " & except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_GetLEDs()
    '* 
    '* DESCRIPTION:
    '*     Get the value of LEDs on a device. 
    '*
    '* ARGUMENTS:
    '*     handle       Device handle.
    '*     pActiveLEDs  Pointer to variable that will receive bit-pattern of LED values.  Set bits 
    '*                  indicate LEDs that are on; clear bits indicate LEDs that are off.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_GetLEDs( _
            ByVal handle As Integer, _
            ByRef pActiveLEDs As UInteger) As Integer
    End Function
    Public Shared Function _IBSU_GetLEDs( _
            ByVal handle As Integer, _
            ByRef pActiveLEDs As UInteger) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_GetLEDs(handle, pActiveLEDs)
        Catch except As Exception
            Trace.WriteLine("IBSU_GetLEDs : " & except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_SetLEDs()
    '* 
    '* DESCRIPTION:
    '*     Set the value of LEDs on a device. 
    '*
    '* ARGUMENTS:
    '*     handle      Device handle.
    '*     activeLEDs  Bit-pattern of LED values.  Set bits indicate LEDs to turn on; clear bits indicate 
    '*                 LEDs to turn off.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_SetLEDs( _
            ByVal handle As Integer, _
            ByVal activeLEDs As UInteger) As Integer
    End Function
    Public Shared Function _IBSU_SetLEDs( _
            ByVal handle As Integer, _
            ByVal activeLEDs As UInteger) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_SetLEDs(handle, activeLEDs)
        Catch except As Exception
            Trace.WriteLine("IBSU_SetLEDs : " & except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_CreateClientWindow()
    '* 
    '* DESCRIPTION:
    '*     Create client window associated with device.  (Available only on Windows.)
    '*
    '* ARGUMENTS:
    '*     handle   Device handle.
    '*     hWindow  Windows handle to draw.
    '*     left     Coordinate of left edge of rectangle.
    '*     top      Coordinate of top edge of rectangle.
    '*     right    Coordinate of right edge of rectangle.
    '*     bottom   Coordinate of bottom edge of rectangle.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_CreateClientWindow( _
            ByVal handle As Integer, _
            ByVal hWindow As IntPtr, _
            ByVal left As Integer, _
            ByVal top As Integer, _
            ByVal right As Integer, _
            ByVal bottom As Integer) As Integer
    End Function
    Public Shared Function _IBSU_CreateClientWindow( _
            ByVal handle As Integer, _
            ByVal hWindow As IntPtr, _
            ByVal left As Integer, _
            ByVal top As Integer, _
            ByVal right As Integer, _
            ByVal bottom As Integer) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_CreateClientWindow(handle, hWindow, left, top, right, bottom)
        Catch except As Exception
            Trace.WriteLine("IBSU_CreateClientWindow : " & except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_DestroyClientWindow()
    '* 
    '* DESCRIPTION:
    '*     Destroy client window associated with device.  (Available only on Windows.)
    '*
    '* ARGUMENTS:
    '*     handle             Device handle.
    '*     clearExistingInfo  Indicates whether the existing display information, including display
    '*                        properties and overlay text, will be cleared.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_DestroyClientWindow( _
            ByVal handle As Integer, _
            ByVal clearExistingInfo As Boolean) As Integer
    End Function
    Public Shared Function _IBSU_DestroyClientWindow( _
            ByVal handle As Integer, _
            ByVal clearExistingInfo As Boolean) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_DestroyClientWindow(handle, clearExistingInfo)
        Catch except As Exception
            Trace.WriteLine("IBSU_DestroyClientWindow : " & except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_GetClientWindowProperty()
    '* 
    '* DESCRIPTION:
    '*     Get the value of a property for the client window associated with a device.  For descriptions 
    '*     of properties and values, see definition of 'IBSU_ClientWindowPropertyId'.  (Available only on
    '*     Windows.)
    '*
    '* ARGUMENTS:
    '*     handle          Device handle.
    '*     propertyId      Property for which value will be set.
    '*     propertyValue   Buffer in which value of property will be stored.  This buffer should be 
    '*                     able to hold IBSU_MAX_STR_LEN characters.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_GetClientWindowProperty( _
            ByVal handle As Integer, _
            ByVal propertyId As IBSU_ClientWindowPropertyId, _
            ByVal propertyValue As StringBuilder) As Integer
    End Function
    Public Shared Function _IBSU_GetClientWindowProperty( _
            ByVal handle As Integer, _
            ByVal propertyId As IBSU_ClientWindowPropertyId, _
            ByVal propertyValue As StringBuilder) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_GetClientWindowProperty(handle, propertyId, propertyValue)
        Catch except As Exception
            Trace.WriteLine("IBSU_GetClientWindowProperty : " & except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_SetClientDisplayProperty()
    '* 
    '* DESCRIPTION:
    '*     Set the value of a property for the client window associated with a device.  For descriptions 
    '*     of properties and values, see definition of 'IBSU_ClientWindowPropertyId'.  (Available only on
    '*     Windows.)
    '*
    '* ARGUMENTS:
    '*     handle          Device handle.
    '*     propertyId      Property for which value will be set.
    '*     propertyValue   Value of property to set.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_SetClientDisplayProperty( _
            ByVal handle As Integer, _
            ByVal propertyId As IBSU_ClientWindowPropertyId, _
            ByVal propertyValue As StringBuilder) As Integer
    End Function
    Public Shared Function _IBSU_SetClientDisplayProperty( _
            ByVal handle As Integer, _
            ByVal propertyId As IBSU_ClientWindowPropertyId, _
            ByVal propertyValue As StringBuilder) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_SetClientDisplayProperty(handle, propertyId, propertyValue)
        Catch except As Exception
            Trace.WriteLine("IBSU_SetClientDisplayProperty : " & except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_SetClientWindowOverlayText()
    '* 
    '* DESCRIPTION:
    '*     Set the overlay text for the client window associated with a device.  (Available only on
    '*     Windows.)
    '*
    '* ARGUMENTS:
    '*     handle     Device handle.
    '*     fontName   Font name.
    '*     fontSize   Font size.
    '*     fontBold   Indicates whether font will be bold.
    '*     text       Text to display.
    '*     posX       X-coordinate of text.
    '*     poxY       Y-coordinate of text.
    '*     textColor  Color of text.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_SetClientWindowOverlayText( _
            ByVal handle As Integer, _
            ByVal fontName As String, _
            ByVal fontSize As Integer, _
            ByVal fontBold As Boolean, _
            ByVal text As String, _
            ByVal posX As Integer, _
            ByVal posY As Integer, _
            ByVal textColor As UInteger) As Integer
    End Function
    Public Shared Function _IBSU_SetClientWindowOverlayText( _
            ByVal handle As Integer, _
            ByVal fontName As String, _
            ByVal fontSize As Integer, _
            ByVal fontBold As Boolean, _
            ByVal text As String, _
            ByVal posX As Integer, _
            ByVal posY As Integer, _
            ByVal textColor As UInteger) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_SetClientWindowOverlayText(handle, fontName, fontSize, fontBold, text, posX, _
             posY, textColor)
        Catch except As Exception
            Trace.WriteLine("IBSU_SetClientWindowOverlayText : " & except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_GenerateZoomOutImage()
    '* 
    '* DESCRIPTION:
    '*     Generate scaled version of image.
    '*
    '* ARGUMENTS:
    '*     inImage     Original image.
    '*     outImage    Pointer to buffer that will receive output image.  This buffer must hold at least
    '*                 'outWidth' x 'outHeight' bytes.
    '*     outWidth    Width of output image.
    '*     outHeight   Height of output image.
    '*     bkColor     Background color of output image.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_GenerateZoomOutImage( _
            ByVal inImage As IBSU_ImageData, _
            ByVal outImage As IntPtr, _
            ByVal outWidth As Integer, _
            ByVal outHeight As Integer, _
            ByVal bkColor As Byte) As Integer
    End Function
    Public Shared Function _IBSU_GenerateZoomOutImage( _
            ByVal inImage As IBSU_ImageData, _
            ByVal outImage As IntPtr, _
            ByVal outWidth As Integer, _
            ByVal outHeight As Integer, _
            ByVal bkColor As Byte) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_GenerateZoomOutImage(inImage, outImage, outWidth, outHeight, bkColor)
        Catch except As Exception
            Trace.WriteLine("IBSU_GenerateZoomOutImage : " & except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_SaveBitmapImage()
    '* 
    '* DESCRIPTION:
    '*     Save image to bitmap file.
    '*
    '* ARGUMENTS:
    '*     filePath   Path of file for output image.
    '*     imgBuffer  Pointer to image buffer.
    '*     width      Image width (in pixels).
    '*     height     Image height (in pixels).
    '*     pitch      Image line pitch (in bytes).  A positive value indicates top-down line order; a
    '*                negative value indicates bottom-up line order.
    '*     resX       Horizontal image resolution (in pixels/inch).
    '*     resY       Vertical image resolution (in pixels/inch).
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_SaveBitmapImage( _
            ByVal filePath As String, _
            ByVal imgBuffer As IntPtr, _
            ByVal width As UInteger, _
            ByVal height As UInteger, _
            ByVal pitch As Integer, _
            ByVal resX As Double, _
            ByVal resY As Double) As Integer
    End Function
    Public Shared Function _IBSU_SaveBitmapImage( _
            ByVal filePath As String, _
            ByVal imgBuffer As IntPtr, _
            ByVal width As UInteger, _
            ByVal height As UInteger, _
            ByVal pitch As Integer, _
            ByVal resX As Double, _
            ByVal resY As Double) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_SaveBitmapImage(filePath, imgBuffer, width, height, pitch, resX, resY)
        Catch except As Exception
            Trace.WriteLine("IBSU_SaveBitmapImage : " & except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_AsyncOpenDevice()
    '* 
    '* DESCRIPTION:
    '*     Initialize a device asynchronously.  The handle will be delivered to the application with a
    '*     ENUM_IBSU_ESSENTIAL_EVENT_ASYNC_OPEN_DEVICE callback or with IBSU_BGetInitProgress().  Any 
    '*     initialized device must be released with IBSU_CloseDevice() or IBSU_CloseAllDevice() before 
    '*     shutting down the application.
    '*
    '* ARGUMENTS:
    '*     deviceIndex  Zero-based index of the scanner.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_AsyncOpenDevice( _
            ByVal deviceIndex As Integer) As Integer
    End Function
    Public Shared Function _IBSU_AsyncOpenDevice( _
            ByVal deviceIndex As Integer) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_AsyncOpenDevice(deviceIndex)
        Catch except As Exception
            Trace.WriteLine("IBSU_AsyncOpenDevice : " & except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_OpenDeviceEx()
    '* 
    '* DESCRIPTION:
    '*     Initialize a device and obtain a handle for subsequent function calls.  The uniformity mask
    '*     will be loaded from a file to speed up initialization.  Any initialized device must be 
    '*     released with IBSU_CloseDevice() or IBSU_CloseAllDevice() before shutting down the 
    '*     application.
    '*
    '* ARGUMENTS:
    '*     deviceIndex         Zero-based index of the scanner.
    '*     uniformityMaskPath  Path at which uniformity mask file is located.  If file does not exist,
    '*                         it will be created to accelerate subsequent initializations.
    '*     ayncnOpen           Indicates whether initialization will be performed synchronously or 
    '*                         asynchronously.
    '*     pHandle             Pointer to variable that will receive device handle for subsequent 
    '*                         function calls, if 'asyncOpen' is FALSE.  Otherwise, handle will be 
    '*                         delivered to the application with a ENUM_IBSU_ESSENTIAL_EVEN_ASYNC_OPEN_DEVICE 
    '*                         callback or with IBSU_BGetInitProgress().
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_OpenDeviceEx( _
            ByVal deviceIndex As Integer, _
            ByVal uniformityMaskPath As String, _
            ByVal asyncOpen As Boolean, _
            ByRef pHandle As Integer) As Integer
    End Function
    Public Shared Function _IBSU_OpenDeviceEx( _
            ByVal deviceIndex As Integer, _
            ByVal uniformityMaskPath As String, _
            ByVal asyncOpen As Boolean, _
            ByRef pHandle As Integer) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_OpenDeviceEx(deviceIndex, uniformityMaskPath, asyncOpen, pHandle)
        Catch except As Exception
            Trace.WriteLine("IBSU_OpenDeviceEx : " & except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_GetIBSM_ResultImageInfo() (deprecated)
    '* 
    '* DESCRIPTION:
    '*     Get result image information.
    '*
    '* ARGUMENTS:
    '*     handle                  Device handle.
    '*     fingerPosition          Finger position.
    '*     pResultImage            Pointer to structure that will receive data of preview or result image.   
    '*                             The buffer in this structure points to an internal image buffer; the 
    '*                             data should be copied to an application buffer if desired for future 
    '*                             processing.
    '*     pSplitResultImage       Pointer to array of four structures that will receive individual finger 
    '*                             images split from result image.  The buffers in these structures point
    '*                             to internal image buffers; the data should be copied to application 
    '*                             buffers if desired for future processing.
    '*     pSplitResultImageCount  Pointer to variable that will receive number of finger images split 
    '*                             from result image.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_GetIBSM_ResultImageInfo( _
            ByVal handle As Integer, _
            ByVal fingerPosition As IBSM_FingerPosition, _
            ByRef pResultImage As IBSM_ImageData, _
            ByRef pSplitResultImage As IBSM_ImageData, _
            ByRef pSplitResultImageCount As Integer) As Integer
    End Function
    Public Shared Function _IBSU_GetIBSM_ResultImageInfo( _
            ByVal handle As Integer, _
            ByVal fingerPosition As IBSM_FingerPosition, _
            ByRef pResultImage As IBSM_ImageData, _
            ByRef pSplitResultImage As IBSM_ImageData, _
            ByRef pSplitResultImageCount As Integer) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_GetIBSM_ResultImageInfo(handle, fingerPosition, pResultImage, pSplitResultImage, pSplitResultImageCount)
        Catch except As Exception
            Trace.WriteLine("IBSU_GetIBSM_ResultImageInfo : " & except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_GetNFIQScore()
    '* 
    '* DESCRIPTION:
    '*     Calculate NFIQ score for image.
    '*
    '* ARGUMENTS:
    '*     handle        Device handle.
    '*     imgBuffer     Pointer to image buffer.
    '*     width         Image width (in pixels).
    '*     height        Image height (in pixels).
    '*     bitsPerPixel  Bits per pixel.
    '*     pScore        Pointer to variable that will receive NFIQ score.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_GetNFIQScore( _
            ByVal handle As Integer, _
            ByVal imgBuffer As IntPtr, _
            ByVal width As UInteger, _
            ByVal height As UInteger, _
            ByVal bitsPerPixel As Byte, _
            ByRef pScore As Integer) As Integer
    End Function
    Public Shared Function _IBSU_GetNFIQScore( _
            ByVal handle As Integer, _
            ByVal imgBuffer As IntPtr, _
            ByVal width As UInteger, _
            ByVal height As UInteger, _
            ByVal bitsPerPixel As Byte, _
            ByRef pScore As Integer) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_GetNFIQScore(handle, imgBuffer, width, height, bitsPerPixel, pScore)
        Catch except As Exception
            Trace.WriteLine("IBSU_GetNFIQScore : " & except.Message)
        End Try

        Return nRc
    End Function

    '
    ' ****************************************************************************************************
    '* IBSU_GenerateZoomOutImageEx()
    '* 
    '* DESCRIPTION:
    '*     Generate scaled version of image.
    '*
    '* ARGUMENTS:
    '*     inImage     Original image data.
    '*     inWidth     Width of input image.
    '*     in Height   Width of input image.
    '*     outImage    Pointer to buffer that will receive output image.  This buffer must hold at least
    '*                 'outWidth' x 'outHeight' bytes.
    '*     outWidth    Width of output image.
    '*     outHeight   Height of output image.
    '*     bkColor     Background color of output image.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_GenerateZoomOutImageEx( _
            ByVal inImage As IntPtr, _
            ByVal inWidth As UInteger, _
            ByVal inHeight As UInteger, _
            ByVal outImage As IntPtr, _
            ByVal outWidth As Integer, _
            ByVal outHeight As Integer, _
            ByVal bkColor As Byte) As Integer
    End Function
    Public Shared Function _IBSU_GenerateZoomOutImageEx( _
            ByVal inImage As IntPtr, _
            ByVal inWidth As UInteger, _
            ByVal inHeight As UInteger, _
            ByVal outImage As IntPtr, _
            ByVal outWidth As Integer, _
            ByVal outHeight As Integer, _
            ByVal bkColor As Byte) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_GenerateZoomOutImageEx(inImage, inWidth, inHeight, outImage, outWidth, outHeight, bkColor)
        Catch except As Exception
            Trace.WriteLine("IBSU_GenerateZoomOutImageEx : " & except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_ReleaseCallbacks()
    '* 
    '* DESCRIPTION:
    '*     Unregister a callback function for an event type.  These asynchronous notifications enable 
    '*     event-driven management of scanner processes.  For more information about a particular 
    '*     event or the signature of its callback, see the definition of 'IBSU_Events'.
    '*
    '* ARGUMENTS:
    '*     handle             Handle for device associated with this event (if appropriate).
    '*     event              Event for which the callback is being unregistered.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_ReleaseCallbacks( _
            ByVal handle As Integer, _
            ByVal events As IBSU_Events) As Integer
    End Function
    Public Shared Function _IBSU_ReleaseCallbacks( _
            ByVal handle As Integer, _
            ByVal events As IBSU_Events) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_ReleaseCallbacks(handle, events)
        Catch except As Exception
            Trace.WriteLine("IBSU_ReleaseCallbacks : " + except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_SaveBitmapMem()
    '* 
    '* DESCRIPTION:
    '*     Save image to bitmap memory.
    '*
    '* ARGUMENTS:
    '*     inImage           Point to image data (Gray scale image).
    '*     inWidth           Image width (in pixels).
    '*     inHeight          Image height (in pixels).
    '*     inPitch           Image line pitch (in bytes).  A positive value indicates top-down line order; a
    '*                       negative value indicates bottom-up line order.
    '*     inResX            Horizontal image resolution (in pixels/inch).
    '*     inResY            Vertical image resolution (in pixels/inch).
    '*     outBitmapBuffer   Pointer to output image data buffer which is set image format and zoom-out factor; a
    '*                       Memory must be provided by caller
    '*                       Required memory buffer size with argument @ref outImageFormat, 
    '*                       @ref IBSU_IMG_FORMAT_GRAY, @ref IBSU_BMP_GRAY_HEADER_LEN + outWidth * outHeight bytes
    '*                       @ref IBSU_IMG_FORMAT_RGB24, @ref IBSU_BMP_RGB24_HEADER_LEN + 3 * outWidth * outHeight bytes
    '*                       @ref IBSU_IMG_FORMAT_RGB32,  @ref IBSU_BMP_RGB32_HEADER_LEN + 4 * outWidth * outHeight bytes
    '*     outImageFormat    Set Image color format for output image
    '*     outWidth          Width for zoom-out image
    '*     outHeight         Height for zoom-out image
    '*     bkColor           Background color for remain area from zoom-out image
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_SaveBitmapMem( _
            ByVal inImage As IntPtr, _
            ByVal width As UInteger, _
            ByVal height As UInteger, _
            ByVal pitch As Integer, _
            ByVal resX As Double, _
            ByVal resY As Double, _
            ByVal outBitmapBuffer As IntPtr, _
            ByVal outImageFormat As IBSU_ImageFormat, _
            ByVal outWidth As UInteger, _
            ByVal outHeight As UInteger, _
            ByVal bkColor As Byte) As Integer
    End Function
    Public Shared Function _IBSU_SaveBitmapMem( _
            ByVal inImage As IntPtr, _
            ByVal width As UInteger, _
            ByVal height As UInteger, _
            ByVal pitch As Integer, _
            ByVal resX As Double, _
            ByVal resY As Double, _
            ByVal outBitmapBuffer As IntPtr, _
            ByVal outImageFormat As IBSU_ImageFormat, _
            ByVal outWidth As UInteger, _
            ByVal outHeight As UInteger, _
            ByVal bkColor As Byte) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_SaveBitmapMem(inImage, width, height, pitch, resX, resY, _
             outBitmapBuffer, outImageFormat, outWidth, outHeight, bkColor)
        Catch except As Exception
            Trace.WriteLine("IBSU_SaveBitmapMem : " + except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_ShowOverlayObject()
    '* 
    '* DESCRIPTION:
    '*     Show an overlay objects which is used with overlay handle.
    '*
    '* ARGUMENTS:
    '*     handle            Handle for device associated with this event (if appropriate).
    '*     overlayHandle	    Overlay handle obtained by overlay functions.
    '*     show	            If TRUE, the overlay will be shown on client window.
    '*                       If FALSE, the overlay will be hidden on client window.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_ShowOverlayObject( _
            ByVal handle As Integer, _
            ByVal overlayHandle As Integer, _
            ByVal show As Boolean) As Integer
    End Function
    Public Shared Function _IBSU_ShowOverlayObject( _
            ByVal handle As Integer, _
            ByVal overlayHandle As Integer, _
            ByVal show As Boolean) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_ShowOverlayObject(handle, overlayHandle, show)
        Catch except As Exception
            Trace.WriteLine("IBSU_ShowOverlayObject : " + except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_ShowAllOverlayObject()
    '* 
    '* DESCRIPTION:
    '*     Show all of overlay objects.
    '*
    '* ARGUMENTS:
    '*     handle            Handle for device associated with this event (if appropriate).
    '*     show	            If TRUE, the overlay will be shown on client window.
    '*                       If FALSE, the overlay will be hidden on client window.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_ShowAllOverlayObject( _
            ByVal handle As Integer, _
            ByVal show As Boolean) As Integer
    End Function
    Public Shared Function _IBSU_ShowAllOverlayObject( _
            ByVal handle As Integer, _
            ByVal show As Boolean) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_ShowAllOverlayObject(handle, show)
        Catch except As Exception
            Trace.WriteLine("IBSU_ShowAllOverlayObject : " + except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_RemoveOverlayObject()
    '* 
    '* DESCRIPTION:
    '*     Remove an overlay objects which is used with overlay handle.
    '*
    '* ARGUMENTS:
    '*     handle            Handle for device associated with this event (if appropriate).
    '*     overlayHandle	    Overlay handle obtained by overlay functions.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_RemoveOverlayObject( _
            ByVal handle As Integer, _
            ByVal overlayHandle As Integer) As Integer
    End Function
    Public Shared Function _IBSU_RemoveOverlayObject( _
            ByVal handle As Integer, _
            ByVal overlayHandle As Integer) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_RemoveOverlayObject(handle, overlayHandle)
        Catch except As Exception
            Trace.WriteLine("IBSU_RemoveOverlayObject : " + except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_RemoveAllOverlayObject()
    '* 
    '* DESCRIPTION:
    '*     Remove all of overlay objects.
    '* 
    '* ARGUMENTS:
    '*     handle            Handle for device associated with this event (if appropriate).
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_RemoveAllOverlayObject( _
            ByVal handle As Integer) As Integer
    End Function
    Public Shared Function _IBSU_RemoveAllOverlayObject( _
            ByVal handle As Integer) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_RemoveAllOverlayObject(handle)
        Catch except As Exception
            Trace.WriteLine("IBSU_RemoveAllOverlayObject : " + except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_AddOverlayText()
    '* 
    '* DESCRIPTION:
    '*	  Add an overlay text for display on window    
    '* 
    '* ARGUMENTS:
    '*     handle            Handle for device associated with this event (if appropriate).
    '*	  pOverlayHandle	Function returns overlay handle to be used for client windows functions call
    '*     fontName			used kind of font
    '*	  fontSize			used font size	
    '*	  fontBold			if font is bold or not
    '*	  text				text for display on window
    '*	  posX				X coordinate of text for display on window
    '*	  posY				Y coordinate of text for display on window
    '*	  textColor			text color
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_AddOverlayText( _
            ByVal handle As Integer, _
            ByRef pOverlayHandle As Integer, _
            ByVal fontName As String, _
            ByVal fontSize As Integer, _
            ByVal fontBold As Boolean, _
            ByVal text As String, _
            ByVal posX As Integer, _
            ByVal posY As Integer, _
            ByVal textColor As UInteger) As Integer
    End Function
    Public Shared Function _IBSU_AddOverlayText( _
            ByVal handle As Integer, _
            ByRef pOverlayHandle As Integer, _
            ByVal fontName As String, _
            ByVal fontSize As Integer, _
            ByVal fontBold As Boolean, _
            ByVal text As String, _
            ByVal posX As Integer, _
            ByVal posY As Integer, _
            ByVal textColor As UInteger) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_AddOverlayText(handle, pOverlayHandle, fontName, fontSize, fontBold, text, _
             posX, posY, textColor)
        Catch except As Exception
            Trace.WriteLine("IBSU_AddOverlayText : " + except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_ModifyOverlayText()
    '* 
    '* DESCRIPTION:
    '*	  Modify an existing overlay text for display on window    
    '* 
    '* ARGUMENTS:
    '*     handle            Handle for device associated with this event (if appropriate).
    '*	  OverlayHandle		Overlay handle to be modified
    '*     fontName			used kind of font
    '*	  fontSize			used font size	
    '*	  fontBold			if font is bold or not
    '*	  text				text for display on window
    '*	  posX				X coordinate of text for display on window
    '*	  posY				Y coordinate of text for display on window
    '*	  textColor			text color
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_ModifyOverlayText( _
            ByVal handle As Integer, _
            ByVal overlayHandle As Integer, _
            ByVal fontName As String, _
            ByVal fontSize As Integer, _
            ByVal fontBold As Boolean, _
            ByVal text As String, _
            ByVal posX As Integer, _
            ByVal posY As Integer, _
            ByVal textColor As UInteger) As Integer
    End Function
    Public Shared Function _IBSU_ModifyOverlayText( _
            ByVal handle As Integer, _
            ByVal overlayHandle As Integer, _
            ByVal fontName As String, _
            ByVal fontSize As Integer, _
            ByVal fontBold As Boolean, _
            ByVal text As String, _
            ByVal posX As Integer, _
            ByVal posY As Integer, _
            ByVal textColor As UInteger) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_ModifyOverlayText(handle, overlayHandle, fontName, fontSize, fontBold, text, _
             posX, posY, textColor)
        Catch except As Exception
            Trace.WriteLine("IBSU_ModifyOverlayText : " + except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_AddOverlayLine()
    '* 
    '* DESCRIPTION:
    '*	  Add an overlay line for display on window    
    '* 
    '* ARGUMENTS:
    '*     handle            Handle for device associated with this event (if appropriate).
    '*	  pOverlayHandle    Function returns overlay handle to be used for client windows functions calls
    '*     x1				X coordinate of start point of line
    '*	  y1				Y coordinate of start point of line
    '*	  x2				X coordinate of end point of line
    '*	  y2				Y coordinate of end point of line
    '*	  lineWidth		    line width
    '*	  lineColor			line color
    '*	 
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_AddOverlayLine( _
            ByVal handle As Integer, _
            ByRef pOverlayHandle As Integer, _
            ByVal x1 As Integer, _
            ByVal y1 As Integer, _
            ByVal x2 As Integer, _
            ByVal y2 As Integer, _
            ByVal lineWidth As Integer, _
            ByVal lineColor As UInteger) As Integer
    End Function
    Public Shared Function _IBSU_AddOverlayLine( _
            ByVal handle As Integer, _
            ByRef pOverlayHandle As Integer, _
            ByVal x1 As Integer, _
            ByVal y1 As Integer, _
            ByVal x2 As Integer, _
            ByVal y2 As Integer, _
            ByVal lineWidth As Integer, _
            ByVal lineColor As UInteger) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_AddOverlayLine(handle, pOverlayHandle, x1, y1, x2, y2, _
             lineWidth, lineColor)
        Catch except As Exception
            Trace.WriteLine("IBSU_AddOverlayLine : " + except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_ModifyOverlayLine()
    '* 
    '* DESCRIPTION:
    '*	  Modify an existing line for display on window    
    '* 
    '* ARGUMENTS:
    '*     handle            Handle for device associated with this event (if appropriate).
    '*	  OverlayHandle     OverlayHandle
    '*     x1				X coordinate of start point of line
    '*	  y1				Y coordinate of start point of line
    '*	  x2				X coordinate of end point of line
    '*	  y2				Y coordinate of end point of line
    '*	  lineWidth		    line width
    '*	  lineColor			line color
    '*	 
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_ModifyOverlayLine( _
            ByVal handle As Integer, _
            ByVal overlayHandle As Integer, _
            ByVal x1 As Integer, _
            ByVal y1 As Integer, _
            ByVal x2 As Integer, _
            ByVal y2 As Integer, _
            ByVal lineWidth As Integer, _
            ByVal lineColor As UInteger) As Integer
    End Function
    Public Shared Function _IBSU_ModifyOverlayLine( _
            ByVal handle As Integer, _
            ByVal overlayHandle As Integer, _
            ByVal x1 As Integer, _
            ByVal y1 As Integer, _
            ByVal x2 As Integer, _
            ByVal y2 As Integer, _
            ByVal lineWidth As Integer, _
            ByVal lineColor As UInteger) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_ModifyOverlayLine(handle, overlayHandle, x1, y1, x2, y2, _
             lineWidth, lineColor)
        Catch except As Exception
            Trace.WriteLine("IBSU_ModifyOverlayLine : " + except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_AddOverlayQuadrangle()
    '* 
    '* DESCRIPTION:
    '*	  Add an overlay quadrangle for display on window    
    '* 
    '* ARGUMENTS:
    '*     handle            Handle for device associated with this event (if appropriate).
    '*	  pOverlayHandle    Function returns overlay handle to be used for client windows functions calls
    '*     x1				X coordinate of start point of line
    '*	  y1				Y coordinate of start point of line
    '*	  x2				X coordinate of 1st corner of quadrangle
    '*	  y2				Y coordinate of 1st corner of quadrangle
    '*     x3				X coordinate of 2nd corner of quadrangle
    '*	  y3				Y coordinate of 2nd corner of quadrangle
    '*	  x4				X coordinate of 3rd corner of quadrangle
    '*	  y4				Y coordinate of 2rd corner of quadrangle
    '*	  lineWidth		    line width
    '*	  lineColor			line color
    '*	 
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_AddOverlayQuadrangle( _
            ByVal handle As Integer, _
            ByRef pOverlayHandle As Integer, _
            ByVal x1 As Integer, _
            ByVal y1 As Integer, _
            ByVal x2 As Integer, _
            ByVal y2 As Integer, _
            ByVal x3 As Integer, _
            ByVal y3 As Integer, _
            ByVal x4 As Integer, _
            ByVal y4 As Integer, _
            ByVal lineWidth As Integer, _
            ByVal lineColor As UInteger) As Integer
    End Function
    Public Shared Function _IBSU_AddOverlayQuadrangle( _
            ByVal handle As Integer, _
            ByRef pOverlayHandle As Integer, _
            ByVal x1 As Integer, _
            ByVal y1 As Integer, _
            ByVal x2 As Integer, _
            ByVal y2 As Integer, _
            ByVal x3 As Integer, _
            ByVal y3 As Integer, _
            ByVal x4 As Integer, _
            ByVal y4 As Integer, _
            ByVal lineWidth As Integer, _
            ByVal lineColor As UInteger) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_AddOverlayQuadrangle(handle, pOverlayHandle, x1, y1, x2, y2, _
             x3, y3, x4, y4, lineWidth, lineColor)
        Catch except As Exception
            Trace.WriteLine("IBSU_AddOverlayQuadrangle : " + except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_ModifyOverlayQuadrangle()
    '* 
    '* DESCRIPTION:
    '*	  Modify an existing quadrangle for display on window    
    '* 
    '* ARGUMENTS:
    '*     handle            Handle for device associated with this event (if appropriate).
    '*	  OverlayHandle     Overlay handle to be modified
    '*     x1				X coordinate of start point of line
    '*	  y1				Y coordinate of start point of line
    '*	  x2				X coordinate of 1st corner of quadrangle
    '*	  y2				Y coordinate of 1st corner of quadrangle
    '*     x3				X coordinate of 2nd corner of quadrangle
    '*	  y3				Y coordinate of 2nd corner of quadrangle
    '*	  x4				X coordinate of 3rd corner of quadrangle
    '*	  y4				Y coordinate of 2rd corner of quadrangle
    '*	  lineWidth		    line width
    '*	  lineColor			line color
    '*	 
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_ModifyOverlayQuadrangle( _
            ByVal handle As Integer, _
            ByVal overlayHandle As Integer, _
            ByVal x1 As Integer, _
            ByVal y1 As Integer, _
            ByVal x2 As Integer, _
            ByVal y2 As Integer, _
            ByVal x3 As Integer, _
            ByVal y3 As Integer, _
            ByVal x4 As Integer, _
            ByVal y4 As Integer, _
            ByVal lineWidth As Integer, _
            ByVal lineColor As UInteger) As Integer
    End Function
    Public Shared Function _IBSU_ModifyOverlayQuadrangle( _
            ByVal handle As Integer, _
            ByVal overlayHandle As Integer, _
            ByVal x1 As Integer, _
            ByVal y1 As Integer, _
            ByVal x2 As Integer, _
            ByVal y2 As Integer, _
            ByVal x3 As Integer, _
            ByVal y3 As Integer, _
            ByVal x4 As Integer, _
            ByVal y4 As Integer, _
            ByVal lineWidth As Integer, _
            ByVal lineColor As UInteger) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_ModifyOverlayQuadrangle(handle, overlayHandle, x1, y1, x2, y2, _
             x3, y3, x4, y4, lineWidth, lineColor)
        Catch except As Exception
            Trace.WriteLine("IBSU_ModifyOverlayQuadrangle : " + except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_AddOverlayShape()
    '* 
    '* DESCRIPTION:
    '*	  Add an overlay shape for display on window    
    '* 
    '* ARGUMENTS:
    '*     handle            Handle for device associated with this event (if appropriate).
    '*	  pOverlayHandle    Function returns overlay handle to be used for client windows functions calls
    '*     shapePattern		Predefined overlay shape		
    '*	  x1				X coordinate of start point of overlay shape
    '*	  y1				Y coordinate of start point of overlay shape
    '*     x2				X coordinate of end point of overlay shape
    '*	  y2				Y coordinate of end point of overlay shape
    '*	  lineWidth		    line width
    '*	  lineColor			line color
    '*	  reserved_1		X Reserved
    '*	  reserved_2		Y Reserved
    '*
    '*						If you set shapePattern to ENUM_IBSU_OVERLAY_SHAPE_ARROW
    '*						* reserved_1 can use the width(in pixels) of the full base of the arrowhead
    '*						* reserved_2 can use the angle(in radians) at the arrow tip between the two sides of the arrowhead
    '*	 
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_AddOverlayShape( _
            ByVal handle As Integer, _
            ByRef pOverlayHandle As Integer, _
            ByVal shapePattern As IBSU_OverlayShapePattern, _
            ByVal x1 As Integer, _
            ByVal y1 As Integer, _
            ByVal x2 As Integer, _
            ByVal y2 As Integer, _
            ByVal lineWidth As Integer, _
            ByVal lineColor As UInteger, _
            ByVal reserved_1 As Integer, _
            ByVal reserved_2 As Integer) As Integer
    End Function
    Public Shared Function _IBSU_AddOverlayShape( _
            ByVal handle As Integer, _
            ByRef pOverlayHandle As Integer, _
            ByVal shapePattern As IBSU_OverlayShapePattern, _
            ByVal x1 As Integer, _
            ByVal y1 As Integer, _
            ByVal x2 As Integer, _
            ByVal y2 As Integer, _
            ByVal lineWidth As Integer, _
            ByVal lineColor As UInteger, _
            ByVal reserved_1 As Integer, _
            ByVal reserved_2 As Integer) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_AddOverlayShape(handle, pOverlayHandle, shapePattern, x1, y1, x2, _
             y2, lineWidth, lineColor, reserved_1, reserved_2)
        Catch except As Exception
            Trace.WriteLine("IBSU_AddOverlayShape : " + except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_ModifyOverlayShape()
    '* 
    '* DESCRIPTION:
    '*	  Modify an overlay shape for display on window    
    '* 
    '* ARGUMENTS:
    '*     handle            Handle for device associated with this event (if appropriate).
    '*	  OverlayHandle		Overlay handle to be modified
    '*     shapePattern				
    '*	  x1				X coordinate of start point of overlay shape
    '*	  y1				Y coordinate of start point of overlay shape
    '*     x2				X coordinate of end point of overlay shape
    '*	  y2				Y coordinate of end point of overlay shape
    '*	  lineWidth		    line width
    '*	  lineColor			line color
    '*	  reserved_1		X Reserved
    '*	  reserved_2		Y Reserved
    '*
    '*						If you set shapePattern to ENUM_IBSU_OVERLAY_SHAPE_ARROW
    '*						* reserved_1 can use the width(in pixels) of the full base of the arrowhead
    '*						* reserved_2 can use the angle(in radians) at the arrow tip between the two sides of the arrowhead
    '*	 
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_ModifyOverlayShape( _
            ByVal handle As Integer, _
            ByVal overlayHandle As Integer, _
            ByVal shapePattern As IBSU_OverlayShapePattern, _
            ByVal x1 As Integer, _
            ByVal y1 As Integer, _
            ByVal x2 As Integer, _
            ByVal y2 As Integer, _
            ByVal lineWidth As Integer, _
            ByVal lineColor As UInteger, _
            ByVal reserved_1 As Integer, _
            ByVal reserved_2 As Integer) As Integer
    End Function
    Public Shared Function _IBSU_ModifyOverlayShape( _
            ByVal handle As Integer, _
            ByVal overlayHandle As Integer, _
            ByVal shapePattern As IBSU_OverlayShapePattern, _
            ByVal x1 As Integer, _
            ByVal y1 As Integer, _
            ByVal x2 As Integer, _
            ByVal y2 As Integer, _
            ByVal lineWidth As Integer, _
            ByVal lineColor As UInteger, _
            ByVal reserved_1 As Integer, _
            ByVal reserved_2 As Integer) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_ModifyOverlayShape(handle, overlayHandle, shapePattern, x1, y1, x2, _
             y2, lineWidth, lineColor, reserved_1, reserved_2)
        Catch except As Exception
            Trace.WriteLine("IBSU_ModifyOverlayShape : " + except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_WSQEncodeMem()
    '* 
    '* DESCRIPTION:
    '*     WSQ compresses grayscale fingerprint image.
    '*
    '* ARGUMENTS:
    '*     image             Original image.
    '*     width             Width of original image (in pixels).
    '*     height            Height of original image (in pixels).
    '*     pitch             Image line pitch (in bytes).  A positive value indicates top-down line order; a
    '*                       negative value indicates bottom-up line order.
    '*     bitsPerPixel      Bits per pixel of original image.
    '*     pixelPerInch      Pixel per inch of original image.
    '*     bitRate           Determines the amount of lossy compression.
    '                        Suggested settings:
    '                        bitRate = 2.25 yields around 5:1 compression
    '                        bitRate = 0.75 yields around 15:1 compression
    '*     commentText       Comment to write compressed data.
    '*     compressedData    Pointer of image which is compressed from original image by WSQ compression.
    '*                       This pointer is deallocated by IBSU_FreeMemory() after using it.
    '*     compressedLength  Length of image which is compressed from original image by WSQ compression.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_WSQEncodeMem( _
            ByVal image As IntPtr, _
            ByVal width As Integer, _
            ByVal height As Integer, _
            ByVal pitch As Integer, _
            ByVal bitsPerPixel As Integer, _
            ByVal pixelPerInch As Integer, _
            ByVal bitRate As Double, _
            ByVal commentText As String, _
            ByRef compressedData As IntPtr, _
            ByRef compressedLength As Integer) As Integer
    End Function
    Public Shared Function _IBSU_WSQEncodeMem( _
            ByVal image As IntPtr, _
            ByVal width As Integer, _
            ByVal height As Integer, _
            ByVal pitch As Integer, _
            ByVal bitsPerPixel As Integer, _
            ByVal pixelPerInch As Integer, _
            ByVal bitRate As Double, _
            ByVal commentText As String, _
            ByRef compressedData As IntPtr, _
            ByRef compressedLength As Integer) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_WSQEncodeMem(image, width, height, pitch, bitsPerPixel, pixelPerInch, _
                    bitRate, commentText, compressedData, compressedLength)
        Catch except As Exception
            Trace.WriteLine("IBSU_WSQEncodeMem : " + except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_WSQEncodeToFile()
    '* 
    '* DESCRIPTION:
    '*     Save WSQ compresses grayscale fingerprint image to specific file path.
    '*
    '* ARGUMENTS:
    '*     filePath          File path to save image which is compressed from original image by WSQ compression.
    '*     image             Original image.
    '*     width             Width of original image (in pixels).
    '*     height            Height of original image (in pixels).
    '*     pitch             Image line pitch (in bytes).  A positive value indicates top-down line order; a
    '*                       negative value indicates bottom-up line order.
    '*     bitsPerPixel      Bits per pixel of original image.
    '*     pixelPerInch      Pixel per inch of original image.
    '*     bitRate           Determines the amount of lossy compression.
    '                        Suggested settings:
    '                        bitRate = 2.25 yields around 5:1 compression
    '                        bitRate = 0.75 yields around 15:1 compression
    '*     commentText       Comment to write compressed data.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_WSQEncodeToFile( _
            ByVal filePath As String, _
            ByVal image As IntPtr, _
            ByVal width As Integer, _
            ByVal height As Integer, _
            ByVal pitch As Integer, _
            ByVal bitsPerPixel As Integer, _
            ByVal pixelPerInch As Integer, _
            ByVal bitRate As Double, _
            ByVal commentText As String) As Integer
    End Function
    Public Shared Function _IBSU_WSQEncodeToFile( _
            ByVal filePath As String, _
            ByVal image As IntPtr, _
            ByVal width As Integer, _
            ByVal height As Integer, _
            ByVal pitch As Integer, _
            ByVal bitsPerPixel As Integer, _
            ByVal pixelPerInch As Integer, _
            ByVal bitRate As Double, _
            ByVal commentText As String) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_WSQEncodeToFile(filePath, image, width, height, pitch, bitsPerPixel, pixelPerInch, _
                    bitRate, commentText)
        Catch except As Exception
            Trace.WriteLine("IBSU_WSQEncodeToFile : " + except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_WSQDecodeMem()
    '* 
    '* DESCRIPTION:
    '*     Decompress a WSQ-encoded grayscale fingerprint image.
    '*
    '* ARGUMENTS:
    '*     compressedImage   WSQ-encoded image.
    '*     compressedLength  Length of WSQ-encoded image.
    '*     decompressedImage Pointer of image which is decompressed from WSQ-encoded image.
    '*                       This pointer is deallocated by IBSU_FreeMemory() after using it.
    '*     outWidth          Width of decompressed image (in pixels).
    '*     outHeight         Height of decompressed image (in pixels).
    '*     outPitch          Image line pitch (in bytes).  A positive value indicates top-down line order; a
    '*                       negative value indicates bottom-up line order.
    '*     outBitsPerPixel   Bits per pixel of decompressed image.
    '*     outPixelPerInch   Pixel per inch of decompressed image.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_WSQDecodeMem( _
            ByVal compressedImage As IntPtr, _
            ByVal compressedLength As Integer, _
            ByRef decompressedImage As IntPtr, _
            ByRef outWidth As Integer, _
            ByRef outHeight As Integer, _
            ByRef outPitch As Integer, _
            ByRef outBitsPerPixel As Integer, _
            ByRef outPixelPerInch As Integer) As Integer
    End Function
    Public Shared Function _IBSU_WSQDecodeMem( _
            ByVal compressedImage As IntPtr, _
            ByVal compressedLength As Integer, _
            ByRef decompressedImage As IntPtr, _
            ByRef outWidth As Integer, _
            ByRef outHeight As Integer, _
            ByRef outPitch As Integer, _
            ByRef outBitsPerPixel As Integer, _
            ByRef outPixelPerInch As Integer) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_WSQDecodeMem(compressedImage, compressedLength, decompressedImage, _
                                    outWidth, outHeight, outPitch, outBitsPerPixel, outPixelPerInch)
        Catch except As Exception
            Trace.WriteLine("IBSU_WSQDecodeMem : " + except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_WSQDecodeFromFile()
    '* 
    '* DESCRIPTION:
    '*     Decompress a WSQ-encoded grayscale fingerprint image from specific file path.
    '*
    '* ARGUMENTS:
    '*     filePath          File path of WSQ-encoded image.
    '*     decompressedImage Pointer of image which is decompressed from WSQ-encoded image.
    '*                       This pointer is deallocated by IBSU_FreeMemory() after using it.
    '*     outWidth          Width of decompressed image (in pixels).
    '*     outHeight         Height of decompressed image (in pixels).
    '*     outPitch          Image line pitch (in bytes).  A positive value indicates top-down line order; a
    '*                       negative value indicates bottom-up line order.
    '*     outBitsPerPixel   Bits per pixel of decompressed image.
    '*     outPixelPerInch   Pixel per inch of decompressed image.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_WSQDecodeFromFile( _
            ByVal filePath As String, _
            ByRef decompressedImage As IntPtr, _
            ByRef outWidth As Integer, _
            ByRef outHeight As Integer, _
            ByRef outPitch As Integer, _
            ByRef outBitsPerPixel As Integer, _
            ByRef outPixelPerInch As Integer) As Integer
    End Function
    Public Shared Function _IBSU_WSQDecodeFromFile( _
            ByVal filePath As String, _
            ByRef decompressedImage As IntPtr, _
            ByRef outWidth As Integer, _
            ByRef outHeight As Integer, _
            ByRef outPitch As Integer, _
            ByRef outBitsPerPixel As Integer, _
            ByRef outPixelPerInch As Integer) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_WSQDecodeFromFile(filePath, decompressedImage, _
                                    outWidth, outHeight, outPitch, outBitsPerPixel, outPixelPerInch)
        Catch except As Exception
            Trace.WriteLine("IBSU_WSQDecodeFromFile : " + except.Message)
        End Try

        Return nRc
    End Function

    '
    '****************************************************************************************************
    '* IBSU_FreeMemory()
    '* 
    '* DESCRIPTION:
    '*     Release the allocated memory block on the internal heap of library.
    '*     This is obtained by IBSU_WSQEncodeMem(), IBSU_WSQDecodeMem, IBSU_WSQDecodeFromFile() and other API functions.
    '*
    '* ARGUMENTS:
    '*     memblock          Previously allocated memory block to be freed.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_FreeMemory( _
            ByVal memblock As IntPtr) As Integer
    End Function
    Public Shared Function _IBSU_FreeMemory( _
            ByVal memblock As IntPtr) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_FreeMemory(memblock)
        Catch except As Exception
            Trace.WriteLine("IBSU_FreeMemory : " + except.Message)
        End Try

        Return nRc
    End Function

    '/*
    '****************************************************************************************************
    '* IBSU_SavePngImage()
    '* 
    '* DESCRIPTION:
    '*     Save image to PNG file.
    '*
    '* ARGUMENTS:
    '*     filePath   Path of file for output image.
    '*     imgBuffer  Pointer to image buffer.
    '*     width      Image width (in pixels).
    '*     height     Image height (in pixels).
    '*     pitch      Image line pitch (in bytes).  A positive value indicates top-down line order; a
    '*                negative value indicates bottom-up line order.
    '*     resX       Horizontal image resolution (in pixels/inch).
    '*     resY       Vertical image resolution (in pixels/inch).
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '*/
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_SavePngImage( _
            ByVal filePath As String, _
            ByVal imgBuffer As IntPtr, _
            ByVal width As UInteger, _
            ByVal height As UInteger, _
            ByVal pitch As Integer, _
            ByVal resX As Double, _
            ByVal resY As Double) As Integer
    End Function
    Public Shared Function _IBSU_SavePngImage( _
            ByVal filePath As String, _
            ByVal imgBuffer As IntPtr, _
            ByVal width As UInteger, _
            ByVal height As UInteger, _
            ByVal pitch As Integer, _
            ByVal resX As Double, _
            ByVal resY As Double) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_SavePngImage(filePath, imgBuffer, width, height, pitch, resX, resY)
        Catch except As Exception
            Trace.WriteLine("IBSU_SavePngImage : " & except.Message)
        End Try

        Return nRc
    End Function

    '/*
    '****************************************************************************************************
    '* IBSU_SaveJP2Image()
    '* 
    '* DESCRIPTION:
    '*     Save image to JPEG-2000 file.
    '*
    '* ARGUMENTS:
    '*     filePath   Path of file for output image.
    '*     imgBuffer  Pointer to image buffer.
    '*     width      Image width (in pixels).
    '*     height     Image height (in pixels).
    '*     pitch      Image line pitch (in bytes).  A positive value indicates top-down line order; a
    '*                negative value indicates bottom-up line order.
    '*     resX       Horizontal image resolution (in pixels/inch).
    '*     resY       Vertical image resolution (in pixels/inch).
    '*     fQuality   Quality level for JPEG2000, he valid range is between 0 and 100
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '*/
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_SaveJP2Image( _
            ByVal filePath As String, _
            ByVal imgBuffer As IntPtr, _
            ByVal width As UInteger, _
            ByVal height As UInteger, _
            ByVal pitch As Integer, _
            ByVal resX As Double, _
            ByVal resY As Double, _
            ByVal fQuality As Integer) As Integer
    End Function
    Public Shared Function _IBSU_SaveJP2Image( _
            ByVal filePath As String, _
            ByVal imgBuffer As IntPtr, _
            ByVal width As UInteger, _
            ByVal height As UInteger, _
            ByVal pitch As Integer, _
            ByVal resX As Double, _
            ByVal resY As Double, _
            ByVal fQuality As Integer) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_SaveJP2Image(filePath, imgBuffer, width, height, pitch, resX, resY, fQuality)
        Catch except As Exception
            Trace.WriteLine("IBSU_SaveJP2Image : " & except.Message)
        End Try

        Return nRc
    End Function

    '/*
    '****************************************************************************************************
    '* IBSU_RedrawClientWindow()
    '* 
    '* DESCRIPTION:
    '*     Update the specified client window which is defined by IBSU_CreateClientWindow().  (Available only on Windows.)
    '*
    '* ARGUMENTS:
    '*     handle          Device handle.
    '*     flags           Bit-pattern of redraw flags.  See flag codes in 'IBScanUltimateApi_def
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '*/
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_RedrawClientWindow( _
            ByVal handle As UInteger) As Integer
    End Function
    Public Shared Function _IBSU_RedrawClientWindow( _
            ByVal handle As UInteger) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_RedrawClientWindow(handle)
        Catch except As Exception
            Trace.WriteLine("IBSU_RedrawClientWindow : " & except.Message)
        End Try

        Return nRc
    End Function

    '/*
    '****************************************************************************************************
    '* IBSU_UnloadLibrary()
    '* 
    '* DESCRIPTION:
    '*     The library is unmapped from the address space manually, and the library is no longer valid
    '*     So APIs will not be worked correctly after calling 
    '*     Some platform SDKs (Windows Mobile, Android)
    '*     can be needed to call IBSU_UnloadLibrary() before shutting down the application.
    '*
    '* ARGUMENTS:
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '*/
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_UnloadLibrary() As Integer
    End Function
    Public Shared Function _IBSU_UnloadLibrary() As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_UnloadLibrary()
        Catch except As Exception
            Trace.WriteLine("IBSU_UnloadLibrary : " & except.Message)
        End Try

        Return nRc
    End Function

    '****************************************************************************************************
    '* IBSU_GetOperableBeeper()
    '* 
    '* DESCRIPTION:
    '*     Get characteristics of operable Beeper on a device. 
    '*
    '* ARGUMENTS:
    '*     handle         Device handle.
    '*     pBeeperType    Pointer to variable that will receive type of Beeper.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '*/
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_GetOperableBeeper( _
            ByVal handle As Integer, _
            ByRef pBeeperType As IBSU_BeeperType) As Integer
    End Function
    Public Shared Function _IBSU_GetOperableBeeper( _
            ByVal handle As Integer, _
            ByRef pBeeperType As IBSU_BeeperType) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_GetOperableBeeper(handle, pBeeperType)
        Catch except As Exception
            Trace.WriteLine("IBSU_GetOperableBeeper : " + except.Message)
        End Try

        Return nRc
    End Function

    '/*
    '****************************************************************************************************
    '* IBSU_SetBeeper()
    '* 
    '* DESCRIPTION:
    '*     Set the value of Beeper on a device. 
    '*
    '* ARGUMENTS:
    '*     handle          Device handle.
    '*     beepPattern     Pattern of beep.
    '*     soundTone       The frequency of the sound, in specific value. The parameter must be
    '*                     in the range 0 through 2.
    '*     duration        The duration of the sound, in 25 miliseconds. The parameter must be
    '*                     in the range 1 through 200 at ENUM_IBSU_BEEP_PATTERN_GENERIC,
    '*                     in the range 1 through 7 at ENUM_IBSU_BEEP_PATTERN_REPEAT.
    '*     reserved_1      Reserved
    '*     reserved_2      Reserved
    '*                     If you set beepPattern to ENUM_IBSU_BEEP_PATTERN_REPEAT
    '*                     reserved_1 can use the sleep time after duration of the sound, in 25 miliseconds.
    '*                     The parameter must be in the range 1 through 8
    '*                     reserved_2 can use the operation(start/stop of pattern repeat), 1 to start; 0 to stop 
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '*/
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_SetBeeper( _
            ByVal handle As Integer, _
            ByVal beepPattern As IBSU_BeepPattern, _
            ByVal soundTone As UInteger, _
            ByVal duration As UInteger, _
            ByVal reserved_1 As UInteger, _
            ByVal reserved_2 As UInteger) As Integer
    End Function
    Public Shared Function _IBSU_SetBeeper( _
            ByVal handle As Integer, _
            ByVal beepPattern As IBSU_BeepPattern, _
            ByVal soundTone As UInteger, _
            ByVal duration As UInteger, _
            ByVal reserved_1 As UInteger, _
            ByVal reserved_2 As UInteger) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_SetBeeper(handle, beepPattern, soundTone, duration, reserved_1, reserved_2)
        Catch except As Exception
            Trace.WriteLine("IBSU_SetBeeper : " + except.Message)
        End Try

        Return nRc
    End Function

    '/*
    '****************************************************************************************************
    '* IBSU_CombineImage()
    '* 
    '* DESCRIPTION:
    '*     Combine two images (2 flat fingers) into a single image (left/right hands) 
    '*
    '* ARGUMENTS:
    '*     inImage1		  Pointer to IBSU_ImageData ( index and middle finger )
    '*     inImage2		  Pointer to IBSU_ImageData ( ring and little finger )
    '*	   whichHand      Information of left or right hand
    '*     outImage		  Pointer to IBSU_ImageData ( 1600 x 1500 fixed size image )
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '*/
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_CombineImage( _
            ByVal inImage1 As IBSU_ImageData, _
            ByVal inImage2 As IBSU_ImageData, _
            ByVal whichHand As IBSU_CombineImageWhichHand, _
            ByVal outImage As IntPtr) As Integer
    End Function
    Public Shared Function _IBSU_CombineImage( _
            ByVal inImage1 As IBSU_ImageData, _
            ByVal inImage2 As IBSU_ImageData, _
            ByVal whichHand As IBSU_CombineImageWhichHand, _
            ByVal outImage As IntPtr) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_CombineImage(inImage1, inImage2, whichHand, outImage)
        Catch except As Exception
            Trace.WriteLine("IBSU_CombineImage : " & except.Message)
        End Try

        Return nRc
    End Function

    '/*
    '****************************************************************************************************
    '* IBSU_CombineImageEx()
    '* 
    '* DESCRIPTION:
    '*     Combine two images (2 flat fingers) into a single image (left/right hands) 
    '*     and return segment information as well
    '*
    '* ARGUMENTS:
    '*     inImage1		  Pointer to IBSU_ImageData ( index and middle finger )
    '*     inImage2		  Pointer to IBSU_ImageData ( ring and little finger )
    '*	   whichHand      Information of left or right hand
    '*     outImage		  Pointer to IBSU_ImageData ( 1600 x 1500 fixed size image )
    '*     pSegmentImageArray        Pointer to array of four structures that will receive individual finger 
    '*                               image segments from output image.  The buffers in these structures point
    '*                               to internal image buffers; the data should be copied to application 
    '*                               buffers if desired for future processing.
    '*     pSegmentPositionArray     Pointer to array of four structures that will receive position data for 
    '*                               individual fingers split from output image.
    '*     pSegmentImageArrayCount   Pointer to variable that will receive number of finger images split 
    '*                               from output image.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '*/
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_CombineImageEx( _
            ByVal inImage1 As IBSU_ImageData, _
            ByVal inImage2 As IBSU_ImageData, _
            ByVal whichHand As IBSU_CombineImageWhichHand, _
            ByVal outImage As IntPtr, _
            ByVal pSegmentImageArray As IntPtr, _
            ByVal pSegmentPositionArray As IntPtr, _
            ByRef pSegmentImageArrayCount As Integer) As Integer
    End Function
    Public Shared Function _IBSU_CombineImageEx( _
            ByVal inImage1 As IBSU_ImageData, _
            ByVal inImage2 As IBSU_ImageData, _
            ByVal whichHand As IBSU_CombineImageWhichHand, _
            ByVal outImage As IntPtr, _
            ByVal pSegmentImageArray As IntPtr, _
            ByVal pSegmentPositionArray As IntPtr, _
            ByRef pSegmentImageArrayCount As Integer) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_CombineImageEx(inImage1, inImage2, whichHand, outImage, pSegmentImageArray, pSegmentPositionArray, pSegmentImageArrayCount)
        Catch except As Exception
            Trace.WriteLine("IBSU_CombineImageEx : " & except.Message)
        End Try

        Return nRc
    End Function

    '/*
    '****************************************************************************************************
    '* IBSU_BGetRollingInfo()
    '* 
    '* DESCRIPTION:
    '*     Get information about the status of the rolled print capture for a device. 
    '*
    '* ARGUMENTS:
    '*     handle         Device handle.
    '*     pRollingState  Pointer to variable that will receive rolling state.
    '*     pRollingLineX  Pointer to variable that will receive x-coordinate of current "rolling line" 
    '*                    for display as a guide.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '*/
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_BGetRollingInfo( _
            ByVal handle As Integer, _
            ByRef pRollingState As IBSU_RollingState, _
            ByRef pRollingLineX As Integer) As Integer
    End Function
    Public Shared Function _IBSU_BGetRollingInfo( _
            ByVal handle As Integer, _
            ByRef pRollingState As IBSU_RollingState, _
            ByRef pRollingLineX As Integer) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_BGetRollingInfo(handle, pRollingState, pRollingLineX)
        Catch except As Exception
            Trace.WriteLine("IBSU_BGetRollingInfo : " & except.Message)
        End Try

        Return nRc
    End Function

    '/*
    '****************************************************************************************************
    '* IBSU_BGetRollingInfoEx()
    '* 
    '* DESCRIPTION:
    '*     Get information about the status of the rolled print capture for a device. 
    '*
    '* ARGUMENTS:
    '*     handle         Device handle.
    '*     pRollingState  Pointer to variable that will receive rolling state.
    '*     pRollingLineX  Pointer to variable that will receive x-coordinate of current "rolling line" 
    '*                    for display as a guide.
    '*     pRollDirection Pointer to variable that will receive rolling direction
    '*                    0 : can't determin yet
    '*                    1 : left to right  --->
    '*                    2 : right to left  <---
    '*     pRollWidth     Pointer to vairable that will receive rolling width (mm)
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '*/
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_BGetRollingInfoEx( _
            ByVal handle As Integer, _
            ByRef pRollingState As IBSU_RollingState, _
            ByRef pRollingLineX As Integer, _
            ByRef pRollDirection As Integer, _
            ByRef pRollWidth As Integer) As Integer
    End Function
    Public Shared Function _IBSU_BGetRollingInfoEx( _
            ByVal handle As Integer, _
            ByRef pRollingState As IBSU_RollingState, _
            ByRef pRollingLineX As Integer, _
            ByRef pRollDirection As Integer, _
            ByRef pRollWidth As Integer) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_BGetRollingInfoEx(handle, pRollingState, pRollingLineX, pRollDirection, pRollWidth)
        Catch except As Exception
            Trace.WriteLine("IBSU_BGetRollingInfoEx : " & except.Message)
        End Try

        Return nRc
    End Function

    '/*
    '****************************************************************************************************
    '* IBSU_CheckWetFinger()
    '* 
    '* DESCRIPTION:
    '*     Check if the image is wet or not.
    '*
    '* ARGUMENTS:
    '*     handle                 Device handle.
    '*     inImage                Input image data which is returned from result callback.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See warning codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '*/
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_CheckWetFinger( _
            ByVal handle As Integer, _
            ByVal inImage As IBSU_ImageData) As Integer
    End Function
    Public Shared Function _IBSU_CheckWetFinger( _
            ByVal handle As Integer, _
            ByVal inImage As IBSU_ImageData) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_CheckWetFinger(handle, inImage)
        Catch except As Exception
            Trace.WriteLine("IBSU_CheckWetFinger : " & except.Message)
        End Try

        Return nRc
    End Function


    '/*
    '****************************************************************************************************
    '* IBSU_GetImageWidth()
    '* 
    '* DESCRIPTION:
    '*     Get the image width of input image by millimeter(mm).
    '*
    '* ARGUMENTS:
    '*     handle                 Device handle.
    '*     inImage                Input image data which is returned from result callback.
    '*     Width_MM				 Output millimeter (width) of Input image.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See warning codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '*/
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_GetImageWidth( _
            ByVal handle As Integer, _
            ByVal inImage As IBSU_ImageData, _
            ByRef Width_MM As Integer) As Integer
    End Function
    Public Shared Function _IBSU_GetImageWidth( _
            ByVal handle As Integer, _
            ByVal inImage As IBSU_ImageData, _
            ByRef Width_MM As Integer) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_GetImageWidth(handle, inImage, Width_MM)
        Catch except As Exception
            Trace.WriteLine("IBSU_GetImageWidth : " & except.Message)
        End Try

        Return nRc
    End Function

    '/*
    '****************************************************************************************************
    '* IBSU_IsWritableDirectory()
    '* 
    '* DESCRIPTION:
    '*     Check whether a directory is writable.
    '*
    '* ARGUMENTS:
    '*     dirpath                Directory path.
    '*     needCreateSubFolder	 Check whether need to create subfolder into the directory path.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if a directory is writable.
    '*     Error code < 0, otherwise.  See warning codes in 'IBScanUltimateApi_err'.
    '*         IBSU_ERR_CHANNEL_IO_WRITE_FAILED: Directory does not writable.
    '****************************************************************************************************
    '*/
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_IsWritableDirectory( _
            ByVal dirpath As String, _
            ByVal needCreateSubFolder As Boolean) As Integer
    End Function
    Public Shared Function _IBSU_IsWritableDirectory( _
            ByVal dirpath As String, _
            ByVal needCreateSubFolder As Boolean) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_IsWritableDirectory(dirpath, needCreateSubFolder)
        Catch except As Exception
            Trace.WriteLine("IBSU_IsWritableDirectory : " & except.Message)
        End Try

        Return nRc
    End Function

    '/*
    '****************************************************************************************************
    '* RESERVED_GetFinalImageByNative()
    '* 
    '* DESCRIPTION:
    '*     get a native image for the final capture.
    '*
    '* ARGUMENTS:
    '*     handle                 Device handle.
    '*     pReservedKey           Key to unlock reserved functionality.
    '*     finalImage             Pointer to structure that will receive data of final image by native
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
		'*     Error code < 0, otherwise.  See warning codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '*/
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function RESERVED_GetFinalImageByNative( _
            ByVal handle As Integer, _
            ByVal pReservedKey As String, _
            ByRef finalImage As IBSU_ImageData) As Integer
    End Function
    Public Shared Function _RESERVED_GetFinalImageByNative( _
            ByVal handle As Integer, _
            ByVal pReservedKey As String, _
            ByRef finalImage As IBSU_ImageData) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = RESERVED_GetFinalImageByNative(handle, pReservedKey, finalImage)
        Catch except As Exception
            Trace.WriteLine("RESERVED_GetFinalImageByNative : " & except.Message)
        End Try

        Return nRc
    End Function
    
    '/*
		'****************************************************************************************************
		'* IBSU_GenerateDisplayImage()
		'* 
		'* DESCRIPTION:
		'*     Generate scaled image in various formats for fast image display on canvas.
		'*     You can use instead of IBSU_GenerateZoomOutImageEx()
		'*
		'* ARGUMENTS:
		'*     inImage     Original grayscale image data.
		'*     inWidth     Width of input image.
		'*     in Height   Height of input image.
		'*     outImage    Pointer to buffer that will receive output image.  This buffer must hold at least
		'*                 'outWidth' x 'outHeight' x 'bitsPerPixel' bytes.
		'*     outWidth    Width of output image.
		'*     outHeight   Height of output image.
		'*     outBkColor     Background color of output image.
		'*     outFormat   IBSU_ImageFormat of output image.
		'*     outQualityLevel  Image quality of output image. The parameter must be in the range 0 through 2
		'*     outVerticalFlip  Enable/disable vertical flip of output image.
		'*
		'* RETURNS:
		'*     IBSU_STATUS_OK, if successful.
		'*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
		'****************************************************************************************************
		'*/
		<DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_GenerateDisplayImage( _
            ByVal pInImage As IntPtr, _
            ByVal inWidth As Integer, _
            ByVal inHeight As Integer, _
            ByVal outImage As IntPtr, _
            ByVal outWidth As Integer, _
            ByVal outHeight As Integer, _
            ByVal outBkColor As Byte, _
            ByVal outFormat As IBSU_ImageFormat, _
            ByVal outQualityLevel As Integer, _
            ByVal outVerticalFlip As Boolean) As Integer
    End Function
    Public Shared Function _IBSU_GenerateDisplayImage( _
            ByVal pInImage As IntPtr, _
            ByVal inWidth As Integer, _
            ByVal inHeight As Integer, _
            ByVal outImage As IntPtr, _
            ByVal outWidth As Integer, _
            ByVal outHeight As Integer, _
            ByVal outBkColor As Byte, _
            ByVal outFormat As IBSU_ImageFormat, _
            ByVal outQualityLevel As Integer, _
            ByVal outVerticalFlip As Boolean) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_GenerateDisplayImage(pInImage, inWidth, inHeight, outImage, outWidth, outHeight, _
                                            outBkColor, outFormat, outQualityLevel, outVerticalFlip)
        Catch except As Exception
            Trace.WriteLine("IBSU_GenerateDisplayImage : " & except.Message)
        End Try

        Return nRc
    End Function
    
    '/*
		'****************************************************************************************************
		'* IBSU_RemoveFingerImages()
		'* 
		'* DESCRIPTION:
		'*     Remove finger images.
		'* 
		'* ARGUMENTS:
		'*     handle              Handle for device associated with this event (if appropriate).
		'*     fIndex              Bit-pattern of finger index of input image. 
		'*                         ex) IBSU_FINGER_LEFT_LITTLE | IBSU_FINGER_LEFT_RING in IBScanUltimateApi_defs.h
		'*
		'* RETURNS:
		'*     IBSU_STATUS_OK, if successful.
		'*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
		'****************************************************************************************************
		'*/
		<DllImport("IBScanUltimate.DLL")> _
		Private Shared Function IBSU_RemoveFingerImage( _
            ByVal handle As Integer, _
            ByVal fIndex As UInteger)
		End Function
		Public Shared Function _IBSU_RemoveFingerImage( _
          ByVal handle As Integer, _
          ByVal fIndex As UInteger)
				Dim nRc As Integer = IBSU_STATUS_OK
				Try
						nRc = IBSU_RemoveFingerImage(handle, fIndex)
                                            
				Catch except As Exception
		  			Trace.WriteLine("IBSU_RemoveFingerImage : " & except.Message)
        End Try

        Return nRc
    End Function
    
    '/*
		'****************************************************************************************************
		'* IBSU_AddFingerImage()
		'* 
		'* DESCRIPTION:
		'*     Add an finger image for the fingerprint duplicate check and roll to slap comparison.
		'*     It can have only ten prints 
		'* 
		'* ARGUMENTS:
		'*     handle              Handle for device associated with this event (if appropriate).
		'*     image               input image data.
		'*     fIndex              Bit-pattern of finger index of input image. 
		'*                         ex) IBSU_FINGER_LEFT_LITTLE | IBSU_FINGER_LEFT_RING in IBScanUltimateApi_defs.h
		'*     imageType           Image type of input image.
		'*     flagForce           Indicates whether input image should be saved even if another image is already stord or not.  TRUE to be stored force; FALSE to 
		'*                         be not stored force.
		'*
		'* RETURNS:
		'*     IBSU_STATUS_OK, if successful.
		'*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
		'****************************************************************************************************
		'*/
		<DllImport("IBScanUltimate.DLL")> _
		Private Shared Function IBSU_AddFingerImage( _
          ByVal handle As Integer, _
		    		ByVal image As IBSU_ImageData, _
		    		ByVal fIndex As UInteger, _
		    		ByVal imageType As IBSU_ImageType, _
		    		ByVal flagForce As Boolean )
		End Function
		Public Shared Function _IBSU_AddFingerImage( _
          ByVal handle As Integer, _
		    		ByVal image As IBSU_ImageData, _
		    		ByVal fIndex As UInteger, _
		    		ByVal imageType As IBSU_ImageType, _
		    		ByVal flagForce As Boolean )
				Dim nRc As Integer = IBSU_STATUS_OK
				Try
						nRc = IBSU_AddFingerImage(handle, image, fIndex, imageType, flagForce)
                                            
				Catch except As Exception
		  			Trace.WriteLine("IBSU_AddFingerImage : " & except.Message)
        End Try

        Return nRc
    End Function
    
    '/*
		'****************************************************************************************************
		'* IBSU_IsFingerDuplicated()
		'* 
		'* DESCRIPTION:
		'*     Checks for the fingerprint duplicate from the stored prints by IBSU_AddFingerImage(). 
		'* 
		'* ARGUMENTS:
		'*     handle              Handle for device associated with this event (if appropriate).
		'*     image               input image data for the fingerprint duplicate check.
		'*     fIndex              Bit-pattern of finger index of input image. 
		'*                         ex) IBSU_FINGER_LEFT_LITTLE | IBSU_FINGER_LEFT_RING in IBScanUltimateApi_defs.h
		'*     imageType           Image type of input image.
		'*     securityLevel       security level for the duplicate checks.
		'*     pMatchedPosition    Pointer to variable that will receive result of duplicate.
		'*
		'* RETURNS:
		'*     IBSU_STATUS_OK, if successful.
		'*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
		'****************************************************************************************************
		'*/
		<DllImport("IBScanUltimate.DLL")> _
		Private Shared Function IBSU_IsFingerDuplicated( _
          ByVal handle As Integer, _
		    		ByVal image As IBSU_ImageData, _
		    		ByVal fIndex As UInteger, _
		    		ByVal imageType As IBSU_ImageType, _
		    		ByVal securityLevel As Integer, _
		    		ByRef pMatchedPosition As Integer ) As Integer
		End Function
		Public Shared Function _IBSU_IsFingerDuplicated( _
          ByVal handle As Integer, _
		    		ByVal image As IBSU_ImageData, _
		    		ByVal fIndex As UInteger, _
		    		ByVal imageType As IBSU_ImageType, _
		    		ByVal securityLevel As Integer, _
		    		ByRef pMatchedPosition As Integer ) As Integer
				Dim nRc As Integer = IBSU_STATUS_OK
				Try
						nRc = IBSU_IsFingerDuplicated(handle, image, fIndex, imageType, securityLevel, pMatchedPosition)
                                            
				Catch except As Exception
		  			Trace.WriteLine("IBSU_IsFingerDuplicated : " & except.Message)
        End Try

        Return nRc
    End Function
    
    '/*
		'****************************************************************************************************
		'* IBSU_IsValidFingerGeometry()
		'* 
		'* DESCRIPTION:
		'*     Check for hand and finger geometry whether it is correct or not. 
		'* 
		'* ARGUMENTS:
		'*     handle              Handle for device associated with this event (if appropriate).
		'*     image               input image data for roll to slap comparison.
		'*     fIndex              Bit-pattern of finger index of input image. 
		'*                         ex) IBSU_FINGER_LEFT_LITTLE | IBSU_FINGER_LEFT_RING in IBScanUltimateApi_defs.h
		'*     imageType           Image type of input image.
		'*     pValid              Pointer to variable that will receive whether it is valid or not.  TRUE to valid; FALSE to invalid.
		'*
		'* RETURNS:
		'*     IBSU_STATUS_OK, if successful.
		'*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
		'****************************************************************************************************
		'*/
		<DllImport("IBScanUltimate.DLL")> _
		Private Shared Function IBSU_IsValidFingerGeometry( _
          ByVal handle As Integer, _
		    		ByVal image As IBSU_ImageData, _
		    		ByVal fIndex As UInteger, _
		    		ByVal imageType As IBSU_ImageType, _
		    		ByRef pValid As Boolean ) As Integer
		End Function
		Public Shared Function _IBSU_IsValidFingerGeometry( _
          ByVal handle As Integer, _
		    		ByVal image As IBSU_ImageData, _
		    		ByVal fIndex As UInteger, _
		    		ByVal imageType As IBSU_ImageType, _
		    		ByRef pValid As Boolean ) As Integer
				Dim nRc As Integer = IBSU_STATUS_OK
				Try
						nRc = IBSU_IsValidFingerGeometry(handle, image, fIndex, imageType, pValid)
                                            
				Catch except As Exception
		  			Trace.WriteLine("IBSU_IsValidFingerGeometry : " & except.Message)
        End Try

        Return nRc
    End Function

    '/*
    '****************************************************************************************************
    '* IBSU_GetRequiredSDKVersion()
    '* 
    '* DESCRIPTION:
    '*     Get minimum SDK version required for running 
    '* 
    '* ARGUMENTS:
    '*     deviceIndex         Device index
    '*     minSDKVersion       Minimum SDK Version to be returned
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '*/
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_GetRequiredSDKVersion( _
          ByVal deviceIndex As Integer, _
          ByVal minSDKVersion As StringBuilder) As Integer
    End Function
    Public Shared Function _IBSU_GetRequiredSDKVersion( _
          ByVal deviceIndex As Integer, _
          ByVal minSDKVersion As StringBuilder) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_GetRequiredSDKVersion(deviceIndex, minSDKVersion)

        Catch except As Exception
            Trace.WriteLine("IBSU_GetRequiredSDKVersion : " & except.Message)
        End Try

        Return nRc
    End Function

		'/*
		'****************************************************************************************************
		'* IBSU_SetEncryptionKey()
		'* 
		'* DESCRIPTION:
		'*     Set encryption key and mode
		'* 
		'* ARGUMENTS:
		'*     handle              Handle for device associated with this event (if appropriate).
		'*     pEncyptionKey       input data for encryption key (should be 32 bytes).
		'*     encMode             input data for encryption mode. (defualt, random, custom)
		'*
		'* RETURNS:
		'*     IBSU_STATUS_OK, if successful.
		'*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
		'****************************************************************************************************
		'*/
		<DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_SetEncryptionKey( _
            ByVal handle As Integer, _
            ByVal pEncyptionKey As Integer, _
          ByVal encMode As IBSU_EncryptionMode) As Integer
    End Function
    Public Shared Function _IBSU_SetEncryptionKey( _
          ByVal handle As Integer, _
          ByVal pEncyptionKey As Integer, _
          ByVal encMode As IBSU_EncryptionMode) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_SetEncryptionKey(handle, pEncyptionKey, encMode)

        Catch except As Exception
            Trace.WriteLine("IBSU_SetEncryptionKey : " & except.Message)
        End Try

        Return nRc
    End Function

    '/*
    '****************************************************************************************************
    '* IBSU_SetCustomerKey()
    '* 
    '* DESCRIPTION:
    '*     Set CustomoerKey to use locked devices, This is must perfomed on locked devices before IBSU_OpenDevice.
    '* 
    '* ARGUMENTS:
    '*     deviceIndex         Device index
    '*     nHashType           Type of Hash
    '*     pCustomerKey        Customer Key to match lock info written in the locked device.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '*/
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_SetCustomerKey( _
          ByVal deviceIndex As Integer, _
          ByVal hashType As IBSU_HashType, _
          ByVal pCustomerKey As String) As Integer
    End Function
    Public Shared Function _IBSU_SetCustomerKey( _
          ByVal deviceIndex As Integer, _
          ByVal hashType As IBSU_HashType, _
          ByVal pCustomerKey As String) As Integer

        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_SetCustomerKey(deviceIndex, hashType, pCustomerKey)

        Catch except As Exception
            Trace.WriteLine("IBSU_SetCustomerKey : " & except.Message)
        End Try

        Return nRc
    End Function

    '/*
    '****************************************************************************************************
    '* IBSU_GetErrorString()
    '* 
    '* DESCRIPTION:
    '*     Returns a string description of the error code.
    '* 
    '* ARGUMENTS:
    '*     errorCode       error code
    '*     errorString     Buffer in which value of error string description will be stored.  This buffer should be 
    '*                     able to hold IBSU_MAX_STR_LEN characters.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '*/
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_GetErrorString( _
          ByVal errorCode As Integer, _
          ByVal errorString As StringBuilder) As Integer
    End Function
    Public Shared Function _IBSU_GetErrorString( _
          ByVal errorCode As Integer, _
          ByVal errorString As StringBuilder) As Integer

        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_GetErrorString(errorCode, errorString)

        Catch except As Exception
            Trace.WriteLine("IBSU_GetErrorString : " & except.Message)
        End Try

        Return nRc
    End Function

    '/*
    '****************************************************************************************************
    '* IBSU_IsSpoofFingerDetected()
    '* 
    '* DESCRIPTION:
    '*     Detect the finger print is Live of Fake.
    '* 
    '* ARGUMENTS:
    '*     handle          Handle for device associated with this event (if appropriate).
    '*     IBSU_ImageData  input image data.
    '*     
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '*/
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_IsSpoofFingerDetected( _
          ByVal handle As Integer, _
          ByVal image As IBSU_ImageData, _
          ByRef IsSpoof As Boolean) As Integer
    End Function
    Public Shared Function _IBSU_IsSpoofFingerDetected( _
         ByVal handle As Integer, _
          ByVal image As IBSU_ImageData, _
          ByRef IsSpoof As Boolean) As Integer

        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_IsSpoofFingerDetected(handle, image, IsSpoof)

        Catch except As Exception
            Trace.WriteLine("IBSU_IsSpoofFingerDetected : " & except.Message)
        End Try

        Return nRc
    End Function

    '/*
    '****************************************************************************************************
    '* RESERVED_GetSpoofScore()
    '* 
    '* DESCRIPTION:
    '*     get a spoof score for the finger.
    '*
    '* ARGUMENTS:
    '*     pReservedKey           Key to unlock reserved functionality.
    '*     pImage                 Pointer to fingerprint image
    '*     Width                  Width of pImage
    '*     Height                 Height of pImage
    '*     pScore                 Pointer to return spoof score (the score range is from 0 to 1000)
    '*                            The closer to 1000 score, it means Live finger.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '*/
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function RESERVED_GetSpoofScore( _
               ByVal pReservedKey As String, _
          ByVal pImage As IntPtr, _
          ByVal Width As Integer, _
       ByVal Height As Integer, _
          ByRef pScore As Integer) As Integer
    End Function
    Public Shared Function _RESERVED_GetSpoofScore( _
               ByVal pReservedKey As String, _
          ByVal pImage As IntPtr, _
          ByVal Width As Integer, _
       ByVal Height As Integer, _
          ByRef pScore As Integer) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = RESERVED_GetSpoofScore(pReservedKey, pImage, Width, Height, pScore)

        Catch except As Exception
            Trace.WriteLine("RESERVED_GetSpoofScore : " & except.Message)
        End Try

        Return nRc
    End Function


    '****************************************************************************************************
    '* IBSU_ConvertImageToISOANSI()
    '* 
    '* DESCRIPTION:
    '*     Convert Image Data to Standard Format for write file.
    '*     (ISO 19794-2:2005, ISO 19794-4:2005, ISO 19794-2:2011, ISO 19794-4:2011, ANSI/INCITS 378:2004, ANSI/INCITS 381:2004)
    '* 
    '* ARGUMENTS:
    '*     handle              Handle for device associated with this event (if appropriate).
    '*     image               input image data for roll to slap comparison.
    '*     imageCount          Number of image.
    '*     imageFormat         Image compression format of output data
    '*     STDformat           ISO format of output data
    '*     pdata               Pointer to output data
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '*/
    <DllImport("IBScanUltimate.DLL")> _
    Private Shared Function IBSU_ConvertImageToISOANSI( _
        ByVal handle As Integer, _
        ByVal image As IBSM_ImageData(), _
        ByVal imageCount As Integer, _
        ByVal imageFormat As IBSM_ImageFormat, _
        ByVal STDformat As IBSM_StandardFormat, _
        ByRef pdata As IBSM_StandardFormatData) As Integer
    End Function
    Public Shared Function _IBSU_ConvertImageToISOANSI( _
        ByVal handle As Integer, _
        ByVal image As IBSM_ImageData(), _
        ByVal imageCount As Integer, _
        ByVal imageFormat As IBSM_ImageFormat, _
        ByVal STDformat As IBSM_StandardFormat, _
        ByRef pdata As IBSM_StandardFormatData) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = IBSU_ConvertImageToISOANSI(handle, image, imageCount, imageFormat, STDformat, pdata)

        Catch except As Exception
            Trace.WriteLine("IBSU_ConvertImageToISOANSI : " & except.Message)
        End Try

        Return nRc
    End Function
    '/*
    '****************************************************************************************************
    '* RESERVED_GetEncryptedImage()
    '* 
    '* DESCRIPTION:
    '*     Get encrypted image from a device, without using capture thread.
    '*
    '* ARGUMENTS:
    '*     handle        Device handle.
    '*     pReservedKey  Key to unlock reserved functionality.
    '*     pEncKey       Key to be encrypted.
    '*     pEncRawImage  Pointer to structure that will receive raw encrypted image data.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '*/
    <DllImport("IBScanUltimate.DLL")> _
      Private Shared Function RESERVED_GetEncryptedImage( _
          ByVal handle As Integer, _
          ByVal pReservedKey As String, _
          ByVal pEncyptionKey As Integer, _
          ByRef pRawEncImage As IBSU_ImageData) As Integer
    End Function
    Public Shared Function _RESERVED_GetEncryptedImage( _
        ByVal handle As Integer, _
        ByVal pReservedKey As String, _
        ByVal pEncyptionKey As Integer, _
        ByRef pRawEncImage As IBSU_ImageData) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = RESERVED_GetEncryptedImage(handle, pReservedKey, pEncyptionKey, pRawEncImage)

        Catch except As Exception
            Trace.WriteLine("RESERVED_GetEncryptedImage : " & except.Message)
        End Try

        Return nRc
    End Function

    '/*
    '****************************************************************************************************
    '* RESERVED_ConvertToDecryptImage()
    '* 
    '* DESCRIPTION:
    '*     Convert to decrypted image from an encrypted image.
    '*
    '* ARGUMENTS:
    '*     pReservedKey  Key to unlock reserved functionality.
    '*     pEncKey       Key to be encrypted.
    '*     pEncRawImage  Pointer to structure that was received raw encrypted image data.
    '*     pDecRawImage  Decrypted image from an encrypted image.
    '*
    '* RETURNS:
    '*     IBSU_STATUS_OK, if successful.
    '*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    '****************************************************************************************************
    '*/
    <DllImport("IBScanUltimate.DLL")> _
      Private Shared Function RESERVED_ConvertToDecryptImage( _
          ByVal pReservedKey As String, _
          ByVal pEncyptionKey As Integer, _
          ByVal rawEncImage As IBSU_ImageData, _
          ByRef pRawDecImage As IBSU_ImageData) As Integer
    End Function
    Public Shared Function _RESERVED_ConvertToDecryptImage( _
        ByVal pReservedKey As String, _
        ByVal pEncyptionKey As Integer, _
        ByVal rawEncImage As IBSU_ImageData, _
        ByRef pRawDecImage As IBSU_ImageData) As Integer
        Dim nRc As Integer = IBSU_STATUS_OK
        Try
            nRc = RESERVED_ConvertToDecryptImage(pReservedKey, pEncyptionKey, rawEncImage, pRawDecImage)

        Catch except As Exception
            Trace.WriteLine("RESERVED_ConvertToDecryptImage : " & except.Message)
        End Try

        Return nRc
    End Function

End Class
