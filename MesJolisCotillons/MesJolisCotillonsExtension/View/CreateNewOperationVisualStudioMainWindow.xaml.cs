using MesJolisCotillonsOperationGeneratorExtension;
using MesJolisCotillonsOperationGeneratorExtension.Model;
using Microsoft.FSharp.Core;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace MesJolisCotillonsExtension
{
    /// <summary>
    /// Interaction logic for CreateNewOperationMainWindow.xaml
    /// </summary>
    public partial class CreateNewOperationVisualStudioMainWindow : DialogWindow
    {
        public CreateNewOperationVisualStudioMainWindow(string helpTopic, IVsUIShell5 uiShell) 
            : base(helpTopic)
        {
            InitializeComponent();
            this.SetupWindowTheme(uiShell);
        }

        private void SetupWindowTheme(IVsUIShell5 uiShell)
        {
            var color = VsColors.GetThemedWPFColor(uiShell, EnvironmentColors.NewProjectBackgroundColorKey);
            this.Background = new SolidColorBrush(color);

            var colorText = VsColors.GetThemedWPFColor(uiShell, EnvironmentColors.PanelTextColorKey);
            var brushColorText = new SolidColorBrush(colorText);

            CreateOperationTitle.Foreground = brushColorText;
            OperationNameLabel.Foreground = brushColorText;
            ValidationCheckboxLabel.Foreground = brushColorText;
            ErrorlabelMessage.Foreground = brushColorText;
            RelativeFolderPathLabel.Foreground = brushColorText;
            RadioCustomResponseBuilder.Foreground = brushColorText;
            RadioDefaultResponseBuilder.Foreground = brushColorText;
            ResponseBuilderLabel.Foreground = brushColorText;
            ExecutorCheckBoxLabel.Foreground = brushColorText;
            AdapterCheckboxLabel.Foreground = brushColorText;            
        }

        public CreateNewOperationVisualStudioMainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Very simple validation form...no fancy MVVM stuff used for the moment...
            if (this.IsFormValid())
            {
                this.IsEnabled = false;

                // Call F# dll to generate classes
                this.WriteIntoOutputConsole("Starting generating classes..." + Environment.NewLine + Environment.NewLine);
                var model = this.BuildModel();

                Unit logger(string loggingMessage)
                {
                    this.WriteIntoOutputConsole(loggingMessage);
                    // return explicitely a Unit to match Fsharp type
                    return (Unit)Activator.CreateInstance(typeof(Unit), true);
                }

                // wrap the logger into fsharp function
                var fsharpLogger = Microsoft.FSharp.Core.FSharpFunc<string, Unit>.FromConverter(new Converter<string, Unit>(logger));

                var command = new Types.GenerateFilesCommand
                    (
                     model.OperationName,
                     model.RelativePathFolder,
                     model.HasAdapter,
                     model.HasValidations,
                     model.HasExecutor,
                     model.UseCustomResponseBuilder
                    );

                OperationGenerator.GenerateOperation.generateAllOperationClasses(
                    command,
                    fsharpLogger);

                this.WriteIntoOutputConsole(Environment.NewLine + Environment.NewLine + "Operation ended with success..." + Environment.NewLine + Environment.NewLine);
                this.WriteIntoOutputConsole("==========    Generate file classes END    ===================" + Environment.NewLine);
                this.IsEnabled = true;
            }
        }

        private OperationGenerationModel BuildModel()
        {
            return new OperationGenerationModel
            {
                OperationName = OperationNameTextBox.Text,
                RelativePathFolder = RelativeFolderTextBox.Text,
                HasAdapter = AdapterCheckBox?.IsChecked ?? false,
                HasValidations = ValidationCheckBox?.IsChecked ?? false,
                HasExecutor = ExecutorCheckBox?.IsChecked ?? false,
                UseCustomResponseBuilder = RadioCustomResponseBuilder?.IsChecked ?? false,
            };
        }

        private bool IsFormValid()
        {
            bool isFormValid = true;
            var brushConverter = new BrushConverter();
            ErrorlabelMessage.Visibility = Visibility.Hidden;
            OperationNameTextBox.BorderBrush = brushConverter.ConvertFromString("#FFABADB3") as Brush;
            RelativeFolderTextBox.BorderBrush = brushConverter.ConvertFromString("#FFABADB3") as Brush;

            if (string.IsNullOrEmpty(OperationNameTextBox.Text))
            {
                OperationNameTextBox.BorderBrush = Brushes.Red;
                ErrorlabelMessage.Visibility = Visibility.Visible;
                isFormValid = false;
            }

            if (string.IsNullOrEmpty(RelativeFolderTextBox.Text))
            {
                RelativeFolderTextBox.BorderBrush = Brushes.Red;
                ErrorlabelMessage.Visibility = Visibility.Visible;
                isFormValid = false;
            }

            return isFormValid;
        }

        private void ResetForm()
        {
            var brushConverter = new BrushConverter();
            OperationNameTextBox.BorderBrush = brushConverter.ConvertFromString("#FFABADB3") as Brush;
            RelativeFolderTextBox.BorderBrush = brushConverter.ConvertFromString("#FFABADB3") as Brush;

            OperationNameTextBox.Text = string.Empty;
            RelativeFolderTextBox.Text = string.Empty;
            ValidationCheckBox.IsChecked = false;
            ErrorlabelMessage.Visibility = Visibility.Hidden;
            RadioCustomResponseBuilder.IsChecked = true;
        }

        private void WriteIntoOutputConsole(string text)
        {
            IVsOutputWindow outWindow = Package.GetGlobalService(typeof(SVsOutputWindow)) as IVsOutputWindow;

            IVsOutputWindowPane outputPane;
            outWindow.GetPane(ref MesJolisCotillonsExtensionConstants.MesJolisCotillonsPaneGuid, out outputPane);

            outputPane.OutputString(text);
            outputPane.Activate();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            this.ResetForm();
            e.Cancel = true;  // cancels the window close    
            this.Hide();      // Programmatically hides the window
        }

    }
}
