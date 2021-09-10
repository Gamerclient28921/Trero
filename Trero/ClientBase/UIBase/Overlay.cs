﻿using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Trero.ClientBase.EntityBase;
using Trero.ClientBase.KeyBase;
using Trero.ClientBase.UIBase.TreroUILibrary;
using Trero.Modules;

namespace Trero.ClientBase.UIBase
{
    public partial class Overlay : Form
    {
        public Overlay()
        {
            InitializeComponent();
            handle = this;

            new Thread(() => {
                Thread.Sleep(100);
                Invoke((MethodInvoker)delegate {
                    Focus();
                });
                while (!Program.quit)
                {
                    // Thread.Sleep(1);
                    try
                    {
                        Invoke((MethodInvoker)delegate {
                            MCM.RECT rect = MCM.getMinecraftRect();

                            Placement e = new Placement();
                            GetWindowPlacement(MCM.mcWinHandle, ref e); // Change window size if fullscreen to match extra offsets
                            int vE = 0;
                            int vA = 0;
                            int vB = 0;
                            int vC = 0;
                            if (e.showCmd == 3) // Perfect window offsets
                            {
                                vE = 8;
                                vA = 2;

                                vB = 9; // these have extra because of the windows shadow effect (Not exactly required but oh well)
                                vC = 3;
                            }

                            Location = new Point(rect.Left + 9 + vA, rect.Top + 35 + vE); // Title bar is 32 pixels high
                            Size = new Size(rect.Right - rect.Left - 18 - vC, rect.Bottom - rect.Top - 44 - vB);
                        });
                    }
                    catch { }
                }
            }).Start();
        }

        [DllImport("user32.dll")] static extern IntPtr GetForegroundWindow();
        [DllImportAttribute("User32.dll")]  static extern IntPtr SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")] static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        [DllImport("user32.dll")] [return: MarshalAs(UnmanagedType.Bool)] static extern bool GetWindowPlacement(IntPtr hWnd, ref Placement lpwndpl);

        private struct Placement
        {
            public int length;
            public int flags;
            public int showCmd;
            public Point ptMinPosition;
            public Point ptMaxPosition;
            public Rectangle rcNormalPosition;
        }

        public static Overlay handle;

        private void timer1_Tick(object sender, EventArgs e)
        {
            string list = "";
            try
            {
                foreach (Actor plr in Game.getPlayers())
                {
                    list += plr.username + " | " + Game.position.Distance(plr.position) + "\r\n";
                }
            }
            catch { }
            playerList.Text = "No players";

            try
            {
                Actor ent = Game.getClosestPlayer();

                if (ent == null)
                {
                    label2.Text = "None";
                    label2.Text = "";
                    label3.Text = "";
                    return;
                }

                var vec = Base.Vec3((int)ent.position.x, (int)ent.position.y, (int)ent.position.z);

                label1.Text = ent.username;
                label2.Text = vec.ToString();
                label3.Text = Game.position.Distance(vec) + "b";

                if (MCM.isMinecraftFocused() && !TopMost)
                    TopMost = true;
                if (!MCM.isMinecraftFocused() && TopMost)
                {
                    if (ActiveForm != this)
                    {
                        TopMost = false;
                        SetWindowPos(Handle, new IntPtr(1), 0, 0, 0, 0, 2 | 1 | 10);
                        Console.WriteLine(GetForegroundWindow());
                    }
                }
            }
            catch { }
        }

        Font df = new Font(FontFamily.GenericSansSerif, 12f);

        private void Overlay_Paint(object sender, PaintEventArgs e)
        {
            /*
            
            // e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(255, 22, 22, 44)), new Rectangle(0, Size.Height - 2 - (6 * (int)df.Size + 4 * DrawingUtils.screenSize), (int)e.Graphics.MeasureString("ClientInstance: " + Game.clientInstance.ToString("X"), df).Width + 4, Size.Height - (4 * (int)df.Size + 4 * DrawingUtils.screenSize)));

            //e.Graphics.DrawString("Trero Template", df, Brushes.Orange, new PointF(0, 0));

            if (Game.screenData.StartsWith("start_screen"))
            {
                e.Graphics.DrawString("Tretard Edition", new Font(FontFamily.GenericSansSerif, 10f * DrawingUtils.screenSize), Brushes.Orange, // Size.Width / 24f
                    new PointF(DrawingUtils.screenCenter.x - (int)(e.Graphics.MeasureString("Tretard Edition", new Font(FontFamily.GenericSansSerif, 10f * DrawingUtils.screenSize)).Width / 2), DrawingUtils.LogoVCenter.y));
            }

            e.Graphics.DrawString("screenCenter: " + DrawingUtils.screenCenter, df, Brushes.Orange, new PointF(0, Size.Height - 6 - (6 * 14 * DrawingUtils.screenSize)));
            e.Graphics.DrawString("screenSize: " + DrawingUtils.screenSize, df, Brushes.Orange, new PointF(0, Size.Height - 6 - (5 * 14 * DrawingUtils.screenSize)));
            e.Graphics.DrawString("ClientInstance: " + Game.clientInstance.ToString("X"), df, Brushes.Orange, new PointF(0, Size.Height - 6 - (4 * 14 * DrawingUtils.screenSize)));
            e.Graphics.DrawString("Pos: " + Game.position, df, Brushes.Orange, new PointF(0, Size.Height - 6 - (3 * 14 * DrawingUtils.screenSize)));
            e.Graphics.DrawString("Players: " + Game.getPlayers().Count, df, Brushes.Orange, new PointF(0, Size.Height - 6 - (2 * 14 * DrawingUtils.screenSize)));
            e.Graphics.DrawString("Entities: " + Game.getEntites().Count, df, Brushes.Orange, new PointF(0, Size.Height - 6 - (1 * 14 * DrawingUtils.screenSize)));

            */
        }

        private Point MouseDownLocation;
        private Point MouseDownLocation2;

        private void panel2_MouseDown_1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
            }
        }

        private void panel2_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                panel2.Left = e.X + panel2.Left - MouseDownLocation.X;
                panel2.Top = e.Y + panel2.Top - MouseDownLocation.Y;
            }
        }

        private void Overlay_Load(object sender, EventArgs e)
        {
            foreach (Module mod in Program.modules)
            {
                Button moduleButton = ClonableButton.Clone();
                moduleButton.Visible = true;
                moduleButton.Text = mod.name;
                moduleButton.Click += moduleActivated;
                moduleButton.FlatAppearance.BorderSize = 0;
                moduleButton.FlatAppearance.BorderColor = TestCategory.BackColor;
            }
        }

        private void moduleActivated(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn == null) return;

            foreach (Module mod in Program.modules)
            {
                if (mod.name == btn.Text)
                {
                    if (mod.enabled)
                    {
                        mod.onDisable();
                        btn.BackColor = Color.FromArgb(255, 44, 44, 44);
                    }
                    else
                    {
                        mod.onEnable();
                        btn.BackColor = Color.FromArgb(255, 39, 39, 39);
                    }
                }
            }
        }

        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MouseDownLocation2 = e.Location;
            }
        }

        private void panel3_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                panel3.Left = e.X + panel3.Left - MouseDownLocation2.X;
                panel3.Top = e.Y + panel3.Top - MouseDownLocation2.Y;
            }
        }

        private void ClonableButton_Click(object sender, EventArgs e) { }
    }
}
