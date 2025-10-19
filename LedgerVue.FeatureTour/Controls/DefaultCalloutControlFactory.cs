// Copyright (c) ML Labs. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System.Windows;

namespace LedgerVue.FeatureTouring.Controls
{
    /// <summary>
    /// Default factory implementation that creates the standard TourControl callout.
    /// This is the built-in implementation that requires no external dependencies.
    /// </summary>
    public class DefaultCalloutControlFactory : ICalloutControlFactory
    {
        /// <summary>
        /// Creates a new instance of the default TourControl.
        /// </summary>
        /// <returns>A TourControl instance</returns>
        public FrameworkElement CreateCalloutControl()
        {
            return new TourControl();
        }

        /// <summary>
        /// Gets the name of this implementation.
        /// </summary>
        public string ImplementationName => "Default TourControl";
    }
}

