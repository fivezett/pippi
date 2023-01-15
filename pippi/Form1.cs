using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace pippi
{
    public partial class Form1 : Form
    {

        //==========================
        //Win32API Import START
        //==========================

        //RECT 
        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        private enum SWP : int
        {
            NOSIZE = 0x0001,
            NOMOVE = 0x0002,
            NOZORDER = 0x0004,
            NOREDRAW = 0x0008,
            NOACTIVATE = 0x0010,
            FRAMECHANGED = 0x0020,
            SHOWWINDOW = 0x0040,
            HIDEWINDOW = 0x0080,
            NOCOPYBITS = 0x0100,
            NOOWNERZORDER = 0x0200,
            NOSENDCHANGING = 0x400
        }
        private enum GWL : int
        {
            WINDPROC = -4,
            HINSTANCE = -6,
            HWNDPARENT = -8,
            ID = -12,
            STYLE = -16,
            EXSTYLE = -20,
            USERDATA = -21,
        }

        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOMOVE = 0x0002;
        private const int SWP_SHOWWINDOW = 0x0040;

        private const int HWND_TOPMOST = -1;
        private const int HWND_NOTOPMOST = -2;

        private const int SRCCOPY = 13369376;
        private const int DWMWA_EXTENDED_FRAME_BOUNDS = 9;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int EnumWindows(EnumWindowsDelegate lpEnumFunc, IntPtr lparam);
        public delegate bool EnumWindowsDelegate(IntPtr hWnd, IntPtr lparam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int index);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int index, int unValue);

        [DllImport("user32")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        internal struct LASTINPUTINFO
        {
            public uint cbSize;
            public uint dwTime;
        }

        Dictionary<int, IntPtr> windowList = new Dictionary<int, IntPtr>();

        public Form1()
        {
            InitializeComponent();
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            IntPtr target = windowList[windowComboBox.SelectedIndex];
        }

        private void windowSelectorFocus(object sender, EventArgs e)
        {
            windowComboBox.Items.Clear();
            windowList.Clear();
            EnumWindows(new EnumWindowsDelegate(EnumWindowCallBack), IntPtr.Zero);
        }

        private bool EnumWindowCallBack(IntPtr hWnd, IntPtr lparam)
        {
            //https://www.natsuneko.blog/entry/2018/08/09/002742
            //不可視化ウィンドウ除外
            if (!IsWindowVisible(hWnd))
                return true;

            // タイトル空
            var title = new StringBuilder(1024);
            GetWindowText(hWnd, title, title.Capacity);
            if (string.IsNullOrWhiteSpace(title.ToString()))
                return true; // Skipped

            int textLen = GetWindowTextLength(hWnd);
            if (0 < textLen)
            {
                StringBuilder tsb = new StringBuilder(textLen + 1);
                GetWindowText(hWnd, tsb, tsb.Capacity);

                windowComboBox.Items.Add(tsb.ToString());
                windowList.Add(windowList.Count, hWnd);
            }

            //すべてのウィンドウを列挙する
            return true;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (windowList.Count == 0) return;
            IntPtr hWnd;
            try
            {
                hWnd = windowList[windowComboBox.SelectedIndex];
            }
            catch (Exception)
            {
                return;
            }
            SetWindowLong(hWnd, -16, GetWindowLong(hWnd, -16) & ~(0x00800000 | 0x00400000));
            SetWindowPos(hWnd, -1, 0, 0, 0, 0, 0x0020| 0x0002 | 0x0001);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (windowList.Count == 0) return;
            IntPtr hWnd;
            try
            {
                hWnd = windowList[windowComboBox.SelectedIndex];
            }
            catch (Exception)
            {
                return;
            }
            SetWindowLong(hWnd, -16,0x14CF0000);
            SetWindowPos(hWnd,-2, 0, 0, 0, 0, 0x0020 | 0x0002 | 0x0001);
        }
    }
}
