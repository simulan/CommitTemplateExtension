using Microsoft.VisualStudio.Imaging.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace CommitTemplateExtension.Utils
{
    public static class ImageUtils
    {
        public static BitmapSource ToBitMapSource(this ImageMoniker imageMoniker, int sizeX, int sizeY)
        {
            IVsImageService2 vsIconService = ServiceProvider.GlobalProvider.GetService(typeof(SVsImageService)) as IVsImageService2;

            if (vsIconService == null)
            {
                return null;
            }

            ImageAttributes imageAttributes = new ImageAttributes
            {
                Flags = (uint)_ImageAttributesFlags.IAF_RequiredFlags,
                ImageType = (uint)_UIImageType.IT_Bitmap,
                Format = (uint)_UIDataFormat.DF_WPF,
                LogicalHeight = sizeY,
                LogicalWidth = sizeX,
                StructSize = Marshal.SizeOf(typeof(ImageAttributes))
            };

            IVsUIObject result = vsIconService.GetImage(imageMoniker, imageAttributes);

            object data;
            result.get_Data(out data);
            BitmapSource glyph = data as BitmapSource;

            if (glyph != null)
            {
                glyph.Freeze();
            }

            return glyph;
        }

    }
}
