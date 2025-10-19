// Copyright (c) ML Labs. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using System.Text;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LedgerVue.FeatureTouring.Navigation;
using LedgerVue.FeatureTouring.Touring;
using LedgerVue.FeatureTouring.Recording;

namespace LedgerVue.FeatureTouring
{
    public class MainWindowViewModel : ObservableObject
    {
        private ICommand? _cmdStartPositioning;
        private ICommand? _cmdStartIntroduction;
        private ICommand? _cmdStartActiveTour;
        private ICommand? _cmdStartDialogTour;
        private ICommand? _cmdStartCustomViewTour;
        private ICommand? _cmdStartOverView;
        private ICommand? _cmdOpenDialog;
        private ICommand? _cmdClear;
        private ICommand? _cmdUseDefaultCallout;
        private ICommand? _cmdUseTelerikCallout;
        private ICommand? _cmdStartRecording;
        private string _currentCalloutType = "Default TourControl";
        private Placement _placement;
        private int _colorSchemaIndex;
        private int _tabIndex;
        private string? _path;
        private string? _result;
        private int _selectedIndex;
        private string? _styleText;

        private readonly PopupStyle myPopupStyle = new PopupStyle();


        // .ctor

        private MainWindowViewModel()
        {
            FeatureTour.SetViewModelFactoryMethod(tourRun => new CustomTourViewModel(tourRun));

            var navigator = FeatureTour.GetNavigator();

            navigator.ForStep(ElementID.TextBoxPath).AttachDoable(s => Path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            navigator.ForStep(ElementID.ComboBoxOption).AttachDoable(s => SelectedIndex = 1);
            navigator.ForStep(ElementID.TextBoxPath).AttachDoable(s => Path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

            PopupStyle.PropertyChanged += (s, e) => StyleText = GetStyleText();
            StyleText = GetStyleText();
        }

        private string GetStyleText()
        {
            var sb = new StringBuilder();

            sb.AppendLine("...");
            sb.AppendLine("xmlns:featureTouringControls=\"clr -namespace:LedgerVue.FeatureTouring.Controls;assembly=ThinkSharp.FeatureTour\"");
            sb.AppendLine("...");
            sb.AppendLine("");
            sb.AppendLine("");
            sb.AppendLine("<Style TargetType=\"featureTouringControls: TourControl\">");
            sb.AppendLine($"    <Setter Property=\"Background\" Value=\"{myPopupStyle.BackgroundColor}\"/>");
            sb.AppendLine($"    <Setter Property=\"BorderBrush\" Value=\"{myPopupStyle.BorderBrushColor}\"/>");
            sb.AppendLine($"    <Setter Property=\"Foreground\" Value=\"{myPopupStyle.ForegroundColor}\"/>");
            sb.AppendLine($"    <Setter Property=\"FontSize\" Value=\"{myPopupStyle.FontSize:0}\"/>");
            sb.AppendLine($"    <Setter Property=\"CornerRadius\" Value=\"{myPopupStyle.CornerRadius:0}\"/>");
            sb.AppendLine($"    <Setter Property=\"BorderThickness\" Value=\"{myPopupStyle.BorderThickness.Top:0}\"/>");
            sb.AppendLine("</Style>");
            return sb.ToString();
        }


        // Methods

        private void Clear()
        {
            Result = "";
            Path = "";
            SelectedIndex = 0;
            FeatureTour.GetNavigator()
                .IfCurrentStepEquals(ElementID.ButtonClear)
                .Close();
        }


        // Commands

        public ICommand CmdStartPositioning
        {
            get
            {
                if (_cmdStartPositioning == null)
                {
                    _cmdStartPositioning = new RelayCommand(TourStarter.StartPositioning);
                }
                return _cmdStartPositioning;
            }
        }
        
        public ICommand CmdStartIntroduction
        {
            get
            {
                if (_cmdStartIntroduction == null)
                {
                    _cmdStartIntroduction = new RelayCommand(TourStarter.StartIntroduction);
                }
                return _cmdStartIntroduction;
            }
        }

        public ICommand CmdStartActiveTour
        {
            get
            {
                if (_cmdStartActiveTour == null)
                {
                    _cmdStartActiveTour = new RelayCommand(TourStarter.StartActiveTour);
                }
                return _cmdStartActiveTour;
            }
        }

        public ICommand CmdStartDialogTour
        {
            get
            {
                if (_cmdStartDialogTour == null)
                {
                    _cmdStartDialogTour = new RelayCommand(TourStarter.StartDialogTour);
                }
                return _cmdStartDialogTour;
            }
        }

        public ICommand CmdStartCustomView
        {
            get
            {
                if (_cmdStartCustomViewTour == null)
                {
                    _cmdStartCustomViewTour = new RelayCommand(TourStarter.StartCustomViewTour);
                }
                return _cmdStartCustomViewTour;
            }
        }

        public ICommand CmdStartOverView
        {
            get
            {
                if (_cmdStartOverView == null)
                {
                    _cmdStartOverView = new RelayCommand(TourStarter.StartOverView);
                }
                return _cmdStartOverView;
            }
        }

        public ICommand CmdOpenDialog
        {
            get
            {
                if (_cmdOpenDialog == null)
                {
                    _cmdOpenDialog = new RelayCommand(() =>
                    {
                        var dlg = new Dialog();
                        dlg.ShowDialog();
                    });
                }
                return _cmdOpenDialog;
            }
        }

        public ICommand CmdClear
        {
            get
            {
                if (_cmdClear == null)
                {
                    _cmdClear = new RelayCommand(Clear);
                }
                return _cmdClear;
            }
        }

        public ICommand CmdUseDefaultCallout
        {
            get
            {
                if (_cmdUseDefaultCallout == null)
                {
                    _cmdUseDefaultCallout = new RelayCommand(() =>
                    {
                        TourConfiguration.UseDefaultCallout();
                        CurrentCalloutType = TourConfiguration.CalloutControlFactory.ImplementationName;
                    });
                }
                return _cmdUseDefaultCallout;
            }
        }

        public ICommand CmdUseTelerikCallout
        {
            get
            {
                if (_cmdUseTelerikCallout == null)
                {
                    _cmdUseTelerikCallout = new RelayCommand(() =>
                    {
                        try
                        {
                            TourConfiguration.UseTelerikCallout();
                            CurrentCalloutType = TourConfiguration.CalloutControlFactory.ImplementationName;
                        }
                        catch (InvalidOperationException ex)
                        {
                            System.Windows.MessageBox.Show(ex.Message, "Telerik Not Available", 
                                System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                        }
                    });
                }
                return _cmdUseTelerikCallout;
            }
        }

        public ICommand CmdStartRecording
        {
            get
            {
                if (_cmdStartRecording == null)
                {
                    _cmdStartRecording = new RelayCommand(() =>
                    {
                        try
                        {
                            var recorderWindow = new TourRecorderWindow(System.Windows.Application.Current.MainWindow);
                            recorderWindow.Show();
                        }
                        catch (System.Exception ex)
                        {
                            System.Windows.MessageBox.Show($"Error starting recorder: {ex.Message}", 
                                "Recording Error", 
                                System.Windows.MessageBoxButton.OK, 
                                System.Windows.MessageBoxImage.Error);
                        }
                    });
                }
                return _cmdStartRecording;
            }
        }


        // Properties

        public Placement Placement
        {
            get { return _placement; }
            set { SetProperty(ref _placement, value); }
        }
        
        public int ColorSchemaIndex
        {
            get { return _colorSchemaIndex; }
            set { SetProperty(ref _colorSchemaIndex, value); }
        }

        public int TabIndex
        {
            get { return _tabIndex; }
            set { SetProperty(ref _tabIndex, value); }
        }

        public string? Path
        {
            get { return _path; }
            set { SetProperty(ref _path, value); }
        }

        public string? Result
        {
            get { return _result; }
            set { SetProperty(ref _result, value); }
        }

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { SetProperty(ref _selectedIndex, value); }
        }

        public string? StyleText
        {
            get { return _styleText; }
            set { SetProperty(ref _styleText, value); }
        }

        public PopupStyle PopupStyle => myPopupStyle;

        public string CurrentCalloutType
        {
            get { return _currentCalloutType; }
            set { SetProperty(ref _currentCalloutType, value); }
        }

        public static MainWindowViewModel Instance { get; } = new MainWindowViewModel();
    }
}
