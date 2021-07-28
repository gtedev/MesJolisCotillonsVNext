using System;
using System.ComponentModel.Design;
using MesJolisCotillonsOperationGeneratorExtension;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;

namespace MesJolisCotillonsExtension
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class CreateOperationCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("bfeeb36a-113c-41f7-ac4e-1423e0d6bf57");

        public static IVsUIShell UiShell;

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        private CreateNewOperationVisualStudioMainWindow CreateNewOperationWindow;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOperationCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private CreateOperationCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static CreateOperationCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in TestCommand's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;

            Instance = new CreateOperationCommand(package, commandService);

            IVsOutputWindow output = await package.GetServiceAsync(typeof(IVsOutputWindow)) as IVsOutputWindow;
            IVsOutputWindowPane pane;

            // Create a new pane.
            output.CreatePane(
                ref MesJolisCotillonsExtensionConstants.MesJolisCotillonsPaneGuid,
                "Mes Jolis Cotillons",
                Convert.ToInt32(true),
                Convert.ToInt32(true));
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Execute(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
#if (!DEBUG)
            var dte = Package.GetGlobalService(typeof(SDTE)) as DTE2;
            string fullName = dte.Solution.FullName;
            var isMesJolisCotillonsVNext = false;

            foreach (EnvDTE.Project project in dte.Solution.Projects)
            {
                if (project.Name == "MesJolisCotillons.VNext")
                {
                    isMesJolisCotillonsVNext = true;
                    break;
                }
            }

            if (!isMesJolisCotillonsVNext && dte.Solution.IsOpen)
            {
                string message = "You need to be run under MesJolisCotillons solution to run that operation";
                string title = "MesJolisCotillons Extension";

                VsShellUtilities.ShowMessageBox(
                this.package,
                message,
                title,
                OLEMSGICON.OLEMSGICON_WARNING,
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
                return;
            }
#endif

            if (this.CreateNewOperationWindow == null)
            {
                this.InitCreateOperationWindow();
            }

            var taskOne = ServiceProvider.GetServiceAsync(typeof(SVsUIShell));
            IVsUIShell uiShell1 = (IVsUIShell)taskOne.GetAwaiter().GetResult();
            IntPtr hwnd;
            uiShell1.GetDialogOwnerHwnd(out hwnd);

            try
            {
                WindowHelper.ShowModal(this.CreateNewOperationWindow, hwnd);
            }
            finally
            {
                // This will take place after the window is closed.
                uiShell1.EnableModeless(1);
            }
        }

        private void InitCreateOperationWindow()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var taskOne = ServiceProvider.GetServiceAsync(typeof(SVsUIShell));
            IVsUIShell5 uiShell2 = (IVsUIShell5)taskOne.GetAwaiter().GetResult();

            this.CreateNewOperationWindow = new CreateNewOperationVisualStudioMainWindow("Microsoft.VisualStudio.PlatformUI.DialogWindow", uiShell2);
            this.CreateNewOperationWindow.HasMinimizeButton = false;
            this.CreateNewOperationWindow.HasMaximizeButton = false;
            this.CreateNewOperationWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            this.CreateNewOperationWindow.Title = "Mes Jolis Cotillons - Create new Operation";
        }
    }
}
