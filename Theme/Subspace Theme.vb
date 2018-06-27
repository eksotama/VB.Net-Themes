Imports System, System.IO, System.Collections.Generic
Imports System.Drawing, System.Drawing.Drawing2D
Imports System.ComponentModel, System.Windows.Forms
Imports System.Runtime.InteropServices
Imports System.Drawing.Imaging

'------------------
'Creator: dlwhdrlf
'Created: 17/12/2011
'Version: 1.0.0
'------------------
Class SubspaceTheme
    Inherits ThemeContainer154
    Sub New()
        TransparencyKey = Color.Fuchsia
        BackColor = Color.FromArgb(30, 30, 30)
        Header = 30
    End Sub
    Protected Overrides Sub ColorHook()

    End Sub

    Protected Overrides Sub PaintHook()
        'Body
        G.Clear(Color.FromArgb(30, 30, 30))
        G.FillRectangle(Brushes.Fuchsia, 0, 0, Width, 5)
        DrawBorders(Pens.Black, 0, 30, Width, Height)

        'HeaderShadow
        DrawGradient(Color.Black, Color.FromArgb(30, 30, 30), 1, 28, Width, 10)

        'BottomBody
        DrawGradient(Color.FromArgb(30, 30, 30), Color.Black, 0, Height - 23, Width, 10)
        DrawGradient(Color.FromArgb(0, 0, 0), Color.FromArgb(57, 57, 58), 0, Height - 12, Width \ 2, Height, 360)
        DrawGradient(Color.FromArgb(0, 0, 0), Color.FromArgb(57, 57, 58), Width \ 2, Height - 12, Width \ 2, Height, 180)
        G.DrawLine(Pens.Black, 0, Height - 13, Width, Height - 13)
        G.DrawLine(New Pen(Color.FromArgb(57, 57, 58)), Width \ 2, Height - 11, Width \ 2, Height)


        'LeftSideBody
        G.FillRectangle(Brushes.Black, 0, 30, 8, Height)
        DrawBorders(Pens.Black, 1, 30, 9, Height - 2)
        G.DrawLine(New Pen(Color.FromArgb(40, 40, 40)), 8, 30, 8, Height)

        'RightSideBody
        G.FillRectangle(Brushes.Black, Width - 9, 30, 11, Height - 20)
        DrawBorders(Pens.Black, Width - 10, 30, 11, Height - 2)
        G.DrawLine(New Pen(Color.FromArgb(60, 60, 60)), Width - 9, 30, Width - 9, Height)



        'Header
        G.FillRectangle(Brushes.Black, 0, 5, Width, 24)
        DrawText(Brushes.DodgerBlue, HorizontalAlignment.Left, 55, 2)
        G.FillRectangle(New SolidBrush(Color.FromArgb(50, Color.White)), 0, 5, Width - 1, 11)
        DrawBorders(Pens.Black, 1, 4, Width - 2, 24)
        G.DrawLine(New Pen(Color.FromArgb(108, 108, 108)), 1, 5, Width, 5)
        G.DrawLine(New Pen(Color.FromArgb(60, 60, 60)), 1, 28, Width - 2, 28)
        G.DrawLine(Pens.Black, 1, 30, Width - 3, 30)

        '-----------------------------------------------------
        'Box
        DrawBorders(Pens.Black, 8, 0, 34, 32)
        DrawGradient(Color.FromArgb(57, 57, 58), Color.FromArgb(2, 4, 12), 9, 1, 32, 16)
        DrawGradient(Color.FromArgb(2, 4, 23), Color.FromArgb(57, 57, 58), 9, 15, 32, 16)
        'Lines
        DrawGradient(Color.FromArgb(100, 213, 255), Color.FromArgb(51, 162, 255), 17, 8, 3, 15)
        DrawGradient(Color.FromArgb(100, 213, 255), Color.FromArgb(51, 162, 255), CInt(47 / 2), 4.5, 3, 20.5)
        DrawGradient(Color.FromArgb(100, 213, 255), Color.FromArgb(51, 162, 255), 30, 8, 3, 15)

        DrawImage(HorizontalAlignment.Left, 9, 1)

        'Gloss
        G.FillRectangle(New SolidBrush(Color.FromArgb(15, Color.White)), 10, 2, 31, 13)
        '------------------------------------------------------

        'SideBoxes
        DrawGradient(Color.FromArgb(10, 10, 10), Color.FromArgb(47, 47, 47), 42, 2, 5, 15)
        DrawGradient(Color.FromArgb(47, 47, 47), Color.FromArgb(10, 10, 10), 42, 15, 5, 15)
        DrawGradient(Color.FromArgb(47, 47, 47), Color.FromArgb(10, 10, 10), 3, 2, 5, 15)
        DrawGradient(Color.FromArgb(10, 10, 10), Color.FromArgb(47, 47, 47), 3, 15, 5, 15)
        DrawBorders(Pens.Black, 42, 2, 5, 29)
        DrawBorders(Pens.Black, 3, 2, 5, 29)
        G.DrawLine(Pens.Black, 3, 15, 7, 15)
        G.DrawLine(Pens.Black, 42, 15, 46, 15)

        'Animation


        'Icon

        G.DrawLine(Pens.Black, 0, Height - 1, Width, Height - 1)
        DrawCorners(Color.Fuchsia)
    End Sub
End Class

'------------------
'Creator: dlwhdrlf
'Created: 17/12/2011
'Version: 1.0.0
'------------------
Class SubspaceTopButton
    Inherits ThemeControl154
    Sub New()
        Transparent = True
        BackColor = Color.Transparent
    End Sub
    Protected Overrides Sub ColorHook()

    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(Color.Black)
        DrawBorders(Pens.Black)
        DrawBorders(New Pen(Color.FromArgb(69, 71, 70)), 1)
        DrawBorders(Pens.Black, 2)
        DrawGradient(Color.White, Color.FromArgb(69, 71, 70), 1, 1, Width \ 2, 1, 360)
        DrawGradient(Color.White, Color.FromArgb(69, 71, 70), 1, 1, 1, Height \ 2)
        G.FillRectangle(New SolidBrush(Color.FromArgb(40, Color.White)), 0, 0, Width, 13)

        If State = MouseState.Over Then
            DrawGradient(Color.FromArgb(98, 192, 255), Color.FromArgb(59, 144, 255), 0, 0, Width, Height)
            DrawBorders(New Pen(Color.FromArgb(0, 44, 190), 0))
            DrawBorders(New Pen(Color.FromArgb(84, 177, 255)), 1)
            DrawGradient(Color.White, Color.FromArgb(84, 177, 255), 1, 1, Width \ 2, 1, 360)
            DrawGradient(Color.White, Color.FromArgb(84, 177, 255), 1, 1, 1, Height \ 2)
            G.FillRectangle(New SolidBrush(Color.FromArgb(40, Color.White)), 0, 0, Width, 13)
        ElseIf State = MouseState.Down Then
            DrawGradient(Color.FromArgb(84, 182, 255), Color.FromArgb(45, 134, 255), 0, 0, Width, Height)
            DrawBorders(New Pen(Color.FromArgb(0, 44, 190), 0))
            DrawBorders(New Pen(Color.FromArgb(84, 177, 255)), 1)
            DrawGradient(Color.White, Color.FromArgb(84, 177, 255), 1, 1, Width \ 2, 1, 360)
            DrawGradient(Color.White, Color.FromArgb(84, 177, 255), 1, 1, 1, Height \ 2)
            G.FillRectangle(New SolidBrush(Color.FromArgb(40, Color.White)), 0, 0, Width, 13)
        Else
        End If

        G.FillRectangle(New SolidBrush(Color.FromArgb(13, Color.White)), 3, 3, Width - 6, 8)
        DrawCorners(BackColor, 1)
    End Sub
End Class

'------------------
'Creator: dlwhdrlf
'Created: 17/12/2011
'Version: 1.0.0
'------------------
Class SubspaceButton
    Inherits ThemeControl154

    Sub New()
        Transparent = True
        BackColor = Color.Transparent
    End Sub

    Protected Overrides Sub ColorHook()

    End Sub

    Protected Overrides Sub PaintHook()
        DrawGradient(Color.FromArgb(25, 25, 25), Color.Black, 0, 0, Width, Height, 45)
        DrawText(Brushes.DodgerBlue, HorizontalAlignment.Center, 0, 0)
        G.FillRectangle(New SolidBrush(Color.FromArgb(40, Color.White)), 0, 0, Width, Height \ 2)
        DrawBorders(Pens.Black)
        DrawBorders(New Pen(Color.FromArgb(69, 71, 70)), 1)
        DrawGradient(Color.White, Color.FromArgb(69, 71, 70), 1, 1, Width \ 2, 1, 360)
        DrawGradient(Color.White, Color.FromArgb(69, 71, 70), 1, 1, 1, Height \ 2)
        DrawGradient(Color.White, Color.FromArgb(69, 71, 70), Width - 2, 1, 1, Height \ 2, 270)
        DrawGradient(Color.White, Color.FromArgb(69, 71, 70), Width - 2, Height \ 2, 1, Height \ 2)

        If State = MouseState.Over Then
            DrawGradient(Color.FromArgb(98, 192, 255), Color.FromArgb(59, 144, 255), 0, 0, Width, Height, 45)
            DrawBorders(New Pen(Color.FromArgb(0, 44, 190), 0))
            DrawBorders(New Pen(Color.FromArgb(84, 177, 255)), 1)
            DrawGradient(Color.White, Color.FromArgb(84, 177, 255), 1, 1, Width \ 2, 1, 360)
            DrawGradient(Color.White, Color.FromArgb(84, 177, 255), 1, 1, 1, Height \ 2)
            DrawText(Brushes.Black, HorizontalAlignment.Center, 0, 0)
            G.FillRectangle(New SolidBrush(Color.FromArgb(40, Color.White)), 0, 0, Width, 13)
            DrawCorners(BackColor, 1)
        ElseIf State = MouseState.Down Then
            DrawGradient(Color.FromArgb(84, 182, 255), Color.FromArgb(45, 134, 255), 0, 0, Width, Height, 45)
            DrawBorders(New Pen(Color.FromArgb(0, 44, 190), 0))
            DrawBorders(New Pen(Color.FromArgb(84, 177, 255)), 1)
            DrawGradient(Color.White, Color.FromArgb(84, 177, 255), 1, 1, Width \ 2, 1, 360)
            DrawGradient(Color.White, Color.FromArgb(84, 177, 255), 1, 1, 1, Height \ 2)
            DrawText(Brushes.Black, HorizontalAlignment.Center, 0, 0)
            G.FillRectangle(New SolidBrush(Color.FromArgb(40, Color.White)), 0, 0, Width, 13)
            DrawCorners(BackColor, 1)
        Else
        End If

        DrawCorners(BackColor, 1)
    End Sub
End Class

'------------------
'Creator: dlwhdrlf
'Created: 17/12/2011
'Version: 1.0.0
'------------------
Class SubspaceProgressbar
    Inherits ThemeControl154

    Private _Value As Integer
    Property Value() As Integer
        Get
            Return _Value
        End Get
        Set(ByVal value As Integer)
            If value > _Maximum Then value = _Maximum
            If value < 0 Then value = 0

            _Value = value
            Invalidate()
        End Set
    End Property

    Private _Maximum As Integer = 100
    Property Maximum() As Integer
        Get
            Return _Maximum
        End Get
        Set(ByVal value As Integer)
            If value < 1 Then value = 1
            If _Value > value Then _Value = value

            _Maximum = value
            Invalidate()
        End Set
    End Property

    Protected Overrides Sub ColorHook()

    End Sub

    Protected Overrides Sub PaintHook()
        DrawGradient(Color.Black, Color.FromArgb(40, 40, 40), 0, 0, Width, Height, 2)

        DrawGradient(Color.FromArgb(84, 182, 255), Color.FromArgb(45, 134, 255), 0, 0, CInt((_Value / _Maximum) * Width - 1), Height)
        G.FillRectangle(New SolidBrush(Color.FromArgb(30, Color.White)), 0, 0, CInt((_Value / _Maximum) * Width - 1), Height \ 2)

        DrawBorders(Pens.Black)
        DrawBorders(Pens.Black, 2)
        DrawBorders(New Pen(Color.FromArgb(69, 71, 70)), 1)
        DrawGradient(Color.White, Color.Black, 0, 0, Width \ 4, 1, 360)
        DrawGradient(Color.White, Color.Black, 0, 0, 1, Height \ 2)
    End Sub
End Class

'------------------
'Creator: dlwhdrlf
'Created: 17/12/2011
'Version: 1.0.0
'------------------
Class Subspacegroupbox
    Inherits ThemeContainer154
    Sub New()
        ControlMode = True
        Header = 18
    End Sub
    Protected Overrides Sub ColorHook()

    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(Color.FromArgb(30, 30, 30))
        DrawBorders(Pens.Black)


        G.FillRectangle(Brushes.Black, 2, 2, Width - 4, 18)
        G.DrawLine(New Pen(Color.FromArgb(60, 60, 60)), 2, 18, Width - 2, 18)
        G.FillRectangle(New SolidBrush(Color.FromArgb(40, Color.White)), 2, 2, Width - 4, 7)

        DrawGradient(Color.Black, Color.FromArgb(30, 30, 30), 2, 19, Width - 4, 8)


        DrawGradient(Color.FromArgb(30, 30, 30), Color.Black, 7, Height - 16, Width - 14, 8)

        DrawGradient(Color.FromArgb(0, 0, 0), Color.FromArgb(57, 57, 58), 0, Height - 8, Width \ 2, Height - 4, 360)
        DrawGradient(Color.FromArgb(0, 0, 0), Color.FromArgb(57, 57, 58), Width \ 2, Height - 8, Width \ 2, Height - 4, 180)
        G.DrawLine(New Pen(Color.FromArgb(57, 57, 58)), Width \ 2, Height - 8, Width \ 2, Height)

        DrawText(Brushes.DodgerBlue, HorizontalAlignment.Left, 8, 1)

        'SideBoxes
        G.FillRectangle(Brushes.Black, 2, 19, 5, Height - 4)
        G.DrawLine(New Pen(Color.FromArgb(60, 60, 60)), 5, 19, 5, Height - 2)

        G.FillRectangle(Brushes.Black, Width - 6, 19, 10, Height - 4)
        G.DrawLine(New Pen(Color.FromArgb(60, 60, 60)), Width - 6, 19, Width - 6, Height - 2)
        'EndofSideboxes

        DrawBorders(New Pen(Color.FromArgb(60, 60, 60)), 1)
    End Sub
End Class

'------------------
'Creator: dlwhdrlf
'Created: 17/12/2011
'Version: 1.0.0
'------------------
Class SubspaceSeperator
    Inherits ThemeControl154
    Private _Orientation As Orientation
    Property Orientation() As Orientation
        Get
            Return _Orientation
        End Get
        Set(ByVal value As Orientation)
            _Orientation = value

            If value = Windows.Forms.Orientation.Vertical Then
                LockHeight = 0
                LockWidth = 14
            Else
                LockHeight = 14
                LockWidth = 0
            End If

            Invalidate()
        End Set
    End Property

    Sub New()
        Transparent = True
        BackColor = Color.Transparent

        LockHeight = 14
    End Sub
    Protected Overrides Sub ColorHook()

    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(BackColor)

        Dim BL1, BL2 As New ColorBlend
        BL1.Positions = New Single() {0.0F, 0.15F, 0.85F, 1.0F}
        BL2.Positions = New Single() {0.0F, 0.15F, 0.5F, 0.85F, 1.0F}

        BL1.Colors = New Color() {Color.Transparent, Color.FromArgb(60, 60, 60), Color.FromArgb(60, 60, 60), Color.Transparent}
        BL2.Colors = New Color() {Color.Transparent, Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 0), Color.Transparent}

        If _Orientation = Windows.Forms.Orientation.Vertical Then
            DrawGradient(BL1, 6, 0, 1, Height)
            DrawGradient(BL2, 7, 0, 1, Height)
        Else
            DrawGradient(BL1, 0, 6, Width, 1, 0.0F)
            DrawGradient(BL2, 0, 7, Width, 1, 0.0F)
        End If
    End Sub
End Class

'------------------
'Creator: dlwhdrlf
'Created: 17/12/2011
'Version: 1.0.0
'------------------
<DefaultEvent("CheckedChanged")> _
Class SubspaceCheckbox
    Inherits ThemeControl154

    Sub New()
        Transparent = True
        BackColor = Color.Transparent
        LockHeight = 15
    End Sub
    Protected Overrides Sub ColorHook()

    End Sub

    Protected Overrides Sub PaintHook()
        Dim CheckRectangle As New Rectangle(1, 1, Height - 3, Height - 3)

        G.Clear(BackColor)
        DrawGradient(Color.FromArgb(124, 175, 214), Color.FromArgb(95, 141, 205), CheckRectangle, 45)
        G.FillRectangle(New SolidBrush(Color.FromArgb(13, Color.White)), 0, 0, Height - 1, Height \ 2)


        Select Case _Checked
            Case True
                'Put your checked state here
                DrawGradient(Color.FromArgb(84, 182, 255), Color.FromArgb(45, 134, 255), CheckRectangle, 45)
                G.FillRectangle(New SolidBrush(Color.FromArgb(50, Color.White)), 0, 0, Height - 1, Height \ 2)
                G.DrawRectangle(New Pen(Color.FromArgb(84, 177, 255)), CheckRectangle)
            Case False
                'Put your unchecked state here

        End Select

        DrawText(Brushes.DodgerBlue, HorizontalAlignment.Left, 18, 1)
    End Sub


    Private Property _Checked As Boolean = False
    Property Checked() As Boolean
        Get
            Return _Checked
        End Get
        Set(ByVal v As Boolean)
            _Checked = v
        End Set
    End Property
    Protected Overrides Sub OnClick(ByVal e As System.EventArgs)
        _checked = Not _checked
        RaiseEvent CheckedChanged(Me)
        MyBase.OnClick(e)
    End Sub
    Event CheckedChanged(ByVal sender As Object)
End Class