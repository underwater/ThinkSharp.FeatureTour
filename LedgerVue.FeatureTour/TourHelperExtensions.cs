// Copyright (c) ML Labs. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Media;

namespace LedgerVue.FeatureTouring
{
    /// <summary>
    /// Extension methods for TourHelper to support alternative identification methods.
    /// </summary>
    public static class TourHelperExtensions
    {
        /// <summary>
        /// Automatically sets ElementID based on the AutomationProperties.AutomationId if ElementID is not set.
        /// This allows using AutomationId as an alternative to ElementID.
        /// </summary>
        public static void SyncAutomationIdToElementId(this UIElement element)
        {
            if (element == null)
                return;

            var elementId = TourHelper.GetElementID(element);
            var automationId = AutomationProperties.GetAutomationId(element);

            // If ElementID is not set but AutomationId is, use AutomationId as ElementID
            if (string.IsNullOrEmpty(elementId) && !string.IsNullOrEmpty(automationId))
            {
                TourHelper.SetElementID(element, automationId);
            }
        }

        /// <summary>
        /// Finds an element by AutomationId in the visual tree.
        /// </summary>
        public static UIElement FindElementByAutomationId(this DependencyObject root, string automationId)
        {
            if (root == null || string.IsNullOrEmpty(automationId))
                return null;

            if (root is UIElement element)
            {
                var id = AutomationProperties.GetAutomationId(element);
                if (id == automationId)
                    return element;
            }

            var childCount = VisualTreeHelper.GetChildrenCount(root);
            for (int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(root, i);
                var result = FindElementByAutomationId(child, automationId);
                if (result != null)
                    return result;
            }

            return null;
        }

        /// <summary>
        /// Scans the entire visual tree and automatically sets ElementID for all elements
        /// that have an AutomationId but no ElementID.
        /// Call this once during application startup.
        /// </summary>
        public static void EnableAutomationIdSupport(this Window window)
        {
            if (window == null)
                throw new ArgumentNullException(nameof(window));

            SyncAutomationIdsRecursive(window);
            
            // Re-sync when new controls are loaded
            window.Loaded += (s, e) => SyncAutomationIdsRecursive(window);
        }

        private static void SyncAutomationIdsRecursive(DependencyObject element)
        {
            if (element is UIElement uiElement)
            {
                uiElement.SyncAutomationIdToElementId();
            }

            var childCount = VisualTreeHelper.GetChildrenCount(element);
            for (int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(element, i);
                SyncAutomationIdsRecursive(child);
            }
        }
    }
}

