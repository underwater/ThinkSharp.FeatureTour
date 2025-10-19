# Comprehensive Rename Plan: LedgerVue → LedgerVue

## Overview
Rename all instances of "LedgerVue" to "LedgerVue" throughout the entire solution, including:
- Project names and folders
- Namespaces
- Assembly names
- Solution files
- All code references
- Documentation

---

## Phase 1: Folder and File Renames

### 1.1 Project Folders
```
LedgerVue.FeatureTour/              → LedgerVue.FeatureTour/
LedgerVue.FeatureTour.Demo/         → LedgerVue.FeatureTour.Demo/
LedgerVue.FeatureTour.Test/         → LedgerVue.FeatureTour.Test/
```

### 1.2 Project Files (.csproj)
```
LedgerVue.FeatureTour.csproj        → LedgerVue.FeatureTour.csproj
LedgerVue.FeatureTour.Demo.csproj   → LedgerVue.FeatureTour.Demo.csproj
LedgerVue.FeatureTour.Test.csproj   → LedgerVue.FeatureTour.Test.csproj
```

### 1.3 Solution Files
```
LedgerVue.FeatureTour.sln           → LedgerVue.FeatureTour.sln
LedgerVue.FeatureTour.Demo.sln      → LedgerVue.FeatureTour.Demo.sln
```

### 1.4 NuGet Package Specification
```
LedgerVue.FeatureTour.nuspec        → LedgerVue.FeatureTour.nuspec
```

### 1.5 Documentation Files
```
README.md
README_CALLOUT_FACTORY.md
LedgerVue_OverylayTour_Integration.md
```

---

## Phase 2: Project File Updates

### 2.1 Update LedgerVue.FeatureTour.csproj
```xml
<!-- Change -->
<RootNamespace>LedgerVue.FeatureTouring</RootNamespace>
<AssemblyName>LedgerVue.FeatureTour</AssemblyName>

<!-- To -->
<RootNamespace>LedgerVue.FeatureTouring</RootNamespace>
<AssemblyName>LedgerVue.FeatureTour</AssemblyName>
```

### 2.2 Update LedgerVue.FeatureTour.Demo.csproj
```xml
<!-- Change -->
<RootNamespace>LedgerVue.FeatureTouring</RootNamespace>
<AssemblyName>LedgerVue.FeatureTouring.Demo</AssemblyName>

<!-- To -->
<RootNamespace>LedgerVue.FeatureTouring</RootNamespace>
<AssemblyName>LedgerVue.FeatureTouring.Demo</AssemblyName>
```

### 2.3 Update LedgerVue.FeatureTour.Test.csproj
```xml
<!-- Change -->
<RootNamespace>LedgerVue.FeatureTouring.Test</RootNamespace>
<AssemblyName>LedgerVue.FeatureTour.Test</AssemblyName>

<!-- To -->
<RootNamespace>LedgerVue.FeatureTouring.Test</RootNamespace>
<AssemblyName>LedgerVue.FeatureTour.Test</AssemblyName>
```

### 2.4 Update Project References
In all .csproj files, update:
```xml
<ProjectReference Include="..\LedgerVue.FeatureTour\LedgerVue.FeatureTour.csproj" />
<!-- To -->
<ProjectReference Include="..\LedgerVue.FeatureTour\LedgerVue.FeatureTour.csproj" />
```

---

## Phase 3: Solution File Updates

### 3.1 Update LedgerVue.FeatureTour.sln
Replace all instances:
- `LedgerVue.FeatureTour` → `LedgerVue.FeatureTour`
- Project paths
- Project GUIDs references
- Configuration sections

### 3.2 Update LedgerVue.FeatureTour.Demo.sln
Same as above

---

## Phase 4: C# Namespace Updates (62 files)

### 4.1 Namespace Declarations
In all `.cs` files, replace:
```csharp
namespace LedgerVue.FeatureTouring
// With
namespace LedgerVue.FeatureTouring
```

Pattern variations to replace:
- `namespace LedgerVue.FeatureTouring`
- `namespace LedgerVue.FeatureTouring.Controls`
- `namespace LedgerVue.FeatureTouring.Helper`
- `namespace LedgerVue.FeatureTouring.Logging`
- `namespace LedgerVue.FeatureTouring.Models`
- `namespace LedgerVue.FeatureTouring.Navigation`
- `namespace LedgerVue.FeatureTouring.Recording`
- `namespace LedgerVue.FeatureTouring.Touring`
- `namespace LedgerVue.FeatureTouring.ViewModels`
- `namespace LedgerVue.FeatureTouring.Test`

### 4.2 Using Statements
In all `.cs` files, replace:
```csharp
using LedgerVue.FeatureTouring;
using LedgerVue.FeatureTouring.Controls;
using LedgerVue.FeatureTouring.Helper;
using LedgerVue.FeatureTouring.Logging;
using LedgerVue.FeatureTouring.Models;
using LedgerVue.FeatureTouring.Navigation;
using LedgerVue.FeatureTouring.Recording;
using LedgerVue.FeatureTouring.Touring;
using LedgerVue.FeatureTouring.ViewModels;

// With LedgerVue equivalents
using LedgerVue.FeatureTouring;
using LedgerVue.FeatureTouring.Controls;
// ... etc
```

### 4.3 Special Files to Update

**Properties/AssemblyInfo.cs** (3 files)
- Update assembly title, product, description
- Replace "LedgerVue.FeatureTour" → "LedgerVue.FeatureTour"

**Properties/Resources.Designer.cs**
- Update namespace
- Update resource class names

**Properties/Settings.Designer.cs**
- Update namespace

---

## Phase 5: XAML Namespace Updates (10 files)

### 5.1 XAML Namespace Declarations
In all `.xaml` files, replace:
```xml
xmlns:featureTouring="clr-namespace:LedgerVue.FeatureTouring;assembly=LedgerVue.FeatureTour"
<!-- With -->
xmlns:featureTouring="clr-namespace:LedgerVue.FeatureTouring;assembly=LedgerVue.FeatureTour"

xmlns:app="clr-namespace:LedgerVue.FeatureTouring"
<!-- With -->
xmlns:app="clr-namespace:LedgerVue.FeatureTouring"

xmlns:tour="clr-namespace:LedgerVue.FeatureTouring;assembly=LedgerVue.FeatureTour"
<!-- With -->
xmlns:tour="clr-namespace:LedgerVue.FeatureTouring;assembly=LedgerVue.FeatureTour"
```

### 5.2 XAML Class Names
```xml
x:Class="LedgerVue.FeatureTouring.MainWindow"
<!-- To -->
x:Class="LedgerVue.FeatureTouring.MainWindow"
```

### 5.3 Files to Update
- App.xaml
- MainWindow.xaml
- Dialog.xaml
- TourViews.xaml
- ExampleTheme.xaml
- OrginalTheme.xaml
- Generic.xaml
- TelerikCallout.xaml
- RecordStepDialog.xaml
- TourRecorderWindow.xaml

---

## Phase 6: NuGet Package Specification

### 6.1 Update LedgerVue.FeatureTour.nuspec
```xml
<!-- Change -->
<id>LedgerVue.FeatureTour</id>
<title>LedgerVue.FeatureTour</title>
<file src="bin\Release\net8.0-windows\LedgerVue.FeatureTour.dll" 
      target="lib\net8.0-windows\LedgerVue.FeatureTour.dll" />
<file src="bin\Release\net8.0-windows\LedgerVue.FeatureTour.xml" 
      target="lib\net8.0-windows\LedgerVue.FeatureTour.xml" />

<!-- To -->
<id>LedgerVue.FeatureTour</id>
<title>LedgerVue.FeatureTour</title>
<file src="bin\Release\net8.0-windows\LedgerVue.FeatureTour.dll" 
      target="lib\net8.0-windows\LedgerVue.FeatureTour.dll" />
<file src="bin\Release\net8.0-windows\LedgerVue.FeatureTour.xml" 
      target="lib\net8.0-windows\LedgerVue.FeatureTour.xml" />
```

---

## Phase 7: Documentation Updates

### 7.1 README.md
- Update title
- Update package name references
- Update code examples with new namespaces
- Update installation instructions

### 7.2 README_CALLOUT_FACTORY.md
- Update namespace references
- Update code examples

### 7.3 LedgerVue_OverylayTour_Integration.md
- Already references LedgerVue correctly
- Update any LedgerVue.FeatureTour references to LedgerVue.FeatureTour

### 7.4 Obsolete Plan Files
- Rename to: `integrate-ledgervue-featuretour.plan.md`
- Update all project references

---

## Phase 8: Special Considerations

### 8.1 Binary/Output Folders
These will be automatically regenerated:
- bin/
- obj/
- TestResults/

### 8.2 License and Copyright
Update if needed:
- LICENSE.TXT (keep author, but note fork/derivation if applicable)
- Copyright notices in AssemblyInfo.cs files

### 8.3 Git History
After rename:
```bash
git add -A
git commit -m "Rename LedgerVue → LedgerVue across entire solution"
```

---

## Complete File List (85 files to process)

### C# Files (62)
All files in:
- LedgerVue.FeatureTour/ (42 files)
- LedgerVue.FeatureTour.Demo/ (14 files)
- LedgerVue.FeatureTour.Test/ (6 files)

### XAML Files (10)
- App.xaml
- MainWindow.xaml
- Dialog.xaml
- TourViews.xaml
- ExampleTheme.xaml
- OrginalTheme.xaml
- Generic.xaml
- TelerikCallout.xaml
- RecordStepDialog.xaml
- TourRecorderWindow.xaml

### Project Files (3)
- LedgerVue.FeatureTour.csproj
- LedgerVue.FeatureTour.Demo.csproj
- LedgerVue.FeatureTour.Test.csproj

### Solution Files (2)
- LedgerVue.FeatureTour.sln
- LedgerVue.FeatureTour.Demo.sln

### Package/Documentation (4)
- LedgerVue.FeatureTour.nuspec
- README.md
- README_CALLOUT_FACTORY.md
- LedgerVue_OverylayTour_Integration.md

---

## Execution Order

1. ✅ Create backup/commit current state
2. ✅ Rename project folders
3. ✅ Rename project files (.csproj)
4. ✅ Rename solution files (.sln)
5. ✅ Update solution file contents
6. ✅ Update .csproj file contents (RootNamespace, AssemblyName, ProjectReference)
7. ✅ Update all C# namespaces (62 files)
8. ✅ Update all C# using statements (62 files)
9. ✅ Update all XAML namespaces (10 files)
10. ✅ Rename and update .nuspec file
11. ✅ Update documentation (3-4 .md files)
12. ✅ Build and test

---

## Validation Checklist

- [ ] All project folders renamed
- [ ] All .csproj files renamed and updated
- [ ] Both .sln files updated
- [ ] All 62 C# files have updated namespaces
- [ ] All 10 XAML files have updated namespaces
- [ ] NuGet spec file updated
- [ ] Documentation updated
- [ ] Solution builds without errors
- [ ] Demo application runs
- [ ] Unit tests pass
- [ ] No "LedgerVue" references remain (except in LICENSE/credits)

---

## Search & Replace Patterns

### For C# Files:
```
Find: namespace LedgerVue\.FeatureTouring
Replace: namespace LedgerVue.FeatureTouring

Find: using LedgerVue\.FeatureTouring
Replace: using LedgerVue.FeatureTouring
```

### For XAML Files:
```
Find: clr-namespace:LedgerVue\.FeatureTouring
Replace: clr-namespace:LedgerVue.FeatureTouring

Find: assembly=LedgerVue\.FeatureTour
Replace: assembly=LedgerVue.FeatureTour

Find: x:Class="LedgerVue\.FeatureTouring
Replace: x:Class="LedgerVue.FeatureTouring
```

### For Project Files:
```
Find: LedgerVue\.FeatureTour
Replace: LedgerVue.FeatureTour

Find: LedgerVue\.FeatureTouring
Replace: LedgerVue.FeatureTouring
```

### For Documentation:
```
Find: LedgerVue\.FeatureTour
Replace: LedgerVue.FeatureTour

Find: LedgerVue\.FeatureTouring
Replace: LedgerVue.FeatureTouring
```

---

## Estimated Time: 30-45 minutes

This is a comprehensive rename affecting 85+ files and requires careful attention to maintain build integrity.

