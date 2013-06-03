﻿using MediaBrowser.Controller.Drawing;
using MediaBrowser.Controller.MediaInfo;
using MediaBrowser.Controller.Weather;
using System.Collections.Generic;

namespace MediaBrowser.Controller
{
    /// <summary>
    /// Class Kernel
    /// </summary>
    public class Kernel 
    {
        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static Kernel Instance { get; private set; }

        /// <summary>
        /// Gets the image manager.
        /// </summary>
        /// <value>The image manager.</value>
        public ImageManager ImageManager { get; set; }

        /// <summary>
        /// Gets the FFMPEG controller.
        /// </summary>
        /// <value>The FFMPEG controller.</value>
        public FFMpegManager FFMpegManager { get; set; }

        /// <summary>
        /// Gets the list of currently registered weather prvoiders
        /// </summary>
        /// <value>The weather providers.</value>
        public IEnumerable<IWeatherProvider> WeatherProviders { get; set; }

        /// <summary>
        /// Creates a kernel based on a Data path, which is akin to our current programdata path
        /// </summary>
        public Kernel()
        {
            Instance = this;
        }
    }
}
