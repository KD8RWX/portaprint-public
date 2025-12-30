'/*
'****************************************************************************************************
'* IBScanUltimate.vb
'*
'* DESCRIPTION:
'*     Definition of image data structures for IBScanUltimate.
'*     http://www.integratedbiometrics.com
'*
'* NOTES:
'*     Copyright (c) Integrated Biometrics, 2009-2015
'*
'* HISTORY:
'*     2012/04/12  Created.
'*     2014/07/23  Reformatted.
'*     2015/04/07  Added enumeration value to IBSU_ImageData.
'*                     (ProcessThres)
'****************************************************************************************************
'*/

Partial Public Class DLL
    '/*
    '****************************************************************************************************
    '* GLOBAL TYPES
    '****************************************************************************************************
    '*/

    '/*
    '****************************************************************************************************
    '* IBSU_ImageFormat
    '*
    '* DESCRIPTION:
    '*     Enumeration of image formats.
    '****************************************************************************************************
    '*/
    Public Enum IBSU_ImageFormat
        IBSU_IMG_FORMAT_GRAY                    '/* Gray-scale image. */
        IBSU_IMG_FORMAT_RGB24                   '/* 24-bit color image. */
        IBSU_IMG_FORMAT_RGB32                   '/* True-color RGB image. */
        IBSU_IMG_FORMAT_UNKNOWN                 '/* Unknown format. */
    End Enum

    '/*
    '****************************************************************************************************
    '* IBSU_ImageData
    '*
    '* DESCRIPTION:
    '*     Container for image data and metadata.
    '****************************************************************************************************
    '*/
    Public Structure IBSU_ImageData
        '/* Pointer to image buffer.  If this structure is supplied by a callback function, this pointer 
        ' * must not be retained; the data should be copied to an application buffer for any processing
        ' * after the callback returns. */
        Public Buffer As IntPtr

        '/* Image horizontal size (in pixels). */
        Public Width As UInteger

        '/* Image vertical size (in pixels). */
        Public Height As UInteger

        '/* Horizontal image resolution (in pixels/inch). */
        Public ResolutionX As Double

        '/* Vertical image resolution (in pixels/inch). */
        Public ResolutionY As Double

        '/* Image acquisition time, excluding processing time (in seconds). */
        Public FrameTime As Double

        '/* Image line pitch (in bytes).  A positive value indicates top-down line order; a negative 
        ' * value indicates bottom-up line order. */
        Public Pitch As Integer

        '/* Number of bits per pixel. */
        Public BitsPerPixel As Byte

        '/* Image color format. */
        Public Format As IBSU_ImageFormat

        '/* Marks image as the final processed result from the capture.  If this is FALSE, the image is
        ' * a preview image or a preliminary result. */
        Public IsFinal As Boolean

        '/* Threshold of image processing. */
        Public ProcessThres As UInteger
    End Structure
End Class
