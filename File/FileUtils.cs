using Community.VisualStudio.Toolkit;
using System.Windows.Forms;
using Solution = Community.VisualStudio.Toolkit.Solution;

namespace CommitTemplateExtension.File
{
    public static class FileUtils
    {
        public static string GetBaseDir()
        {
            Solution solution = VS.Solutions.GetCurrentSolutionAsync().Result;
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
