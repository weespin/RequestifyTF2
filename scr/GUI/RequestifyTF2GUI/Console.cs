using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using RequestifyTF2Forms.Properties;

namespace RequestifyTF2Forms
{
    public partial class Console : Form
    {
        private readonly int _offsetX = 59;

        private readonly int _offsetY = 1;

        public Console()
        {
            InitializeComponent();
            Icon = Resources._1481916367_letter_r_red;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
        }

        private void Thanks_Load(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.None;
            var xs = Main.instance.Location.X + _offsetX + Main.instance.Height;
            var ys = Main.instance.Location.Y + _offsetY;
            ThreadHelperClass.Position(this, this, new Point(xs, ys));
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                while (true)
                {
                    if (!Main.ConsoleShowed)
                        continue;
                    try
                    {
                        if (Main.instance.Location.Y + _offsetY != Location.Y)
                        {
                            var y = Main.instance.Location.Y + _offsetY;
                            ThreadHelperClass.Position(this, this, new Point(Location.X, y));
                        }
                        if (Main.instance.Location.X + Main.instance.Height + _offsetX != Location.X)
                        {
                            var x = Main.instance.Location.X + _offsetX + Main.instance.Height;

                            ThreadHelperClass.Position(this, this, new Point(x, Location.Y));
                        }
                    }
                    catch (Exception)
                    {
                        //ignored
                    }
                    //16ms = 60fps
                    Thread.Sleep(16);
                }
            }).Start();
        }

        public static class ThreadHelperClass
        {
            /// <summary>
            ///     Set text property of various controls
            /// </summary>
            /// <param name="form">The calling form</param>
            /// <param name="ctrl"></param>
            /// <param name="pos"></param>
            public static void Position(Form form, Control ctrl, Point pos)
            {
                // InvokeRequired required compares the thread ID of the 
                // calling thread to the thread ID of the creating thread. 
                // If these threads are different, it returns true. 
                if (ctrl.InvokeRequired)
                {
                    SetPosCallback d = Position;
                    try
                    {
                        form.Invoke(d, form, ctrl, pos);
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
                else
                {
                    ctrl.Location = pos;
                }
            }

            private delegate void SetPosCallback(Form f, Control ctrl, Point p);
        }
    }
}