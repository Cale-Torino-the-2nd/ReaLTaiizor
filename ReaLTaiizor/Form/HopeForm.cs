﻿#region Imports

using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Drawing2D;

#endregion

namespace ReaLTaiizor
{
    #region HopeForm

    public class HopeForm : ContainerControl
    {
        private bool mouseFlag = false;
        private Point mousePoint;
        private Rectangle minRectangle;
        private Rectangle maxRectangle;
        private Rectangle closeRectangle;

        private Color _themeColor = HopeColors.LightPrimary;
        private Image _iconImage = null;

        #region Settings

        public Color ThemeColor
        {
            get { return _themeColor; }
            set
            {
                _themeColor = value;
                Invalidate();
            }
        }

        [DefaultValue(true)]
        public bool MinimizeBox
        {
            get
            {
                try
                {
                    return ParentForm.MinimizeBox;
                }
                catch
                {
                    return true;
                }
            }
            set
            {
                try
                {
                    ParentForm.MinimizeBox = value;
                    Invalidate();
                }
                catch
                {
                }
            }
        }

        [DefaultValue(true)]
        public bool MaximizeBox
        {
            get
            {
                try
                {
                    return ParentForm.MaximizeBox;
                }
                catch
                {
                    return true;
                }
            }
            set
            {
                try
                {
                    ParentForm.MaximizeBox = value;
                    Invalidate();
                }
                catch
                {
                }
            }
        }

        [DefaultValue(true)]
        public bool ControlBox
        {
            get
            {
                try
                {
                    return ParentForm.ControlBox;
                }
                catch
                {
                    return true;
                }
            }
            set
            {
                try
                {
                    ParentForm.ControlBox = value;
                    Invalidate();
                }
                catch
                {
                }
            }
        }
        #endregion

        #region Events
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                mouseFlag = true;
                mousePoint = e.Location;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (mouseFlag)
            {
                if (Dock == DockStyle.Top)
                    Parent.Location = new Point(MousePosition.X - mousePoint.X, MousePosition.Y - mousePoint.Y);
                else
                    Parent.Location = new Point(MousePosition.X - mousePoint.X, MousePosition.Y - Parent.Height + mousePoint.Y);
            }
            else
            {
                mousePoint = e.Location;
                Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            mouseFlag = false;
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            if (minRectangle.Contains(mousePoint))
                ParentForm.WindowState = FormWindowState.Minimized;
            if (maxRectangle.Contains(mousePoint))
            {
                if (ParentForm.WindowState == FormWindowState.Maximized)
                    ParentForm.WindowState = FormWindowState.Normal;
                else
                    ParentForm.WindowState = FormWindowState.Maximized;
            }
            if (closeRectangle.Contains(mousePoint))
                Environment.Exit(0);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Height = 40;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            ParentForm.FormBorderStyle = FormBorderStyle.None;
            ParentForm.AllowTransparency = false;
            ParentForm.FindForm().StartPosition = FormStartPosition.CenterScreen;
            ParentForm.MaximumSize = Screen.PrimaryScreen.WorkingArea.Size;
            Invalidate();
        }
        #endregion

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (Dock == DockStyle.Left || Dock == DockStyle.Right || Dock == DockStyle.None)
                Dock = DockStyle.Top;

            if (Dock == DockStyle.Top && Location.X != 0 && Location.Y != 0)
                Location = new Point(0, 0);
            else if (Dock == DockStyle.Bottom && Location.X != 0 && Location.Y != ParentForm.Height - Height)
                Location = new Point(0, ParentForm.Height - Height);

            Width = ParentForm.Width;
            ParentForm.MinimumSize = new Size(190, 40);

            Bitmap bitmap = new Bitmap(Width, Height);
            Graphics graphics = Graphics.FromImage(bitmap);

            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            graphics.Clear(_themeColor);

            var icoFont = new Font("Marlett", 12);

            if (_iconImage != null)
            {
                graphics.DrawImage(_iconImage, new Rectangle(10, 10, 25, 25));

                graphics.DrawString(Text, new Font("Segoe UI", 12f), new SolidBrush(HopeColors.FourLevelBorder), new Rectangle(45, 1, Width - 100, Height), HopeStringAlign.Left);
            }
            else
                graphics.DrawString(Text, new Font("Segoe UI", 12f), new SolidBrush(HopeColors.FourLevelBorder), new Rectangle(15, 1, Width - 100, Height), HopeStringAlign.Left);

            if (ControlBox)
            {
                if (MinimizeBox)
                {
                    minRectangle = new Rectangle(Width - 54 - (MaximizeBox ? 1 : 0) * 22, (Height - 16) / 2, 18, 18);

                    if (minRectangle.Contains(mousePoint))
                        graphics.DrawString("0", icoFont, new SolidBrush(HopeColors.TwoLevelBorder), minRectangle, HopeStringAlign.Center);
                    else
                        graphics.DrawString("0", icoFont, new SolidBrush(Color.White), minRectangle, HopeStringAlign.Center);
                }
                if (MaximizeBox)
                {
                    maxRectangle = new Rectangle(Width - 54, (Height - 16) / 2, 18, 18);

                    if (maxRectangle.Contains(mousePoint))
                    {
                        if (ParentForm.WindowState == FormWindowState.Normal)
                            graphics.DrawString("1", icoFont, new SolidBrush(HopeColors.TwoLevelBorder), maxRectangle, HopeStringAlign.Center);
                        else
                            graphics.DrawString("2", icoFont, new SolidBrush(HopeColors.TwoLevelBorder), maxRectangle, HopeStringAlign.Center);
                    }
                    else
                    {
                        if (ParentForm.WindowState == FormWindowState.Normal)
                            graphics.DrawString("1", icoFont, new SolidBrush(Color.White), maxRectangle, HopeStringAlign.Center);
                        else
                            graphics.DrawString("2", icoFont, new SolidBrush(Color.White), maxRectangle, HopeStringAlign.Center);
                    }
                }

                closeRectangle = new Rectangle(Width - 32, (Height - 16) / 2, 18, 18);

                if (closeRectangle.Contains(mousePoint))
                    graphics.DrawString("r", icoFont, new SolidBrush(HopeColors.Danger), closeRectangle, HopeStringAlign.Center);
                else
                    graphics.DrawString("r", icoFont, new SolidBrush(Color.White), closeRectangle, HopeStringAlign.Center);
            }

            base.OnPaint(e);
            graphics.Dispose();
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.DrawImageUnscaled(bitmap, 0, 0);
            bitmap.Dispose();
        }

        public HopeForm()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            Font = new Font("Segoe UI", 12);
            Height = 40;
            Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            Dock = DockStyle.Top;
        }
    }

    #endregion
}