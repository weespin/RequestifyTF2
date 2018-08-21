// RequestifyTF2GUIOld(unsupported)
// Copyright (C) 2018  Villiam Nmerukini
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using MaterialSkin.Controls;

using RequestifyTF2GUIOld.Properties;

namespace RequestifyTF2Forms
{
    public partial class Console : MaterialForm
    {
        private readonly int _offsetX = 51;

        private const int _offsetY = 0;

        public Console()
        {
            InitializeComponent();
            Icon = Resources.Icon;
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
            new Thread(
                () =>
                {
                    Thread.CurrentThread.IsBackground = true;

                    while (true)
                    {
                        Thread.Sleep(1);
                        if (!Main.ConsoleShowed)
                        {
                            Thread.Sleep(200);
                            continue;
                        }

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
                            // ignored
                        }

                        // 16ms = 60fps
                    }
                }).Start();
        }

      

        public static class ThreadHelperClass
        {
            public static void MiniMaxi(Form form, FormWindowState state)
            {
                // InvokeRequired required compares the thread ID of the 
                // calling thread to the thread ID of the creating thread. 
                // If these threads are different, it returns true. 
                if (form.InvokeRequired)
                {
                    SetFormstateCallback d = MiniMaxi;
                    try
                    {
                        form.Invoke(d, form, state);
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
                else
                {
                    form.WindowState = state;
                }
            }

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

            private delegate void SetFormstateCallback(Form f, FormWindowState state);

            private delegate void SetPosCallback(Form f, Control ctrl, Point p);
        }
    }
}