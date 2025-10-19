// Copyright (c) ML Labs. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using LedgerVue.FeatureTouring.Controls;

namespace LedgerVue.FeatureTouring
{
    /// <summary>
    /// Configuration settings for the FeatureTour system.
    /// </summary>
    public static class TourConfiguration
    {
        private static ICalloutControlFactory calloutFactory = new DefaultCalloutControlFactory();

        /// <summary>
        /// Gets or sets the factory used to create callout controls for feature tours.
        /// Default is <see cref="DefaultCalloutControlFactory"/>.
        /// Set this before starting any tours to use a different implementation (e.g., TelerikCalloutControlFactory).
        /// </summary>
        public static ICalloutControlFactory CalloutControlFactory
        {
            get => calloutFactory;
            set => calloutFactory = value ?? new DefaultCalloutControlFactory();
        }

        /// <summary>
        /// Resets the configuration to default values.
        /// </summary>
        public static void Reset()
        {
            calloutFactory = new DefaultCalloutControlFactory();
        }

        /// <summary>
        /// Convenience method to switch to the default callout implementation.
        /// </summary>
        public static void UseDefaultCallout()
        {
            CalloutControlFactory = new DefaultCalloutControlFactory();
        }

        /// <summary>
        /// Convenience method to switch to Telerik RadCallout implementation.
        /// Requires Telerik UI for WPF to be installed and the project built with TELERIK_AVAILABLE symbol.
        /// </summary>
        public static void UseTelerikCallout()
        {
            CalloutControlFactory = new TelerikCalloutControlFactory();
        }
    }
}

