using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;

namespace CommitTemplateExtension
{
    [Guid("13e95dcc-deb1-4478-b07a-b057eeba467d")]
    public class CommitTemplateToolWindow : ToolWindowPane
    {
        public CommitTemplateToolWindow() : base(null)
        {
            this.Caption = "Commit Template";
            this.BitmapImageMoniker = KnownMonikers.GitNoColor;
            this.Content = new CommitTemplateToolWindowControl();
        }
    }
}
