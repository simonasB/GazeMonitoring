using Ookii.Dialogs.Wpf;

namespace GazeMonitoring.IO
{
    public interface IFolderDialogService
    {
        string OpenFolderDialog();
    }

    public class FolderDialogService : IFolderDialogService
    {
        public string OpenFolderDialog()
        {
            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();
            dialog.Description = "Please select a folder.";
            dialog.UseDescriptionForTitle = true; // This applies to the Vista style dialog only, not the old dialog.

            return dialog.ShowDialog() == true ? dialog.SelectedPath : null;
        }
    }
}
