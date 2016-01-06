﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureCms.Core.Plugins
{
    /// <summary>
    /// Interface denoting plug-in attributes that are displayed throughout 
    /// the editing interface.
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// Gets or sets the plugin descriptor
        /// </summary>
        //PluginDescriptor PluginDescriptor { get; }

        /// <summary>
        /// Install plugin
        /// </summary>
        void Install();

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        void Uninstall();

    }
}