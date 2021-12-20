using Community.VisualStudio.Toolkit;
using Microsoft.VisualStudio.Imaging.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Solution = Community.VisualStudio.Toolkit.Solution;

namespace CommitTemplateExtension.Utils
{
    public static class FileUtils
    {
   
        public static async Task<string> GetBaseDir()
        {
            Solution solution = await VS.Solutions.GetCurrentSolutionAsync();
            if (solution != null)
                return System.IO.Path.GetDirectoryName(solution.FullPath);
            else
                return null;
        }

        public static string GetDirFromUser()
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    return fbd.SelectedPath;
                else
                    return null;
            }
        }
    }
}
