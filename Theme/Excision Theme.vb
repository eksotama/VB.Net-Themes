Imports System.Drawing.Drawing2D
Imports System.ComponentModel

'PLEASE LEAVE CREDITS IN SOURCE, DO NOT REDISTRIBUTE!

'--------------------- [ Theme ] --------------------
'Creator: Mephobia & Tedd
'Contact: Mephobia.HF (Skype)
'Created: 12.17.2012
'Changed: 12.17.2012
'-------------------- [ /Theme ] ---------------------

'PLEASE LEAVE CREDITS IN SOURCE, DO NOT REDISTRIBUTE!

Enum MouseState As Byte
    None = 0
    Over = 1
    Down = 2
    Block = 3
End Enum
Module Draw
    Public Function RoundRect(ByVal Rectangle As Rectangle, ByVal Curve As Integer) As GraphicsPath
        Dim P As GraphicsPath = New GraphicsPath()
        Dim ArcRectangleWidth As Integer = Curve * 2
        P.AddArc(New Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90)
        P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90)
        P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 0, 90)
        P.AddArc(New Rectangle(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 90, 90)
        P.AddLine(New Point(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y), New Point(Rectangle.X, Curve + Rectangle.Y))
        Return P
    End Function
    Public Function RoundRect(ByVal X As Integer, ByVal Y As Integer, ByVal Width As Integer, ByVal Height As Integer, ByVal Curve As Integer) As GraphicsPath
        Dim Rectangle As Rectangle = New Rectangle(X, Y, Width, Height)
        Dim P As GraphicsPath = New GraphicsPath()
        Dim ArcRectangleWidth As Integer = Curve * 2
        P.AddArc(New Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90)
        P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90)
        P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 0, 90)
        P.AddArc(New Rectangle(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 90, 90)
        P.AddLine(New Point(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y), New Point(Rectangle.X, Curve + Rectangle.Y))
        Return P
    End Function

End Module
Public Class ImageToCodeClass
    Public Function ImageToCode(ByVal Img As Bitmap) As String
        Return Convert.ToBase64String(DirectCast(System.ComponentModel.TypeDescriptor.GetConverter(Img).ConvertTo(Img, GetType(Byte())), Byte()))
    End Function

    Public Function CodeToImage(ByVal Code As String) As Image
        Return Image.FromStream(New System.IO.MemoryStream(Convert.FromBase64String(Code)))
    End Function
End Class
Public Class ExcisionTheme : Inherits ContainerControl
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
    Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.FromArgb(25, 25, 25)
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)

        Dim CR As New Rectangle(0, 0, Width - 1, 35)
        Dim Main As New Rectangle(0, 35, Width, Height - 35)

        MyBase.OnPaint(e)

        G.Clear(Color.Fuchsia)

        G.CompositingQuality = CompositingQuality.HighQuality

        Dim itc As New ImageToCodeClass
        Dim BG As Bitmap = itc.CodeToImage("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")
        Dim BGT As New TextureBrush(BG, WrapMode.TileFlipXY)
        G.FillRectangle(BGT, Main)


        Dim lbb As New LinearGradientBrush(CR, Color.FromArgb(68, 68, 68), Color.FromArgb(45, 45, 45), 90S)
        G.FillRectangle(lbb, CR)
        G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(26, 26, 26))), CR)

        G.DrawLine(New Pen(New SolidBrush(Color.FromArgb(70, 70, 70))), 0, 36, Width - 1, 36)

        G.DrawPath(New Pen(New SolidBrush(Color.FromArgb(15, 15, 15))), CreateRound(New Rectangle(0, 0, Width - 1, Height - 1), 10))
        G.DrawPath(New Pen(New SolidBrush(Color.FromArgb(88, 88, 88))), CreateRound(New Rectangle(1, 1, Width - 3, Height - 3), 10))

        Dim drawFont As New Font("Tahoma", 10, FontStyle.Bold)
        G.DrawString(FindForm.Text, drawFont, Brushes.WhiteSmoke, 12, 10)

        '////left upper corner
        G.FillRectangle(Brushes.Fuchsia, 0, 0, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, 1, 0, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, 2, 0, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, 3, 0, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, 0, 1, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, 0, 2, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, 0, 3, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, 1, 1, 1, 1)
        ''////right upper corner
        G.FillRectangle(Brushes.Fuchsia, Width - 1, 0, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, Width - 2, 0, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, Width - 3, 0, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, Width - 4, 0, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, Width - 1, 1, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, Width - 1, 2, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, Width - 1, 3, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, Width - 2, 1, 1, 1)
        ''////left bottom corner
        G.FillRectangle(Brushes.Fuchsia, 0, Height - 1, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, 0, Height - 2, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, 0, Height - 3, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, 0, Height - 4, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, 1, Height - 1, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, 2, Height - 1, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, 3, Height - 1, 1, 1)
        G.FillRectangle(Brushes.Fuchsia, 1, Height - 1, 1, 1)
        'G.FillRectangle(Brushes.Fuchsia, 2, Height - 2, 1, 1)
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


        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
    Private MouseP As Point = New Point(0, 0)
    Private Cap As Boolean = False
    Private MoveHeight% = 35 : Private pos% = 0
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        If e.Button = Windows.Forms.MouseButtons.Left And New Rectangle(0, 0, Width, MoveHeight).Contains(e.Location) Then
            Cap = True : MouseP = e.Location
        End If
    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseUp(e) : Cap = False
    End Sub
    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(e)
        If Cap Then
            Parent.Location = MousePosition - MouseP
        End If
    End Sub
    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        Me.ParentForm.FormBorderStyle = FormBorderStyle.None
        Me.ParentForm.TransparencyKey = Color.Fuchsia
        Dock = DockStyle.Fill
    End Sub
End Class
Public Class ExcisionButtonDefault : Inherits Control
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

    Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        ForeColor = Color.FromArgb(205, 205, 205)
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim ClientRectangle As New Rectangle(0, 0, Width - 1, Height - 1)
        Dim InnerRect As New Rectangle(1, 1, Width - 3, Height - 3)


        MyBase.OnPaint(e)

        G.Clear(BackColor)
        Dim drawFont As New Font("Tahoma", 10, FontStyle.Bold)

        'G.CompositingQuality = CompositingQuality.HighQuality
        G.SmoothingMode = SmoothingMode.HighQuality
        Dim cti As New ImageToCodeClass

        Dim BUTTONGRAIN As Bitmap = cti.CodeToImage("/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAIBAQIBAQICAgICAgICAwUDAwMDAwYEBAMFBwYHBwcGBwcICQsJCAgKCAcHCg0KCgsMDAwMBwkODw0MDgsMDAz/2wBDAQICAgMDAwYDAwYMCAcIDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAz/wAARCAA1AHUDASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwD8nk3IuWycY6HFDsFIJYHnPTPB9aUAMc4OFxz05oCbmxgKDz1xxQUgYGTJzvYf/qoQkglQwPUnOfaiP94xyOnT34pVjC5ABGDgjPXigaEkZiWVsceopFLeVtXvwBjilRMMTJkdvunmkjAJ68g8Y60CE37woJwMHOTxT45AMEBiMYx26daRgrIPlICt2HShXVUBIy2RyOKBjRG2wFslc8+tGQvHuRzyacRuBIJYevak3/NuJLA8EZ6UCFZCVJYDJ5z/AEpRl4yAAS3f0oPCAlWIznIPWo1I4GCR6E0DFGGfBAyfwxRwCW3AAnIyKdncAcKQvJyfWgox+TIIH48UBYVQhGGVyw6kUU0kEA7ACfVaKBIG7sATkntxQu7gEjJA56/hQoYspGcnjjAFKBlgBwT6CgY7Z935dox1J703c27DZO71pQGXqdzjqDzgU0syoBjHHWgLDlYKQFLIT0GOKbExJJIIJ4pVBJBBI47DH40oO1yWGS2CMc0AGCN43AY496FJZDkgAHjPAo3/ADE4AOc+9NZl6hT9TyaBAc5BKlgPTikXjduJA7jHSiNgzlgRjHGTTtuDkEkk8+lACgFUUqAMjk570nDrk5BBxxSTKyYBACjvTmbeVXDYPH/16AGiUK4Gcgnnnt9KHbYFBBO3npg0uN0hHBKjuMUNhGJBIDde9Axy2vmEjfnaB+tFJGzKSASD3xRQJDWcbyVAGTjHp70oyFVWyB7AGhlBByCCTnnpRtLNgnJPP0oC4MoJKqpUNxQ6HauF5I5HTNJtKIcAkHucE0u0CJQwyT0OKAuIoCttCg7eMZORSgoEA24btzgjrSlcYZQpJ54poyzZBJI4ANADombgkDB6np3ppjzgsTgnGcUofPyg7c5Ht+tIg5AJ68YyKBodEAxJyFAH6004yPmYkHGf/rUqgjcM5J49h1pHKnAIBI7+poEKcBiCBg8ZFNY4PGcD88UrZHAPygjj8KVV5I6Y7ngCgBqDMgzuyOeAc053boMBicj/ADmguwQhiGweM4NL8zEADbg8cUDTGjbyWDEHpzRSuFLYIGR6ciigVwjYvjPIwOvNICGYHBG4kHFFFA0JwzEqNuQf5U8sUUk4ZSenTtRRQJBuEiYAK454pquFQrtyVzz3oooBMCxOAMLkHkdetPiUPKytzlcknk0UUAQl96g4wQaeF+QE9G9PqKKKAYMuVLAkZJp+3MUj5IYn8KKKBoYSWbGcY4HApAxMjAk/KfWiigTY5CyLlTt3UUUUCbP/2Q==")
        Dim BUTTONIMAGE As New TextureBrush(BUTTONGRAIN, WrapMode.TileFlipXY)
        G.FillPath(BUTTONIMAGE, Draw.RoundRect(ClientRectangle, 4))
        Dim p As New Pen(New SolidBrush(Color.FromArgb(117, 120, 117)))
        G.DrawPath(Pens.Black, Draw.RoundRect(ClientRectangle, 4))
        Dim Ip As New Pen(Color.FromArgb(45, Color.White))
        G.DrawPath(Ip, Draw.RoundRect(InnerRect, 4))

        Select Case State
            Case MouseState.None
                G.DrawString(Text, drawFont, New SolidBrush(Color.FromArgb(178, 178, 178)), New Rectangle(0, 1, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
            Case MouseState.Over
                G.DrawString(Text, drawFont, New SolidBrush(Color.WhiteSmoke), New Rectangle(0, 1, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
            Case MouseState.Down
                G.DrawString(Text, drawFont, New SolidBrush(Color.FromArgb(178, 178, 178)), New Rectangle(0, 1, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})

        End Select


        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class
Public Class ExcisionCircleButton : Inherits Control
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

    Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        ForeColor = Color.FromArgb(205, 205, 205)
        Size = New Size(64, 64)
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim ClientRectangle As New Rectangle(0, 0, Width - 1, Height - 1)
        Dim InnerRect As New Rectangle(1, 1, Width - 3, Height - 3)


        MyBase.OnPaint(e)

        G.Clear(BackColor)
        Dim drawFont As New Font("Tahoma", 10, FontStyle.Bold)

        'G.CompositingQuality = CompositingQuality.HighQuality
        G.SmoothingMode = SmoothingMode.HighQuality
        Dim cti As New ImageToCodeClass

        Dim BUTTONGRAIN As Bitmap = cti.CodeToImage("/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAIBAQIBAQICAgICAgICAwUDAwMDAwYEBAMFBwYHBwcGBwcICQsJCAgKCAcHCg0KCgsMDAwMBwkODw0MDgsMDAz/2wBDAQICAgMDAwYDAwYMCAcIDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAz/wAARCAA1AHUDASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwD8nk3IuWycY6HFDsFIJYHnPTPB9aUAMc4OFxz05oCbmxgKDz1xxQUgYGTJzvYf/qoQkglQwPUnOfaiP94xyOnT34pVjC5ABGDgjPXigaEkZiWVsceopFLeVtXvwBjilRMMTJkdvunmkjAJ68g8Y60CE37woJwMHOTxT45AMEBiMYx26daRgrIPlICt2HShXVUBIy2RyOKBjRG2wFslc8+tGQvHuRzyacRuBIJYevak3/NuJLA8EZ6UCFZCVJYDJ5z/AEpRl4yAAS3f0oPCAlWIznIPWo1I4GCR6E0DFGGfBAyfwxRwCW3AAnIyKdncAcKQvJyfWgox+TIIH48UBYVQhGGVyw6kUU0kEA7ACfVaKBIG7sATkntxQu7gEjJA56/hQoYspGcnjjAFKBlgBwT6CgY7Z935dox1J703c27DZO71pQGXqdzjqDzgU0syoBjHHWgLDlYKQFLIT0GOKbExJJIIJ4pVBJBBI47DH40oO1yWGS2CMc0AGCN43AY496FJZDkgAHjPAo3/ADE4AOc+9NZl6hT9TyaBAc5BKlgPTikXjduJA7jHSiNgzlgRjHGTTtuDkEkk8+lACgFUUqAMjk570nDrk5BBxxSTKyYBACjvTmbeVXDYPH/16AGiUK4Gcgnnnt9KHbYFBBO3npg0uN0hHBKjuMUNhGJBIDde9Axy2vmEjfnaB+tFJGzKSASD3xRQJDWcbyVAGTjHp70oyFVWyB7AGhlBByCCTnnpRtLNgnJPP0oC4MoJKqpUNxQ6HauF5I5HTNJtKIcAkHucE0u0CJQwyT0OKAuIoCttCg7eMZORSgoEA24btzgjrSlcYZQpJ54poyzZBJI4ANADombgkDB6np3ppjzgsTgnGcUofPyg7c5Ht+tIg5AJ68YyKBodEAxJyFAH6004yPmYkHGf/rUqgjcM5J49h1pHKnAIBI7+poEKcBiCBg8ZFNY4PGcD88UrZHAPygjj8KVV5I6Y7ngCgBqDMgzuyOeAc053boMBicj/ADmguwQhiGweM4NL8zEADbg8cUDTGjbyWDEHpzRSuFLYIGR6ciigVwjYvjPIwOvNICGYHBG4kHFFFA0JwzEqNuQf5U8sUUk4ZSenTtRRQJBuEiYAK454pquFQrtyVzz3oooBMCxOAMLkHkdetPiUPKytzlcknk0UUAQl96g4wQaeF+QE9G9PqKKKAYMuVLAkZJp+3MUj5IYn8KKKBoYSWbGcY4HApAxMjAk/KfWiigTY5CyLlTt3UUUUCbP/2Q==")
        Dim BUTTONIMAGE As New TextureBrush(BUTTONGRAIN, WrapMode.TileFlipXY)
        G.FillEllipse(BUTTONIMAGE, ClientRectangle)
        Dim p As New Pen(New SolidBrush(Color.FromArgb(117, 120, 117)))
        G.DrawEllipse(Pens.Black, ClientRectangle)
        Dim Ip As New Pen(Color.FromArgb(45, Color.White))
        G.DrawEllipse(Ip, InnerRect)

        Select Case State
            Case MouseState.None
                G.DrawString(Text, drawFont, New SolidBrush(Color.FromArgb(178, 178, 178)), New Rectangle(0, 1, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
            Case MouseState.Over
                G.DrawString(Text, drawFont, New SolidBrush(Color.WhiteSmoke), New Rectangle(0, 1, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
            Case MouseState.Down
                G.DrawString(Text, drawFont, New SolidBrush(Color.FromArgb(178, 178, 178)), New Rectangle(0, 1, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})

        End Select


        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class
Public Class ExcisionButtonBlue : Inherits Control
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

    Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        ForeColor = Color.FromArgb(205, 205, 205)
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim ClientRectangle As New Rectangle(0, 0, Width - 1, Height - 1)
        Dim InnerRect As New Rectangle(1, 1, Width - 3, Height - 3)


        MyBase.OnPaint(e)

        G.Clear(BackColor)
        Dim drawFont As New Font("Tahoma", 10, FontStyle.Bold)

        'G.CompositingQuality = CompositingQuality.HighQuality
        G.SmoothingMode = SmoothingMode.HighQuality

        Select Case State
            Case MouseState.None
                Dim BBG As New LinearGradientBrush(ClientRectangle, Color.FromArgb(85, 156, 188), Color.FromArgb(58, 136, 175), 90S)
                G.FillPath(BBG, Draw.RoundRect(ClientRectangle, 4))
                Dim p As New Pen(New SolidBrush(Color.FromArgb(29, 46, 54)))
                G.DrawPath(Pens.Black, Draw.RoundRect(ClientRectangle, 4))
                Dim Ip As New Pen(Color.FromArgb(129, 175, 201))
                G.DrawPath(Ip, Draw.RoundRect(InnerRect, 4))
            Case MouseState.Over
                Dim BBG As New LinearGradientBrush(ClientRectangle, Color.FromArgb(45, 105, 135), Color.FromArgb(30, 90, 120), 90S)
                G.FillPath(BBG, Draw.RoundRect(ClientRectangle, 4))
                Dim p As New Pen(New SolidBrush(Color.FromArgb(29, 46, 54)))
                G.DrawPath(Pens.Black, Draw.RoundRect(ClientRectangle, 4))
                Dim Ip As New Pen(Color.FromArgb(114, 160, 186))
                G.DrawPath(Ip, Draw.RoundRect(InnerRect, 4))
            Case MouseState.Down
                Dim BBG As New LinearGradientBrush(ClientRectangle, Color.FromArgb(85, 156, 188), Color.FromArgb(58, 136, 175), 90S)
                G.FillPath(BBG, Draw.RoundRect(ClientRectangle, 4))
                Dim p As New Pen(New SolidBrush(Color.FromArgb(29, 46, 54)))
                G.DrawPath(Pens.Black, Draw.RoundRect(ClientRectangle, 4))
                Dim Ip As New Pen(Color.FromArgb(129, 175, 201))
                G.DrawPath(Ip, Draw.RoundRect(InnerRect, 4))
        End Select
        G.DrawString(Text, drawFont, New SolidBrush(Color.WhiteSmoke), New Rectangle(0, 1, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})


        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class
Public Class ExcisionButtonWhite : Inherits Control
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

    Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        ForeColor = Color.FromArgb(205, 205, 205)
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim ClientRectangle As New Rectangle(0, 0, Width - 1, Height - 1)
        Dim InnerRect As New Rectangle(1, 1, Width - 3, Height - 3)


        MyBase.OnPaint(e)

        G.Clear(BackColor)
        Dim drawFont As New Font("Tahoma", 10, FontStyle.Bold)

        'G.CompositingQuality = CompositingQuality.HighQuality
        G.SmoothingMode = SmoothingMode.HighQuality

        Select Case State
            Case MouseState.None
                Dim BBG As New LinearGradientBrush(ClientRectangle, Color.FromArgb(225, 225, 225), Color.FromArgb(210, 210, 210), 90S)
                G.FillPath(BBG, Draw.RoundRect(ClientRectangle, 4))
                Dim p As New Pen(New SolidBrush(Color.FromArgb(254, 254, 254)))
                G.DrawPath(Pens.Black, Draw.RoundRect(ClientRectangle, 4))
                Dim Ip As New Pen(Color.FromArgb(255, 255, 255))
                G.DrawPath(Ip, Draw.RoundRect(InnerRect, 4))
                G.DrawString(Text, drawFont, New SolidBrush(Color.FromArgb(82, 87, 93)), New Rectangle(0, 1, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
            Case MouseState.Over
                Dim BBG As New LinearGradientBrush(ClientRectangle, Color.FromArgb(222, 222, 222), Color.FromArgb(222, 222, 222), 90S)
                G.FillPath(BBG, Draw.RoundRect(ClientRectangle, 4))
                Dim p As New Pen(New SolidBrush(Color.FromArgb(222, 222, 222)))
                G.DrawPath(Pens.Black, Draw.RoundRect(ClientRectangle, 4))
                Dim Ip As New Pen(Color.FromArgb(255, 255, 255))
                G.DrawPath(Ip, Draw.RoundRect(InnerRect, 4))
                G.DrawString(Text, drawFont, New SolidBrush(Color.FromArgb(85, 85, 85)), New Rectangle(0, 1, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})

            Case MouseState.Down
                Dim BBG As New LinearGradientBrush(ClientRectangle, Color.FromArgb(254, 254, 254), Color.FromArgb(248, 246, 247), 90S)
                G.FillPath(BBG, Draw.RoundRect(ClientRectangle, 4))
                Dim p As New Pen(New SolidBrush(Color.FromArgb(254, 254, 254)))
                G.DrawPath(Pens.Black, Draw.RoundRect(ClientRectangle, 4))
                Dim Ip As New Pen(Color.FromArgb(255, 255, 255))
                G.DrawPath(Ip, Draw.RoundRect(InnerRect, 4))
                G.DrawString(Text, drawFont, New SolidBrush(Color.FromArgb(82, 87, 93)), New Rectangle(0, 1, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
        End Select


        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class
Public Class ExcisionSeparator : Inherits Control
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
    Private _Horizontal As Boolean = True
    Public Property Horizontal() As Boolean
        Get
            Return _Horizontal
        End Get
        Set(ByVal v As Boolean)
            _Horizontal = v
            Invalidate()
        End Set
    End Property
    Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor Or ControlStyles.FixedHeight, True)
        BackColor = Color.Transparent
        ForeColor = Color.FromArgb(205, 205, 205)
        Height = 4
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)


        MyBase.OnPaint(e)

        G.Clear(BackColor)

        'G.CompositingQuality = CompositingQuality.HighQuality
        G.SmoothingMode = SmoothingMode.HighQuality
        Select Case _Horizontal
            Case True
                G.DrawLine(New Pen(New SolidBrush(Color.FromArgb(26, 24, 25))), 0, 0, Width - 1, 0)
                G.DrawLine(New Pen(New SolidBrush(Color.FromArgb(75, 73, 74))), 0, 1, Width - 1, 1)
            Case False
                G.DrawLine(New Pen(New SolidBrush(Color.FromArgb(26, 24, 25))), 0, 0, 0, Height - 1)
                G.DrawLine(New Pen(New SolidBrush(Color.FromArgb(75, 73, 74))), 1, 0, 1, Height - 1)
        End Select


        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class
Public Class ExcisionGroupBox : Inherits ContainerControl

    Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        DoubleBuffered = True
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim Top As New Rectangle(0, 0, Width - 1, 45)
        Dim TopInner As New Rectangle(1, 1, Width - 3, 43)
        Dim Bottom As New Rectangle(0, 43, Width - 1, Height - 44)
        Dim BottomInner As New Rectangle(1, 45, Width - 3, Height - 47)

        MyBase.OnPaint(e)

        G.Clear(Color.Transparent)
        Dim d As New ImageToCodeClass

        G.SmoothingMode = SmoothingMode.HighQuality
        G.CompositingQuality = CompositingQuality.HighQuality

        Dim GRAINIMAGE2 As Bitmap = d.CodeToImage("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")
        Dim TEXTUREIMAGE2 As New TextureBrush(GRAINIMAGE2, WrapMode.TileFlipXY)
        G.FillPath(TEXTUREIMAGE2, Draw.RoundRect(Bottom, 2))
        G.DrawPath(New Pen(New SolidBrush(Color.FromArgb(26, 24, 25))), Draw.RoundRect(Bottom, 2))
        G.DrawPath(New Pen(New SolidBrush(Color.FromArgb(10, Color.White))), Draw.RoundRect(BottomInner, 2))


        Dim GRAINIMAGE As Bitmap = d.CodeToImage("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")
        Dim TEXTUREIMAGE As New TextureBrush(GRAINIMAGE, WrapMode.TileFlipXY)
        G.FillPath(TEXTUREIMAGE, Draw.RoundRect(Top, 2))
        G.DrawPath(New Pen(New SolidBrush(Color.FromArgb(26, 24, 25))), Draw.RoundRect(Top, 2))
        G.DrawPath(New Pen(New SolidBrush(Color.FromArgb(10, Color.White))), Draw.RoundRect(TopInner, 2))

        G.DrawString(Text, New Font("Tahoma", 10, FontStyle.Bold), New SolidBrush(Color.FromArgb(229, 229, 229)), New Rectangle(14, 14, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Near})


        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class
Public Class ExcisionPanel : Inherits ContainerControl

    Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        Size = New Size(100, 45)
        DoubleBuffered = True
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim MainRect As New Rectangle(0, 0, Width - 1, Height - 1)

        MyBase.OnPaint(e)

        G.Clear(Color.Transparent)
        Dim d As New ImageToCodeClass

        G.SmoothingMode = SmoothingMode.HighQuality
        G.CompositingQuality = CompositingQuality.HighQuality

        G.FillPath(New SolidBrush(Color.FromArgb(58, 56, 57)), RoundRect(MainRect, 3))
        G.DrawPath(New Pen(New SolidBrush(Color.FromArgb(77, 77, 77))), RoundRect(MainRect, 3))

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class
<DefaultEvent("CheckedChanged")> Public Class ExcisionCheckBox : Inherits Control

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
        Height = 14
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
        Size = New Size(145, 16)
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim checkBoxRectangle As New Rectangle(0, 0, Height, Height - 1)
        Dim Inner As New Rectangle(1, 1, Height - 2, Height - 3)

        G.SmoothingMode = SmoothingMode.HighQuality
        G.CompositingQuality = CompositingQuality.HighQuality
        G.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit

        G.Clear(Color.Transparent)

        Dim bodyGrad As New LinearGradientBrush(checkBoxRectangle, Color.FromArgb(60, 60, 60), Color.FromArgb(45, 45, 45), 90S)
        G.FillRectangle(bodyGrad, bodyGrad.Rectangle)
        G.DrawRectangle(New Pen(Color.FromArgb(26, 26, 26)), checkBoxRectangle)
        G.DrawRectangle(New Pen(Color.FromArgb(70, 70, 70)), Inner)

        If Checked Then
            Dim t As New Font("Marlett", 10, FontStyle.Regular)
            G.DrawString("a", t, New SolidBrush(Color.FromArgb(222, 222, 222)), -1.5, 0)
        End If

        Dim drawFont As New Font("Tahoma", 8, FontStyle.Bold)
        Dim nb As Brush = New SolidBrush(Color.FromArgb(200, 200, 200))
        G.DrawString(Text, drawFont, nb, New Point(18, 7), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()

    End Sub

End Class
<DefaultEvent("CheckedChanged")> Public Class ExcisionRadioButton : Inherits Control

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
        Height = 16
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
            If C IsNot Me AndAlso TypeOf C Is ExcisionRadioButton Then
                DirectCast(C, ExcisionRadioButton).Checked = False
            End If
        Next
    End Sub
#End Region

    Sub New()
        MyBase.New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        ForeColor = Color.Black
        Size = New Size(150, 16)
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim radioBtnRectangle = New Rectangle(0, 0, Height, Height - 1)
        Dim Inner As New Rectangle(1, 1, Height - 2, Height - 3)

        G.SmoothingMode = SmoothingMode.HighQuality
        G.CompositingQuality = CompositingQuality.HighQuality
        G.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit

        G.Clear(BackColor)

        Dim bgGrad As New LinearGradientBrush(radioBtnRectangle, Color.FromArgb(60, 60, 60), Color.FromArgb(45, 45, 45), 90S)
        G.FillEllipse(bgGrad, radioBtnRectangle)

        G.DrawEllipse(New Pen(Color.FromArgb(26, 26, 26)), radioBtnRectangle)
        G.DrawEllipse(New Pen(Color.FromArgb(70, 70, 70)), Inner)

        If Checked Then
            Dim t As New Font("Marlett", 6, FontStyle.Bold)
            G.DrawString("n", t, New SolidBrush(Color.FromArgb(222, 222, 222)), 3, 4)
        End If

        Dim drawFont As New Font("Tahoma", 8, FontStyle.Bold)
        Dim nb As Brush = New SolidBrush(Color.FromArgb(200, 200, 200))
        G.DrawString(Text, drawFont, nb, New Point(18, 7), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub

End Class
Public Class ExcisionControlBox : Inherits Control
    Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        ForeColor = Color.FromArgb(205, 205, 205)
        Size = New Size(65, 26)
        DoubleBuffered = True
    End Sub
#Region " MouseStates "
    Dim State As MouseState = MouseState.None
    Dim X As Integer
    Dim MinBtn As New Rectangle(0, 0, 32, 25)
    Dim CloseBtn As New Rectangle(33, 0, 65, 25)
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        If X > MinBtn.X And X < MinBtn.X + 32 Then
            FindForm.WindowState = FormWindowState.Minimized
        Else
            FindForm.Close()
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
        Dim ClientRectangle As New Rectangle(0, 0, 64, 25)
        Dim InnerRect As New Rectangle(1, 1, 62, 23)


        MyBase.OnPaint(e)

        G.Clear(BackColor)
        Dim drawFont As New Font("Marlett", 10, FontStyle.Bold)

        'G.CompositingQuality = CompositingQuality.HighQuality
        G.SmoothingMode = SmoothingMode.HighQuality

        Select Case State
            Case MouseState.None
                G.DrawString("r", drawFont, New SolidBrush(Color.FromArgb(178, 178, 178)), New Rectangle(17, 1, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                G.DrawString("0", drawFont, New SolidBrush(Color.FromArgb(178, 178, 178)), New Rectangle(8, 1, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})
            Case MouseState.Over
                If X > MinBtn.X And X < MinBtn.X + 32 Then
                    G.DrawString("0", drawFont, New SolidBrush(Color.FromArgb(178, 178, 178)), New Rectangle(8, 1, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})
                    G.DrawString("0", drawFont, New SolidBrush(Color.FromArgb(95, Color.Green)), New Rectangle(8, 1, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})
                    G.DrawString("r", drawFont, New SolidBrush(Color.FromArgb(178, 178, 178)), New Rectangle(17, 1, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                Else
                    G.DrawString("r", drawFont, New SolidBrush(Color.FromArgb(178, 178, 178)), New Rectangle(17, 1, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                    G.DrawString("r", drawFont, New SolidBrush(Color.FromArgb(95, Color.Red)), New Rectangle(17, 1, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                    G.DrawString("0", drawFont, New SolidBrush(Color.FromArgb(178, 178, 178)), New Rectangle(8, 1, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})
                End If
            Case MouseState.Down
                G.DrawString("r", drawFont, New SolidBrush(Color.FromArgb(178, 178, 178)), New Rectangle(17, 1, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                G.DrawString("0", drawFont, New SolidBrush(Color.FromArgb(178, 178, 178)), New Rectangle(8, 1, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})
        End Select


        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class
Public Class ExcisionProgressBar : Inherits Control

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
        ' Dim tmr As New Timer With {.Interval = Speed}
        ' AddHandler tmr.Tick, AddressOf Animate
        ' tmr.Start()
        Dim T As New Threading.Thread(AddressOf Animate)
        T.IsBackground = True
        'T.Start()
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
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)

        G.SmoothingMode = SmoothingMode.HighQuality

        Dim intValue As Integer = CInt(_Value / _Maximum * Width)
        G.Clear(BackColor)

        Dim d As New ImageToCodeClass
        Dim GRAINIMAGE2 As Bitmap = d.CodeToImage("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")
        Dim TEXTUREIMAGE2 As New TextureBrush(GRAINIMAGE2, WrapMode.TileFlipX)
        '//// Inner Fill
        G.FillPath(TEXTUREIMAGE2, Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 2))
        G.DrawPath(New Pen(New SolidBrush(Color.FromArgb(26, 26, 26))), Draw.RoundRect(New Rectangle(1, 1, Width - 3, Height - 3), 2))

        '//// Bar Fill
        Dim g1 As New LinearGradientBrush(New Rectangle(2, 2, intValue - 5, Height - 5), Color.FromArgb(60, 60, 60), Color.FromArgb(45, 45, 45), 90S)
        G.FillPath(g1, Draw.RoundRect(New Rectangle(2, 2, intValue - 5, Height - 5), 2))

        '/// Bar Overlap
        'Dim h1 As New HatchBrush(HatchStyle.DarkUpwardDiagonal, Color.FromArgb(40, Color.White), Color.FromArgb(20, Color.White))
        'G.FillPath(h1, Draw.RoundRect(New Rectangle(0, 0, intValue - 1, Height - 2), 1))

        '//// Outer Rectangle
        G.DrawPath(New Pen(Color.FromArgb(190, 70, 70, 70)), Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 2))

        '//// Bar Size
        G.DrawPath(New Pen(Color.FromArgb(150, 97, 94, 90)), Draw.RoundRect(New Rectangle(2, 2, intValue - 5, Height - 5), 2))

        If _ShowPercentage Then
            G.DrawString(Convert.ToString(String.Concat(Value, "%")), New Font("Tahoma", 9, FontStyle.Bold), Brushes.White, New Rectangle(0, 1, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
        End If

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class