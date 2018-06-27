Imports System.ComponentModel, System.Drawing.Drawing2D, System.Windows.Forms

'----------------------------
'The Blue and White GUI Theme
'Creator: FuckFace
'Version: 1.0
'Created: 14/04/2014
'Changed: 26/04/2014
'----------------------------
Class BaWGUIButton : Inherits Control
#Region " Declarations "
    Private State As Integer
    Private Shape As GraphicsPath
    Private InactiveGB, ActiveGB, ActiveContourGB, PressedGB, PressedContourGB As LinearGradientBrush
    Private R1, R2 As Rectangle
    Private P1, P2, P3, P4 As Pen
    Private B1, B2, B3 As SolidBrush
    Private CSF As StringFormat = New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
#End Region

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or _
                ControlStyles.ResizeRedraw Or ControlStyles.SupportsTransparentBackColor Or ControlStyles.UserPaint, True)

        BackColor = Color.Transparent
        DoubleBuffered = True
        Font = New Font("Arial", 12)
        Size = New Size(130, 45)

        P1 = New Pen(Color.FromArgb(185, 185, 185))
        ' P2 is in the OnResize event.
        ' P3 is in the OnResize event.
        P4 = New Pen(Color.FromArgb(135, 182, 233))
        B1 = New SolidBrush(Color.FromArgb(114, 114, 114))
        B2 = New SolidBrush(Color.FromArgb(246, 247, 250))
        B3 = New SolidBrush(Color.FromArgb(25, 55, 82))
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        State = 2
        Invalidate()

        MyBase.OnMouseDown(e)
    End Sub

    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        State = 1
        Invalidate()

        MyBase.OnMouseEnter(e)
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        State = 0
        Invalidate()

        MyBase.OnMouseLeave(e)
    End Sub

    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        State = 1
        Invalidate()

        MyBase.OnMouseUp(e)
    End Sub

    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        Invalidate() : MyBase.OnTextChanged(e)
    End Sub

    Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
        If Width > 0 AndAlso Height > 0 Then
            Shape = New GraphicsPath

            R1 = New Rectangle(0, 0, Width, Height)
            R2 = New Rectangle(0, 1, Width, Height)

            InactiveGB = New LinearGradientBrush(New Rectangle(0, 0, Width, Height), Color.FromArgb(249, 249, 249) _
                                                 , Color.FromArgb(222, 222, 222), 90)
            ActiveGB = New LinearGradientBrush(New Rectangle(0, 0, Width, Height), Color.FromArgb(171, 210, 244) _
                                               , Color.FromArgb(84, 153, 228), 90)
            ActiveContourGB = New LinearGradientBrush(New Rectangle(0, 0, Width, Height), Color.FromArgb(121, 180, 235) _
                                                      , Color.FromArgb(70, 137, 201), 90)
            PressedGB = New LinearGradientBrush(New Rectangle(0, 1, Width, Height), Color.FromArgb(97, 162, 228) _
                                                , Color.FromArgb(114, 173, 233), 90)
            PressedContourGB = New LinearGradientBrush(New Rectangle(0, 0, Width, Height), Color.FromArgb(74, 141, 208) _
                                                       , Color.FromArgb(114, 173, 230), 90)

            P2 = New Pen(ActiveContourGB)
            P3 = New Pen(PressedContourGB)

            With Shape
                .AddArc(0, 0, 10, 10, 180, 90)
                .AddArc(Width - 11, 0, 10, 10, -90, 90)
                .AddArc(Width - 11, Height - 11, 10, 10, 0, 90)
                .AddArc(0, Height - 11, 10, 10, 90, 90)
                .CloseAllFigures()
            End With
        End If

        Invalidate()
        MyBase.OnResize(e)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        With e.Graphics
            .SmoothingMode = SmoothingMode.HighQuality

            Select Case State
                Case 0 'Inactive
                    .FillPath(InactiveGB, Shape)
                    .DrawPath(P1, Shape)
                    .DrawString(Text, Font, B1, R1, CSF)
                Case 1 'Active
                    .FillPath(ActiveGB, Shape)
                    .DrawPath(P2, Shape)
                    .DrawString(Text, Font, Brushes.DarkSlateGray, R2, CSF)
                    .DrawString(Text, Font, B2, R1, CSF)
                Case 2 'Pressed
                    .FillPath(PressedGB, Shape)
                    .DrawPath(P3, Shape)
                    .DrawLine(P4, 1, 1, Width - 2, 1)
                    .DrawString(Text, Font, Brushes.White, R2, CSF)
                    .DrawString(Text, Font, B3, R1, CSF)
            End Select
        End With

        MyBase.OnPaint(e)
    End Sub
End Class

<DefaultEvent("CheckedChanged")> Public Class BaWGUICheckBox : Inherits Control
#Region " Declarations "
    Private MFont As New Font("Marlett", 16)
    Private Shape As GraphicsPath
    Private GB As LinearGradientBrush
    Private P1 As Pen
    Private R1, R2 As Rectangle
    Private B1, B2 As SolidBrush
    Private CSF As StringFormat = New StringFormat() With {.LineAlignment = StringAlignment.Center}

    Event CheckedChanged(ByVal sender As Object)

    Private _Checked As Boolean
#End Region

#Region " Properties "
    <Category("Appearance")> _
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

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or _
        ControlStyles.ResizeRedraw Or ControlStyles.SupportsTransparentBackColor Or ControlStyles.UserPaint, True)

        BackColor = Color.Transparent
        DoubleBuffered = True
        Font = New Font("Arial", 12)
        Size = New Size(185, 26)

        P1 = New Pen(Color.FromArgb(160, 160, 160))
        B1 = New SolidBrush(Color.FromArgb(114, 114, 114))
        B2 = New SolidBrush(Color.FromArgb(110, 175, 235))
    End Sub

    Protected Overrides Sub OnClick(ByVal e As EventArgs)
        _Checked = Not _Checked
        RaiseEvent CheckedChanged(Me)
        Invalidate()

        MyBase.OnClick(e)
    End Sub

    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        Invalidate() : MyBase.OnTextChanged(e)
    End Sub

    Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
        If Width > 0 AndAlso Height > 0 Then
            Shape = New GraphicsPath

            R1 = New Rectangle(30, 0, Width, Height)
            R2 = New Rectangle(0, 0, Width, Height)

            GB = New LinearGradientBrush(New Rectangle(0, 0, 25, 25), Color.FromArgb(244, 245, 244) _
                                         , Color.FromArgb(227, 227, 227), 90)


            With Shape
                .AddArc(0, 0, 10, 10, 180, 90)
                .AddArc(14, 0, 10, 10, -90, 90)
                .AddArc(14, 14, 10, 10, 0, 90)
                .AddArc(0, 14, 10, 10, 90, 90)
                .CloseAllFigures()
            End With

            Height = 26
        End If

        Invalidate()
        MyBase.OnResize(e)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        With e.Graphics
            .SmoothingMode = SmoothingMode.HighQuality

            .FillPath(GB, Shape)
            .DrawPath(P1, Shape)

            .DrawString(Text, Font, B1, R1, CSF)

            If Checked Then
                .DrawString("a", MFont, B2, R2, CSF)
            End If
        End With

        MyBase.OnPaint(e)
    End Sub
End Class

Public Class BaWGUIComboBox : Inherits ComboBox
#Region " Declarations "
    Private Side, Button, Arrow1, Arrow2 As GraphicsPath
    Private SideGB, ButtonOGB, ButtonGB As LinearGradientBrush
    Private R1 As Rectangle
    Private P1, P2, P3, P4 As Pen
    Private B1, B2, B3 As SolidBrush
    Private CSF As StringFormat = New StringFormat() With {.LineAlignment = StringAlignment.Center}

    Private _StartIndex As Integer = 0
#End Region

#Region " Properties "
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
#End Region

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or _
        ControlStyles.ResizeRedraw Or ControlStyles.SupportsTransparentBackColor Or ControlStyles.UserPaint, True)

        BackColor = Color.Transparent
        DoubleBuffered = True
        DrawMode = DrawMode.OwnerDrawFixed
        DropDownStyle = ComboBoxStyle.DropDownList
        Font = New Font("Arial", 12)
        ForeColor = Color.Black
        ItemHeight = 39
        StartIndex = 0

        P1 = New Pen(Color.FromArgb(180, 180, 180))
        P3 = New Pen(Color.FromArgb(110, 170, 230))
        P4 = New Pen(Color.FromArgb(50, 130, 215))
        B1 = New SolidBrush(Color.FromArgb(100, 130, 160))
        B2 = New SolidBrush(Color.FromArgb(114, 114, 114))
        B3 = New SolidBrush(Color.FromArgb(246, 247, 250))
    End Sub

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        If Width > 0 AndAlso Height > 0 Then
            Side = New GraphicsPath
            Button = New GraphicsPath
            Arrow1 = New GraphicsPath
            Arrow2 = New GraphicsPath

            ButtonOGB = New LinearGradientBrush(New Rectangle(Width - 51, 0, Width, Height), Color.FromArgb(125, 180, 235) _
                                               , Color.FromArgb(45, 125, 200), 90)
            ButtonGB = New LinearGradientBrush(New Rectangle(Width - 50, 0, Width, Height), Color.FromArgb(175, 215, 240) _
                                               , Color.FromArgb(70, 145, 215), 90)
            SideGB = New LinearGradientBrush(New Rectangle(0, 0, Width - 50, Height), Color.FromArgb(253, 253, 253) _
                                             , Color.FromArgb(223, 223, 223), 90)

            R1 = New Rectangle(12, 0, Width, Height)

            P2 = New Pen(ButtonOGB)


            With Side
                .AddArc(0, 0, 10, 10, 180, 90)
                .AddLine(Width - 50, 0, Width - 50, Height - 1)
                .AddArc(0, Height - 11, 10, 10, 90, 90)
                .CloseFigure()
            End With
            With Button
                .AddLine(Width - 51, Height - 1, Width - 51, 0)
                .AddArc(Width - 11, 0, 10, 10, -90, 90)
                .AddArc(Width - 11, Height - 11, 10, 10, 0, 90)
                .CloseFigure()
            End With
            With Arrow1
                .AddLine(Width - 35, 21, Width - 19, 21)
                .AddLine(Width - 19, 21, Width - 27, 11)
                .CloseFigure()
            End With
            With Arrow2
                .AddLine(Width - 35, 26, Width - 19, 26)
                .AddLine(Width - 19, 26, Width - 27, 36)
                .CloseFigure()
            End With
        End If

        Invalidate()
        MyBase.OnResize(e)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        With e.Graphics
            .SmoothingMode = SmoothingMode.HighQuality

            .FillPath(SideGB, Side)
            .DrawPath(P1, Side)

            .FillPath(ButtonGB, Button)
            .DrawPath(P2, Button)

            .FillPath(B1, Arrow1)
            .FillPath(B1, Arrow2)

            .DrawString(Text, Font, B2, R1, CSF)
        End With

        MyBase.OnPaint(e)
    End Sub

    Private Sub DrawItm(ByVal sender As Object, ByVal e As DrawItemEventArgs) Handles Me.DrawItem
        If e.Index < 0 Then Exit Sub

        e.DrawBackground()

        Dim NormalGB As LinearGradientBrush = New LinearGradientBrush(e.Bounds, Color.FromArgb(251, 251, 251) _
                                                                      , Color.FromArgb(237, 237, 237), 90)
        Dim OverGB As LinearGradientBrush = New LinearGradientBrush(e.Bounds, Color.FromArgb(155, 200, 240) _
                                                                      , Color.FromArgb(75, 144, 225), 90)


        With e.Graphics
            .SmoothingMode = SmoothingMode.HighQuality

            If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
                .FillRectangle(OverGB, e.Bounds)
                .DrawLine(P3, e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Y)
                .DrawLine(P4, e.Bounds.X, e.Bounds.Y + 38, e.Bounds.Width - 1, e.Bounds.Y + 38)

                .DrawString(Text, Font, Brushes.DarkSlateGray _
                            , New Rectangle(e.Bounds.X + 12, e.Bounds.Y + 1, e.Bounds.Width, e.Bounds.Height), CSF)
                .DrawString(Text, Font, B3, New Rectangle(e.Bounds.X + 12, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height), CSF)
            Else
                .FillRectangle(NormalGB, e.Bounds)
                .DrawLine(P1, e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Y)

                .DrawString(MyBase.GetItemText(MyBase.Items(e.Index)), Font, B2 _
                            , New Rectangle(e.Bounds.X + 12, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height), CSF)
            End If
        End With
    End Sub
End Class

Public Class BaWGUIProgressBar : Inherits Control
#Region " Declarations "
    Private OPath, IPath, Indicator As GraphicsPath
    Private FillValue As Integer
    Private OGB, IGB, IOGB, IndicatorGB As LinearGradientBrush
    Private P1, P2, P3 As Pen
    Private B1 As SolidBrush
    Private TB As New TextureBrush(Image.FromStream(New System.IO.MemoryStream(Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAYAAACNiR0NAAACUElEQVQYGa3BwY1dVRBF0X2q6mExtWDgAQEQIDmRDzn0wBISYtL9771Vx/81CBNAr6Xffv9jbEAC8WRukrh5jBBgbiED5mYbSZjAFrf6/PlnelrDPyRhm5swIQMmUoREBk/GNjdJgBiER9QvX35in8E2Bsx30lDRRIiqoFJkBhFgG9uAwMEguqG+/vkX3WZsbgYskEQGXAlVyVVBVZApxCDANm2YgbPNOkN9/fuBARMYc5NERHBl8qmSa4pyED1IwzsZD3QPezVrN2dD8MFqzYVsJJ4aMaChqvgUw4/xRqoIJ27hEIOw4fSwz7B2s07TberNCTPIjQJSSWXRmXQFO0wTyMkgGDEjdg/7HHbD2uYcmDF1zmA30ASgK1AlUQWZWMHhyWIQbnHarDZ7iXXE2qJHYFGeg+YA5gpxKbhkYoaQcAxjwMKIGbHW4W1t9m5OmxnAxojgg1WvB4GpK0iJSpGCwPQsZgYIPGIs9mleXw9vj81pA8IGI5CoZbgqqUx0JY6iBWNjGww2jM3eh7Wa18dm78YWtgAB5laugAy4iongYGyBjQfaQfewT/NYzVrN3mYc2DwFQtwkU0FTAakBNzR0G9kMwZ7g7Obx2KxHs1bTBpRgnpqbAiJE8MHqh7hIBTHAiOHJ4BmOmz2wVvN4O+zddPMkYLB5FyEyxBWi3laxG0oCjLiJGTg9PPZh78M+5rTBAoTNO8nIphS0Tb28vBASGAKDzM0ztGEkZoYZY/Mv8Z25RYgMqF+/FE8WAyOk4D8ShLANBO8k/s9usAEDwzcn97xOZ2JTrwAAAABJRU5ErkJggg=="))), WrapMode.Tile)

    Private _Value As Integer = 0
    Private _Maximum As Integer = 100
#End Region

#Region " Properties "
    <Category("Control")>
    Public Property Maximum As Integer
        Get
            Return _Maximum
        End Get
        Set(ByVal value As Integer)
            If value < _Value Then _Value = value
            _Maximum = value
            FillValue = CInt((_Value / _Maximum) * (Width - 50))
            ChangePaths()
            Invalidate()
        End Set
    End Property

    <Category("Control")>
    Public Property Value As Integer
        Get
            Select Case _Value
                Case 0
                    Return 0
                    FillValue = CInt((_Value / _Maximum) * (Width - 50))
                    ChangePaths()
                    Invalidate()
                Case Else
                    Return _Value
                    FillValue = CInt((_Value / _Maximum) * (Width - 50))
                    ChangePaths()
                    Invalidate()
            End Select
        End Get
        Set(ByVal value As Integer)
            If value > _Maximum Then value = _Maximum
            _Value = value
            FillValue = CInt((_Value / _Maximum) * (Width - 50))
            ChangePaths()
            Invalidate()
        End Set
    End Property
#End Region

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or _
        ControlStyles.ResizeRedraw Or ControlStyles.SupportsTransparentBackColor Or ControlStyles.UserPaint, True)

        BackColor = Color.Transparent
        DoubleBuffered = True
        Font = New Font("Arial", 16)
        MinimumSize = New Size(60, 80)

        TB.TranslateTransform(0, -5, MatrixOrder.Prepend)

        P3 = New Pen(Color.FromArgb(190, 190, 190))
        B1 = New SolidBrush(Color.FromArgb(84, 83, 81))
    End Sub

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        FillValue = CInt((_Value / _Maximum) * (Width - 50))

        OPath = New GraphicsPath

        With OPath
            .AddArc(14, Height - 31, 20, 20, 180, 90)
            .AddArc(Width - 44, Height - 31, 20, 20, -90, 90)
            .AddArc(Width - 44, Height - 21, 20, 20, 0, 90)
            .AddArc(14, Height - 21, 20, 20, 90, 90)
            .CloseAllFigures()
        End With
        ChangePaths()

        Height = 80

        Invalidate()
        MyBase.OnResize(e)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        With e.Graphics
            .SmoothingMode = SmoothingMode.HighQuality

            Select Case _Value
                Case 0
                    .FillPath(IGB, OPath)
                    .DrawPath(P1, OPath)
                Case Else
                    .FillPath(IGB, OPath)
                    .DrawPath(P1, OPath)
                    .FillPath(TB, IPath)
                    .DrawPath(P2, IPath)

                    .FillPath(IndicatorGB, Indicator)
                    .DrawPath(P3, Indicator)

                    Select Case _Value
                        Case Is < 10
                            .DrawString(_Value & "%", Font, B1, FillValue + 6, 7)
                        Case Is < 100
                            .DrawString(_Value & "%", Font, B1, FillValue - 5, 7)
                        Case Else
                            .DrawString(_Value & "%", Font, B1, FillValue - 11, 7)
                    End Select
            End Select
        End With

        MyBase.OnPaint(e)
    End Sub

    Private Sub ChangePaths()
        If Width > 0 AndAlso Height > 0 Then
            IPath = New GraphicsPath
            Indicator = New GraphicsPath

            OGB = New LinearGradientBrush(New Rectangle(Width - 44, Height - 31, 31, 31), Color.FromArgb(173, 173, 173) _
                                              , Color.FromArgb(195, 195, 195), 90)
            IGB = New LinearGradientBrush(New Rectangle(Width - 43, Height - 30, 30, 30), Color.FromArgb(201, 201, 201) _
                                              , Color.FromArgb(225, 225, 225), 90)
            IOGB = New LinearGradientBrush(New Rectangle(19, Height - 26, Width - 6, Height - 26) _
                                               , Color.FromArgb(125, 175, 225), Color.FromArgb(55, 130, 205), -90)
            IndicatorGB = New LinearGradientBrush(New Rectangle(FillValue - 11, 0, FillValue + 19, 45) _
                                                  , Color.FromArgb(232, 232, 232), Color.FromArgb(202, 202, 202), 90)

            P1 = New Pen(OGB)
            P2 = New Pen(IOGB)


            If FillValue >= 13 Then
                With IPath
                    .AddArc(19, Height - 26, 20, 20, 180, 90)
                    .AddArc(FillValue, Height - 26, 20, 20, -90, 90)
                    .AddArc(FillValue, Height - 26, 20, 20, 0, 90)
                    .AddArc(19, Height - 26, 20, 20, 90, 90)
                End With
            End If
            With Indicator
                .AddArc(FillValue - 11, 0, 8, 8, 180, 90)
                .AddArc(FillValue + 40, 0, 8, 8, -90, 90)
                .AddArc(FillValue + 40, 27, 8, 8, 0, 90)
                .AddLine(FillValue + 24, 35, FillValue + 19, 45)
                .AddLine(FillValue + 14, 35, FillValue - 3, 35)
                .AddArc(FillValue - 11, 27, 8, 8, 90, 90)
                .CloseFigure()
            End With
        End If
    End Sub

    Public Sub Increment(ByVal Amount As Integer)
        Value += Amount
    End Sub
End Class

<DefaultEvent("CheckedChanged")> Public Class BaWGUIRadioButton : Inherits Control
#Region " Declarations "
    Private GB As LinearGradientBrush
    Private R1 As Rectangle
    Private P1 As Pen
    Private B1, B2 As SolidBrush
    Private CSF As StringFormat = New StringFormat() With {.LineAlignment = StringAlignment.Center}

    Event CheckedChanged(ByVal sender As Object)

    Private _Checked As Boolean
    Private _Group As Integer = 1
#End Region

#Region " Properties "
    <Category("Appearance")> _
    Property Checked As Boolean
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

    <Category("Custom")> _
    Property Group As Integer
        Get
            Return _Group
        End Get
        Set(ByVal value As Integer)
            _Group = value
        End Set
    End Property
#End Region
    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or _
        ControlStyles.ResizeRedraw Or ControlStyles.SupportsTransparentBackColor Or ControlStyles.UserPaint, True)

        BackColor = Color.Transparent
        DoubleBuffered = True
        Font = New Font("Arial", 12)
        Size = New Size(200, 26)

        P1 = New Pen(Color.FromArgb(160, 160, 160))
        B1 = New SolidBrush(Color.FromArgb(114, 114, 114))
        B2 = New SolidBrush(Color.FromArgb(95, 165, 230))
    End Sub

  Private Sub InvalidateControls()
        If Not IsHandleCreated OrElse Not _Checked Then Return

        For Each C As Control In Parent.Controls
            If C IsNot Me AndAlso TypeOf C Is BaWGUIRadioButton AndAlso DirectCast(C, BaWGUIRadioButton).Group = _Group Then
                DirectCast(C, BaWGUIRadioButton).Checked = False
            End If
        Next
    End Sub

    Protected Overrides Sub OnClick(ByVal e As EventArgs)
        If Not _Checked Then Checked = True
        MyBase.OnClick(e)
    End Sub

    Protected Overrides Sub OnCreateControl()
        InvalidateControls()
        MyBase.OnCreateControl()
    End Sub

    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        Invalidate() : MyBase.OnTextChanged(e)
    End Sub

    Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
        Height = 26

        R1 = New Rectangle(30, 0, Width, Height)

        GB = New LinearGradientBrush(New Rectangle(0, 0, 25, 25), Color.FromArgb(244, 245, 244) _
                                     , Color.FromArgb(227, 227, 227), 90)

        Invalidate()
        MyBase.OnResize(e)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        With e.Graphics
            .SmoothingMode = SmoothingMode.HighQuality

            .FillEllipse(GB, 0, 0, 25, 25)
            .DrawEllipse(P1, 0, 0, 24, 24)

            .DrawString(Text, Font, B1, R1, CSF)

            If Checked Then
                .FillEllipse(B2, 8, 8, 8, 8)
            End If
        End With

        MyBase.OnPaint(e)
    End Sub
End Class

<DefaultEvent("TextChanged")> Public Class BaWGUITextBox : Inherits Control
#Region " Declarations "
    Private Side, Button As GraphicsPath
    Private ButtonOGB, ButtonGB As LinearGradientBrush
    Private P1, P2 As Pen
    Private B1 As SolidBrush

    Private WithEvents TBox As TextBox

    Private _MaxLength As Integer = 32767
    Private _ReadOnly As Boolean
    Private _UseSystemPasswordChar As Boolean
    Private _Multiline As Boolean
    Private _Image As Image
#End Region

#Region " Properties "
    <Category("Options")> _
    Overrides Property Font As Font
        Get
            Return MyBase.Font
        End Get
        Set(ByVal value As Font)
            MyBase.Font = value
            If TBox IsNot Nothing Then
                TBox.Font = value
                TBox.Location = New Point(3, 5)
                TBox.Width = Width - 6

                If Not _Multiline Then
                    Height = TBox.Height + 11
                End If
            End If
        End Set
    End Property

    <Category("Appearance")> _
    Property Image As Image
        Get
            Return _Image
        End Get
        Set(ByVal value As Image)
            _Image = value
        End Set
    End Property

    <Category("Options")> _
    Property MaxLength As Integer
        Get
            Return _MaxLength
        End Get
        Set(ByVal value As Integer)
            _MaxLength = value
            If TBox IsNot Nothing Then
                TBox.MaxLength = value
            End If
        End Set
    End Property

    <Category("Options")> _
    Property Multiline As Boolean
        Get
            Return _Multiline
        End Get
        Set(ByVal value As Boolean)
            _Multiline = value
            If TBox IsNot Nothing Then
                TBox.Multiline = value

                If value Then
                    TBox.Height = Height - 11
                Else
                    Height = TBox.Height + 11
                End If

            End If
        End Set
    End Property

    <Category("Options")> _
    Property [ReadOnly] As Boolean
        Get
            Return _ReadOnly
        End Get
        Set(ByVal value As Boolean)
            _ReadOnly = value
            If TBox IsNot Nothing Then
                TBox.ReadOnly = value
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
            If TBox IsNot Nothing Then
                TBox.Text = value
            End If
        End Set
    End Property

    <Category("Options")> _
    Property UseSystemPasswordChar As Boolean
        Get
            Return _UseSystemPasswordChar
        End Get
        Set(ByVal value As Boolean)
            _UseSystemPasswordChar = value
            If TBox IsNot Nothing Then
                TBox.UseSystemPasswordChar = value
            End If
        End Set
    End Property
#End Region

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or _
        ControlStyles.ResizeRedraw Or ControlStyles.SupportsTransparentBackColor Or ControlStyles.UserPaint, True)

        BackColor = Color.Transparent
        DoubleBuffered = True
        Font = New Font("Arial", 16)
        Size = New Size(125, 53)

        TBox = New TextBox
        With TBox
            .BackColor = Color.FromArgb(245, 245, 245)
            .BorderStyle = BorderStyle.None
            .Cursor = Cursors.IBeam
            .Font = Font
            .ForeColor = Color.FromArgb(185, 185, 185)
            .Height = Height - 11
            .Location = New Point(5, 5)
            .MaxLength = _MaxLength
            .Multiline = _Multiline
            .ReadOnly = _ReadOnly
            .Text = Text
            .UseSystemPasswordChar = _UseSystemPasswordChar
            .Width = Width - 10
        End With

        P1 = New Pen(Color.FromArgb(180, 180, 180))
        B1 = New SolidBrush(Color.FromArgb(245, 245, 245))
    End Sub

    Private Sub OnBaseKeyDown(ByVal s As Object, ByVal e As KeyEventArgs) Handles TBox.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.A Then
            TBox.SelectAll()
            e.SuppressKeyPress = True
        End If
        If e.Control AndAlso e.KeyCode = Keys.C Then
            TBox.Copy()
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub OnBaseTextChanged(ByVal s As Object, ByVal e As EventArgs) Handles TBox.TextChanged
        Text = TBox.Text
    End Sub

    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        If Not Controls.Contains(TBox) Then
            Controls.Add(TBox)
        End If
    End Sub

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        If Width > 0 AndAlso Height > 0 Then
            If Controls.Contains(TBox) AndAlso Not _Multiline AndAlso _Image IsNot Nothing Then
                TBox.Location = New Point(20, 14)
                TBox.Width = Width - 81
                Height = 53
            ElseIf Controls.Contains(TBox) AndAlso _Multiline Then
                TBox.Location = New Point(5, 5)
                TBox.Width = Width - 10
                TBox.Height = Height - 11
            ElseIf Controls.Contains(TBox) AndAlso Not _Multiline Then
                TBox.Location = New Point(5, 5)
                TBox.Width = Width - 10
                TBox.Height = Height - 11
                Height = 53
            End If


            Side = New GraphicsPath
            Button = New GraphicsPath

            ButtonOGB = New LinearGradientBrush(New Rectangle(Width - 51, 0, Width, Height), Color.FromArgb(125, 180, 235) _
                                               , Color.FromArgb(45, 125, 200), 90)
            ButtonGB = New LinearGradientBrush(New Rectangle(Width - 50, 0, Width, Height), Color.FromArgb(175, 215, 240) _
                                               , Color.FromArgb(70, 145, 215), 90)

            P2 = New Pen(ButtonOGB)

            With Side
                .AddArc(0, 0, 10, 10, 180, 90)
                .AddArc(Width - 11, 0, 10, 10, -90, 90)
                .AddArc(Width - 11, Height - 11, 10, 10, 0, 90)
                .AddArc(0, Height - 11, 10, 10, 90, 90)
                .CloseFigure()
            End With
            With Button
                .AddLine(Width - 51, Height - 1, Width - 51, 0)
                .AddArc(Width - 11, 0, 10, 10, -90, 90)
                .AddArc(Width - 11, Height - 11, 10, 10, 0, 90)
                .CloseFigure()
            End With
        End If

        Invalidate()
        MyBase.OnResize(e)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        With e.Graphics
            .SmoothingMode = SmoothingMode.HighQuality

            .FillPath(B1, Side)
            .DrawPath(P1, Side)

            If Not _Multiline AndAlso _Image IsNot Nothing Then
                .FillPath(ButtonGB, Button)
                .DrawPath(P2, Button)

                .DrawImage(_Image, Width - 42, 11, 32, 32)
            End If
        End With

        MyBase.OnPaint(e)
    End Sub
End Class

Class BaWGUIThemeContainer : Inherits ContainerControl
#Region " Declarations "
    Private OverCls, OverMax, OverMin As Boolean
    Private XF, PF, MF As Font
    Private Base, Header As GraphicsPath
    Private BaseGB, HeaderGB, HeaderOutlineGB, ButtonOuterGB, ButtonInnerGB, ButtonOInnerGB As LinearGradientBrush
    Private R1, R2 As Rectangle
    Private P1, P2, P3 As Pen
    Private B1, B2, B3 As SolidBrush
    Private CSF As StringFormat = New StringFormat() With {.Alignment = StringAlignment.Center}

    Private _DrawButtonStrings As Boolean = True
#End Region

#Region " Properties "
    <Category("Drawing")> _
    Property DrawButtonStrings As Boolean
        Get
            Return _DrawButtonStrings
        End Get
        Set(ByVal value As Boolean)
            _DrawButtonStrings = value
        End Set
    End Property
#End Region

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or _
        ControlStyles.ResizeRedraw Or ControlStyles.UserPaint, True)

        BackColor = Color.White
        Dock = DockStyle.Fill
        DoubleBuffered = True
        Font = New Font("Arial", 12, FontStyle.Bold)
        XF = New Font("Tahoma", 12)
        PF = New Font("Arial", 16)
        MF = New Font("Arial", 20)

        P1 = New Pen(Color.FromArgb(128, 128, 128))
        ' P2 is in the OnSizeChanged event.
        P3 = New Pen(Color.FromArgb(180, 217, 246))
        B1 = New SolidBrush(Color.FromArgb(58, 118, 181))
        B2 = New SolidBrush(Color.FromArgb(71, 132, 191))
        B3 = New SolidBrush(Color.FromArgb(128, 0, 0, 0))
    End Sub

    Protected Overrides Sub CreateHandle()
        FindForm.FormBorderStyle = FormBorderStyle.None
        If FindForm.TransparencyKey = Nothing Then FindForm.TransparencyKey = Color.Fuchsia

        MyBase.CreateHandle()
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        If e.Button = MouseButtons.Left Then
            If OverMin Then
                FindForm.WindowState = FormWindowState.Minimized
            ElseIf OverMax Then
                If FindForm.WindowState = FormWindowState.Maximized Then FindForm.WindowState = FormWindowState.Normal _
                    Else FindForm.WindowState = FormWindowState.Maximized
            ElseIf OverCls Then
                FindForm.Close()
            Else
                If New Rectangle(FindForm.Location.X, FindForm.Location.Y, Width, 50).IntersectsWith(New Rectangle(MousePosition.X, MousePosition.Y, 1, 1)) AndAlso _
                                          Not FindForm.WindowState = FormWindowState.Maximized Then
                    Capture = False
                    Dim M As Message = Message.Create(FindForm.Handle, 161, New IntPtr(2), IntPtr.Zero)
                    DefWndProc(M)
                End If
            End If
        End If

        MyBase.OnMouseDown(e)
    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)
        If e.Location.X >= 33 AndAlso e.Location.Y >= 19 AndAlso e.Location.X <= 120 AndAlso e.Location.Y <= 37 Then
            If e.Location.X >= 33 AndAlso e.Location.X <= 51 Then
                OverCls = True : Invalidate()
            Else
                OverCls = False : Invalidate()
            End If

            If e.Location.X >= 104 Then
                OverMax = True : Invalidate()
            Else
                OverMax = False : Invalidate()
            End If

            If e.Location.X >= 68 AndAlso e.Location.X <= 86 Then
                OverMin = True : Invalidate()
            Else
                OverMin = False : Invalidate()
            End If
        Else
            OverMin = False : OverMax = False : OverCls = False
            Invalidate()
        End If

        MyBase.OnMouseMove(e)
    End Sub

    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        Invalidate() : MyBase.OnTextChanged(e)
    End Sub

    Protected Overrides Sub OnSizeChanged(ByVal e As System.EventArgs)
        If Width > 0 AndAlso Height > 0 Then
            Base = New GraphicsPath
            Header = New GraphicsPath

            R1 = New Rectangle(-1, CInt((50 - Font.Size) / 2 - 1), Width, Height)
            R2 = New Rectangle(0, CInt((50 - Font.Size) / 2 - 2), Width, Height)

            BaseGB = New LinearGradientBrush(New Rectangle(0, 50, Width, Height) _
                                             , Color.FromArgb(236, 236, 236), Color.FromArgb(232, 232, 232), 90)
            HeaderGB = New LinearGradientBrush(New Rectangle(0, 0, Width, 50) _
                                             , Color.FromArgb(161, 207, 243), Color.FromArgb(80, 152, 222), 90)
            HeaderOutlineGB = New LinearGradientBrush(New Rectangle(0, 0, Width, 50) _
                                             , Color.FromArgb(102, 167, 227), Color.FromArgb(52, 130, 202), -90)
            ButtonOuterGB = New LinearGradientBrush(New Rectangle(0, 15, 25, 25) _
                                             , Color.FromArgb(89, 158, 228), Color.FromArgb(150, 202, 241), 90)
            ButtonInnerGB = New LinearGradientBrush(New Rectangle(0, 20, 16, 16) _
                                             , Color.FromArgb(185, 211, 239), Color.FromArgb(145, 191, 238), 90)
            ButtonOInnerGB = New LinearGradientBrush(New Rectangle(0, 22, 16, 16) _
                                             , Color.FromArgb(94, 163, 215), Color.FromArgb(13, 67, 141), 90)

            P2 = New Pen(HeaderOutlineGB)


            With Base
                .AddLine(Width - 1, 50, Width - 1, Height - 11)
                .AddArc(Width - 11, Height - 11, 10, 10, 0, 90)
                .AddArc(0, Height - 11, 10, 10, 90, 90)
                .AddLine(0, Height - 11, 0, 50)
            End With

            With Header
                .AddArc(0, 0, 10, 10, 180, 90)
                .AddArc(Width - 11, 0, 10, 10, -90, 90)
                .AddLine(Width - 1, 50, 0, 50)
                .AddLine(0, 50, 0, 5)
            End With
            Invalidate()
        End If

        Invalidate()
        MyBase.OnSizeChanged(e)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        With e.Graphics
            .SmoothingMode = SmoothingMode.HighQuality
            .Clear(FindForm.TransparencyKey)

            ' Base & Header
            .FillPath(BaseGB, Base)
            .DrawPath(P1, Base)

            .FillPath(HeaderGB, Header)
            .DrawPath(P2, Header)
            .DrawLine(P3, 4, 1, Width - 5, 1) ' Little header shine


            ' Buttons
            If OverCls Then
                .FillEllipse(ButtonOuterGB, 30, 15, 25, 25)
                .FillEllipse(B1, 33, 19, 18, 18)
                .FillEllipse(ButtonOInnerGB, 34, 21, 16, 16)
                If _DrawButtonStrings Then .DrawString("x", XF, B3, 36, 17)
            Else
                .FillEllipse(ButtonOuterGB, 30, 15, 25, 25)
                .FillEllipse(B2, 33, 19, 18, 18)
                .FillEllipse(ButtonInnerGB, 34, 19, 16, 16)
            End If

            If OverMax Then
                .FillEllipse(ButtonOuterGB, 100, 15, 25, 25)
                .FillEllipse(B1, 103, 19, 18, 18)
                .FillEllipse(ButtonOInnerGB, 104, 21, 16, 16)
                If _DrawButtonStrings Then .DrawString("+", PF, B3, 102, 17)
            Else
                .FillEllipse(ButtonOuterGB, 100, 15, 25, 25)
                .FillEllipse(B2, 103, 19, 18, 18)
                .FillEllipse(ButtonInnerGB, 104, 19, 16, 16)
            End If

            If OverMin Then
                .FillEllipse(ButtonOuterGB, 65, 15, 25, 25)
                .FillEllipse(B1, 68, 19, 18, 18)
                .FillEllipse(ButtonOInnerGB, 69, 21, 16, 16)
                If _DrawButtonStrings Then .DrawString("-", MF, B3, 69, 10)
            Else
                .FillEllipse(ButtonOuterGB, 65, 15, 25, 25)
                .FillEllipse(B2, 68, 19, 18, 18)
                .FillEllipse(ButtonInnerGB, 69, 19, 16, 16)
            End If


            .DrawString(Text, Font, Brushes.DarkSlateGray, R1, CSF)
            .DrawString(Text, Font, Brushes.WhiteSmoke, R2, CSF)
        End With

        MyBase.OnPaint(e)
    End Sub
End Class

<DefaultEvent("Scroll")> Public Class BaWGUITrackBar : Inherits Control
#Region " Declarations "
    Private Moveable As Boolean
    Private Slider, Track As GraphicsPath
    Private DrawValue As Integer
    Private SliderGB, SliderOGB As LinearGradientBrush
    Private P1, P2, P3 As Pen
    Private B1, B2 As SolidBrush

    Event Scroll(ByVal sender As Object)

    Private _Maximum As Integer = 10
    Private _Minimum As Integer
    Private _Value As Integer
#End Region

#Region " Properties "
    <Category("Behavior")> _
    Property Maximum As Integer
        Get
            Return _Maximum
        End Get
        Set(ByVal value As Integer)
            _Maximum = value
            If value < _Value Then _Value = value
            If value < _Minimum Then _Minimum = value

            DrawValue = CInt((_Value - _Minimum) / (_Maximum - _Minimum) * (Width - 24))
            ChangePaths()
            Invalidate()
        End Set
    End Property

    <Category("Behavior")> _
    Property Minimum As Integer
        Get
            Return _Minimum
        End Get
        Set(ByVal value As Integer)
            If value < 0 Then
            End If

            _Minimum = value

            If value > _Value Then _Value = value
            If value > _Maximum Then _Maximum = value

            DrawValue = CInt((_Value - _Minimum) / (_Maximum - _Minimum) * (Width - 24))
            ChangePaths()
            Invalidate()
        End Set
    End Property

    <Category("Behavior")> _
    Property Value As Integer
        Get
            Return _Value
        End Get
        Set(ByVal value As Integer)
            If value = _Value Then Return

            If value > _Maximum Then value = _Maximum
            If value < _Minimum Then value = _Minimum

            _Value = value

            DrawValue = CInt((_Value - _Minimum) / (_Maximum - _Minimum) * (Width - 24))
            ChangePaths()
            Invalidate()
            RaiseEvent Scroll(Me)
        End Set
    End Property
#End Region

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or _
        ControlStyles.ResizeRedraw Or ControlStyles.SupportsTransparentBackColor Or ControlStyles.UserPaint, True)

        BackColor = Color.Transparent
        DoubleBuffered = True

        P1 = New Pen(Color.FromArgb(156, 156, 156))
        P2 = New Pen(Color.FromArgb(205, 205, 205))
        B1 = New SolidBrush(Color.FromArgb(194, 194, 194))
        B2 = New SolidBrush(Color.FromArgb(75, 145, 215))
    End Sub

    Protected Overrides Sub OnKeyDown(ByVal e As KeyEventArgs)
        If e.KeyCode = Keys.Subtract OrElse e.KeyCode = Keys.Down OrElse e.KeyCode = Keys.Left Then
            If Value = 0 Then Exit Sub
            Value -= 1
        ElseIf e.KeyCode = Keys.Add OrElse e.KeyCode = Keys.Up OrElse e.KeyCode = Keys.Right Then
            If Value = _Maximum Then Exit Sub
            Value += 1
        End If

        MyBase.OnKeyDown(e)
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        If e.Button = MouseButtons.Left AndAlso Height > 0 Then
            DrawValue = CInt((_Value - _Minimum) / (_Maximum - _Minimum) * (Width - 24))
            ChangePaths()

            Moveable = New Rectangle(DrawValue, 0, 24, Height).Contains(e.Location)
        End If

        MyBase.OnMouseDown(e)
    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)
        If Moveable AndAlso e.X > -1 AndAlso e.X < Width + 1 Then Value = _Minimum + _
            CInt((_Maximum - _Minimum) * (e.X / Width))

        MyBase.OnMouseMove(e)
    End Sub

    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        Moveable = False : MyBase.OnMouseUp(e)
    End Sub

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        If Width > 0 AndAlso Height > 0 Then
            DrawValue = CInt((_Value - _Minimum) / (_Maximum - _Minimum) * (Width - 24))

            Track = New GraphicsPath

            SliderOGB = New LinearGradientBrush(New Rectangle(DrawValue, 0, 24, Height), Color.FromArgb(100, 170, 230) _
                                                , Color.FromArgb(70, 140, 210), 90)

            P3 = New Pen(SliderOGB)


            With Track
                .AddArc(0, 27, 10, 10, 90, 180)
                .AddArc(Width - 11, 27, 10, 10, -90, 180)
                .CloseFigure()
            End With
            ChangePaths()

            Height = 37
        End If

        Invalidate()
        MyBase.OnResize(e)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        With e.Graphics
            .SmoothingMode = SmoothingMode.HighQuality

            .FillPath(B1, Track)
            .DrawLine(P1, 5, 27, Width - 6, 27)

            For i As Integer = 0 To _Maximum - _Minimum + 1
                .DrawLine(P2, CInt(i / (_Maximum - _Minimum) * (Width - 24) + 10), 25 _
                          , CInt(i / (_Maximum - _Minimum) * (Width - 24) + 10), 18)
            Next

            .FillPath(SliderGB, Slider)
            .DrawPath(P3, Slider)

            .FillEllipse(B2, DrawValue + 5, 10, 2, 2)
            .FillEllipse(B2, DrawValue + 9, 10, 2, 2)
            .FillEllipse(B2, DrawValue + 13, 10, 2, 2)
            .FillEllipse(B2, DrawValue + 5, 14, 2, 2)
            .FillEllipse(B2, DrawValue + 9, 14, 2, 2)
            .FillEllipse(B2, DrawValue + 13, 14, 2, 2)
        End With
    End Sub

    Private Sub ChangePaths()
        If Width > 0 AndAlso Height > 0 Then
            Slider = New GraphicsPath

            SliderGB = New LinearGradientBrush(New Rectangle(DrawValue, 0, 24, Height), Color.FromArgb(170, 210, 245) _
                                               , Color.FromArgb(80, 150, 225), 90)


            With Slider
                .AddArc(DrawValue, 0, 4, 4, 180, 90)
                .AddArc(DrawValue + 17, 0, 4, 4, -90, 90)
                .AddLine(DrawValue + 21, 4, DrawValue + 21, 25)
                .AddLine(DrawValue + 21, 25, DrawValue + 10, Height - 1)
                .AddLine(DrawValue, 25, DrawValue, 2)
            End With
        End If
    End Sub
End Class