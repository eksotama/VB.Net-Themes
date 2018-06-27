Imports System.Data
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Text
Imports System.Windows.Forms
Imports System.Runtime.CompilerServices

#Region "Phsphene_Label"

Public Class Phsphene_Label
    Inherits Label
    Public Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.DoubleBuffer, True)
        BackColor = Color.White
        ForeColor = Color.FromArgb(100, 100, 100)
        Font = New Font("Segoe UI", 9, FontStyle.Bold)
    End Sub

End Class

#End Region

#Region "Phsphene_ProgressBar"

Public Class Phsphene_ProgressBar
    Inherits Control
    Private _Minimum As Integer
    Public Property Minimum() As Integer
        Get
            Return _Minimum
        End Get
        Set(value As Integer)
            If value < 0 Then
                Throw New Exception("Property value is not valid.")
            End If

            _Minimum = value
            If value > _Value Then
                _Value = value
            End If
            If value > _Maximum Then
                _Maximum = value
            End If
            Invalidate()
        End Set
    End Property

    Private _Maximum As Integer = 100
    Public Property Maximum() As Integer
        Get
            Return _Maximum
        End Get
        Set(value As Integer)
            If value < 0 Then
                Throw New Exception("Property value is not valid.")
            End If

            _Maximum = value
            If value < _Value Then
                _Value = value
            End If
            If value < _Minimum Then
                _Minimum = value
            End If
            Invalidate()
        End Set
    End Property

    Private _Value As Integer
    Public Property Value() As Integer
        Get
            Return _Value
        End Get
        Set(value As Integer)
            If value > _Maximum OrElse value < _Minimum Then
                Throw New Exception("Property value is not valid.")
            End If

            _Value = value
            Invalidate()
        End Set
    End Property

    Private I1 As Integer

    Private Sub Increment(amount As Integer)
        Value += amount
    End Sub

    Public Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.DoubleBuffer, True)
        Font = New Font("Segoe UI", 10, FontStyle.Bold)
        Size = New Size(176, 25)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim G As Graphics = e.Graphics
        G.Clear(Color.LightGray)
        G.DrawRectangle(New Pen(Color.FromArgb(75, 157, 219)), 0, 0, Width - 1, Height - 1)
        'I1 = Convert.ToInt32((_Value - _Minimum) / (_Maximum - _Minimum) * (Width - 3)); 
        I1 = (_Value \ 10) * Width \ 200
        'I1 = (Width / 90) * 20;
        G.FillRectangle(New SolidBrush(Color.FromArgb(75, 157, 219)), 1, 1, I1 - 2, Height - 2)
        G.DrawString(((_Value \ 5) * Width \ 20).ToString(), Font, New SolidBrush(Color.Black), New PointF(5, 5))
        MyBase.OnPaint(e)
    End Sub
End Class

#End Region

#Region "Phsphene_GroupBoxOne"

Public Class Phsphene_GroupBoxOne
    Inherits ContainerControl
    Public Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.DoubleBuffer, True)
        ForeColor = Color.FromArgb(0, 0, 0)
        Size = New Size(176, 98)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim G As Graphics = e.Graphics
        G.Clear(Color.White)
        G.DrawString(Text, New Font("Segoe UI", 10, FontStyle.Bold), New SolidBrush(ForeColor), 3, 0)
        Dim X As Integer = CInt(Math.Truncate(G.MeasureString(Text, New Font("Segoe UI", 10, FontStyle.Bold)).Width))
        'G.DrawLine(new Pen(Color.FromArgb(238, 240, 242)), new Point(X, Height - 6), new Point(Width - 2, Height - 6));
        G.DrawLine(New Pen(Color.FromArgb(238, 240, 242)), New Point(0, Height - 6), New Point(Width - 2, Height - 6))
        MyBase.OnPaint(e)
    End Sub
End Class

#End Region

#Region "Phsphene_GroupBoxTwo"

Public Class Phsphene_GroupBoxTwo
    Inherits ContainerControl
    Public Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.DoubleBuffer, True)
        ForeColor = Color.FromArgb(0, 0, 0)
        Size = New Size(176, 98)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim G As Graphics = e.Graphics
        G.Clear(Color.White)
        Dim X As Integer = CInt(Math.Truncate(G.MeasureString(Text, New Font("Segoe UI", 10, FontStyle.Bold)).Width))
        G.DrawRectangle(New Pen(Color.FromArgb(238, 240, 242)), New Rectangle(0, 9, Width - 2, Height - 10))
        G.DrawLine(New Pen(Color.White), New Point(4, 9), New Point(X - 3, 9))
        G.DrawString(Text, New Font("Segoe UI", 10, FontStyle.Bold), New SolidBrush(ForeColor), 4, 0)
        MyBase.OnPaint(e)
    End Sub

End Class

#End Region

#Region "Phsphene_TabControlOne"

Public Class Phsphene_TabControlOne
    Inherits TabControl
    Public Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.DoubleBuffer, True)

        DoubleBuffered = True
        SizeMode = TabSizeMode.Fixed
        Alignment = TabAlignment.Left
        ItemSize = New Size(30, 152)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        G.Clear(Color.White)

        For i As Integer = 0 To TabCount - 1
            Dim x2 As New Rectangle(New Point(GetTabRect(i).Location.X - 2, GetTabRect(i).Location.Y - 2), New Size(GetTabRect(i).Width + 3, GetTabRect(i).Height - 1))

            If i = SelectedIndex Then
                G.FillRectangle(New SolidBrush(Color.FromArgb(75, 157, 219)), New Rectangle(New Point(x2.Location.X, x2.Location.Y + 3), New Size(GetTabRect(i).Size.Width, GetTabRect(i).Size.Height)))

                G.DrawString(TabPages(i).Text, New Font("Segoe UI", 10), Brushes.White, New Rectangle(x2.Location.X + 6, x2.Location.Y + 3, x2.Width - 20, x2.Height), New StringFormat() With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Near})
            Else
                G.FillRectangle(New SolidBrush(Color.FromArgb(255, 255, 255)), New Rectangle(New Point(x2.Location.X + 3, x2.Location.Y + 3), New Size(GetTabRect(i).Size.Width, GetTabRect(i).Size.Height)))

                '123, 143, 162
				G.DrawString(TabPages(i).Text, New Font("Segoe UI", 10), New SolidBrush(Color.FromArgb(64, 80, 95)), New Rectangle(x2.Location.X + 6, x2.Location.Y + 3, x2.Width - 20, x2.Height), New StringFormat() With { .LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Near})

            End If
        Next

        G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(238, 240, 242))), New Rectangle(New Point(0, 3), New Size(ItemSize.Height, Size.Height - 5)))

        e.Graphics.DrawImage(DirectCast(B.Clone(), Image), 0, 0)

        G.Dispose()
        B.Dispose()
    End Sub
End Class

#End Region

#Region "Phsphene_TabControlTwo"

Public Class Phsphene_TabControlTwo
    Inherits TabControl
    Public Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.DoubleBuffer, True)

        DoubleBuffered = True
        SizeMode = TabSizeMode.Fixed
        Alignment = TabAlignment.Left
        ItemSize = New Size(30, 152)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        G.Clear(Color.White)

        For i As Integer = 0 To TabCount - 1
            Dim x2 As New Rectangle(New Point(GetTabRect(i).Location.X - 2, GetTabRect(i).Location.Y - 2), New Size(GetTabRect(i).Width + 3, GetTabRect(i).Height - 1))

            If i = SelectedIndex Then
                G.FillRectangle(New SolidBrush(Color.FromArgb(75, 157, 219)), New Rectangle(New Point(x2.Location.X, x2.Location.Y + 3), New Size(GetTabRect(i).Size.Width, GetTabRect(i).Size.Height)))

                G.DrawRectangle(New Pen(Color.Gray), New Rectangle(x2.Location.X + 5, x2.Location.Y + 9, 3, ItemSize.Width - 12))
                G.DrawRectangle(New Pen(Color.Gray), New Rectangle(x2.Location.X + 11, x2.Location.Y + 9, 3, ItemSize.Width - 12))

                G.FillRectangle(New SolidBrush(Color.Gray), New Rectangle(x2.Location.X + 5, x2.Location.Y + 9, 3, ItemSize.Width - 12))
                G.FillRectangle(New SolidBrush(Color.Gray), New Rectangle(x2.Location.X + 11, x2.Location.Y + 9, 3, ItemSize.Width - 12))
                G.DrawString(TabPages(i).Text, New Font("Segoe UI", 10), Brushes.White, New Rectangle(x2.Location.X + 24, x2.Location.Y + 3, x2.Width - 20, x2.Height), New StringFormat() With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Near})
            Else
                G.FillRectangle(New SolidBrush(Color.FromArgb(255, 255, 255)), New Rectangle(New Point(x2.Location.X + 3, x2.Location.Y + 3), New Size(GetTabRect(i).Size.Width, GetTabRect(i).Size.Height)))

                G.DrawRectangle(New Pen(Color.Gray), New Rectangle(x2.Location.X + 5, x2.Location.Y + 9, 3, ItemSize.Width - 12))
                G.DrawRectangle(New Pen(Color.Gray), New Rectangle(x2.Location.X + 11, x2.Location.Y + 9, 3, ItemSize.Width - 12))

                '123, 143, 162
                G.DrawString(TabPages(i).Text, New Font("Segoe UI", 10), New SolidBrush(Color.FromArgb(64, 80, 95)), New Rectangle(x2.Location.X + 24, x2.Location.Y + 3, x2.Width - 20, x2.Height), New StringFormat() With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Near})
            End If
        Next

        G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(238, 240, 242))), New Rectangle(New Point(0, 3), New Size(ItemSize.Height, Size.Height - 5)))

        e.Graphics.DrawImage(DirectCast(B.Clone(), Image), 0, 0)

        G.Dispose()
        B.Dispose()
    End Sub
End Class

#End Region

#Region "Phsphene_Button"

Public Class Phsphene_Button
    Inherits Control
    Private _RelC As Color
    Private _PresC As Color
    Private _Intensity As Short
    Private IsMouseDown As Boolean
    Public Property RelC() As Color
        Get
            Return _RelC
        End Get
        Set(value As Color)
            _RelC = value
            _PresC = New DarkIt().Lerp(value, Color.Black, 0.5)
            Invalidate()
        End Set
    End Property
    Public Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.DoubleBuffer, True)
        Size = New Size(122, 31)
        ForeColor = Color.White
        _RelC = Color.FromArgb(75, 157, 219)
        _Intensity = 0.5
        IsMouseDown = False
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Color.FromArgb(238, 240, 242)
        Dim G As Graphics = e.Graphics
        G.Clear(Color.White)

        If IsMouseDown = True Then
            G.FillRectangle(New SolidBrush(_PresC), New Rectangle(0, 0, Width - 1, Height - 1))
        Else
            G.FillRectangle(New SolidBrush(_RelC), New Rectangle(0, 0, Width - 1, Height - 1))
        End If

        Dim TextSize As SizeF = G.MeasureString(Text, New Font("Segoe UI", 10, FontStyle.Bold))
        G.DrawString(Text, New Font("Segoe UI", 10), New SolidBrush(ForeColor), New Rectangle(0, 0, Width, Height), New StringFormat() With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
        MyBase.OnPaint(e)

    End Sub

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        IsMouseDown = True
        Invalidate()
        MyBase.OnMouseDown(e)
    End Sub

    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        IsMouseDown = False
        Invalidate()
        MyBase.OnMouseUp(e)
    End Sub
End Class
Public Class DarkIt
    Public Function Lerp(start As Single, [end] As Single, amount As Single) As Single
        Dim difference As Single = [end] - start
        Dim adjusted As Single = difference * amount
        Return start + adjusted
    End Function

    Public Function Lerp(colour As Color, [to] As Color, amount As Single) As Color
        ' start colours as lerp-able floats
        Dim sr As Single = colour.R, sg As Single = colour.G, sb As Single = colour.B

        ' end colours as lerp-able floats
        Dim er As Single = [to].R, eg As Single = [to].G, eb As Single = [to].B

        ' lerp the colours to get the difference
        Dim r As Byte = CByte(Math.Truncate(Lerp(sr, er, amount))), g As Byte = CByte(Math.Truncate(Lerp(sg, eg, amount))), b As Byte = CByte(Math.Truncate(Lerp(sb, eb, amount)))

        ' return the new colour
        Return Color.FromArgb(r, g, b)
    End Function

End Class
#End Region
