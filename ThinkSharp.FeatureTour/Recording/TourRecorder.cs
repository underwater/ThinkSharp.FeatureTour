// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Input;
using System.Windows.Media;
using ThinkSharp.FeatureTouring.Models;

namespace ThinkSharp.FeatureTouring.Recording
{
    /// <summary>
    /// Records user interactions to automatically generate tour definitions.
    /// </summary>
    public class TourRecorder
    {
        private readonly List<RecordedStep> recordedSteps = new List<RecordedStep>();
        private bool isRecording;
        private Window targetWindow;
        private Window overlayWindow;

        /// <summary>
        /// Starts recording mode.
        /// </summary>
        /// <param name="window">The window to record interactions on</param>
        public void StartRecording(Window window)
        {
            if (isRecording)
                throw new InvalidOperationException("Already recording");

            targetWindow = window ?? throw new ArgumentNullException(nameof(window));
            recordedSteps.Clear();
            isRecording = true;

            // Create overlay to capture clicks
            CreateOverlay();

            OnRecordingStarted?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Stops recording and returns the recorded steps.
        /// </summary>
        public IReadOnlyList<RecordedStep> StopRecording()
        {
            if (!isRecording)
                throw new InvalidOperationException("Not currently recording");

            isRecording = false;
            CloseOverlay();

            OnRecordingStopped?.Invoke(this, EventArgs.Empty);

            return recordedSteps.AsReadOnly();
        }

        /// <summary>
        /// Generates C# code for the recorded tour.
        /// </summary>
        public string GenerateTourCode(string tourName, string tourId)
        {
            var sb = new StringBuilder();
            
            sb.AppendLine($"public class {MakeSafeIdentifier(tourName)}Tour : ITourDefinition");
            sb.AppendLine("{");
            sb.AppendLine($"    public string TourId => \"{tourId}\";");
            sb.AppendLine($"    public string TourName => \"{tourName}\";");
            sb.AppendLine($"    public string Description => \"TODO: Add description\";");
            sb.AppendLine($"    public int DisplayOrder => 1;");
            sb.AppendLine();
            sb.AppendLine("    public Tour CreateTour()");
            sb.AppendLine("    {");
            sb.AppendLine("        return new Tour");
            sb.AppendLine("        {");
            sb.AppendLine($"            Name = \"{tourName}\",");
            sb.AppendLine("            ShowNextButtonDefault = true,");
            sb.AppendLine("            Steps = new[]");
            sb.AppendLine("            {");

            foreach (var step in recordedSteps)
            {
                var headerEscaped = EscapeString(step.Header);
                var contentEscaped = EscapeString(step.Content);
                
                sb.AppendLine($"                new Step(\"{step.ElementId}\", ");
                sb.AppendLine($"                    \"{headerEscaped}\", ");
                sb.AppendLine($"                    \"{contentEscaped}\"),");
                sb.AppendLine();
            }

            sb.AppendLine("            }");
            sb.AppendLine("        };");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }

        /// <summary>
        /// Generates JSON representation of the recorded tour.
        /// </summary>
        public string GenerateTourJson(string tourName, string tourId)
        {
            var sb = new StringBuilder();
            
            sb.AppendLine("{");
            sb.AppendLine($"  \"tourId\": \"{tourId}\",");
            sb.AppendLine($"  \"tourName\": \"{EscapeJsonString(tourName)}\",");
            sb.AppendLine($"  \"showNextButtonDefault\": true,");
            sb.AppendLine($"  \"steps\": [");

            for (int i = 0; i < recordedSteps.Count; i++)
            {
                var step = recordedSteps[i];
                var comma = i < recordedSteps.Count - 1 ? "," : "";
                
                sb.AppendLine("    {");
                sb.AppendLine($"      \"elementId\": \"{EscapeJsonString(step.ElementId)}\",");
                sb.AppendLine($"      \"header\": \"{EscapeJsonString(step.Header)}\",");
                sb.AppendLine($"      \"content\": \"{EscapeJsonString(step.Content)}\",");
                sb.AppendLine($"      \"placement\": \"{step.Placement}\"");
                sb.AppendLine($"    }}{comma}");
            }

            sb.AppendLine("  ]");
            sb.AppendLine("}");

            return sb.ToString();
        }

        /// <summary>
        /// Event raised when recording starts.
        /// </summary>
        public event EventHandler OnRecordingStarted;

        /// <summary>
        /// Event raised when recording stops.
        /// </summary>
        public event EventHandler OnRecordingStopped;

        /// <summary>
        /// Event raised when a new step is recorded.
        /// </summary>
        public event EventHandler<RecordedStep> OnStepRecorded;

        private void CreateOverlay()
        {
            overlayWindow = new Window
            {
                WindowStyle = WindowStyle.None,
                AllowsTransparency = true,
                Background = new SolidColorBrush(Color.FromArgb(30, 255, 0, 0)),
                ShowInTaskbar = false,
                Topmost = true,
                Owner = targetWindow,
                WindowState = WindowState.Maximized
            };

            var textBlock = new System.Windows.Controls.TextBlock
            {
                Text = "🎥 RECORDING MODE - Click on any element to add a tour step\nPress ESC to finish recording",
                FontSize = 18,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.White,
                Background = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0)),
                Padding = new Thickness(20),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 50, 0, 0)
            };

            var grid = new System.Windows.Controls.Grid();
            grid.Children.Add(textBlock);
            overlayWindow.Content = grid;

            overlayWindow.MouseDown += Overlay_MouseDown;
            overlayWindow.KeyDown += Overlay_KeyDown;

            overlayWindow.Show();
        }

        private void CloseOverlay()
        {
            if (overlayWindow != null)
            {
                overlayWindow.MouseDown -= Overlay_MouseDown;
                overlayWindow.KeyDown -= Overlay_KeyDown;
                overlayWindow.Close();
                overlayWindow = null;
            }
        }

        private void Overlay_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                StopRecording();
            }
        }

        private void Overlay_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Get the position relative to the target window
            var position = e.GetPosition(targetWindow);

            // Find the element at this position
            var element = FindElementAtPosition(targetWindow, position);

            if (element != null)
            {
                RecordElement(element);
            }
        }

        private UIElement FindElementAtPosition(Visual reference, Point position)
        {
            var hitTestResult = VisualTreeHelper.HitTest(reference, position);
            return hitTestResult?.VisualHit as UIElement;
        }

        private void RecordElement(UIElement element)
        {
            // Get or create ElementID
            var elementId = TourHelper.GetElementID(element);
            var automationId = AutomationProperties.GetAutomationId(element);
            var name = (element as FrameworkElement)?.Name;

            if (string.IsNullOrEmpty(elementId))
            {
                elementId = !string.IsNullOrEmpty(automationId) ? automationId :
                           !string.IsNullOrEmpty(name) ? name :
                           $"{element.GetType().Name}_{recordedSteps.Count + 1}";
            }

            var placement = TourHelper.GetPlacement(element);

            // Show input dialog for step details
            var inputDialog = new RecordStepDialog
            {
                ElementId = elementId,
                ElementType = element.GetType().Name,
                Owner = overlayWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            overlayWindow.Hide();
            var result = inputDialog.ShowDialog();
            overlayWindow.Show();

            if (result == true)
            {
                var step = new RecordedStep
                {
                    ElementId = inputDialog.ElementId,
                    Header = inputDialog.StepHeader,
                    Content = inputDialog.StepContent,
                    Placement = placement,
                    ElementType = element.GetType().Name,
                    AutomationId = automationId,
                    ControlName = name
                };

                recordedSteps.Add(step);
                OnStepRecorded?.Invoke(this, step);
            }
        }

        private string MakeSafeIdentifier(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "UnnamedTour";

            var sb = new StringBuilder();
            foreach (var c in input)
            {
                if (char.IsLetterOrDigit(c))
                    sb.Append(c);
            }

            var result = sb.ToString();
            return string.IsNullOrEmpty(result) ? "UnnamedTour" : result;
        }

        private string EscapeString(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            return input.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "");
        }

        private string EscapeJsonString(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            return input.Replace("\\", "\\\\")
                       .Replace("\"", "\\\"")
                       .Replace("\n", "\\n")
                       .Replace("\r", "\\r")
                       .Replace("\t", "\\t");
        }
    }

    /// <summary>
    /// Represents a recorded tour step.
    /// </summary>
    public class RecordedStep
    {
        public string ElementId { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }
        public Placement Placement { get; set; }
        public string ElementType { get; set; }
        public string AutomationId { get; set; }
        public string ControlName { get; set; }
    }
}

