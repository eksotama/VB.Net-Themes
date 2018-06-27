Imports System.Drawing.Drawing2D, System.ComponentModel, System.Windows.Forms
Imports System, System.IO, System.Collections.Generic
Imports System.Drawing
Imports System.Runtime.InteropServices
Imports System.Drawing.Imaging
''' <summary>
''' MetroDisk
''' Created by: iSynthesis (HF)
''' Modified by: King Aldrin
''' Version: 1.0.6
''' Date Crafted: 26/01/2014
''' Email: king.aldrin31@gmail.com
''' For any bugs / errors, PM me.
''' </summary>
''' <remarks></remarks>
''' to remove the text, goto line 99

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

    Public Function RoundRect(x!, y!, w!, h!, Optional r! = 0.3, Optional TL As Boolean = True, Optional TR As Boolean = True, Optional BR As Boolean = True, Optional BL As Boolean = True) As GraphicsPath
        Dim d! = Math.Min(w, h) * r, xw = x + w, yh = y + h
        RoundRect = New GraphicsPath

        With RoundRect
            If TL Then .AddArc(x, y, d, d, 180, 90) Else .AddLine(x, y, x, y)
            If TR Then .AddArc(xw - d, y, d, d, 270, 90) Else .AddLine(xw, y, xw, y)
            If BR Then .AddArc(xw - d, yh - d, d, d, 0, 90) Else .AddLine(xw, yh, xw, yh)
            If BL Then .AddArc(x, yh - d, d, d, 90, 90) Else .AddLine(x, yh, x, yh)

            .CloseFigure()
        End With
    End Function

    '-- Credit: AeonHack
    Public Function DrawArrow(x As Integer, y As Integer, flip As Boolean) As GraphicsPath
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

#Region "Controls"

Class FormSkin : Inherits ContainerControl

#Region " Variables"

    Private W, H As Integer
    Private Cap As Boolean = False
    Private _HeaderMaximize As Boolean = False
    Private MousePoint As New Point(0, 0)
    Private MoveHeight = 50
    Private _Theme As Boolean
    Private _MDcolor As Color
    Private _text As String = ""
    Private _Font = New Font("tahoma", 7)
#End Region

#Region " Properties"

#Region " Colors"

    <Category("Colors")> _
    Public Property HeaderColor() As Color
        Get
            Return _HeaderColor
        End Get
        Set(value As Color)
            _HeaderColor = value
        End Set
    End Property
    <Category("Colors")> _
    Public Property BaseColor() As Color
        Get
            Return _BaseColor
        End Get
        Set(value As Color)
            _BaseColor = value
        End Set
    End Property
    <Category("Colors")> _
    Public Property BorderColor() As Color
        Get
            Return _BorderColor
        End Get
        Set(value As Color)
            _BorderColor = value
        End Set
    End Property
    <Category("Colors")> _
    Public Property FlatColor() As Color
        Get
            Return _FlatColor
        End Get
        Set(value As Color)
            _FlatColor = value
        End Set
    End Property

#End Region

#Region " Options"

    <Category("Options")> _
    Public Property HeaderMaximize As Boolean
        Get
            Return _HeaderMaximize
        End Get
        Set(value As Boolean)
            _HeaderMaximize = value
        End Set
    End Property

#End Region

    Public Property LightTheme As Boolean
        Get
            Return _Theme
        End Get
        Set(value As Boolean)
            _Theme = value
        End Set
    End Property

    Public Property MDColor As Color
        Get
            Return _MDcolor
        End Get
        Set(value As Color)
            _MDcolor = value
        End Set
    End Property

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        If e.Button = Windows.Forms.MouseButtons.Left And New Rectangle(0, 0, Width, MoveHeight).Contains(e.Location) Then
            Cap = True
            MousePoint = e.Location
        End If
    End Sub

    Private Sub FormSkin_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles Me.MouseDoubleClick
        If HeaderMaximize Then
            If e.Button = Windows.Forms.MouseButtons.Left And New Rectangle(0, 0, Width, MoveHeight).Contains(e.Location) Then
                If FindForm.WindowState = FormWindowState.Normal Then
                    FindForm.WindowState = FormWindowState.Maximized : FindForm.Refresh()
                ElseIf FindForm.WindowState = FormWindowState.Maximized Then
                    FindForm.WindowState = FormWindowState.Normal : FindForm.Refresh()
                End If
            End If
        End If
    End Sub

    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e) : Cap = False
    End Sub

    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
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

#Region " Dark Colors"

    Private _HeaderColor As Color = Color.FromArgb(60, 200, 80)
    Private _BaseColor As Color = Color.FromArgb(60, 70, 73)
    Private _BorderColor As Color = Color.FromArgb(53, 58, 60)
    Private TextColor As Color = Color.FromArgb(234, 234, 234)

#End Region

#Region " Light Colors"

    Private _HeaderLight As Color = Color.FromArgb(171, 171, 172)
    Private _BaseLight As Color = Color.FromArgb(196, 199, 200)
    Public TextLight As Color = Color.FromArgb(45, 47, 49)

#End Region

#End Region

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                 ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        _MDcolor = Color.FromArgb(45, 150, 45)
        DoubleBuffered = True
        BackColor = Color.White
        Font = New Font("Segoe UI", 12)

    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        If _Theme = True Then
            'light
            _HeaderColor = Color.FromArgb(255, 255, 255)
            _BaseColor = Color.FromArgb(255, 255, 255)
            _BorderColor = Color.FromArgb(0, 0, 0)
            _BorderColor = Color.FromArgb(0, 0, 0)
        Else
            'dark
            _HeaderColor = Color.FromArgb(0, 0, 0)
            _BaseColor = Color.FromArgb(0, 0, 0)
            _BorderColor = Color.FromArgb(200, 200, 200)
            _BorderColor = Color.FromArgb(200, 200, 200)
        End If

        B = New Bitmap(Width, Height) : G = Graphics.FromImage(B)
        W = Width : H = Height
        Dim Base As New Rectangle(0, 0, W, H), Header As New Rectangle(0, 0, W, 40)

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
            .DrawString(Text, Font, New SolidBrush(TextColor), New Rectangle(23, 10, W, H), NearSF)
            .DrawString(_text, _Font, New SolidBrush(Color.DimGray), New Rectangle(W - 120, H - 15, W, H), NearSF)
            .FillRectangle(New SolidBrush(_MDcolor), New Rectangle(1, 40, 12, 50))

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

Class MDGroupBox : Inherits ContainerControl

#Region " Variables"

    Private W, H As Integer
    Private _ShowText As Boolean = True
    Private _Arrows As Boolean = True
    Private _LightTheme As Boolean
    Private _Curve As Integer = 1
#End Region

#Region " Properties"

    <Category("Colors")> _
    Public Property BaseColor As Color
        Get
            Return _BaseColor
        End Get
        Set(value As Color)
            _BaseColor = value
        End Set
    End Property

    Public Property ShowText As Boolean
        Get
            Return _ShowText
        End Get
        Set(value As Boolean)
            _ShowText = value
        End Set
    End Property

    Public Property Arrows As Boolean
        Get
            Return _Arrows
        End Get
        Set(value As Boolean)
            _Arrows = value
        End Set
    End Property

    Public Property LightTheme As Boolean
        Get
            Return _LightTheme
        End Get
        Set(value As Boolean)
            _LightTheme = value
        End Set
    End Property

    Public Property Curve As Integer
        Get
            Return _Curve
        End Get
        Set(value As Integer)
            _Curve = value
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

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        If _LightTheme Then
            'light
            _BaseColor = Color.FromArgb(240, 240, 240)
        Else
            'dark
            _BaseColor = Color.FromArgb(20, 20, 20)
        End If

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

            GP = Helpers.RoundRec(Base, _Curve)
            .FillPath(New SolidBrush(_BaseColor), GP)
            'curve here


            If _Arrows Then
                '-- Arrows
                GP2 = Helpers.DrawArrow(28, 2, False)
                .FillPath(New SolidBrush(_BaseColor), GP2)
                GP3 = Helpers.DrawArrow(28, 8, True)
                If _LightTheme Then
                    .FillPath(New SolidBrush(Color.FromArgb(240, 240, 240)), GP3)
                Else
                    .FillPath(New SolidBrush(Color.FromArgb(20, 20, 20)), GP3)
                End If

            End If


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

Class MDButton : Inherits Control

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
        Set(value As Color)
            _BaseColor = value
        End Set
    End Property

    <Category("Colors")> _
    Public Property TextColor As Color
        Get
            Return _TextColor
        End Get
        Set(value As Color)
            _TextColor = value
        End Set
    End Property

    <Category("Options")> _
    Public Property Rounded As Boolean
        Get
            Return _Rounded
        End Get
        Set(value As Boolean)
            _Rounded = value
        End Set
    End Property

#End Region

#Region " Mouse States"

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(e As EventArgs)
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

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
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

<DefaultEvent("CheckedChanged")> Class MDToggle : Inherits Control

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
        Set(value As _Options)
            O = value
        End Set
    End Property

    <Category("Options")> _
    Public Property Checked As Boolean
        Get
            Return _Checked
        End Get
        Set(value As Boolean)
            _Checked = value
        End Set
    End Property

#End Region

    Protected Overrides Sub OnTextChanged(e As EventArgs)
        MyBase.OnTextChanged(e) : Invalidate()
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
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
    Protected Overrides Sub OnClick(e As EventArgs)
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

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
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
                        .DrawString("ü", New Font("Wingdings", 14), New SolidBrush(TextColor), New Rectangle(8, 7, Width, Height), NearSF)
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

<DefaultEvent("CheckedChanged")> Class MDRadioButton : Inherits Control

#Region " Variables"

    Private State As MouseState = MouseState.None
    Private W, H As Integer
    Private O As _Options
    Private _Checked As Boolean

#End Region

#Region " Properties"
    Property Checked() As Boolean
        Get
            Return _Checked
        End Get
        Set(value As Boolean)
            _Checked = value
            InvalidateControls()
            RaiseEvent CheckedChanged(Me)
            Invalidate()
        End Set
    End Property
    Event CheckedChanged(ByVal sender As Object)
    Protected Overrides Sub OnClick(e As EventArgs)
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

    <Flags> _
    Enum _Options
        Style1
        Style2
    End Enum

    <Category("Options")> _
    Public Property Options As _Options
        Get
            Return O
        End Get
        Set(value As _Options)
            O = value
        End Set
    End Property

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        Height = 22
    End Sub

#Region " Mouse States"

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None : Invalidate()
    End Sub

#End Region
#End Region

#Region " Colors"

    Private _BaseColor As Color = Color.FromArgb(45, 47, 49)
    Private _BorderColor As Color = Color.FromArgb(100, 100, 100)
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
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        B = New Bitmap(Width, Height) : G = Graphics.FromImage(B)
        W = Width - 1 : H = Height - 1

        Dim Base As New Rectangle(0, 2, Height - 5, Height - 5), Dot As New Rectangle(4, 6, H - 12, H - 12)

        With G
            .SmoothingMode = 2
            .TextRenderingHint = 5
            .Clear(BackColor)

            Select Case O
                Case _Options.Style1
                    '-- Base
                    .FillEllipse(New SolidBrush(_BaseColor), Base)

                    Select Case State
                        Case MouseState.Over
                            .DrawEllipse(New Pen(_BorderColor), Base)
                        Case MouseState.Down
                            .DrawEllipse(New Pen(_BorderColor), Base)
                    End Select

                    '-- If Checked 
                    If Checked Then
                        .FillEllipse(New SolidBrush(_BorderColor), Dot)
                    End If
                Case _Options.Style2
                    '-- Base
                    .FillEllipse(New SolidBrush(_BaseColor), Base)

                    Select Case State
                        Case MouseState.Over
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
            End Select

            .DrawString(Text, Font, New SolidBrush(_TextColor), New Rectangle(20, 2, W, H), NearSF)
        End With

        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
End Class

<DefaultEvent("CheckedChanged")> Class MDCheckBox : Inherits Control

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

    <Flags> _
    Enum _Options
        Style1
        Style2
    End Enum

    <Category("Options")> _
    Public Property Options As _Options
        Get
            Return O
        End Get
        Set(value As _Options)
            O = value
        End Set
    End Property

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        Height = 22
    End Sub

#Region " Colors"

    <Category("Colors")> _
    Public Property BaseColor As Color
        Get
            Return _BaseColor
        End Get
        Set(value As Color)
            _BaseColor = value
        End Set
    End Property

    <Category("Colors")> _
    Public Property BorderColor As Color
        Get
            Return _BorderColor
        End Get
        Set(value As Color)
            _BorderColor = value
        End Set
    End Property

#End Region

#Region " Mouse States"

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None : Invalidate()
    End Sub

#End Region

#End Region

#Region " Colors"

    Private _BaseColor As Color = Color.FromArgb(45, 47, 49)
    Private _BorderColor As Color = _FlatColor
    Private _TextColor As Color

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

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        _TextColor = ForeColor
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
                        .DrawString("P", New Font("Wingdings 2", 18), New SolidBrush(_BorderColor), New Rectangle(5, 7, H - 9, H - 9), CenterSF)
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
                            .FillRectangle(New SolidBrush(Color.FromArgb(45, 47, 49)), Base)
                    End Select

                    '-- If Checked
                    If Checked Then
                        .DrawString("ü", New Font("Wingdings", 18), New SolidBrush(_BorderColor), New Rectangle(5, 7, H - 9, H - 9), CenterSF)
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

<DefaultEvent("TextChanged")> Class MDTextBox : Inherits Control

#Region " Variables"

    Private W, H As Integer
    Private State As MouseState = MouseState.None
    Private WithEvents TB As Windows.Forms.TextBox
    Private _Theme As Boolean

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

    Public Property LightTheme As Boolean
        Get
            Return _Theme
        End Get
        Set(value As Boolean)
            _Theme = value
        End Set
    End Property

#End Region

#Region " Colors"

    <Category("Colors")> _
    Public Property TextColor As Color
        Get
            Return _TextColor
        End Get
        Set(value As Color)
            _TextColor = value
        End Set
    End Property

    Public Overrides Property ForeColor() As Color
        Get
            Return _TextColor
        End Get
        Set(value As Color)
            _TextColor = value
        End Set
    End Property

#End Region

#Region " Mouse States"

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None : Invalidate()
    End Sub

#End Region

#End Region

#Region " Colors"

    'true = light | false = dark

    Private _BaseColor As Color = Color.FromArgb(200, 200, 200)
    Private _TextColor As Color = Color.FromArgb(192, 192, 192)
    Private _BorderColor As Color = _FlatColor

#End Region

    Sub New()
        If _Theme = True Then
            _BaseColor = Color.FromArgb(200, 200, 200)
            _TextColor = Color.FromArgb(0, 0, 0)
            _BorderColor = Color.Black

            SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                 ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or _
                 ControlStyles.SupportsTransparentBackColor, True)
            DoubleBuffered = True

            BackColor = Color.Transparent

            TB = New Windows.Forms.TextBox
            TB.Font = New Font("Segoe UI", 10, FontStyle.Bold)
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
        Else
            _BaseColor = Color.FromArgb(0, 0, 0)
            _TextColor = Color.FromArgb(200, 200, 200)

            SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                 ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or _
                 ControlStyles.SupportsTransparentBackColor, True)
            DoubleBuffered = True

            BackColor = Color.Transparent

            TB = New Windows.Forms.TextBox
            TB.Font = New Font("Segoe UI", 10, FontStyle.Bold)
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
        End If
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)

        If _Theme = True Then
            _BaseColor = Color.FromArgb(255, 255, 255)
            _TextColor = Color.FromArgb(175, 175, 175)
            _FlatColor = Color.FromArgb(0, 0, 0)
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

                .DrawRectangle(New Pen(Color.FromArgb(200, 200, 200)), New Rectangle(0, 0, W, H))
            End With

            MyBase.OnPaint(e)
            G.Dispose()
            e.Graphics.InterpolationMode = 7
            e.Graphics.DrawImageUnscaled(B, 0, 0)
            B.Dispose()
        Else
            _BaseColor = Color.FromArgb(50, 50, 50)
            _TextColor = Color.FromArgb(200, 200, 200)
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

                .DrawRectangle(New Pen(Color.FromArgb(25, 25, 25)), New Rectangle(0, 0, W, H))
            End With

            MyBase.OnPaint(e)
            G.Dispose()
            e.Graphics.InterpolationMode = 7
            e.Graphics.DrawImageUnscaled(B, 0, 0)
            B.Dispose()
        End If
    End Sub

End Class

Class MDTabControl : Inherits TabControl

#Region " Variables"

    Private W, H As Integer
    Private _Theme As Boolean

#End Region

#Region " Properties"

    Public Property LightTheme As Boolean
        Get
            Return _Theme
        End Get
        Set(value As Boolean)
            _Theme = value
        End Set
    End Property

    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        Alignment = TabAlignment.Top
    End Sub

#Region " Colors"

    <Category("Colors")> _
    Public Property BaseColor As Color
        Get
            Return _BaseColor
        End Get
        Set(value As Color)
            _BaseColor = value
        End Set
    End Property

    <Category("Colors")> _
    Public Property ActiveColor As Color
        Get
            Return _ActiveColor
        End Get
        Set(value As Color)
            _ActiveColor = value
        End Set
    End Property

#End Region

#End Region

#Region " Colors"

    Private BGColor As Color = Color.FromArgb(0, 0, 0)
    Private _BaseColor As Color = Color.FromArgb(45, 47, 49)
    Private _ActiveColor As Color = _FlatColor
    Private _unactive As Brush
    Private _active As Brush
    Private unactive As Color
    Private active As Color

#End Region

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                 ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        DoubleBuffered = True
        BackColor = Color.FromArgb(60, 70, 73)

        Font = New Font("Segoe UI", 15)
        SizeMode = TabSizeMode.Fixed
        ItemSize = New Size(140, 40)

    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)

        If _Theme = True Then
            'light
            BGColor = Color.FromArgb(255, 255, 255)
            _BaseColor = Color.FromArgb(255, 255, 255)
            ActiveColor = Color.FromArgb(225, 225, 225)
            _active = Brushes.Silver
            _unactive = Brushes.DimGray
            active = Color.Silver
            unactive = Color.DimGray

        Else
            'dark
            BGColor = Color.FromArgb(0, 0, 0)
            _BaseColor = Color.FromArgb(0, 0, 0)
            ActiveColor = Color.FromArgb(25, 25, 25)
            _active = Brushes.DimGray
            _active = Brushes.Silver
            active = Color.DimGray
            unactive = Color.Silver
        End If

        B = New Bitmap(Width, Height) : G = Graphics.FromImage(B)
        W = Width - 1 : H = Height - 1

        With G
            .SmoothingMode = 2
            .PixelOffsetMode = 2
            .TextRenderingHint = 5
            .Clear(_BaseColor)

            Try : SelectedTab.BackColor = BGColor : Catch : End Try

            For i = 0 To TabCount - 1
                Dim Base As New Rectangle(New Point(GetTabRect(i).Location.X + 2, GetTabRect(i).Location.Y), New Size(GetTabRect(i).Width, GetTabRect(i).Height))
                Dim BaseSize As New Rectangle(Base.Location, New Size(Base.Width, Base.Height))

                If i = SelectedIndex Then
                    '-- Base
                    '.FillRectangle(New SolidBrush(_BaseColor), BaseSize)

                    '-- Gradiant
                    '.fill
                    '.FillRectangle(New SolidBrush(_ActiveColor), BaseSize)

                    '-- ImageList
                    If ImageList IsNot Nothing Then
                        Try
                            If ImageList.Images(TabPages(i).ImageIndex) IsNot Nothing Then
                                '-- Image
                                .DrawImage(ImageList.Images(TabPages(i).ImageIndex), New Point(BaseSize.Location.X + 8, BaseSize.Location.Y + 6))
                                '-- Text
                                .DrawString("      " & "MD" & TabPages(i).Text, Font, _unactive, BaseSize, CenterSF)
                            Else
                                '-- Text
                                .DrawString(TabPages(i).Text, Font, _active, BaseSize, CenterSF)
                            End If
                        Catch ex As Exception
                            Throw New Exception(ex.Message)
                        End Try
                    Else
                        '-- Text
                        '.DrawString(TabPages(i).Text, Font, Brushes.White, BaseSize, CenterSF)
                        .DrawString(TabPages(i).Text, Font, New SolidBrush(unactive), BaseSize, New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
                    End If
                Else
                    '-- Base
                    .FillRectangle(New SolidBrush(_BaseColor), BaseSize)

                    '-- ImageList
                    If ImageList IsNot Nothing Then
                        Try
                            If ImageList.Images(TabPages(i).ImageIndex) IsNot Nothing Then
                                '-- Image
                                .DrawImage(ImageList.Images(TabPages(i).ImageIndex), New Point(BaseSize.Location.X + 8, BaseSize.Location.Y + 6))
                                '-- Text
                                .DrawString("      " & TabPages(i).Text, Font, New SolidBrush(Color.White), BaseSize, New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
                            Else
                                '-- Text
                                .DrawString(TabPages(i).Text, Font, New SolidBrush(active), BaseSize, New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
                            End If
                        Catch ex As Exception
                            Throw New Exception(ex.Message)
                        End Try
                    Else
                        '-- Text
                        .DrawString(TabPages(i).Text, Font, New SolidBrush(active), BaseSize, New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})


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

Class MDAlertBox : Inherits Control

    ''' <summary>
    ''' How to use: FlatAlertBox.ShowControl(Kind, String, Interval)
    ''' </summary>
    ''' <remarks></remarks>

#Region " Variables"

    Private W, H As Integer
    Private K As _Kind
    Private _Text As String
    Private State As MouseState = MouseState.None
    Private X As Integer
    Private WithEvents T As Timer

#End Region

#Region " Properties"

    <Flags()> _
    Enum _Kind
        [Success]
        [Error]
        [Info]
    End Enum

#Region " Options"

    <Category("Options")> _
    Public Property kind As _Kind
        Get
            Return K
        End Get
        Set(value As _Kind)
            K = value
        End Set
    End Property

    <Category("Options")> _
    Overrides Property Text As String
        Get
            Return MyBase.Text
        End Get
        Set(ByVal value As String)
            MyBase.Text = value
            If _Text IsNot Nothing Then
                _Text = value
            End If
        End Set
    End Property

    <Category("Options")> _
    Shadows Property Visible As Boolean
        Get
            Return MyBase.Visible = False
        End Get
        Set(value As Boolean)
            MyBase.Visible = value
        End Set
    End Property

#End Region

    Protected Overrides Sub OnTextChanged(e As EventArgs)
        MyBase.OnTextChanged(e) : Invalidate()
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        Height = 42
    End Sub

    Public Sub ShowControl(Kind As _Kind, Str As String, Interval As Integer)
        K = Kind
        Text = Str
        Me.Visible = True
        T = New Timer
        T.Interval = Interval
        T.Enabled = True
    End Sub

    Private Sub T_Tick(sender As Object, e As EventArgs) Handles T.Tick
        Me.Visible = False
        T.Enabled = False
        T.Dispose()
    End Sub

#Region " Mouse States"

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)
        X = e.X : Invalidate()
    End Sub

    Protected Overrides Sub OnClick(e As EventArgs)
        MyBase.OnClick(e)
        Me.Visible = False
    End Sub

#End Region

#End Region

#Region " Colors"

    Private SuccessColor As Color = Color.FromArgb(60, 85, 79)
    Private SuccessText As Color = Color.FromArgb(35, 169, 110)
    Private ErrorColor As Color = Color.FromArgb(87, 71, 71)
    Private ErrorText As Color = Color.FromArgb(254, 142, 122)
    Private InfoColor As Color = Color.FromArgb(70, 91, 94)
    Private InfoText As Color = Color.FromArgb(97, 185, 186)

#End Region

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                 ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        DoubleBuffered = True
        BackColor = Color.FromArgb(60, 70, 73)
        Size = New Size(50, 42)
        Location = New Point(10, 61)
        Font = New Font("Segoe UI", 10)
        Cursor = Cursors.Hand
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        B = New Bitmap(Width, Height) : G = Graphics.FromImage(B)
        W = Width - 1 : H = Height - 1

        Dim Base As New Rectangle(0, 0, W, H)

        With G
            .SmoothingMode = 2

            .Clear(BackColor)

            Select Case K
                Case _Kind.Success
                    '-- Base
                    .FillRectangle(New SolidBrush(SuccessColor), Base)

                    '-- Ellipse
                    .FillEllipse(New SolidBrush(SuccessText), New Rectangle(8, 9, 24, 24))
                    .FillEllipse(New SolidBrush(SuccessColor), New Rectangle(10, 11, 20, 20))

                    '-- Checked Sign
                    .DrawString("ü", New Font("Wingdings", 22), New SolidBrush(SuccessText), New Rectangle(7, 7, W, H), NearSF)
                    .DrawString(Text, Font, New SolidBrush(SuccessText), New Rectangle(48, 12, W, H), NearSF)

                    '-- X button
                    .FillEllipse(New SolidBrush(Color.FromArgb(35, Color.Black)), New Rectangle(W - 30, H - 29, 17, 17))
                    .DrawString("r", New Font("Marlett", 8), New SolidBrush(SuccessColor), New Rectangle(W - 28, 16, W, H), NearSF)

                    Select Case State ' -- Mouse Over
                        Case MouseState.Over
                            .DrawString("r", New Font("Marlett", 8), New SolidBrush(Color.FromArgb(25, Color.White)), New Rectangle(W - 28, 16, W, H), NearSF)
                    End Select

                Case _Kind.Error
                    '-- Base
                    .FillRectangle(New SolidBrush(ErrorColor), Base)

                    '-- Ellipse
                    .FillEllipse(New SolidBrush(ErrorText), New Rectangle(8, 9, 24, 24))
                    .FillEllipse(New SolidBrush(ErrorColor), New Rectangle(10, 11, 20, 20))

                    '-- X Sign
                    .DrawString("r", New Font("Marlett", 16), New SolidBrush(ErrorText), New Rectangle(6, 11, W, H), NearSF)
                    .DrawString(Text, Font, New SolidBrush(ErrorText), New Rectangle(48, 12, W, H), NearSF)

                    '-- X button
                    .FillEllipse(New SolidBrush(Color.FromArgb(35, Color.Black)), New Rectangle(W - 32, H - 29, 17, 17))
                    .DrawString("r", New Font("Marlett", 8), New SolidBrush(ErrorColor), New Rectangle(W - 30, 17, W, H), NearSF)

                    Select Case State
                        Case MouseState.Over ' -- Mouse Over
                            .DrawString("r", New Font("Marlett", 8), New SolidBrush(Color.FromArgb(25, Color.White)), New Rectangle(W - 30, 15, W, H), NearSF)
                    End Select

                Case _Kind.Info
                    '-- Base
                    .FillRectangle(New SolidBrush(InfoColor), Base)

                    '-- Ellipse
                    .FillEllipse(New SolidBrush(InfoText), New Rectangle(8, 9, 24, 24))
                    .FillEllipse(New SolidBrush(InfoColor), New Rectangle(10, 11, 20, 20))

                    '-- Info Sign
                    .DrawString("¡", New Font("Segoe UI", 20, FontStyle.Bold), New SolidBrush(InfoText), New Rectangle(12, -4, W, H), NearSF)
                    .DrawString(Text, Font, New SolidBrush(InfoText), New Rectangle(48, 12, W, H), NearSF)

                    '-- X button
                    .FillEllipse(New SolidBrush(Color.FromArgb(35, Color.Black)), New Rectangle(W - 32, H - 29, 17, 17))
                    .DrawString("r", New Font("Marlett", 8), New SolidBrush(InfoColor), New Rectangle(W - 30, 17, W, H), NearSF)

                    Select Case State
                        Case MouseState.Over ' -- Mouse Over
                            .DrawString("r", New Font("Marlett", 8), New SolidBrush(Color.FromArgb(25, Color.White)), New Rectangle(W - 30, 17, W, H), NearSF)
                    End Select
            End Select

        End With

        MyBase.OnPaint(e)
        G.Dispose()

        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub

End Class

Class MDProgressBar : Inherits Control

#Region " Variables"

    Private W, H As Integer
    Private _Value As Integer = 0
    Private _Maximum As Integer = 100

#End Region

#Region " Properties"

#Region " Control"

    <Category("Control")>
    Public Property Maximum() As Integer
        Get
            Return _Maximum
        End Get
        Set(V As Integer)
            Select Case V
                Case Is < _Value
                    _Value = V
            End Select
            _Maximum = V
            Invalidate()
        End Set
    End Property

    <Category("Control")>
    Public Property Value() As Integer
        Get
            Select Case _Value
                Case 0
                    Return 0
                    Invalidate()
                Case Else
                    Return _Value
                    Invalidate()
            End Select
        End Get
        Set(V As Integer)
            Select Case V
                Case Is > _Maximum
                    V = _Maximum
                    Invalidate()
            End Select
            _Value = V
            Invalidate()
        End Set
    End Property

#End Region

#Region " Colors"

    <Category("Colors")>
    Public Property ProgressColor As Color
        Get
            Return _ProgressColor
        End Get
        Set(value As Color)
            _ProgressColor = value
        End Set
    End Property

    <Category("Colors")>
    Public Property DarkerProgress As Color
        Get
            Return _DarkerProgress
        End Get
        Set(value As Color)
            _DarkerProgress = value
        End Set
    End Property

#End Region

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        Height = 42
    End Sub

    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        Height = 42
    End Sub

    Public Sub Increment(ByVal Amount As Integer)
        Value += Amount
    End Sub

#End Region

#Region " Colors"

    Private _BaseColor As Color = Color.FromArgb(45, 47, 49)
    Private _ProgressColor As Color = _FlatColor
    Private _DarkerProgress As Color = Color.FromArgb(23, 148, 92)

#End Region

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                 ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        DoubleBuffered = True
        BackColor = Color.FromArgb(60, 70, 73)
        Height = 42
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        B = New Bitmap(Width, Height) : G = Graphics.FromImage(B)
        W = Width - 1 : H = Height - 1

        Dim Base As New Rectangle(0, 24, W, H)
        Dim GP, GP2, GP3 As New GraphicsPath

        With G
            .SmoothingMode = 2
            .PixelOffsetMode = 2
            .TextRenderingHint = 5
            .Clear(BackColor)

            '-- Progress Value
            Dim iValue As Integer = CInt(_Value / _Maximum * Width)

            Select Case Value
                Case 0
                    '-- Base
                    .FillRectangle(New SolidBrush(_BaseColor), Base)
                    '--Progress
                    .FillRectangle(New SolidBrush(_ProgressColor), New Rectangle(0, 24, iValue - 1, H - 1))
                Case 100
                    '-- Base
                    .FillRectangle(New SolidBrush(_BaseColor), Base)
                    '--Progress
                    .FillRectangle(New SolidBrush(_ProgressColor), New Rectangle(0, 24, iValue - 1, H - 1))
                Case Else
                    '-- Base
                    .FillRectangle(New SolidBrush(_BaseColor), Base)

                    '--Progress
                    GP.AddRectangle(New Rectangle(0, 24, iValue - 1, H - 1))
                    .FillPath(New SolidBrush(_ProgressColor), GP)

                    '-- Hatch Brush
                    'Dim HB As New HatchBrush(HatchStyle.Plaid, _DarkerProgress, _ProgressColor)
                    '.FillRectangle(HB, New Rectangle(0, 24, iValue - 1, H - 1))

                    '-- Balloon
                    'Dim Balloon As New Rectangle(iValue - 18, 25, 34, 16)
                    'GP2 = Helpers.RoundRec(Balloon, 4)
                    '.FillPath(New SolidBrush(_BaseColor), GP2)

                    '-- Arrow
                    ''GP3 = Helpers.DrawArrow(iValue - 9, 16, True)
                    ''.FillPath(New SolidBrush(_BaseColor), GP3)

                    '-- Value > You can add "%" > value & "%"
                    '.DrawString(Value, New Font("Segoe UI", 10), New SolidBrush(_ProgressColor), New Rectangle(iValue - 11, 25, W, H), NearSF)
            End Select
        End With

        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub

End Class

Class MDComboBox : Inherits Windows.Forms.ComboBox

#Region " Variables"

    Private W, H As Integer
    Private _StartIndex As Integer = 0
    Private x, y As Integer
    Private _LightTheme As Boolean

#End Region

#Region " Properties"

#Region " Mouse States"

    Private State As MouseState = MouseState.None
    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)
        x = e.Location.X
        y = e.Location.Y
        Invalidate()
        If e.X < Width - 41 Then Cursor = Cursors.IBeam Else Cursor = Cursors.Hand
    End Sub

    Protected Overrides Sub OnDrawItem(e As DrawItemEventArgs)
        MyBase.OnDrawItem(e) : Invalidate()
        If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
            Invalidate()
        End If
    End Sub

    Protected Overrides Sub OnClick(e As EventArgs)
        MyBase.OnClick(e) : Invalidate()
    End Sub

#End Region

#Region " Colors"

    <Category("Colors")> _
    Public Property HoverColor As Color
        Get
            Return _HoverColor
        End Get
        Set(value As Color)
            _HoverColor = value
        End Set
    End Property

#End Region

    Public Property LightTheme As Boolean
        Get
            Return _LightTheme
        End Get
        Set(value As Boolean)
            _LightTheme = value
        End Set
    End Property

    Private Property StartIndex As Integer
        Get
            Return _StartIndex
        End Get
        Set(ByVal value As Integer)
            _StartIndex = value
            Try
                MyBase.SelectedIndex = value
            Catch
            End Try
            Invalidate()
        End Set
    End Property

    Sub DrawItem_(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles Me.DrawItem
        If e.Index < 0 Then Exit Sub
        e.DrawBackground()
        e.DrawFocusRectangle()

        e.Graphics.SmoothingMode = 2
        e.Graphics.PixelOffsetMode = 2
        e.Graphics.TextRenderingHint = 5
        e.Graphics.InterpolationMode = 7

        If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
            '-- Selected item
            e.Graphics.FillRectangle(New SolidBrush(_HoverColor), e.Bounds)
        Else
            '-- Not Selected
            e.Graphics.FillRectangle(New SolidBrush(_BaseColor), e.Bounds)
        End If

        '-- Text
        e.Graphics.DrawString(MyBase.GetItemText(MyBase.Items(e.Index)), New Font("Segoe UI", 8), _
                     Brushes.White, New Rectangle(e.Bounds.X + 2, e.Bounds.Y + 2, e.Bounds.Width, e.Bounds.Height))


        e.Graphics.Dispose()
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        Height = 18
    End Sub

#End Region

#Region " Colors"

    Private _BaseColor As Color = Color.FromArgb(25, 27, 29)
    Private _BGColor As Color = Color.FromArgb(45, 47, 49)
    Private _HoverColor As Color = Color.FromArgb(35, 168, 109)

#End Region

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                 ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        DoubleBuffered = True

        DrawMode = DrawMode.OwnerDrawFixed
        BackColor = Color.FromArgb(45, 45, 48)
        ForeColor = Color.White
        DropDownStyle = ComboBoxStyle.DropDownList
        Cursor = Cursors.Hand
        StartIndex = 0
        ItemHeight = 18
        Font = New Font("Segoe UI", 8, FontStyle.Regular)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        If _LightTheme Then
            _BaseColor = Color.FromArgb(200, 200, 200)
            _BGColor = Color.FromArgb(250, 250, 250)
            _HoverColor = Color.FromArgb(150, 150, 150)
            ForeColor = Color.FromArgb(20, 20, 20)
        Else
            _BaseColor = Color.FromArgb(25, 25, 25)
            _BGColor = Color.FromArgb(40, 40, 40)
            _HoverColor = Color.FromArgb(80, 80, 80)
            ForeColor = Color.FromArgb(100, 100, 100)
        End If
        B = New Bitmap(Width, Height) : G = Graphics.FromImage(B)
        W = Width : H = Height

        Dim Base As New Rectangle(0, 0, W, H)
        Dim Button As New Rectangle(CInt(W - 40), 0, W, H)
        Dim GP, GP2 As New GraphicsPath

        With G
            .Clear(Color.FromArgb(45, 45, 48))
            .SmoothingMode = 2
            .PixelOffsetMode = 2
            .TextRenderingHint = 5

            '-- Base
            .FillRectangle(New SolidBrush(_BGColor), Base)

            '-- Button
            GP.Reset()
            GP.AddRectangle(Button)
            .SetClip(GP)
            .FillRectangle(New SolidBrush(_BaseColor), Button)
            .ResetClip()

            '-- Lines
            .DrawLine(Pens.White, W - 10, 6, W - 30, 6)
            .DrawLine(Pens.White, W - 10, 12, W - 30, 12)
            .DrawLine(Pens.White, W - 10, 18, W - 30, 18)

            '-- Text
            .DrawString(Text, Font, Brushes.White, New Point(4, 6), NearSF)
        End With

        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
End Class

Class MDStickyButton : Inherits Control

#Region " Variables"

    Private W, H As Integer
    Private State As MouseState = MouseState.None
    Private _Rounded As Boolean = False

#End Region

#Region " Properties"

#Region " MouseStates"

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None : Invalidate()
    End Sub

#End Region

#Region " Function"

    Private Function GetConnectedSides() As Boolean()
        Dim Bool = New Boolean(3) {False, False, False, False}

        For Each B As Control In Parent.Controls

        Next

        Return Bool
    End Function

    Private ReadOnly Property Rect() As Rectangle
        Get
            Return New Rectangle(Left, Top, Width, Height)
        End Get
    End Property

#End Region

#Region " Colors"

    <Category("Colors")> _
    Public Property BaseColor As Color
        Get
            Return _BaseColor
        End Get
        Set(value As Color)
            _BaseColor = value
        End Set
    End Property

    <Category("Colors")> _
    Public Property TextColor As Color
        Get
            Return _TextColor
        End Get
        Set(value As Color)
            _TextColor = value
        End Set
    End Property

    <Category("Options")> _
    Public Property Rounded As Boolean
        Get
            Return _Rounded
        End Get
        Set(value As Boolean)
            _Rounded = value
        End Set
    End Property

#End Region

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        'Height = 32
    End Sub

    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        'Size = New Size(112, 32)
    End Sub

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

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        B = New Bitmap(Width, Height) : G = Graphics.FromImage(B)
        W = Width : H = Height

        Dim GP As New GraphicsPath

        Dim GCS = GetConnectedSides()
        Dim RoundedBase = Helpers.RoundRect(0, 0, W, H, , Not (GCS(2) Or GCS(1)), Not (GCS(1) Or GCS(0)), Not (GCS(3) Or GCS(0)), Not (GCS(3) Or GCS(2)))
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
                        GP = RoundedBase
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
                        GP = RoundedBase
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
                        GP = RoundedBase
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

Class MDNumeric : Inherits Control

#Region " Variables"

    Private W, H As Integer
    Private State As MouseState = MouseState.None
    Private x, y As Integer
    Private _Value, _Min, _Max As Long
    Private Bool As Boolean

#End Region

#Region " Properties"

    Public Property Value As Long
        Get
            Return _Value
        End Get
        Set(value As Long)
            If value <= _Max And value >= _Min Then _Value = value
            Invalidate()
        End Set
    End Property

    Public Property Maximum As Long
        Get
            Return _Max
        End Get
        Set(value As Long)
            If value > _Min Then _Max = value
            If _Value > _Max Then _Value = _Max
            Invalidate()
        End Set
    End Property

    Public Property Minimum As Long
        Get
            Return _Min
        End Get
        Set(value As Long)
            If value < _Max Then _Min = value
            If _Value < _Min Then _Value = Minimum
            Invalidate()
        End Set
    End Property

    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)
        x = e.Location.X
        y = e.Location.Y
        Invalidate()
        If e.X < Width - 23 Then Cursor = Cursors.IBeam Else Cursor = Cursors.Hand
    End Sub

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        If x > Width - 21 AndAlso x < Width - 3 Then
            If y < 15 Then
                If (Value + 1) <= _Max Then _Value += 1
            Else
                If (Value - 1) >= _Min Then _Value -= 1
            End If
        Else
            Bool = Not Bool
            Focus()
        End If
        Invalidate()
    End Sub

    Protected Overrides Sub OnKeyPress(e As KeyPressEventArgs)
        MyBase.OnKeyPress(e)
        Try
            If Bool Then _Value = CStr(CStr(_Value) & e.KeyChar.ToString())
            If _Value > _Max Then _Value = _Max
            Invalidate()
        Catch : End Try
    End Sub

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        MyBase.OnKeyDown(e)
        If e.KeyCode = Keys.Back Then
            Value = 0
        End If
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        Height = 30
    End Sub

#Region " Colors"

    <Category("Colors")> _
    Public Property BaseColor As Color
        Get
            Return _BaseColor
        End Get
        Set(value As Color)
            _BaseColor = value
        End Set
    End Property

    <Category("Colors")> _
    Public Property ButtonColor As Color
        Get
            Return _ButtonColor
        End Get
        Set(value As Color)
            _ButtonColor = value
        End Set
    End Property

#End Region

#End Region

#Region " Colors"

    Private _BaseColor As Color = Color.FromArgb(45, 47, 49)
    Private _ButtonColor As Color = _FlatColor

#End Region

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
        ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or _
        ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        Font = New Font("Segoe UI", 10)
        BackColor = Color.FromArgb(60, 70, 73)
        ForeColor = Color.White
        _Min = 0
        _Max = 9999999
    End Sub


    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        B = New Bitmap(Width, Height) : G = Graphics.FromImage(B)
        W = Width : H = Height

        Dim Base As New Rectangle(0, 0, W, H)

        With G
            .SmoothingMode = 2
            .PixelOffsetMode = 2
            .TextRenderingHint = 5
            .Clear(BackColor)

            '-- Base
            .FillRectangle(New SolidBrush(_BaseColor), Base)
            .FillRectangle(New SolidBrush(_ButtonColor), New Rectangle(Width - 24, 0, 24, H))

            '-- Add
            .DrawString("+", New Font("Segoe UI", 12), Brushes.White, New Point(Width - 12, 8), CenterSF)
            '-- Subtract
            .DrawString("-", New Font("Segoe UI", 10, FontStyle.Bold), Brushes.White, New Point(Width - 12, 22), CenterSF)

            '-- Text
            .DrawString(Value, Font, Brushes.White, New Rectangle(5, 1, W, H), New StringFormat() With {.LineAlignment = StringAlignment.Center})
        End With

        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub

End Class

Class MDListBox : Inherits Control

#Region " Variables"

    Private WithEvents ListBx As New ListBox
    Private _items As String() = {""}
    Private _LightTheme As Boolean

#End Region

#Region " Poperties"

    Public Property LightTheme As Boolean
        Get
            Return _LightTheme
        End Get
        Set(value As Boolean)
            _LightTheme = value
        End Set
    End Property

    <Category("Options")> _
    Public Property items As String()
        Get
            Return _items
        End Get
        Set(value As String())
            _items = value
            ListBx.Items.Clear()
            ListBx.Items.AddRange(value)
            Invalidate()
        End Set
    End Property

    <Category("Colors")> _
    Public Property SelectedColor As Color
        Get
            Return _SelectedColor
        End Get
        Set(value As Color)
            _SelectedColor = value
        End Set
    End Property

    Public ReadOnly Property SelectedItem() As String
        Get
            Return ListBx.SelectedItem
        End Get
    End Property

    Public ReadOnly Property SelectedIndex() As Integer
        Get
            Return ListBx.SelectedIndex
            If ListBx.SelectedIndex < 0 Then Exit Property
        End Get
    End Property

    Public Sub Clear()
        ListBx.Items.Clear()
    End Sub

    Public Sub ClearSelected()
        For i As Integer = (ListBx.SelectedItems.Count - 1) To 0 Step -1
            ListBx.Items.Remove(ListBx.SelectedItems(i))
        Next
    End Sub

    Sub Drawitem(ByVal sender As Object, ByVal e As DrawItemEventArgs) Handles ListBx.DrawItem
        If e.Index < 0 Then Exit Sub
        e.DrawBackground()
        e.DrawFocusRectangle()

        e.Graphics.SmoothingMode = 2
        e.Graphics.PixelOffsetMode = 2
        e.Graphics.InterpolationMode = 7
        e.Graphics.TextRenderingHint = 5

        If InStr(e.State.ToString, "Selected,") > 0 Then '-- if selected
            '-- Base
            e.Graphics.FillRectangle(New SolidBrush(_SelectedColor), New Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height))

            '-- Text
            e.Graphics.DrawString(" " & ListBx.Items(e.Index).ToString(), New Font("Segoe UI", 8), Brushes.White, e.Bounds.X, e.Bounds.Y + 2)
        Else
            '-- Base
            e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(51, 53, 55)), New Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height))

            '-- Text 
            e.Graphics.DrawString(" " & ListBx.Items(e.Index).ToString(), New Font("Segoe UI", 8), Brushes.White, e.Bounds.X, e.Bounds.Y + 2)
        End If

        e.Graphics.Dispose()
    End Sub

    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        If Not Controls.Contains(ListBx) Then
            Controls.Add(ListBx)
        End If
    End Sub

    Sub AddRange(ByVal items As Object())
        ListBx.Items.Remove("")
        ListBx.Items.AddRange(items)
    End Sub

    Sub AddItem(ByVal item As Object)
        ListBx.Items.Remove("")
        ListBx.Items.Add(item)
    End Sub

#End Region

#Region " Colors"

    Private BaseColor As Color = Color.FromArgb(45, 47, 49)
    Private _SelectedColor As Color = _FlatColor

#End Region

    Sub New()
        If _LightTheme Then
            BaseColor = Color.FromArgb(255, 255, 255)
            ForeColor = Color.FromArgb(0, 0, 0)
            _SelectedColor = Color.FromArgb(200, 200, 200)
        Else
            BaseColor = Color.FromArgb(0, 0, 0)
            ForeColor = Color.FromArgb(255, 255, 255)
            _SelectedColor = Color.FromArgb(20, 20, 20)
        End If
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
            ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        DoubleBuffered = True

        ListBx.DrawMode = Windows.Forms.DrawMode.OwnerDrawFixed
        ListBx.ScrollAlwaysVisible = False
        ListBx.HorizontalScrollbar = False
        ListBx.BorderStyle = BorderStyle.None
        ListBx.BackColor = BaseColor
        'ListBx.ForeColor = Color.White
        ListBx.Location = New Point(3, 3)
        ListBx.Font = New Font("Segoe UI", 8)
        ListBx.ItemHeight = 20
        ListBx.Items.Clear()
        ListBx.IntegralHeight = False

        Size = New Size(131, 101)
        BackColor = BaseColor
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        If _LightTheme Then
            BaseColor = Color.FromArgb(255, 255, 255)
            ForeColor = Color.FromArgb(0, 0, 0)
        Else
            BaseColor = Color.FromArgb(0, 0, 0)
            ForeColor = Color.FromArgb(255, 255, 255)
        End If
        B = New Bitmap(Width, Height) : G = Graphics.FromImage(B)

        Dim Base As New Rectangle(0, 0, Width, Height)

        With G
            .SmoothingMode = 2
            .PixelOffsetMode = 2
            .TextRenderingHint = 5
            .Clear(BackColor)

            '-- Size
            ListBx.Size = New Size(Width - 6, Height - 2)

            '-- Base
            .FillRectangle(New SolidBrush(BaseColor), Base)
        End With

        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub

End Class

Class MDContextMenuStrip : Inherits ContextMenuStrip
    Private _LightTheme As Boolean
    Public Property LightTheme As Boolean
        Get
            Return _LightTheme
        End Get
        Set(value As Boolean)
            _LightTheme = value
        End Set
    End Property

    Protected Overrides Sub OnTextChanged(e As EventArgs)
        MyBase.OnTextChanged(e) : Invalidate()
    End Sub

    Sub New()
        MyBase.New()
        Renderer = New ToolStripProfessionalRenderer(New TColorTable())
        ShowImageMargin = False
        ForeColor = Color.White
        Font = New Font("Segoe UI", 8)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        If _LightTheme Then

        End If
        MyBase.OnPaint(e)
        e.Graphics.TextRenderingHint = 5
    End Sub

    Class TColorTable : Inherits ProfessionalColorTable

#Region " Properties"

#Region " Colors"

        <Category("Colors")> _
        Public Property _BackColor As Color
            Get
                Return BackColor
            End Get
            Set(value As Color)
                BackColor = value
            End Set
        End Property

        <Category("Colors")> _
        Public Property _CheckedColor As Color
            Get
                Return CheckedColor
            End Get
            Set(value As Color)
                CheckedColor = value
            End Set
        End Property

        <Category("Colors")> _
        Public Property _BorderColor As Color
            Get
                Return BorderColor
            End Get
            Set(value As Color)
                BorderColor = value
            End Set
        End Property

#End Region

#End Region

#Region " Colors"

        Private BackColor As Color = Color.FromArgb(45, 47, 49)
        Private CheckedColor As Color = _FlatColor
        Private BorderColor As Color = Color.FromArgb(53, 58, 60)

#End Region

#Region " Overrides"

        Public Overrides ReadOnly Property ButtonSelectedBorder As Color
            Get
                Return BackColor
            End Get
        End Property
        Public Overrides ReadOnly Property CheckBackground() As Color
            Get
                Return CheckedColor
            End Get
        End Property
        Public Overrides ReadOnly Property CheckPressedBackground() As Color
            Get
                Return CheckedColor
            End Get
        End Property
        Public Overrides ReadOnly Property CheckSelectedBackground() As Color
            Get
                Return CheckedColor
            End Get
        End Property
        Public Overrides ReadOnly Property ImageMarginGradientBegin() As Color
            Get
                Return CheckedColor
            End Get
        End Property
        Public Overrides ReadOnly Property ImageMarginGradientEnd() As Color
            Get
                Return CheckedColor
            End Get
        End Property
        Public Overrides ReadOnly Property ImageMarginGradientMiddle() As Color
            Get
                Return CheckedColor
            End Get
        End Property
        Public Overrides ReadOnly Property MenuBorder() As Color
            Get
                Return BorderColor
            End Get
        End Property
        Public Overrides ReadOnly Property MenuItemBorder() As Color
            Get
                Return BorderColor
            End Get
        End Property
        Public Overrides ReadOnly Property MenuItemSelected() As Color
            Get
                Return CheckedColor
            End Get
        End Property
        Public Overrides ReadOnly Property SeparatorDark() As Color
            Get
                Return BorderColor
            End Get
        End Property
        Public Overrides ReadOnly Property ToolStripDropDownBackground() As Color
            Get
                Return BackColor
            End Get
        End Property

#End Region

    End Class

End Class

<DefaultEvent("Scroll")> Class MDTrackBar : Inherits Control

#Region " Variables"

    Private W, H As Integer
    Private Val As Integer
    Private Bool As Boolean
    Private Track As Rectangle
    Private Knob As Rectangle
    Private Style_ As _Style
    Private _LightTheme As Boolean

#End Region

#Region " Properties"

#Region " Mouse States"

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        If e.Button = Windows.Forms.MouseButtons.Left Then
            Val = CInt((_Value - _Minimum) / (_Maximum - _Minimum) * (Width - 11))
            Track = New Rectangle(Val, 0, 10, 20)

            Bool = Track.Contains(e.Location)
        End If
    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)
        MyBase.OnMouseMove(e)
        If Bool AndAlso e.X > -1 AndAlso e.X < (Width + 1) Then
            Value = _Minimum + CInt((_Maximum - _Minimum) * (e.X / Width))
        End If
    End Sub

    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        MyBase.OnMouseUp(e) : Bool = False
    End Sub

#End Region

#Region " Styles"

    <Flags> _
    Enum _Style
        Slider
        Knob
    End Enum

    Public Property Style As _Style
        Get
            Return Style_
        End Get
        Set(value As _Style)
            Style_ = value
        End Set
    End Property

#End Region

#Region " Colors"

    <Category("Colors")> _
    Public Property TrackColor As Color
        Get
            Return _TrackColor
        End Get
        Set(value As Color)
            _TrackColor = value
        End Set
    End Property

    <Category("Colors")> _
    Public Property HatchColor As Color
        Get
            Return _HatchColor
        End Get
        Set(value As Color)
            _HatchColor = value
        End Set
    End Property

#End Region

    Public Property LightTheme As Boolean
        Get
            Return _LightTheme
        End Get
        Set(value As Boolean)
            _LightTheme = value
        End Set
    End Property

    Event Scroll(ByVal sender As Object)
    Private _Minimum As Integer
    Public Property Minimum As Integer
        Get
            Return Minimum
        End Get
        Set(value As Integer)
            If value < 0 Then
            End If

            _Minimum = value

            If value > _Value Then _Value = value
            If value > _Maximum Then _Maximum = value
            Invalidate()
        End Set
    End Property
    Private _Maximum As Integer = 10
    Public Property Maximum As Integer
        Get
            Return _Maximum
        End Get
        Set(value As Integer)
            If value < 0 Then
            End If

            _Maximum = value
            If value < _Value Then _Value = value
            If value < _Minimum Then _Minimum = value
            Invalidate()
        End Set
    End Property
    Private _Value As Integer
    Public Property Value As Integer
        Get
            Return _Value
        End Get
        Set(value As Integer)
            If value = _Value Then Return

            If value > _Maximum OrElse value < _Minimum Then
            End If

            _Value = value
            Invalidate()
            RaiseEvent Scroll(Me)
        End Set
    End Property
    Private _ShowValue As Boolean = False
    Public Property ShowValue As Boolean
        Get
            Return _ShowValue
        End Get
        Set(value As Boolean)
            _ShowValue = value
        End Set
    End Property

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        MyBase.OnKeyDown(e)
        If e.KeyCode = Keys.Subtract Then
            If Value = 0 Then Exit Sub
            Value -= 1
        ElseIf e.KeyCode = Keys.Add Then
            If Value = _Maximum Then Exit Sub
            Value += 1
        End If
    End Sub

    Protected Overrides Sub OnTextChanged(e As EventArgs)
        MyBase.OnTextChanged(e) : Invalidate()
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        Height = 23
    End Sub

#End Region

#Region " Colors"

    Private BaseColor As Color = Color.FromArgb(45, 47, 49)
    Private _TrackColor As Color = _FlatColor
    Private SliderColor As Color = Color.FromArgb(25, 27, 29)
    Private _HatchColor As Color = Color.FromArgb(23, 148, 92)

#End Region

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                 ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        DoubleBuffered = True
        Height = 18

        BackColor = Color.FromArgb(60, 70, 73)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        If _LightTheme Then
            BaseColor = Color.FromArgb(0, 0, 0)
            _TrackColor = Color.FromArgb(200, 200, 200)
            SliderColor = Color.FromArgb(225, 225, 225)
            _HatchColor = Color.FromArgb(150, 150, 150)
        Else
            BaseColor = Color.FromArgb(250, 250, 250)
            _TrackColor = Color.FromArgb(100, 100, 100)
            SliderColor = Color.FromArgb(20, 20, 20)
            _HatchColor = Color.FromArgb(50, 50, 50)
        End If

        B = New Bitmap(Width, Height) : G = Graphics.FromImage(B)
        W = Width - 1 : H = Height - 1

        Dim Base As New Rectangle(1, 6, W - 2, 8)
        Dim GP, GP2 As New GraphicsPath

        With G
            .SmoothingMode = 2
            .PixelOffsetMode = 2
            .TextRenderingHint = 5
            .Clear(BackColor)

            '-- Value
            Val = CInt((_Value - _Minimum) / (_Maximum - _Minimum) * (W - 10))
            Track = New Rectangle(Val, 0, 10, 20)
            Knob = New Rectangle(Val, 4, 11, 14)

            '-- Base
            GP.AddRectangle(Base)
            .SetClip(GP)
            .FillRectangle(New SolidBrush(BaseColor), New Rectangle(0, 7, W, 8))
            .FillRectangle(New SolidBrush(_TrackColor), New Rectangle(0, 7, Track.X + Track.Width, 8))
            .ResetClip()

            '-- Hatch Brush
            'Dim HB As New HatchBrush(HatchStyle.Plaid, HatchColor, _TrackColor)
            '.FillRectangle(HB, New Rectangle(-10, 7, Track.X + Track.Width, 8))

            '-- Slider/Knob
            Select Case Style
                Case _Style.Slider
                    GP2.AddRectangle(Track)
                    .FillPath(New SolidBrush(SliderColor), GP2)
                Case _Style.Knob
                    GP2.AddEllipse(Knob)
                    .FillPath(New SolidBrush(SliderColor), GP2)
            End Select

            '-- Show the value 
            If ShowValue Then
                .DrawString(Value, New Font("Segoe UI", 8), Brushes.White, New Rectangle(1, 6, W, H), New StringFormat() _
                            With {.Alignment = StringAlignment.Far, .LineAlignment = StringAlignment.Far})
            End If
        End With

        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
End Class

Class MDStatusBar : Inherits Control

#Region " Variables"

    Private W, H As Integer
    Private _ShowTimeDate As Boolean = False
    Private _LightTheme As Boolean = False

#End Region

#Region " Properties"

    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        Dock = DockStyle.Bottom
    End Sub

    Protected Overrides Sub OnTextChanged(e As EventArgs)
        MyBase.OnTextChanged(e) : Invalidate()
    End Sub

#Region " Colors"

    <Category("Colors")> _
    Public Property BaseColor As Color
        Get
            Return _BaseColor
        End Get
        Set(value As Color)
            _BaseColor = value
        End Set
    End Property

    <Category("Colors")> _
    Public Property TextColor As Color
        Get
            Return _TextColor
        End Get
        Set(value As Color)
            _TextColor = value
        End Set
    End Property

    <Category("Colors")> _
    Public Property RectColor As Color
        Get
            Return _RectColor
        End Get
        Set(value As Color)
            _RectColor = value
        End Set
    End Property

#End Region

    Public Property ShowTimeDate As Boolean
        Get
            Return _ShowTimeDate
        End Get
        Set(value As Boolean)
            _ShowTimeDate = value
        End Set
    End Property

    Public Property LightTheme As Boolean
        Get
            Return _LightTheme
        End Get
        Set(value As Boolean)
            _LightTheme = value
        End Set
    End Property

    Function GetTimeDate() As String
        Return DateTime.Now.Date & " " & DateTime.Now.Hour & ":" & DateTime.Now.Minute
    End Function

#End Region

#Region " Colors"

    Private _BaseColor As Color = Color.FromArgb(45, 47, 49)
    Private _TextColor As Color = Color.White
    Private _RectColor As Color = _FlatColor

#End Region

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                 ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        DoubleBuffered = True
        Font = New Font("Segoe UI", 8)
        ForeColor = Color.White
        Size = New Size(Width, 20)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        If _LightTheme Then
            'light
            _BaseColor = Color.FromArgb(255, 255, 255)
            _TextColor = Color.FromArgb(0, 0, 0)
            ForeColor = Color.FromArgb(0, 0, 0)
        Else
            'dark
            _BaseColor = Color.FromArgb(0, 0, 0)
            _TextColor = Color.FromArgb(255, 255, 255)
            ForeColor = Color.FromArgb(255, 255, 255)
        End If
        B = New Bitmap(Width, Height) : G = Graphics.FromImage(B)
        W = Width : H = Height

        Dim Base As New Rectangle(2, 2, W - 10, H - 10)

        With G
            .SmoothingMode = 2
            .PixelOffsetMode = 2
            .TextRenderingHint = 5
            .Clear(BaseColor)

            '-- Base
            .FillRectangle(New SolidBrush(BaseColor), Base)

            '-- Text
            .DrawString(Text, Font, Brushes.White, New Rectangle(10, 4, W, H), NearSF)

            '-- Rectangle
            .FillRectangle(New SolidBrush(_RectColor), New Rectangle(4, 4, 4, 14))

            '-- TimeDate
            If ShowTimeDate Then
                .DrawString(GetTimeDate, Font, New SolidBrush(_TextColor), New Rectangle(-4, 2, W, H), New StringFormat() _
                            With {.Alignment = StringAlignment.Far, .LineAlignment = StringAlignment.Center})
            End If
        End With

        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
End Class

Class MDLabel : Inherits Label

    Protected Overrides Sub OnTextChanged(e As EventArgs)
        MyBase.OnTextChanged(e) : Invalidate()
    End Sub

    Sub New()
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)
        Font = New Font("Segoe UI", 8)
        ForeColor = Color.White
        BackColor = Color.Transparent
        Text = Text
    End Sub

End Class

Class MDTreeView : Inherits TreeView

#Region " Variables"

    Private State As TreeNodeStates

#End Region

#Region " Properties"

    Protected Overrides Sub OnDrawNode(e As DrawTreeNodeEventArgs)
        Try
            Dim Bounds As New Rectangle(e.Bounds.Location.X, e.Bounds.Location.Y, e.Bounds.Width, e.Bounds.Height)
            'e.Node.Nodes.Item.
            Select Case State
                Case TreeNodeStates.Default
                    e.Graphics.FillRectangle(Brushes.Red, Bounds)
                    e.Graphics.DrawString(e.Node.Text, New Font("Segoe UI", 8), Brushes.LimeGreen, New Rectangle(Bounds.X + 2, Bounds.Y + 2, Bounds.Width, Bounds.Height), NearSF)
                    Invalidate()
                Case TreeNodeStates.Checked
                    e.Graphics.FillRectangle(Brushes.Green, Bounds)
                    e.Graphics.DrawString(e.Node.Text, New Font("Segoe UI", 8), Brushes.Black, New Rectangle(Bounds.X + 2, Bounds.Y + 2, Bounds.Width, Bounds.Height), NearSF)
                    Invalidate()
                Case TreeNodeStates.Selected
                    e.Graphics.FillRectangle(Brushes.Green, Bounds)
                    e.Graphics.DrawString(e.Node.Text, New Font("Segoe UI", 8), Brushes.Black, New Rectangle(Bounds.X + 2, Bounds.Y + 2, Bounds.Width, Bounds.Height), NearSF)
                    Invalidate()
            End Select

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        MyBase.OnDrawNode(e)
    End Sub

#End Region

#Region " Colors"

    Private _BaseColor As Color = Color.FromArgb(45, 47, 49)
    Private _LineColor As Color = Color.FromArgb(25, 27, 29)

#End Region
    Sub New()

        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                 ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        DoubleBuffered = True

        BackColor = _BaseColor
        ForeColor = Color.White
        LineColor = _LineColor
        DrawMode = TreeViewDrawMode.OwnerDrawAll
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        B = New Bitmap(Width, Height) : G = Graphics.FromImage(B)

        Dim Base As New Rectangle(0, 0, Width, Height)

        With G
            .SmoothingMode = 2
            .PixelOffsetMode = 2
            .TextRenderingHint = 5
            .Clear(BackColor)

            .FillRectangle(New SolidBrush(_BaseColor), Base)
            .DrawString(Text, New Font("Segoe UI", 8), Brushes.Black, New Rectangle(Bounds.X + 2, Bounds.Y + 2, Bounds.Width, Bounds.Height), NearSF)

        End With

        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub

End Class

Class MDIcon : Inherits Windows.Forms.Button

#Region " Variables"

    Private _MDColor_border As Color
    Private _MDColor_click As Color
    Private _MDColor_hover As Color

#End Region

#Region " Properties"
    Public Property MDcolor_border As Color
        Get
            Return _MDColor_border
        End Get
        Set(value As Color)
            _MDColor_border = value
        End Set
    End Property

    Public Property MDcolor_click As Color
        Get
            Return _MDColor_click
        End Get
        Set(value As Color)
            _MDColor_click = value
        End Set
    End Property

    Public Property MDcolor_hover As Color
        Get
            Return _MDColor_hover
        End Get
        Set(value As Color)
            _MDColor_hover = value
        End Set
    End Property

#End Region


    Sub New()
        Me.Size = New System.Drawing.Point(33, 33)
        Me.Text = ""
        Me.FlatStyle = Windows.Forms.FlatStyle.Flat
        Me.BackColor = BackColor
        Me.Font = New System.Drawing.Font("Segoe UI", 8, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = Drawing.Color.White
        Me.FlatAppearance.BorderColor = _MDColor_border
        Me.FlatAppearance.MouseDownBackColor = _MDColor_click
        Me.FlatAppearance.MouseOverBackColor = _MDColor_hover
        Me.FlatAppearance.BorderSize = 2
    End Sub

    Private Sub Metro_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
        Me.BackColor = _MDColor_click
    End Sub

    Private Sub Metro_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseEnter
        Me.BackColor = _MDColor_hover
    End Sub

    Private Sub MDIcon_MouseHover(sender As Object, e As EventArgs) Handles Me.MouseHover
        Me.BackColor = _MDColor_hover
    End Sub

    Private Sub Metro_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseLeave
        Me.BackColor = BackColor
    End Sub

    Private Sub Metro_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseUp
        Me.BackColor = _MDColor_hover
    End Sub

End Class

Class MDlblButton : Inherits Windows.Forms.Label

#Region " Variables"

    Private _MDColor_normal As Color
    Private _MDColor_click As Color
    Private _MDColor_hover As Color

#End Region

#Region " Properties"
    Public Property MDcolor_normal As Color
        Get
            Return _MDColor_normal
        End Get
        Set(value As Color)
            _MDColor_normal = value
        End Set
    End Property

    Public Property MDcolor_click As Color
        Get
            Return _MDColor_click
        End Get
        Set(value As Color)
            _MDColor_click = value
        End Set
    End Property

    Public Property MDcolor_hover As Color
        Get
            Return _MDColor_hover
        End Get
        Set(value As Color)
            _MDColor_hover = value
        End Set
    End Property

#End Region


    Sub New()
        _MDColor_click = Color.DarkGray
        _MDColor_hover = Color.LightGray
        _MDColor_normal = Color.Silver


        Me.FlatStyle = Windows.Forms.FlatStyle.Flat
        Me.BackColor = Color.Transparent
        Me.Font = New System.Drawing.Font("Segoe UI", 8, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = _MDColor_normal
    End Sub

    Private Sub Metro_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
        Me.ForeColor = _MDColor_click
    End Sub

    Private Sub Metro_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseEnter
        Me.ForeColor = _MDColor_hover
    End Sub

    Private Sub MDIcon_MouseHover(sender As Object, e As EventArgs) Handles Me.MouseHover
        Me.ForeColor = _MDColor_hover
    End Sub

    Private Sub Metro_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseLeave
        Me.ForeColor = _MDColor_normal
    End Sub

    Private Sub Metro_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseUp
        Me.ForeColor = _MDColor_hover
    End Sub

End Class

Class DialogSkin : Inherits ContainerControl

#Region " Variables"

    Private W, H As Integer
    Private Cap As Boolean = False
    Private _HeaderMaximize As Boolean = False
    Private MousePoint As New Point(0, 0)
    Private MoveHeight = 50
    Private _MDcolor As Color
    Private _text As String = "MetroDisk by SilverMachine"
    Private _Font = New Font("tahoma", 7)
    Private __Font = New Font("Segoe UI", 18, FontStyle.Bold)
#End Region

#Region " Properties"

#Region " Colors"

    <Category("Colors")> _
    Public Property HeaderColor() As Color
        Get
            Return _HeaderColor
        End Get
        Set(value As Color)
            _HeaderColor = value
        End Set
    End Property
    <Category("Colors")> _
    Public Property BaseColor() As Color
        Get
            Return _BaseColor
        End Get
        Set(value As Color)
            _BaseColor = value
        End Set
    End Property
    <Category("Colors")> _
    Public Property BorderColor() As Color
        Get
            Return _BorderColor
        End Get
        Set(value As Color)
            _BorderColor = value
        End Set
    End Property
    <Category("Colors")> _
    Public Property FlatColor() As Color
        Get
            Return _FlatColor
        End Get
        Set(value As Color)
            _FlatColor = value
        End Set
    End Property

#End Region

#Region " Options"

    <Category("Options")> _
    Public Property HeaderMaximize As Boolean
        Get
            Return _HeaderMaximize
        End Get
        Set(value As Boolean)
            _HeaderMaximize = value
        End Set
    End Property

#End Region

    Public Property MDColor As Color
        Get
            Return _MDcolor
        End Get
        Set(value As Color)
            _MDcolor = value
        End Set
    End Property

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        If e.Button = Windows.Forms.MouseButtons.Left And New Rectangle(0, 0, Width, MoveHeight).Contains(e.Location) Then
            Cap = True
            MousePoint = e.Location
        End If
    End Sub

    Private Sub FormSkin_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles Me.MouseDoubleClick
        If HeaderMaximize Then
            If e.Button = Windows.Forms.MouseButtons.Left And New Rectangle(0, 0, Width, MoveHeight).Contains(e.Location) Then
                If FindForm.WindowState = FormWindowState.Normal Then
                    FindForm.WindowState = FormWindowState.Maximized : FindForm.Refresh()
                ElseIf FindForm.WindowState = FormWindowState.Maximized Then
                    FindForm.WindowState = FormWindowState.Normal : FindForm.Refresh()
                End If
            End If
        End If
    End Sub

    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e) : Cap = False
    End Sub

    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
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

#Region " Dark Colors"

    Private _HeaderColor As Color = Color.FromArgb(60, 200, 80)
    Private _BaseColor As Color = Color.FromArgb(60, 70, 73)
    Private _BorderColor As Color = Color.FromArgb(53, 58, 60)
    Private TextColor As Color = Color.FromArgb(234, 234, 234)

#End Region

#Region " Light Colors"

    Private _HeaderLight As Color = Color.FromArgb(171, 171, 172)
    Private _BaseLight As Color = Color.FromArgb(196, 199, 200)
    Public TextLight As Color = Color.FromArgb(45, 47, 49)

#End Region

#End Region

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                 ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        _MDcolor = Color.FromArgb(45, 150, 45)
        DoubleBuffered = True
        BackColor = Color.White
        Font = New Font("Segoe UI", 12)

    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
            _HeaderColor = Color.FromArgb(255, 255, 255)
            _BaseColor = Color.FromArgb(255, 255, 255)
        _BorderColor = Color.FromArgb(245, 245, 245)

        B = New Bitmap(Width, Height) : G = Graphics.FromImage(B)
        W = Width : H = Height
        Dim Base As New Rectangle(0, 0, W, H), Header As New Rectangle(0, 0, W, 40)

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
            .DrawString(Text, __Font, New SolidBrush(Color.Black), New Rectangle(20, 20, W, H), NearSF)
            .DrawString(_text, _Font, New SolidBrush(Color.DimGray), New Rectangle(W - 120, H - 15, W, H), NearSF)
            .FillRectangle(New SolidBrush(_MDcolor), New Rectangle(20, 15, 150, 5))

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

#Region "ThemeBase"


'------------------
'Creator: aeonhack
'Site: elitevs.net
'Created: 08/02/2011
'Changed: 12/06/2011
'Version: 1.5.4
'------------------

MustInherit Class ThemeContainer154
    Inherits ContainerControl

#Region " Initialization "

    Protected G As Graphics, B As Bitmap

    Sub New()
        SetStyle(DirectCast(139270, ControlStyles), True)

        _ImageSize = Size.Empty
        Font = New Font("Verdana", 8S)

        MeasureBitmap = New Bitmap(1, 1)
        MeasureGraphics = Graphics.FromImage(MeasureBitmap)

        DrawRadialPath = New GraphicsPath

        InvalidateCustimization()
    End Sub

    Protected NotOverridable Overrides Sub OnHandleCreated(ByVal e As EventArgs)
        If DoneCreation Then InitializeMessages()

        InvalidateCustimization()
        ColorHook()

        If Not _LockWidth = 0 Then Width = _LockWidth
        If Not _LockHeight = 0 Then Height = _LockHeight
        If Not _ControlMode Then MyBase.Dock = DockStyle.Fill

        Transparent = _Transparent
        If _Transparent AndAlso _BackColor Then BackColor = Color.Transparent

        MyBase.OnHandleCreated(e)
    End Sub

    Private DoneCreation As Boolean
    Protected NotOverridable Overrides Sub OnParentChanged(ByVal e As EventArgs)
        MyBase.OnParentChanged(e)

        If Parent Is Nothing Then Return
        _IsParentForm = TypeOf Parent Is Form

        If Not _ControlMode Then
            InitializeMessages()

            If _IsParentForm Then
                ParentForm.FormBorderStyle = _BorderStyle
                ParentForm.TransparencyKey = _TransparencyKey

                If Not DesignMode Then
                    AddHandler ParentForm.Shown, AddressOf FormShown
                End If
            End If

            Parent.BackColor = BackColor
        End If

        OnCreation()
        DoneCreation = True
        InvalidateTimer()
    End Sub

#End Region

    Private Sub DoAnimation(ByVal i As Boolean)
        OnAnimation()
        If i Then Invalidate()
    End Sub

    Protected NotOverridable Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        If Width = 0 OrElse Height = 0 Then Return

        If _Transparent AndAlso _ControlMode Then
            PaintHook()
            e.Graphics.DrawImage(B, 0, 0)
        Else
            G = e.Graphics
            PaintHook()
        End If
    End Sub

    Protected Overrides Sub OnHandleDestroyed(ByVal e As EventArgs)
        RemoveAnimationCallback(AddressOf DoAnimation)
        MyBase.OnHandleDestroyed(e)
    End Sub

    Private HasShown As Boolean
    Private Sub FormShown(ByVal sender As Object, ByVal e As EventArgs)
        If _ControlMode OrElse HasShown Then Return

        If _StartPosition = FormStartPosition.CenterParent OrElse _StartPosition = FormStartPosition.CenterScreen Then
            Dim SB As Rectangle = Screen.PrimaryScreen.Bounds
            Dim CB As Rectangle = ParentForm.Bounds
            ParentForm.Location = New Point(SB.Width \ 2 - CB.Width \ 2, SB.Height \ 2 - CB.Width \ 2)
        End If

        HasShown = True
    End Sub


#Region " Size Handling "

    Private Frame As Rectangle
    Protected NotOverridable Overrides Sub OnSizeChanged(ByVal e As EventArgs)
        If _Movable AndAlso Not _ControlMode Then
            Frame = New Rectangle(7, 7, Width - 14, _Header - 7)
        End If

        InvalidateBitmap()
        Invalidate()

        MyBase.OnSizeChanged(e)
    End Sub

    Protected Overrides Sub SetBoundsCore(ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal specified As BoundsSpecified)
        If Not _LockWidth = 0 Then width = _LockWidth
        If Not _LockHeight = 0 Then height = _LockHeight
        MyBase.SetBoundsCore(x, y, width, height, specified)
    End Sub

#End Region

#Region " State Handling "

    Protected State As MouseState
    Private Sub SetState(ByVal current As MouseState)
        State = current
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)
        If Not (_IsParentForm AndAlso ParentForm.WindowState = FormWindowState.Maximized) Then
            If _Sizable AndAlso Not _ControlMode Then InvalidateMouse()
        End If

        MyBase.OnMouseMove(e)
    End Sub

    Protected Overrides Sub OnEnabledChanged(ByVal e As EventArgs)
        If Enabled Then SetState(MouseState.None) Else SetState(MouseState.Block)
        MyBase.OnEnabledChanged(e)
    End Sub

    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        SetState(MouseState.Over)
        MyBase.OnMouseEnter(e)
    End Sub

    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        SetState(MouseState.Over)
        MyBase.OnMouseUp(e)
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        SetState(MouseState.None)

        If GetChildAtPoint(PointToClient(MousePosition)) IsNot Nothing Then
            If _Sizable AndAlso Not _ControlMode Then
                Cursor = Cursors.Default
                Previous = 0
            End If
        End If

        MyBase.OnMouseLeave(e)
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        If e.Button = Windows.Forms.MouseButtons.Left Then SetState(MouseState.Down)

        If Not (_IsParentForm AndAlso ParentForm.WindowState = FormWindowState.Maximized OrElse _ControlMode) Then
            If _Movable AndAlso Frame.Contains(e.Location) Then
                If Not New Rectangle(Width - 22, 5, 15, 15).Contains(e.Location) Then
                    Capture = False
                End If
                WM_LMBUTTONDOWN = True
                DefWndProc(Messages(0))
            ElseIf _Sizable AndAlso Not Previous = 0 Then
                Capture = False
                WM_LMBUTTONDOWN = True
                DefWndProc(Messages(Previous))
            End If
        End If

        MyBase.OnMouseDown(e)
    End Sub

    Private WM_LMBUTTONDOWN As Boolean
    Protected Overrides Sub WndProc(ByRef m As Message)
        MyBase.WndProc(m)

        If WM_LMBUTTONDOWN AndAlso m.Msg = 513 Then
            WM_LMBUTTONDOWN = False

            SetState(MouseState.Over)
            If Not _SmartBounds Then Return

            If IsParentMdi Then
                CorrectBounds(New Rectangle(Point.Empty, Parent.Parent.Size))
            Else
                CorrectBounds(Screen.FromControl(Parent).WorkingArea)
            End If
        End If
    End Sub

    Private GetIndexPoint As Point
    Private B1, B2, B3, B4 As Boolean
    Private Function GetIndex() As Integer
        GetIndexPoint = PointToClient(MousePosition)
        B1 = GetIndexPoint.X < 7
        B2 = GetIndexPoint.X > Width - 7
        B3 = GetIndexPoint.Y < 7
        B4 = GetIndexPoint.Y > Height - 7

        If B1 AndAlso B3 Then Return 4
        If B1 AndAlso B4 Then Return 7
        If B2 AndAlso B3 Then Return 5
        If B2 AndAlso B4 Then Return 8
        If B1 Then Return 1
        If B2 Then Return 2
        If B3 Then Return 3
        If B4 Then Return 6
        Return 0
    End Function

    Private Current, Previous As Integer
    Private Sub InvalidateMouse()
        Current = GetIndex()
        If Current = Previous Then Return

        Previous = Current
        Select Case Previous
            Case 0
                Cursor = Cursors.Default
            Case 1, 2
                Cursor = Cursors.SizeWE
            Case 3, 6
                Cursor = Cursors.SizeNS
            Case 4, 8
                Cursor = Cursors.SizeNWSE
            Case 5, 7
                Cursor = Cursors.SizeNESW
        End Select
    End Sub

    Private Messages(8) As Message
    Private Sub InitializeMessages()
        Messages(0) = Message.Create(Parent.Handle, 161, New IntPtr(2), IntPtr.Zero)
        For I As Integer = 1 To 8
            Messages(I) = Message.Create(Parent.Handle, 161, New IntPtr(I + 9), IntPtr.Zero)
        Next
    End Sub

    Private Sub CorrectBounds(ByVal bounds As Rectangle)
        If Parent.Width > bounds.Width Then Parent.Width = bounds.Width
        If Parent.Height > bounds.Height Then Parent.Height = bounds.Height

        Dim X As Integer = Parent.Location.X
        Dim Y As Integer = Parent.Location.Y

        If X < bounds.X Then X = bounds.X
        If Y < bounds.Y Then Y = bounds.Y

        Dim Width As Integer = bounds.X + bounds.Width
        Dim Height As Integer = bounds.Y + bounds.Height

        If X + Parent.Width > Width Then X = Width - Parent.Width
        If Y + Parent.Height > Height Then Y = Height - Parent.Height

        Parent.Location = New Point(X, Y)
    End Sub

#End Region


#Region " Base Properties "

    Overrides Property Dock As DockStyle
        Get
            Return MyBase.Dock
        End Get
        Set(ByVal value As DockStyle)
            If Not _ControlMode Then Return
            MyBase.Dock = value
        End Set
    End Property

    Private _BackColor As Boolean
    <Category("Misc")> _
    Overrides Property BackColor() As Color
        Get
            Return MyBase.BackColor
        End Get
        Set(ByVal value As Color)
            If value = MyBase.BackColor Then Return

            If Not IsHandleCreated AndAlso _ControlMode AndAlso value = Color.Transparent Then
                _BackColor = True
                Return
            End If

            MyBase.BackColor = value
            If Parent IsNot Nothing Then
                If Not _ControlMode Then Parent.BackColor = value
                ColorHook()
            End If
        End Set
    End Property

    Overrides Property MinimumSize As Size
        Get
            Return MyBase.MinimumSize
        End Get
        Set(ByVal value As Size)
            MyBase.MinimumSize = value
            If Parent IsNot Nothing Then Parent.MinimumSize = value
        End Set
    End Property

    Overrides Property MaximumSize As Size
        Get
            Return MyBase.MaximumSize
        End Get
        Set(ByVal value As Size)
            MyBase.MaximumSize = value
            If Parent IsNot Nothing Then Parent.MaximumSize = value
        End Set
    End Property

    Overrides Property Text() As String
        Get
            Return MyBase.Text
        End Get
        Set(ByVal value As String)
            MyBase.Text = value
            Invalidate()
        End Set
    End Property

    Overrides Property Font() As Font
        Get
            Return MyBase.Font
        End Get
        Set(ByVal value As Font)
            MyBase.Font = value
            Invalidate()
        End Set
    End Property

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Overrides Property ForeColor() As Color
        Get
            Return Color.Empty
        End Get
        Set(ByVal value As Color)
        End Set
    End Property
    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Overrides Property BackgroundImage() As Image
        Get
            Return Nothing
        End Get
        Set(ByVal value As Image)
        End Set
    End Property
    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Overrides Property BackgroundImageLayout() As ImageLayout
        Get
            Return ImageLayout.None
        End Get
        Set(ByVal value As ImageLayout)
        End Set
    End Property

#End Region

#Region " Public Properties "

    Private _SmartBounds As Boolean = True
    Property SmartBounds() As Boolean
        Get
            Return _SmartBounds
        End Get
        Set(ByVal value As Boolean)
            _SmartBounds = value
        End Set
    End Property

    Private _Movable As Boolean = True
    Property Movable() As Boolean
        Get
            Return _Movable
        End Get
        Set(ByVal value As Boolean)
            _Movable = value
        End Set
    End Property

    Private _Sizable As Boolean = True
    Property Sizable() As Boolean
        Get
            Return _Sizable
        End Get
        Set(ByVal value As Boolean)
            _Sizable = value
        End Set
    End Property

    Private _TransparencyKey As Color
    Property TransparencyKey() As Color
        Get
            If _IsParentForm AndAlso Not _ControlMode Then Return ParentForm.TransparencyKey Else Return _TransparencyKey
        End Get
        Set(ByVal value As Color)
            If value = _TransparencyKey Then Return
            _TransparencyKey = value

            If _IsParentForm AndAlso Not _ControlMode Then
                ParentForm.TransparencyKey = value
                ColorHook()
            End If
        End Set
    End Property

    Private _BorderStyle As FormBorderStyle
    Property BorderStyle() As FormBorderStyle
        Get
            If _IsParentForm AndAlso Not _ControlMode Then Return ParentForm.FormBorderStyle Else Return _BorderStyle
        End Get
        Set(ByVal value As FormBorderStyle)
            _BorderStyle = value

            If _IsParentForm AndAlso Not _ControlMode Then
                ParentForm.FormBorderStyle = value

                If Not value = FormBorderStyle.None Then
                    Movable = False
                    Sizable = False
                End If
            End If
        End Set
    End Property

    Private _StartPosition As FormStartPosition
    Property StartPosition As FormStartPosition
        Get
            If _IsParentForm AndAlso Not _ControlMode Then Return ParentForm.StartPosition Else Return _StartPosition
        End Get
        Set(ByVal value As FormStartPosition)
            _StartPosition = value

            If _IsParentForm AndAlso Not _ControlMode Then
                ParentForm.StartPosition = value
            End If
        End Set
    End Property

    Private _NoRounding As Boolean
    Property NoRounding() As Boolean
        Get
            Return _NoRounding
        End Get
        Set(ByVal v As Boolean)
            _NoRounding = v
            Invalidate()
        End Set
    End Property

    Private _Image As Image
    Property Image() As Image
        Get
            Return _Image
        End Get
        Set(ByVal value As Image)
            If value Is Nothing Then _ImageSize = Size.Empty Else _ImageSize = value.Size

            _Image = value
            Invalidate()
        End Set
    End Property

    Private Items As New Dictionary(Of String, Color)
    Property Colors() As Bloom()
        Get
            Dim T As New List(Of Bloom)
            Dim E As Dictionary(Of String, Color).Enumerator = Items.GetEnumerator

            While E.MoveNext
                T.Add(New Bloom(E.Current.Key, E.Current.Value))
            End While

            Return T.ToArray
        End Get
        Set(ByVal value As Bloom())
            For Each B As Bloom In value
                If Items.ContainsKey(B.Name) Then Items(B.Name) = B.Value
            Next

            InvalidateCustimization()
            ColorHook()
            Invalidate()
        End Set
    End Property

    Private _Customization As String
    Property Customization() As String
        Get
            Return _Customization
        End Get
        Set(ByVal value As String)
            If value = _Customization Then Return

            Dim Data As Byte()
            Dim Items As Bloom() = Colors

            Try
                Data = Convert.FromBase64String(value)
                For I As Integer = 0 To Items.Length - 1
                    Items(I).Value = Color.FromArgb(BitConverter.ToInt32(Data, I * 4))
                Next
            Catch
                Return
            End Try

            _Customization = value

            Colors = Items
            ColorHook()
            Invalidate()
        End Set
    End Property

    Private _Transparent As Boolean
    Property Transparent() As Boolean
        Get
            Return _Transparent
        End Get
        Set(ByVal value As Boolean)
            _Transparent = value
            If Not (IsHandleCreated OrElse _ControlMode) Then Return

            If Not value AndAlso Not BackColor.A = 255 Then
                Throw New Exception("Unable to change value to false while a transparent BackColor is in use.")
            End If

            SetStyle(ControlStyles.Opaque, Not value)
            SetStyle(ControlStyles.SupportsTransparentBackColor, value)

            InvalidateBitmap()
            Invalidate()
        End Set
    End Property

#End Region

#Region " Private Properties "

    Private _ImageSize As Size
    Protected ReadOnly Property ImageSize() As Size
        Get
            Return _ImageSize
        End Get
    End Property

    Private _IsParentForm As Boolean
    Protected ReadOnly Property IsParentForm As Boolean
        Get
            Return _IsParentForm
        End Get
    End Property

    Protected ReadOnly Property IsParentMdi As Boolean
        Get
            If Parent Is Nothing Then Return False
            Return Parent.Parent IsNot Nothing
        End Get
    End Property

    Private _LockWidth As Integer
    Protected Property LockWidth() As Integer
        Get
            Return _LockWidth
        End Get
        Set(ByVal value As Integer)
            _LockWidth = value
            If Not LockWidth = 0 AndAlso IsHandleCreated Then Width = LockWidth
        End Set
    End Property

    Private _LockHeight As Integer
    Protected Property LockHeight() As Integer
        Get
            Return _LockHeight
        End Get
        Set(ByVal value As Integer)
            _LockHeight = value
            If Not LockHeight = 0 AndAlso IsHandleCreated Then Height = LockHeight
        End Set
    End Property

    Private _Header As Integer = 24
    Protected Property Header() As Integer
        Get
            Return _Header
        End Get
        Set(ByVal v As Integer)
            _Header = v

            If Not _ControlMode Then
                Frame = New Rectangle(7, 7, Width - 14, v - 7)
                Invalidate()
            End If
        End Set
    End Property

    Private _ControlMode As Boolean
    Protected Property ControlMode() As Boolean
        Get
            Return _ControlMode
        End Get
        Set(ByVal v As Boolean)
            _ControlMode = v

            Transparent = _Transparent
            If _Transparent AndAlso _BackColor Then BackColor = Color.Transparent

            InvalidateBitmap()
            Invalidate()
        End Set
    End Property

    Private _IsAnimated As Boolean
    Protected Property IsAnimated() As Boolean
        Get
            Return _IsAnimated
        End Get
        Set(ByVal value As Boolean)
            _IsAnimated = value
            InvalidateTimer()
        End Set
    End Property

#End Region


#Region " Property Helpers "

    Protected Function GetPen(ByVal name As String) As Pen
        Return New Pen(Items(name))
    End Function
    Protected Function GetPen(ByVal name As String, ByVal width As Single) As Pen
        Return New Pen(Items(name), width)
    End Function

    Protected Function GetBrush(ByVal name As String) As SolidBrush
        Return New SolidBrush(Items(name))
    End Function

    Protected Function GetColor(ByVal name As String) As Color
        Return Items(name)
    End Function

    Protected Sub SetColor(ByVal name As String, ByVal value As Color)
        If Items.ContainsKey(name) Then Items(name) = value Else Items.Add(name, value)
    End Sub
    Protected Sub SetColor(ByVal name As String, ByVal r As Byte, ByVal g As Byte, ByVal b As Byte)
        SetColor(name, Color.FromArgb(r, g, b))
    End Sub
    Protected Sub SetColor(ByVal name As String, ByVal a As Byte, ByVal r As Byte, ByVal g As Byte, ByVal b As Byte)
        SetColor(name, Color.FromArgb(a, r, g, b))
    End Sub
    Protected Sub SetColor(ByVal name As String, ByVal a As Byte, ByVal value As Color)
        SetColor(name, Color.FromArgb(a, value))
    End Sub

    Private Sub InvalidateBitmap()
        If _Transparent AndAlso _ControlMode Then
            If Width = 0 OrElse Height = 0 Then Return
            B = New Bitmap(Width, Height, PixelFormat.Format32bppPArgb)
            G = Graphics.FromImage(B)
        Else
            G = Nothing
            B = Nothing
        End If
    End Sub

    Private Sub InvalidateCustimization()
        Dim M As New MemoryStream(Items.Count * 4)

        For Each B As Bloom In Colors
            M.Write(BitConverter.GetBytes(B.Value.ToArgb), 0, 4)
        Next

        M.Close()
        _Customization = Convert.ToBase64String(M.ToArray)
    End Sub

    Private Sub InvalidateTimer()
        If DesignMode OrElse Not DoneCreation Then Return

        If _IsAnimated Then
            AddAnimationCallback(AddressOf DoAnimation)
        Else
            RemoveAnimationCallback(AddressOf DoAnimation)
        End If
    End Sub

#End Region


#Region " User Hooks "

    Protected MustOverride Sub ColorHook()
    Protected MustOverride Sub PaintHook()

    Protected Overridable Sub OnCreation()
    End Sub

    Protected Overridable Sub OnAnimation()
    End Sub

#End Region


#Region " Offset "

    Private OffsetReturnRectangle As Rectangle
    Protected Function Offset(ByVal r As Rectangle, ByVal amount As Integer) As Rectangle
        OffsetReturnRectangle = New Rectangle(r.X + amount, r.Y + amount, r.Width - (amount * 2), r.Height - (amount * 2))
        Return OffsetReturnRectangle
    End Function

    Private OffsetReturnSize As Size
    Protected Function Offset(ByVal s As Size, ByVal amount As Integer) As Size
        OffsetReturnSize = New Size(s.Width + amount, s.Height + amount)
        Return OffsetReturnSize
    End Function

    Private OffsetReturnPoint As Point
    Protected Function Offset(ByVal p As Point, ByVal amount As Integer) As Point
        OffsetReturnPoint = New Point(p.X + amount, p.Y + amount)
        Return OffsetReturnPoint
    End Function

#End Region

#Region " Center "

    Private CenterReturn As Point

    Protected Function Center(ByVal p As Rectangle, ByVal c As Rectangle) As Point
        CenterReturn = New Point((p.Width \ 2 - c.Width \ 2) + p.X + c.X, (p.Height \ 2 - c.Height \ 2) + p.Y + c.Y)
        Return CenterReturn
    End Function
    Protected Function Center(ByVal p As Rectangle, ByVal c As Size) As Point
        CenterReturn = New Point((p.Width \ 2 - c.Width \ 2) + p.X, (p.Height \ 2 - c.Height \ 2) + p.Y)
        Return CenterReturn
    End Function

    Protected Function Center(ByVal child As Rectangle) As Point
        Return Center(Width, Height, child.Width, child.Height)
    End Function
    Protected Function Center(ByVal child As Size) As Point
        Return Center(Width, Height, child.Width, child.Height)
    End Function
    Protected Function Center(ByVal childWidth As Integer, ByVal childHeight As Integer) As Point
        Return Center(Width, Height, childWidth, childHeight)
    End Function

    Protected Function Center(ByVal p As Size, ByVal c As Size) As Point
        Return Center(p.Width, p.Height, c.Width, c.Height)
    End Function

    Protected Function Center(ByVal pWidth As Integer, ByVal pHeight As Integer, ByVal cWidth As Integer, ByVal cHeight As Integer) As Point
        CenterReturn = New Point(pWidth \ 2 - cWidth \ 2, pHeight \ 2 - cHeight \ 2)
        Return CenterReturn
    End Function

#End Region

#Region " Measure "

    Private MeasureBitmap As Bitmap
    Private MeasureGraphics As Graphics

    Protected Function Measure() As Size
        SyncLock MeasureGraphics
            Return MeasureGraphics.MeasureString(Text, Font, Width).ToSize
        End SyncLock
    End Function
    Protected Function Measure(ByVal text As String) As Size
        SyncLock MeasureGraphics
            Return MeasureGraphics.MeasureString(text, Font, Width).ToSize
        End SyncLock
    End Function

#End Region


#Region " DrawPixel "

    Private DrawPixelBrush As SolidBrush

    Protected Sub DrawPixel(ByVal c1 As Color, ByVal x As Integer, ByVal y As Integer)
        If _Transparent Then
            B.SetPixel(x, y, c1)
        Else
            DrawPixelBrush = New SolidBrush(c1)
            G.FillRectangle(DrawPixelBrush, x, y, 1, 1)
        End If
    End Sub

#End Region

#Region " DrawCorners "

    Private DrawCornersBrush As SolidBrush

    Protected Sub DrawCorners(ByVal c1 As Color, ByVal offset As Integer)
        DrawCorners(c1, 0, 0, Width, Height, offset)
    End Sub
    Protected Sub DrawCorners(ByVal c1 As Color, ByVal r1 As Rectangle, ByVal offset As Integer)
        DrawCorners(c1, r1.X, r1.Y, r1.Width, r1.Height, offset)
    End Sub
    Protected Sub DrawCorners(ByVal c1 As Color, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal offset As Integer)
        DrawCorners(c1, x + offset, y + offset, width - (offset * 2), height - (offset * 2))
    End Sub

    Protected Sub DrawCorners(ByVal c1 As Color)
        DrawCorners(c1, 0, 0, Width, Height)
    End Sub
    Protected Sub DrawCorners(ByVal c1 As Color, ByVal r1 As Rectangle)
        DrawCorners(c1, r1.X, r1.Y, r1.Width, r1.Height)
    End Sub
    Protected Sub DrawCorners(ByVal c1 As Color, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
        If _NoRounding Then Return

        If _Transparent Then
            B.SetPixel(x, y, c1)
            B.SetPixel(x + (width - 1), y, c1)
            B.SetPixel(x, y + (height - 1), c1)
            B.SetPixel(x + (width - 1), y + (height - 1), c1)
        Else
            DrawCornersBrush = New SolidBrush(c1)
            G.FillRectangle(DrawCornersBrush, x, y, 1, 1)
            G.FillRectangle(DrawCornersBrush, x + (width - 1), y, 1, 1)
            G.FillRectangle(DrawCornersBrush, x, y + (height - 1), 1, 1)
            G.FillRectangle(DrawCornersBrush, x + (width - 1), y + (height - 1), 1, 1)
        End If
    End Sub

#End Region

#Region " DrawBorders "

    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal offset As Integer)
        DrawBorders(p1, 0, 0, Width, Height, offset)
    End Sub
    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal r As Rectangle, ByVal offset As Integer)
        DrawBorders(p1, r.X, r.Y, r.Width, r.Height, offset)
    End Sub
    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal offset As Integer)
        DrawBorders(p1, x + offset, y + offset, width - (offset * 2), height - (offset * 2))
    End Sub

    Protected Sub DrawBorders(ByVal p1 As Pen)
        DrawBorders(p1, 0, 0, Width, Height)
    End Sub
    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal r As Rectangle)
        DrawBorders(p1, r.X, r.Y, r.Width, r.Height)
    End Sub
    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
        G.DrawRectangle(p1, x, y, width - 1, height - 1)
    End Sub

#End Region

#Region " DrawText "

    Private DrawTextPoint As Point
    Private DrawTextSize As Size

    Protected Sub DrawText(ByVal b1 As Brush, ByVal a As HorizontalAlignment, ByVal x As Integer, ByVal y As Integer)
        DrawText(b1, Text, a, x, y)
    End Sub
    Protected Sub DrawText(ByVal b1 As Brush, ByVal text As String, ByVal a As HorizontalAlignment, ByVal x As Integer, ByVal y As Integer)
        If text.Length = 0 Then Return

        DrawTextSize = Measure(text)
        DrawTextPoint = New Point(Width \ 2 - DrawTextSize.Width \ 2, Header \ 2 - DrawTextSize.Height \ 2)

        Select Case a
            Case HorizontalAlignment.Left
                G.DrawString(text, Font, b1, x, DrawTextPoint.Y + y)
            Case HorizontalAlignment.Center
                G.DrawString(text, Font, b1, DrawTextPoint.X + x, DrawTextPoint.Y + y)
            Case HorizontalAlignment.Right
                G.DrawString(text, Font, b1, Width - DrawTextSize.Width - x, DrawTextPoint.Y + y)
        End Select
    End Sub

    Protected Sub DrawText(ByVal b1 As Brush, ByVal p1 As Point)
        If Text.Length = 0 Then Return
        G.DrawString(Text, Font, b1, p1)
    End Sub
    Protected Sub DrawText(ByVal b1 As Brush, ByVal x As Integer, ByVal y As Integer)
        If Text.Length = 0 Then Return
        G.DrawString(Text, Font, b1, x, y)
    End Sub

#End Region

#Region " DrawImage "

    Private DrawImagePoint As Point

    Protected Sub DrawImage(ByVal a As HorizontalAlignment, ByVal x As Integer, ByVal y As Integer)
        DrawImage(_Image, a, x, y)
    End Sub
    Protected Sub DrawImage(ByVal image As Image, ByVal a As HorizontalAlignment, ByVal x As Integer, ByVal y As Integer)
        If image Is Nothing Then Return
        DrawImagePoint = New Point(Width \ 2 - image.Width \ 2, Header \ 2 - image.Height \ 2)

        Select Case a
            Case HorizontalAlignment.Left
                G.DrawImage(image, x, DrawImagePoint.Y + y, image.Width, image.Height)
            Case HorizontalAlignment.Center
                G.DrawImage(image, DrawImagePoint.X + x, DrawImagePoint.Y + y, image.Width, image.Height)
            Case HorizontalAlignment.Right
                G.DrawImage(image, Width - image.Width - x, DrawImagePoint.Y + y, image.Width, image.Height)
        End Select
    End Sub

    Protected Sub DrawImage(ByVal p1 As Point)
        DrawImage(_Image, p1.X, p1.Y)
    End Sub
    Protected Sub DrawImage(ByVal x As Integer, ByVal y As Integer)
        DrawImage(_Image, x, y)
    End Sub

    Protected Sub DrawImage(ByVal image As Image, ByVal p1 As Point)
        DrawImage(image, p1.X, p1.Y)
    End Sub
    Protected Sub DrawImage(ByVal image As Image, ByVal x As Integer, ByVal y As Integer)
        If image Is Nothing Then Return
        G.DrawImage(image, x, y, image.Width, image.Height)
    End Sub

#End Region

#Region " DrawGradient "

    Private DrawGradientBrush As LinearGradientBrush
    Private DrawGradientRectangle As Rectangle

    Protected Sub DrawGradient(ByVal blend As ColorBlend, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
        DrawGradientRectangle = New Rectangle(x, y, width, height)
        DrawGradient(blend, DrawGradientRectangle)
    End Sub
    Protected Sub DrawGradient(ByVal blend As ColorBlend, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal angle As Single)
        DrawGradientRectangle = New Rectangle(x, y, width, height)
        DrawGradient(blend, DrawGradientRectangle, angle)
    End Sub

    Protected Sub DrawGradient(ByVal blend As ColorBlend, ByVal r As Rectangle)
        DrawGradientBrush = New LinearGradientBrush(r, Color.Empty, Color.Empty, 90.0F)
        DrawGradientBrush.InterpolationColors = blend
        G.FillRectangle(DrawGradientBrush, r)
    End Sub
    Protected Sub DrawGradient(ByVal blend As ColorBlend, ByVal r As Rectangle, ByVal angle As Single)
        DrawGradientBrush = New LinearGradientBrush(r, Color.Empty, Color.Empty, angle)
        DrawGradientBrush.InterpolationColors = blend
        G.FillRectangle(DrawGradientBrush, r)
    End Sub


    Protected Sub DrawGradient(ByVal c1 As Color, ByVal c2 As Color, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
        DrawGradientRectangle = New Rectangle(x, y, width, height)
        DrawGradient(c1, c2, DrawGradientRectangle)
    End Sub
    Protected Sub DrawGradient(ByVal c1 As Color, ByVal c2 As Color, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal angle As Single)
        DrawGradientRectangle = New Rectangle(x, y, width, height)
        DrawGradient(c1, c2, DrawGradientRectangle, angle)
    End Sub

    Protected Sub DrawGradient(ByVal c1 As Color, ByVal c2 As Color, ByVal r As Rectangle)
        DrawGradientBrush = New LinearGradientBrush(r, c1, c2, 90.0F)
        G.FillRectangle(DrawGradientBrush, r)
    End Sub
    Protected Sub DrawGradient(ByVal c1 As Color, ByVal c2 As Color, ByVal r As Rectangle, ByVal angle As Single)
        DrawGradientBrush = New LinearGradientBrush(r, c1, c2, angle)
        G.FillRectangle(DrawGradientBrush, r)
    End Sub

#End Region

#Region " DrawRadial "

    Private DrawRadialPath As GraphicsPath
    Private DrawRadialBrush1 As PathGradientBrush
    Private DrawRadialBrush2 As LinearGradientBrush
    Private DrawRadialRectangle As Rectangle

    Sub DrawRadial(ByVal blend As ColorBlend, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
        DrawRadialRectangle = New Rectangle(x, y, width, height)
        DrawRadial(blend, DrawRadialRectangle, width \ 2, height \ 2)
    End Sub
    Sub DrawRadial(ByVal blend As ColorBlend, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal center As Point)
        DrawRadialRectangle = New Rectangle(x, y, width, height)
        DrawRadial(blend, DrawRadialRectangle, center.X, center.Y)
    End Sub
    Sub DrawRadial(ByVal blend As ColorBlend, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal cx As Integer, ByVal cy As Integer)
        DrawRadialRectangle = New Rectangle(x, y, width, height)
        DrawRadial(blend, DrawRadialRectangle, cx, cy)
    End Sub

    Sub DrawRadial(ByVal blend As ColorBlend, ByVal r As Rectangle)
        DrawRadial(blend, r, r.Width \ 2, r.Height \ 2)
    End Sub
    Sub DrawRadial(ByVal blend As ColorBlend, ByVal r As Rectangle, ByVal center As Point)
        DrawRadial(blend, r, center.X, center.Y)
    End Sub
    Sub DrawRadial(ByVal blend As ColorBlend, ByVal r As Rectangle, ByVal cx As Integer, ByVal cy As Integer)
        DrawRadialPath.Reset()
        DrawRadialPath.AddEllipse(r.X, r.Y, r.Width - 1, r.Height - 1)

        DrawRadialBrush1 = New PathGradientBrush(DrawRadialPath)
        DrawRadialBrush1.CenterPoint = New Point(r.X + cx, r.Y + cy)
        DrawRadialBrush1.InterpolationColors = blend

        If G.SmoothingMode = SmoothingMode.AntiAlias Then
            G.FillEllipse(DrawRadialBrush1, r.X + 1, r.Y + 1, r.Width - 3, r.Height - 3)
        Else
            G.FillEllipse(DrawRadialBrush1, r)
        End If
    End Sub


    Protected Sub DrawRadial(ByVal c1 As Color, ByVal c2 As Color, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
        DrawRadialRectangle = New Rectangle(x, y, width, height)
        DrawRadial(c1, c2, DrawGradientRectangle)
    End Sub
    Protected Sub DrawRadial(ByVal c1 As Color, ByVal c2 As Color, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal angle As Single)
        DrawRadialRectangle = New Rectangle(x, y, width, height)
        DrawRadial(c1, c2, DrawGradientRectangle, angle)
    End Sub

    Protected Sub DrawRadial(ByVal c1 As Color, ByVal c2 As Color, ByVal r As Rectangle)
        DrawRadialBrush2 = New LinearGradientBrush(r, c1, c2, 90.0F)
        G.FillRectangle(DrawGradientBrush, r)
    End Sub
    Protected Sub DrawRadial(ByVal c1 As Color, ByVal c2 As Color, ByVal r As Rectangle, ByVal angle As Single)
        DrawRadialBrush2 = New LinearGradientBrush(r, c1, c2, angle)
        G.FillEllipse(DrawGradientBrush, r)
    End Sub

#End Region

#Region " CreateRound "

    Private CreateRoundPath As GraphicsPath
    Private CreateRoundRectangle As Rectangle

    Function CreateRound(ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal slope As Integer) As GraphicsPath
        CreateRoundRectangle = New Rectangle(x, y, width, height)
        Return CreateRound(CreateRoundRectangle, slope)
    End Function

    Function CreateRound(ByVal r As Rectangle, ByVal slope As Integer) As GraphicsPath
        CreateRoundPath = New GraphicsPath(FillMode.Winding)
        CreateRoundPath.AddArc(r.X, r.Y, slope, slope, 180.0F, 90.0F)
        CreateRoundPath.AddArc(r.Right - slope, r.Y, slope, slope, 270.0F, 90.0F)
        CreateRoundPath.AddArc(r.Right - slope, r.Bottom - slope, slope, slope, 0.0F, 90.0F)
        CreateRoundPath.AddArc(r.X, r.Bottom - slope, slope, slope, 90.0F, 90.0F)
        CreateRoundPath.CloseFigure()
        Return CreateRoundPath
    End Function

#End Region

End Class

MustInherit Class ThemeControl154
    Inherits Control


#Region " Initialization "

    Protected G As Graphics, B As Bitmap

    Sub New()
        SetStyle(DirectCast(139270, ControlStyles), True)

        _ImageSize = Size.Empty
        Font = New Font("Verdana", 8S)

        MeasureBitmap = New Bitmap(1, 1)
        MeasureGraphics = Graphics.FromImage(MeasureBitmap)

        DrawRadialPath = New GraphicsPath

        InvalidateCustimization() 'Remove?
    End Sub

    Protected NotOverridable Overrides Sub OnHandleCreated(ByVal e As EventArgs)
        InvalidateCustimization()
        ColorHook()

        If Not _LockWidth = 0 Then Width = _LockWidth
        If Not _LockHeight = 0 Then Height = _LockHeight

        Transparent = _Transparent
        If _Transparent AndAlso _BackColor Then BackColor = Color.Transparent

        MyBase.OnHandleCreated(e)
    End Sub

    Private DoneCreation As Boolean
    Protected NotOverridable Overrides Sub OnParentChanged(ByVal e As EventArgs)
        If Parent IsNot Nothing Then
            OnCreation()
            DoneCreation = True
            InvalidateTimer()
        End If

        MyBase.OnParentChanged(e)
    End Sub

#End Region

    Private Sub DoAnimation(ByVal i As Boolean)
        OnAnimation()
        If i Then Invalidate()
    End Sub

    Protected NotOverridable Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        If Width = 0 OrElse Height = 0 Then Return

        If _Transparent Then
            PaintHook()
            e.Graphics.DrawImage(B, 0, 0)
        Else
            G = e.Graphics
            PaintHook()
        End If
    End Sub

    Protected Overrides Sub OnHandleDestroyed(ByVal e As EventArgs)
        RemoveAnimationCallback(AddressOf DoAnimation)
        MyBase.OnHandleDestroyed(e)
    End Sub

#Region " Size Handling "

    Protected NotOverridable Overrides Sub OnSizeChanged(ByVal e As EventArgs)
        If _Transparent Then
            InvalidateBitmap()
        End If

        Invalidate()
        MyBase.OnSizeChanged(e)
    End Sub

    Protected Overrides Sub SetBoundsCore(ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal specified As BoundsSpecified)
        If Not _LockWidth = 0 Then width = _LockWidth
        If Not _LockHeight = 0 Then height = _LockHeight
        MyBase.SetBoundsCore(x, y, width, height, specified)
    End Sub

#End Region

#Region " State Handling "

    Private InPosition As Boolean
    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        InPosition = True
        SetState(MouseState.Over)
        MyBase.OnMouseEnter(e)
    End Sub

    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        If InPosition Then SetState(MouseState.Over)
        MyBase.OnMouseUp(e)
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        If e.Button = Windows.Forms.MouseButtons.Left Then SetState(MouseState.Down)
        MyBase.OnMouseDown(e)
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        InPosition = False
        SetState(MouseState.None)
        MyBase.OnMouseLeave(e)
    End Sub

    Protected Overrides Sub OnEnabledChanged(ByVal e As EventArgs)
        If Enabled Then SetState(MouseState.None) Else SetState(MouseState.Block)
        MyBase.OnEnabledChanged(e)
    End Sub

    Protected State As MouseState
    Private Sub SetState(ByVal current As MouseState)
        State = current
        Invalidate()
    End Sub

#End Region


#Region " Base Properties "

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Overrides Property ForeColor() As Color
        Get
            Return Color.Empty
        End Get
        Set(ByVal value As Color)
        End Set
    End Property
    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Overrides Property BackgroundImage() As Image
        Get
            Return Nothing
        End Get
        Set(ByVal value As Image)
        End Set
    End Property
    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Overrides Property BackgroundImageLayout() As ImageLayout
        Get
            Return ImageLayout.None
        End Get
        Set(ByVal value As ImageLayout)
        End Set
    End Property

    Overrides Property Text() As String
        Get
            Return MyBase.Text
        End Get
        Set(ByVal value As String)
            MyBase.Text = value
            Invalidate()
        End Set
    End Property
    Overrides Property Font() As Font
        Get
            Return MyBase.Font
        End Get
        Set(ByVal value As Font)
            MyBase.Font = value
            Invalidate()
        End Set
    End Property

    Private _BackColor As Boolean
    <Category("Misc")> _
    Overrides Property BackColor() As Color
        Get
            Return MyBase.BackColor
        End Get
        Set(ByVal value As Color)
            If Not IsHandleCreated AndAlso value = Color.Transparent Then
                _BackColor = True
                Return
            End If

            MyBase.BackColor = value
            If Parent IsNot Nothing Then ColorHook()
        End Set
    End Property

#End Region

#Region " Public Properties "

    Private _NoRounding As Boolean
    Property NoRounding() As Boolean
        Get
            Return _NoRounding
        End Get
        Set(ByVal v As Boolean)
            _NoRounding = v
            Invalidate()
        End Set
    End Property

    Private _Image As Image
    Property Image() As Image
        Get
            Return _Image
        End Get
        Set(ByVal value As Image)
            If value Is Nothing Then
                _ImageSize = Size.Empty
            Else
                _ImageSize = value.Size
            End If

            _Image = value
            Invalidate()
        End Set
    End Property

    Private _Transparent As Boolean
    Property Transparent() As Boolean
        Get
            Return _Transparent
        End Get
        Set(ByVal value As Boolean)
            _Transparent = value
            If Not IsHandleCreated Then Return

            If Not value AndAlso Not BackColor.A = 255 Then
                Throw New Exception("Unable to change value to false while a transparent BackColor is in use.")
            End If

            SetStyle(ControlStyles.Opaque, Not value)
            SetStyle(ControlStyles.SupportsTransparentBackColor, value)

            If value Then InvalidateBitmap() Else B = Nothing
            Invalidate()
        End Set
    End Property

    Private Items As New Dictionary(Of String, Color)
    Property Colors() As Bloom()
        Get
            Dim T As New List(Of Bloom)
            Dim E As Dictionary(Of String, Color).Enumerator = Items.GetEnumerator

            While E.MoveNext
                T.Add(New Bloom(E.Current.Key, E.Current.Value))
            End While

            Return T.ToArray
        End Get
        Set(ByVal value As Bloom())
            For Each B As Bloom In value
                If Items.ContainsKey(B.Name) Then Items(B.Name) = B.Value
            Next

            InvalidateCustimization()
            ColorHook()
            Invalidate()
        End Set
    End Property

    Private _Customization As String
    Property Customization() As String
        Get
            Return _Customization
        End Get
        Set(ByVal value As String)
            If value = _Customization Then Return

            Dim Data As Byte()
            Dim Items As Bloom() = Colors

            Try
                Data = Convert.FromBase64String(value)
                For I As Integer = 0 To Items.Length - 1
                    Items(I).Value = Color.FromArgb(BitConverter.ToInt32(Data, I * 4))
                Next
            Catch
                Return
            End Try

            _Customization = value

            Colors = Items
            ColorHook()
            Invalidate()
        End Set
    End Property

#End Region

#Region " Private Properties "

    Private _ImageSize As Size
    Protected ReadOnly Property ImageSize() As Size
        Get
            Return _ImageSize
        End Get
    End Property

    Private _LockWidth As Integer
    Protected Property LockWidth() As Integer
        Get
            Return _LockWidth
        End Get
        Set(ByVal value As Integer)
            _LockWidth = value
            If Not LockWidth = 0 AndAlso IsHandleCreated Then Width = LockWidth
        End Set
    End Property

    Private _LockHeight As Integer
    Protected Property LockHeight() As Integer
        Get
            Return _LockHeight
        End Get
        Set(ByVal value As Integer)
            _LockHeight = value
            If Not LockHeight = 0 AndAlso IsHandleCreated Then Height = LockHeight
        End Set
    End Property

    Private _IsAnimated As Boolean
    Protected Property IsAnimated() As Boolean
        Get
            Return _IsAnimated
        End Get
        Set(ByVal value As Boolean)
            _IsAnimated = value
            InvalidateTimer()
        End Set
    End Property

#End Region


#Region " Property Helpers "

    Protected Function GetPen(ByVal name As String) As Pen
        Return New Pen(Items(name))
    End Function
    Protected Function GetPen(ByVal name As String, ByVal width As Single) As Pen
        Return New Pen(Items(name), width)
    End Function

    Protected Function GetBrush(ByVal name As String) As SolidBrush
        Return New SolidBrush(Items(name))
    End Function

    Protected Function GetColor(ByVal name As String) As Color
        Return Items(name)
    End Function

    Protected Sub SetColor(ByVal name As String, ByVal value As Color)
        If Items.ContainsKey(name) Then Items(name) = value Else Items.Add(name, value)
    End Sub
    Protected Sub SetColor(ByVal name As String, ByVal r As Byte, ByVal g As Byte, ByVal b As Byte)
        SetColor(name, Color.FromArgb(r, g, b))
    End Sub
    Protected Sub SetColor(ByVal name As String, ByVal a As Byte, ByVal r As Byte, ByVal g As Byte, ByVal b As Byte)
        SetColor(name, Color.FromArgb(a, r, g, b))
    End Sub
    Protected Sub SetColor(ByVal name As String, ByVal a As Byte, ByVal value As Color)
        SetColor(name, Color.FromArgb(a, value))
    End Sub

    Private Sub InvalidateBitmap()
        If Width = 0 OrElse Height = 0 Then Return
        B = New Bitmap(Width, Height, PixelFormat.Format32bppPArgb)
        G = Graphics.FromImage(B)
    End Sub

    Private Sub InvalidateCustimization()
        Dim M As New MemoryStream(Items.Count * 4)

        For Each B As Bloom In Colors
            M.Write(BitConverter.GetBytes(B.Value.ToArgb), 0, 4)
        Next

        M.Close()
        _Customization = Convert.ToBase64String(M.ToArray)
    End Sub

    Private Sub InvalidateTimer()
        If DesignMode OrElse Not DoneCreation Then Return

        If _IsAnimated Then
            AddAnimationCallback(AddressOf DoAnimation)
        Else
            RemoveAnimationCallback(AddressOf DoAnimation)
        End If
    End Sub
#End Region


#Region " User Hooks "

    Protected MustOverride Sub ColorHook()
    Protected MustOverride Sub PaintHook()

    Protected Overridable Sub OnCreation()
    End Sub

    Protected Overridable Sub OnAnimation()
    End Sub

#End Region


#Region " Offset "

    Private OffsetReturnRectangle As Rectangle
    Protected Function Offset(ByVal r As Rectangle, ByVal amount As Integer) As Rectangle
        OffsetReturnRectangle = New Rectangle(r.X + amount, r.Y + amount, r.Width - (amount * 2), r.Height - (amount * 2))
        Return OffsetReturnRectangle
    End Function

    Private OffsetReturnSize As Size
    Protected Function Offset(ByVal s As Size, ByVal amount As Integer) As Size
        OffsetReturnSize = New Size(s.Width + amount, s.Height + amount)
        Return OffsetReturnSize
    End Function

    Private OffsetReturnPoint As Point
    Protected Function Offset(ByVal p As Point, ByVal amount As Integer) As Point
        OffsetReturnPoint = New Point(p.X + amount, p.Y + amount)
        Return OffsetReturnPoint
    End Function

#End Region

#Region " Center "

    Private CenterReturn As Point

    Protected Function Center(ByVal p As Rectangle, ByVal c As Rectangle) As Point
        CenterReturn = New Point((p.Width \ 2 - c.Width \ 2) + p.X + c.X, (p.Height \ 2 - c.Height \ 2) + p.Y + c.Y)
        Return CenterReturn
    End Function
    Protected Function Center(ByVal p As Rectangle, ByVal c As Size) As Point
        CenterReturn = New Point((p.Width \ 2 - c.Width \ 2) + p.X, (p.Height \ 2 - c.Height \ 2) + p.Y)
        Return CenterReturn
    End Function

    Protected Function Center(ByVal child As Rectangle) As Point
        Return Center(Width, Height, child.Width, child.Height)
    End Function
    Protected Function Center(ByVal child As Size) As Point
        Return Center(Width, Height, child.Width, child.Height)
    End Function
    Protected Function Center(ByVal childWidth As Integer, ByVal childHeight As Integer) As Point
        Return Center(Width, Height, childWidth, childHeight)
    End Function

    Protected Function Center(ByVal p As Size, ByVal c As Size) As Point
        Return Center(p.Width, p.Height, c.Width, c.Height)
    End Function

    Protected Function Center(ByVal pWidth As Integer, ByVal pHeight As Integer, ByVal cWidth As Integer, ByVal cHeight As Integer) As Point
        CenterReturn = New Point(pWidth \ 2 - cWidth \ 2, pHeight \ 2 - cHeight \ 2)
        Return CenterReturn
    End Function

#End Region

#Region " Measure "

    Private MeasureBitmap As Bitmap
    Private MeasureGraphics As Graphics 'TODO: Potential issues during multi-threading.

    Protected Function Measure() As Size
        Return MeasureGraphics.MeasureString(Text, Font, Width).ToSize
    End Function
    Protected Function Measure(ByVal text As String) As Size
        Return MeasureGraphics.MeasureString(text, Font, Width).ToSize
    End Function

#End Region


#Region " DrawPixel "

    Private DrawPixelBrush As SolidBrush

    Protected Sub DrawPixel(ByVal c1 As Color, ByVal x As Integer, ByVal y As Integer)
        If _Transparent Then
            B.SetPixel(x, y, c1)
        Else
            DrawPixelBrush = New SolidBrush(c1)
            G.FillRectangle(DrawPixelBrush, x, y, 1, 1)
        End If
    End Sub

#End Region

#Region " DrawCorners "

    Private DrawCornersBrush As SolidBrush

    Protected Sub DrawCorners(ByVal c1 As Color, ByVal offset As Integer)
        DrawCorners(c1, 0, 0, Width, Height, offset)
    End Sub
    Protected Sub DrawCorners(ByVal c1 As Color, ByVal r1 As Rectangle, ByVal offset As Integer)
        DrawCorners(c1, r1.X, r1.Y, r1.Width, r1.Height, offset)
    End Sub
    Protected Sub DrawCorners(ByVal c1 As Color, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal offset As Integer)
        DrawCorners(c1, x + offset, y + offset, width - (offset * 2), height - (offset * 2))
    End Sub

    Protected Sub DrawCorners(ByVal c1 As Color)
        DrawCorners(c1, 0, 0, Width, Height)
    End Sub
    Protected Sub DrawCorners(ByVal c1 As Color, ByVal r1 As Rectangle)
        DrawCorners(c1, r1.X, r1.Y, r1.Width, r1.Height)
    End Sub
    Protected Sub DrawCorners(ByVal c1 As Color, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
        If _NoRounding Then Return

        If _Transparent Then
            B.SetPixel(x, y, c1)
            B.SetPixel(x + (width - 1), y, c1)
            B.SetPixel(x, y + (height - 1), c1)
            B.SetPixel(x + (width - 1), y + (height - 1), c1)
        Else
            DrawCornersBrush = New SolidBrush(c1)
            G.FillRectangle(DrawCornersBrush, x, y, 1, 1)
            G.FillRectangle(DrawCornersBrush, x + (width - 1), y, 1, 1)
            G.FillRectangle(DrawCornersBrush, x, y + (height - 1), 1, 1)
            G.FillRectangle(DrawCornersBrush, x + (width - 1), y + (height - 1), 1, 1)
        End If
    End Sub

#End Region

#Region " DrawBorders "

    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal offset As Integer)
        DrawBorders(p1, 0, 0, Width, Height, offset)
    End Sub
    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal r As Rectangle, ByVal offset As Integer)
        DrawBorders(p1, r.X, r.Y, r.Width, r.Height, offset)
    End Sub
    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal offset As Integer)
        DrawBorders(p1, x + offset, y + offset, width - (offset * 2), height - (offset * 2))
    End Sub

    Protected Sub DrawBorders(ByVal p1 As Pen)
        DrawBorders(p1, 0, 0, Width, Height)
    End Sub
    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal r As Rectangle)
        DrawBorders(p1, r.X, r.Y, r.Width, r.Height)
    End Sub
    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
        G.DrawRectangle(p1, x, y, width - 1, height - 1)
    End Sub

#End Region

#Region " DrawText "

    Private DrawTextPoint As Point
    Private DrawTextSize As Size

    Protected Sub DrawText(ByVal b1 As Brush, ByVal a As HorizontalAlignment, ByVal x As Integer, ByVal y As Integer)
        DrawText(b1, Text, a, x, y)
    End Sub
    Protected Sub DrawText(ByVal b1 As Brush, ByVal text As String, ByVal a As HorizontalAlignment, ByVal x As Integer, ByVal y As Integer)
        If text.Length = 0 Then Return

        DrawTextSize = Measure(text)
        DrawTextPoint = Center(DrawTextSize)

        Select Case a
            Case HorizontalAlignment.Left
                G.DrawString(text, Font, b1, x, DrawTextPoint.Y + y)
            Case HorizontalAlignment.Center
                G.DrawString(text, Font, b1, DrawTextPoint.X + x, DrawTextPoint.Y + y)
            Case HorizontalAlignment.Right
                G.DrawString(text, Font, b1, Width - DrawTextSize.Width - x, DrawTextPoint.Y + y)
        End Select
    End Sub

    Protected Sub DrawText(ByVal b1 As Brush, ByVal p1 As Point)
        If Text.Length = 0 Then Return
        G.DrawString(Text, Font, b1, p1)
    End Sub
    Protected Sub DrawText(ByVal b1 As Brush, ByVal x As Integer, ByVal y As Integer)
        If Text.Length = 0 Then Return
        G.DrawString(Text, Font, b1, x, y)
    End Sub

#End Region

#Region " DrawImage "

    Private DrawImagePoint As Point

    Protected Sub DrawImage(ByVal a As HorizontalAlignment, ByVal x As Integer, ByVal y As Integer)
        DrawImage(_Image, a, x, y)
    End Sub
    Protected Sub DrawImage(ByVal image As Image, ByVal a As HorizontalAlignment, ByVal x As Integer, ByVal y As Integer)
        If image Is Nothing Then Return
        DrawImagePoint = Center(image.Size)

        Select Case a
            Case HorizontalAlignment.Left
                G.DrawImage(image, x, DrawImagePoint.Y + y, image.Width, image.Height)
            Case HorizontalAlignment.Center
                G.DrawImage(image, DrawImagePoint.X + x, DrawImagePoint.Y + y, image.Width, image.Height)
            Case HorizontalAlignment.Right
                G.DrawImage(image, Width - image.Width - x, DrawImagePoint.Y + y, image.Width, image.Height)
        End Select
    End Sub

    Protected Sub DrawImage(ByVal p1 As Point)
        DrawImage(_Image, p1.X, p1.Y)
    End Sub
    Protected Sub DrawImage(ByVal x As Integer, ByVal y As Integer)
        DrawImage(_Image, x, y)
    End Sub

    Protected Sub DrawImage(ByVal image As Image, ByVal p1 As Point)
        DrawImage(image, p1.X, p1.Y)
    End Sub
    Protected Sub DrawImage(ByVal image As Image, ByVal x As Integer, ByVal y As Integer)
        If image Is Nothing Then Return
        G.DrawImage(image, x, y, image.Width, image.Height)
    End Sub

#End Region

#Region " DrawGradient "

    Private DrawGradientBrush As LinearGradientBrush
    Private DrawGradientRectangle As Rectangle

    Protected Sub DrawGradient(ByVal blend As ColorBlend, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
        DrawGradientRectangle = New Rectangle(x, y, width, height)
        DrawGradient(blend, DrawGradientRectangle)
    End Sub
    Protected Sub DrawGradient(ByVal blend As ColorBlend, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal angle As Single)
        DrawGradientRectangle = New Rectangle(x, y, width, height)
        DrawGradient(blend, DrawGradientRectangle, angle)
    End Sub

    Protected Sub DrawGradient(ByVal blend As ColorBlend, ByVal r As Rectangle)
        DrawGradientBrush = New LinearGradientBrush(r, Color.Empty, Color.Empty, 90.0F)
        DrawGradientBrush.InterpolationColors = blend
        G.FillRectangle(DrawGradientBrush, r)
    End Sub
    Protected Sub DrawGradient(ByVal blend As ColorBlend, ByVal r As Rectangle, ByVal angle As Single)
        DrawGradientBrush = New LinearGradientBrush(r, Color.Empty, Color.Empty, angle)
        DrawGradientBrush.InterpolationColors = blend
        G.FillRectangle(DrawGradientBrush, r)
    End Sub


    Protected Sub DrawGradient(ByVal c1 As Color, ByVal c2 As Color, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
        DrawGradientRectangle = New Rectangle(x, y, width, height)
        DrawGradient(c1, c2, DrawGradientRectangle)
    End Sub
    Protected Sub DrawGradient(ByVal c1 As Color, ByVal c2 As Color, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal angle As Single)
        DrawGradientRectangle = New Rectangle(x, y, width, height)
        DrawGradient(c1, c2, DrawGradientRectangle, angle)
    End Sub

    Protected Sub DrawGradient(ByVal c1 As Color, ByVal c2 As Color, ByVal r As Rectangle)
        DrawGradientBrush = New LinearGradientBrush(r, c1, c2, 90.0F)
        G.FillRectangle(DrawGradientBrush, r)
    End Sub
    Protected Sub DrawGradient(ByVal c1 As Color, ByVal c2 As Color, ByVal r As Rectangle, ByVal angle As Single)
        DrawGradientBrush = New LinearGradientBrush(r, c1, c2, angle)
        G.FillRectangle(DrawGradientBrush, r)
    End Sub

#End Region

#Region " DrawRadial "

    Private DrawRadialPath As GraphicsPath
    Private DrawRadialBrush1 As PathGradientBrush
    Private DrawRadialBrush2 As LinearGradientBrush
    Private DrawRadialRectangle As Rectangle

    Sub DrawRadial(ByVal blend As ColorBlend, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
        DrawRadialRectangle = New Rectangle(x, y, width, height)
        DrawRadial(blend, DrawRadialRectangle, width \ 2, height \ 2)
    End Sub
    Sub DrawRadial(ByVal blend As ColorBlend, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal center As Point)
        DrawRadialRectangle = New Rectangle(x, y, width, height)
        DrawRadial(blend, DrawRadialRectangle, center.X, center.Y)
    End Sub
    Sub DrawRadial(ByVal blend As ColorBlend, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal cx As Integer, ByVal cy As Integer)
        DrawRadialRectangle = New Rectangle(x, y, width, height)
        DrawRadial(blend, DrawRadialRectangle, cx, cy)
    End Sub

    Sub DrawRadial(ByVal blend As ColorBlend, ByVal r As Rectangle)
        DrawRadial(blend, r, r.Width \ 2, r.Height \ 2)
    End Sub
    Sub DrawRadial(ByVal blend As ColorBlend, ByVal r As Rectangle, ByVal center As Point)
        DrawRadial(blend, r, center.X, center.Y)
    End Sub
    Sub DrawRadial(ByVal blend As ColorBlend, ByVal r As Rectangle, ByVal cx As Integer, ByVal cy As Integer)
        DrawRadialPath.Reset()
        DrawRadialPath.AddEllipse(r.X, r.Y, r.Width - 1, r.Height - 1)

        DrawRadialBrush1 = New PathGradientBrush(DrawRadialPath)
        DrawRadialBrush1.CenterPoint = New Point(r.X + cx, r.Y + cy)
        DrawRadialBrush1.InterpolationColors = blend

        If G.SmoothingMode = SmoothingMode.AntiAlias Then
            G.FillEllipse(DrawRadialBrush1, r.X + 1, r.Y + 1, r.Width - 3, r.Height - 3)
        Else
            G.FillEllipse(DrawRadialBrush1, r)
        End If
    End Sub


    Protected Sub DrawRadial(ByVal c1 As Color, ByVal c2 As Color, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
        DrawRadialRectangle = New Rectangle(x, y, width, height)
        DrawRadial(c1, c2, DrawRadialRectangle)
    End Sub
    Protected Sub DrawRadial(ByVal c1 As Color, ByVal c2 As Color, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal angle As Single)
        DrawRadialRectangle = New Rectangle(x, y, width, height)
        DrawRadial(c1, c2, DrawRadialRectangle, angle)
    End Sub

    Protected Sub DrawRadial(ByVal c1 As Color, ByVal c2 As Color, ByVal r As Rectangle)
        DrawRadialBrush2 = New LinearGradientBrush(r, c1, c2, 90.0F)
        G.FillEllipse(DrawRadialBrush2, r)
    End Sub
    Protected Sub DrawRadial(ByVal c1 As Color, ByVal c2 As Color, ByVal r As Rectangle, ByVal angle As Single)
        DrawRadialBrush2 = New LinearGradientBrush(r, c1, c2, angle)
        G.FillEllipse(DrawRadialBrush2, r)
    End Sub

#End Region

#Region " CreateRound "

    Private CreateRoundPath As GraphicsPath
    Private CreateRoundRectangle As Rectangle

    Function CreateRound(ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal slope As Integer) As GraphicsPath
        CreateRoundRectangle = New Rectangle(x, y, width, height)
        Return CreateRound(CreateRoundRectangle, slope)
    End Function

    Function CreateRound(ByVal r As Rectangle, ByVal slope As Integer) As GraphicsPath
        CreateRoundPath = New GraphicsPath(FillMode.Winding)
        CreateRoundPath.AddArc(r.X, r.Y, slope, slope, 180.0F, 90.0F)
        CreateRoundPath.AddArc(r.Right - slope, r.Y, slope, slope, 270.0F, 90.0F)
        CreateRoundPath.AddArc(r.Right - slope, r.Bottom - slope, slope, slope, 0.0F, 90.0F)
        CreateRoundPath.AddArc(r.X, r.Bottom - slope, slope, slope, 90.0F, 90.0F)
        CreateRoundPath.CloseFigure()
        Return CreateRoundPath
    End Function

#End Region

End Class

Module ThemeShare

#Region " Animation "

    Private Frames As Integer
    Private Invalidate As Boolean
    Public ThemeTimer As New PrecisionTimer

    Private Const FPS As Integer = 50 '1000 / 50 = 20 FPS
    Private Const Rate As Integer = 10

    Public Delegate Sub AnimationDelegate(ByVal invalidate As Boolean)

    Private Callbacks As New List(Of AnimationDelegate)

    Private Sub HandleCallbacks(ByVal state As IntPtr, ByVal reserve As Boolean)
        Invalidate = (Frames >= FPS)
        If Invalidate Then Frames = 0

        SyncLock Callbacks
            For I As Integer = 0 To Callbacks.Count - 1
                Callbacks(I).Invoke(Invalidate)
            Next
        End SyncLock

        Frames += Rate
    End Sub

    Private Sub InvalidateThemeTimer()
        If Callbacks.Count = 0 Then
            ThemeTimer.Delete()
        Else
            ThemeTimer.Create(0, Rate, AddressOf HandleCallbacks)
        End If
    End Sub

    Sub AddAnimationCallback(ByVal callback As AnimationDelegate)
        SyncLock Callbacks
            If Callbacks.Contains(callback) Then Return

            Callbacks.Add(callback)
            InvalidateThemeTimer()
        End SyncLock
    End Sub

    Sub RemoveAnimationCallback(ByVal callback As AnimationDelegate)
        SyncLock Callbacks
            If Not Callbacks.Contains(callback) Then Return

            Callbacks.Remove(callback)
            InvalidateThemeTimer()
        End SyncLock
    End Sub

#End Region

End Module


Structure Bloom

    Public _Name As String
    ReadOnly Property Name() As String
        Get
            Return _Name
        End Get
    End Property

    Private _Value As Color
    Property Value() As Color
        Get
            Return _Value
        End Get
        Set(ByVal value As Color)
            _Value = value
        End Set
    End Property

    Property ValueHex() As String
        Get
            Return String.Concat("#", _
            _Value.R.ToString("X2", Nothing), _
            _Value.G.ToString("X2", Nothing), _
            _Value.B.ToString("X2", Nothing))
        End Get
        Set(ByVal value As String)
            Try
                _Value = ColorTranslator.FromHtml(value)
            Catch
                Return
            End Try
        End Set
    End Property


    Sub New(ByVal name As String, ByVal value As Color)
        _Name = name
        _Value = value
    End Sub
End Structure

'------------------
'Creator: aeonhack
'Site: elitevs.net
'Created: 11/30/2011
'Changed: 11/30/2011
'Version: 1.0.0
'------------------
Class PrecisionTimer
    Implements IDisposable

    Private _Enabled As Boolean
    ReadOnly Property Enabled() As Boolean
        Get
            Return _Enabled
        End Get
    End Property

    Private Handle As IntPtr
    Private TimerCallback As TimerDelegate

    <DllImport("kernel32.dll", EntryPoint:="CreateTimerQueueTimer")> _
    Private Shared Function CreateTimerQueueTimer( _
    ByRef handle As IntPtr, _
    ByVal queue As IntPtr, _
    ByVal callback As TimerDelegate, _
    ByVal state As IntPtr, _
    ByVal dueTime As UInteger, _
    ByVal period As UInteger, _
    ByVal flags As UInteger) As Boolean
    End Function

    <DllImport("kernel32.dll", EntryPoint:="DeleteTimerQueueTimer")> _
    Private Shared Function DeleteTimerQueueTimer( _
    ByVal queue As IntPtr, _
    ByVal handle As IntPtr, _
    ByVal callback As IntPtr) As Boolean
    End Function

    Delegate Sub TimerDelegate(ByVal r1 As IntPtr, ByVal r2 As Boolean)

    Sub Create(ByVal dueTime As UInteger, ByVal period As UInteger, ByVal callback As TimerDelegate)
        If _Enabled Then Return

        TimerCallback = callback
        Dim Success As Boolean = CreateTimerQueueTimer(Handle, IntPtr.Zero, TimerCallback, IntPtr.Zero, dueTime, period, 0)

        If Not Success Then ThrowNewException("CreateTimerQueueTimer")
        _Enabled = Success
    End Sub

    Sub Delete()
        If Not _Enabled Then Return
        Dim Success As Boolean = DeleteTimerQueueTimer(IntPtr.Zero, Handle, IntPtr.Zero)

        If Not Success AndAlso Not Marshal.GetLastWin32Error = 997 Then
            ThrowNewException("DeleteTimerQueueTimer")
        End If

        _Enabled = Not Success
    End Sub

    Private Sub ThrowNewException(ByVal name As String)
        Throw New Exception(String.Format("{0} failed. Win32Error: {1}", name, Marshal.GetLastWin32Error))
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Delete()
    End Sub
End Class
#End Region

Class MDSimpleButton
    Inherits ThemeControl154

    Sub New()
        Font = New Font("Segoe UI", 9)
        SetColor("Gradient top normal", 237, 237, 237)
        SetColor("Gradient top over", 242, 242, 242)
        SetColor("Gradient top down", 235, 235, 235)
        SetColor("Gradient bottom normal", 230, 230, 230)
        SetColor("Gradient bottom over", 235, 235, 235)
        SetColor("Gradient bottom down", 223, 223, 223)
        SetColor("Border", 167, 167, 167)
        SetColor("Text normal", 60, 60, 60)
        SetColor("Text down/over", 20, 20, 20)
        SetColor("Text disabled", Color.Gray)
    End Sub

    Dim GTN, GTO, GTD, GBN, GBO, GBD, Bo, TN, TD, TDO As Color
    Protected Overrides Sub ColorHook()
        GTN = GetColor("Gradient top normal")
        GTO = GetColor("Gradient top over")
        GTD = GetColor("Gradient top down")
        GBN = GetColor("Gradient bottom normal")
        GBO = GetColor("Gradient bottom over")
        GBD = GetColor("Gradient bottom down")
        Bo = GetColor("Border")
        TN = GetColor("Text normal")
        TDO = GetColor("Text down/over")
        TD = GetColor("Text disabled")
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(BackColor)
        Dim LGB As LinearGradientBrush
        G.SmoothingMode = SmoothingMode.HighQuality


        Select Case State
            Case MouseState.None
                LGB = New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), GTN, GBN, 90.0F)
            Case MouseState.Over
                LGB = New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), GTO, GBO, 90.0F)
            Case Else
                LGB = New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), GTD, GBD, 90.0F)
        End Select

        If Not Enabled Then
            LGB = New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), GTN, GBN, 90.0F)
        End If

        Dim buttonpath As GraphicsPath = CreateRound(Rectangle.Round(LGB.Rectangle), 3)
        G.FillPath(LGB, CreateRound(Rectangle.Round(LGB.Rectangle), 3))
        If Not Enabled Then G.FillPath(New SolidBrush(Color.FromArgb(50, Color.White)), CreateRound(Rectangle.Round(LGB.Rectangle), 3))
        G.SetClip(buttonpath)
        LGB = New LinearGradientBrush(New Rectangle(0, 0, Width, Height / 6), Color.FromArgb(80, Color.White), Color.Transparent, 90.0F)
        G.FillRectangle(LGB, Rectangle.Round(LGB.Rectangle))



        G.ResetClip()
        G.DrawPath(New Pen(Bo), buttonpath)

        If Enabled Then
            Select Case State
                Case MouseState.None
                    DrawText(New SolidBrush(TN), HorizontalAlignment.Center, 1, 0)
                Case Else
                    DrawText(New SolidBrush(TDO), HorizontalAlignment.Center, 1, 0)
            End Select
        Else
            DrawText(New SolidBrush(TD), HorizontalAlignment.Center, 1, 0)
        End If
    End Sub
End Class

#End Region