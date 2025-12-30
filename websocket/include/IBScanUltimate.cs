/*
****************************************************************************************************
* IBScanUltimate.cs
*
* DESCRIPTION:
*     Definition of image data structures for IBScanUltimate.
*     http://www.integratedbiometrics.com
*
* NOTES:
*     Copyright (c) Integrated Biometrics, 2009-2015
*
* HISTORY:
*     2012/04/12  Created.
*     2014/07/23  Reformatted.
*     2015/04/07  Added enumeration value to IBSU_ImageData.
*                     (ProcessThres)
****************************************************************************************************
*/


using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace IBscanUltimate
{
    public partial class DLL
    {
        /*
        ****************************************************************************************************
        * GLOBAL TYPES
        ****************************************************************************************************
        */

        /*
        ****************************************************************************************************
        * IBSU_ImageFormat
        *
        * DESCRIPTION:
        *     Enumeration of image formats.
        ****************************************************************************************************
        */
        public enum IBSU_ImageFormat
        {
            IBSU_IMG_FORMAT_GRAY,                                    /* Gray scale image. */
            IBSU_IMG_FORMAT_RGB24,                                   /* 24 bit RGB color image. */
            IBSU_IMG_FORMAT_RGB32,                                   /* True color RGB image. */
            IBSU_IMG_FORMAT_UNKNOWN                                  /* Unknown format. */
        }

        /*
        ****************************************************************************************************
        * IBSU_ImageData
        *
        * DESCRIPTION:
        *     Container for image data and metadata.
        ****************************************************************************************************
        */
        public struct IBSU_ImageData
        {
            /* Pointer to image buffer.  If this structure is supplied by a callback function, this pointer 
             * must not be retained; the data should be copied to an application buffer for any processing
             * after the callback returns. */
            public IntPtr Buffer;

            /* Image horizontal size (in pixels). */
            public uint Width;

            /* Image vertical size (in pixels). */
            public uint Height;

            /* Horizontal image resolution (in pixels/inch). */
            public double ResolutionX;

            /* Vertical image resolution (in pixels/inch). */
            public double ResolutionY;

            /* Image acquisition time, excluding processing time (in seconds). */
            public double FrameTime;

            /* Image line pitch (in bytes).  A positive value indicates top-down line order; a negative 
             * value indicates bottom-up line order. */
            public int Pitch;

            /* Number of bits per pixel. */
            public byte BitsPerPixel;

            /* Image color format. */
            public IBSU_ImageFormat Format;

            /* Marks image as the final processed result from the capture.  If this is FALSE, the image is
             * a preview image or a preliminary result. */
            public bool IsFinal;

            /* Threshold of image processing. */
            public uint ProcessThres;
        }
    }
}
