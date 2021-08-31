using System;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows;

namespace MouseDragScrollViewer
{
    /// <summary>
    /// 
    /// </summary>
    public class MouseUtilities
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct Win32Point
        {
            public Int32 X;
            public Int32 Y;
        };

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(ref Win32Point pt);

        [DllImport("user32.dll")]
        private static extern bool ScreenToClient(IntPtr hwnd, ref Win32Point pt);

        public static Point GetMousePosition(Visual relativeTo)
        {
            Win32Point mouse = new Win32Point();
            GetCursorPos(ref mouse);

            System.Windows.Interop.HwndSource presentationSource =
                (System.Windows.Interop.HwndSource)PresentationSource.FromVisual(relativeTo);

            ScreenToClient(presentationSource.Handle, ref mouse);

            GeneralTransform transform = relativeTo.TransformToAncestor(presentationSource.RootVisual);

            Point offset = transform.Transform(new Point(0, 0));
// 
//             Point p = new Point(mouse.X - offset.X, mouse.Y - offset.Y);
//             System.Diagnostics.Debug.WriteLine(string.Format("mouse {0:0.0}|{1:0.0} offset {2:0.0}|{3:0.0} res {4:0.0}|{5:0.0}",
//                 mouse.X, mouse.Y, offset.X, offset.Y, p.X, p.Y));

            return new Point(mouse.X - offset.X, mouse.Y - offset.Y);
        }
    }
}
