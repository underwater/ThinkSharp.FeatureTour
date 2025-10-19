// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#if TELERIK_AVAILABLE
using System.Windows;
using Telerik.Windows.Controls;

namespace ThinkSharp.FeatureTouring.Controls
{
    /// <summary>
    /// Factory implementation that creates Telerik RadCallout controls.
    /// This requires Telerik UI for WPF to be installed and licensed.
    /// Only available when building with TELERIK_AVAILABLE compilation symbol.
    /// </summary>
    public class TelerikCalloutControlFactory : ICalloutControlFactory
    {
        /// <summary>
        /// Creates a new instance of a control wrapper around RadCallout.
        /// </summary>
        /// <returns>A TelerikCalloutWrapper instance containing RadCallout</returns>
        public FrameworkElement CreateCalloutControl()
        {
            return new TelerikCalloutWrapper();
        }

        /// <summary>
        /// Gets the name of this implementation.
        /// </summary>
        public string ImplementationName => "Telerik RadCallout";
    }

    /// <summary>
    /// Wrapper control that adapts RadCallout to work with the FeatureTour system.
    /// </summary>
    internal class TelerikCalloutWrapper : RadCallout
    {
        static TelerikCalloutWrapper()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TelerikCalloutWrapper), 
                new FrameworkPropertyMetadata(typeof(TelerikCalloutWrapper)));
        }

        public TelerikCalloutWrapper()
        {
            // Configure RadCallout for feature tour usage
            ArrowType = CalloutArrowType.Triangle;
            CalloutType = CalloutType.RoundedRectangle;
            
            // Make sure it inherits DataContext for binding to TourViewModel
            SetBinding(DataContextProperty, new System.Windows.Data.Binding());
        }
    }
}
#else
// Telerik not available - create a placeholder that tells users how to enable it
using System.Windows;

namespace ThinkSharp.FeatureTouring.Controls
{
    /// <summary>
    /// Placeholder factory when Telerik is not available.
    /// To enable Telerik support, build with TELERIK_AVAILABLE symbol and ensure Telerik assemblies are referenced.
    /// </summary>
    public class TelerikCalloutControlFactory : ICalloutControlFactory
    {
        /// <summary>
        /// Throws an exception indicating Telerik is not available.
        /// </summary>
        public FrameworkElement CreateCalloutControl()
        {
            throw new System.InvalidOperationException(
                "Telerik RadCallout is not available. " +
                "To use Telerik callouts, you must:\n" +
                "1. Install Telerik UI for WPF\n" +
                "2. Add a reference to Telerik assemblies\n" +
                "3. Build with TELERIK_AVAILABLE compilation symbol\n" +
                "4. Set UseTelerik property to 'true' in your project file or use /p:UseTelerik=true when building");
        }

        /// <summary>
        /// Gets the name of this implementation.
        /// </summary>
        public string ImplementationName => "Telerik RadCallout (Not Available)";
    }
}
#endif

