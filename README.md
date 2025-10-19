# ThinkSharp.FeatureTour

[![Build status](https://ci.appveyor.com/api/projects/status/l3aagqmbfmgxwv3t?svg=true)](https://ci.appveyor.com/project/JanDotNet/thinksharp-featuretour)
[![NuGet](https://img.shields.io/nuget/v/ThinkSharp.FeatureTour.svg)](https://www.nuget.org/packages/ThinkSharp.FeatureTour/) [![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE.TXT)
[![Donate](https://img.shields.io/badge/Donate-PayPal-green.svg)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=MSBFDUU5UUQZL)

## 🎯 Introduction

**ThinkSharp.FeatureTour** is a powerful WPF component for creating **interactive product tours** that guide users through your application's features. 

### ✨ Key Features

- 🎥 **Visual Tour Recorder** - Point-and-click tour creation (no coding required!)
- 📍 **Element-Based Positioning** - No pixel coordinates, auto-adapts to window resizing
- 🎨 **Customizable Callouts** - Default or Telerik RadCallout support
- 🔄 **Resolution Independent** - Works on any screen size or DPI
- 📝 **Code Generation** - Export tours as C# code or JSON
- ▶️ **Live Preview** - Test tours instantly before integrating
- 🎯 **AutomationID Support** - Leverage existing UI test identifiers
- 🎭 **Full Customization** - Styling, templating, and localization support

### 📦 What's New

- ✨ **Tour Recorder with Live Preview** - Record tours by clicking UI elements and test them immediately
- 🎨 **Multiple Callout Options** - Choose between default or Telerik callouts
- ✏️ **Full Editing Support** - Edit, delete, and reorder recorded steps
- 📋 **Dual Export** - Generate both C# code and JSON formats

---

## 📋 Table of Contents

1. [Quick Start](#-quick-start)
2. [Installation](#-installation)
3. [Tour Recorder - Creating Tours Visually](#-tour-recorder---creating-tours-visually)
4. [Creating Tours Programmatically](#-creating-tours-programmatically)
5. [Integration Guide](#-integration-guide)
6. [Element-Based Positioning](#-element-based-positioning)
7. [Callout Options](#-callout-options)
8. [Advanced Features](#-advanced-features)
9. [Best Practices](#-best-practices)
10. [Troubleshooting](#-troubleshooting)
11. [API Reference](#-api-reference)

---

## 🚀 Quick Start

### Option 1: Using the Tour Recorder (Recommended)

The **easiest way** to create tours:

```bash
# Run the demo
dotnet run --project ThinkSharp.FeatureTour.Demo
```

1. Click **"🎥 Record New Tour"**
2. Click **"▶ Start Recording"**
3. Click UI elements in your desired order
4. Fill in header and content for each step
5. Click **"⏹ Stop Recording"**
6. Click **"▶ Preview Tour"** to test it immediately! ✨
7. Click **"Generate C# Code"** to get ready-to-use code

**That's it!** Copy the code and paste it into your project.

### Option 2: Manual Code Creation

```csharp
// 1. Add ElementIDs to your XAML
<Button featureTouring:TourHelper.ElementID="WelcomeButton" Content="Welcome"/>

// 2. Create a tour
var tour = new Tour
{
    Name = "Introduction",
    ShowNextButtonDefault = true,
    Steps = new[]
    {
        new Step("WelcomeButton", "Welcome!", "This button starts your journey."),
        new Step("MenuBar", "Navigation", "Use this menu to access features."),
    }
};

// 3. Start the tour
tour.Start();
```

---

## 📦 Installation

### Via NuGet
```
      Install-Package ThinkSharp.FeatureTour
```

### Via Project Reference
```xml
<ProjectReference Include="path\to\ThinkSharp.FeatureTour\ThinkSharp.FeatureTour.csproj" />
```

### Optional: Telerik Callout Support

To use Telerik RadCallout instead of the default callout:

```xml
<!-- In your .csproj -->
<PropertyGroup>
  <UseTelerik>true</UseTelerik>
</PropertyGroup>
```

Then in your app startup:
```csharp
TourConfiguration.UseTelerikCallout();
```

---

## 🎥 Tour Recorder - Creating Tours Visually

The **Tour Recorder** is the fastest way to create product tours. Instead of writing code, you simply click through your UI and enter descriptions.

### Complete Workflow

```
┌─────────────┐    ┌──────────┐    ┌──────────┐    ┌─────────────┐
│   Record    │ ─> │ Preview  │ ─> │ Generate │ ─> │  Integrate  │
│   Steps     │    │   Tour   │    │   Code   │    │    Code     │
└─────────────┘    └──────────┘    └──────────┘    └─────────────┘
```

### Step-by-Step Guide

#### 1. Add Recorder to Your App

```csharp
using ThinkSharp.FeatureTouring.Recording;

// In your ViewModel
public ICommand CmdStartRecording => new RelayCommand(() =>
{
    var recorderWindow = new TourRecorderWindow(Application.Current.MainWindow);
    recorderWindow.Show();
});
```

```xaml
<!-- In your UI -->
<Button Content="🎥 Record New Tour" Command="{Binding CmdStartRecording}"/>
```

#### 2. Record Your Tour

1. Click **"▶ Start Recording"**
2. Click on any UI element you want in the tour
3. A dialog appears for each element:
   - **Element ID**: Auto-detected from `TourHelper.ElementID` or `AutomationId`
   - **Step Header**: Title for this step (e.g., "Welcome to Dashboard")
   - **Step Content**: Description (e.g., "This is your main workspace...")
4. Click **"OK"** to record the step
5. Repeat for all desired steps
6. Click **"⏹ Stop Recording"** when done

#### 3. Edit Your Steps

After recording, you can:
- ✏️ **Edit** steps - Select and click "Edit Selected Step"
- 🗑️ **Delete** steps - Select and click "Delete Selected Step"  
- ↕️ **Reorder** steps - Use "Move Up" / "Move Down" buttons

#### 4. Preview Your Tour ✨ NEW!

Before generating code, test your tour:

1. Click **"▶ Preview Tour"** (orange button)
2. Recorder window minimizes
3. Tour runs immediately on your main window
4. Navigate through using Next/Close buttons
5. Recorder window restores automatically

**Benefits of Preview:**
- ✅ Instant feedback - see exactly how users will experience it
- ✅ Catch mistakes early - verify all element IDs work
- ✅ Iterate quickly - preview → edit → preview
- ✅ No code needed - test before committing

#### 5. Generate Code

Enter tour metadata:
- **Tour Name**: "Introduction to Dashboard"
- **Tour ID**: "intro-dashboard"

Then choose:
- **"📄 Generate C# Code"** - Get C# code for direct integration
- **"📋 Generate JSON"** - Get JSON for configuration files

#### 6. Integrate Generated Code

**Generated C# Output:**
```csharp
public class IntroductionToDashboardTour : ITourDefinition
{
    public string TourId => "intro-dashboard";
    public string TourName => "Introduction to Dashboard";
    
    public Tour CreateTour()
    {
        return new Tour
        {
            Name = "Introduction to Dashboard",
            ShowNextButtonDefault = true,
            Steps = new[]
            {
                new Step("DashboardGrid", "Dashboard", "Your main workspace..."),
                new Step("MenuButton", "Menu", "Access features here..."),
            }
        };
    }
}
```

**How to Use It:**
```csharp
// Option A: Direct usage
public void StartMyTour()
{
    var tour = new Tour
    {
        Name = "Introduction to Dashboard",
        ShowNextButtonDefault = true,
        Steps = new[]
        {
            new Step("DashboardGrid", "Dashboard", "Your main workspace..."),
            new Step("MenuButton", "Menu", "Access features here..."),
        }
    };
    tour.Start();
}

// Option B: Structured (recommended for production)
// Create Tours/MyTours.cs
public static class MyTours
{
    public static void StartDashboardTour()
    {
        // Paste generated tour code here
    }
}
```

---

## 💻 Creating Tours Programmatically

If you prefer coding tours manually (or need advanced customization):

### Basic Tour

```csharp
var tour = new Tour
{
    Name = "My Tour",
    ShowNextButtonDefault = true,
    Steps = new[]
    {
        new Step("ElementID1", "Step Title", "Step description"),
        new Step("ElementID2", "Next Step", "More details"),
    }
};

tour.Start();
```

### Active Tour (User Must Interact)

```csharp
var tour = new Tour
{
    Name = "Interactive Tour",
    ShowNextButtonDefault = false, // User must complete actions
    Steps = new[]
    {
        new Step("UsernameField", "Enter Name", "Type your username") 
        { 
            ShowNextButton = false // Forces user interaction
        },
        new Step("PasswordField", "Enter Password", "Type your password"),
    }
};

tour.Start();
```

### Tour with Custom Actions

```csharp
var navigator = FeatureTour.GetNavigator();

// Attach automatic actions to steps
navigator.ForStep("UsernameField").AttachDoable(step => 
{
    // Auto-fill demo data
    UsernameTextBox.Text = "demo@example.com";
});

tour.Start();
```

---

## 🏗️ Integration Guide

### Recommended Project Structure

For applications with multiple tours:

```
YourProject/
├── Tours/
│   ├── TourDefinitions/
│   │   ├── IntroductionTour.cs
│   │   ├── AccountManagementTour.cs
│   │   ├── InstrumentDefinitionTour.cs
│   │   └── DealCreationTour.cs
│   ├── TourManager.cs
│   ├── TourRegistry.cs
│   └── ElementIDs.cs
└── Views/
    └── YourViews.xaml
```

### 1. Define Element IDs (Tours/ElementIDs.cs)

```csharp
public static class ElementIDs
{
    // Main Navigation
    public const string MainMenu = "MainMenu";
    public const string DashboardTab = "DashboardTab";
    
    // Account Management
    public const string AccountsGrid = "AccountsGrid";
    public const string NewAccountButton = "NewAccountButton";
    
    // Portfolio Management
    public const string PortfoliosGrid = "PortfoliosGrid";
    public const string NewPortfolioButton = "NewPortfolioButton";
}
```

### 2. Create Tour Definitions

```csharp
public class IntroductionTour : ITourDefinition
{
    public string TourId => "introduction";
    public string TourName => "Introduction to UI";
    public int DisplayOrder => 1;

    public Tour CreateTour()
    {
        return new Tour
        {
            Name = TourName,
            ShowNextButtonDefault = true,
            Steps = new[]
            {
                new Step(ElementIDs.MainMenu, "Welcome", "Welcome to the app!"),
                new Step(ElementIDs.DashboardTab, "Dashboard", "Your main view."),
            }
        };
    }
}
```

### 3. Set Element IDs in XAML

```xaml
<Window xmlns:tour="clr-namespace:ThinkSharp.FeatureTouring;assembly=ThinkSharp.FeatureTour"
        xmlns:local="clr-namespace:YourApp.Tours">
    
    <DataGrid tour:TourHelper.ElementID="{x:Static local:ElementIDs.AccountsGrid}"
              tour:TourHelper.Placement="TopCenter"/>
    
    <Button Content="New Account"
            tour:TourHelper.ElementID="{x:Static local:ElementIDs.NewAccountButton}"
            tour:TourHelper.Placement="BottomCenter"/>
</Window>
```

### 4. Start Tours

```csharp
// Direct start
TourManager.Instance.StartTour("introduction");

// Or via command
public ICommand CmdStartTour => new RelayCommand(() => 
{
    MyTours.StartIntroductionTour();
});
```

---

## 📍 Element-Based Positioning

### How It Works

ThinkSharp.FeatureTour uses **element-based positioning**, NOT pixel coordinates:

✅ **No pixel coordinates** - Tours adapt to any screen resolution  
✅ **Auto-resizing** - Callouts follow elements when window resizes  
✅ **Dynamic positioning** - Updates in real-time  
✅ **DPI-independent** - Works on 4K displays  

### Three Ways to Identify Elements

#### 1. TourHelper.ElementID (Recommended)
```xaml
<Button tour:TourHelper.ElementID="SaveButton" Content="Save"/>
```

#### 2. AutomationProperties.AutomationId
```xaml
<Button AutomationProperties.AutomationId="SaveButton" Content="Save"/>
```

Enable AutomationID support:
```csharp
// App.xaml.cs
protected override void OnStartup(StartupEventArgs e)
{
    base.OnStartup(e);
    MainWindow.EnableAutomationIdSupport();
}
```

#### 3. Control Name
```xaml
<Button x:Name="SaveButton" Content="Save"/>
```

### Placement Options

Position callouts around target elements:

```csharp
public enum Placement
{
    TopLeft, TopCenter, TopRight,
    RightTop, RightCenter, RightBottom,
    BottomRight, BottomCenter, BottomLeft,
    LeftBottom, LeftCenter, LeftTop,
    Center
}
```

Set in XAML:
```xaml
<Button tour:TourHelper.Placement="BottomCenter"/>
```

Or per-step:
```csharp
new Step("MyButton", "Title", "Content") 
{ 
    Tag = Placement.RightCenter 
}
```

---

## 🎨 Callout Options

### Default Callout

Built-in, customizable callout:

```csharp
TourConfiguration.UseDefaultCallout();
```

### Telerik RadCallout

If you have Telerik UI for WPF:

1. Enable in `.csproj`:
```xml
<PropertyGroup>
  <UseTelerik>true</UseTelerik>
</PropertyGroup>
```

2. Switch at startup:
```csharp
TourConfiguration.UseTelerikCallout();
```

3. Switch at runtime:
```csharp
// In your UI
<Button Content="Use Telerik Callout" Command="{Binding CmdUseTelerikCallout}"/>

// In ViewModel
public ICommand CmdUseTelerikCallout => new RelayCommand(() => 
{
    TourConfiguration.UseTelerikCallout();
});
```

---

## 🔧 Advanced Features

### Custom ViewModel

```csharp
FeatureTour.SetViewModelFactoryMethod(tourRun => new MyCustomTourViewModel(tourRun));
```

### Custom Styling

```xaml
<Style TargetType="tour:TourControl">
    <Setter Property="Background" Value="#2D2D30"/>
    <Setter Property="Foreground" Value="White"/>
    <Setter Property="FontSize" Value="14"/>
    <Setter Property="CornerRadius" Value="8"/>
</Style>
```

### Custom Templates

```xaml
<Window.Resources>
    <DataTemplate x:Key="MyContentTemplate">
        <StackPanel>
            <Image Source="icon.png"/>
            <TextBlock Text="{Binding}" TextWrapping="Wrap"/>
        </StackPanel>
    </DataTemplate>
</Window.Resources>

<Button tour:TourHelper.ElementID="MyButton"
        tour:TourHelper.ContentDataTemplateKey="MyContentTemplate"/>
```

### Localization

```csharp
// Set text for buttons
TextLocalization.NextButtonText = "Weiter";
TextLocalization.CloseButtonText = "Schließen";
TextLocalization.DoItButtonText = "Mach es";
```

---

## 💡 Best Practices

### 1. Use Descriptive Element IDs
```csharp
// ✅ Good
public const string NewAccountButton = "NewAccountButton";

// ❌ Bad
public const string Button1 = "Button1";
```

### 2. Keep Tours Focused
Each tour should cover **one specific workflow**. Don't try to cover everything in one tour.

### 3. Test Tours with Preview
Always use the **"▶ Preview Tour"** button to test tours before integrating.

### 4. Use the Tour Recorder
For complex UIs, the recorder is **10x faster** than manual coding.

### 5. Organize Tours by Feature
```
Tours/
├── Onboarding/
│   └── IntroductionTour.cs
├── AccountManagement/
│   ├── CreateAccountTour.cs
│   └── EditAccountTour.cs
└── Reporting/
    └── GenerateReportTour.cs
```

### 6. Provide Context
Each step should explain **WHY**, not just **WHAT**:

```csharp
// ✅ Good
new Step("SaveButton", "Save Your Work", 
    "Click this button to save your changes. Your work will be preserved even if you close the app.")

// ❌ Bad
new Step("SaveButton", "Save Button", "This is the save button.")
```

---

## 🔍 Troubleshooting

### Tour Doesn't Start

**Problem:** Tour doesn't appear when called  
**Solutions:**
- ✅ Ensure all Element IDs exist in XAML
- ✅ Check Element IDs are case-sensitive matches
- ✅ Verify window is fully loaded before starting tour
- ✅ Confirm elements are visible (not collapsed/hidden)

### Steps Don't Appear

**Problem:** Some steps are skipped  
**Solutions:**
- ✅ Element might not be loaded yet
- ✅ Element might be in a different window/tab
- ✅ Check element is in the visual tree

### Preview Shows "Element Not Found"

**Problem:** Preview tour can't find elements  
**Solutions:**
- ✅ Element IDs in recording don't match XAML
- ✅ Edit the recorded step to fix the Element ID
- ✅ Or add the Element ID to your XAML

### Callout Positioning Issues

**Problem:** Callout appears in wrong position  
**Solutions:**
- ✅ Try different `Placement` values
- ✅ Ensure element has actual size (not collapsed)
- ✅ Check element is visible on screen

---

## 📚 API Reference

### Core Classes

- **`Tour`** - Defines a tour with steps
- **`Step`** - Individual tour step
- **`TourHelper`** - XAML attached properties
- **`FeatureTour`** - Main API entry point
- **`ITourRun`** - Running tour instance
- **`ITourNavigator`** - Tour navigation and actions

### Tour Recorder Classes

- **`TourRecorderWindow`** - Main recorder UI
- **`TourRecorder`** - Recording logic
- **`RecordedStep`** - Captured step data

### Configuration

- **`TourConfiguration`** - Global tour settings
- **`TextLocalization`** - Localized text
- **`Log`** - Logging configuration

### Full Documentation

Browse the [FeatureTour Wiki](https://github.com/JanDotNet/ThinkSharp.FeatureTour/wiki) for detailed API documentation:

- [Popup Placement](https://github.com/JanDotNet/ThinkSharp.FeatureTour/wiki/Popup-Placement)
- [Tour Definition](https://github.com/JanDotNet/ThinkSharp.FeatureTour/wiki/Tour-Definition)
- [Tour Navigation](https://github.com/JanDotNet/ThinkSharp.FeatureTour/wiki/Tour-Navigation)
- [Styling](https://github.com/JanDotNet/ThinkSharp.FeatureTour/wiki/Styling)
- [Templating](https://github.com/JanDotNet/ThinkSharp.FeatureTour/wiki/Templating)
- [Localization](https://github.com/JanDotNet/ThinkSharp.FeatureTour/wiki/Localization)
- [Logging](https://github.com/JanDotNet/ThinkSharp.FeatureTour/wiki/Logging)

---

## 📝 License

FeatureTour is released under [The MIT license (MIT)](LICENSE.TXT)

## 🏷️ Versioning

We use [SemVer](http://semver.org/) for versioning. For available versions, see the [tags on this repository](https://github.com/JanDotNet/ThinkSharp.FeatureTour/tags). 

## ☕ Donation

If you like FeatureTour and use it in your projects, feel free to buy me a cup of coffee! :) 

[![paypal](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=MSBFDUU5UUQZL)

---

## 🎓 Example: Complete Tour System

See the [Demo Application](https://github.com/JanDotNet/ThinkSharp.FeatureTour/tree/master/ThinkSharp.FeatureTour.Demo) for a complete working example showing:

- ✅ Tour Recorder in action
- ✅ Multiple tour definitions
- ✅ Callout switching (Default ↔ Telerik)
- ✅ Custom styling and templating
- ✅ Active vs passive tours
- ✅ Tours with dialogs
- ✅ Custom positioning

**Run the demo:**
```bash
git clone https://github.com/JanDotNet/ThinkSharp.FeatureTour.git
cd ThinkSharp.FeatureTour
dotnet run --project ThinkSharp.FeatureTour.Demo
```

---

**Made with ❤️ by the ThinkSharp team**
