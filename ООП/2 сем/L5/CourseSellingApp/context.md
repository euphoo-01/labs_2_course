# Project Context: Course Selling App (Avalonia UI)

## Overview
This is a cross-platform desktop application built with **Avalonia UI** (v11.3.12) and **.NET 10**, strictly adhering to the **MVVM** (Model-View-ViewModel) architecture via **ReactiveUI**. The application simulates a platform for selling courses, featuring two user roles: Administrator and Client.

## Core Technologies
- **C# 12 / .NET 10**
- **Avalonia UI** for cross-platform UI rendering.
- **ReactiveUI** for MVVM implementation (Commands, Interactions, Observables).
- **Avalonia.Xaml.Behaviors** for UI interactivity (Triggers and Behaviors).
- **Newtonsoft.Json** for local data persistence.

## Key Features & Implementations
1. **Localization**: 
   - Supports English and Russian. 
   - Strings are kept in Resources/Strings/ as Resource Dictionaries.
   - LocalizationManager handles dynamic replacement of the active dictionary and triggers a LanguageChanged event.
2. **Theming**: 
   - Supports 4 dynamic themes: Optimistic, Pink, Grayscale, Dark.
   - Colors/Brushes are stored in Resources/Themes/.
   - ThemeManager dynamically loads and unloads the selected dictionary.
3. **Styling & Control Templates**: 
   - Centralized application styles in Resources/AppStyles.axaml.
   - Custom ControlTemplate applied to Button to provide rounded corners.
   - Styles defined for ComboBox, ListBox, TextBox, NumericUpDown, and CheckBox to guarantee high contrast and theme responsiveness. 
   - Note: ComboBox uses PlaceholderForeground DynamicResource AppForegroundBrush for placeholder styling.
4. **User Control & Triggers**:
   - AnimatedCourseCard.axaml is a custom UserControl used in the main ListBox.
   - Includes EventTriggerBehavior hover color changes and DataTriggerBehavior opacity changes based on course availability/quantity.
5. **Profile System & Data Persistence**:
   - ProfileView / ProfileViewModel allow the user to change their name, email, language, and theme.
   - Settings are serialized locally to user.json via UserSettingsService. Settings are automatically loaded on app launch.
6. **Undo/Redo System**:
   - Built via the Command Pattern.
   - ICommand interface ExecuteAsync, UndoAsync.
   - UndoRedoManager orchestrates the Stacks for Redo and Undo operations.
   - Implemented for Adding, Editing, and Deleting courses.

## Project Structure Overview
- Models/: Course.cs, UserSettings.cs.
- ViewModels/: 
  - ViewModelBase.cs: Base class inheriting from ReactiveObject.
  - MainWindowViewModel.cs: Core logic, filters, categories, and Undo/Redo tracking.
  - EditCourseViewModel.cs: Editing logic, file drag-and-drop for images.
  - ProfileViewModel.cs: User settings state handling.
- Views/: 
  - MainWindow.axaml: Uses DockPanel and Grid layouts. Contains the Toolbar for Admin actions.
  - EditCourseView.axaml: The course modification dialog.
  - ProfileView.axaml: The user settings dialog.
  - CustomControls/AnimatedCourseCard.axaml: Item template for the courses list.
- Services/:
  - CourseService.cs: Mock database/CRUD logic.
  - LocalizationManager.cs, ThemeManager.cs: ResourceDictionary hot-swapping.
  - UndoRedoManager.cs, CourseCommands.cs: Command pattern implementation.
  - UserSettingsService.cs: JSON IO for settings.
- Resources/:
  - Contains .axaml files for global AppStyles, Strings, and Themes.

## Current Application State
- The project successfully compiles and runs.
- Task 8 from task.md has been fully completed.
- The UI properly recalculates and reapplies dynamic resources for Themes and Localization without restarting the application.

## Notes for Future Development (Agent Context)
1. Dynamic Resources: Always use DynamicResource BrushName defined in the themes when styling new UI components. Hardcoded colors should be avoided.
2. Dialogs: View/ViewModel separation for dialogs is handled via ReactiveUI.Interaction. See MainWindow.axaml.cs for interaction registration.
3. Observables: ReactiveUI this.WhenAnyValue is used extensively to re-evaluate command execution states.
