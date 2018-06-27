Imports System.Drawing.Drawing2D

'Creator: Aeonhack
'Site: **********
'Version: 1.0

Public Class Draw
    Shared Sub Gradient(ByVal g As Graphics, ByVal c1 As Color, ByVal c2 As Color, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
        Dim R As New Rectangle(x, y, width, height)
        Using T As New LinearGradientBrush(R, c1, c2, LinearGradientMode.Vertical)
            g.FillRectangle(T, R)
        End Using
    End Sub
    Shared Sub Blend(ByVal g As Graphics, ByVal c1 As Color, ByVal c2 As Color, ByVal c3 As Color, ByVal c As Single, ByVal d As Integer, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
        Dim V As New ColorBlend(3)
        V.Colors = New Color() {c1, c2, c3}
        V.Positions = New Single() {0, c, 1}
        Dim R As New Rectangle(x, y, width, height)
        Using T As New LinearGradientBrush(R, c1, c1, CType(d, LinearGradientMode))
            T.InterpolationColors = V : g.FillRectangle(T, R)
        End Using
    End Sub
End Class

Public Class VSTheme : Inherits Control
    Private _TitleHeight As Integer = 23
    Public Property TitleHeight() As Integer
        Get
            Return _TitleHeight
        End Get
        Set(ByVal v As Integer)
            If v > Height Then v = Height
            If v < 2 Then Height = 1
            _TitleHeight = v : Invalidate()
        End Set
    End Property
    Private _TitleAlign As HorizontalAlignment
    Public Property TitleAlign() As HorizontalAlignment
        Get
            Return _TitleAlign
        End Get
        Set(ByVal v As HorizontalAlignment)
            _TitleAlign = v : Invalidate()
        End Set
    End Property
    Sub New()
        Using B As New Bitmap(3, 3)
            Using G = Graphics.FromImage(B)
                G.Clear(Color.FromArgb(53, 67, 88))
                G.DrawLine(New Pen(Color.FromArgb(33, 46, 67)), 0, 0, 2, 2)
                Tile = B.Clone
            End Using
        End Using
    End Sub
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        Invalidate()
        MyBase.OnTextChanged(e)
    End Sub
    Protected Overrides Sub OnHandleCreated(ByVal e As System.EventArgs)
        Dock = 5
        If TypeOf Parent Is Form Then
            CType(Parent, Form).FormBorderStyle = 0
            CType(Parent, Form).TransparencyKey = Color.Fuchsia
        End If
        MyBase.OnHandleCreated(e)
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        If New Rectangle(Parent.Location.X, Parent.Location.Y, Width - 1, _TitleHeight - 1).IntersectsWith(New Rectangle(MousePosition.X, MousePosition.Y, 1, 1)) Then
            Capture = False : Dim M = Message.Create(Parent.Handle, 161, 2, 0) : DefWndProc(M)
        End If : MyBase.OnMouseDown(e)
    End Sub
    Dim C1 As Color = Color.FromArgb(249, 245, 226), C2 As Color = Color.FromArgb(255, 232, 165), C3 As Color = Color.FromArgb(64, 90, 127)
    Dim Tile As Image
    Protected Overrides Sub OnPaintBackground(ByVal pevent As System.Windows.Forms.PaintEventArgs)
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Using B As New Bitmap(Width, Height)
            Using G = Graphics.FromImage(B)

                Using T As New TextureBrush(Tile, 0)
                    G.FillRectangle(T, 0, _TitleHeight, Width, Height - _TitleHeight)
                End Using
                Draw.Blend(G, Color.Transparent, Color.Transparent, C3, 0.1, 1, 0, 0, Width, Height - _TitleHeight * 2)
                G.FillRectangle(New SolidBrush(C3), 0, Height - _TitleHeight * 2, Width, _TitleHeight * 2)

                Draw.Gradient(G, C1, C2, 0, 0, Width, _TitleHeight)
                G.FillRectangle(New SolidBrush(Color.FromArgb(100, 255, 255, 255)), 0, 0, Width, CInt(_TitleHeight / 2))

                G.DrawRectangle(New Pen(C2, 2), 1, _TitleHeight - 1, Width - 2, Height - _TitleHeight)
                G.DrawArc(New Pen(Color.Fuchsia, 2), -1, -1, 9, 9, 180, 90)
                G.DrawArc(New Pen(Color.Fuchsia, 2), Width - 9, -1, 9, 9, 270, 90)

                G.TextRenderingHint = 5
                Dim S = G.MeasureString(Text, Font), O = 6
                If _TitleAlign = 2 Then O = Width / 2 - S.Width / 2
                If _TitleAlign = 1 Then O = Width - S.Width - 6
                G.DrawString(Text, Font, New SolidBrush(Color.FromArgb(111, 88, 38)), O, CInt(_TitleHeight / 2 - S.Height / 2))

                e.Graphics.DrawImage(B.Clone, 0, 0)
            End Using
        End Using
    End Sub
End Class
Public Class VSButton : Inherits Control
    Sub New()
        ForeColor = C3
    End Sub
    Private State As Integer
    Protected Overrides Sub OnMouseEnter(ByVal e As System.EventArgs)
        State = 1 : Invalidate() : MyBase.OnMouseEnter(e)
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        State = 2 : Invalidate() : MyBase.OnMouseDown(e)
    End Sub
    Protected Overrides Sub OnMouseLeave(ByVal e As System.EventArgs)
        State = 0 : Invalidate() : MyBase.OnMouseLeave(e)
    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        State = 1 : Invalidate() : MyBase.OnMouseUp(e)
    End Sub
    Dim C1 As Color = Color.FromArgb(249, 245, 226), C2 As Color = Color.FromArgb(255, 232, 165), C3 As Color = Color.FromArgb(111, 88, 38)
    Protected Overrides Sub OnPaintBackground(ByVal pevent As System.Windows.Forms.PaintEventArgs)
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Using B As New Bitmap(Width, Height)
            Using G = Graphics.FromImage(B)
                If State = 2 Then
                    Draw.Gradient(G, C2, C1, 0, 0, Width, Height)
                Else
                    Draw.Gradient(G, C1, C2, 0, 0, Width, Height)
                End If

                If State < 2 Then G.FillRectangle(New SolidBrush(Color.FromArgb(100, 255, 255, 255)), 0, 0, Width, CInt(Height / 2))

                G.TextRenderingHint = 5
                Dim S = G.MeasureString(Text, Font)
                G.DrawString(Text, Font, New SolidBrush(ForeColor), Width / 2 - S.Width / 2, Height / 2 - S.Height / 2)
                G.DrawRectangle(New Pen(C1), 0, 0, Width - 1, Height - 1)

                e.Graphics.DrawImage(B.Clone, 0, 0)
            End Using
        End Using
    End Sub
End Class
