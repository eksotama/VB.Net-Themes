Imports System.Drawing.Drawing2D
Imports System.ComponentModel

Public Class TheEmpireThemeContainer
    Inherits ContainerControl

    Sub New()
        DoubleBuffered = True
        SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw, True)
        BackColor = Color.FromArgb(50, 50, 50)
        ForeColor = Color.White
        Dock = DockStyle.Fill
    End Sub

    Dim EmpirePurple As Color = Color.FromArgb(55, 173, 242)

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        e.Graphics.Clear(Color.FromArgb(200, 200, 200))

        Dim LGB As New LinearGradientBrush(New Rectangle(0, 0, Width, 37), Color.FromArgb(36, 36, 36), Color.FromArgb(25, 25, 25), 90.0F)
        e.Graphics.FillRectangle(LGB, LGB.Rectangle)
        'LGB = New LinearGradientBrush(New Rectangle(0, 41, Width, 4), Color.FromArgb(80, Color.Black), Color.Transparent, 90.0F)
        'e.Graphics.FillRectangle(LGB, LGB.Rectangle)

        e.Graphics.FillRectangle(New SolidBrush(EmpirePurple), New Rectangle(13, 31, e.Graphics.MeasureString(Text, New Font("Segoe UI", 11)).Width + 6, 4))
        e.Graphics.FillRectangle(New SolidBrush(EmpirePurple), New Rectangle(0, 35, Width, 2))

        e.Graphics.DrawString(Text, New Font("Segoe UI", 11), Brushes.Black, New Point(15, 9))
        e.Graphics.DrawString(Text, New Font("Segoe UI", 11), Brushes.White, New Point(15, 8))

        MyBase.OnPaint(e)
    End Sub

End Class

Public Class TheEmpireStatusStrip
    Inherits ContainerControl

    Sub New()
        DoubleBuffered = True
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        Dock = DockStyle.Bottom
        Height = 27
    End Sub

    Dim EmpirePurple As Color = Color.FromArgb(55, 173, 242)

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        e.Graphics.Clear(BackColor)

        Dim LGB As New LinearGradientBrush(New Rectangle(0, 0, Width, Height), Color.FromArgb(25, 25, 25), Color.FromArgb(36, 36, 36), 90.0F)
        e.Graphics.FillRectangle(LGB, LGB.Rectangle)
        e.Graphics.FillRectangle(New SolidBrush(EmpirePurple), New Rectangle(0, Height - 2, Width, 2))

        e.Graphics.DrawString(Text, New Font("Segoe UI", 9), Brushes.White, New Point(6, 4))
        MyBase.OnPaint(e)
    End Sub

End Class

Public Class TheEmpireStatusLabel
    Inherits Label

    Sub New()
        ForeColor = Color.White
        BackColor = Color.Transparent
        Font = New Font("Segoe UI", 9)
    End Sub

End Class


Public Class TheEmpireHeaderButton
    Inherits Control

#Region "CreateRound"
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

#Region "Mouse states"
    Private State As MouseStates
    Enum MouseStates
        None = 0
        Over = 1
        Down = 2
    End Enum

    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        State = MouseStates.Over
        Invalidate()
        MyBase.OnMouseEnter(e)
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        State = MouseStates.None
        Invalidate()
        MyBase.OnMouseEnter(e)
    End Sub

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        State = MouseStates.Down
        Invalidate()
        MyBase.OnMouseDown(e)
    End Sub

    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        State = MouseStates.Over
        Invalidate()
        If e.Button = Windows.Forms.MouseButtons.Left Then MyBase.OnClick(Nothing) 'This fixes some fucked up lag you get...
        MyBase.OnMouseDown(e)
    End Sub

    Protected Overrides Sub OnClick(e As EventArgs) 'Do nothing here or it fires twice
    End Sub
#End Region

    Dim EmpirePurple As Color = Color.FromArgb(55, 173, 242)

    Sub New()
        Size = New Size(75, 37)
        Text = "Button"
        DoubleBuffered = True
        SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw, True)
        Font = New Font("Segoe UI", 9)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        e.Graphics.Clear(BackColor)

        Dim LGB As New LinearGradientBrush(New Rectangle(0, 0, Width, 37), Color.FromArgb(36, 36, 36), Color.FromArgb(25, 25, 25), 90.0F)
        e.Graphics.FillRectangle(LGB, LGB.Rectangle)
        LGB = New LinearGradientBrush(New Rectangle(0, 37, Width, 8), Color.FromArgb(80, Color.Black), Color.Transparent, 90.0F)
        e.Graphics.FillRectangle(LGB, LGB.Rectangle)
        e.Graphics.FillRectangle(New SolidBrush(EmpirePurple), New Rectangle(0, 35, Width, 2))

        LGB = New LinearGradientBrush(New Rectangle(1, 5, 1, 30), Color.FromArgb(180, EmpirePurple), Color.Transparent, -90.0F)
        e.Graphics.FillRectangle(LGB, LGB.Rectangle)
        e.Graphics.FillRectangle(LGB, New Rectangle(Width - 2, 5, 1, 30))

        LGB = New LinearGradientBrush(New Rectangle(0, 5, 1, 30), Color.FromArgb(180, Color.Black), Color.Transparent, -90.0F)
        e.Graphics.FillRectangle(LGB, LGB.Rectangle)
        e.Graphics.FillRectangle(LGB, New Rectangle(Width - 1, 5, 1, 30))

        Select Case State
            Case MouseStates.Over
                LGB = New LinearGradientBrush(New Rectangle(2, 15, Width - 5, 20), Color.Transparent, Color.FromArgb(15, Color.White), 90.0F)
                e.Graphics.FillRectangle(LGB, LGB.Rectangle)
            Case MouseStates.Down
                LGB = New LinearGradientBrush(New Rectangle(2, 13, Width - 5, 22), Color.Transparent, Color.FromArgb(7, Color.White), 90.0F)
                e.Graphics.FillRectangle(LGB, LGB.Rectangle)
        End Select

        e.Graphics.DrawString(Text, Font, Brushes.White, New Rectangle(3, 9, Width - 7, Height - 14), New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center, .Trimming = StringTrimming.EllipsisCharacter})

        MyBase.OnPaint(e)
    End Sub

End Class

Public Class TheEmpireButton
    Inherits Control

#Region "CreateRound"
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

#Region "Mouse states"
    Private State As MouseStates
    Enum MouseStates
        None = 0
        Over = 1
        Down = 2
    End Enum

    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        State = MouseStates.Over
        Invalidate()
        MyBase.OnMouseEnter(e)
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        State = MouseStates.None
        Invalidate()
        MyBase.OnMouseEnter(e)
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        State = MouseStates.Down
        Invalidate()
        MyBase.OnMouseDown(e)
    End Sub

    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        State = MouseStates.Over
        Invalidate()
        If e.Button = Windows.Forms.MouseButtons.Left Then MyBase.OnClick(Nothing) 'This fixes some fucked up lag you get...
        MyBase.OnMouseDown(e)
    End Sub

    Protected Overrides Sub OnClick(ByVal e As EventArgs) 'Do nothing here or it fires twice
    End Sub
#End Region

#Region "Properties"

    Enum ImageAlignments
        Left = 0
        Center = 1
        Right = 2
    End Enum

    Dim _ImageAlignment As ImageAlignments = ImageAlignments.Left
    Property ImageAlignment As ImageAlignments
        Get
            Return _ImageAlignment
        End Get
        Set(ByVal value As ImageAlignments)
            _ImageAlignment = value
            Invalidate()
        End Set
    End Property

    Dim _Image As Image
    Property Image As Image
        Get
            Return _Image
        End Get
        Set(ByVal value As Image)
            _Image = value
            Invalidate()
        End Set
    End Property

#End Region

    Sub New()
        Size = New Size(120, 31)
        DoubleBuffered = True
        SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        Font = New Font("Segoe UI", 9)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        e.Graphics.SmoothingMode = SmoothingMode.HighQuality

        Dim LGB As Brush

        Select Case State
            Case MouseStates.None
                LGB = New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(36, 36, 36), Color.FromArgb(25, 25, 25), 90.0F)
            Case MouseStates.Over
                LGB = New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(42, 42, 42), Color.FromArgb(25, 25, 25), 90.0F)
            Case Else
                LGB = New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(36, 36, 36), Color.FromArgb(18, 18, 18), 90.0F)
        End Select
        If Not Enabled Then
            LGB = New SolidBrush(Color.FromArgb(55, 55, 55))
        End If
        e.Graphics.FillPath(LGB, CreateRound(0, 0, Width - 1, Height - 1, 6))

        If IsNothing(_Image) Then
            e.Graphics.DrawString(Text, Font, Brushes.White, New Rectangle(3, 2, Width - 7, Height - 5), New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center, .Trimming = StringTrimming.EllipsisCharacter})
        Else
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic
            Select Case _ImageAlignment
                Case ImageAlignments.Left
                    Dim ImageRect As New Rectangle(9, 6, Height - 13, Height - 13)
                    e.Graphics.DrawImage(_Image, ImageRect)
                    e.Graphics.DrawString(Text, Font, Brushes.White, New Rectangle(ImageRect.X + ImageRect.Width + 6, 2, Width - ImageRect.Width - 22, Height - 5), New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center, .Trimming = StringTrimming.EllipsisCharacter})
                Case ImageAlignments.Center
                    Dim ImageRect As New Rectangle(((Width - 1) / 2) - (Height - 13) / 2, 6, Height - 13, Height - 13)
                    e.Graphics.DrawImage(_Image, ImageRect)
                Case ImageAlignments.Right
                    Dim ImageRect As New Rectangle(Width - Height + 3, 6, Height - 13, Height - 13)
                    e.Graphics.DrawImage(_Image, ImageRect)
                    e.Graphics.DrawString(Text, Font, Brushes.White, New Rectangle(3, 2, Width - ImageRect.Width - 22, Height - 5), New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center, .Trimming = StringTrimming.EllipsisCharacter})
            End Select
        End If

        MyBase.OnPaint(e)
    End Sub

    Public Sub PerformClick()
        MyBase.OnClick(Nothing)
    End Sub

End Class

<DefaultEvent("CheckedChanged")> _
Public Class TheEmpireCheckbox
    Inherits Control

#Region "CreateRound"
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

#Region "Mouse states"
    Private State As MouseStates
    Private X As Integer
    Enum MouseStates
        None = 0
        Over = 1
        Down = 2
    End Enum

    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        State = MouseStates.Over
        Invalidate()
        MyBase.OnMouseEnter(e)
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        State = MouseStates.None
        X = -1
        Invalidate()
        MyBase.OnMouseEnter(e)
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        State = MouseStates.Down
        Invalidate()
        MyBase.OnMouseDown(e)
    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)
        X = e.X
        Invalidate()
        MyBase.OnMouseMove(e)
    End Sub

    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        State = MouseStates.Over
        Invalidate()
        If e.Button = Windows.Forms.MouseButtons.Left Then
            MyBase.OnClick(Nothing)
            _Checked = Not _Checked
            Invalidate()
        End If

        MyBase.OnMouseDown(e)
    End Sub

    Protected Overrides Sub OnClick(ByVal e As EventArgs) 'Do nothing here or it fires twice
    End Sub
#End Region

#Region "Properties"

    Event CheckedChanged(ByVal sender As Object)

    Private _Checked As Boolean
    Property Checked As Boolean
        Get
            Return _Checked
        End Get
        Set(ByVal value As Boolean)
            _Checked = value
            RaiseEvent CheckedChanged(Me)
            Invalidate()
        End Set
    End Property

    Enum SliderLocations
        Left = 0
        Right = 1
    End Enum

    Private _SliderLocation As SliderLocations = SliderLocations.Right
    Property SliderLocation As SliderLocations
        Get
            Return _SliderLocation
        End Get
        Set(ByVal value As SliderLocations)
            _SliderLocation = value
            Invalidate()
        End Set
    End Property

#End Region

    Sub New()
        DoubleBuffered = True
        SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw Or ControlStyles.SupportsTransparentBackColor, True)
        Font = New Font("Segoe UI", 9)
        ForeColor = Color.Black
        BackColor = Color.Transparent
        Size = New Size(150, 21)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        e.Graphics.SmoothingMode = SmoothingMode.HighQuality

        Dim LGB As Brush

        If X >= 0 And X < 17 Then
            Select Case State
                Case MouseStates.None
                    LGB = New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(36, 36, 36), Color.FromArgb(25, 25, 25), 90.0F)
                Case MouseStates.Over
                    LGB = New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(48, 48, 48), Color.FromArgb(25, 25, 25), 90.0F)
                Case Else
                    LGB = New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(36, 36, 36), Color.FromArgb(10, 10, 10), 90.0F)
            End Select
        Else
            LGB = New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(36, 36, 36), Color.FromArgb(25, 25, 25), 90.0F)
        End If
        e.Graphics.FillPath(LGB, CreateRound(1, 2, 15, 15, 7))

        If _Checked Then
            e.Graphics.DrawString("a", New Font("Marlett", 13), Brushes.White, New Point(-2, 1))
        End If

        e.Graphics.DrawString(Text, Font, New SolidBrush(ForeColor), New Rectangle(20, 0, Width - 21, Height - 1), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center, .Trimming = StringTrimming.EllipsisCharacter})
        MyBase.OnPaint(e)
    End Sub
End Class

<DefaultEvent("CheckedChanged")> _
Public Class TheEmpireRadioButton
    Inherits Control

#Region "CreateRound"
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

#Region "Properties"

    Event CheckedChanged(ByVal sender As Object)

    Private _Checked As Boolean
    Property Checked As Boolean
        Get
            Return _Checked
        End Get
        Set(ByVal value As Boolean)
            _Checked = value
            RaiseEvent CheckedChanged(Me)
            Invalidate()
        End Set
    End Property

#End Region

#Region "Mouse states"
    Private State As MouseStates
    Private X As Integer
    Enum MouseStates
        None = 0
        Over = 1
        Down = 2
    End Enum

    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        State = MouseStates.Over
        Invalidate()
        MyBase.OnMouseEnter(e)
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        State = MouseStates.None
        X = -1
        Invalidate()
        MyBase.OnMouseEnter(e)
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        State = MouseStates.Down
        Invalidate()
        MyBase.OnMouseDown(e)
    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)
        X = e.X
        Invalidate()
        MyBase.OnMouseMove(e)
    End Sub

    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        State = MouseStates.Over
        Invalidate()
        For Each C As Control In Parent.Controls
            If TypeOf (C) Is TheEmpireRadioButton Then
                DirectCast(C, TheEmpireRadioButton).Checked = False
            End If
        Next
        _Checked = True
        MyBase.OnMouseDown(e)
    End Sub

    Protected Overrides Sub OnClick(ByVal e As EventArgs) 'Do nothing here or the Click event fires twice
    End Sub
#End Region

    Sub New()
        DoubleBuffered = True
        SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw Or ControlStyles.SupportsTransparentBackColor, True)
        Font = New Font("Segoe UI", 9)
        ForeColor = Color.Black
        BackColor = Color.Transparent
        Size = New Size(150, 21)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        e.Graphics.SmoothingMode = SmoothingMode.HighQuality

        Dim LGB As LinearGradientBrush

        If X >= 0 And X < 17 Then
            Select Case State
                Case MouseStates.None
                    LGB = New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(36, 36, 36), Color.FromArgb(25, 25, 25), 90.0F)
                Case MouseStates.Over
                    LGB = New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(48, 48, 48), Color.FromArgb(25, 25, 25), 90.0F)
                Case Else
                    LGB = New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(36, 36, 36), Color.FromArgb(10, 10, 10), 90.0F)
            End Select
        Else
            LGB = New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(36, 36, 36), Color.FromArgb(25, 25, 25), 90.0F)
        End If
        e.Graphics.FillEllipse(LGB, New Rectangle(1, 2, 15, 15))

        If _Checked Then
            LGB = New LinearGradientBrush(New Rectangle(5, 6, 7, 7), Color.White, Color.Gainsboro, 90.0F)
            e.Graphics.FillEllipse(LGB, LGB.Rectangle)
        End If

        e.Graphics.DrawString(Text, Font, New SolidBrush(ForeColor), New Rectangle(20, 0, Width - 21, Height - 1), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center, .Trimming = StringTrimming.EllipsisCharacter})
        MyBase.OnPaint(e)
    End Sub

End Class

Public Class TheEmpireGroupBox
    Inherits ContainerControl

    Sub New()
        Font = New Font("Segoe UI", 9)
        ForeColor = Color.Black
        DoubleBuffered = True
        SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
    End Sub

    Dim EmpirePurple As Color = Color.FromArgb(55, 173, 242)

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        e.Graphics.FillRectangle(New SolidBrush(EmpirePurple), New Rectangle(13, 19, e.Graphics.MeasureString(Text, New Font("Segoe UI", 10)).Width + 2, 4))
        e.Graphics.FillRectangle(New SolidBrush(EmpirePurple), New Rectangle(0, 23, Width, 2))

        e.Graphics.DrawString(Text, New Font("Segoe UI", 10), Brushes.Black, New Point(16, 0))

        MyBase.OnPaint(e)
    End Sub

End Class

Public Class TheEmpireDropdownButton
    Inherits ComboBox

#Region "CreateRound"
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

#Region "Mouse states"
    Private State As MouseStates
    Enum MouseStates
        None = 0
        Over = 1
        Down = 2
    End Enum

    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        State = MouseStates.Over
        Invalidate()
        MyBase.OnMouseEnter(e)
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        State = MouseStates.None
        Invalidate()
        MyBase.OnMouseEnter(e)
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        State = MouseStates.Down
        Invalidate()
        MyBase.OnMouseDown(e)
    End Sub

    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        State = MouseStates.Over
        Invalidate()
        MyBase.OnMouseDown(e)
    End Sub

    Public Shadows Event Click(ByVal Sender As Object, ByVal ItemIndex As Integer)
#End Region

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.DoubleBuffer, True)
        DoubleBuffered = True
        ForeColor = Color.White
        DrawMode = Windows.Forms.DrawMode.OwnerDrawFixed
        ItemHeight = 23
        Size = New Size(144, 30)
        Font = New Font("Segoe UI", 9)
        ForeColor = Color.White
        BackColor = Color.FromArgb(200, 200, 200)
        DropDownStyle = ComboBoxStyle.DropDownList
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        e.Graphics.Clear(BackColor)
        e.Graphics.TextContrast = 0.1
        e.Graphics.SmoothingMode = SmoothingMode.HighQuality

        Dim LGB As Brush
        Select Case State
            Case MouseStates.None
                LGB = New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(36, 36, 36), Color.FromArgb(25, 25, 25), 90.0F)
            Case MouseStates.Over
                LGB = New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(42, 42, 42), Color.FromArgb(25, 25, 25), 90.0F)
            Case Else
                LGB = New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(36, 36, 36), Color.FromArgb(18, 18, 18), 90.0F)
        End Select
        If Not Enabled Then
            LGB = New SolidBrush(Color.FromArgb(55, 55, 55))
        End If

        e.Graphics.FillPath(LGB, CreateRound(0, 0, Width - 1, Height - 1, 6))

        Dim TextToDraw As String = SelectedItem
        If String.IsNullOrEmpty(TextToDraw) Then TextToDraw = "..."
        e.Graphics.DrawString(TextToDraw, Font, New SolidBrush(ForeColor), New Rectangle(0, 0, Width - 10, Height), New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center, .Trimming = StringTrimming.EllipsisCharacter})

        Dim P() As Point = {New Point(Width - 18, 12), New Point(Width - 10, 12), New Point(Width - 14, 17)}
        e.Graphics.FillPolygon(Brushes.White, P)
    End Sub

    Protected Overrides Sub OnDrawItem(ByVal e As DrawItemEventArgs)
        If e.Index < 0 Then Exit Sub
        Dim rect As New Rectangle()
        rect.X = e.Bounds.X
        rect.Y = e.Bounds.Y
        rect.Width = e.Bounds.Width - 1
        rect.Height = e.Bounds.Height - 1

        e.DrawBackground()
        If e.State > 0 Then
            e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(70, 70, 70)), e.Bounds)
            e.Graphics.DrawString(Me.Items(e.Index).ToString(), New Font(Font.FontFamily, 8), Brushes.White, e.Bounds, New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center, .Trimming = StringTrimming.EllipsisCharacter})
        Else
            e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(36, 36, 36)), e.Bounds)
            e.Graphics.DrawString(Me.Items(e.Index).ToString(), New Font(Font.FontFamily, 8), Brushes.White, e.Bounds, New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center, .Trimming = StringTrimming.EllipsisCharacter})
        End If
        e.Graphics.DrawLine(New Pen(Color.FromArgb(55, Color.Black)), New Point(e.Bounds.X, e.Bounds.Y + e.Bounds.Height - 1), New Point(e.Bounds.X + e.Bounds.Width, e.Bounds.Y + e.Bounds.Height - 1))
        MyBase.OnDrawItem(e)
    End Sub

    Protected Overrides Sub OnTextChanged(ByVal e As EventArgs)
        Invalidate()
        MyBase.OnTextChanged(e)
    End Sub

End Class

Public Class TheEmpireListbox
    Inherits ListBox

#Region "CreateRound"

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

    Sub New()
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        DoubleBuffered = True
        BorderStyle = Windows.Forms.BorderStyle.None
        DrawMode = Windows.Forms.DrawMode.OwnerDrawFixed
        ItemHeight = 24
        ForeColor = Color.Black
        BackColor = Color.FromArgb(200, 200, 200)
        IntegralHeight = False
    End Sub

    Protected Overrides Sub WndProc(ByRef m As Message)
        If m.Msg = 15 Then
            Dim G As Graphics = CreateGraphics()
            G.SmoothingMode = SmoothingMode.AntiAlias
            G.DrawPath(New Pen(Color.FromArgb(100, 100, 100)), CreateRound(0, 0, Width - 1, Height - 1, 7))
        End If
        MyBase.WndProc(m)
    End Sub

    Protected Overrides Sub OnDrawItem(ByVal e As System.Windows.Forms.DrawItemEventArgs)
        CreateGraphics.DrawPath(New Pen(Color.FromArgb(100, 100, 100)), CreateRound(0, 0, Width - 1, Height - 1, 7))

        If e.Index < 0 Or Items.Count < 1 Then Exit Sub
        Dim ItemRectangle As Rectangle = New Rectangle(e.Bounds.X + 3, e.Bounds.Y + 1, e.Bounds.Width - 7, e.Bounds.Height - 2)


        e.Graphics.FillRectangle(New SolidBrush(BackColor), ItemRectangle)
        e.Graphics.FillRectangle(New SolidBrush(BackColor), e.Bounds)
        If e.State > 0 Then
            Dim LGB As New LinearGradientBrush(ItemRectangle, Color.FromArgb(36, 36, 36), Color.FromArgb(25, 25, 25), 90.0F)
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality
            e.Graphics.FillPath(LGB, CreateRound(ItemRectangle, 4))
            e.Graphics.SmoothingMode = SmoothingMode.None

            e.Graphics.DrawString(Items(e.Index).ToString(), Font, Brushes.White, 7, e.Bounds.Y + 4)
        Else
            e.Graphics.FillRectangle(New SolidBrush(BackColor), e.Bounds)
            e.Graphics.DrawString(Items(e.Index).ToString(), Font, Brushes.Black, 7, e.Bounds.Y + 4)
        End If

        MyBase.OnDrawItem(e)

        CreateGraphics.DrawPath(New Pen(Color.FromArgb(100, 100, 100)), CreateRound(0, 0, Width - 1, Height - 1, 7))
    End Sub
End Class

<DefaultEvent("TextChanged")> _
Public Class TheEmpireTextBox
    Inherits Control

#Region "CreateRound"
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

#Region "Properties"
    Private _TextAlign As HorizontalAlignment = HorizontalAlignment.Left
    Property TextAlign() As HorizontalAlignment
        Get
            Return _TextAlign
        End Get
        Set(ByVal value As HorizontalAlignment)
            _TextAlign = value
            If Base IsNot Nothing Then
                Base.TextAlign = value
            End If
        End Set
    End Property
    Private _MaxLength As Integer = 32767
    Property MaxLength() As Integer
        Get
            Return _MaxLength
        End Get
        Set(ByVal value As Integer)
            _MaxLength = value
            If Base IsNot Nothing Then
                Base.MaxLength = value
            End If
        End Set
    End Property
    Private _ReadOnly As Boolean
    Property [ReadOnly]() As Boolean
        Get
            Return _ReadOnly
        End Get
        Set(ByVal value As Boolean)
            _ReadOnly = value
            If Base IsNot Nothing Then
                Base.ReadOnly = value
            End If
        End Set
    End Property
    Private _UseSystemPasswordChar As Boolean
    Property UseSystemPasswordChar() As Boolean
        Get
            Return _UseSystemPasswordChar
        End Get
        Set(ByVal value As Boolean)
            _UseSystemPasswordChar = value
            If Base IsNot Nothing Then
                Base.UseSystemPasswordChar = value
            End If
        End Set
    End Property
    Private _Multiline As Boolean
    Property Multiline() As Boolean
        Get
            Return _Multiline
        End Get
        Set(ByVal value As Boolean)
            _Multiline = value
            If Base IsNot Nothing Then
                Base.Multiline = value

                If value Then
                    Base.Height = Height - 7
                    Base.Location = New Point(3, 3)
                Else
                    Height = Base.Height + 7
                    Base.Location = New Point(6, 3)
                End If
            End If
        End Set
    End Property
    Overrides Property Text As String
        Get
            Return MyBase.Text
        End Get
        Set(ByVal value As String)
            MyBase.Text = value
            If Base IsNot Nothing Then
                Base.Text = value
            End If
        End Set
    End Property
    Overrides Property Font As Font
        Get
            Return MyBase.Font
        End Get
        Set(ByVal value As Font)
            MyBase.Font = value
            If Base IsNot Nothing Then
                Base.Font = value
                Base.Location = New Point(6, 3)
                Base.Width = Width - 12

                If Not _Multiline Then
                    Height = Base.Height + 7
                End If
            End If
        End Set
    End Property

    Property SelectionStart As Integer
        Get
            Return Base.SelectionStart
        End Get
        Set(ByVal value As Integer)
            Base.SelectionStart = value
            Invalidate()
        End Set
    End Property

    Property SelectionLength As Integer
        Get
            Return Base.SelectionLength
        End Get
        Set(ByVal value As Integer)
            Base.SelectionLength = value
            Invalidate()
        End Set
    End Property

    ReadOnly Property TextLength As Integer
        Get
            Return Base.TextLength
        End Get
    End Property

    Public Sub ScrollToCaret()
        Base.ScrollToCaret()
    End Sub

    Public Sub Clear()
        Base.Text = String.Empty
    End Sub

#End Region

#Region "Mouse events"

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        Invalidate()
        MyBase.OnMouseDown(e)
    End Sub

    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        Invalidate()
        MyBase.OnMouseDown(e)
    End Sub

#End Region

    Protected Overrides Sub InitLayout()
        If Not Controls.Contains(Base) Then
            Controls.Add(Base)
        End If
        MyBase.InitLayout()
    End Sub

    Private Base As TextBox
    Sub New()
        DoubleBuffered = True
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        Font = New Font("Segoe UI", 9)

        Base = New TextBox

        Base.Font = Font
        Base.Text = Text
        Base.ForeColor = Color.Black
        Base.BackColor = Color.LightGray
        Base.MaxLength = _MaxLength
        Base.Multiline = _Multiline
        Base.ReadOnly = _ReadOnly
        Base.UseSystemPasswordChar = _UseSystemPasswordChar
        Base.BorderStyle = BorderStyle.None
        Base.Location = New Point(6, 3)
        Base.Width = Width - 12

        If _Multiline Then
            Base.Height = Height - 7
        Else
            Height = Base.Height + 7
        End If

        AddHandler Base.TextChanged, AddressOf OnBaseTextChanged
        AddHandler Base.KeyDown, AddressOf OnBaseKeyDown
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        e.Graphics.Clear(BackColor)
        e.Graphics.SmoothingMode = SmoothingMode.HighQuality
        e.Graphics.FillPath(New SolidBrush(Color.LightGray), CreateRound(0, 0, Width - 1, Height - 1, 6))
        e.Graphics.DrawPath(New Pen(Color.FromArgb(100, 100, 100)), CreateRound(0, 0, Width - 1, Height - 1, 6))

        MyBase.OnPaint(e)
    End Sub

    Private Sub OnBaseTextChanged(ByVal s As Object, ByVal e As EventArgs)
        Text = Base.Text
        Base.BringToFront()
        If Text.Length = 0 Then
            Base.Focus()
            Base.SelectionStart = 0
            Base.SelectionLength = 0
            Base.Select()
        End If
    End Sub

    Private Sub OnBaseKeyDown(ByVal s As Object, ByVal e As KeyEventArgs)
        If e.Control AndAlso e.KeyCode = Keys.A Then
            Base.SelectAll()
            e.SuppressKeyPress = True
        End If
    End Sub

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        Base.Location = New Point(6, 3)
        Base.Width = Width - 12

        If _Multiline Then
            Base.Height = Height - 7
            Base.Location = New Point(3, 3)
        Else
            Base.Location = New Point(6, 3)
        End If
        MyBase.OnResize(e)
    End Sub

End Class

Public Class TheEmpireTabcontrol
    Inherits TabControl

#Region "Declarations, functions"
    Dim _IndexOver As Integer = -1
    Dim X, Y As Integer
    Dim EmpirePurple As Color = Color.FromArgb(55, 173, 242)
#End Region

#Region "Initialization"
    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.DoubleBuffer, True)
        DoubleBuffered = True
        SizeMode = TabSizeMode.Fixed
        ItemSize = New Size(37, 120)
    End Sub

    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        Alignment = TabAlignment.Left
    End Sub
#End Region

#Region "Mouse Events"

    Dim _OldIndexOver As Integer = 0
    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        X = e.Location.X
        Y = e.Location.Y
        If e.X > ItemSize.Height Then
            _IndexOver = -1
        Else
            Y = (Y - (Y Mod ItemSize.Width)) / ItemSize.Width
            _IndexOver = Y
        End If

        If _IndexOver <> _OldIndexOver Then
            Invalidate()
        End If

        _OldIndexOver = _IndexOver
        MyBase.OnMouseMove(e)
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        _IndexOver = -1
        Invalidate()
        MyBase.OnMouseLeave(e)
    End Sub
#End Region

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        e.Graphics.Clear(Color.FromArgb(36, 36, 36))
        e.Graphics.FillRectangle(New LinearGradientBrush(New Rectangle(0, 0, Width, Height), Color.FromArgb(42, 42, 42), Color.FromArgb(25, 25, 25), 90.0F), New Rectangle(0, 0, Width, Height))

        e.Graphics.FillRectangle(Brushes.Gainsboro, New Rectangle(ItemSize.Height, 0, Width - ItemSize.Height, Height))
        Dim LinearGB As New LinearGradientBrush(New Rectangle(ItemSize.Height, 0, Width - ItemSize.Height, 4), Color.FromArgb(90, Color.Black), Color.Transparent, 90.0F)
        e.Graphics.FillRectangle(LinearGB, LinearGB.Rectangle)
        e.Graphics.DrawLine(Pens.Black, New Point(ItemSize.Height, 0), New Point(ItemSize.Height, Height))

        Try : For Each T As TabPage In TabPages
                T.BackColor = Color.FromArgb(200, 200, 200)
            Next : Catch : End Try


        For i = 0 To TabCount - 1
            Dim x2 As Rectangle = New Rectangle(New Point(GetTabRect(i).Location.X - 2, GetTabRect(i).Location.Y - 2), New Size(GetTabRect(i).Width, GetTabRect(i).Height))
            Dim textrectangle As New Rectangle(x2.Location.X + 34, x2.Location.Y, x2.Width - 34, x2.Height)

            If i = SelectedIndex Then
                Dim LGB As New LinearGradientBrush(x2, Color.FromArgb(36, 36, 36), Color.FromArgb(25, 25, 25), 90.0F)
                e.Graphics.FillRectangle(LGB, LGB.Rectangle)
                'e.Graphics.FillRectangle(New SolidBrush(EmpirePurple), New Rectangle(x2.Location, New Size(6, x2.Height)))

                e.Graphics.DrawRectangle(New Pen(Color.FromArgb(51, 51, 51)), x2)
                e.Graphics.DrawLine(New Pen(Color.FromArgb(17, 17, 17)), New Point(x2.Location.X + 1, x2.Location.Y + x2.Height - 1), New Point(x2.Location.X + x2.Width, x2.Location.Y + x2.Height - 1))

                e.Graphics.DrawString(TabPages(i).Text, Font, Brushes.Gainsboro, textrectangle, New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Near})
            Else
                e.Graphics.DrawString(TabPages(i).Text, Font, Brushes.Gray, textrectangle, New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Near})
                e.Graphics.DrawRectangle(New Pen(Color.FromArgb(51, 51, 51)), x2)
                e.Graphics.DrawLine(New Pen(Color.FromArgb(17, 17, 17)), New Point(x2.Location.X + 1, x2.Location.Y + x2.Height - 1), New Point(x2.Location.X + x2.Width, x2.Location.Y + x2.Height - 1))
            End If

            If i = _IndexOver Then e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(3, Color.White)), x2)
        Next
    End Sub


End Class