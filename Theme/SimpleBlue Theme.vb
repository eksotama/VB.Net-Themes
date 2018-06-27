Option Strict On

Imports System.Drawing.Drawing2D
Imports System.Drawing.Text

'Please Leave Credits in Source, Do not redistribute

'<info>
' -----------------SimpleBlue Theme-----------------
' Creator - SaketSaket (HF)
' UID - 1869668
' Inspiration & Credits to all Theme creaters of HF
' Version - 1.0
' Date Created - 17th June 2014
' Date Modified - 21th June 2014
'
' For bugs & Constructive Criticism contact me on HF
' If you like it & want to donate then pm me on HF
' -----------------SimpleBlue Theme-----------------
'<info>

'Please Leave Credits in Source, Do not redistribute

Enum MouseState As Byte
    None = 0
    Over = 1
    Down = 2
End Enum

Module Draw
    'Special Thanks to Aeonhack for RoundRect Functins... ;)
    Public Function RoundRect(ByVal rectangle As Rectangle, ByVal curve As Integer) As GraphicsPath
        Dim p As GraphicsPath = New GraphicsPath()
        Dim arcRectangleWidth As Integer = curve * 2
        p.AddArc(New Rectangle(rectangle.X, rectangle.Y, arcRectangleWidth, arcRectangleWidth), -180, 90)
        p.AddArc(New Rectangle(rectangle.Width - arcRectangleWidth + rectangle.X, rectangle.Y, arcRectangleWidth, arcRectangleWidth), -90, 90)
        p.AddArc(New Rectangle(rectangle.Width - arcRectangleWidth + rectangle.X, rectangle.Height - arcRectangleWidth + rectangle.Y, arcRectangleWidth, arcRectangleWidth), 0, 90)
        p.AddArc(New Rectangle(rectangle.X, rectangle.Height - arcRectangleWidth + rectangle.Y, arcRectangleWidth, arcRectangleWidth), 90, 90)
        p.AddLine(New Point(rectangle.X, rectangle.Height - arcRectangleWidth + rectangle.Y), New Point(rectangle.X, curve + rectangle.Y))
        Return p
    End Function
    Public Function RoundRect(ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal curve As Integer) As GraphicsPath
        Dim rectangle As Rectangle = New Rectangle(x, y, width, height)
        Dim p As GraphicsPath = New GraphicsPath()
        Dim arcRectangleWidth As Integer = curve * 2
        p.AddArc(New Rectangle(rectangle.X, rectangle.Y, arcRectangleWidth, arcRectangleWidth), -180, 90)
        p.AddArc(New Rectangle(rectangle.Width - arcRectangleWidth + rectangle.X, rectangle.Y, arcRectangleWidth, arcRectangleWidth), -90, 90)
        p.AddArc(New Rectangle(rectangle.Width - arcRectangleWidth + rectangle.X, rectangle.Height - arcRectangleWidth + rectangle.Y, arcRectangleWidth, arcRectangleWidth), 0, 90)
        p.AddArc(New Rectangle(rectangle.X, rectangle.Height - arcRectangleWidth + rectangle.Y, arcRectangleWidth, arcRectangleWidth), 90, 90)
        p.AddLine(New Point(rectangle.X, rectangle.Height - arcRectangleWidth + rectangle.Y), New Point(rectangle.X, curve + rectangle.Y))
        Return p
    End Function
End Module

Public Class SimpleBlueTheme : Inherits ContainerControl

    Private _icon As Icon

    Public Property Icon() As Icon
        Get
            Return _icon
        End Get
        Set(ByVal value As Icon)
            _icon = value
            Invalidate()
        End Set
    End Property

    Sub New()
        MyBase.New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        DoubleBuffered = True
        Dock = DockStyle.Fill
    End Sub

    Protected Overrides Sub OnInvalidated(ByVal e As InvalidateEventArgs)
        MyBase.OnInvalidated(e)
        ParentForm.FindForm.Text = Text
    End Sub

    Protected Overrides Sub OnTextChanged(ByVal e As EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        ParentForm.FormBorderStyle = FormBorderStyle.None
        If ParentForm.FindForm.TransparencyKey = Nothing Then ParentForm.FindForm.TransparencyKey = Color.Fuchsia
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g As Graphics = e.Graphics
        g.SmoothingMode = SmoothingMode.HighQuality
        Dim bluePen As New Pen(Color.FromArgb(100, 150, 180), 3)
        Dim totalrect As Rectangle = New Rectangle(0, 0, Width - 1, Height - 1)
        Dim headerrect As Rectangle = New Rectangle(5, 5, Width - 11, 35)
        Dim shadowheaderrect As Rectangle = New Rectangle(7, 8, Width - 11, 35)
        g.FillRectangle(New SolidBrush(Color.FromArgb(230, 230, 230)), totalrect)
        g.DrawRectangle(bluePen, totalrect)
        g.FillPath(New SolidBrush(Color.FromArgb(100, 150, 180)), RoundRect(headerrect, 5))
        g.DrawString(Text, New Font("Tahoma", 12, FontStyle.Bold), New SolidBrush(Color.FromArgb(0, 0, 0)), headerrect, New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
        g.DrawString(Text, New Font("Tahoma", 12, FontStyle.Bold), New SolidBrush(Color.FromArgb(255, 255, 255)), shadowheaderrect, New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
        Try
            g.DrawIcon(_icon, New Rectangle(8, 8, 30, 30))
        Catch : End Try
    End Sub
End Class

Public Class SimpleBlueButton : Inherits Control

    Dim _state As MouseState

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        _state = MouseState.Down
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        _state = MouseState.Over
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        MyBase.OnMouseEnter(e)
        _state = MouseState.Over
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        MyBase.OnMouseLeave(e)
        _state = MouseState.None
        Invalidate()
    End Sub

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        Size = New Size(150, 30)
        _state = MouseState.None
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g As Graphics = e.Graphics
        g.SmoothingMode = SmoothingMode.HighQuality
        Dim outerrect As Rectangle = New Rectangle(0, 0, Width - 1, Height - 1)
        Dim innerrect As Rectangle = New Rectangle(2, 2, Width - 5, Height - 5)
        g.FillPath(New SolidBrush(Color.FromArgb(100, 150, 180)), RoundRect(outerrect, 3))
        g.FillPath(New SolidBrush(Color.FromArgb(210, 210, 210)), RoundRect(innerrect, 3))
        Dim btnfont As New Font("Tahoma", 10, FontStyle.Bold)
        Select Case _state
            Case MouseState.None
                g.DrawString(Text, btnfont, Brushes.Black, New Rectangle(0, 0, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
            Case MouseState.Down
                g.DrawString(Text, btnfont, Brushes.LightSlateGray, New Rectangle(0, 0, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
            Case MouseState.Over
                g.DrawString(Text, btnfont, Brushes.Blue, New Rectangle(0, 0, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
        End Select
    End Sub
End Class

Public Class SimpleTransperentLabel : Inherits Label

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        BackColor = Color.Transparent
        Font = New Font("Tahoma", 10, FontStyle.Regular)
    End Sub

    Protected Overrides Sub OnTextChanged(e As EventArgs)
        MyBase.OnTextChanged(e) : Invalidate()
    End Sub
End Class

Public Class SimpleBluePanel : Inherits ContainerControl

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        MyBase.OnResize(e)
        Invalidate()
    End Sub

    Sub New()
        MyBase.New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        Size = New Size(240, 160)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g As Graphics = e.Graphics
        g.SmoothingMode = SmoothingMode.HighQuality
        Dim outerrect As Rectangle = New Rectangle(0, 0, Width - 1, Height - 1)
        Dim innerrect As Rectangle = New Rectangle(2, 2, Width - 5, Height - 5)
        g.FillPath(New SolidBrush(Color.FromArgb(100, 150, 180)), RoundRect(outerrect, 3))
        g.FillPath(New SolidBrush(Color.FromArgb(210, 210, 210)), RoundRect(innerrect, 3))
    End Sub
End Class

Public Class SimpleBlueGroupBox : Inherits ContainerControl

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        MyBase.OnResize(e)
        Invalidate()
    End Sub

    Sub New()
        MyBase.New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        Size = New Size(240, 160)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g As Graphics = e.Graphics
        g.SmoothingMode = SmoothingMode.HighQuality
        Dim semiBluePen As New Pen(Color.FromArgb(100, 150, 180), 2)
        Dim outerrect As Rectangle = New Rectangle(0, 0, Width - 1, Height - 1)
        Dim innerrect As Rectangle = New Rectangle(2, 2, Width - 5, Height - 5)
        Dim headerrect As Rectangle = New Rectangle(7, 7, Width - 15, 20)
        g.FillPath(New SolidBrush(Color.FromArgb(100, 150, 180)), RoundRect(outerrect, 3))
        g.FillPath(New SolidBrush(Color.FromArgb(210, 210, 210)), RoundRect(innerrect, 3))
        g.DrawRectangle(semiBluePen, headerrect)
        g.DrawString(Text, New Font("Tahoma", 10, FontStyle.Bold), New SolidBrush(Color.FromArgb(0, 0, 0)), New Rectangle(0, 0, Width - 1, 35), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
    End Sub
End Class

Public Class SimpleBlueRadioButton : Inherits Control

    Private _check As Boolean

    Public Property Checked As Boolean
        Get
            Return _check
        End Get
        Set(value As Boolean)
            _check = value
            Invalidate()
        End Set
    End Property

    Sub New()
        MyBase.New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        Size = New Size(200, 25)
    End Sub

    Protected Overrides Sub OnTextChanged(ByVal e As EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnClick(ByVal e As EventArgs)
        MyBase.OnClick(e)
        If Not Checked Then Checked = True
        For Each ctrl As Control In Parent.Controls
            If TypeOf ctrl Is SimpleBlueRadioButton Then
                If ctrl.Handle = Handle Then Continue For
                If ctrl.Enabled Then DirectCast(ctrl, SimpleBlueRadioButton).Checked = False
            End If
        Next
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g As Graphics = e.Graphics
        g.SmoothingMode = SmoothingMode.HighQuality
        Dim outerrect As Rectangle = New Rectangle(0, 0, Width, Height)
        Dim innerrect As Rectangle = New Rectangle(2, 2, Width - 4, Height - 4)
        Dim selectionrect As Rectangle = New Rectangle(5, 5, 15, 15)
        g.FillRectangle(New SolidBrush(Color.FromArgb(100, 150, 180)), outerrect)
        g.FillRectangle(New SolidBrush(Color.FromArgb(210, 210, 210)), innerrect)
        g.FillEllipse(New SolidBrush(Color.FromArgb(255, 255, 255)), selectionrect)
        g.DrawString(Text, New Font("Tahoma", 10, FontStyle.Bold), New SolidBrush(Color.FromArgb(0, 0, 0)), New Rectangle(10, 5, Width, 15), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
        If Checked Then
            selectionrect.Inflate(-2, -2)
            g.FillEllipse(New SolidBrush(Color.FromArgb(100, 150, 180)), selectionrect)
        Else
            g.FillEllipse(New SolidBrush(Color.FromArgb(255, 255, 255)), selectionrect)
        End If
    End Sub
End Class

Public Class SimpleBlueCheckBox : Inherits Control

    Private _check As Boolean

    Public Property Checked As Boolean
        Get
            Return _check
        End Get
        Set(value As Boolean)
            _check = value
            Invalidate()
        End Set
    End Property

    Sub New()
        MyBase.New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        Size = New Size(200, 25)
    End Sub

    Protected Overrides Sub OnTextChanged(ByVal e As EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnClick(ByVal e As EventArgs)
        MyBase.OnClick(e)
        If Not Checked Then Checked = True Else Checked = False
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g As Graphics = e.Graphics
        g.SmoothingMode = SmoothingMode.HighQuality
        Dim outerrect As Rectangle = New Rectangle(0, 0, Width, Height)
        Dim innerrect As Rectangle = New Rectangle(2, 2, Width - 4, Height - 4)
        Dim selectionrect As Rectangle = New Rectangle(5, 5, 15, 15)
        g.FillRectangle(New SolidBrush(Color.FromArgb(100, 150, 180)), outerrect)
        g.FillRectangle(New SolidBrush(Color.FromArgb(210, 210, 210)), innerrect)
        g.FillEllipse(New SolidBrush(Color.FromArgb(255, 255, 255)), selectionrect)
        g.DrawString(Text, New Font("Tahoma", 10, FontStyle.Bold), New SolidBrush(Color.FromArgb(0, 0, 0)), New Rectangle(10, 5, Width, 15), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
        Dim innerrectextra As Rectangle = New Rectangle(5, 5, 15, 18)
        If Checked Then
            g.DrawString("b", New Font("Marlett", 10, FontStyle.Bold), New SolidBrush(Color.FromArgb(100, 150, 180)), innerrectextra, New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
        End If
    End Sub
End Class

Public Class SimpleBlueToggle : Inherits Control

    Private _check As Boolean

    Public Property Checked As Boolean
        Get
            Return _check
        End Get
        Set(value As Boolean)
            _check = value
            Invalidate()
        End Set
    End Property

    Sub New()
        MyBase.New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        Size = New Size(80, 25)
    End Sub

    Protected Overrides Sub OnTextChanged(ByVal e As EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnClick(ByVal e As EventArgs)
        MyBase.OnClick(e)
        If Not Checked Then Checked = True Else Checked = False
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g As Graphics = e.Graphics
        g.SmoothingMode = SmoothingMode.HighQuality
        Dim cr As Rectangle = New Rectangle(0, 0, Width - 1, Height - 1)
        g.FillRectangle(New SolidBrush(Color.FromArgb(100, 150, 180)), cr)
        If Checked Then
            g.FillRectangle(New SolidBrush(Color.FromArgb(230, 230, 230)), New Rectangle(CInt((Width / 2) - 2), 2, CInt((Width / 2) - 1), Height - 5))
            g.DrawString("ON", New Font("Tahoma", 10, FontStyle.Bold), New SolidBrush(Color.FromArgb(255, 255, 255)), New Rectangle(0, 5, CInt((Width / 2)), 15), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
            g.DrawLine(New Pen(Color.Black), 56, 5, 56, Height - 7)
            g.DrawLine(New Pen(Color.Black), 58, 3, 58, Height - 5)
            g.DrawLine(New Pen(Color.Black), 60, 5, 60, Height - 7)
        Else
            g.FillRectangle(New SolidBrush(Color.FromArgb(230, 230, 230)), New Rectangle(2, 2, CInt((Width / 2) - 1), Height - 5))
            g.DrawString("OFF", New Font("Tahoma", 10, FontStyle.Bold), New SolidBrush(Color.FromArgb(255, 255, 255)), New Rectangle(CInt((Width / 2)), 5, CInt((Width / 2)), 15), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
            g.DrawLine(New Pen(Color.Black), 20, 5, 20, Height - 7)
            g.DrawLine(New Pen(Color.Black), 22, 3, 22, Height - 5)
            g.DrawLine(New Pen(Color.Black), 24, 5, 24, Height - 7)
        End If
    End Sub
End Class

Public Class SimpleBlueStatusStrip : Inherits StatusStrip

    Sub New()
        BackColor = Color.FromArgb(100, 150, 180)
        ForeColor = Color.Ivory
        Font = New Font("Tahoma", 8)
        SizingGrip = False
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw, True)
    End Sub

    Protected Sub CreateSss()
        Dim g As Graphics = CreateGraphics()
        g.DrawRectangle(New Pen(Color.FromArgb(0, 0, 0)), New Rectangle(1, 1, Width - 3, Height - 3))
    End Sub

    Protected Overrides Sub WndProc(ByRef m As Message)
        If m.Msg = 15 Then
            Invalidate()
            MyBase.WndProc(m)
            CreateSss()
        Else
            MyBase.WndProc(m)
        End If
    End Sub

    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        Dock = DockStyle.Bottom
    End Sub
End Class

Public Class SimpleBlueProgressBar : Inherits Control

    Private _val As Integer
    Public Property Value() As Integer
        Get
            Return _val
        End Get
        Set(ByVal v As Integer)
            If v > _max Then
                _val = _max
            ElseIf v < 0 Then
                _val = 0
            Else
                _val = v
            End If
            Invalidate()
        End Set
    End Property

    Private _max As Integer
    Public Property Maximum() As Integer
        Get
            Return _max
        End Get
        Set(ByVal v As Integer)
            If v < 1 Then
                _max = 1
            Else
                _max = v
            End If
            If v < _val Then
                _val = _max
            End If
            Invalidate()
        End Set
    End Property

    Private _showPercentage As Boolean = False
    Public Property ShowPercentage() As Boolean
        Get
            Return _ShowPercentage
        End Get
        Set(ByVal v As Boolean)
            _ShowPercentage = v
            Invalidate()
        End Set
    End Property

    Sub New()
        MyBase.New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        Size = New Size(300, 30)
        _max = 100
    End Sub

    Protected Overrides Sub OnPaint(e As Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g As Graphics = e.Graphics
        g.SmoothingMode = SmoothingMode.HighQuality
        Dim percent As Integer = CInt((Width - 1) * (_val / _max))
        g.FillRectangle(New SolidBrush(Color.FromArgb(100, 150, 180)), New Rectangle(2, 2, CInt(percent) - 4, Height - 5))
        If _showPercentage Then
            g.DrawString(String.Format("{0}%", _val), New Font("Tahoma", 10, FontStyle.Bold), Brushes.Black, New Rectangle(10, 0, Width - 1, Height - 1), New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
        End If
        g.DrawRectangle(New Pen(Color.FromArgb(100, 150, 180)), New Rectangle(0, 0, Width - 1, Height - 1))
    End Sub
End Class

Public Class SimpleBlueControlBox : Inherits Control

    Dim _state As MouseState = MouseState.None
    Dim _x As Integer
    ReadOnly _minrect As New Rectangle(5, 2, 20, 20)
    ReadOnly _maxrect As New Rectangle(28, 2, 20, 20)
    ReadOnly _closerect As New Rectangle(51, 2, 20, 20)

    Protected Overrides Sub OnMouseDown(ByVal e As Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        If _x > 5 AndAlso _x < 25 Then
            FindForm.WindowState = FormWindowState.Minimized
        ElseIf _x > 28 AndAlso _x < 48 Then
            If FindForm.WindowState = FormWindowState.Maximized Then
                FindForm.WindowState = FormWindowState.Minimized
                FindForm.WindowState = FormWindowState.Normal
            Else
                FindForm.WindowState = FormWindowState.Minimized
                FindForm.WindowState = FormWindowState.Maximized
            End If

        ElseIf _x > 51 AndAlso _x < 71 Then
            FindForm.Close()
        End If
        _state = MouseState.Down : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseUp(ByVal e As Windows.Forms.MouseEventArgs)
        MyBase.OnMouseUp(e)
        _state = MouseState.Over : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        MyBase.OnMouseEnter(e)
        _state = MouseState.Over : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        MyBase.OnMouseLeave(e)
        _state = MouseState.None : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(e)
        _x = e.Location.X
        Invalidate()
    End Sub

    Sub New()
        MyBase.New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        DoubleBuffered = True
        Anchor = AnchorStyles.Top Or AnchorStyles.Right
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g As Graphics = e.Graphics
        g.SmoothingMode = SmoothingMode.HighQuality

        Dim minrdefault As New LinearGradientBrush(_minrect, Color.FromArgb(0, 0, 150), Color.FromArgb(0, 0, 255), 90S)
        g.FillEllipse(minrdefault, _minrect)
        g.DrawEllipse(Pens.DimGray, _minrect)

        Dim maxrdefault As New LinearGradientBrush(_maxrect, Color.FromArgb(0, 150, 0), Color.FromArgb(0, 255, 0), 90S)
        g.FillEllipse(maxrdefault, _maxrect)
        g.DrawEllipse(Pens.DimGray, _maxrect)

        Dim crdefault As New LinearGradientBrush(_closerect, Color.FromArgb(150, 0, 0), Color.FromArgb(255, 0, 0), 90S)
        g.FillEllipse(crdefault, _closerect)
        g.DrawEllipse(Pens.DimGray, _closerect)

        Select Case _state
            Case MouseState.None
                Dim minrnone As New LinearGradientBrush(_minrect, Color.FromArgb(0, 0, 150), Color.FromArgb(0, 0, 255), 90S)
                g.FillEllipse(minrnone, _minrect)
                g.DrawEllipse(Pens.DimGray, _minrect)

                Dim maxrnone As New LinearGradientBrush(_maxrect, Color.FromArgb(0, 150, 0), Color.FromArgb(0, 255, 0), 90S)
                g.FillEllipse(maxrnone, _maxrect)
                g.DrawEllipse(Pens.DimGray, _maxrect)

                Dim crnone As New LinearGradientBrush(_closerect, Color.FromArgb(150, 0, 0), Color.FromArgb(255, 0, 0), 90S)
                g.FillEllipse(crnone, _closerect)
                g.DrawEllipse(Pens.DimGray, _closerect)
            Case MouseState.Over
                If _x > 5 AndAlso _x < 25 Then
                    Dim minrover As New LinearGradientBrush(_minrect, Color.FromArgb(0, 0, 100), Color.FromArgb(0, 0, 200), 90S)
                    g.FillEllipse(minrover, _minrect)
                    g.DrawEllipse(Pens.DimGray, _minrect)
                ElseIf _x > 28 AndAlso _x < 48 Then
                    Dim maxrover As New LinearGradientBrush(_maxrect, Color.FromArgb(0, 100, 0), Color.FromArgb(0, 200, 0), 90S)
                    g.FillEllipse(maxrover, _maxrect)
                    g.DrawEllipse(Pens.DimGray, _maxrect)
                ElseIf _x > 51 AndAlso _x < 71 Then
                    Dim crover As New LinearGradientBrush(_closerect, Color.FromArgb(100, 0, 0), Color.FromArgb(200, 0, 0), 90S)
                    g.FillEllipse(crover, _closerect)
                    g.DrawEllipse(Pens.DimGray, _closerect)
                End If
            Case Else
                Dim minrelse As New LinearGradientBrush(_minrect, Color.FromArgb(0, 0, 150), Color.FromArgb(0, 0, 255), 90S)
                g.FillEllipse(minrelse, _minrect)
                g.DrawEllipse(Pens.DimGray, _minrect)

                Dim maxrelse As New LinearGradientBrush(_maxrect, Color.FromArgb(0, 150, 0), Color.FromArgb(0, 255, 0), 90S)
                g.FillEllipse(maxrelse, _maxrect)
                g.DrawEllipse(Pens.DimGray, _maxrect)

                Dim crelse As New LinearGradientBrush(_closerect, Color.FromArgb(150, 0, 0), Color.FromArgb(255, 0, 0), 90S)
                g.FillEllipse(crelse, _closerect)
                g.DrawEllipse(Pens.DimGray, _closerect)
        End Select
    End Sub
End Class

Public Class SimpleBlueLabel : Inherits Control

    Sub New()
        MyBase.New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        DoubleBuffered = True
        Size = New Size(200, 30)
    End Sub

    Protected Overrides Sub OnTextChanged(e As EventArgs)
        MyBase.OnTextChanged(e) : Invalidate()
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g As Graphics = e.Graphics
        g.SmoothingMode = SmoothingMode.HighQuality
        Dim outerrect As Rectangle = New Rectangle(0, 0, Width - 1, Height - 1)
        Dim innerrect As Rectangle = New Rectangle(2, 2, Width - 5, Height - 5)
        g.FillPath(New SolidBrush(Color.FromArgb(100, 150, 180)), RoundRect(outerrect, 3))
        g.FillPath(New SolidBrush(Color.FromArgb(210, 210, 210)), RoundRect(innerrect, 3))
        g.DrawString(Text, New Font("Tahoma", 10, FontStyle.Bold), New SolidBrush(Color.FromArgb(0, 0, 0)), New Rectangle(0, 0, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
    End Sub
End Class

Public Class SimpleBlueTextBox : Inherits Control
    Dim WithEvents _tb As New TextBox

    Private _allowpassword As Boolean = False
    Public Shadows Property UseSystemPasswordChar() As Boolean
        Get
            Return _allowpassword
        End Get
        Set(ByVal v As Boolean)
            _tb.UseSystemPasswordChar = UseSystemPasswordChar
            _allowpassword = v
            Invalidate()
        End Set
    End Property

    Private _maxChars As Integer = 32767
    Public Shadows Property MaxLength() As Integer
        Get
            Return _maxChars
        End Get
        Set(ByVal v As Integer)
            _maxChars = v
            _tb.MaxLength = MaxLength
            Invalidate()
        End Set
    End Property

    Private _textAlignment As HorizontalAlignment
    Public Shadows Property TextAlign() As HorizontalAlignment
        Get
            Return _textAlignment
        End Get
        Set(ByVal v As HorizontalAlignment)
            _textAlignment = v
            Invalidate()
        End Set
    End Property

    Private _multiLine As Boolean = False
    Public Shadows Property MultiLine() As Boolean
        Get
            Return _multiLine
        End Get
        Set(ByVal v As Boolean)
            _multiLine = v
            _tb.Multiline = v
            OnResize(EventArgs.Empty)
            Invalidate()
        End Set
    End Property

    Private _readOnly As Boolean = False
    Public Shadows Property [ReadOnly]() As Boolean
        Get
            Return _readOnly
        End Get
        Set(ByVal v As Boolean)
            _readOnly = v
            If _tb IsNot Nothing Then
                _tb.ReadOnly = v
            End If
        End Set
    End Property

    Protected Overrides Sub OnTextChanged(ByVal e As EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnBackColorChanged(ByVal e As EventArgs)
        MyBase.OnBackColorChanged(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnForeColorChanged(ByVal e As EventArgs)
        MyBase.OnForeColorChanged(e)
        _tb.ForeColor = ForeColor
        Invalidate()
    End Sub

    Protected Overrides Sub OnFontChanged(ByVal e As EventArgs)
        MyBase.OnFontChanged(e)
        _tb.Font = Font
    End Sub

    Protected Overrides Sub OnGotFocus(ByVal e As EventArgs)
        MyBase.OnGotFocus(e)
        _tb.Focus()
    End Sub

    Private Sub TextChangeTb() Handles _tb.TextChanged
        Text = _tb.Text
    End Sub

    Private Sub TextChng() Handles MyBase.TextChanged
        _tb.Text = Text
    End Sub

    Public Sub NewTextBox()
        With _tb
            .Text = String.Empty
            .BackColor = Color.FromArgb(210, 210, 210)
            .TextAlign = HorizontalAlignment.Center
            .BorderStyle = BorderStyle.None
            .Location = New Point(3, 3)
            .Font = New Font("Tahoma", 10, FontStyle.Bold)
            .Size = New Size(Width - 3, Height - 3)
            .UseSystemPasswordChar = UseSystemPasswordChar
        End With
    End Sub

    Sub New()
        MyBase.New()
        NewTextBox()
        Controls.Add(_tb)
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        ForeColor = Color.Black
        Font = New Font("Tahoma", 10, FontStyle.Bold)
        Size = New Size(150, 30)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g As Graphics = e.Graphics
        g.SmoothingMode = SmoothingMode.HighQuality
        With _tb
            .TextAlign = TextAlign
            .UseSystemPasswordChar = UseSystemPasswordChar
        End With
        Dim outerrect As Rectangle = New Rectangle(0, 0, Width - 1, Height - 1)
        Dim innerrect As Rectangle = New Rectangle(2, 2, Width - 5, Height - 5)
        g.FillPath(New SolidBrush(Color.FromArgb(100, 150, 180)), RoundRect(outerrect, 3))
        g.FillPath(New SolidBrush(Color.FromArgb(210, 210, 210)), RoundRect(innerrect, 3))
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        If Not MultiLine Then
            Dim tbheight As Integer = _tb.Height
            _tb.Location = New Point(10, CType(((Height / 2) - (tbheight / 2) - 1), Integer))
            _tb.Size = New Size(Width - 20, tbheight)
        Else
            _tb.Location = New Point(10, 10)
            _tb.Size = New Size(Width - 20, Height - 20)
        End If
    End Sub
End Class

Public Class SimpleBlueComboBox : Inherits ComboBox

    Private _startIndex As Integer = 0
    Private Property StartIndex As Integer
        Get
            Return _startIndex
        End Get
        Set(ByVal v As Integer)
            _startIndex = v
            Try
                SelectedIndex = v
            Catch
            End Try
            Invalidate()
        End Set
    End Property

    Sub ReplaceItem(ByVal sender As System.Object, ByVal e As Windows.Forms.DrawItemEventArgs) Handles Me.DrawItem
        e.DrawBackground()
        Try
            If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
                e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(220, 220, 220)), e.Bounds)
            Else
                e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(210, 210, 210)), e.Bounds)
            End If
            e.Graphics.DrawString(GetItemText(Items(e.Index)), e.Font, New SolidBrush(Color.FromArgb(0, 0, 0)), New Rectangle(e.Bounds.X, e.Bounds.Y + 1, e.Bounds.Width, e.Bounds.Height))
        Catch
        End Try
    End Sub

    Sub New()
        MyBase.New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.SupportsTransparentBackColor, True)
        DrawMode = Windows.Forms.DrawMode.OwnerDrawFixed
        BackColor = Color.Transparent
        DropDownStyle = ComboBoxStyle.DropDownList
        StartIndex = 0
        ItemHeight = 25
        DoubleBuffered = True
        Width = 200
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        Height = 20
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g As Graphics = e.Graphics
        g.SmoothingMode = SmoothingMode.HighQuality
        Dim outerrect As Rectangle = New Rectangle(0, 0, Width - 1, Height - 1)
        Dim innerrect As Rectangle = New Rectangle(2, 2, Width - 5, Height - 5)
        g.FillPath(New SolidBrush(Color.FromArgb(100, 150, 180)), RoundRect(outerrect, 3))
        g.FillPath(New SolidBrush(Color.FromArgb(210, 210, 210)), RoundRect(innerrect, 3))
        g.SetClip(RoundRect(innerrect, 3))
        g.FillRectangle(New SolidBrush(Color.FromArgb(210, 210, 210)), innerrect)
        g.ResetClip()
        g.DrawLine(Pens.White, Width - 9, 10, Width - 22, 10)
        g.DrawLine(Pens.White, Width - 9, 11, Width - 22, 11)
        g.DrawLine(Pens.White, Width - 9, 15, Width - 22, 15)
        g.DrawLine(Pens.White, Width - 9, 16, Width - 22, 16)
        g.DrawLine(Pens.White, Width - 9, 20, Width - 22, 20)
        g.DrawLine(Pens.White, Width - 9, 21, Width - 22, 21)
        g.DrawLine(New Pen(Color.FromArgb(100, 150, 180)), New Point(Width - 29, 1), New Point(Width - 29, Height - 2))
        g.DrawLine(New Pen(Color.FromArgb(100, 150, 180)), New Point(Width - 30, 1), New Point(Width - 30, Height - 2))
        Try
            g.DrawString(Text, New Font("Tahoma", 10, FontStyle.Bold), New SolidBrush(Color.FromArgb(0, 0, 0)), New Rectangle(7, 0, Width - 1, Height - 1), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Near})
        Catch
        End Try
    End Sub
End Class

Public Class SimpleBlueContextMenu : Inherits ContextMenuStrip

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint, True)
        DoubleBuffered = True
        ForeColor = Color.FromArgb(0, 0, 0)
        BackColor = Color.FromArgb(230, 230, 230)
        Font = New Font("Tahoma", 8, FontStyle.Bold)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        CreateGraphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit
    End Sub

    Private _item As ToolStripItem
    Private Sub DrawItem(g As Graphics)
        If _item IsNot Nothing Then
            Dim rect As Rectangle = New Rectangle(_item.Bounds.X + 2, _item.Bounds.Y + 2, _item.Bounds.Width - 4, _item.Bounds.Height - 4)
            g.SetClip(rect)
            g.FillRectangle(New SolidBrush(Color.FromArgb(230, 230, 230)), rect)
            g.ResetClip()
        End If
    End Sub

    Protected Overrides Sub OnMouseMove(mousearea As MouseEventArgs)
        MyBase.OnMouseMove(mousearea)
        _item = GetItemAt(mousearea.Location)
        DrawItem(CreateGraphics)
    End Sub
End Class

Public Class SimpleBlueTabControl : Inherits TabControl

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint, True)
    End Sub

    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        Select Case Alignment
            Case TabAlignment.Left
                SizeMode = TabSizeMode.Fixed
                ItemSize = New Size(24, 80)
            Case TabAlignment.Right
                SizeMode = TabSizeMode.Fixed
                ItemSize = New Size(24, 80)
            Case Else
                SizeMode = TabSizeMode.Normal
                ItemSize = New Size(80, 24)
        End Select
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim g As Graphics = e.Graphics
        Dim itemBounds As Rectangle
        For tabItemIndex As Integer = 0 To TabCount - 1
            itemBounds = GetTabRect(tabItemIndex)
            If Not tabItemIndex = SelectedIndex Then
                Select Case Alignment
                    Case TabAlignment.Left
                        g.FillPath(New SolidBrush(Color.FromArgb(230, 230, 230)), RoundRect(New Rectangle(New Point(itemBounds.X + 2, itemBounds.Y), New Size(itemBounds.Width, itemBounds.Height)), 2))
                        g.DrawPath(New Pen(Color.FromArgb(100, 150, 180)), RoundRect(New Rectangle(New Point(itemBounds.X, itemBounds.Y), New Size(itemBounds.Width, itemBounds.Height)), 2))
                        Try
                            g.DrawString(TabPages(tabItemIndex).Text, New Font(Font.Name, Font.Size - 1), New SolidBrush(Color.FromArgb(0, 0, 0)), New Rectangle(GetTabRect(tabItemIndex).Location, GetTabRect(tabItemIndex).Size), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
                            TabPages(tabItemIndex).BackColor = Color.FromArgb(210, 210, 210)
                        Catch : End Try
                    Case TabAlignment.Right
                        g.FillPath(New SolidBrush(Color.FromArgb(230, 230, 230)), RoundRect(New Rectangle(New Point(itemBounds.X, itemBounds.Y), New Size(itemBounds.Width, itemBounds.Height)), 2))
                        g.DrawPath(New Pen(Color.FromArgb(100, 150, 180)), RoundRect(New Rectangle(New Point(itemBounds.X, itemBounds.Y), New Size(itemBounds.Width, itemBounds.Height)), 2))
                        Try
                            g.DrawString(TabPages(tabItemIndex).Text, New Font(Font.Name, Font.Size - 1), New SolidBrush(Color.FromArgb(0, 0, 0)), New Rectangle(GetTabRect(tabItemIndex).Location, GetTabRect(tabItemIndex).Size), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
                            TabPages(tabItemIndex).BackColor = Color.FromArgb(210, 210, 210)
                        Catch : End Try
                    Case TabAlignment.Top
                        g.FillPath(New SolidBrush(Color.FromArgb(230, 230, 230)), RoundRect(New Rectangle(New Point(itemBounds.X, itemBounds.Y + 2), New Size(itemBounds.Width, itemBounds.Height)), 2))
                        g.DrawPath(New Pen(Color.FromArgb(100, 150, 180)), RoundRect(New Rectangle(New Point(itemBounds.X, itemBounds.Y + 2), New Size(itemBounds.Width, itemBounds.Height)), 2))
                        Try
                            g.DrawString(TabPages(tabItemIndex).Text, New Font(Font.Name, Font.Size - 1), New SolidBrush(Color.FromArgb(0, 0, 0)), New Rectangle(GetTabRect(tabItemIndex).Location, GetTabRect(tabItemIndex).Size), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
                            TabPages(tabItemIndex).BackColor = Color.FromArgb(210, 210, 210)
                        Catch : End Try
                    Case TabAlignment.Bottom
                        g.FillPath(New SolidBrush(Color.FromArgb(230, 230, 230)), RoundRect(New Rectangle(New Point(itemBounds.X, itemBounds.Y - 6 + 2), New Size(itemBounds.Width, itemBounds.Height)), 2))
                        g.DrawPath(New Pen(Color.FromArgb(100, 150, 180)), RoundRect(New Rectangle(New Point(itemBounds.X, itemBounds.Y - 6 + 2), New Size(itemBounds.Width, itemBounds.Height)), 2))
                        Try
                            g.DrawString(TabPages(tabItemIndex).Text, New Font(Font.Name, Font.Size - 1), New SolidBrush(Color.FromArgb(0, 0, 0)), New Rectangle(GetTabRect(tabItemIndex).Location, GetTabRect(tabItemIndex).Size), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
                            TabPages(tabItemIndex).BackColor = Color.FromArgb(210, 210, 210)
                        Catch : End Try
                End Select
            End If
        Next
        Select Case Alignment
            Case TabAlignment.Top
                g.FillPath(New SolidBrush(Color.FromArgb(230, 230, 230)), RoundRect(0, 23, Width - 1, Height - 26, 2))
                g.DrawPath(New Pen(Color.FromArgb(100, 150, 180)), RoundRect(0, 23, Width - 1, Height - 26, 2))
            Case TabAlignment.Bottom
                g.FillPath(New SolidBrush(Color.FromArgb(230, 230, 230)), RoundRect(0, 1, Width - 3, Height - 26, 2))
                g.DrawPath(New Pen(Color.FromArgb(100, 150, 180)), RoundRect(0, 1, Width - 3, Height - 26, 2))
            Case TabAlignment.Right
                g.FillPath(New SolidBrush(Color.FromArgb(230, 230, 230)), RoundRect(0, 2, Width - 83, Height - 3, 2))
                g.DrawPath(New Pen(Color.FromArgb(100, 150, 180)), RoundRect(0, 2, Width - 83, Height - 3, 2))
            Case TabAlignment.Left
                g.FillPath(New SolidBrush(Color.FromArgb(230, 230, 230)), RoundRect(81, 2, Width - 81, Height - 3, 2))
                g.DrawPath(New Pen(Color.FromArgb(100, 150, 180)), RoundRect(81, 2, Width - 81, Height - 3, 2))
        End Select
        For tabItemIndex As Integer = 0 To TabCount - 1
            itemBounds = GetTabRect(tabItemIndex)
            If tabItemIndex = SelectedIndex Then
                Select Case Alignment
                    Case TabAlignment.Top
                        g.FillPath(New SolidBrush(Color.FromArgb(230, 230, 230)), RoundRect(New Rectangle(New Point(itemBounds.X - 2, itemBounds.Y), New Size(itemBounds.Width + 3, itemBounds.Height - 2)), 2))
                        g.DrawPath(New Pen(Color.FromArgb(100, 150, 180)), RoundRect(New Rectangle(New Point(itemBounds.X - 2, itemBounds.Y), New Size(itemBounds.Width + 2, itemBounds.Height - 2)), 2))
                        g.FillRectangle(New SolidBrush(Color.FromArgb(230, 230, 230)), New Rectangle(New Point(itemBounds.X - 1, itemBounds.Y + 1), New Size(itemBounds.Width + 1, itemBounds.Height)))
                    Case TabAlignment.Bottom
                        g.FillPath(New SolidBrush(Color.FromArgb(230, 230, 230)), RoundRect(New Rectangle(New Point(itemBounds.X - 2, itemBounds.Y), New Size(itemBounds.Width + 3, itemBounds.Height - 2)), 2))
                        g.DrawPath(New Pen(Color.FromArgb(100, 150, 180)), RoundRect(New Rectangle(New Point(itemBounds.X - 2, itemBounds.Y), New Size(itemBounds.Width + 2, itemBounds.Height - 2)), 2))
                        g.FillRectangle(New SolidBrush(Color.FromArgb(230, 230, 230)), New Rectangle(New Point(itemBounds.X - 1, itemBounds.Y - 2), New Size(itemBounds.Width + 1, itemBounds.Height)))
                    Case TabAlignment.Left
                        g.FillPath(New SolidBrush(Color.FromArgb(230, 230, 230)), RoundRect(New Rectangle(New Point(itemBounds.X - 2, itemBounds.Y), New Size(itemBounds.Width + 3, itemBounds.Height + 2)), 2))
                        g.DrawPath(New Pen(Color.FromArgb(100, 150, 180)), RoundRect(New Rectangle(New Point(itemBounds.X - 2, itemBounds.Y), New Size(itemBounds.Width + 2, itemBounds.Height + 1)), 2))
                        g.FillRectangle(New SolidBrush(Color.FromArgb(230, 230, 230)), New Rectangle(New Point(itemBounds.X + 1, itemBounds.Y + 1), New Size(itemBounds.Width + 6, itemBounds.Height)))
                    Case TabAlignment.Right
                        g.FillPath(New SolidBrush(Color.FromArgb(230, 230, 230)), RoundRect(New Rectangle(New Point(itemBounds.X - 2, itemBounds.Y), New Size(itemBounds.Width + 3, itemBounds.Height + 1)), 2))
                        g.DrawPath(New Pen(Color.FromArgb(100, 150, 180)), RoundRect(New Rectangle(New Point(itemBounds.X - 2, itemBounds.Y), New Size(itemBounds.Width + 2, itemBounds.Height + 2)), 2))
                        g.FillRectangle(New SolidBrush(Color.FromArgb(230, 230, 230)), New Rectangle(New Point(itemBounds.X - 7, itemBounds.Y + 1), New Size(itemBounds.Width + 6, itemBounds.Height + 1)))
                End Select
                Try
                    g.DrawString(TabPages(tabItemIndex).Text, New Font(Font.Name, Font.Size + 1), New SolidBrush(Color.FromArgb(0, 0, 0)), New Rectangle(GetTabRect(tabItemIndex).Location, GetTabRect(tabItemIndex).Size), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
                    TabPages(tabItemIndex).BackColor = Color.FromArgb(236, 237, 239)
                Catch : End Try
            End If
        Next
    End Sub
End Class

Public Class SimpleBlueSeperator : Inherits Control

    Enum Style
        Horizontal
        Verticle
    End Enum

    Private _alignment As Style = Style.Horizontal
    Public Property Alignment As Style
        Get
            Return _alignment
        End Get
        Set(ByVal v As Style)
            _alignment = v
        End Set
    End Property

    Private _showInfo As Boolean = False
    Public Property ShowInfo As Boolean
        Get
            Return _showInfo
        End Get
        Set(ByVal v As Boolean)
            _showInfo = v
            Invalidate()
        End Set
    End Property

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        Size = New Size(40, 40)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g As Graphics = e.Graphics
        g.SmoothingMode = SmoothingMode.HighQuality
        Select Case _alignment
            Case Style.Horizontal
                g.DrawLine(New Pen(Color.FromArgb(100, 150, 180)), New Point(0, CType((Height / 2), Integer)), New Point(Width, CType((Height / 2), Integer)))
                If ShowInfo = True Then
                    g.DrawString(Text, New Font("Tahoma", 12, FontStyle.Bold), New SolidBrush(Color.FromArgb(100, 150, 180)), New Rectangle(0, 0, Width - 1, CType((Height / 2), Integer)), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                End If
            Case Style.Verticle
                g.DrawLine(New Pen(Color.FromArgb(100, 150, 180)), New Point(CType((Width / 2), Integer), 0), New Point(CType((Width / 2), Integer), Height))
        End Select
    End Sub
End Class

Public Class SimpleBlueListBox : Inherits ListBox
    Public WithEvents lstbox As New ListBox
    Private __Items As String() = {""}

    Protected Overrides Sub OnSizeChanged(ByVal e As EventArgs)
        MyBase.OnSizeChanged(e)
        lstbox.Size = New Size(Width - 6, Height - 6)
        Invalidate()
    End Sub

    Protected Overrides Sub OnFontChanged(ByVal e As EventArgs)
        MyBase.OnFontChanged(e)
        lstbox.Font = Font
    End Sub

    Protected Overrides Sub OnGotFocus(ByVal e As EventArgs)
        MyBase.OnGotFocus(e)
        lstbox.Focus()
    End Sub

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        MyBase.OnResize(e)
        lstbox.Size = New Size(Width - 11, Height - 11)
        Invalidate()
    End Sub

    Public Overloads Property Items As String()
        Get
            Return __Items
        End Get
        Set(ByVal value As String())
            __Items = value
            lstbox.Items.Clear()
            Invalidate()
            lstbox.Items.AddRange(value)
            Invalidate()
        End Set
    End Property

    Public Overloads ReadOnly Property SelectedItem() As Object
        Get
            Return lstbox.SelectedItem
        End Get
    End Property

    Shadows Sub DrawItem(ByVal sender As Object, ByVal e As Windows.Forms.DrawItemEventArgs) Handles lstbox.DrawItem
        Try
            e.DrawBackground()
            e.Graphics.DrawString(lstbox.Items(e.Index).ToString(), New Font("Tahoma", 8, FontStyle.Regular), New SolidBrush(lstbox.ForeColor), e.Bounds, StringFormat.GenericDefault)
            e.DrawFocusRectangle()
        Catch
        End Try
    End Sub

    Sub AddItem(ByVal Item As Object)
        lstbox.Items.Add(Item)
        Invalidate()
    End Sub

    Sub AddRange(ByVal Items As Object())
        lstbox.Items.Remove("")
        lstbox.Items.AddRange(Items)
        Invalidate()
    End Sub

    Sub NewListBox()
        lstbox.Size = New Size(Width - 8, Height - 8)
        lstbox.BorderStyle = BorderStyle.None
        lstbox.DrawMode = DrawMode.OwnerDrawVariable
        lstbox.Location = New Point(3, 3)
        lstbox.ForeColor = Color.Black
        lstbox.BackColor = Color.FromArgb(210, 210, 210)
        lstbox.Items.Clear()
    End Sub

    Sub New()
        MyBase.New()
        NewListBox()
        Controls.Add(lstbox)
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        DoubleBuffered = True
        Size = New Size(150, 100)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g As Graphics = e.Graphics
        g.SmoothingMode = SmoothingMode.HighQuality
        Dim rect As New Rectangle(0, 0, Width - 1, Height - 1)
        lstbox.Size = New Size(Width - 7, Height - 7)
        g.FillPath(New SolidBrush(Color.FromArgb(210, 210, 210)), RoundRect(New Rectangle(0, 0, Width - 1, CType((Height / 2), Integer)), 2))
        g.FillPath(New SolidBrush(Color.FromArgb(210, 210, 210)), RoundRect(New Rectangle(0, CInt(Height / 2 - 3), Width - 1, CInt(Height / 2 + 2)), 2))
        g.DrawPath(New Pen(Color.FromArgb(100, 150, 180)), RoundRect(New Rectangle(0, 1, Width - 1, Height - 3), 2))
        g.DrawPath(New Pen(Color.FromArgb(210, 210, 210)), RoundRect(rect, 2))
    End Sub
End Class