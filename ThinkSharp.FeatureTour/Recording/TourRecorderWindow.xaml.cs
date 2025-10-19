// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace ThinkSharp.FeatureTouring.Recording
{
    /// <summary>
    /// Main window for the Tour Recorder with full editing capabilities.
    /// </summary>
    public partial class TourRecorderWindow : Window
    {
        private readonly TourRecorder recorder;
        private readonly Window targetWindow;
        private readonly ObservableCollection<RecordedStepViewModel> steps;

        public TourRecorderWindow(Window targetWindow)
        {
            InitializeComponent();
            
            this.targetWindow = targetWindow ?? throw new ArgumentNullException(nameof(targetWindow));
            this.recorder = new TourRecorder();
            this.steps = new ObservableCollection<RecordedStepViewModel>();
            
            StepsList.ItemsSource = steps;
            
            // Setup recorder events
            recorder.OnStepRecorded += Recorder_OnStepRecorded;
            recorder.OnRecordingStopped += Recorder_OnRecordingStopped;
            
            UpdateEmptyState();
        }

        private void BtnStartRecording_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                recorder.StartRecording(targetWindow);
                
                BtnStartRecording.IsEnabled = false;
                BtnStopRecording.IsEnabled = true;
                StatusText.Text = "🔴 RECORDING - Click on any UI element to add a step (Press ESC to stop)";
                
                // Minimize this window while recording
                this.WindowState = WindowState.Minimized;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting recording: {ex.Message}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnStopRecording_Click(object sender, RoutedEventArgs e)
        {
            StopRecording();
        }

        private void StopRecording()
        {
            if (recorder != null && BtnStopRecording.IsEnabled)
            {
                var recordedSteps = recorder.StopRecording();
                
                BtnStartRecording.IsEnabled = true;
                BtnStopRecording.IsEnabled = false;
                StatusText.Text = $"Recording stopped. {steps.Count} step(s) recorded.";
                
                // Restore window
                this.WindowState = WindowState.Normal;
                this.Activate();
            }
        }

        private void Recorder_OnStepRecorded(object sender, RecordedStep step)
        {
            // Add to our list
            Dispatcher.Invoke(() =>
            {
                var stepVm = new RecordedStepViewModel(step)
                {
                    StepNumber = steps.Count + 1
                };
                steps.Add(stepVm);
                UpdateEmptyState();
                UpdateStepNumbers();
            });
        }

        private void Recorder_OnRecordingStopped(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                BtnStartRecording.IsEnabled = true;
                BtnStopRecording.IsEnabled = false;
                StatusText.Text = $"Recording stopped. {steps.Count} step(s) recorded.";
                
                this.WindowState = WindowState.Normal;
                this.Activate();
            });
        }

        private void StepsList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var hasSelection = StepsList.SelectedItem != null;
            BtnEditStep.IsEnabled = hasSelection;
            BtnDeleteStep.IsEnabled = hasSelection;
            BtnMoveUp.IsEnabled = hasSelection && StepsList.SelectedIndex > 0;
            BtnMoveDown.IsEnabled = hasSelection && StepsList.SelectedIndex < steps.Count - 1;
        }

        private void BtnAddStep_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new RecordStepDialog
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                ElementId = $"Element{steps.Count + 1}",
                ElementType = "Manual"
            };

            if (dialog.ShowDialog() == true)
            {
                var step = new RecordedStep
                {
                    ElementId = dialog.ElementId,
                    Header = dialog.StepHeader,
                    Content = dialog.StepContent,
                    Placement = Placement.TopCenter,
                    ElementType = "Manual"
                };

                var stepVm = new RecordedStepViewModel(step)
                {
                    StepNumber = steps.Count + 1
                };
                
                steps.Add(stepVm);
                UpdateEmptyState();
                UpdateStepNumbers();
            }
        }

        private void BtnEditStep_Click(object sender, RoutedEventArgs e)
        {
            if (StepsList.SelectedItem is RecordedStepViewModel selectedStep)
            {
                var dialog = new RecordStepDialog
                {
                    Owner = this,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    ElementId = selectedStep.ElementId,
                    StepHeader = selectedStep.Header,
                    StepContent = selectedStep.Content,
                    ElementType = selectedStep.ElementType
                };

                if (dialog.ShowDialog() == true)
                {
                    selectedStep.ElementId = dialog.ElementId;
                    selectedStep.Header = dialog.StepHeader;
                    selectedStep.Content = dialog.StepContent;
                    
                    // Refresh the list
                    StepsList.Items.Refresh();
                }
            }
        }

        private void BtnDeleteStep_Click(object sender, RoutedEventArgs e)
        {
            if (StepsList.SelectedItem is RecordedStepViewModel selectedStep)
            {
                var result = MessageBox.Show(
                    $"Delete step '{selectedStep.Header}'?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    steps.Remove(selectedStep);
                    UpdateEmptyState();
                    UpdateStepNumbers();
                }
            }
        }

        private void BtnMoveUp_Click(object sender, RoutedEventArgs e)
        {
            var selectedIndex = StepsList.SelectedIndex;
            if (selectedIndex > 0)
            {
                var item = steps[selectedIndex];
                steps.RemoveAt(selectedIndex);
                steps.Insert(selectedIndex - 1, item);
                StepsList.SelectedIndex = selectedIndex - 1;
                UpdateStepNumbers();
            }
        }

        private void BtnMoveDown_Click(object sender, RoutedEventArgs e)
        {
            var selectedIndex = StepsList.SelectedIndex;
            if (selectedIndex < steps.Count - 1)
            {
                var item = steps[selectedIndex];
                steps.RemoveAt(selectedIndex);
                steps.Insert(selectedIndex + 1, item);
                StepsList.SelectedIndex = selectedIndex + 1;
                UpdateStepNumbers();
            }
        }

        private void BtnClearAll_Click(object sender, RoutedEventArgs e)
        {
            if (steps.Count == 0)
                return;

            var result = MessageBox.Show(
                $"Delete all {steps.Count} step(s)?",
                "Confirm Clear",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                steps.Clear();
                UpdateEmptyState();
                StatusText.Text = "All steps cleared.";
            }
        }

        private void BtnGenerateCode_Click(object sender, RoutedEventArgs e)
        {
            if (steps.Count == 0)
            {
                MessageBox.Show("Please record at least one step first.", "No Steps", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var tourName = string.IsNullOrWhiteSpace(TxtTourName.Text) ? "MyTour" : TxtTourName.Text;
            var tourId = string.IsNullOrWhiteSpace(TxtTourId.Text) ? "my-tour" : TxtTourId.Text;

            var code = GenerateCSharpCode(tourName, tourId);
            ShowCodeWindow("Generated C# Code", code);
        }

        private void BtnGenerateJson_Click(object sender, RoutedEventArgs e)
        {
            if (steps.Count == 0)
            {
                MessageBox.Show("Please record at least one step first.", "No Steps", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var tourName = string.IsNullOrWhiteSpace(TxtTourName.Text) ? "MyTour" : TxtTourName.Text;
            var tourId = string.IsNullOrWhiteSpace(TxtTourId.Text) ? "my-tour" : TxtTourId.Text;

            var json = GenerateJson(tourName, tourId);
            ShowCodeWindow("Generated JSON", json);
        }

        private void BtnPreviewTour_Click(object sender, RoutedEventArgs e)
        {
            if (steps.Count == 0)
            {
                MessageBox.Show("Please record at least one step first.", "No Steps", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                // Create a Tour from the recorded steps
                var tourName = string.IsNullOrWhiteSpace(TxtTourName.Text) ? "Preview Tour" : TxtTourName.Text;
                
                var tour = new Models.Tour
                {
                    Name = tourName,
                    ShowNextButtonDefault = true,
                    Steps = steps.Select(step => new Models.Step(
                        step.ElementId,
                        step.Header,
                        step.Content
                    )).ToArray()
                };

                // Minimize the recorder window during the tour
                this.WindowState = WindowState.Minimized;

                // Start the tour
                tour.Start();

                // Restore the recorder window after a short delay
                // (The tour will be running, so we restore immediately)
                System.Threading.Tasks.Task.Delay(500).ContinueWith(_ =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        this.WindowState = WindowState.Normal;
                    });
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error previewing tour: {ex.Message}\n\nMake sure all Element IDs exist in the main window.", 
                    "Preview Error", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Warning);
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void UpdateEmptyState()
        {
            EmptyState.Visibility = steps.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateStepNumbers()
        {
            for (int i = 0; i < steps.Count; i++)
            {
                steps[i].StepNumber = i + 1;
            }
        }

        private string GenerateCSharpCode(string tourName, string tourId)
        {
            var sb = new System.Text.StringBuilder();
            
            var safeName = MakeSafeIdentifier(tourName);
            
            sb.AppendLine($"public class {safeName}Tour : ITourDefinition");
            sb.AppendLine("{");
            sb.AppendLine($"    public string TourId => \"{tourId}\";");
            sb.AppendLine($"    public string TourName => \"{EscapeString(tourName)}\";");
            sb.AppendLine($"    public string Description => \"TODO: Add description\";");
            sb.AppendLine($"    public int DisplayOrder => 1;");
            sb.AppendLine();
            sb.AppendLine("    public Tour CreateTour()");
            sb.AppendLine("    {");
            sb.AppendLine("        return new Tour");
            sb.AppendLine("        {");
            sb.AppendLine($"            Name = \"{EscapeString(tourName)}\",");
            sb.AppendLine("            ShowNextButtonDefault = true,");
            sb.AppendLine("            Steps = new[]");
            sb.AppendLine("            {");

            foreach (var step in steps)
            {
                sb.AppendLine($"                new Step(\"{EscapeString(step.ElementId)}\",");
                sb.AppendLine($"                    \"{EscapeString(step.Header)}\",");
                sb.AppendLine($"                    \"{EscapeString(step.Content)}\"),");
                sb.AppendLine();
            }

            sb.AppendLine("            }");
            sb.AppendLine("        };");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }

        private string GenerateJson(string tourName, string tourId)
        {
            var sb = new System.Text.StringBuilder();
            
            sb.AppendLine("{");
            sb.AppendLine($"  \"tourId\": \"{EscapeJsonString(tourId)}\",");
            sb.AppendLine($"  \"tourName\": \"{EscapeJsonString(tourName)}\",");
            sb.AppendLine($"  \"showNextButtonDefault\": true,");
            sb.AppendLine($"  \"steps\": [");

            for (int i = 0; i < steps.Count; i++)
            {
                var step = steps[i];
                var comma = i < steps.Count - 1 ? "," : "";
                
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

        private void ShowCodeWindow(string title, string code)
        {
            var window = new Window
            {
                Title = title,
                Width = 800,
                Height = 600,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Owner = this
            };

            var grid = new System.Windows.Controls.Grid();
            grid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition { Height = GridLength.Auto });

            var textBox = new System.Windows.Controls.TextBox
            {
                Text = code,
                IsReadOnly = true,
                TextWrapping = TextWrapping.Wrap,
                VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto,
                FontFamily = new System.Windows.Media.FontFamily("Consolas"),
                FontSize = 12,
                Padding = new Thickness(10),
                Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(30, 30, 30)),
                Foreground = System.Windows.Media.Brushes.White
            };
            System.Windows.Controls.Grid.SetRow(textBox, 0);
            grid.Children.Add(textBox);

            var buttonPanel = new System.Windows.Controls.StackPanel
            {
                Orientation = System.Windows.Controls.Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(10)
            };
            
            var copyButton = new System.Windows.Controls.Button
            {
                Content = "📋 Copy to Clipboard",
                Padding = new Thickness(15, 8, 15, 8),
                Margin = new Thickness(0, 0, 10, 0)
            };
            copyButton.Click += (s, e) =>
            {
                Clipboard.SetText(code);
                MessageBox.Show("Code copied to clipboard!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            };
            buttonPanel.Children.Add(copyButton);

            var closeButton = new System.Windows.Controls.Button
            {
                Content = "Close",
                Padding = new Thickness(15, 8, 15, 8),
                IsCancel = true
            };
            closeButton.Click += (s, e) => window.Close();
            buttonPanel.Children.Add(closeButton);

            System.Windows.Controls.Grid.SetRow(buttonPanel, 1);
            grid.Children.Add(buttonPanel);

            window.Content = grid;
            window.ShowDialog();
        }

        private string MakeSafeIdentifier(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "UnnamedTour";

            var sb = new System.Text.StringBuilder();
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

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            // Stop recording if still active
            if (BtnStopRecording.IsEnabled)
            {
                StopRecording();
            }
            
            base.OnClosing(e);
        }
    }

    /// <summary>
    /// ViewModel for displaying recorded steps in the UI.
    /// </summary>
    public class RecordedStepViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private int stepNumber;

        public RecordedStepViewModel(RecordedStep step)
        {
            ElementId = step.ElementId;
            Header = step.Header;
            Content = step.Content;
            Placement = step.Placement;
            ElementType = step.ElementType;
            AutomationId = step.AutomationId;
            ControlName = step.ControlName;
        }

        public int StepNumber
        {
            get => stepNumber;
            set
            {
                stepNumber = value;
                OnPropertyChanged(nameof(StepNumber));
            }
        }

        public string ElementId { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }
        public Placement Placement { get; set; }
        public string ElementType { get; set; }
        public string AutomationId { get; set; }
        public string ControlName { get; set; }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
}

