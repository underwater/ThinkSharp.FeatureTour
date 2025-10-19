# Callout Control Factory

## Overview

ThinkSharp.FeatureTour now supports multiple callout control implementations through a factory pattern. You can choose between the default built-in callout or Telerik's RadCallout control.

## Available Implementations

### 1. Default Callout (Built-in)
- **Factory**: `DefaultCalloutControlFactory`
- **Requirements**: None (included by default)
- **Description**: The standard TourControl with custom polygon-based callout arrows

### 2. Telerik RadCallout
- **Factory**: `TelerikCalloutControlFactory`
- **Requirements**: 
  - Telerik UI for WPF license
  - Build with `TELERIK_AVAILABLE` compilation symbol
  - Set `UseTelerik=true` in project properties
- **Description**: Professional callout control from Telerik with extensive theming support

## Usage

### Basic Usage (Default Callout)

The default callout is used automatically - no configuration needed:

```csharp
var tour = new Tour
{
    Name = "My Tour",
    Steps = new[]
    {
        new Step(ElementID.Button1, "Welcome", "Click here to start"),
    }
};
tour.Start();
```

### Switching to Telerik Callout

In your application startup (e.g., `App.xaml.cs` or `MainWindow` constructor):

```csharp
using ThinkSharp.FeatureTouring;

// Switch to Telerik callout
TourConfiguration.UseTelerikCallout();

// Or manually set the factory
TourConfiguration.CalloutControlFactory = new TelerikCalloutControlFactory();
```

### Runtime Switching

You can switch implementations at runtime:

```csharp
// Switch to default
TourConfiguration.UseDefaultCallout();

// Switch to Telerik
try
{
    TourConfiguration.UseTelerikCallout();
}
catch (InvalidOperationException ex)
{
    MessageBox.Show("Telerik is not available: " + ex.Message);
}
```

## Building with Telerik Support

### Option 1: Using MSBuild Property

Build your project with the `UseTelerik` property:

```bash
dotnet build /p:UseTelerik=true
```

### Option 2: Adding to Project File

Add this to your `.csproj` file:

```xml
<PropertyGroup>
  <UseTelerik>true</UseTelerik>
</PropertyGroup>
```

### Option 3: Manual Configuration

1. Add Telerik package reference to your project
2. Add the `TELERIK_AVAILABLE` compilation symbol to your project properties:

```xml
<PropertyGroup>
  <DefineConstants>$(DefineConstants);TELERIK_AVAILABLE</DefineConstants>
</PropertyGroup>
```

## Creating Custom Callout Implementations

You can create your own callout implementations by implementing `ICalloutControlFactory`:

```csharp
public class MyCustomCalloutFactory : ICalloutControlFactory
{
    public FrameworkElement CreateCalloutControl()
    {
        return new MyCustomCalloutControl();
    }
    
    public string ImplementationName => "My Custom Callout";
}

// Use it:
TourConfiguration.CalloutControlFactory = new MyCustomCalloutFactory();
```

Your custom control should:
- Inherit from `Control` or `ContentControl`
- Bind to the `TourViewModel` through its `DataContext`
- Support the same data binding properties as `TourControl`

## Demo Application

The demo application includes buttons to switch between implementations at runtime. Try both to see the differences:

1. Run the demo application
2. Click "Use Default Callout" or "Use Telerik Callout" buttons
3. Start any tour to see the selected callout style

## Benefits

- **Flexibility**: Choose the callout that fits your application's design
- **Optional Dependencies**: Telerik is only required if you want to use it
- **Extensibility**: Create custom callout implementations for specific needs
- **Runtime Switching**: Change implementations without rebuilding (if assemblies are present)
- **Clean Architecture**: Factory pattern keeps code maintainable and testable

