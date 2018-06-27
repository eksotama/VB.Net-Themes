Imports System.Drawing.Drawing2D
Imports System.ComponentModel

'PLEASE LEAVE CREDITS IN SOURCE, DO NOT REDISTRIBUTE!

'--------------------- [ Theme ] --------------------
'Creator: Mephobia
'Contact: Mephobia.HF (Skype)
'Created: 6.19.2013
'Changed: 6.19.2013
'-------------------- [ /Theme ] ---------------------

'PLEASE LEAVE CREDITS IN SOURCE, DO NOT REDISTRIBUTE!

Enum MouseState As Byte
    None = 0
    Over = 1
    Down = 2
    Block = 3
End Enum
Module Draw
    Public Function GetBrush(ByVal c As Color) As SolidBrush
        Return New SolidBrush(c)
    End Function
    Public Function GetPen(ByVal c As Color) As Pen
        Return New Pen(New SolidBrush(c))
    End Function
    Function NoiseBrush(ByVal colors As Color()) As TextureBrush
        Dim B As New Bitmap(128, 128)
        Dim R As New Random(128)

        For X As Integer = 0 To B.Width - 1
            For Y As Integer = 0 To B.Height - 1
                B.SetPixel(X, Y, colors(R.Next(colors.Length)))
            Next
        Next

        Dim T As New TextureBrush(B)
        B.Dispose()

        Return T
    End Function
    Private CreateRoundPath As GraphicsPath
    Private CreateCreateRoundangle As Rectangle

    Function CreateRound(ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal slope As Integer) As GraphicsPath
        CreateCreateRoundangle = New Rectangle(x, y, width, height)
        Return CreateRound(CreateCreateRoundangle, slope)
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

    Public Sub InnerGlow(ByVal G As Graphics, ByVal Rectangle As Rectangle, ByVal Colors As Color())
        Dim SubtractTwo As Integer = 1
        Dim AddOne As Integer = 0
        For Each c In Colors
            G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(c.R, c.B, c.G))), Rectangle.X + AddOne, Rectangle.Y + AddOne, Rectangle.Width - SubtractTwo, Rectangle.Height - SubtractTwo)
            SubtractTwo += 2
            AddOne += 1
        Next
    End Sub
    Public Sub InnerGlowRounded(ByVal G As Graphics, ByVal Rectangle As Rectangle, ByVal Degree As Integer, ByVal Colors As Color())
        Dim SubtractTwo As Integer = 1
        Dim AddOne As Integer = 0
        For Each c In Colors
            G.DrawPath(New Pen(New SolidBrush(Color.FromArgb(c.R, c.B, c.G))), Draw.CreateRound(Rectangle.X + AddOne, Rectangle.Y + AddOne, Rectangle.Width - SubtractTwo, Rectangle.Height - SubtractTwo, Degree))
            SubtractTwo += 2
            AddOne += 1
        Next
    End Sub
End Module
Public Class xVisualTheme : Inherits ContainerControl

    Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.FromArgb(46, 43, 40)
        DoubleBuffered = True
    End Sub

    Dim TopTexture As TextureBrush = NoiseBrush({Color.FromArgb(66, 64, 62), Color.FromArgb(63, 61, 59), Color.FromArgb(69, 67, 65)})
    Dim InnerTexture As TextureBrush = NoiseBrush({Color.FromArgb(57, 53, 50), Color.FromArgb(56, 52, 49), Color.FromArgb(58, 55, 51)})
    Dim drawFont As New Font("Arial", 11, FontStyle.Bold)
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim G As Graphics = e.Graphics
        MyBase.OnPaint(e)
        G.Clear(Color.Fuchsia)

        Dim mainRect As New Rectangle(0, 0, Width, Height)
        Dim LeftHighlight As New LinearGradientBrush(New Rectangle(0, 0, Width, Height), Color.FromArgb(66, 64, 63), Color.FromArgb(56, 54, 53), 90S)
        Dim RightHighlight As New LinearGradientBrush(New Rectangle(0, 0, Width, Height), Color.FromArgb(80, 78, 77), Color.FromArgb(70, 68, 67), 90S)
        Dim TopOverlay As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, 53), Color.FromArgb(15, Color.White), Color.FromArgb(100, Color.FromArgb(43, 40, 38)), 90S)

        Dim mainGradient As New LinearGradientBrush(mainRect, Color.FromArgb(73, 71, 69), Color.FromArgb(69, 67, 64), 90S)
        G.FillRectangle(mainGradient, mainRect) 'Outside Rectangle
        G.DrawRectangle(Pens.Black, New Rectangle(0, 0, Width - 1, Height - 1))

        G.FillRectangle(InnerTexture, New Rectangle(10, 53, Width - 21, Height - 84)) 'Inner Rectangle
        G.DrawRectangle(Pens.Black, New Rectangle(10, 53, Width - 21, Height - 84))

        G.FillRectangle(TopTexture, New Rectangle(0, 0, Width - 1, 53)) 'Top Bar Rectangle
        G.FillRectangle(TopOverlay, New Rectangle(0, 0, Width - 1, 53))
        G.DrawRectangle(Pens.Black, New Rectangle(0, 0, Width - 1, 53))

        Dim blend As ColorBlend = New ColorBlend()

        'Add the Array of Color
        Dim bColors As Color() = New Color() {Color.FromArgb(10, Color.White), Color.FromArgb(10, Color.Black), Color.FromArgb(10, Color.White)}
        blend.Colors = bColors

        'Add the Array Single (0-1) colorpoints to place each Color
        Dim bPts As Single() = New Single() {0, 0.7, 1}
        blend.Positions = bPts

        Dim rect As New Rectangle(0, 0, Width - 1, 53)
        Using br As New LinearGradientBrush(rect, Color.White, Color.Black, LinearGradientMode.Vertical)

            'Blend the colors into the Brush
            br.InterpolationColors = blend

            'Fill the rect with the blend
            G.FillRectangle(br, rect)

        End Using

        G.DrawLine(GetPen(Color.FromArgb(173, 172, 172)), 4, 1, Width - 5, 1) 'Top Middle Highlight
        G.DrawLine(GetPen(Color.FromArgb(110, 109, 107)), 11, Height - 30, Width - 12, Height - 30) 'Bottom Middle Highlight

        G.FillRectangle(GetBrush(Color.FromArgb(173, 172, 172)), 3, 2, 1, 1) 'Top Left Corner Highlight
        G.FillRectangle(GetBrush(Color.FromArgb(133, 132, 132)), 2, 2, 1, 1)
        G.FillRectangle(GetBrush(Color.FromArgb(113, 112, 112)), 2, 3, 1, 1)
        G.FillRectangle(GetBrush(Color.FromArgb(83, 82, 82)), 1, 4, 1, 1)

        G.FillRectangle(GetBrush(Color.FromArgb(173, 172, 172)), Width - 4, 2, 1, 1) 'Top Right Corner Highlight
        G.FillRectangle(GetBrush(Color.FromArgb(133, 132, 132)), Width - 3, 2, 1, 1)
        G.FillRectangle(GetBrush(Color.FromArgb(113, 112, 112)), Width - 3, 3, 1, 1)
        G.FillRectangle(GetBrush(Color.FromArgb(83, 82, 82)), Width - 2, 4, 1, 1)

        '// Shadows
        G.DrawLine(GetPen(Color.FromArgb(91, 90, 89)), 1, 52, Width - 2, 52) 'Middle Top Horizontal
        G.DrawLine(GetPen(Color.FromArgb(40, 37, 34)), 11, 54, Width - 12, 54)
        G.DrawLine(GetPen(Color.FromArgb(45, 42, 39)), 11, 55, Width - 12, 55)
        G.DrawLine(GetPen(Color.FromArgb(50, 47, 44)), 11, 56, Width - 12, 56)

        G.DrawLine(GetPen(Color.FromArgb(50, 47, 44)), 11, Height - 32, Width - 12, Height - 32) 'Middle Bottom Horizontal
        G.DrawLine(GetPen(Color.FromArgb(52, 49, 46)), 11, Height - 33, Width - 12, Height - 33)
        G.DrawLine(GetPen(Color.FromArgb(54, 51, 48)), 11, Height - 34, Width - 12, Height - 34)


        G.DrawLine(GetPen(Color.FromArgb(59, 57, 55)), 1, 54, 9, 54) 'Left Horizontal
        G.DrawLine(GetPen(Color.FromArgb(64, 62, 60)), 1, 55, 9, 55)
        G.DrawLine(GetPen(Color.FromArgb(73, 71, 69)), 1, 56, 9, 56)

        G.DrawLine(GetPen(Color.FromArgb(59, 57, 55)), Width - 10, 54, Width - 2, 54) 'Right Horizontal
        G.DrawLine(GetPen(Color.FromArgb(64, 62, 60)), Width - 10, 55, Width - 2, 55)
        G.DrawLine(GetPen(Color.FromArgb(73, 71, 69)), Width - 10, 56, Width - 2, 56)

        G.DrawLine(GetPen(Color.FromArgb(59, 57, 55)), 1, 54, 1, Height - 5) 'Left Vertical
        G.DrawLine(GetPen(Color.FromArgb(64, 62, 60)), 2, 55, 2, Height - 4)
        G.DrawLine(GetPen(Color.FromArgb(73, 71, 69)), 3, 56, 3, Height - 3)
        G.DrawLine(New Pen(LeftHighlight), 1, 5, 1, 51)
        G.DrawLine(New Pen(RightHighlight), 2, 5, 2, 51)
        G.DrawLine(GetPen(Color.FromArgb(69, 67, 65)), 9, 56, 9, Height - 31)

        G.DrawLine(GetPen(Color.FromArgb(59, 57, 55)), Width - 2, 54, Width - 2, Height - 5) 'Right Vertical
        G.DrawLine(GetPen(Color.FromArgb(64, 62, 60)), Width - 3, 55, Width - 3, Height - 4)
        G.DrawLine(GetPen(Color.FromArgb(73, 71, 69)), Width - 4, 56, Width - 4, Height - 3)
        G.DrawLine(New Pen(LeftHighlight), Width - 2, 5, Width - 2, 51)
        G.DrawLine(New Pen(RightHighlight), Width - 3, 5, Width - 3, 51)
        G.DrawLine(GetPen(Color.FromArgb(69, 67, 65)), Width - 10, 56, Width - 10, Height - 31)

        G.DrawString(Text, drawFont, New SolidBrush(Color.FromArgb(255, 255, 255)), New Rectangle(0, 0, Width, 37), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})

        '////left upper corner
        G.FillRectangle(Brushes.Fuchsia, 0, 0, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, 1, 0, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, 2, 0, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, 3, 0, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, 0, 1, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, 0, 2, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, 0, 3, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, 1, 1, 1, 1)

        G.FillRectangle(Brushes.Black, 1, 3, 1, 1)
        G.FillRectangle(Brushes.Black, 1, 2, 1, 1)
        G.FillRectangle(Brushes.Black, 2, 1, 1, 1)
        G.FillRectangle(Brushes.Black, 3, 1, 1, 1)
        ''////right upper corner
        G.FillRectangle(Brushes.Fuchsia, Width - 1, 0, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, Width - 2, 0, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, Width - 3, 0, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, Width - 4, 0, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, Width - 1, 1, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, Width - 1, 2, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, Width - 1, 3, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, Width - 2, 1, 1, 1)

        G.FillRectangle(Brushes.Black, Width - 2, 3, 1, 1)
        G.FillRectangle(Brushes.Black, Width - 2, 2, 1, 1)
        G.FillRectangle(Brushes.Black, Width - 3, 1, 1, 1)
        G.FillRectangle(Brushes.Black, Width - 4, 1, 1, 1)
        ''////left bottom corner
        G.FillRectangle(Brushes.Fuchsia, 0, Height - 1, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, 0, Height - 2, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, 0, Height - 3, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, 0, Height - 4, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, 1, Height - 1, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, 2, Height - 1, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, 3, Height - 1, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, 1, Height - 1, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, 1, Height - 2, 1, 1)

        G.FillRectangle(Brushes.Black, 1, Height - 3, 1, 1)
        G.FillRectangle(Brushes.Black, 1, Height - 4, 1, 1)
        G.FillRectangle(Brushes.Black, 3, Height - 2, 1, 1)
        G.FillRectangle(Brushes.Black, 2, Height - 2, 1, 1)
        ''////right bottom corner
        G.FillRectangle(Brushes.Fuchsia, Width - 1, Height, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, Width - 2, Height, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, Width - 3, Height, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, Width - 4, Height, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, Width - 1, Height - 1, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, Width - 1, Height - 2, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, Width - 1, Height - 3, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, Width - 2, Height - 1, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, Width - 3, Height - 1, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, Width - 4, Height - 1, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, Width - 1, Height - 4, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, Width - 2, Height - 2, 1, 1)

        G.FillRectangle(Brushes.Black, Width - 2, Height - 3, 1, 1)
        G.FillRectangle(Brushes.Black, Width - 2, Height - 4, 1, 1)
        G.FillRectangle(Brushes.Black, Width - 4, Height - 2, 1, 1)
        G.FillRectangle(Brushes.Black, Width - 3, Height - 2, 1, 1)

    End Sub
    Private MouseP As Point = New Point(0, 0)
    Private Cap As Boolean = False
    Private MoveHeight% = 53 : Private pos% = 0
    Dim State As MouseState = MouseState.None
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        If e.Button = Windows.Forms.MouseButtons.Left And New Rectangle(0, 0, Width, MoveHeight).Contains(e.Location) Then
            Cap = True
            MouseP = e.Location
        End If
        State = MouseState.Down
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseEnter(ByVal e As System.EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(ByVal e As System.EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseUp(e) : Cap = False
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(e)
        If Cap Then
            Parent.Location = MousePosition - MouseP
        End If
        Invalidate()
    End Sub
    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        Me.ParentForm.FormBorderStyle = FormBorderStyle.None
        Me.ParentForm.TransparencyKey = Color.Fuchsia
        Dock = DockStyle.Fill
    End Sub
End Class
Public Class xVisualControlBox : Inherits Control
    Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        ForeColor = Color.FromArgb(205, 205, 205)
        Size = New Size(83, 28)
        Location = New Point(12, 12)
        DoubleBuffered = True
    End Sub
#Region " MouseStates "
    Dim State As MouseState = MouseState.None
    Dim X As Integer
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)

        If X > 10 And X < 25 Then
            FindForm.Close()
        ElseIf X > 33 And X < 48 Then
            FindForm.WindowState = FormWindowState.Minimized
        ElseIf X > 56 And X < 71 Then
            If FindForm.WindowState = FormWindowState.Normal Then
                FindForm.WindowState = FormWindowState.Maximized
            Else
                FindForm.WindowState = FormWindowState.Normal
            End If
        End If
        State = MouseState.Down : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseEnter(ByVal e As System.EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(ByVal e As System.EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(e)
        X = e.Location.X
        Invalidate()
    End Sub
#End Region
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)

        MyBase.OnPaint(e)

        G.Clear(BackColor)

        G.SmoothingMode = SmoothingMode.HighQuality

        Dim ControlGradient As New LinearGradientBrush(New Rectangle(10, 2, 15, 16), Color.FromArgb(67, 67, 67), Color.FromArgb(80, 80, 81), 90S)
        '// Control Box
        G.FillEllipse(ControlGradient, New Rectangle(10, 2, 15, 16)) 'Main Circle
        G.FillEllipse(ControlGradient, New Rectangle(33, 2, 15, 16))
        G.FillEllipse(ControlGradient, New Rectangle(56, 2, 15, 16))
        G.DrawEllipse(Pens.Black, New Rectangle(10, 2, 15, 16))
        G.DrawEllipse(Pens.Black, New Rectangle(33, 2, 15, 16))
        G.DrawEllipse(Pens.Black, New Rectangle(56, 2, 15, 16))

        Dim ControlTopCircle As New LinearGradientBrush(New Rectangle(13, 4, 9, 7), Color.FromArgb(193, 190, 176), Color.FromArgb(90, 91, 92), 90S)
        G.FillEllipse(ControlTopCircle, New Rectangle(13, 4, 9, 7)) 'Top Circle
        G.FillEllipse(ControlTopCircle, New Rectangle(36, 4, 9, 7)) 'Top Circle
        G.FillEllipse(ControlTopCircle, New Rectangle(59, 4, 9, 7)) 'Top Circle

        Dim NControlBottomCircle As New LinearGradientBrush(New Rectangle(13, 12, 9, 5), Color.FromArgb(90, 91, 92), Color.FromArgb(155, 165, 174), 90S)

        Select Case State
            Case MouseState.None
None:           G.FillEllipse(NControlBottomCircle, New Rectangle(13, 12, 9, 5)) 'Bottom Circle
                G.FillEllipse(NControlBottomCircle, New Rectangle(36, 12, 9, 5))
                G.FillEllipse(NControlBottomCircle, New Rectangle(59, 12, 9, 5))
            Case MouseState.Over
                If X > 10 And X < 25 Then
                    Dim ControlBottomCircle As New LinearGradientBrush(New Rectangle(13, 12, 9, 5), Color.FromArgb(50, Color.Red), Color.FromArgb(10, Color.Red), 90S)
                    G.FillEllipse(NControlBottomCircle, New Rectangle(13, 12, 9, 5)) 'Bottom Circle
                    G.FillEllipse(ControlBottomCircle, New Rectangle(13, 12, 9, 5)) 'Bottom Circle
                    G.FillEllipse(NControlBottomCircle, New Rectangle(36, 12, 9, 5))
                    G.FillEllipse(NControlBottomCircle, New Rectangle(59, 12, 9, 5))
                ElseIf X > 33 And X < 48 Then
                    Dim ControlBottomCircle As New LinearGradientBrush(New Rectangle(13, 12, 9, 5), Color.FromArgb(50, Color.Yellow), Color.FromArgb(10, Color.Yellow), 90S)
                    G.FillEllipse(NControlBottomCircle, New Rectangle(13, 12, 9, 5)) 'Bottom Circle
                    G.FillEllipse(NControlBottomCircle, New Rectangle(36, 12, 9, 5))
                    G.FillEllipse(ControlBottomCircle, New Rectangle(36, 12, 9, 5))
                    G.FillEllipse(NControlBottomCircle, New Rectangle(59, 12, 9, 5))
                ElseIf X > 56 And X < 71 Then
                    Dim ControlBottomCircle As New LinearGradientBrush(New Rectangle(13, 12, 9, 5), Color.FromArgb(50, Color.Green), Color.FromArgb(10, Color.Green), 90S)
                    G.FillEllipse(NControlBottomCircle, New Rectangle(13, 12, 9, 5)) 'Bottom Circle
                    G.FillEllipse(NControlBottomCircle, New Rectangle(36, 12, 9, 5))
                    G.FillEllipse(NControlBottomCircle, New Rectangle(59, 12, 9, 5))
                    G.FillEllipse(ControlBottomCircle, New Rectangle(59, 12, 9, 5))
                Else
                    GoTo None
                End If
            Case MouseState.Down

        End Select


        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class
Public Class xVisualButton : Inherits Control
#Region " MouseStates "
    Dim State As MouseState = MouseState.None
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseEnter(ByVal e As System.EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(ByVal e As System.EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None : Invalidate()
    End Sub
#End Region

    Public Enum InnerShade
        Light
        Dark
    End Enum
    Private _Shade As InnerShade
    Property Shade As InnerShade
        Get
            Return _Shade
        End Get
        Set(ByVal value As InnerShade)
            _Shade = value
            Invalidate()
        End Set
    End Property

    Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        ForeColor = Color.FromArgb(205, 205, 205)
        _Shade = InnerShade.Dark
        DoubleBuffered = True
    End Sub
    Dim InnerTexture As TextureBrush = NoiseBrush({Color.FromArgb(52, 48, 44), Color.FromArgb(54, 50, 46), Color.FromArgb(50, 46, 42)})
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim ClientRectangle As New Rectangle(3, 3, Width - 7, Height - 7)

        MyBase.OnPaint(e)

        G.Clear(BackColor)

        G.SmoothingMode = SmoothingMode.HighQuality

        Select Case _Shade
            Case InnerShade.Dark

                G.FillPath(InnerTexture, Draw.CreateRound(ClientRectangle, 3))

                G.DrawPath(GetPen(Color.FromArgb(40, 38, 36)), Draw.CreateRound(New Rectangle(3, 3, Width - 6, Height - 6), 3))
                G.DrawPath(GetPen(Color.FromArgb(45, 43, 41)), Draw.CreateRound(New Rectangle(3, 3, Width - 6, Height - 5), 3))
                G.DrawPath(GetPen(Color.FromArgb(50, 48, 46)), Draw.CreateRound(New Rectangle(2, 2, Width - 5, Height - 3), 3))

                Dim HighlightGradient As New LinearGradientBrush(New Rectangle(4, 4, Width - 8, Height - 8), Color.FromArgb(160, 158, 157), Color.FromArgb(61, 57, 54), 90S)
                Dim hp As New Pen(HighlightGradient)
                G.DrawPath(hp, Draw.CreateRound(New Rectangle(4, 4, Width - 9, Height - 9), 3))

                Dim OutlineGradient As New LinearGradientBrush(New Rectangle(3, 3, Width - 7, Height - 6), Color.FromArgb(34, 32, 30), Color.Black, 90S)
                Dim op As New Pen(OutlineGradient)
                G.DrawPath(op, Draw.CreateRound(New Rectangle(3, 3, Width - 7, Height - 7), 3))

                Dim drawFont As New Font("Arial", 9, FontStyle.Bold)
                Select Case State
                    Case MouseState.None
                        G.DrawString(Text, drawFont, Brushes.White, New Rectangle(0, 0, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                    Case MouseState.Over
                        G.DrawString(Text, drawFont, Brushes.White, New Rectangle(0, 0, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                    Case MouseState.Down
                        G.DrawString(Text, drawFont, Brushes.White, New Rectangle(0, 0, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                End Select

            Case InnerShade.Light

                Dim MainGradient As New LinearGradientBrush(ClientRectangle, Color.FromArgb(225, 227, 230), Color.FromArgb(199, 201, 204), 90S)
                G.FillPath(MainGradient, Draw.CreateRound(ClientRectangle, 3))

                G.DrawPath(GetPen(Color.FromArgb(167, 168, 171)), Draw.CreateRound(New Rectangle(3, 3, Width - 6, Height - 6), 3))
                G.DrawPath(GetPen(Color.FromArgb(203, 205, 208)), Draw.CreateRound(New Rectangle(2, 2, Width - 5, Height - 4), 3))

                Dim HighlightGradient As New LinearGradientBrush(New Rectangle(4, 4, Width - 8, Height - 8), Color.FromArgb(255, 255, 255), Color.FromArgb(218, 219, 222), 90S)
                Dim hp As New Pen(HighlightGradient)
                G.DrawPath(hp, Draw.CreateRound(New Rectangle(4, 4, Width - 9, Height - 9), 3))

                Dim OutlineGradient As New LinearGradientBrush(New Rectangle(3, 3, Width - 7, Height - 6), Color.FromArgb(173, 174, 177), Color.FromArgb(110, 111, 114), 90S)
                Dim op As New Pen(OutlineGradient)
                G.DrawPath(op, Draw.CreateRound(New Rectangle(3, 3, Width - 7, Height - 7), 3))

                Dim drawFont As New Font("Arial", 9, FontStyle.Bold)
                Select Case State
                    Case MouseState.None
                        G.DrawString(Text, drawFont, GetBrush(Color.FromArgb(109, 109, 110)), New Rectangle(0, 0, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                    Case MouseState.Over
                        G.DrawString(Text, drawFont, GetBrush(Color.FromArgb(109, 109, 110)), New Rectangle(0, 0, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                    Case MouseState.Down
                        G.DrawString(Text, drawFont, GetBrush(Color.FromArgb(109, 109, 110)), New Rectangle(0, 0, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                End Select

        End Select


        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class
Public Class xVisualGroupBox : Inherits ContainerControl

    Public Enum InnerShade
        Light
        Dark
    End Enum
    Private _Shade As InnerShade
    Property Shade As InnerShade
        Get
            Return _Shade
        End Get
        Set(ByVal value As InnerShade)
            _Shade = value
            Invalidate()
        End Set
    End Property

    Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        ForeColor = Color.FromArgb(205, 205, 205)
        Size = New Size(174, 115)
        _Shade = InnerShade.Light
        DoubleBuffered = True
    End Sub
    Dim TopTexture As TextureBrush = NoiseBrush({Color.FromArgb(49, 45, 41), Color.FromArgb(51, 47, 43), Color.FromArgb(47, 43, 39)})
    Dim InnerTexture As TextureBrush = NoiseBrush({Color.FromArgb(55, 52, 48), Color.FromArgb(57, 50, 50), Color.FromArgb(53, 50, 46)})
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim ClientRectangle As New Rectangle(0, 0, Width - 1, Height - 1)
        Dim BarRect As New Rectangle(0, 0, Width - 1, 32)
        MyBase.OnPaint(e)

        G.Clear(BackColor)

        G.SmoothingMode = SmoothingMode.HighQuality

        Select Case _Shade
            Case InnerShade.Light
                Dim mainGradient As New LinearGradientBrush(ClientRectangle, Color.FromArgb(228, 230, 232), Color.FromArgb(199, 201, 205), 90S)
                G.FillRectangle(mainGradient, ClientRectangle)
                G.DrawRectangle(Pens.Black, ClientRectangle)
            Case InnerShade.Dark
                G.FillRectangle(InnerTexture, ClientRectangle)
                G.DrawRectangle(Pens.Black, ClientRectangle)
        End Select

        Dim TopOverlay As New LinearGradientBrush(ClientRectangle, Color.FromArgb(5, Color.White), Color.FromArgb(10, Color.White), 90S)
        G.FillRectangle(TopTexture, BarRect)

        Dim blend As ColorBlend = New ColorBlend()

        'Add the Array of Color
        Dim bColors As Color() = New Color() {Color.FromArgb(20, Color.White), Color.FromArgb(10, Color.Black), Color.FromArgb(10, Color.White)}
        blend.Colors = bColors

        'Add the Array Single (0-1) colorpoints to place each Color
        Dim bPts As Single() = New Single() {0, 0.9, 1}
        blend.Positions = bPts

        Using br As New LinearGradientBrush(BarRect, Color.White, Color.Black, LinearGradientMode.Vertical)

            'Blend the colors into the Brush
            br.InterpolationColors = blend

            'Fill the rect with the blend
            G.FillRectangle(br, BarRect)

        End Using

        G.DrawRectangle(Pens.Black, BarRect)

        '// Top Bar Highlights
        G.DrawLine(GetPen(Color.FromArgb(112, 109, 107)), 1, 1, Width - 2, 1)
        G.DrawLine(GetPen(Color.FromArgb(67, 63, 60)), 1, BarRect.Height - 1, Width - 2, BarRect.Height - 1)

        Select Case _Shade
            Case InnerShade.Light
                Dim c As Color() = {Color.FromArgb(153, 153, 153), Color.FromArgb(173, 174, 177), Color.FromArgb(200, 201, 204)}
                Draw.InnerGlow(G, New Rectangle(1, 33, Width - 2, Height - 34), c)
            Case InnerShade.Dark
                Dim c As Color() = {Color.FromArgb(43, 40, 38), Color.FromArgb(50, 47, 44), Color.FromArgb(55, 52, 49)}
                Draw.InnerGlow(G, New Rectangle(1, 33, Width - 2, Height - 34), c)
        End Select

        Dim drawFont As New Font("Arial", 9, FontStyle.Bold)
        G.DrawString(Text, drawFont, Brushes.White, New Rectangle(15, 3, Width - 1, 26), New StringFormat() With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class
Public Class xVisualHeader : Inherits ContainerControl
    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        Height = 32
        MyBase.OnResize(e)
    End Sub

    Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        ForeColor = Color.FromArgb(205, 205, 205)
        Size = New Size(174, 32)
        DoubleBuffered = True
    End Sub
    Dim TopTexture As TextureBrush = NoiseBrush({Color.FromArgb(49, 45, 41), Color.FromArgb(51, 47, 43), Color.FromArgb(47, 43, 39)})
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim BarRect As New Rectangle(0, 0, Width - 1, Height - 1)
        MyBase.OnPaint(e)

        G.Clear(BackColor)

        G.SmoothingMode = SmoothingMode.HighQuality

        Dim TopOverlay As New LinearGradientBrush(BarRect, Color.FromArgb(5, Color.White), Color.FromArgb(10, Color.White), 90S)
        G.FillRectangle(TopTexture, BarRect)

        Dim blend As ColorBlend = New ColorBlend()

        'Add the Array of Color
        Dim bColors As Color() = New Color() {Color.FromArgb(20, Color.White), Color.FromArgb(10, Color.Black), Color.FromArgb(10, Color.White)}
        blend.Colors = bColors

        'Add the Array Single (0-1) colorpoints to place each Color
        Dim bPts As Single() = New Single() {0, 0.9, 1}
        blend.Positions = bPts

        Using br As New LinearGradientBrush(BarRect, Color.White, Color.Black, LinearGradientMode.Vertical)

            'Blend the colors into the Brush
            br.InterpolationColors = blend

            'Fill the rect with the blend
            G.FillRectangle(br, BarRect)

        End Using

        G.DrawRectangle(Pens.Black, BarRect)

        '// Top Bar Highlights
        G.DrawLine(GetPen(Color.FromArgb(112, 109, 107)), 1, 1, Width - 2, 1)
        G.DrawLine(GetPen(Color.FromArgb(67, 63, 60)), 1, BarRect.Height - 1, Width - 2, BarRect.Height - 1)


        Dim drawFont As New Font("Arial", 9, FontStyle.Bold)
        G.DrawString(Text, drawFont, Brushes.White, New Rectangle(15, 3, Width - 1, 26), New StringFormat() With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class
Public Class xVisualSeperator : Inherits Control

    Public Enum LineStyle
        Horizontal
        Vertical
    End Enum
    Private _Style As LineStyle
    Property Style As LineStyle
        Get
            Return _Style
        End Get
        Set(ByVal value As LineStyle)
            _Style = value
            Invalidate()
        End Set
    End Property

    Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        ForeColor = Color.FromArgb(205, 205, 205)
        _Style = LineStyle.Horizontal

        Size = New Size(174, 3)

        DoubleBuffered = True
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        MyBase.OnPaint(e)

        Size = Size
        _Style = Style

        G.Clear(BackColor)

        G.SmoothingMode = SmoothingMode.HighQuality

        Select Case _Style
            Case LineStyle.Horizontal
                G.DrawLine(GetPen(Color.Black), 0, 0, Width - 1, Height - 3)
                G.DrawLine(GetPen(Color.FromArgb(99, 97, 94)), 0, 1, Width - 1, Height - 2)
            Case LineStyle.Vertical
                G.DrawLine(GetPen(Color.Black), 0, 0, 0, Height - 1)
                G.DrawLine(GetPen(Color.FromArgb(99, 97, 94)), 1, 0, 1, Height - 1)
        End Select

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class
<DefaultEvent("TextChanged")> Public Class xVisualTextBox : Inherits Control

#Region " Variables"

    Private W, H As Integer
    Private State As MouseState = MouseState.None
    Private WithEvents TB As Windows.Forms.TextBox

#End Region

#Region " Properties"

#Region " TextBox Properties"

    Private _TextAlign As HorizontalAlignment = HorizontalAlignment.Left
    Public Enum RoundingStyle
        Normal
        Rounded
    End Enum
    Private _Style As RoundingStyle
    <Category("Options")> _
    Property Style() As RoundingStyle
        Get
            Return _Style
        End Get
        Set(ByVal value As RoundingStyle)
            _Style = value
            If TB IsNot Nothing Then
                TB.TextAlign = value
            End If
        End Set
    End Property

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
        TB.Location = New Point(11, 5)
        TB.Width = Width - 14

        If _Multiline Then
            TB.Height = Height - 11
        Else
            Height = TB.Height + 11
        End If

        MyBase.OnResize(e)
    End Sub

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
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None : Invalidate()
    End Sub

#End Region

#End Region

#Region " Colors"

    Private _BaseColor As Color = Color.FromArgb(242, 242, 242)
    Private _TextColor As Color = Color.FromArgb(30, 30, 30)

#End Region

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                 ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or _
                 ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True

        BackColor = Color.Transparent

        TB = New Windows.Forms.TextBox
        TB.Font = New Font("Arial", 8, FontStyle.Bold)
        TB.Text = Text
        TB.BackColor = _BaseColor
        TB.ForeColor = _TextColor
        TB.MaxLength = _MaxLength
        TB.Multiline = _Multiline
        TB.ReadOnly = _ReadOnly
        TB.UseSystemPasswordChar = _UseSystemPasswordChar
        TB.BorderStyle = BorderStyle.None
        TB.Location = New Point(11, 5)
        TB.Width = Width - 10
        _Style = RoundingStyle.Normal

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
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        G = Graphics.FromImage(B)

        W = Width - 1 : H = Height - 1

        Dim Base As New Rectangle(0, 0, W, H)

        With G
            .SmoothingMode = SmoothingMode.HighQuality
            .Clear(BackColor)

            TB.BackColor = _BaseColor
            TB.ForeColor = _TextColor

            Select Case _Style
                Case RoundingStyle.Normal
                    .FillPath(New SolidBrush(_BaseColor), Draw.CreateRound(Base, 5))
                    Dim tg As New LinearGradientBrush(Base, Color.FromArgb(186, 188, 191), Color.FromArgb(204, 205, 209), 90S)
                    .DrawPath(New Pen(tg), Draw.CreateRound(Base, 5))
                Case RoundingStyle.Rounded
                    .DrawPath(New Pen(New SolidBrush(Color.FromArgb(132, 130, 128))), Draw.CreateRound(New Rectangle(Base.X, Base.Y + 1, Base.Width, Base.Height - 1), 20))

                    .FillPath(New SolidBrush(_BaseColor), Draw.CreateRound(New Rectangle(Base.X, Base.Y, Base.Width, Base.Height - 1), 20))
                    Dim tg As New LinearGradientBrush(New Rectangle(Base.X, Base.Y, Base.Width, Base.Height - 1), Color.Black, Color.FromArgb(31, 28, 24), 90S)
                    .DrawPath(New Pen(tg), Draw.CreateRound(New Rectangle(Base.X, Base.Y, Base.Width, Base.Height - 1), 20))
            End Select
        End With

        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub

End Class
Public Class xVisualProgressBar : Inherits Control

#Region " Control Help - Properties & Flicker Control "
    Private OFS As Integer = 0
    Private Speed As Integer = 50
    Private _Maximum As Integer = 100

    Public Property Maximum() As Integer
        Get
            Return _Maximum
        End Get
        Set(ByVal v As Integer)
            Select Case v
                Case Is < _Value
                    _Value = v
            End Select
            _Maximum = v
            Invalidate()
        End Set
    End Property
    Private _Value As Integer = 0
    Public Property Value() As Integer
        Get
            Select Case _Value
                Case 0
                    Return 0
                Case Else
                    Return _Value
            End Select
        End Get
        Set(ByVal v As Integer)
            Select Case v
                Case Is > _Maximum
                    v = _Maximum
            End Select
            _Value = v
            Invalidate()
        End Set
    End Property
    Private _ShowPercentage As Boolean = False
    Public Property ShowPercentage() As Boolean
        Get
            Return _ShowPercentage
        End Get
        Set(ByVal v As Boolean)
            _ShowPercentage = v
            Invalidate()
        End Set
    End Property

    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
    End Sub
    Sub Animate()
        While True
            If OFS <= Width Then : OFS += 1
            Else : OFS = 0
            End If
            Invalidate()
            Threading.Thread.Sleep(Speed)
        End While
    End Sub
#End Region

    Sub New()
        MyBase.New()
        DoubleBuffered = True
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        Size = New Size(274, 30)
    End Sub

    Dim InnerTexture As TextureBrush = NoiseBrush({Color.FromArgb(55, 52, 48), Color.FromArgb(57, 50, 50), Color.FromArgb(53, 50, 46)})
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)

        'G.SmoothingMode = SmoothingMode.HighQuality

        Dim intValue As Integer = CInt(_Value / _Maximum * Width)
        G.Clear(BackColor)

        Dim percentColor As SolidBrush = New SolidBrush(Color.White)

        G.FillRectangle(InnerTexture, New Rectangle(0, 0, Width - 1, Height - 1))

        Dim blend As ColorBlend = New ColorBlend()

        'Add the Array of Color
        Dim bColors As Color() = New Color() {Color.FromArgb(20, Color.White), Color.FromArgb(10, Color.Black), Color.FromArgb(10, Color.White)}
        blend.Colors = bColors

        'Add the Array Single (0-1) colorpoints to place each Color
        Dim bPts As Single() = New Single() {0, 0.8, 1}
        blend.Positions = bPts

        Using br As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.White, Color.Black, LinearGradientMode.Vertical)

            'Blend the colors into the Brush
            br.InterpolationColors = blend

            'Fill the rect with the blend
            G.FillRectangle(br, New Rectangle(0, 0, Width - 1, Height - 1))

        End Using

        G.DrawRectangle(Pens.Black, New Rectangle(0, 0, Width - 1, Height - 1))
        G.DrawLine(GetPen(Color.FromArgb(99, 97, 94)), 1, 1, Width - 3, 1)
        G.DrawLine(GetPen(Color.FromArgb(64, 60, 57)), 1, Height - 2, Width - 3, Height - 2)

        '//// Bar Fill
        If Not intValue = 0 Then
            G.FillRectangle(New LinearGradientBrush(New Rectangle(2, 2, intValue - 3, Height - 4), Color.FromArgb(114, 203, 232), Color.FromArgb(58, 118, 188), 90S), New Rectangle(2, 2, intValue - 3, Height - 4))
            G.DrawLine(GetPen(Color.FromArgb(235, 255, 255)), 2, 2, intValue - 2, 2)
            'G.DrawLine(GetPen(Color.FromArgb(27, 25, 23)), 2, Height - 2, intValue + 1, Height - 2)
            percentColor = New SolidBrush(Color.White)
        End If

        If _ShowPercentage Then
            G.DrawString(Convert.ToString(String.Concat(Value, "%")), New Font("Arial", 10, FontStyle.Bold), GetBrush(Color.FromArgb(20, 20, 20)), New Rectangle(1, 2, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
            G.DrawString(Convert.ToString(String.Concat(Value, "%")), New Font("Arial", 10, FontStyle.Bold), percentColor, New Rectangle(0, 1, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
        End If

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class
<DefaultEvent("CheckedChanged")> Public Class xVisualRadioButton : Inherits Control

#Region " Control Help - MouseState & Flicker Control"
    Private R1 As Rectangle
    Private G1 As LinearGradientBrush

    Private State As MouseState = MouseState.None
    Protected Overrides Sub OnMouseEnter(ByVal e As System.EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(ByVal e As System.EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over
        Invalidate()
    End Sub
    Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
        MyBase.OnResize(e)
        Height = 21
    End Sub
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub
    Private _Checked As Boolean
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
    Event CheckedChanged(ByVal sender As Object)
    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        InvalidateControls()
    End Sub
    Private Sub InvalidateControls()
        If Not IsHandleCreated OrElse Not _Checked Then Return

        For Each C As Control In Parent.Controls
            If C IsNot Me AndAlso TypeOf C Is xVisualRadioButton Then
                DirectCast(C, xVisualRadioButton).Checked = False
            End If
        Next
    End Sub
#End Region

    Sub New()
        MyBase.New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        ForeColor = Color.Black
        Size = New Size(150, 21)
        DoubleBuffered = True
    End Sub

    Dim InnerTexture As TextureBrush = NoiseBrush({Color.FromArgb(55, 52, 48), Color.FromArgb(57, 50, 50), Color.FromArgb(53, 50, 46)})
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim radioBtnRectangle As New Rectangle(0, 0, Height - 1, Height - 1)

        G.SmoothingMode = SmoothingMode.HighQuality
        G.CompositingQuality = CompositingQuality.HighQuality
        G.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit

        G.Clear(BackColor)

        G.FillRectangle(InnerTexture, radioBtnRectangle)
        G.DrawRectangle(New Pen(Color.Black), radioBtnRectangle)
        G.DrawRectangle(New Pen(Color.FromArgb(99, 97, 94)), New Rectangle(1, 1, Height - 3, Height - 3))

        If Checked Then
            G.DrawString("a", New Font("Marlett", 12, FontStyle.Regular), Brushes.White, New Point(1, 2))
        End If

        Dim drawFont As New Font("Arial", 10, FontStyle.Bold)
        Dim nb As Brush = New SolidBrush(Color.FromArgb(250, 250, 250))
        G.DrawString(Text, drawFont, nb, New Point(25, 10), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub

End Class
Public Class xVisualComboBox : Inherits ComboBox
#Region " Control Help - Properties & Flicker Control "
    Private _StartIndex As Integer = 0
    Public Property StartIndex As Integer
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
    Public Overrides ReadOnly Property DisplayRectangle As System.Drawing.Rectangle
        Get
            Return MyBase.DisplayRectangle
        End Get
    End Property
    Sub ReplaceItem(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles Me.DrawItem
        e.DrawBackground()
        Try
            If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
                e.Graphics.FillRectangle(New SolidBrush(_highlightColor), e.Bounds)
                Dim gloss As New LinearGradientBrush(e.Bounds, Color.FromArgb(20, Color.White), Color.FromArgb(0, Color.White), 90S) 'Highlight Gloss/Color
                e.Graphics.FillRectangle(gloss, New Rectangle(New Point(e.Bounds.X, e.Bounds.Y), New Size(e.Bounds.Width, e.Bounds.Height))) 'Drop Background
                e.Graphics.DrawRectangle(New Pen(Color.FromArgb(90, Color.Black)) With {.DashStyle = DashStyle.Solid}, New Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1))
            Else
                e.Graphics.FillRectangle(InnerTexture, e.Bounds)
            End If
            Using b As New SolidBrush(Color.FromArgb(230, 230, 230))
                e.Graphics.DrawString(MyBase.GetItemText(MyBase.Items(e.Index)), e.Font, b, New Rectangle(e.Bounds.X + 2, e.Bounds.Y, e.Bounds.Width - 4, e.Bounds.Height))
            End Using
        Catch
        End Try
        e.DrawFocusRectangle()
    End Sub
    Protected Sub DrawTriangle(ByVal Clr As Color, ByVal FirstPoint As Point, ByVal SecondPoint As Point, ByVal ThirdPoint As Point, ByVal G As Graphics)
        Dim points As New List(Of Point)()
        points.Add(FirstPoint)
        points.Add(SecondPoint)
        points.Add(ThirdPoint)
        G.FillPolygon(New SolidBrush(Clr), points.ToArray)
        G.DrawPolygon(New Pen(New SolidBrush(Color.Black)), points.ToArray)
    End Sub
    Private _highlightColor As Color = Color.FromArgb(99, 97, 94)
    Public Property ItemHighlightColor() As Color
        Get
            Return _highlightColor
        End Get
        Set(ByVal v As Color)
            _highlightColor = v
            Invalidate()
        End Set
    End Property
#End Region

    Sub New()
        MyBase.New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.DoubleBuffer Or ControlStyles.SupportsTransparentBackColor, True)
        DrawMode = Windows.Forms.DrawMode.OwnerDrawFixed
        BackColor = Color.Transparent
        ForeColor = Color.Silver
        Font = New Font("Arial", 9, FontStyle.Bold)
        DropDownStyle = ComboBoxStyle.DropDownList
        DoubleBuffered = True
        Size = New Size(Width + 1, 21)
        ItemHeight = 16
    End Sub

    Dim InnerTexture As TextureBrush = NoiseBrush({Color.FromArgb(55, 52, 48), Color.FromArgb(57, 50, 50), Color.FromArgb(53, 50, 46)})
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        G.SmoothingMode = SmoothingMode.HighQuality


        G.Clear(BackColor)
        G.FillRectangle(InnerTexture, New Rectangle(0, 0, Width, Height - 1))
        G.DrawLine(New Pen(Color.FromArgb(99, 97, 94)), 1, 1, Width - 2, 1)
        G.DrawRectangle(New Pen(Color.FromArgb(99, 97, 94)), New Rectangle(1, 1, Width - 3, Height - 3))

        DrawTriangle(Color.FromArgb(99, 97, 94), New Point(Width - 14, 9), New Point(Width - 6, 9), New Point(Width - 10, 14), G) 'Triangle Fill Color
        G.DrawRectangle(Pens.Black, New Rectangle(0, 0, Width - 1, Height - 1))

        'Draw Separator line
        G.DrawLine(New Pen(Color.FromArgb(99, 97, 94)), New Point(Width - 21, 1), New Point(Width - 21, Height - 3))
        G.DrawLine(New Pen(Color.Black), New Point(Width - 20, 2), New Point(Width - 20, Height - 3))
        G.DrawLine(New Pen(Color.FromArgb(99, 97, 94)), New Point(Width - 19, 1), New Point(Width - 19, Height - 3))

        Dim blend As ColorBlend = New ColorBlend()

        'Add the Array of Color
        Dim bColors As Color() = New Color() {Color.FromArgb(15, Color.White), Color.FromArgb(10, Color.Black), Color.FromArgb(10, Color.White)}
        blend.Colors = bColors

        'Add the Array Single (0-1) colorpoints to place each Color
        Dim bPts As Single() = New Single() {0, 0.75, 1}
        blend.Positions = bPts

        Using br As New LinearGradientBrush(New Rectangle(0, 0, Width, Height - 1), Color.White, Color.Black, LinearGradientMode.Vertical)

            'Blend the colors into the Brush
            br.InterpolationColors = blend

            'Fill the rect with the blend
            G.FillRectangle(br, New Rectangle(0, 0, Width, Height - 1))

        End Using
        Try
            G.DrawString(Text, Font, New SolidBrush(Color.FromArgb(250, 250, 250)), New Rectangle(5, 0, Width - 20, Height), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Near})
        Catch
        End Try

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class
Public Class xVisualTabControl : Inherits TabControl

    Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
        MyBase.OnResize(e)
    End Sub

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.DoubleBuffer, True)
        DoubleBuffered = True
        SizeMode = TabSizeMode.Fixed
        ItemSize = New Size(35, 122)
    End Sub
    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        Alignment = TabAlignment.Left
    End Sub

    Function ToPen(ByVal color As Color) As Pen
        Return New Pen(color)
    End Function

    Function ToBrush(ByVal color As Color) As Brush
        Return New SolidBrush(color)
    End Function
    Dim InnerTexture As TextureBrush = NoiseBrush({Color.FromArgb(45, 41, 37), Color.FromArgb(47, 43, 39), Color.FromArgb(43, 39, 35)})
    Dim TabBGTexture As TextureBrush = NoiseBrush({Color.FromArgb(55, 51, 48), Color.FromArgb(57, 53, 50), Color.FromArgb(53, 49, 46)})

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim FF As New Font("Arial", 9, FontStyle.Bold)

        Try : SelectedTab.BackColor = Color.FromArgb(56, 52, 49) : Catch : End Try
        G.Clear(Parent.FindForm.BackColor)

        G.FillRectangle(TabBGTexture, New Rectangle(0, 0, ItemSize.Height + 3, Height - 1)) 'Full Tab Background
        G.DrawLine(GetPen(Color.FromArgb(44, 42, 39)), 1, Height - 3, ItemSize.Height + 3, Height - 3)
        G.DrawLine(GetPen(Color.FromArgb(48, 45, 43)), 1, Height - 4, ItemSize.Height + 3, Height - 4)
        G.DrawLine(GetPen(Color.FromArgb(53, 50, 47)), 1, Height - 5, ItemSize.Height + 3, Height - 5)

        Dim y As Integer = GetTabRect(0).Height * 2
        Do Until y >= Height - 1
            G.DrawLine(Pens.Black, 1, y, Width - 2, y)
            G.DrawLine(GetPen(Color.FromArgb(99, 97, 94)), 1, y + 1, Width - 2, y + 1)
            y = y + GetTabRect(0).Height
        Loop

        For i = 0 To TabCount - 1
            If i = SelectedIndex Then
                Dim x2 As Rectangle = New Rectangle(New Point(GetTabRect(i).Location.X - 2, GetTabRect(i).Location.Y - 2), New Size(GetTabRect(i).Width + 3, GetTabRect(i).Height - 1))

                If SelectedIndex = 0 Then
                    Dim tabRect As New Rectangle(GetTabRect(i).Location.X, GetTabRect(i).Location.Y - 1, GetTabRect(i).Size.Width - 1, GetTabRect(i).Size.Height - 1)
                    Dim TabOverlay As New LinearGradientBrush(tabRect, Color.FromArgb(114, 203, 232), Color.FromArgb(58, 118, 188), 90S)
                    G.FillRectangle(TabOverlay, tabRect)

                    G.DrawLine(GetPen(Color.FromArgb(235, 255, 255)), GetTabRect(i).Location.X, GetTabRect(i).Location.Y - 1, GetTabRect(i).Size.Width, GetTabRect(i).Location.Y - 1)
                Else
                    Dim tabRect As New Rectangle(GetTabRect(i).Location.X, GetTabRect(i).Location.Y - 2, GetTabRect(i).Size.Width - 1, GetTabRect(i).Size.Height)
                    Dim TabOverlay As New LinearGradientBrush(tabRect, Color.FromArgb(114, 203, 232), Color.FromArgb(58, 118, 188), 90S)
                    G.FillRectangle(TabOverlay, tabRect)

                    G.DrawLine(GetPen(Color.FromArgb(235, 255, 255)), GetTabRect(i).Location.X, GetTabRect(i).Location.Y - 2, GetTabRect(i).Size.Width, GetTabRect(i).Location.Y - 2)
                End If

                G.DrawLine(Pens.Black, GetTabRect(i).Location.X, GetTabRect(i).Location.Y + 33, GetTabRect(i).Size.Width, GetTabRect(i).Location.Y + 33)

                G.SmoothingMode = SmoothingMode.HighQuality

                G.DrawString(TabPages(i).Text, FF, GetBrush(Color.FromArgb(20, 20, 20)), New Rectangle(x2.X, x2.Y - 1, x2.Width + 1, x2.Height + 2), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
                G.DrawString(TabPages(i).Text, FF, Brushes.White, New Rectangle(x2.X, x2.Y - 1, x2.Width, x2.Height), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})

                G.DrawLine(New Pen(Color.FromArgb(96, 110, 121)), New Point(x2.Location.X - 1, x2.Location.Y - 1), New Point(x2.Location.X, x2.Location.Y))
                G.DrawLine(New Pen(Color.FromArgb(96, 110, 121)), New Point(x2.Location.X - 1, x2.Bottom - 1), New Point(x2.Location.X, x2.Bottom))
            Else
                Dim x2 As Rectangle = New Rectangle(New Point(GetTabRect(i).Location.X - 2, GetTabRect(i).Location.Y - 2), New Size(GetTabRect(i).Width + 3, GetTabRect(i).Height - 1))
                Dim tabRect As New Rectangle(GetTabRect(i).Location.X, GetTabRect(i).Location.Y - 2, GetTabRect(i).Size.Width - 1, GetTabRect(i).Size.Height - 1)

                G.FillRectangle(InnerTexture, tabRect) 'Highlight Fill Background
                Dim TabOverlay As New LinearGradientBrush(tabRect, Color.FromArgb(15, Color.White), Color.FromArgb(100, Color.FromArgb(43, 40, 38)), 90S)
                G.FillRectangle(TabOverlay, tabRect)

                G.DrawLine(GetPen(Color.FromArgb(113, 110, 108)), GetTabRect(i).Location.X, GetTabRect(i).Location.Y - 1, GetTabRect(i).Size.Width, GetTabRect(i).Location.Y - 1)
                G.DrawLine(Pens.Black, GetTabRect(i).Location.X, GetTabRect(i).Location.Y + 32, GetTabRect(i).Size.Width, GetTabRect(i).Location.Y + 32)

                If i = TabCount - 1 Then
                    G.DrawLine(GetPen(Color.FromArgb(64, 60, 57)), GetTabRect(i).Location.X, GetTabRect(i).Location.Y + 31, GetTabRect(i).Size.Width, GetTabRect(i).Location.Y + 31)
                    G.DrawLine(GetPen(Color.FromArgb(35, 33, 31)), GetTabRect(i).Location.X, GetTabRect(i).Location.Y + 33, GetTabRect(i).Size.Width, GetTabRect(i).Location.Y + 33)
                    G.DrawLine(GetPen(Color.FromArgb(43, 41, 38)), GetTabRect(i).Location.X, GetTabRect(i).Location.Y + 34, GetTabRect(i).Size.Width, GetTabRect(i).Location.Y + 34)
                    G.DrawLine(GetPen(Color.FromArgb(53, 50, 47)), GetTabRect(i).Location.X, GetTabRect(i).Location.Y + 35, GetTabRect(i).Size.Width, GetTabRect(i).Location.Y + 35)
                    G.DrawLine(GetPen(Color.FromArgb(58, 55, 51)), GetTabRect(i).Location.X, GetTabRect(i).Location.Y + 36, GetTabRect(i).Size.Width, GetTabRect(i).Location.Y + 36)
                End If

                G.DrawString(TabPages(i).Text, FF, New SolidBrush(Color.FromArgb(210, 220, 230)), New Rectangle(x2.X, x2.Y - 1, x2.Width, x2.Height), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
            End If
            G.FillRectangle(New SolidBrush(Color.FromArgb(56, 52, 49)), New Rectangle(123, -1, Width - 123, Height + 1)) 'Page Fill Full

            G.DrawRectangle(Pens.Black, New Rectangle(123, 0, Width - 124, Height - 2))
            Dim c As Color() = {Color.FromArgb(43, 40, 38), Color.FromArgb(50, 47, 44), Color.FromArgb(55, 52, 49)}
            Draw.InnerGlow(G, New Rectangle(124, 1, Width - 125, Height - 3), c)
        Next

        G.DrawLine(GetPen(Color.FromArgb(56, 52, 49)), -1, Height - 1, ItemSize.Height + 1, Height - 1)
        G.DrawLine(GetPen(Color.FromArgb(56, 52, 49)), 0, -1, 0, Height - 1)
        G.DrawRectangle(New Pen(New SolidBrush(Color.Black)), New Rectangle(1, 0, ItemSize.Height, Height - 2)) 'Full Tab Inner Outline

        e.Graphics.DrawImage(B.Clone, 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class
<DefaultEvent("CheckedChanged")> Public Class xVisualCheckBox : Inherits Control

#Region " Control Help - MouseState & Flicker Control"
    Private State As MouseState = MouseState.None
    Protected Overrides Sub OnMouseEnter(ByVal e As System.EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(ByVal e As System.EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over
        Invalidate()
    End Sub
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub
    Private _Checked As Boolean
    Property Checked() As Boolean
        Get
            Return _Checked
        End Get
        Set(ByVal value As Boolean)
            _Checked = value
            Invalidate()
        End Set
    End Property
    Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
        MyBase.OnResize(e)
        Height = 21
    End Sub
    Protected Overrides Sub OnClick(ByVal e As System.EventArgs)
        _Checked = Not _Checked
        RaiseEvent CheckedChanged(Me)
        MyBase.OnClick(e)
    End Sub
    Event CheckedChanged(ByVal sender As Object)
#End Region


    Sub New()
        MyBase.New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor Or ControlStyles.OptimizedDoubleBuffer, True)
        BackColor = Color.Transparent
        ForeColor = Color.Black
        Size = New Size(250, 21)
        DoubleBuffered = True
    End Sub

    Dim InnerTexture As TextureBrush = NoiseBrush({Color.FromArgb(55, 52, 48), Color.FromArgb(57, 50, 50), Color.FromArgb(53, 50, 46)})
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim radioBtnRectangle As New Rectangle(0, 0, Height - 1, Height - 1)

        G.SmoothingMode = SmoothingMode.HighQuality
        G.CompositingQuality = CompositingQuality.HighQuality
        G.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit

        G.Clear(BackColor)

        G.FillRectangle(InnerTexture, radioBtnRectangle)
        G.DrawRectangle(New Pen(Color.Black), radioBtnRectangle)
        G.DrawRectangle(New Pen(Color.FromArgb(99, 97, 94)), New Rectangle(1, 1, Height - 3, Height - 3))

        If Checked Then
            G.DrawString("a", New Font("Marlett", 12, FontStyle.Regular), Brushes.White, New Point(1, 2))
        End If

        Dim drawFont As New Font("Arial", 10, FontStyle.Bold)
        Dim nb As Brush = New SolidBrush(Color.FromArgb(250, 250, 250))
        G.DrawString(Text, drawFont, nb, New Point(25, 10), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})
        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()

    End Sub

End Class