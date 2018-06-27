Imports System, System.Collections
Imports System.Drawing, System.Drawing.Drawing2D
Imports System.ComponentModel, System.Windows.Forms
Imports System.IO, System.Collections.Generic
Imports System.Runtime.InteropServices
Imports System.Drawing.Imaging
'''''''''''''''''''''''''''''''''''''''''''''''''''
''''Credits: Aeonhack, Mavamaarten,  Support™.'''''
''''''''''''Themebase version: 1.5.4'''''''''''''''
'''''''''''''''''''''''''''''''''''''''''''''''''''
'#Region "VITheme"


Class VITheme
    Inherits ThemeContainer154

    Sub New()
        TransparencyKey = Color.Fuchsia
    End Sub

    Protected Overrides Sub ColorHook()

    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(Color.FromArgb(15, 15, 15))
        Dim P As New Pen(Color.FromArgb(32, 32, 32))
        G.DrawLine(P, 11, 31, Width - 12, 31)
        G.DrawLine(P, 11, 8, Width - 12, 8)
        G.FillRectangle(New LinearGradientBrush(New Rectangle(8, 38, Width - 16, Height - 46), Color.FromArgb(12, 12, 12), Color.FromArgb(18, 18, 18), LinearGradientMode.BackwardDiagonal), 8, 38, Width - 16, Height - 46)
        DrawText(Brushes.White, HorizontalAlignment.Left, 25, 6)
        DrawBorders(New Pen(Color.FromArgb(60, 60, 60)), 1)
        DrawBorders(Pens.Black)

        P = New Pen(Color.FromArgb(25, 25, 25))
        G.DrawLine(Pens.Black, 6, 0, 6, Height - 6)
        G.DrawLine(Pens.Black, Width - 6, 0, Width - 6, Height - 6)
        G.DrawLine(P, 6, 0, 6, Height - 6)
        G.DrawLine(P, Width - 8, 0, Width - 8, Height - 6)

        G.DrawRectangle(Pens.Black, 11, 4, Width - 23, 22)
        G.DrawLine(P, 6, Height - 6, Width - 8, Height - 6)
        G.DrawLine(Pens.Black, 6, Height - 8, Width - 8, Height - 8)
        DrawCorners(Color.Fuchsia)
    End Sub

End Class

Class VITabControl
    Inherits TabControl
    Dim OldIndex As Integer
    Private _Speed As Integer = 10
    Public Property Speed() As Integer
        Get
            Return _Speed
        End Get
        Set(ByVal value As Integer)
            If value > 20 Or value < -20 Then
                MsgBox("Speed needs to be in between -20 and 20.")
            Else
                _Speed = value
            End If
        End Set
    End Property
    Private LightBlack As Color = Color.FromArgb(18, 18, 18)
    Private LighterBlack As Color = Color.FromArgb(21, 21, 21)
    Private DrawGradientBrush, DrawGradientBrush2, DrawGradientBrushPen, DrawGradientBrushTab As LinearGradientBrush
    Private _ControlBColor As Color
    Public Property TabTextColor() As Color
        Get
            Return _ControlBColor
        End Get
        Set(ByVal v As Color)
            _ControlBColor = v
            Invalidate()
        End Set
    End Property

    Sub New()
        MyBase.New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint, True)
        TabTextColor = Color.White
        Alignment = TabAlignment.Top
        ItemSize = New Size(25, 30)
        SizeMode = TabSizeMode.FillToRight
        DrawMode = TabDrawMode.OwnerDrawFixed
    End Sub
    Sub DoAnimationScrollDown(ByVal Control1 As Control, ByVal Control2 As Control)
        Dim G As Graphics = Control1.CreateGraphics()
        Dim P1 As New Bitmap(Control1.Width, Control1.Height)
        Dim P2 As New Bitmap(Control2.Width, Control2.Height)
        Control1.DrawToBitmap(P1, New Rectangle(0, 0, Control1.Width, Control1.Height))
        Control2.DrawToBitmap(P2, New Rectangle(0, 0, Control2.Width, Control2.Height))
        For Each c As Control In Control1.Controls
            c.Hide()
        Next
        Dim Slide As Integer = Control1.Height - (Control1.Height Mod _Speed)
        Dim a As Integer
        For a = 0 To Slide Step _Speed
            G.DrawImage(P1, New Rectangle(0, a, Control1.Width, Control1.Height))
            G.DrawImage(P2, New Rectangle(0, a - Control2.Height, Control2.Width, Control2.Height))
        Next
        a = Control1.Width
        G.DrawImage(P1, New Rectangle(0, a, Control1.Width, Control1.Height))
        G.DrawImage(P2, New Rectangle(0, a - Control2.Height, Control2.Width, Control2.Height))
        SelectedTab = Control2
        For Each c As Control In Control2.Controls
            c.Show()
        Next
        For Each c As Control In Control1.Controls
            c.Show()
        Next
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        MyBase.OnPaint(e)
        e.Graphics.Clear(Color.FromArgb(18, 18, 18))
        Dim ItemBounds As Rectangle
        Dim TextBrush As New SolidBrush(Color.Empty)
        Dim TabBrush As New SolidBrush(Color.Black)
        For TabItemIndex As Integer = 0 To Me.TabCount - 1
            ItemBounds = Me.GetTabRect(TabItemIndex)
            e.Graphics.FillRectangle(TabBrush, ItemBounds)
            Dim BorderPen As Pen
            If TabItemIndex = SelectedIndex Then
                BorderPen = New Pen(Color.Black, 1)
            Else
                BorderPen = New Pen(Color.Black, 1)
            End If
            Dim rPen As New Rectangle(ItemBounds.Location.X + 3, ItemBounds.Location.Y + 0, ItemBounds.Width - 4, ItemBounds.Height - 2)
            e.Graphics.DrawRectangle(BorderPen, rPen)
            'Dim B1 As Brush = New HatchBrush(HatchStyle.Percent10, Color.FromArgb(35, 35, 35), Color.FromArgb(10, 10, 10))
            Dim B1 As Brush = New LinearGradientBrush(rPen, Color.FromArgb(15, 15, 15), Color.FromArgb(24, 24, 24), LinearGradientMode.Vertical)

            e.Graphics.FillRectangle(B1, rPen)

            BorderPen.Dispose()
            Dim sf As New StringFormat
            sf.LineAlignment = StringAlignment.Center
            sf.Alignment = StringAlignment.Center

            If Me.SelectedIndex = TabItemIndex Then
                TextBrush.Color = TabTextColor
            Else
                TextBrush.Color = Color.Gray
            End If
            e.Graphics.DrawString( _
            Me.TabPages(TabItemIndex).Text, _
            Me.Font, TextBrush, _
            RectangleF.op_Implicit(Me.GetTabRect(TabItemIndex)), sf)
            Try
                Me.TabPages(TabItemIndex).BackColor = Color.FromArgb(15, 15, 15)

            Catch
            End Try
        Next
        Try
            For Each Page As TabPage In Me.TabPages
                Page.BorderStyle = BorderStyle.None
            Next
        Catch
        End Try
        e.Graphics.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(255, Color.Black))), 2, 0, Width - 3, Height - 3)
        e.Graphics.DrawRectangle(New Pen(New SolidBrush(LighterBlack)), New Rectangle(3, 2, Width - 5, Height - 7))
        e.Graphics.DrawLine(New Pen(New SolidBrush(Color.FromArgb(255, Color.Black))), 2, 2, Width - 2, 2)
        e.Graphics.DrawLine(New Pen(New SolidBrush(Color.FromArgb(35, 35, 35))), 0, 0, 1, 1)
        e.Graphics.DrawLine(New Pen(New SolidBrush(Color.FromArgb(70, 70, 70))), 2, Height - 2, Width + 1, Height - 2)

    End Sub
    Protected Overrides Sub OnSelecting(ByVal e As System.Windows.Forms.TabControlCancelEventArgs)
        If OldIndex < e.TabPageIndex Then
            DoAnimationScrollUp(TabPages(OldIndex), TabPages(e.TabPageIndex))
        Else
            DoAnimationScrollDown(TabPages(OldIndex), TabPages(e.TabPageIndex))
        End If
    End Sub

    Protected Overrides Sub OnDeselecting(ByVal e As System.Windows.Forms.TabControlCancelEventArgs)
        OldIndex = e.TabPageIndex
    End Sub
    Sub DoAnimationScrollUp(ByVal Control1 As Control, ByVal Control2 As Control)
        Dim G As Graphics = Control1.CreateGraphics()
        Dim P1 As New Bitmap(Control1.Width, Control1.Height)
        Dim P2 As New Bitmap(Control2.Width, Control2.Height)
        Control1.DrawToBitmap(P1, New Rectangle(0, 0, Control1.Width, Control1.Height))
        Control2.DrawToBitmap(P2, New Rectangle(0, 0, Control2.Width, Control2.Height))

        For Each c As Control In Control1.Controls
            c.Hide()
        Next
        Dim Slide As Integer = Control1.Height - (Control1.Height Mod _Speed)
        Dim a As Integer
        For a = 0 To -Slide Step -_Speed
            G.DrawImage(P1, New Rectangle(0, a, Control1.Width, Control1.Height))
            G.DrawImage(P2, New Rectangle(0, a + Control2.Height, Control2.Width, Control2.Height))
        Next
        a = Control1.Width
        G.DrawImage(P1, New Rectangle(0, a, Control1.Width, Control1.Height))
        G.DrawImage(P2, New Rectangle(0, a + Control2.Height, Control2.Width, Control2.Height))

        SelectedTab = Control2

        For Each c As Control In Control2.Controls
            c.Show()
        Next

        For Each c As Control In Control1.Controls
            c.Show()
        Next
    End Sub
End Class

Class VIGroupBox
    Inherits ThemeContainer154

    Sub New()
        ControlMode = True
        Size = New Size(150, 150)
    End Sub

    Protected Overrides Sub ColorHook()

    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(Color.Transparent)
        Dim P1 As Pen = New Pen(Color.FromArgb(36, 36, 36))
        Dim P2 As Pen = New Pen(Color.FromArgb(48, 48, 48))
        DrawBorders(P1, 1)
        DrawBorders(Pens.Black)
        G.DrawLine(P2, 1, 1, Width - 2, 1)
        G.FillRectangle(New LinearGradientBrush(New Rectangle(4, 4, Width - 8, Height - 8), Color.FromArgb(12, 12, 12), Color.FromArgb(18, 18, 18), LinearGradientMode.BackwardDiagonal), 4, 4, Width - 8, Height - 8)
        DrawBorders(P1, 3)
        DrawBorders(Pens.Black, 5)
        Dim R1 As Rectangle = New Rectangle(5, 5, Width - 25, 20)
        G.DrawRectangle(Pens.Black, R1)
        G.DrawLine(P1, 5, 27, Width - 20, 27)
        G.DrawLine(P1, Width - 19, 6, Width - 19, 27)
        G.DrawLine(P2, 5, 25, Width - 22, 25)
        G.DrawLine(P2, Width - 21, 6, Width - 21, 25)
        DrawText(Brushes.White, 35, 7.5F)
    End Sub
End Class

Class VISeperator
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

        BL1.Colors = New Color() {Color.Transparent, Color.Black, Color.Black, Color.Transparent}
        BL2.Colors = New Color() {Color.Transparent, Color.FromArgb(24, 24, 24), Color.FromArgb(40, 40, 40), Color.FromArgb(36, 36, 36), Color.Transparent}

        If _Orientation = Windows.Forms.Orientation.Vertical Then
            DrawGradient(BL1, 6, 0, 1, Height)
            DrawGradient(BL2, 7, 0, 1, Height)
        Else
            DrawGradient(BL1, 0, 6, Width, 1, 0.0F)
            DrawGradient(BL2, 0, 7, Width, 1, 0.0F)
        End If

    End Sub

End Class

Class VIToggle
    Inherits ThemeControl154
    Private P1, P2 As Pen
    Private B1 As Brush
    Private B2 As Brush
    Private B3 As Brush
    Private _Checked As Boolean = False
    Public Property Checked() As Boolean
        Get
            Return _Checked
        End Get
        Set(ByVal checked As Boolean)
            _Checked = checked
            Invalidate()
        End Set
    End Property
    Sub New()
        BackColor = Color.Transparent
        Transparent = True
        Size = New Size(120, 25)
    End Sub
    Sub changeChecked() Handles Me.Click
        Select Case _Checked
            Case False
                _Checked = True
            Case True
                _Checked = False
        End Select
    End Sub
    Protected Overrides Sub ColorHook()
        P1 = New Pen(Color.FromArgb(0, 0, 0))
        P2 = New Pen(Color.FromArgb(24, 24, 24))
        B1 = New SolidBrush(Color.FromArgb(15, Color.FromArgb(26, 26, 26)))
        B2 = New SolidBrush(Color.White)
        B3 = New SolidBrush(Color.FromArgb(0, 0, 0))
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(BackColor)

        If (_Checked = False) Then
            G.FillRectangle(B3, 4, 4, 45, 15)
            G.DrawRectangle(P1, 4, 4, 45, 15)
            G.DrawRectangle(P2, 5, 5, 43, 13)
            G.DrawString("OFF", Font, Brushes.Red, 7, 5)
            G.FillRectangle(New LinearGradientBrush(New Rectangle(32, 2, 13, 19), Color.FromArgb(35, 35, 35), Color.FromArgb(25, 25, 25), 90S), 32, 2, 13, 19)
            G.DrawRectangle(P2, 32, 2, 13, 19)
            G.DrawRectangle(P1, 33, 3, 11, 17)
            G.DrawRectangle(P1, 31, 1, 15, 21)
        Else
            G.FillRectangle(B3, 4, 4, 45, 15)
            G.DrawRectangle(P1, 4, 4, 45, 15)
            G.DrawRectangle(P2, 5, 5, 43, 13)
            G.DrawString("ON", Font, Brushes.Green, 23, 5)
            G.FillRectangle(New LinearGradientBrush(New Rectangle(8, 2, 13, 19), Color.FromArgb(80, 80, 80), Color.FromArgb(60, 60, 60), 90S), 8, 2, 13, 19)
            G.DrawRectangle(P2, 8, 2, 13, 19)
            G.DrawRectangle(P1, 9, 3, 11, 17)
            G.DrawRectangle(P1, 7, 1, 15, 21)
        End If
        G.FillRectangle(B1, 2, 2, 41, 11)
        DrawText(B2, HorizontalAlignment.Left, 50, 0)
    End Sub
End Class