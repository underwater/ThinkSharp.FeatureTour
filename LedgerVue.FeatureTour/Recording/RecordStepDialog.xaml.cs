// Copyright (c) ML Labs. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System.Windows;

namespace LedgerVue.FeatureTouring.Recording
{
    /// <summary>
    /// Dialog for inputting tour step details during recording.
    /// </summary>
    public partial class RecordStepDialog : Window
    {
        public string ElementId
        {
            get => txtElementId.Text;
            set => txtElementId.Text = value;
        }

        public string StepHeader
        {
            get => txtHeader.Text;
            set => txtHeader.Text = value;
        }

        public string StepContent
        {
            get => txtContent.Text;
            set => txtContent.Text = value;
        }

        public string ElementType { get; set; }

        public RecordStepDialog()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(StepHeader))
            {
                MessageBox.Show("Please enter a step header.", "Validation", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtHeader.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(StepContent))
            {
                MessageBox.Show("Please enter step content.", "Validation", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtContent.Focus();
                return;
            }

            DialogResult = true;
            Close();
        }

        private void BtnSkip_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}

