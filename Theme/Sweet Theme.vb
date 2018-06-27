Option Strict On

Imports System.Drawing.Drawing2D

'Please Leave Credits in Source, Do not redistribute

'<info>
' -----------------Sweet Theme-----------------
' Creator - SaketSaket (HF)
' Version - 1.0
' Date Created - 25th February 2014
' Date Modified - 15th March 2014
'
' UID - 
' For bugs & Constructive Criticism contact me on HF
' If you like it & want to donate then pm me on HF
' -----------------Sweet Theme-----------------
'<info>

'Please Leave Credits in Source, Do not redistribute

Public Class SweetForm : Inherits ContainerControl

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
        Dock = DockStyle.Fill
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.ResizeRedraw, True)
    End Sub

    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        ParentForm.FormBorderStyle = FormBorderStyle.None
    End Sub

    Protected Overrides Sub OnInvalidated(ByVal e As InvalidateEventArgs)
        MyBase.OnInvalidated(e)
        ParentForm.FindForm.Text = Text
    End Sub

    Protected Overrides Sub OnTextChanged(ByVal e As EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub

    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        If ParentForm.FindForm.TransparencyKey = Nothing Then ParentForm.FindForm.TransparencyKey = Color.Fuchsia
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g As Graphics = e.Graphics
        g.SmoothingMode = SmoothingMode.HighQuality
        Dim cr As Rectangle = New Rectangle(0, 0, Width, Height)
        g.FillRectangle(New SolidBrush(Color.FromArgb(86, 186, 236)), cr)
        g.FillRectangle(New SolidBrush(Color.FromArgb(255, 255, 240)), New Rectangle(5, 40, Width - 10, Height - 45))
        g.DrawString(Text, New Font("Tahoma", 12, FontStyle.Bold), New SolidBrush(Color.FromArgb(255, 255, 255)), New Rectangle(0, 0, Width - 5, 45), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
        If ParentForm.FindForm.Width > 400 Then
            g.FillEllipse(New SolidBrush(Color.FromArgb(255, 255, 240)), New Rectangle(Width - 150, 5, 40, 30))
            Dim themelogo As String
            Dim x, y As Integer
            Dim ncr As Rectangle = New Rectangle(35, 5, 55, 40)
            Dim themelogofont As New Font("Wingdings", 8, FontStyle.Bold)
            Dim themelogomybrush As New LinearGradientBrush(ncr, Color.Chocolate, Color.BurlyWood, LinearGradientMode.Vertical)
            Dim themelogodrawformat As New StringFormat()
            themelogodrawformat.FormatFlags = StringFormatFlags.NoFontFallback
            themelogo = "t"
            x = 58
            y = 7
            g.DrawString(themelogo, themelogofont, themelogomybrush, x, y, themelogodrawformat)
            themelogo = "ttt"
            x = 49
            y = 16
            g.DrawString(themelogo, themelogofont, themelogomybrush, x, y, themelogodrawformat)
            themelogo = "ttttt"
            x = 40
            y = 25
            g.DrawString(themelogo, themelogofont, themelogomybrush, x, y, themelogodrawformat)
            Dim newthemelogofont As New Font("Tahoma", 12, FontStyle.Bold)
            Dim newthemelogomybrush As New SolidBrush(Color.Blue)
            themelogo = "S"
            x = Width - 138
            y = 11
            g.DrawString(themelogo, newthemelogofont, newthemelogomybrush, x, y, themelogodrawformat)
        End If
        Try
            g.DrawIcon(_icon, New Rectangle(8, 8, 28, 28))
        Catch : End Try
        g.Dispose()
    End Sub
End Class

Public Class SweetControlBox : Inherits Control

    Dim _minr, _maxr, _cr As Rectangle

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g As Graphics = e.Graphics
        _minr = New Rectangle(0, 0, 16, 16)
        _maxr = New Rectangle(16, 0, 16, 16)
        _cr = New Rectangle(32, 0, 16, 16)
        g.FillRectangle(New SolidBrush(Color.FromArgb(255, 255, 240)), _minr)
        g.FillRectangle(New SolidBrush(Color.FromArgb(255, 255, 240)), _maxr)
        g.FillRectangle(New SolidBrush(Color.FromArgb(255, 255, 240)), _cr)
        g.DrawString("0", New Font("Marlett", 11.5), New SolidBrush(Color.FromArgb(0, 0, 0)), _minr, New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
        If FindForm.WindowState = FormWindowState.Maximized Then
            g.DrawString("2", New Font("Marlett", 11.5), New SolidBrush(Color.FromArgb(0, 0, 0)), _maxr, New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
        Else
            g.DrawString("1", New Font("Marlett", 11.5), New SolidBrush(Color.FromArgb(0, 0, 0)), _maxr, New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
        End If
        g.DrawString("r", New Font("Marlett", 11.5), New SolidBrush(Color.FromArgb(0, 0, 0)), _cr, New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
        'g.Dispose()
    End Sub

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.ResizeRedraw, True)
        Width = 48
        Height = 16
        Anchor = AnchorStyles.Top Or AnchorStyles.Right
    End Sub

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        MyBase.OnResize(e)
        Size = New Size(48, 16)
    End Sub

    Protected Overrides Sub OnMouseClick(ByVal e As MouseEventArgs)
        MyBase.OnMouseClick(e)
        If _cr.Contains(e.Location) Then
            FindForm.Close()
        End If
        If _maxr.Contains(e.Location) Then
            If FindForm.WindowState = FormWindowState.Maximized Then
                FindForm.WindowState = FormWindowState.Normal
                Invalidate()
            Else
                FindForm.WindowState = FormWindowState.Maximized
                Invalidate()
            End If
        End If
        If _minr.Contains(e.Location) Then
            FindForm.WindowState = FormWindowState.Minimized
            Invalidate()
        End If
    End Sub
End Class

Public Class SweetStatusStrip : Inherits StatusStrip
    Sub New()
        BackColor = Color.FromArgb(86, 186, 236)
        ForeColor = Color.Ivory
        Font = New Font("Tahoma", 8)
        SizingGrip = False
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        SetStyle(ControlStyles.ResizeRedraw, True)
    End Sub

    Protected Sub CreateSss()
        Dim g As Graphics = CreateGraphics()
        g.DrawRectangle(New Pen(Color.FromArgb(150, 150, 150)), New Rectangle(1, 1, Width - 3, Height - 3))
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

Public Class SweetLabel : Inherits Label

    Sub New()
        BackColor = Color.Transparent
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        SetStyle(ControlStyles.ResizeRedraw, True)
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)
        Font = New Font("Tahoma", 10, FontStyle.Regular)
    End Sub

    Protected Overrides Sub OnTextChanged(e As EventArgs)
        MyBase.OnTextChanged(e) : Invalidate()
    End Sub
End Class

Public Class SweetPanel : Inherits ContainerControl

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        MyBase.OnResize(e)
        Invalidate()
    End Sub

    Sub New()
        MyBase.New()
        Size = New Size(240, 160)
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.ResizeRedraw, True)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g As Graphics = e.Graphics
        g.SmoothingMode = SmoothingMode.HighQuality
        Dim cr As Rectangle = New Rectangle(0, 0, Width, Height)
        g.FillRectangle(New SolidBrush(Color.FromArgb(86, 186, 236)), cr)
        'g.Dispose()
    End Sub
End Class

Public Class SweetGroupBox : Inherits ContainerControl

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        MyBase.OnResize(e)
        Invalidate()
    End Sub

    Sub New()
        MyBase.New()
        Size = New Size(240, 160)
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.ResizeRedraw, True)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g As Graphics = e.Graphics
        g.SmoothingMode = SmoothingMode.HighQuality
        Dim cr As Rectangle = New Rectangle(0, 0, Width, Height)
        Dim cr1 As Rectangle = New Rectangle(1, 1, Width - 2, Height - 2)
        Dim cr2 As Rectangle = New Rectangle(1, 1, Width - 2, 25)
        g.FillRectangle(New SolidBrush(Color.FromArgb(86, 186, 236)), cr)
        g.DrawRectangle(New Pen(Color.Black), cr1)
        g.FillRectangle(New SolidBrush(Color.FromArgb(255, 255, 240)), cr2)
        g.DrawRectangle(New Pen(Color.Black), cr2)
        g.DrawString(Text, New Font("Tahoma", 10, FontStyle.Bold), New SolidBrush(Color.FromArgb(128, 128, 128)), New Rectangle(0, 0, Width - 5, 30), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
        'g.Dispose()
    End Sub
End Class

Public Class SweetRadioButton : Inherits Control

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
        Size = New Size(160, 20)
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.ResizeRedraw, True)
        SetStyle(ControlStyles.Selectable, False)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g As Graphics = e.Graphics
        Dim cr As Rectangle = New Rectangle(0, 0, Width, Height)
        Dim cr1 As Rectangle = New Rectangle(2, 2, 15, 15)
        g.FillRectangle(New SolidBrush(Color.FromArgb(86, 186, 236)), cr)
        g.FillEllipse(New SolidBrush(Color.FromArgb(255, 255, 240)), cr1)
        g.DrawString(Text, New Font("Tahoma", 10, FontStyle.Bold), New SolidBrush(Color.FromArgb(255, 255, 255)), New Rectangle(10, 5, Width, 10), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
        If Checked Then
            cr1.Inflate(-2, -2)
            g.FillEllipse(New SolidBrush(Color.FromArgb(0, 0, 0)), cr1)
        Else
            g.FillEllipse(New SolidBrush(Color.FromArgb(255, 255, 240)), cr1)
        End If
    End Sub

    Protected Overrides Sub OnTextChanged(ByVal e As EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnClick(ByVal e As EventArgs)
        MyBase.OnClick(e)
        If Not Checked Then Checked = True
        For Each ctrl As Control In Parent.Controls
            If TypeOf ctrl Is SweetRadioButton Then
                If ctrl.Handle = Handle Then Continue For
                If ctrl.Enabled Then DirectCast(ctrl, SweetRadioButton).Checked = False
            End If
        Next
    End Sub
End Class

Public Class SweetCheckBox : Inherits Control

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
        Size = New Size(160, 20)
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.ResizeRedraw, True)
        SetStyle(ControlStyles.Selectable, False)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g As Graphics = e.Graphics
        Dim cr As Rectangle = New Rectangle(0, 0, Width, Height)
        Dim cr1 As Rectangle = New Rectangle(3, 3, 14, 14)
        g.FillRectangle(New SolidBrush(Color.FromArgb(86, 186, 236)), cr)
        g.FillRectangle(New SolidBrush(Color.FromArgb(255, 255, 240)), cr1)
        g.DrawString(Text, New Font("Tahoma", 10, FontStyle.Bold), New SolidBrush(Color.FromArgb(255, 255, 255)), New Rectangle(10, 5, Width, 10), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
        If Checked Then
            g.DrawString("b", New Font("Marlett", 10, FontStyle.Bold), New SolidBrush(Color.FromArgb(0, 0, 0)), cr1, New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
        End If
    End Sub

    Protected Overrides Sub OnTextChanged(ByVal e As EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnClick(ByVal e As EventArgs)
        MyBase.OnClick(e)
        If Not Checked Then Checked = True Else Checked = False
    End Sub
End Class

Public Class SweetToggle : Inherits Control

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
        Size = New Size(80, 25)
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.ResizeRedraw, True)
        SetStyle(ControlStyles.Selectable, False)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g As Graphics = e.Graphics
        Dim cr As Rectangle = New Rectangle(0, 0, Width, Height)
        g.FillRectangle(New SolidBrush(Color.FromArgb(86, 186, 236)), cr)
        If Checked Then
            g.FillRectangle(New SolidBrush(Color.FromArgb(222, 184, 135)), New Rectangle(CInt((Width / 2) - 2), 2, CInt((Width / 2)), Height - 4))
            g.DrawString("ON", New Font("Tahoma", 10, FontStyle.Bold), New SolidBrush(Color.FromArgb(255, 255, 255)), New Rectangle(0, 5, CInt((Width / 2)), 15), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
            g.DrawLine(New Pen(Color.Black), 56, 5, 56, Height - 7)
            g.DrawLine(New Pen(Color.Black), 58, 3, 58, Height - 5)
            g.DrawLine(New Pen(Color.Black), 60, 5, 60, Height - 7)
        Else
            g.FillRectangle(New SolidBrush(Color.FromArgb(222, 184, 135)), New Rectangle(2, 2, CInt((Width / 2)), Height - 4))
            g.DrawString("OFF", New Font("Tahoma", 10, FontStyle.Bold), New SolidBrush(Color.FromArgb(255, 255, 255)), New Rectangle(CInt((Width / 2)), 5, CInt((Width / 2)), 15), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
            g.DrawLine(New Pen(Color.Black), 20, 5, 20, Height - 7)
            g.DrawLine(New Pen(Color.Black), 22, 3, 22, Height - 5)
            g.DrawLine(New Pen(Color.Black), 24, 5, 24, Height - 7)
        End If
    End Sub

    Protected Overrides Sub OnTextChanged(ByVal e As EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnClick(ByVal e As EventArgs)
        MyBase.OnClick(e)
        If Not Checked Then Checked = True Else Checked = False
    End Sub
End Class

Public Class SweetButton : Inherits Control

    Enum MouseState As Byte
        None = 0
        Over = 1
        Down = 2
    End Enum

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
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.ResizeRedraw, True)
        Size = New Size(125, 30)
        _state = MouseState.None
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g As Graphics = e.Graphics
        g.SmoothingMode = SmoothingMode.HighQuality
        Dim cr As Rectangle = New Rectangle(0, 0, Width, Height)
        Dim cr1 As Rectangle = New Rectangle(2, 2, Width - 4, Height - 4)
        g.FillRectangle(New SolidBrush(Color.FromArgb(86, 186, 236)), cr)
        Dim btnfont As New Font("Tahoma", 10, FontStyle.Bold)
        Select Case _state
            Case MouseState.None
                g.DrawString(Text, btnfont, Brushes.White, New Rectangle(0, 0, Width, Height), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
            Case MouseState.Down
                g.FillRectangle(New SolidBrush(Color.FromArgb(222, 184, 135)), cr1)
                g.DrawString(Text, btnfont, Brushes.Blue, New Rectangle(0, 0, Width, Height), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
            Case MouseState.Over
                g.FillRectangle(New SolidBrush(Color.FromArgb(222, 184, 135)), cr1)
                g.DrawString(Text, btnfont, Brushes.Black, New Rectangle(0, 0, Width, Height), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
        End Select
        'g.Dispose()
    End Sub
End Class