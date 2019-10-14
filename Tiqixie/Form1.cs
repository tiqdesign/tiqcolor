using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tiqixie
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private  void Form1_Load(object sender, EventArgs e)
        {
            Task bul = new Task(() =>
            {
                while (true)
                {
                    Point cursor = new Point();
                    GetCursorPos(ref cursor);

                    var c = GetColorAt(cursor);
                    pn_color.BackColor = c;

                    this.Invoke(new EventHandler(delegate
                    {
                        lb_hex.Text = ColorTranslator.ToHtml(c);

                    }));

                   
                }
            });
             bul.Start();
        }


        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point lpPoint);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        Bitmap screenPixel = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
        public Color GetColorAt(Point location)
        {
            using (Graphics gdest = Graphics.FromImage(screenPixel))
            {
                using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero))
                {
                    IntPtr hSrcDC = gsrc.GetHdc();
                    IntPtr hDC = gdest.GetHdc();
                    int retval = BitBlt(hDC, 0, 0, 1, 1, hSrcDC, location.X, location.Y, (int)CopyPixelOperation.SourceCopy);
                    gdest.ReleaseHdc();
                    gsrc.ReleaseHdc();
                }
            }
            return screenPixel.GetPixel(0, 0);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
                if (e.Shift && e.KeyCode == Keys.C)
                {
                    Clipboard.SetText(lb_hex.Text);
                }
        }
    }
}
