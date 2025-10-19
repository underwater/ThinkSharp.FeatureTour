// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System.Windows;

namespace ThinkSharp.FeatureTouring.Controls
{
    /// <summary>
    /// Factory interface for creating callout controls used in feature tours.
    /// Allows switching between different callout implementations (default, Telerik, etc.)
    /// </summary>
    public interface ICalloutControlFactory
    {
        /// <summary>
        /// Creates a new callout control instance.
        /// </summary>
        /// <returns>A FrameworkElement that will be used as the popup content</returns>
        FrameworkElement CreateCalloutControl();

        /// <summary>
        /// Gets the name of this callout implementation.
        /// </summary>
        string ImplementationName { get; }
    }
}

