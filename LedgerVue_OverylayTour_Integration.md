# Integration Plan: LedgerVue.FeatureTour → LedgerVue

## Overview
Add LedgerVue.FeatureTour as a project reference to LedgerVue, enabling visual tour recording, playback, and browsing capabilities with support for both compiled and dynamic tour definitions.

---

## Phase 1: Project Setup

### 1.1 Add Project Reference
In `LedgerVue.csproj` (or main executable project):
```xml
<ItemGroup>
  <ProjectReference Include="..\LedgerVue.FeatureTour\LedgerVue.FeatureTour\LedgerVue.FeatureTour.csproj" />
</ItemGroup>
```

### 1.2 Copy LedgerVue.FeatureTour to Solution
- Copy the entire `LedgerVue.FeatureTour` folder into your LedgerVue solution directory
- Add to solution file: Right-click solution → Add → Existing Project → select `LedgerVue.FeatureTour.csproj`

### 1.3 Optional: Enable Telerik RadCallout
If LedgerVue already uses Telerik UI:
```xml
<!-- In LedgerVue.csproj -->
<PropertyGroup>
  <UseTelerik>true</UseTelerik>
</PropertyGroup>
```

Then in `App.xaml.cs`:
```csharp
protected override void OnStartup(StartupEventArgs e)
{
    base.OnStartup(e);
    TourConfiguration.UseTelerikCallout(); // or UseDefaultCallout()
}
```

---

## Phase 2: Tour Infrastructure

### 2.1 Create Tours Folder Structure
```
LedgerVue/
  Tours/
    ElementIDs.cs          # Centralized element ID constants
    TourRegistry.cs        # Tour registration and management
    TourBrowserWindow.xaml # UI for browsing available tours
    Definitions/
      IntroductionTour.cs  # Built-in tours (C#)
      AccountsTour.cs
      PortfoliosTour.cs
      InstrumentsTour.cs
      DealsTour.cs
    Recorded/              # JSON tours from recorder
      custom-tour-1.json
      custom-tour-2.json
```

### 2.2 Create ElementIDs.cs
```csharp
namespace LedgerVue.Tours
{
    public static class ElementIDs
    {
        // Main Navigation
        public const string MainRibbon = "MainRibbon";
        public const string HomeTab = "HomeTab";
        public const string HelpButton = "HelpButton";
        
        // Accounts
        public const string AccountsView = "AccountsView";
        public const string NewAccountButton = "NewAccountButton";
        public const string AccountsGrid = "AccountsGrid";
        
        // Portfolios
        public const string PortfoliosView = "PortfoliosView";
        public const string NewPortfolioButton = "NewPortfolioButton";
        
        // Custodians
        public const string CustodiansView = "CustodiansView";
        
        // Instruments
        public const string InstrumentsView = "InstrumentsView";
        public const string InstrumentSearchBox = "InstrumentSearchBox";
        
        // Deals
        public const string DealsView = "DealsView";
        public const string NewDealButton = "NewDealButton";
    }
}
```

### 2.3 Create TourRegistry.cs
```csharp
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LedgerVue.FeatureTouring.Models;
using LedgerVue.FeatureTouring.Helper;

namespace LedgerVue.Tours
{
    public class TourRegistry
    {
        private static readonly Lazy<TourRegistry> _instance = new(() => new TourRegistry());
        public static TourRegistry Instance => _instance.Value;
        
        private readonly Dictionary<string, Func<Tour>> _tours = new();
        private readonly string _recordedToursPath;
        
        private TourRegistry()
        {
            _recordedToursPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "LedgerVue", "Tours");
            
            Directory.CreateDirectory(_recordedToursPath);
            
            RegisterBuiltInTours();
            LoadRecordedTours();
        }
        
        private void RegisterBuiltInTours()
        {
            Register("introduction", "Introduction to User Interface", () => new IntroductionTour().Create());
            Register("accounts", "Defining Accounts, Portfolios, Custodians", () => new AccountsTour().Create());
            Register("instruments", "Defining Instruments", () => new InstrumentsTour().Create());
            Register("deals", "Creating Deals", () => new DealsTour().Create());
        }
        
        private void LoadRecordedTours()
        {
            if (!Directory.Exists(_recordedToursPath)) return;
            
            foreach (var file in Directory.GetFiles(_recordedToursPath, "*.json"))
            {
                try
                {
                    var json = File.ReadAllText(file);
                    var serializer = new GenericXmlSerializer<Tour>(); // or JSON serializer
                    var tour = serializer.Deserialize(json);
                    var tourId = $"recorded_{Path.GetFileNameWithoutExtension(file)}";
                    Register(tourId, tour.Name, () => tour);
                }
                catch { /* Log error */ }
            }
        }
        
        public void Register(string id, string displayName, Func<Tour> tourFactory)
        {
            _tours[id] = tourFactory;
        }
        
        public IEnumerable<(string Id, string Name)> GetAllTours()
        {
            return _tours.Select(t => (t.Key, GetTourName(t.Key)));
        }
        
        public void StartTour(string tourId)
        {
            if (_tours.TryGetValue(tourId, out var factory))
            {
                var tour = factory();
                tour.Start();
            }
        }
        
        public void SaveRecordedTour(Tour tour, string fileName)
        {
            var filePath = Path.Combine(_recordedToursPath, $"{fileName}.json");
            var serializer = new GenericXmlSerializer<Tour>();
            File.WriteAllText(filePath, serializer.Serialize(tour));
            LoadRecordedTours(); // Refresh registry
        }
        
        private string GetTourName(string tourId)
        {
            var tour = _tours[tourId]();
            return tour.Name;
        }
    }
}
```

### 2.4 Create Sample Tour Definition
```csharp
// Tours/Definitions/IntroductionTour.cs
using LedgerVue.FeatureTouring.Models;

namespace LedgerVue.Tours.Definitions
{
    public class IntroductionTour
    {
        public Tour Create()
        {
            return new Tour
            {
                Name = "Introduction to User Interface",
                ShowNextButtonDefault = true,
                Steps = new[]
                {
                    new Step(ElementIDs.MainRibbon, 
                        "Welcome to LedgerVue", 
                        "This is your main navigation ribbon where you'll access all major features."),
                    
                    new Step(ElementIDs.HomeTab, 
                        "Home Tab", 
                        "The Home tab contains your most frequently used actions."),
                    
                    new Step(ElementIDs.AccountsView, 
                        "Accounts & Portfolios", 
                        "Manage your accounts, portfolios, and custodians here."),
                }
            };
        }
    }
}
```

---

## Phase 3: UI Integration

### 3.1 Add XAML Namespace to MainWindow
```xaml
<Window ...
        xmlns:tour="clr-namespace:LedgerVue.FeatureTouring;assembly=LedgerVue.FeatureTour"
        xmlns:tours="clr-namespace:LedgerVue.Tours">
```

### 3.2 Tag UI Elements with ElementIDs
Throughout your XAML files:
```xaml
<!-- Example for RadRibbon -->
<telerik:RadRibbon x:Name="MainRibbon"
                   tour:TourHelper.ElementID="{x:Static tours:ElementIDs.MainRibbon}"
                   tour:TourHelper.Placement="BottomCenter">
    
    <telerik:RadRibbonTab Header="Home"
                          tour:TourHelper.ElementID="{x:Static tours:ElementIDs.HomeTab}">
        ...
    </telerik:RadRibbonTab>
</telerik:RadRibbon>

<!-- Example for grids/buttons -->
<Button Content="New Account"
        tour:TourHelper.ElementID="{x:Static tours:ElementIDs.NewAccountButton}"
        tour:TourHelper.Placement="BottomCenter"/>
```

### 3.3 Add Help Menu to RadRibbon
```xaml
<telerik:RadRibbonTab Header="Help"
                      tour:TourHelper.ElementID="{x:Static tours:ElementIDs.HelpTab}">
    <telerik:RadRibbonGroup Header="Product Tours">
        <telerik:RadRibbonButton Text="Browse Tours"
                                 LargeImage="pack://application:,,,/Images/tour-icon.png"
                                 Command="{Binding CmdBrowseTours}"
                                 tour:TourHelper.ElementID="{x:Static tours:ElementIDs.BrowseToursButton}"/>
        
        <telerik:RadRibbonButton Text="Quick Start"
                                 Command="{Binding CmdStartIntroTour}"
                                 SmallImage="pack://application:,,,/Images/play-icon.png"/>
    </telerik:RadRibbonGroup>
    
    <!-- Admin/Developer section (could be conditional) -->
    <telerik:RadRibbonGroup Header="Admin">
        <telerik:RadRibbonButton Text="Record Tour"
                                 Command="{Binding CmdRecordTour}"
                                 SmallImage="pack://application:,,,/Images/record-icon.png"
                                 ToolTip="Record new product tour (Admin only)"/>
    </telerik:RadRibbonGroup>
</telerik:RadRibbonTab>
```

### 3.4 Create TourBrowserWindow.xaml
```xaml
<Window x:Class="LedgerVue.Tours.TourBrowserWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Product Tours" Height="500" Width="700"
        WindowStartupLocation="CenterOwner">
    <Grid Margin="20">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
            <RowDefinition Height="Auto"/>
        </Grid.RowDefinitions>
        
        <TextBlock Text="Available Product Tours" 
                   FontSize="20" FontWeight="Bold" Margin="0,0,0,20"/>
        
        <ListBox Grid.Row="1" x:Name="ToursList" 
                 ItemsSource="{Binding Tours}"
                 SelectedItem="{Binding SelectedTour}"
                 DisplayMemberPath="Name"
                 FontSize="14"
                 Padding="10"/>
        
        <StackPanel Grid.Row="2" Orientation="Horizontal" 
                    HorizontalAlignment="Right" Margin="0,20,0,0">
            <Button Content="Start Tour" Width="100" Height="35" 
                    Command="{Binding CmdStartTour}" Margin="5"/>
            <Button Content="Close" Width="100" Height="35" 
                    IsCancel="True" Margin="5"/>
        </StackPanel>
    </Grid>
</Window>
```

---

## Phase 4: ViewModel Integration

### 4.1 Add Commands to MainViewModel
```csharp
using LedgerVue.FeatureTouring;
using LedgerVue.FeatureTouring.Recording;
using LedgerVue.Tours;

public class MainViewModel : ViewModelBase
{
    private ICommand? _cmdBrowseTours;
    private ICommand? _cmdStartIntroTour;
    private ICommand? _cmdRecordTour;
    
    public ICommand CmdBrowseTours => _cmdBrowseTours ??= new RelayCommand(() =>
    {
        var browser = new TourBrowserWindow { Owner = Application.Current.MainWindow };
        browser.ShowDialog();
    });
    
    public ICommand CmdStartIntroTour => _cmdStartIntroTour ??= new RelayCommand(() =>
    {
        TourRegistry.Instance.StartTour("introduction");
    });
    
    public ICommand CmdRecordTour => _cmdRecordTour ??= new RelayCommand(() =>
    {
        var recorder = new TourRecorderWindow(Application.Current.MainWindow);
        recorder.TourSaved += (sender, tour) =>
        {
            // Prompt for tour name
            var tourName = PromptForTourName();
            if (!string.IsNullOrEmpty(tourName))
            {
                TourRegistry.Instance.SaveRecordedTour(tour, tourName);
                MessageBox.Show($"Tour '{tour.Name}' saved successfully!", "Tour Saved");
            }
        };
        recorder.Show();
    });
    
    private string? PromptForTourName()
    {
        // Use Telerik RadWindow or simple input dialog
        // Return user-entered name
        return "my-custom-tour";
    }
}
```

---

## Phase 5: First-Run Experience

### 5.1 Add First-Run Detection in App.xaml.cs
```csharp
protected override void OnStartup(StartupEventArgs e)
{
    base.OnStartup(e);
    
    // Initialize tour configuration
    TourConfiguration.UseTelerikCallout(); // or UseDefaultCallout()
    
    // Check first run
    if (IsFirstRun())
    {
        Application.Current.MainWindow.Loaded += (s, args) =>
        {
            // Slight delay to ensure UI is fully loaded
            System.Threading.Tasks.Task.Delay(500).ContinueWith(_ =>
            {
                Dispatcher.Invoke(() =>
                {
                    var result = MessageBox.Show(
                        "Welcome to LedgerVue! Would you like a quick tour?",
                        "Welcome",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);
                    
                    if (result == MessageBoxResult.Yes)
                    {
                        TourRegistry.Instance.StartTour("introduction");
                    }
                });
            });
        };
        
        MarkFirstRunComplete();
    }
}

private bool IsFirstRun()
{
    var settingsPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "LedgerVue", "settings.json");
    
    return !File.Exists(settingsPath) || 
           !File.ReadAllText(settingsPath).Contains("\"FirstRunComplete\":true");
}

private void MarkFirstRunComplete()
{
    var settingsPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "LedgerVue", "settings.json");
    
    Directory.CreateDirectory(Path.GetDirectoryName(settingsPath)!);
    File.WriteAllText(settingsPath, "{\"FirstRunComplete\":true}");
}
```

---

## Phase 6: Tour Recorder Enhancements

### 6.1 Extended TourRecorderWindow for Save Callback
Since the existing `TourRecorderWindow` already has export functionality, add event for programmatic save:

```csharp
// In TourRecorderWindow.xaml.cs - add event
public event EventHandler<Tour>? TourSaved;

// Modify existing save/export to raise event
private void OnTourExported(Tour tour)
{
    TourSaved?.Invoke(this, tour);
}
```

### 6.2 Admin-Only Recorder Access (Optional)
```csharp
// In MainViewModel
public Visibility RecorderVisibility => IsAdminUser() ? Visibility.Visible : Visibility.Collapsed;

private bool IsAdminUser()
{
    // Check user role/permissions
    return CurrentUser.HasRole("Admin") || Debugger.IsAttached;
}
```

---

## Summary of Files to Create

**Required in LedgerVue:**
1. `Tours/ElementIDs.cs` - Element identifier constants
2. `Tours/TourRegistry.cs` - Tour management singleton
3. `Tours/TourBrowserWindow.xaml` + `.cs` - Tour selection UI
4. `Tours/Definitions/IntroductionTour.cs` - Sample built-in tour
5. `Tours/Definitions/AccountsTour.cs` - Accounts/Portfolios tour
6. `Tours/Definitions/InstrumentsTour.cs` - Instruments tour
7. `Tours/Definitions/DealsTour.cs` - Deals tour

**Modify in LedgerVue:**
1. Main `.csproj` - Add project reference
2. `App.xaml.cs` - Initialize tours, first-run logic
3. `MainWindow.xaml` - Add tour namespace, tag elements with ElementIDs
4. `MainViewModel.cs` - Add tour-related commands
5. All XAML views - Tag important elements with `tour:TourHelper.ElementID`

**Copy to Solution:**
1. Entire `LedgerVue.FeatureTour` folder

---

## Benefits of This Approach

✅ **No Embedded Code** - LedgerVue.FeatureTour remains separate, reusable project  
✅ **Dual Storage** - Built-in C# tours + dynamic JSON tours  
✅ **Admin Recorder** - Power users can create tours in production  
✅ **Discoverable** - Help menu + first-run makes tours accessible  
✅ **Maintainable** - Centralized ElementIDs and TourRegistry  
✅ **Scalable** - Easy to add new tours without code changes (JSON)  

---

## Testing Checklist

- [ ] Build succeeds with project reference
- [ ] Help menu appears in RadRibbon
- [ ] "Browse Tours" opens TourBrowserWindow
- [ ] Built-in tours start and navigate correctly
- [ ] Tour Recorder opens and can record steps
- [ ] Recorded tours save to AppData
- [ ] Recorded tours appear in browser
- [ ] First-run tour prompts on fresh install
- [ ] Element-based positioning works on different resolutions
- [ ] Callout follows elements when window resizes

