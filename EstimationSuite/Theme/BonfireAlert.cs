﻿
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

class BonfireAlertBox : Control
{

    private Point exitLocation;
    private bool overExit;

    public enum Style
    {
        _Error,
        _Success,
        _Warning,
        _Notice
    }

    private Style _alertStyle;
    public Style AlertStyle
    {
        get { return _alertStyle; }
        set
        {
            _alertStyle = value;
            Invalidate();
        }
    }

    public BonfireAlertBox()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
        Font = new Font("Verdana", 8);
        Size = new Size(200, 40);
    }

    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
    {
        base.OnPaint(e);

        Graphics G = e.Graphics;

        G.SmoothingMode = SmoothingMode.HighQuality;
        G.Clear(Parent.BackColor);

        Color borderColor = default(Color);
        Color innerColor = default(Color);
        Color textColor = default(Color);
        switch (_alertStyle)
        {
            case Style._Error:
                borderColor = Color.FromArgb(250, 195, 195);
                innerColor = Color.FromArgb(255, 235, 235);
                textColor = Color.FromArgb(220, 90, 90);
                break;
            case Style._Notice:
                borderColor = Color.FromArgb(180, 215, 230);
                innerColor = Color.FromArgb(235, 245, 255);
                textColor = Color.FromArgb(80, 145, 180);
                break;
            case Style._Success:
                borderColor = Color.FromArgb(180, 220, 130);
                innerColor = Color.FromArgb(235, 245, 225);
                textColor = Color.FromArgb(95, 145, 45);
                break;
            case Style._Warning:
                borderColor = Color.FromArgb(220, 215, 140);
                innerColor = Color.FromArgb(250, 250, 220);
                textColor = Color.FromArgb(145, 135, 110);
                break;
        }

        Rectangle mainRect = new Rectangle(0, 0, Width - 1, Height - 1);
        G.FillRectangle(new SolidBrush(innerColor), mainRect);
        G.DrawRectangle(new Pen(borderColor), mainRect);

        string styleText = null;
        switch (_alertStyle)
        {
            case Style._Error:
                styleText = "Error";
                break;
            case Style._Notice:
                styleText = "Notice";
                break;
            case Style._Success:
                styleText = "Success";
                break;
            case Style._Warning:
                styleText = "Warning";
                break;
        }

        Font styleFont = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
        int textY = Convert.ToInt32((Height - 1) / 2 - (G.MeasureString(Text, Font).Height / 2));
        G.DrawString(styleText, styleFont, new SolidBrush(textColor), new Point(10, textY));
        G.DrawString(Text, Font, new SolidBrush(textColor), new Point(Convert.ToInt32(10 + G.MeasureString(styleText, styleFont).Width + 4), textY));

        Font exitFont = new Font("Marlett", 6);
        int exitY = Convert.ToInt32(((Height - 1) / 2) - (G.MeasureString("r", exitFont).Height / 2) + 1);
        exitLocation = new Point(Width - 26, exitY - 3);
        G.DrawString("r", exitFont, new SolidBrush(textColor), new Point(Width - 23, exitY));

    }

    protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
    {
        base.OnMouseMove(e);

        if (e.X >= Width - 26 && e.X <= Width - 12 && e.Y > exitLocation.Y && e.Y < exitLocation.Y + 12)
        {
            if (Cursor != Cursors.Hand)
                Cursor = Cursors.Hand;
            overExit = true;
        }
        else
        {
            if (Cursor != Cursors.Arrow)
                Cursor = Cursors.Arrow;
            overExit = false;
        }

        Invalidate();

    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);

        if (overExit)
            this.Visible = false;

    }

}