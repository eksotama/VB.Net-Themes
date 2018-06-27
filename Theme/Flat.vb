Imports System.Drawing.Drawing2D, System.ComponentModel

''' <summary>
''' Flat UI Theme
''' Coder: iSynthesis (HF)
''' Version: 1.0.1
''' Date Created: 16/06/2013
''' Date Changed: 18/06/2013
''' UID: 374648
''' </summary>
''' <remarks></remarks>

Module Helpers

#Region " Variables"
    Friend G As Graphics, B As Bitmap
    Friend _FlatColor As Color = Color.FromArgb(35, 168, 109)
    Friend NearSF As New StringFormat() With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Near}
    Friend CenterSF As New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
#End Region

#Region " Functions"

    Public Function RoundRec(ByVal Rectangle As Rectangle, ByVal Curve As Integer) As GraphicsPath
        Dim P As GraphicsPath = New GraphicsPath()
        Dim ArcRectangleWidth As Integer = Curve * 2
        P.AddArc(New Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90)
        P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90)
        P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 0, 90)
        P.AddArc(New Rectangle(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 90, 90)
        P.AddLine(New Point(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y), New Point(Rectangle.X, Curve + Rectangle.Y))
        Return P
    End Function

    '-- Credit: AeonHack
    Public Function DrawArrow(ByVal x As Integer, ByVal y As Integer, ByVal flip As Boolean) As GraphicsPath
        Dim GP As New GraphicsPath()

        Dim W As Integer = 12
        Dim H As Integer = 6

        If flip Then
            GP.AddLine(x + 1, y, x + W + 1, y)
            GP.AddLine(x + W, y, x + H, y + H - 1)
        Else
            GP.AddLine(x, y + H, x + W, y + H)
            GP.AddLine(x + W, y + H, x + H, y)
        End If

        GP.CloseFigure()
        Return GP
    End Function

#End Region

End Module

#Region " Mouse States"

Enum MouseState As Byte
    None = 0
    Over = 1
    Down = 2
    Block = 3
End Enum

#End Region

Class FormSkin : Inherits ContainerControl

#Region " Variables"

    Private W, H As Integer
    Private Cap As Boolean = False
    Private _HeaderMaximize As Boolean = False
    Private MousePoint As New Point(0, 0)
    Private MoveHeight = 50

#End Region

#Region " Properties"

#Region " Colors"

    <Category("Colors")> _
    Public Property HeaderColor() As Color
        Get
            Return _HeaderColor
        End Get
        Set(ByVal value As Color)
            _HeaderColor = value
        End Set
    End Property
    <Category("Colors")> _
    Public Property BaseColor() As Color
        Get
            Return _BaseColor
        End Get
        Set(ByVal value As Color)
            _BaseColor = value
        End Set
    End Property
    <Category("Colors")> _
    Public Property BorderColor() As Color
        Get
            Return _BorderColor
        End Get
        Set(ByVal value As Color)
            _BorderColor = value
        End Set
    End Property
    <Category("Colors")> _
    Public Property FlatColor() As Color
        Get
            Return _FlatColor
        End Get
        Set(ByVal value As Color)
            _FlatColor = value
        End Set
    End Property

#End Region

    <Category("Options")>
    Public Property HeaderMaximize As Boolean
        Get
            Return _HeaderMaximize
        End Get
        Set(ByVal value As Boolean)
            _HeaderMaximize = value
        End Set
    End Property

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        If e.Button = Windows.Forms.MouseButtons.Left And New Rectangle(0, 0, Width, MoveHeight).Contains(e.Location) Then
            Cap = True
            MousePoint = e.Location
        End If
    End Sub

    Private Sub FormSkin_MouseDoubleClick(ByVal sender As Object, ByVal e As MouseEventArgs) Handles Me.MouseDoubleClick
        If HeaderMaximize Then
            If e.Button = Windows.Forms.MouseButtons.Left And New Rectangle(0, 0, Width, MoveHeight).Contains(e.Location) Then
                If FindForm.WindowState = FormWindowState.Normal Then
                    FindForm.WindowState = FormWindowState.Maximized : FindForm()
                ElseIf FindForm.WindowState = FormWindowState.Maximized Then
                    FindForm.WindowState = FormWindowState.Normal : FindForm()
                End If
            End If
        End If
    End Sub

    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        MyBase.OnMouseUp(e) : Cap = False
    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)
        MyBase.OnMouseMove(e)
        If Cap Then
            Parent.Location = MousePosition - MousePoint
        End If
    End Sub

    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        ParentForm.FormBorderStyle = FormBorderStyle.None
        ParentForm.AllowTransparency = False
        ParentForm.TransparencyKey = Color.Fuchsia
        ParentForm.FindForm.StartPosition = FormStartPosition.CenterScreen
        Dock = DockStyle.Fill
        Invalidate()
    End Sub

#End Region

#Region " Colors"

    Private _HeaderColor As Color = Color.FromArgb(45, 47, 49)
    Private _BaseColor As Color = Color.FromArgb(60, 70, 73)
    Private _BorderColor As Color = Color.FromArgb(53, 58, 60)
    Private TextColor As Color = Color.FromArgb(234, 234, 234)

#End Region

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                 ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        DoubleBuffered = True
        BackColor = Color.White
        Font = New Font("Segoe UI", 12)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        B = New Bitmap(Width, Height) : G = Graphics.FromImage(B)
        W = Width : H = Height

        Dim Base As New Rectangle(0, 0, W, H), Header As New Rectangle(0, 0, W, 50)

        With G
            .SmoothingMode = 2
            .PixelOffsetMode = 2
            .TextRenderingHint = 5
            .Clear(BackColor)

            '-- Base
            .FillRectangle(New SolidBrush(_BaseColor), Base)

            '-- Header
            .FillRectangle(New SolidBrush(_HeaderColor), Header)

            '-- Logo
            .FillRectangle(New SolidBrush(Color.FromArgb(243, 243, 243)), New Rectangle(8, 16, 4, 18))
            .FillRectangle(New SolidBrush(_FlatColor), 16, 16, 4, 18)
            .DrawString(Text, Font, New SolidBrush(TextColor), New Rectangle(26, 15, W, H), NearSF)

            '-- Border
            .DrawRectangle(New Pen(_BorderColor), Base)
        End With

        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
End Class

Class FlatClose : Inherits Control

#Region " Variables"

    Private State As MouseState = MouseState.None
    Private x As Integer

#End Region

#Region " Properties"

#Region " Mouse States"

    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)
        MyBase.OnMouseMove(e)
        x = e.X : Invalidate()
    End Sub

    Protected Overrides Sub OnClick(ByVal e As EventArgs)
        MyBase.OnClick(e)
        Environment.Exit(0)
    End Sub

#End Region

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        MyBase.OnResize(e)
        Size = New Size(18, 18)
    End Sub

#Region " Colors"

    <Category("Colors")> _
    Public Property BaseColor As Color
        Get
            Return _BaseColor
        End Get
        Set(ByVal value As Color)
            _BaseColor = value
        End Set
    End Property

    <Category("Colors")> _
    Public Property TextColor As Color
        Get
            Return _TextColor
        End Get
        Set(ByVal value As Color)
            _TextColor = value
        End Set
    End Property

#End Region

#End Region

#Region " Colors"

    Private _BaseColor As Color = Color.FromArgb(168, 35, 35)
    Private _TextColor As Color = Color.FromArgb(243, 243, 243)

#End Region

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                 ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        DoubleBuffered = True
        BackColor = Color.White
        Size = New Size(18, 18)
        Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Font = New Font("Marlett", 10)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)

        Dim Base As New Rectangle(0, 0, Width, Height)

        With G
            .SmoothingMode = 2
            .PixelOffsetMode = 2
            .TextRenderingHint = 5
            .Clear(BackColor)

            '-- Base
            .FillRectangle(New SolidBrush(_BaseColor), Base)

            '-- X
            .DrawString("r", Font, New SolidBrush(TextColor), New Rectangle(0, 0, Width, Height), CenterSF)

            '-- Hover/down
            Select Case State
                Case MouseState.Over
                    .FillRectangle(New SolidBrush(Color.FromArgb(30, Color.White)), Base)
                Case MouseState.Down
                    .FillRectangle(New SolidBrush(Color.FromArgb(30, Color.Black)), Base)
            End Select
        End With

        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
End Class

Class FlatMax : Inherits Control

#Region " Variables"

    Private State As MouseState = MouseState.None
    Private x As Integer

#End Region

#Region " Properties"

#Region " Mouse States"

    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)
        MyBase.OnMouseMove(e)
        x = e.X : Invalidate()
    End Sub

    Protected Overrides Sub OnClick(ByVal e As EventArgs)
        MyBase.OnClick(e)
        Select Case FindForm.WindowState
            Case FormWindowState.Maximized
                FindForm.WindowState = FormWindowState.Normal
            Case FormWindowState.Normal
                FindForm.WindowState = FormWindowState.Maximized
        End Select
    End Sub

#End Region

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        MyBase.OnResize(e)
        Size = New Size(18, 18)
    End Sub

#Region " Colors"

    <Category("Colors")> _
    Public Property BaseColor As Color
        Get
            Return _BaseColor
        End Get
        Set(ByVal value As Color)
            _BaseColor = value
        End Set
    End Property

    <Category("Colors")> _
    Public Property TextColor As Color
        Get
            Return _TextColor
        End Get
        Set(ByVal value As Color)
            _TextColor = value
        End Set
    End Property

#End Region

#End Region

#Region " Colors"

    Private _BaseColor As Color = Color.FromArgb(45, 47, 49)
    Private _TextColor As Color = Color.FromArgb(243, 243, 243)

#End Region

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                 ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        DoubleBuffered = True
        BackColor = Color.White
        Size = New Size(18, 18)
        Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Font = New Font("Marlett", 12)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)

        Dim Base As New Rectangle(0, 0, Width, Height)

        With G
            .SmoothingMode = 2
            .PixelOffsetMode = 2
            .TextRenderingHint = 5
            .Clear(BackColor)

            '-- Base
            .FillRectangle(New SolidBrush(_BaseColor), Base)

            '-- Maximize
            If FindForm.WindowState = FormWindowState.Maximized Then
                .DrawString("1", Font, New SolidBrush(TextColor), New Rectangle(1, 1, Width, Height), CenterSF)
            ElseIf FindForm.WindowState = FormWindowState.Normal Then
                .DrawString("2", Font, New SolidBrush(TextColor), New Rectangle(1, 1, Width, Height), CenterSF)
            End If

            '-- Hover/down
            Select Case State
                Case MouseState.Over
                    .FillRectangle(New SolidBrush(Color.FromArgb(30, Color.White)), Base)
                Case MouseState.Down
                    .FillRectangle(New SolidBrush(Color.FromArgb(30, Color.Black)), Base)
            End Select
        End With

        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
End Class

Class FlatMini : Inherits Control

#Region " Variables"

    Private State As MouseState = MouseState.None
    Private x As Integer

#End Region

#Region " Properties"

#Region " Mouse States"

    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)
        MyBase.OnMouseMove(e)
        x = e.X : Invalidate()
    End Sub

    Protected Overrides Sub OnClick(ByVal e As EventArgs)
        MyBase.OnClick(e)
        Select Case FindForm.WindowState
            Case FormWindowState.Normal
                FindForm.WindowState = FormWindowState.Minimized
            Case FormWindowState.Maximized
                FindForm.WindowState = FormWindowState.Minimized
        End Select
    End Sub

#End Region

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        MyBase.OnResize(e)
        Size = New Size(18, 18)
    End Sub

#Region " Colors"

    <Category("Colors")> _
    Public Property BaseColor As Color
        Get
            Return _BaseColor
        End Get
        Set(ByVal value As Color)
            _BaseColor = value
        End Set
    End Property

    <Category("Colors")> _
    Public Property TextColor As Color
        Get
            Return _TextColor
        End Get
        Set(ByVal value As Color)
            _TextColor = value
        End Set
    End Property

#End Region

#End Region

#Region " Colors"

    Private _BaseColor As Color = Color.FromArgb(45, 47, 49)
    Private _TextColor As Color = Color.FromArgb(243, 243, 243)

#End Region

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                 ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        DoubleBuffered = True
        BackColor = Color.White
        Size = New Size(18, 18)
        Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Font = New Font("Marlett", 12)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)

        Dim Base As New Rectangle(0, 0, Width, Height)

        With G
            .SmoothingMode = 2
            .PixelOffsetMode = 2
            .TextRenderingHint = 5
            .Clear(BackColor)

            '-- Base
            .FillRectangle(New SolidBrush(_BaseColor), Base)

            '-- Minimize
            .DrawString("0", Font, New SolidBrush(TextColor), New Rectangle(2, 1, Width, Height), CenterSF)

            '-- Hover/down
            Select Case State
                Case MouseState.Over
                    .FillRectangle(New SolidBrush(Color.FromArgb(30, Color.White)), Base)
                Case MouseState.Down
                    .FillRectangle(New SolidBrush(Color.FromArgb(30, Color.Black)), Base)
            End Select
        End With

        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
End Class

Class FlatColorPalette : Inherits Control

#Region " Variables"

    Private W, H As Integer

#End Region

#Region " Properties"

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        MyBase.OnResize(e)
        Width = 180
        Height = 80
    End Sub

#Region " Colors"

    <Category("Colors")> _
    Public Property Red As Color
        Get
            Return _Red
        End Get
        Set(ByVal value As Color)
            _Red = value
        End Set
    End Property

    <Category("Colors")> _
    Public Property Cyan As Color
        Get
            Return _Cyan
        End Get
        Set(ByVal value As Color)
            _Cyan = value
        End Set
    End Property

    <Category("Colors")> _
    Public Property Blue As Color
        Get
            Return _Blue
        End Get
        Set(ByVal value As Color)
            _Blue = value
        End Set
    End Property

    <Category("Colors")> _
    Public Property LimeGreen As Color
        Get
            Return _LimeGreen
        End Get
        Set(ByVal value As Color)
            _LimeGreen = value
        End Set
    End Property

    <Category("Colors")> _
    Public Property Orange As Color
        Get
            Return _Orange
        End Get
        Set(ByVal value As Color)
            _Orange = value
        End Set
    End Property

    <Category("Colors")> _
    Public Property Purple As Color
        Get
            Return _Purple
        End Get
        Set(ByVal value As Color)
            _Purple = value
        End Set
    End Property

    <Category("Colors")> _
    Public Property Black As Color
        Get
            Return _Black
        End Get
        Set(ByVal value As Color)
            _Black = value
        End Set
    End Property

    <Category("Colors")> _
    Public Property Gray As Color
        Get
            Return _Gray
        End Get
        Set(ByVal value As Color)
            _Gray = value
        End Set
    End Property

    <Category("Colors")> _
    Public Property White As Color
        Get
            Return _White
        End Get
        Set(ByVal value As Color)
            _White = value
        End Set
    End Property

#End Region

#End Region

#Region " Colors"

    Private _Red As Color = Color.FromArgb(220, 85, 96)
    Private _Cyan As Color = Color.FromArgb(10, 154, 157)
    Private _Blue As Color = Color.FromArgb(0, 128, 255)
    Private _LimeGreen As Color = Color.FromArgb(35, 168, 109)
    Private _Orange As Color = Color.FromArgb(253, 181, 63)
    Private _Purple As Color = Color.FromArgb(155, 88, 181)
    Private _Black As Color = Color.FromArgb(45, 47, 49)
    Private _Gray As Color = Color.FromArgb(63, 70, 73)
    Private _White As Color = Color.FromArgb(243, 243, 243)

#End Region

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                 ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        DoubleBuffered = True
        BackColor = Color.FromArgb(60, 70, 73)
        Size = New Size(160, 80)
        Font = New Font("Segoe UI", 12)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        B = New Bitmap(Width, Height) : G = Graphics.FromImage(B)
        W = Width - 1 : H = Height - 1

        With G
            .SmoothingMode = 2
            .PixelOffsetMode = 2
            .TextRenderingHint = 5
            .Clear(BackColor)

            '-- Colors
            .FillRectangle(New SolidBrush(_Red), New Rectangle(0, 0, 20, 40))
            .FillRectangle(New SolidBrush(_Cyan), New Rectangle(20, 0, 20, 40))
            .FillRectangle(New SolidBrush(_Blue), New Rectangle(40, 0, 20, 40))
            .FillRectangle(New SolidBrush(_LimeGreen), New Rectangle(60, 0, 20, 40))
            .FillRectangle(New SolidBrush(_Orange), New Rectangle(80, 0, 20, 40))
            .FillRectangle(New SolidBrush(_Purple), New Rectangle(100, 0, 20, 40))
            .FillRectangle(New SolidBrush(_Black), New Rectangle(120, 0, 20, 40))
            .FillRectangle(New SolidBrush(_Gray), New Rectangle(140, 0, 20, 40))
            .FillRectangle(New SolidBrush(_White), New Rectangle(160, 0, 20, 40))

            '-- Text
            .DrawString("Color Palette", Font, New SolidBrush(_White), New Rectangle(0, 22, W, H), CenterSF)
        End With

        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
End Class

Class FlatGroupBox : Inherits ContainerControl

#Region " Variables"

    Private W, H As Integer
    Private _ShowText As Boolean = True

#End Region

#Region " Properties"

    <Category("Colors")> _
    Public Property BaseColor As Color
        Get
            Return _BaseColor
        End Get
        Set(ByVal value As Color)
            _BaseColor = value
        End Set
    End Property

    Public Property ShowText As Boolean
        Get
            Return _ShowText
        End Get
        Set(ByVal value As Boolean)
            _ShowText = value
        End Set
    End Property

#End Region

#Region " Colors"

    Private _BaseColor As Color = Color.FromArgb(60, 70, 73)

#End Region

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                 ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or _
                 ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        Size = New Size(240, 180)
        Font = New Font("Segoe ui", 10)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        B = New Bitmap(Width, Height) : G = Graphics.FromImage(B)
        W = Width - 1 : H = Height - 1

        Dim GP, GP2, GP3 As New GraphicsPath
        Dim Base As New Rectangle(8, 8, W - 16, H - 16)

        With G
            .SmoothingMode = 2
            .PixelOffsetMode = 2
            .TextRenderingHint = 5
            .Clear(BackColor)

            '-- Base
            GP = Helpers.RoundRec(Base, 8)
            .FillPath(New SolidBrush(_BaseColor), GP)

            '-- Arrows
            GP2 = Helpers.DrawArrow(28, 2, False)
            .FillPath(New SolidBrush(_BaseColor), GP2)
            GP3 = Helpers.DrawArrow(28, 8, True)
            .FillPath(New SolidBrush(Color.FromArgb(60, 70, 73)), GP3)

            '-- if ShowText
            If ShowText Then
                .DrawString(Text, Font, New SolidBrush(_FlatColor), New Rectangle(16, 16, W, H), NearSF)
            End If
        End With

        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
End Class

Class FlatButton : Inherits Control

#Region " Variables"

    Private W, H As Integer
    Private _Rounded As Boolean = False
    Private State As MouseState = MouseState.None

#End Region

#Region " Properties"

#Region " Colors"

    <Category("Colors")> _
    Public Property BaseColor As Color
        Get
            Return _BaseColor
        End Get
        Set(ByVal value As Color)
            _BaseColor = value
        End Set
    End Property

    <Category("Colors")> _
    Public Property TextColor As Color
        Get
            Return _TextColor
        End Get
        Set(ByVal value As Color)
            _TextColor = value
        End Set
    End Property

    <Category("Options")> _
    Public Property Rounded As Boolean
        Get
            Return _Rounded
        End Get
        Set(ByVal value As Boolean)
            _Rounded = value
        End Set
    End Property

#End Region

#Region " Mouse States"

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None : Invalidate()
    End Sub

#End Region

#End Region

#Region " Colors"

    Private _BaseColor As Color = _FlatColor
    Private _TextColor As Color = Color.FromArgb(243, 243, 243)

#End Region

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                 ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or _
                 ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        Size = New Size(106, 32)
        BackColor = Color.Transparent
        Font = New Font("Segoe UI", 12)
        Cursor = Cursors.Hand
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        B = New Bitmap(Width, Height) : G = Graphics.FromImage(B)
        W = Width - 1 : H = Height - 1

        Dim GP As New GraphicsPath
        Dim Base As New Rectangle(0, 0, W, H)

        With G
            .SmoothingMode = 2
            .PixelOffsetMode = 2
            .TextRenderingHint = 5
            .Clear(BackColor)

            Select Case State
                Case MouseState.None
                    If Rounded Then
                        '-- Base
                        GP = Helpers.RoundRec(Base, 6)
                        .FillPath(New SolidBrush(_BaseColor), GP)

                        '-- Text
                        .DrawString(Text, Font, New SolidBrush(_TextColor), Base, CenterSF)
                    Else
                        '-- Base
                        .FillRectangle(New SolidBrush(_BaseColor), Base)

                        '-- Text
                        .DrawString(Text, Font, New SolidBrush(_TextColor), Base, CenterSF)
                    End If
                Case MouseState.Over
                    If Rounded Then
                        '-- Base
                        GP = Helpers.RoundRec(Base, 6)
                        .FillPath(New SolidBrush(_BaseColor), GP)
                        .FillPath(New SolidBrush(Color.FromArgb(20, Color.White)), GP)

                        '-- Text
                        .DrawString(Text, Font, New SolidBrush(_TextColor), Base, CenterSF)
                    Else
                        '-- Base
                        .FillRectangle(New SolidBrush(_BaseColor), Base)
                        .FillRectangle(New SolidBrush(Color.FromArgb(20, Color.White)), Base)

                        '-- Text
                        .DrawString(Text, Font, New SolidBrush(_TextColor), Base, CenterSF)
                    End If
                Case MouseState.Down
                    If Rounded Then
                        '-- Base
                        GP = Helpers.RoundRec(Base, 6)
                        .FillPath(New SolidBrush(_BaseColor), GP)
                        .FillPath(New SolidBrush(Color.FromArgb(20, Color.Black)), GP)

                        '-- Text
                        .DrawString(Text, Font, New SolidBrush(_TextColor), Base, CenterSF)
                    Else
                        '-- Base
                        .FillRectangle(New SolidBrush(_BaseColor), Base)
                        .FillRectangle(New SolidBrush(Color.FromArgb(20, Color.Black)), Base)

                        '-- Text
                        .DrawString(Text, Font, New SolidBrush(_TextColor), Base, CenterSF)
                    End If
            End Select
        End With

        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
End Class

<DefaultEvent("CheckedChanged")> Class FlatToggle : Inherits Control

#Region " Variables"

    Private W, H As Integer
    Private O As _Options
    Private _Checked As Boolean = False
    Private State As MouseState = MouseState.None

#End Region

#Region " Properties"
    Public Event CheckedChanged(ByVal sender As Object)

    <Flags()> _
    Enum _Options
        Style1
        Style2
        Style3
        Style4 '-- TODO: New Style
        Style5 '-- TODO: New Style
    End Enum

#Region " Options"

    <Category("Options")> _
    Public Property Options As _Options
        Get
            Return O
        End Get
        Set(ByVal value As _Options)
            O = value
        End Set
    End Property

    <Category("Options")> _
    Public Property Checked As Boolean
        Get
            Return _Checked
        End Get
        Set(ByVal value As Boolean)
            _Checked = value
        End Set
    End Property

#End Region

    Protected Overrides Sub OnTextChanged(ByVal e As EventArgs)
        MyBase.OnTextChanged(e) : Invalidate()
    End Sub

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        MyBase.OnResize(e)
        Width = 76
        Height = 33
    End Sub

#Region " Mouse States"

    Protected Overrides Sub OnMouseEnter(ByVal e As System.EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(ByVal e As System.EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnClick(ByVal e As EventArgs)
        MyBase.OnClick(e)
        _Checked = Not _Checked
        RaiseEvent CheckedChanged(Me)
    End Sub

#End Region

#End Region

#Region " Colors"

    Private BaseColor As Color = _FlatColor
    Private BaseColorRed As Color = Color.FromArgb(220, 85, 96)
    Private BGColor As Color = Color.FromArgb(84, 85, 86)
    Private ToggleColor As Color = Color.FromArgb(45, 47, 49)
    Private TextColor As Color = Color.FromArgb(243, 243, 243)

#End Region

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                 ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or _
                 ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        Size = New Size(44, Height + 1)
        Cursor = Cursors.Hand
        Font = New Font("Segoe UI", 10)
        Size = New Size(76, 33)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        B = New Bitmap(Width, Height) : G = Graphics.FromImage(B)
        W = Width - 1 : H = Height - 1

        Dim GP, GP2 As New GraphicsPath
        Dim Base As New Rectangle(0, 0, W, H), Toggle As New Rectangle(CInt(W \ 2), 0, 38, H)

        With G
            .SmoothingMode = 2
            .PixelOffsetMode = 2
            .TextRenderingHint = 5
            .Clear(BackColor)

            Select Case O
                Case _Options.Style1   '-- Style 1
                    '-- Base
                    GP = Helpers.RoundRec(Base, 6)
                    GP2 = Helpers.RoundRec(Toggle, 6)
                    .FillPath(New SolidBrush(BGColor), GP)
                    .FillPath(New SolidBrush(ToggleColor), GP2)

                    '-- Text
                    .DrawString("OFF", Font, New SolidBrush(BGColor), New Rectangle(19, 1, W, H), CenterSF)

                    If Checked Then
                        '-- Base
                        GP = Helpers.RoundRec(Base, 6)
                        GP2 = Helpers.RoundRec(New Rectangle(CInt(W \ 2), 0, 38, H), 6)
                        .FillPath(New SolidBrush(ToggleColor), GP)
                        .FillPath(New SolidBrush(BaseColor), GP2)

                        '-- Text
                        .DrawString("ON", Font, New SolidBrush(BaseColor), New Rectangle(8, 7, W, H), NearSF)
                    End If
                Case _Options.Style2   '-- Style 2
                    '-- Base
                    GP = Helpers.RoundRec(Base, 6)
                    Toggle = New Rectangle(4, 4, 36, H - 8)
                    GP2 = Helpers.RoundRec(Toggle, 4)
                    .FillPath(New SolidBrush(BaseColorRed), GP)
                    .FillPath(New SolidBrush(ToggleColor), GP2)

                    '-- Lines
                    .DrawLine(New Pen(BGColor), 18, 20, 18, 12)
                    .DrawLine(New Pen(BGColor), 22, 20, 22, 12)
                    .DrawLine(New Pen(BGColor), 26, 20, 26, 12)

                    '-- Text
                    .DrawString("r", New Font("Marlett", 8), New SolidBrush(TextColor), New Rectangle(19, 2, Width, Height), CenterSF)

                    If Checked Then
                        GP = Helpers.RoundRec(Base, 6)
                        Toggle = New Rectangle(CInt(W \ 2) - 2, 4, 36, H - 8)
                        GP2 = Helpers.RoundRec(Toggle, 4)
                        .FillPath(New SolidBrush(BaseColor), GP)
                        .FillPath(New SolidBrush(ToggleColor), GP2)

                        '-- Lines
                        .DrawLine(New Pen(BGColor), CInt(W \ 2) + 12, 20, CInt(W \ 2) + 12, 12)
                        .DrawLine(New Pen(BGColor), CInt(W \ 2) + 16, 20, CInt(W \ 2) + 16, 12)
                        .DrawLine(New Pen(BGColor), CInt(W \ 2) + 20, 20, CInt(W \ 2) + 20, 12)

                        '-- Text
                        .DrawString("?", New Font("Wingdings", 14), New SolidBrush(TextColor), New Rectangle(8, 7, Width, Height), NearSF)
                    End If
                Case _Options.Style3   '-- Style 3
                    '-- Base
                    GP = Helpers.RoundRec(Base, 16)
                    Toggle = New Rectangle(W - 28, 4, 22, H - 8)
                    GP2.AddEllipse(Toggle)
                    .FillPath(New SolidBrush(ToggleColor), GP)
                    .FillPath(New SolidBrush(BaseColorRed), GP2)

                    '-- Text
                    .DrawString("OFF", Font, New SolidBrush(BaseColorRed), New Rectangle(-12, 2, W, H), CenterSF)

                    If Checked Then
                        '-- Base
                        GP = Helpers.RoundRec(Base, 16)
                        Toggle = New Rectangle(6, 4, 22, H - 8)
                        GP2.Reset()
                        GP2.AddEllipse(Toggle)
                        .FillPath(New SolidBrush(ToggleColor), GP)
                        .FillPath(New SolidBrush(BaseColor), GP2)

                        '-- Text
                        .DrawString("ON", Font, New SolidBrush(BaseColor), New Rectangle(12, 2, W, H), CenterSF)
                    End If
                Case _Options.Style4
                    '-- TODO: New Styles
                    If Checked Then
                        '--
                    End If
                Case _Options.Style5
                    '-- TODO: New Styles
                    If Checked Then
                        '--
                    End If
            End Select

        End With

        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
End Class

<DefaultEvent("CheckedChanged")> Class FlatRadioButton : Inherits Control

#Region " Variables"

    Private State As MouseState = MouseState.None
    Private W, H As Integer
    Private O As _Options
    Private _Checked As Boolean

#End Region

#Region " Properties"

    Event CheckedChanged(ByVal sender As Object)
    Property Checked() As Boolean
        Get
            Return _Checked
        End Get
        Set(ByVal value As Boolean)
            _Checked = value
            InvalidateControls()
            RaiseEvent CheckedChanged(Me)
            Invalidate()
        End Set
    End Property

    Protected Overrides Sub OnClick(ByVal e As EventArgs)
        If Not _Checked Then Checked = True
        MyBase.OnClick(e)
    End Sub

    Private Sub InvalidateControls()
        If Not IsHandleCreated OrElse Not _Checked Then Return
        For Each C As Control In Parent.Controls
            If C IsNot Me AndAlso TypeOf C Is RadioButton Then
                DirectCast(C, RadioButton).Checked = False
                Invalidate()
            End If
        Next
    End Sub

    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        InvalidateControls()
    End Sub

    <Flags()> _
    Enum _Options
        Style1
        Style2
    End Enum

    <Category("Options")> _
    Public Property Options As _Options
        Get
            Return O
        End Get
        Set(ByVal value As _Options)
            O = value
        End Set
    End Property

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        MyBase.OnResize(e)
        Height = 22
    End Sub

#Region " Colors"

    <Category("Colors")> _
    Public Property BaseColor As Color
        Get
            Return _BaseColor
        End Get
        Set(ByVal value As Color)
            _BaseColor = value
        End Set
    End Property

    <Category("Colors")> _
    Public Property BorderColor As Color
        Get
            Return _BorderColor
        End Get
        Set(ByVal value As Color)
            _BorderColor = value
        End Set
    End Property

#End Region

#Region " Mouse States"

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None : Invalidate()
    End Sub

#End Region

#End Region

#Region " Colors"

    Private _BaseColor As Color = Color.FromArgb(45, 47, 49)
    Private _BorderColor As Color = _FlatColor
    Private _TextColor As Color = Color.FromArgb(243, 243, 243)

#End Region

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                   ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        DoubleBuffered = True
        Cursor = Cursors.Hand
        Size = New Size(100, 22)
        BackColor = Color.FromArgb(60, 70, 73)
        Font = New Font("Segoe UI", 10)
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        B = New Bitmap(Width, Height) : G = Graphics.FromImage(B)
        W = Width - 1 : H = Height - 1

        Dim Base As New Rectangle(0, 2, Height - 5, Height - 5), Dot As New Rectangle(4, 6, H - 12, H - 12)

        With G
            .SmoothingMode = 2
            .TextRenderingHint = 5
            .Clear(BackColor)

            Select Case O
                Case _Options.Style1 '-- Style 1
                    '-- Base
                    .FillEllipse(New SolidBrush(_BaseColor), Base)

                    Select Case State '-- Mouse States
                        Case MouseState.Over
                            '-- Base
                            .DrawEllipse(New Pen(_BorderColor), Base)
                        Case MouseState.Down
                            '-- Base
                            .DrawEllipse(New Pen(_BorderColor), Base)
                    End Select

                    '-- If Checked
                    If Checked Then
                        '-- Base
                        .FillEllipse(New SolidBrush(_BorderColor), Dot)
                    End If

                    '-- If Enabled
                    If Me.Enabled = False Then
                        '-- Base
                        .FillEllipse(New SolidBrush(Color.FromArgb(54, 58, 61)), Base)
                        '-- Text
                        .DrawString(Text, Font, New SolidBrush(Color.FromArgb(140, 142, 143)), New Rectangle(20, 2, W, H), NearSF)
                    End If

                    '-- Text
                    .DrawString(Text, Font, New SolidBrush(_TextColor), New Rectangle(20, 2, W, H), NearSF)
                Case _Options.Style2 '-- Style 2
                    '-- Base
                    .FillEllipse(New SolidBrush(_BaseColor), Base)

                    Select Case State
                        Case MouseState.Over '-- Mouse States
                            '-- Base
                            .DrawEllipse(New Pen(_BorderColor), Base)
                            .FillEllipse(New SolidBrush(Color.FromArgb(118, 213, 170)), Base)
                        Case MouseState.Down
                            '-- Base
                            .DrawEllipse(New Pen(_BorderColor), Base)
                            .FillEllipse(New SolidBrush(Color.FromArgb(118, 213, 170)), Base)
                    End Select

                    '-- If Checked
                    If Checked Then
                        '-- Base
                        .FillEllipse(New SolidBrush(_BorderColor), Dot)
                    End If

                    '-- If Enabled
                    If Me.Enabled = False Then
                        '-- Base
                        .FillEllipse(New SolidBrush(Color.FromArgb(54, 58, 61)), Base)
                        '-- Text
                        .DrawString(Text, Font, New SolidBrush(Color.FromArgb(48, 119, 91)), New Rectangle(20, 2, W, H), NearSF)
                    End If

                    '-- Text
                    .DrawString(Text, Font, New SolidBrush(_TextColor), New Rectangle(20, 2, W, H), NearSF)
            End Select
        End With

        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
End Class

<DefaultEvent("CheckedChanged")> Class FlatCheckBox : Inherits Control

#Region " Variables"

    Private W, H As Integer
    Private State As MouseState = MouseState.None
    Private O As _Options
    Private _Checked As Boolean

#End Region

#Region " Properties"
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub

    Property Checked() As Boolean
        Get
            Return _Checked
        End Get
        Set(ByVal value As Boolean)
            _Checked = value
            Invalidate()
        End Set
    End Property

    Event CheckedChanged(ByVal sender As Object)
    Protected Overrides Sub OnClick(ByVal e As System.EventArgs)
        _Checked = Not _Checked
        RaiseEvent CheckedChanged(Me)
        MyBase.OnClick(e)
    End Sub

    <Flags()> _
    Enum _Options
        Style1
        Style2
    End Enum

    <Category("Options")> _
    Public Property Options As _Options
        Get
            Return O
        End Get
        Set(ByVal value As _Options)
            O = value
        End Set
    End Property

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        MyBase.OnResize(e)
        Height = 22
    End Sub

#Region " Colors"

    <Category("Colors")> _
    Public Property BaseColor As Color
        Get
            Return _BaseColor
        End Get
        Set(ByVal value As Color)
            _BaseColor = value
        End Set
    End Property

    <Category("Colors")> _
    Public Property BorderColor As Color
        Get
            Return _BorderColor
        End Get
        Set(ByVal value As Color)
            _BorderColor = value
        End Set
    End Property

#End Region

#Region " Mouse States"

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None : Invalidate()
    End Sub

#End Region

#End Region

#Region " Colors"

    Private _BaseColor As Color = Color.FromArgb(45, 47, 49)
    Private _BorderColor As Color = _FlatColor
    Private _TextColor As Color = Color.FromArgb(243, 243, 243)

#End Region

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                 ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        DoubleBuffered = True
        BackColor = Color.FromArgb(60, 70, 73)
        Cursor = Cursors.Hand
        Font = New Font("Segoe UI", 10)
        Size = New Size(112, 22)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        B = New Bitmap(Width, Height) : G = Graphics.FromImage(B)
        W = Width - 1 : H = Height - 1

        Dim Base As New Rectangle(0, 2, Height - 5, Height - 5)

        With G
            .SmoothingMode = 2
            .TextRenderingHint = 5
            .Clear(BackColor)
            Select Case O
                Case _Options.Style1 '-- Style 1
                    '-- Base
                    .FillRectangle(New SolidBrush(_BaseColor), Base)

                    Select Case State
                        Case MouseState.Over
                            '-- Base
                            .DrawRectangle(New Pen(_BorderColor), Base)
                        Case MouseState.Down
                            '-- Base
                            .DrawRectangle(New Pen(_BorderColor), Base)
                    End Select

                    '-- If Checked
                    If Checked Then
                        .DrawString("?", New Font("Wingdings", 18), New SolidBrush(_BorderColor), New Rectangle(5, 7, H - 9, H - 9), CenterSF)
                    End If

                    '-- If Enabled
                    If Me.Enabled = False Then
                        .FillRectangle(New SolidBrush(Color.FromArgb(54, 58, 61)), Base)
                        .DrawString(Text, Font, New SolidBrush(Color.FromArgb(140, 142, 143)), New Rectangle(20, 2, W, H), NearSF)
                    End If

                    '-- Text
                    .DrawString(Text, Font, New SolidBrush(_TextColor), New Rectangle(20, 2, W, H), NearSF)
                Case _Options.Style2 '-- Style 2
                    '-- Base
                    .FillRectangle(New SolidBrush(_BaseColor), Base)

                    Select Case State
                        Case MouseState.Over
                            '-- Base
                            .DrawRectangle(New Pen(_BorderColor), Base)
                            .FillRectangle(New SolidBrush(Color.FromArgb(118, 213, 170)), Base)
                        Case MouseState.Down
                            '-- Base
                            .DrawRectangle(New Pen(_BorderColor), Base)
                            .FillRectangle(New SolidBrush(Color.FromArgb(118, 213, 170)), Base)
                    End Select

                    '-- If Checked
                    If Checked Then
                        .DrawString("?", New Font("Wingdings", 18), New SolidBrush(_BorderColor), New Rectangle(5, 7, H - 9, H - 9), CenterSF)
                    End If

                    '-- If Enabled
                    If Me.Enabled = False Then
                        .FillRectangle(New SolidBrush(Color.FromArgb(54, 58, 61)), Base)
                        .DrawString(Text, Font, New SolidBrush(Color.FromArgb(48, 119, 91)), New Rectangle(20, 2, W, H), NearSF)
                    End If

                    '-- Text
                    .DrawString(Text, Font, New SolidBrush(_TextColor), New Rectangle(20, 2, W, H), NearSF)
            End Select
        End With

        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
End Class

<DefaultEvent("TextChanged")> Class FlatTextBox : Inherits Control

#Region " Variables"

    Private W, H As Integer
    Private State As MouseState = MouseState.None
    Private WithEvents TB As Windows.Forms.TextBox

#End Region

#Region " Properties"

#Region " TextBox Properties"

    Private _TextAlign As HorizontalAlignment = HorizontalAlignment.Left
    <Category("Options")> _
    Property TextAlign() As HorizontalAlignment
        Get
            Return _TextAlign
        End Get
        Set(ByVal value As HorizontalAlignment)
            _TextAlign = value
            If TB IsNot Nothing Then
                TB.TextAlign = value
            End If
        End Set
    End Property
    Private _MaxLength As Integer = 32767
    <Category("Options")> _
    Property MaxLength() As Integer
        Get
            Return _MaxLength
        End Get
        Set(ByVal value As Integer)
            _MaxLength = value
            If TB IsNot Nothing Then
                TB.MaxLength = value
            End If
        End Set
    End Property
    Private _ReadOnly As Boolean
    <Category("Options")> _
    Property [ReadOnly]() As Boolean
        Get
            Return _ReadOnly
        End Get
        Set(ByVal value As Boolean)
            _ReadOnly = value
            If TB IsNot Nothing Then
                TB.ReadOnly = value
            End If
        End Set
    End Property
    Private _UseSystemPasswordChar As Boolean
    <Category("Options")> _
    Property UseSystemPasswordChar() As Boolean
        Get
            Return _UseSystemPasswordChar
        End Get
        Set(ByVal value As Boolean)
            _UseSystemPasswordChar = value
            If TB IsNot Nothing Then
                TB.UseSystemPasswordChar = value
            End If
        End Set
    End Property
    Private _Multiline As Boolean
    <Category("Options")> _
    Property Multiline() As Boolean
        Get
            Return _Multiline
        End Get
        Set(ByVal value As Boolean)
            _Multiline = value
            If TB IsNot Nothing Then
                TB.Multiline = value

                If value Then
                    TB.Height = Height - 11
                Else
                    Height = TB.Height + 11
                End If

            End If
        End Set
    End Property
    <Category("Options")> _
    Overrides Property Text As String
        Get
            Return MyBase.Text
        End Get
        Set(ByVal value As String)
            MyBase.Text = value
            If TB IsNot Nothing Then
                TB.Text = value
            End If
        End Set
    End Property
    <Category("Options")> _
    Overrides Property Font As Font
        Get
            Return MyBase.Font
        End Get
        Set(ByVal value As Font)
            MyBase.Font = value
            If TB IsNot Nothing Then
                TB.Font = value
                TB.Location = New Point(3, 5)
                TB.Width = Width - 6

                If Not _Multiline Then
                    Height = TB.Height + 11
                End If
            End If
        End Set
    End Property

    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        If Not Controls.Contains(TB) Then
            Controls.Add(TB)
        End If
    End Sub
    Private Sub OnBaseTextChanged(ByVal s As Object, ByVal e As EventArgs)
        Text = TB.Text
    End Sub
    Private Sub OnBaseKeyDown(ByVal s As Object, ByVal e As KeyEventArgs)
        If e.Control AndAlso e.KeyCode = Keys.A Then
            TB.SelectAll()
            e.SuppressKeyPress = True
        End If
        If e.Control AndAlso e.KeyCode = Keys.C Then
            TB.Copy()
            e.SuppressKeyPress = True
        End If
    End Sub
    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        TB.Location = New Point(5, 5)
        TB.Width = Width - 10

        If _Multiline Then
            TB.Height = Height - 11
        Else
            Height = TB.Height + 11
        End If

        MyBase.OnResize(e)
    End Sub

#End Region

#Region " Colors"

    <Category("Colors")> _
    Public Property TextColor As Color
        Get
            Return _TextColor
        End Get
        Set(ByVal value As Color)
            _TextColor = value
        End Set
    End Property

    Public Overrides Property ForeColor() As Color
        Get
            Return _TextColor
        End Get
        Set(ByVal value As Color)
            _TextColor = value
        End Set
    End Property

#End Region

#Region " Mouse States"

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over : TB.Focus() : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over : TB.Focus() : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None : Invalidate()
    End Sub

#End Region

#End Region

#Region " Colors"

    Private _BaseColor As Color = Color.FromArgb(45, 47, 49)
    Private _TextColor As Color = Color.FromArgb(192, 192, 192)
    Private _BorderColor As Color = _FlatColor

#End Region

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                 ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or _
                 ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True

        BackColor = Color.Transparent

        TB = New Windows.Forms.TextBox
        TB.Font = New Font("Segoe UI", 10)
        TB.Text = Text
        TB.BackColor = _BaseColor
        TB.ForeColor = _TextColor
        TB.MaxLength = _MaxLength
        TB.Multiline = _Multiline
        TB.ReadOnly = _ReadOnly
        TB.UseSystemPasswordChar = _UseSystemPasswordChar
        TB.BorderStyle = BorderStyle.None
        TB.Location = New Point(5, 5)
        TB.Width = Width - 10

        TB.Cursor = Cursors.IBeam

        If _Multiline Then
            TB.Height = Height - 11
        Else
            Height = TB.Height + 11
        End If

        AddHandler TB.TextChanged, AddressOf OnBaseTextChanged
        AddHandler TB.KeyDown, AddressOf OnBaseKeyDown
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        B = New Bitmap(Width, Height) : G = Graphics.FromImage(B)
        W = Width - 1 : H = Height - 1

        Dim Base As New Rectangle(0, 0, W, H)

        With G
            .SmoothingMode = 2
            .PixelOffsetMode = 2
            .TextRenderingHint = 5
            .Clear(BackColor)

            '-- Colors
            TB.BackColor = _BaseColor
            TB.ForeColor = _TextColor

            '-- Base
            .FillRectangle(New SolidBrush(_BaseColor), Base)
        End With

        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub

End Class

Class FlatTabControl : Inherits TabControl

#Region " Variables"

    Private W, H As Integer

#End Region

#Region " Properties"

    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        Alignment = TabAlignment.Top
    End Sub

#Region " Colors"

    <Category("Colors")> _
    Public Property ActiveColor As Color
        Get
            Return _ActiveColor
        End Get
        Set(ByVal value As Color)
            _ActiveColor = value
        End Set
    End Property

    '<Category("Colors")> _ '-- Arrow
    'Public Property IndicatorColor As Color
    ' **** ****Get
    ' **** **** **** ****Return _IndicatorColor
    ' **** ****End Get
    ' **** ****Set(value As Color)
    ' **** **** **** ****_IndicatorColor = value
    ' **** ****End Set
    'End Property

#End Region

#End Region

#Region " Colors"

    Private BaseColor As Color = Color.FromArgb(60, 70, 73)
    Private _ActiveColor As Color = _FlatColor
    'Private _IndicatorColor As Color = Color.FromArgb(44, 56, 54) '-- Arrow

#End Region

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                 ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        DoubleBuffered = True
        BackColor = Color.FromArgb(60, 70, 73)

        Font = New Font("Segoe UI", 10)
        SizeMode = TabSizeMode.Fixed
        ItemSize = New Size(120, 40)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        B = New Bitmap(Width, Height) : G = Graphics.FromImage(B)
        W = Width - 1 : H = Height - 1

        Dim GP As New GraphicsPath
        Dim PGB As PathGradientBrush

        With G
            .SmoothingMode = 2
            .PixelOffsetMode = 2
            .TextRenderingHint = 5
            .Clear(_ActiveColor)

            Try : SelectedTab.BackColor = BaseColor : Catch : End Try

            For i = 0 To TabCount - 1
                Dim Base As New Rectangle(New Point(GetTabRect(i).Location.X + 2, GetTabRect(i).Location.Y), New Size(GetTabRect(i).Width, GetTabRect(i).Height))
                Dim BaseSize As New Rectangle(Base.Location, New Size(Base.Width, Base.Height))

                If i = SelectedIndex Then
                    '-- Base
                    .FillRectangle(New SolidBrush(_ActiveColor), BaseSize)

                    GP.Reset()
                    GP.AddRectangle(BaseSize)

                    '-- Gradiant
                    PGB = New PathGradientBrush(GP)
                    With PGB
                        .CenterColor = _ActiveColor
                        .SurroundColors = {Color.FromArgb(45, BaseColor)}
                        .FocusScales = New PointF(0.5F, 0.5F)
                    End With
                    .FillPath(PGB, GP)

                    '-- ImageList
                    If ImageList IsNot Nothing Then
                        Try
                            If ImageList.Images(TabPages(i).ImageIndex) IsNot Nothing Then
                                '-- Image
                                .DrawImage(ImageList.Images(TabPages(i).ImageIndex), New Point(BaseSize.Location.X + 8, BaseSize.Location.Y + 6))
                                '-- Text
                                .DrawString(" **** **** ****" & TabPages(i).Text, Font, Brushes.White, BaseSize, CenterSF)
                            Else
                                '-- Text
                                .DrawString(TabPages(i).Text, Font, Brushes.White, BaseSize, CenterSF)
                            End If
                        Catch ex As Exception
                            Throw New Exception(ex.Message)
                        End Try
                    Else
                        '-- Text
                        .DrawString(TabPages(i).Text, Font, Brushes.White, BaseSize, CenterSF)
                    End If
                Else
                    '-- Base
                    .FillRectangle(New SolidBrush(_ActiveColor), BaseSize)

                    '-- ImageList
                    If ImageList IsNot Nothing Then
                        Try
                            If ImageList.Images(TabPages(i).ImageIndex) IsNot Nothing Then
                                '-- Image
                                .DrawImage(ImageList.Images(TabPages(i).ImageIndex), New Point(BaseSize.Location.X + 8, BaseSize.Location.Y + 6))
                                '-- Text
                                .DrawString(" **** **** ****" & TabPages(i).Text, Font, New SolidBrush(Color.White), BaseSize, New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
                            Else
                                '-- Text
                                .DrawString(TabPages(i).Text, Font, New SolidBrush(Color.White), BaseSize, New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
                            End If
                        Catch ex As Exception
                            Throw New Exception(ex.Message)
                        End Try
                    Else
                        '-- Text
                        .DrawString(TabPages(i).Text, Font, New SolidBrush(Color.White), BaseSize, New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
                    End If
                End If
            Next
        End With

        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
End Class

'Subscribe - http://bit.ly/2eUyZwy