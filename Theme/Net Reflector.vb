Imports System.Drawing.Drawing2D

Friend Module Methods

    Private TargetStringMeasure As SizeF

    Enum MouseState As Byte
        None = 0
        Over = 1
        Down = 2
    End Enum

    Enum RoundingStyle As Byte
        All = 0
        Top = 1
        Bottom = 2
        Left = 3
        Right = 4
        TopRight = 5
        BottomRight = 6
    End Enum

    Public Function MiddlePoint(G As Graphics, TargetText As String, TargetFont As Font, Rect As Rectangle) As Point
        TargetStringMeasure = G.MeasureString(TargetText, TargetFont)
        Return New Point(CInt((Rect.X + Rect.Width / 2) - (TargetStringMeasure.Width / 2)), CInt((Rect.Y + Rect.Height / 2) - (TargetStringMeasure.Height / 2)))
    End Function

    Public Function RoundRect(Rect As Rectangle, Rounding As Integer, Optional Style As RoundingStyle = RoundingStyle.All) As GraphicsPath

        Dim GP As New GraphicsPath()
        Dim AW As Integer = Rounding * 2

        GP.StartFigure()

        If Rounding = 0 Then
            GP.AddRectangle(Rect)
            GP.CloseAllFigures()
            Return GP
        End If

        Select Case Style
            Case RoundingStyle.All
                GP.AddArc(New Rectangle(Rect.X, Rect.Y, AW, AW), -180, 90)
                GP.AddArc(New Rectangle(Rect.Width - AW + Rect.X, Rect.Y, AW, AW), -90, 90)
                GP.AddArc(New Rectangle(Rect.Width - AW + Rect.X, Rect.Height - AW + Rect.Y, AW, AW), 0, 90)
                GP.AddArc(New Rectangle(Rect.X, Rect.Height - AW + Rect.Y, AW, AW), 90, 90)
            Case RoundingStyle.Top
                GP.AddArc(New Rectangle(Rect.X, Rect.Y, AW, AW), -180, 90)
                GP.AddArc(New Rectangle(Rect.Width - AW + Rect.X, Rect.Y, AW, AW), -90, 90)
                GP.AddLine(New Point(Rect.X + Rect.Width, Rect.Y + Rect.Height), New Point(Rect.X, Rect.Y + Rect.Height))
            Case RoundingStyle.Bottom
                GP.AddLine(New Point(Rect.X, Rect.Y), New Point(Rect.X + Rect.Width, Rect.Y))
                GP.AddArc(New Rectangle(Rect.Width - AW + Rect.X, Rect.Height - AW + Rect.Y, AW, AW), 0, 90)
                GP.AddArc(New Rectangle(Rect.X, Rect.Height - AW + Rect.Y, AW, AW), 90, 90)
            Case RoundingStyle.Left
                GP.AddArc(New Rectangle(Rect.X, Rect.Y, AW, AW), -180, 90)
                GP.AddLine(New Point(Rect.X + Rect.Width, Rect.Y), New Point(Rect.X + Rect.Width, Rect.Y + Rect.Height))
                GP.AddArc(New Rectangle(Rect.X, Rect.Height - AW + Rect.Y, AW, AW), 90, 90)
            Case RoundingStyle.Right
                GP.AddLine(New Point(Rect.X, Rect.Y + Rect.Height), New Point(Rect.X, Rect.Y))
                GP.AddArc(New Rectangle(Rect.Width - AW + Rect.X, Rect.Y, AW, AW), -90, 90)
                GP.AddArc(New Rectangle(Rect.Width - AW + Rect.X, Rect.Height - AW + Rect.Y, AW, AW), 0, 90)
            Case RoundingStyle.TopRight
                GP.AddLine(New Point(Rect.X, Rect.Y + 1), New Point(Rect.X, Rect.Y))
                GP.AddArc(New Rectangle(Rect.Width - AW + Rect.X, Rect.Y, AW, AW), -90, 90)
                GP.AddLine(New Point(Rect.X + Rect.Width, Rect.Y + Rect.Height - 1), New Point(Rect.X + Rect.Width, Rect.Y + Rect.Height))
                GP.AddLine(New Point(Rect.X + 1, Rect.Y + Rect.Height), New Point(Rect.X, Rect.Y + Rect.Height))
            Case RoundingStyle.BottomRight
                GP.AddLine(New Point(Rect.X, Rect.Y + 1), New Point(Rect.X, Rect.Y))
                GP.AddLine(New Point(Rect.X + Rect.Width - 1, Rect.Y), New Point(Rect.X + Rect.Width, Rect.Y))
                GP.AddArc(New Rectangle(Rect.Width - AW + Rect.X, Rect.Height - AW + Rect.Y, AW, AW), 0, 90)
                GP.AddLine(New Point(Rect.X + 1, Rect.Y + Rect.Height), New Point(Rect.X, Rect.Y + Rect.Height))
        End Select

        GP.CloseAllFigures()

        Return GP

    End Function

End Module

Public Class rTabControl
    Inherits TabControl

    Private G As Graphics
    Private Rect As Rectangle

    Sub New()
        DoubleBuffered = True
        Alignment = TabAlignment.Left
        SizeMode = TabSizeMode.Fixed
        ItemSize = New Size(40, 170)
        Font = New Font("Segoe UI", 10)
        SetStyle(ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer, True)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)

        G = e.Graphics

        G.Clear(Color.FromArgb(56, 62, 73))

        For T As Integer = 0 To TabPages.Count - 1

            Rect = GetTabRect(T)

            If SelectedIndex = T Then

                Using Selection As New SolidBrush(Color.FromArgb(71, 77, 88))
                    G.FillRectangle(Selection, New Rectangle(Rect.X - 6, Rect.Y + 2, Rect.Width + 8, Rect.Height - 2))
                End Using

                Using Lines As New Pen(Color.FromArgb(50, 55, 65))
                    G.DrawLine(Lines, Rect.X - 6, Rect.Y + 2, Rect.Width + 8, Rect.Y + 2)
                    G.DrawLine(Lines, Rect.X - 6, Rect.Y + 39, Rect.Width + 8, Rect.Y + 39)
                End Using

                Using TextBrush As New SolidBrush(Color.FromArgb(220, 220, 220))
                    G.DrawString(TabPages(T).Text, Font, TextBrush, New Point(Rect.X + 40, Rect.Y + 11))
                End Using

            Else

                Using TextBrush As New SolidBrush(Color.FromArgb(180, 180, 180))
                    G.DrawString(TabPages(T).Text, Font, TextBrush, New Point(Rect.X + 40, Rect.Y + 11))
                End Using

            End If

        Next

        MyBase.OnPaint(e)
    End Sub

End Class

Public Class rGroupBox
    Inherits GroupBox

    Private G As Graphics

    Sub New()
        DoubleBuffered = True
        Font = New Font("Segoe UI", 9)
        BackColor = Color.White
        ForeColor = Color.FromArgb(92, 94, 100)
        SetStyle(ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer, True)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)

        G = e.Graphics

        MyBase.OnPaint(e)

        G.Clear(Color.White)

        Using Border As New Pen(Color.FromArgb(235, 235, 235))
            G.DrawRectangle(Border, New Rectangle(0, 0, Width - 1, Height - 1))
        End Using

    End Sub

End Class

Public Class rProgressBar
    Inherits ProgressBar

    Private G As Graphics

    Sub New()
        DoubleBuffered = True
        Font = New Font("Segoe UI", 9)
        ForeColor = Color.FromArgb(200, 200, 200)
        SetStyle(ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer, True)
    End Sub

    Public Shadows Property Value As Integer
        Get
            Return MyBase.Value
        End Get
        Set(value As Integer)
            MyBase.Value = value
            Invalidate()
        End Set
    End Property

    Protected Overrides Sub OnPaint(e As PaintEventArgs)

        G = e.Graphics

        MyBase.OnPaint(e)

        G.Clear(Parent.BackColor)

        Using DefaultBrush As New SolidBrush(Color.FromArgb(239, 239, 239)), ValueBrush As New SolidBrush(Color.FromArgb(109, 140, 203))
            G.FillRectangle(DefaultBrush, New Rectangle(0, 0, Width, Height))
            G.FillRectangle(ValueBrush, New Rectangle(0, 0, CInt(Value / Maximum * Width), Height))
        End Using

    End Sub

End Class

Public Class rSeparator
    Inherits Control

    Private G As Graphics

    Sub New()
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)

        G = e.Graphics

        Using Separator As New Pen(Color.FromArgb(240, 240, 240))
            G.DrawLine(Separator, 0, 0, Width, 0)
        End Using

        MyBase.OnPaint(e)

    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        Size = New Size(Width, 6)
        MyBase.OnResize(e)
    End Sub

End Class

Public Class rBoxLabel
    Inherits Control

    Private G As Graphics

    Sub New()
        DoubleBuffered = True
        Font = New Font("Segoe UI", 9)
    End Sub

    Public Shadows Property Text As String
        Get
            Return MyBase.Text
        End Get
        Set(value As String)
            MyBase.Text = value
            Invalidate()
        End Set
    End Property

    Protected Overrides Sub OnPaint(e As PaintEventArgs)

        G = e.Graphics

        G.Clear(Parent.BackColor)

        Using Border As New Pen(Color.FromArgb(230, 230, 230))
            G.DrawRectangle(Border, New Rectangle(0, 0, Width - 1, Height - 1))
        End Using

        Using TextBrush As New SolidBrush(ForeColor)
            G.DrawString(Text, Font, TextBrush, MiddlePoint(G, Text, Font, New Rectangle(0, 0, Width, Height)))
        End Using

        MyBase.OnPaint(e)

    End Sub

End Class

Public Class rCheckBox
    Inherits CheckBox

    Private G As Graphics

    Private EnabledCheck As String = "iVBORw0KGgoAAAANSUhEUgAAAAsAAAALCAMAAACecocUAAAABGdBTUEAALGPC/xhBQAAACBjSFJNAAB6JgAAgIQAAPoAAACA6AAAdTAAAOpgAAA6mAAAF3CculE8AAABU1BMVEUAAABJVFNIUlhKVVdKVVVLVVdLVVZNVl1OU1dHVlVLU1NMVFdIV0dNVlhEVl1LVFZKVVVKVFdMVVdMVFhLVFZKVVVMVVdNV1dNU1ZKVVVJVFVLVVdKVVdKVVVLVFZLVFZMVVdMVFVLU1VLVVdKVVZNVFdLVVZMVVdLVFdKVVVOU1hMVFhKVVdKVFRMVVZMVVdKVVRIUlRLVFVMVFZLVVVQZ4lLU1FMVFZMVVdMVFZLVVZMVVdMVFZKP0FLVVZLVFZMVldMVVdLVVdMVVdLVFdJVVlMVFVMUlVMVVdMVVdMVVdMVVZDVE5OVFlKVVVLVVZLVVZMVVdNUldLVVdMVVdLVFZLVFhLVVZLVVdLVFZLVFZKVVVMVVdOVVlKU1VKU1Via2NNVFZNWFhMVVdMVVdMVVdMVVdMVVdMVVdMVVdMVVdMVVdMVVdMVVdMVVdMVVf///+oEs6nAAAAY3RSTlMAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAHGgADYNdEAAIABD/xxwECAAIg2uIwBCcGAQAMxfpMYP1+AqZsA8aRio0DARix/v2sBAIBCZPLEQIAAgNv2isDAAEBARwAAQDj9TSyAAAAAWJLR0Rw2ABsdAAAAAd0SU1FB+ABCw4GGQhePd8AAACISURBVAjXY2AAAklGKSZpGWYQk0GWRY5VXkGRQUlZhY1dlUNNPVmDQZNTS1uHS1cvRd+AwdDI2MTUzDzVwpKbwcrahsfWLi3dnteBwTEj08k5K9uFz9WNwd0jx9Mr19vH14+fQcA/IC8/MEgwWCiEITRMODwiUiRKNJqBQSwmNi4+IVE8SYIBAPBzGEl9vmIOAAAAJXRFWHRkYXRlOmNyZWF0ZQAyMDE2LTAxLTExVDE0OjA2OjI1LTA1OjAwV5YflwAAACV0RVh0ZGF0ZTptb2RpZnkAMjAxNi0wMS0xMVQxNDowNjoyNS0wNTowMCbLpysAAAAASUVORK5CYII="
    Private DisabledCheck As String = "iVBORw0KGgoAAAANSUhEUgAAAAsAAAALCAMAAACecocUAAAABGdBTUEAALGPC/xhBQAAACBjSFJNAAB6JgAAgIQAAPoAAACA6AAAdTAAAOpgAAA6mAAAF3CculE8AAABU1BMVEUAAACjpaecpaekpaelpaWmpaeopKmep52lpKenpqako6ampaedsKampaear5ympKelpaOmo6WmpKeopqqmpKelpaWmpaampqmmpqalpaekpaelpaekpaelpaWmpKempKempaelpKamo6empaelpaalpaalpaempaelpaalpaelo6epoqikpaWkpKWlpaempaelpaekoqWoo6ippKqgpqKRvJWooqmlpKampaelpaempaempaelpKa4kLmmpaempKempaampKempaempaampaehqaOooqmno6mmpaempaempaempaehpZ6opKqlpaWmpaSmpaempaempaWmpKempKempKempammpaempaempKempKelpaWmpaanpaemo6emo6emu7Gmpqempqmmpaempaempaempaempaempaempaempaempaempaempaempaempaf///9UdKmRAAAAY3RSTlMAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAHGgADYNdEAAIABD/xxwECAAIg2uIwBCcGAQAMxfpMYP1+AqZsA8aRio0DARix/v2sBAIBCZPLEQIAAgNv2isDAAEBARwAAQDj9TSyAAAAAWJLR0Rw2ABsdAAAAAd0SU1FB+ABCw4MOyfRlLEAAACISURBVAjXY2AAAklGKSZpGWYQk0GWRY5VXkGRQUlZhY1dlUNNPVmDQZNTS1uHS1cvRd+AwdDI2MTUzDzVwpKbwcrahsfWLi3dnteBwTEj08k5K9uFz9WNwd0jx9Mr19vH14+fQcA/IC8/MEgwWCiEITRMODwiUiRKNJqBQSwmNi4+IVE8SYIBAPBzGEl9vmIOAAAAJXRFWHRkYXRlOmNyZWF0ZQAyMDE2LTAxLTExVDE0OjEyOjU5LTA1OjAwUpYDQAAAACV0RVh0ZGF0ZTptb2RpZnkAMjAxNi0wMS0xMVQxNDoxMjo1OS0wNTowMCPLu/wAAAAASUVORK5CYII="

    Sub New()
        DoubleBuffered = True
        Font = New Font("Segoe UI", 8)
        SetStyle(ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer, True)
    End Sub

    Public Shadows Property Text As String
        Get
            Return MyBase.Text
        End Get
        Set(value As String)
            MyBase.Text = value
            Invalidate()
        End Set
    End Property

    Protected Overrides Sub OnPaint(e As PaintEventArgs)

        G = e.Graphics

        MyBase.OnPaint(e)

        G.Clear(Parent.BackColor)

        If Enabled Then

            Using Background As New SolidBrush(Color.FromArgb(240, 240, 240)), Border As New Pen(Color.FromArgb(200, 200, 200))
                G.FillRectangle(Background, New Rectangle(0, 0, 16, 16))
                G.DrawRectangle(Border, New Rectangle(0, 0, 16, 16))
            End Using

            Using TextBrush As New SolidBrush(ForeColor), TextFont As New Font("Segoe UI", 9)
                G.DrawString(Text, TextFont, TextBrush, New Point(20, 0))
            End Using

            If Checked Then

                Using Mark As Image = Image.FromStream(New IO.MemoryStream(Convert.FromBase64String(EnabledCheck)))
                    G.DrawImage(Mark, New Point(3, 3))
                End Using

            End If

        Else

            Using Background As New SolidBrush(Color.FromArgb(245, 245, 245)), Border As New Pen(Color.FromArgb(220, 220, 220))
                G.FillRectangle(Background, New Rectangle(0, 0, 16, 16))
                G.DrawRectangle(Border, New Rectangle(0, 0, 16, 16))
            End Using

            Using TextBrush As New SolidBrush(Color.FromArgb(165, 165, 165)), TextFont As New Font("Segoe UI", 9)
                G.DrawString(Text, TextFont, TextBrush, New Point(20, 0))
            End Using

            If Checked Then

                Using Mark As Image = Image.FromStream(New IO.MemoryStream(Convert.FromBase64String(DisabledCheck)))
                    G.DrawImage(Mark, New Point(3, 3))
                End Using

            End If

        End If

    End Sub

End Class

Public Class rRadioButton
    Inherits RadioButton

    Private G As Graphics

    Sub New()
        DoubleBuffered = True
        Font = New Font("Segoe UI", 8)
        SetStyle(ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer, True)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)

        G = e.Graphics
        G.SmoothingMode = SmoothingMode.HighQuality

        MyBase.OnPaint(e)

        G.Clear(Parent.BackColor)

        If Enabled Then

            Using Background As New SolidBrush(Color.FromArgb(240, 240, 240)), Border As New Pen(Color.FromArgb(200, 200, 200))
                G.FillEllipse(Background, New Rectangle(0, 0, 16, 16))
                G.DrawEllipse(Border, New Rectangle(0, 0, 16, 16))
            End Using

            Using TextBrush As New SolidBrush(ForeColor), TextFont As New Font("Segoe UI", 9)
                G.DrawString(Text, TextFont, TextBrush, New Point(20, 0))
            End Using

            If Checked Then

                Using Fill As New SolidBrush(Color.FromArgb(76, 85, 87))
                    G.FillEllipse(Fill, New Rectangle(4, 4, 8, 8))
                End Using

            End If

        Else

            Using Background As New SolidBrush(Color.FromArgb(245, 245, 245)), Border As New Pen(Color.FromArgb(220, 220, 220))
                G.FillEllipse(Background, New Rectangle(0, 0, 16, 16))
                G.DrawEllipse(Border, New Rectangle(0, 0, 16, 16))
            End Using

            Using TextBrush As New SolidBrush(Color.FromArgb(165, 165, 165)), TextFont As New Font("Segoe UI", 9)
                G.DrawString(Text, TextFont, TextBrush, New Point(20, 0))
            End Using

            If Checked Then

                Using Fill As New SolidBrush(Color.FromArgb(165, 165, 165))
                    G.FillEllipse(Fill, New Rectangle(4, 4, 8, 8))
                End Using

            End If

        End If

    End Sub

End Class

Public Class rAlertBox
    Inherits Control

    Private G As Graphics
    Private ClearColor, BorderColor, TextColor As Color

    Public Property Style As Styles = Styles.Yellow

    Enum Styles As Byte
        Yellow = 0
        Blue = 1
        Red = 2
        Green = 3
    End Enum

    Sub New()
        DoubleBuffered = True
        Font = New Font("Segoe UI", 9)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)

        G = e.Graphics

        Select Case Style

            Case Styles.Yellow
                ClearColor = Color.FromArgb(254, 251, 242)
                BorderColor = Color.FromArgb(250, 247, 238)
                TextColor = Color.FromArgb(154, 144, 114)

            Case Styles.Blue
                ClearColor = Color.FromArgb(222, 232, 242)
                BorderColor = Color.FromArgb(218, 228, 238)
                TextColor = Color.FromArgb(113, 142, 154)

            Case Styles.Red
                ClearColor = Color.FromArgb(242, 222, 222)
                BorderColor = Color.FromArgb(238, 218, 218)
                TextColor = Color.FromArgb(154, 113, 113)

            Case Styles.Green
                ClearColor = Color.FromArgb(222, 242, 222)
                BorderColor = Color.FromArgb(218, 238, 218)
                TextColor = Color.FromArgb(120, 154, 113)

        End Select

        G.Clear(ClearColor)

        Using Border As New Pen(BorderColor)
            G.DrawRectangle(Border, New Rectangle(0, 0, Width - 1, Height - 1))
        End Using

        Using TextBrush As New SolidBrush(TextColor)
            G.DrawString(Text, Font, TextBrush, MiddlePoint(G, Text, Font, New Rectangle(0, 0, Width, Height)))
        End Using

        MyBase.OnPaint(e)

    End Sub

End Class

Public Class rButton
    Inherits Button

    Private G As Graphics

    Public Property Blue As Boolean = True

    Private State As MouseState
    Private Gradient As LinearGradientBrush

    Sub New()
        DoubleBuffered = True
        Cursor = Cursors.Hand
        Font = New Font("Segoe UI", 9)
        SetStyle(ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer, True)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)

        G = e.Graphics

        MyBase.OnPaint(e)

        G.Clear(Color.White)

        If Blue Then

            Select Case State

                Case MouseState.None
                    Gradient = New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(119, 150, 213), Color.FromArgb(109, 140, 203), LinearGradientMode.Vertical)

                Case MouseState.Down
                    Gradient = New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(109, 140, 203), Color.FromArgb(99, 130, 193), LinearGradientMode.Vertical)

                Case MouseState.Over
                    Gradient = New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(129, 160, 223), Color.FromArgb(119, 150, 213), LinearGradientMode.Vertical)

            End Select

            Using Border As New Pen(Color.FromArgb(110, 134, 181))
                G.FillPath(Gradient, RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 1))
                G.DrawPath(Border, RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 1))
            End Using

            G.DrawString(Text, Font, Brushes.White, MiddlePoint(G, Text, Font, New Rectangle(0, 0, Width, Height)))

        Else

            Select Case State

                Case MouseState.None
                    Gradient = New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(248, 249, 251), Color.FromArgb(237, 238, 242), LinearGradientMode.Vertical)

                Case MouseState.Down
                    Gradient = New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(243, 244, 246), Color.FromArgb(232, 233, 237), LinearGradientMode.Vertical)

                Case MouseState.Over
                    Gradient = New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(254, 255, 255), Color.FromArgb(242, 243, 248), LinearGradientMode.Vertical)

            End Select

            Using Border As New Pen(Color.FromArgb(209, 220, 224))
                G.FillPath(Gradient, RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 1))
                G.DrawPath(Border, RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 1))
            End Using

            Using TextBrush As New SolidBrush(Color.FromArgb(129, 140, 144))
                G.DrawString(Text, Font, TextBrush, MiddlePoint(G, Text, Font, New Rectangle(1, 1, Width - 1, Height - 1)))
            End Using

        End If

    End Sub

    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        State = MouseState.Over : Invalidate()
        MyBase.OnMouseEnter(e)
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        State = MouseState.None : Invalidate()
        MyBase.OnMouseLeave(e)
    End Sub

    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        State = MouseState.Over : Invalidate()
        MyBase.OnMouseUp(e)
    End Sub

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        State = MouseState.Down : Invalidate()
        MyBase.OnMouseDown(e)
    End Sub

End Class

Public Class rTextBox
    Inherits Control

    Private G As Graphics
    Private WithEvents T As TextBox
    Private State As MouseState

    Public Shadows Property Text As String
        Get
            Return T.Text
        End Get
        Set(value As String)
            MyBase.Text = value
            T.Text = value
            Invalidate()
        End Set
    End Property

    Public Shadows Property Enabled As Boolean
        Get
            Return T.Enabled
        End Get
        Set(value As Boolean)
            T.Enabled = value
            Invalidate()
        End Set
    End Property

    Public Property UseSystemPasswordChar As Boolean
        Get
            Return T.UseSystemPasswordChar
        End Get
        Set(value As Boolean)
            T.UseSystemPasswordChar = value
            Invalidate()
        End Set
    End Property

    Public Property MultiLine() As Boolean
        Get
            Return T.Multiline
        End Get
        Set(ByVal value As Boolean)
            T.Multiline = value
            Size = New Size(T.Width + 2, T.Height + 2)
            Invalidate()
        End Set
    End Property

    Public Property MaxLength As Integer
        Get
            Return T.MaxLength
        End Get
        Set(ByVal value As Integer)
            T.MaxLength = value
            Invalidate()
        End Set
    End Property

    Public Shadows Property [ReadOnly]() As Boolean
        Get
            Return T.ReadOnly
        End Get
        Set(ByVal value As Boolean)
            T.ReadOnly = value
            Invalidate()
        End Set
    End Property

    Protected Overrides Sub OnGotFocus(e As EventArgs)
        T.Focus()
        MyBase.OnGotFocus(e)
    End Sub

    Sub New()
        DoubleBuffered = True

        T = New TextBox With {
            .BorderStyle = BorderStyle.None,
            .BackColor = Color.White,
            .ForeColor = ForeColor,
            .Location = New Point(6, 5)}

        Controls.Add(T)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)

        G = e.Graphics

        If Enabled Then

            T.BackColor = Color.White

            Using Border As New Pen(Color.FromArgb(235, 235, 235))
                G.DrawRectangle(Border, New Rectangle(0, 0, Width - 1, Height - 1))
            End Using

            If State = MouseState.Down Then

                Using Border As New Pen(Color.FromArgb(109, 140, 203))
                    G.DrawRectangle(Border, New Rectangle(0, 0, Width - 1, Height - 1))
                End Using

            End If

        Else

            T.BackColor = Color.FromArgb(245, 245, 245)

            Using Border As New Pen(Color.FromArgb(240, 240, 240))
                G.DrawRectangle(Border, New Rectangle(0, 0, Width - 1, Height - 1))
            End Using

        End If

        MyBase.OnPaint(e)

    End Sub

    Protected Overrides Sub OnEnter(e As EventArgs)
        State = MouseState.Down : Invalidate()
        MyBase.OnEnter(e)
    End Sub

    Protected Overrides Sub OnLeave(e As EventArgs)
        State = MouseState.None : Invalidate()
        MyBase.OnLeave(e)
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        If MultiLine Then
            T.Size = New Size(Width - 10, Height - 10)
        Else
            T.Size = New Size(Width - 10, T.Height - 5)
        End If

        MyBase.OnResize(e)
    End Sub

    Private Sub TTextChanged() Handles T.TextChanged
        MyBase.OnTextChanged(EventArgs.Empty)
    End Sub

End Class

Public Class rTag
    Inherits Control

    Private G As Graphics

    Public Property Background As Color = Color.Red

    Public Property Border As Color = Color.DarkRed

    Sub New()
        DoubleBuffered = True
        Font = New Font("Segoe UI", 7, FontStyle.Bold)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)

        G = e.Graphics

        Using Back As New SolidBrush(Background), Bord As New Pen(Border)
            G.FillPath(Back, RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 2))
            G.DrawPath(Bord, RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 2))
        End Using

        G.DrawString(Text, Font, Brushes.White, MiddlePoint(G, Text, Font, New Rectangle(0, 0, Width, Height)))

        MyBase.OnPaint(e)
    End Sub

End Class