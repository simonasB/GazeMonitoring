namespace GazeMonitoring.IO
{
    public interface IFileDialogService
    {
        string OpenFileDialog();
    }

    public class FileDialogService : IFileDialogService
    {
        public string OpenFileDialog()
        {
            var fileDialog = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".pptx", Filter = "PPTX Files (*.pptx)|*.pptx|PPT Files (*.ppt)|*.ppt"
            };

            var result = fileDialog.ShowDialog();

            return result == true ? fileDialog.FileName : null;
        }
    }
}
