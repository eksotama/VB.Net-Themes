Public Module Helpers

    Public Enum MouseState
        Hover = 1
        Down = 2
        None = 3
    End Enum
    Public Enum TxtAlign
        Left = 1
        Center = 2
        Right = 3
    End Enum

    Public Function b64Image(ByVal b64 As String) As Image
        Return Image.FromStream(New System.IO.MemoryStream(Convert.FromBase64String(b64)))
    End Function

    Public Function FromHex(hex As String) As Color
        Return ColorTranslator.FromHtml(hex)
    End Function
End Module

Public Class VelocityButton
    Inherits Control

    Private state As MouseState = MouseState.None
    Private _enabled As Boolean = True

    Private _txtAlign As TxtAlign = TxtAlign.Center
    Public Property TextAlign As TxtAlign
        Get
            Return _txtAlign
        End Get
        Set(value As TxtAlign)
            _txtAlign = value
            Invalidate()
        End Set
    End Property

    Sub New()
        DoubleBuffered = True
        Font = New Font("Segoe UI Semilight", 9)
        ForeColor = Color.White
        Size = New Size(94, 40)
    End Sub

    Public Overloads Property Enabled As Boolean
        Get
            Return _enabled
        End Get
        Set(value As Boolean)
            _enabled = value
            Invalidate()
        End Set
    End Property

    Sub PerformClick()
        MyBase.OnClick(EventArgs.Empty)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g As Graphics = e.Graphics
        Select Case _enabled
            Case True
                Select Case state
                    Case MouseState.None
                        g.Clear(FromHex("#435363"))
                    Case MouseState.Hover
                        g.Clear(FromHex("#38495A"))
                    Case MouseState.Down
                        g.Clear(BackColor)
                        g.FillRectangle(New SolidBrush(FromHex("#2c3e50")), 1, 1, Width - 2, Height - 2)
                End Select
            Case False
                g.Clear(FromHex("#38495A"))
        End Select

        Select Case _txtAlign
            Case TxtAlign.Left
                g.DrawString(Text, Font, New SolidBrush(ForeColor), New Rectangle(8, 0, Width, Height), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})
            Case TxtAlign.Center
                g.DrawString(Text, Font, New SolidBrush(ForeColor), New Rectangle(0, 0, Width, Height), New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
            Case TxtAlign.Right
                g.DrawString(Text, Font, New SolidBrush(ForeColor), New Rectangle(0, 0, Width - 8, Height), New StringFormat With {.Alignment = StringAlignment.Far, .LineAlignment = StringAlignment.Center})
        End Select
    End Sub

    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        MyBase.OnMouseEnter(e)
        state = MouseState.Hover : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseHover(e As EventArgs)
        MyBase.OnMouseHover(e)
        state = MouseState.Hover : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        state = MouseState.None : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        state = MouseState.Down : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        state = MouseState.Hover : Invalidate()
    End Sub

    Protected Overrides Sub OnTextChanged(e As EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub
End Class

<DefaultEvent("CheckChanged")> _
Public Class VelocityCheckBox
    Inherits Control

    Dim _state As MouseState = MouseState.None
    Public Event CheckChanged(sender As Object, e As EventArgs)

    Private _autoSize As Boolean = True
    Public Overrides Property AutoSize As Boolean
        Get
            Return _autoSize
        End Get
        Set(value As Boolean)
            _autoSize = value
            Invalidate()
        End Set
    End Property

    Private _checked As Boolean = False
    Public Property Checked As Boolean
        Get
            Return _checked
        End Get
        Set(value As Boolean)
            _checked = value
            Invalidate()
        End Set
    End Property

    Sub New()
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        Select Case AutoSize
            Case True
                Size = New Size(TextRenderer.MeasureText(Text, Font).Width + 28, Height)
        End Select
        Dim g As Graphics = e.Graphics
        Select Case _state
            Case MouseState.Hover
                g.FillRectangle(New SolidBrush(FromHex("#DBDBDB")), 4, 4, 14, 14)
            Case Else
                g.FillRectangle(Brushes.White, 4, 4, 14, 14)
        End Select
        If _checked Then
            g.FillRectangle(New SolidBrush(FromHex("#435363")), 7, 7, 9, 9)
        End If
        g.DrawRectangle(New Pen(FromHex("#435363")), New Rectangle(4, 4, 14, 14))
        g.DrawString(Text, Font, New SolidBrush(ForeColor), New Rectangle(22, 0, Width, Height), New StringFormat With {.LineAlignment = StringAlignment.Center})
    End Sub

    Protected Overrides Sub OnTextChanged(e As EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        Select Case Checked
            Case True
                Checked = False
            Case False
                Checked = True
        End Select
        _state = MouseState.Hover : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        MyBase.OnMouseEnter(e)
        _state = MouseState.Hover : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseHover(e As EventArgs)
        MyBase.OnMouseHover(e)
        _state = MouseState.Hover : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        _state = MouseState.None : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        _state = MouseState.Down : Invalidate()
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        Invalidate()
    End Sub
End Class

<DefaultEvent("CheckChanged")> _
Public Class VelocityRadioButton
    Inherits Control

    Dim _state As MouseState 
    Public Event CheckChanged(sender As Object, e As EventArgs)

    Private _autoSize As Boolean = True
    Public Overrides Property AutoSize As Boolean
        Get
            Return _autoSize
        End Get
        Set(value As Boolean)
            _autoSize = value
            Invalidate()
        End Set
    End Property

    Private _checked As Boolean = False
    Public Property Checked As Boolean
        Get
            Return _checked
        End Get
        Set(value As Boolean)
            _checked = value
            Invalidate()
        End Set
    End Property

    Sub New()
        DoubleBuffered = True
        InvalidateControls()
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        Select Case AutoSize
            Case True
                Size = New Size(TextRenderer.MeasureText(Text, Font).Width + 24, Height)
        End Select
        Dim g As Graphics = e.Graphics
        g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        Select Case _state
            Case MouseState.Hover
                g.FillEllipse(New SolidBrush(FromHex("#DBDBDB")), 4, 4, 14, 14)
            Case Else
                g.FillEllipse(Brushes.White, 4, 4, 14, 14)
        End Select
        g.DrawEllipse(New Pen(FromHex("#435363")), New Rectangle(4, 4, 14, 14))
        g.DrawString(Text, Font, New SolidBrush(ForeColor), New Rectangle(22, 0, Width, Height), New StringFormat With {.LineAlignment = StringAlignment.Center})
        If _checked = True Then
            g.FillEllipse(New SolidBrush(FromHex("#435363")), 7, 7, 8, 8)
        End If
    End Sub

    Private Sub InvalidateControls()
        If Not IsHandleCreated OrElse Not _checked Then Return
        For Each C As Control In Parent.Controls
            If C IsNot Me AndAlso TypeOf C Is VelocityRadioButton Then
                DirectCast(C, VelocityRadioButton).Checked = False
                Invalidate()
            End If
        Next
    End Sub

    Protected Overrides Sub OnTextChanged(e As EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        _state = MouseState.Hover
        Select Case Checked
            Case True
                Checked = False
            Case False
                Checked = True
        End Select
        _state = MouseState.Hover
        InvalidateControls()
    End Sub

    Protected Overrides Sub OnMouseHover(e As EventArgs)
        MyBase.OnMouseHover(e)
        _state = MouseState.Hover : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        _state = MouseState.None : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        _state = MouseState.None : Invalidate()
    End Sub 

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        Invalidate()
    End Sub
End Class

Public Class VelocityTitle
    Inherits Control 

    Private _txtAlign As TxtAlign = TxtAlign.Left
    Public Property TextAlign As TxtAlign
        Get
            Return _txtAlign
        End Get
        Set(value As TxtAlign)
            _txtAlign = value
            Invalidate()
        End Set
    End Property

    Sub New()
        DoubleBuffered = True
        Size = New Size(180, 23)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g As Graphics = e.Graphics
        g.DrawLine(New Pen(FromHex("#435363")), New Point(0, Height / 2), New Point(Width, Height / 2)) 
            Dim txtRect As New Size(g.MeasureString(Text, Font).ToSize)
            Select Case _txtAlign
                Case TxtAlign.Left
                    g.FillRectangle(New SolidBrush(BackColor), New Rectangle(18, Height / 2 - txtRect.Height - 2, txtRect.Width + 6, Height / 2 + txtRect.Height / 2 + 6))
                    g.DrawString(Text, Font, New SolidBrush(ForeColor), 20, Height / 2 - txtRect.Height / 2)
                Case TxtAlign.Center
                    g.FillRectangle(New SolidBrush(BackColor), New Rectangle(Width / 2 - txtRect.Width / 2 - 2, Height / 2 - txtRect.Height / 2 - 2, txtRect.Width + 2, txtRect.Height + 2))
                    g.DrawString(Text, Font, New SolidBrush(ForeColor), Width / 2 - txtRect.Width / 2, Height / 2 - txtRect.Height / 2)
                Case TxtAlign.Right
                    g.FillRectangle(New SolidBrush(BackColor), New Rectangle(Width - (txtRect.Width + 18), Height / 2 - txtRect.Height - 2, txtRect.Width + 4, Height + 6))
                    g.DrawString(Text, Font, New SolidBrush(ForeColor), Width - (txtRect.Width + 16), Height / 2 - txtRect.Height / 2)
            End Select 
    End Sub

    Protected Overrides Sub OnFontChanged(e As EventArgs)
        MyBase.OnFontChanged(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnTextChanged(e As EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub
End Class

Public Class VelocitySplitter
    Inherits Control

    Private _offset As Integer = 8
    Public Property Offset As Integer
        Get
            Return _offset
        End Get
        Set(value As Integer)
            _offset = value
            Invalidate()
        End Set
    End Property
    Sub New()
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g As Graphics = e.Graphics
        g.DrawLine(New Pen(ForeColor), New Point(_offset, Height / 2 - 2), New Point(Width - _offset, Height / 2 - 1))
    End Sub
End Class

<DefaultEvent("XClicked")> _
Public Class VelocityAlert
    Inherits Control

    Public Event XClicked(sender As Object, e As EventArgs)
    Dim _xHover As Boolean = False

#Region "Filler Image Base64"
    Dim FillerImage As String = "iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAIAAAAlC+aJAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAZdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuMTM0A1t6AAADZUlEQVRoQ+2W2ytsYRjG9x85Cikpp1KORSkihITkyinkdCERm0wi3IiS5ELKIQkXkiQkh9m/tZ7PNHuwWi707dl9v4vpfd/1zut51ncYvyIpzv9iIJaCSLkzYA8pdwbsIeXOgD2k3Bmwh5Q7A/aQcmfAHlLuDNhDyp0Be0i5M2APKXcG7CHlzoA9pPx7Bh4fH4+Pj/f29m5vb03pHSpnZ2cmCeTt7Y1OhlxdXZnSO8/PzyGHgJR/w8Dc3Fx6err6s7OzTdXn9fW1sLCQ+urqqil9AQ15eXkaAkdHR+aBT21tLcWJiQmTB6IJYQ1sbW3RtrKyovTy8lKBGBsb05zl5WVT+ozT01N6pqenlSatwNLSkoaMjIyYUiBqDmugoaGhr6/PJH/DojNhf3+fz2g0SoXPqampu7s7Nfz2Iejt7W1sbFQxiaenJ1YG/wwZHBw01UA83eENsHnW1tZ4Z4eHh/f396bq09bW1tLSQpE5MgBFRUWdnZ0EJycn1Dc2NojLysrYHhhjSNIpGh4ezs/PJ6C5v79fxWA83SENXF9f01NVVaVmGBgY0KPNzU1SgiQDKCZlx1dUVHR0dKiYlZVVWlrqfd+nvb1ddS4G0ouLC2ICFkr1YPwZ4Qycn5/TMzQ0pHRmZoYU6cTl5eXaHkkGoLm52Rsdidzc3JBy+RC3trZqASV6dnaWmAXs6enxvvNDBlh0era3t00eixUUFKCb45iZmcnqj4+P6xwjJf5eFxYWqLACSiEnJ4erzCSxWF1dHedKa8W+ZwIbjJillrFg6ISwZyA3N3d+fl4xvwZpaWmsAAa6u7u7uro4Bk1NTcwpLi4uKSmhhy3OsRkdHaUY/2JNTU187wHNTMAAQwDnGsJhYKZp+hpPd3gD/IHq6moOw8vLC++JG4O73zzzeXh4YA4HXSn99fX1BLzajIwM7aLJyUl+Lg4ODogXFxfpZ3N63QmwpD9yjfJGWXE18+Z2d3fNg3cSDezs7BDHf1MrKytJFbNW3ohIhO20vr6uYiIsNXvJJIFoTlgDgs3DhW2SD3z8/+JTmJB0EScScghI+fcM/FNIuTNgDyl3Buwh5c6APaTcGbCHlDsD9pByZ8AeUu4M2EPKnQF7SLkzYA8pdwbsIeXOgD2k3Bmwh5QbA6lLihuIRP4AXubLj7lh8ksAAAAASUVORK5CYII="
#End Region

    Private _xChangeCursor As Boolean = True
    Public Property XChangeCursor As Boolean
        Get
            Return _xChangeCursor
        End Get
        Set(value As Boolean)
            _xChangeCursor = value
            Invalidate()
        End Set
    End Property

    Private _title As String = Name
    Public Property Title As String
        Get
            Return _title
        End Get
        Set(value As String)
            _title = value
            Invalidate()
        End Set
    End Property

    Private _exitButton As Boolean = False
    Public Property ShowExit As Boolean
        Get
            Return _exitButton
        End Get
        Set(value As Boolean)
            _exitButton = value
            Invalidate()
        End Set
    End Property

    Private _showImage As Boolean = True
    Public Property ShowImage As Boolean
        Get
            Return _showImage
        End Get
        Set(value As Boolean)
            _showImage = value
            Invalidate()
        End Set
    End Property

    Private _image As Image
    Public Property Image As Image
        Get
            Return _image
        End Get
        Set(value As Image)
            _image = value
            Invalidate()
        End Set
    End Property

    Private _border As Color = FromHex("#435363")
    Public Property Border As Color
        Get
            Return _border
        End Get
        Set(value As Color)
            _border = value
            Invalidate()
        End Set
    End Property

    Sub New() 
        Size = New Size(370, 80)
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g As Graphics = e.Graphics
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit


        Select Case ShowImage
            Case True
                If _image Is Nothing Then
                    g.DrawImage(b64Image(FillerImage), 13, 8)
                Else
                    g.DrawImage(_image, 12, 8, 64, 64)
                End If
                g.DrawString(_title, New Font("Segoe UI Semilight", 14), New SolidBrush(ForeColor), 84, 6)
                g.DrawString(Text, Font, New SolidBrush(ForeColor), New Rectangle(86, 33, Width - 88, Height - 10))
            Case False
                g.DrawString(_title, New Font("Segoe UI Semilight", 14), New SolidBrush(ForeColor), 18, 6)
                g.DrawString(Text, Font, New SolidBrush(ForeColor), New Rectangle(20, 33, Width - 28, Height - 10))
        End Select

        If ShowExit Then
            If _xHover Then
                g.DrawString("r", New Font("Marlett", 9), New SolidBrush(FromHex("#596372")), Width - 18, 4)
            Else
                g.DrawString("r", New Font("Marlett", 9), New SolidBrush(FromHex("#435363")), Width - 18, 4)
            End If
        End If

        g.DrawRectangle(New Pen(_border), 0, 0, Width - 1, Height - 1)
        g.FillRectangle(New SolidBrush(_border), 0, 0, 6, Height)
    End Sub

    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)
        If _exitButton Then
            If New Rectangle(Width - 16, 4, 12, 13).Contains(e.X, e.Y) Then
                _xHover = True
                If _xChangeCursor Then
                    Cursor = Cursors.Hand
                End If
            Else
                _xHover = False
                Cursor = Cursors.Default
            End If
        End If
        Invalidate()
    End Sub 

    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        If _exitButton Then
            If _xHover Then
                RaiseEvent XClicked(Me, EventArgs.Empty)
            End If
        End If
    End Sub

    Protected Overrides Sub OnTextChanged(e As EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub
End Class

Public Class VelocityTabControl
    Inherits TabControl

    Private _overtab As Integer = 0

    Private _txtAlign As TxtAlign = TxtAlign.Center
    Public Property TextAlign As TxtAlign
        Get
            Return _txtAlign
        End Get
        Set(value As TxtAlign)
            _txtAlign = value
            Invalidate()
        End Set
    End Property

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint, True)
        DoubleBuffered = True
        SizeMode = TabSizeMode.Fixed
        ItemSize = New Size(40, 130)
        Alignment = TabAlignment.Left
        Font = New Font("Segoe UI Semilight", 9)
    End Sub
     
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim b As New Bitmap(Width, Height)
        Dim g As Graphics = Graphics.FromImage(b)
        g.Clear(FromHex("#435363"))
        For i = 0 To TabCount - 1
            Dim tabRect As Rectangle = GetTabRect(i)
            If i = SelectedIndex Then
                g.FillRectangle(New SolidBrush(FromHex("#2c3e50")), tabRect)
            ElseIf i = _overtab Then
                g.FillRectangle(New SolidBrush(FromHex("#435363")), tabRect)
            Else
                g.FillRectangle(New SolidBrush(FromHex("#38495A")), tabRect)
            End If
            Select Case _txtAlign
                Case TxtAlign.Left
                    g.DrawString(TabPages(i).Text, Font, Brushes.White, New Rectangle(tabRect.X + 8, tabRect.Y, tabRect.Width, tabRect.Height), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})
                Case TxtAlign.Center
                    g.DrawString(TabPages(i).Text, Font, Brushes.White, tabRect, New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                Case TxtAlign.Right
                    g.DrawString(TabPages(i).Text, Font, Brushes.White, New Rectangle(tabRect.X - 8, tabRect.Y, tabRect.Width, tabRect.Height), New StringFormat With {.Alignment = StringAlignment.Far, .LineAlignment = StringAlignment.Center})
            End Select
        Next

        e.Graphics.DrawImage(b.Clone, 0, 0)
        g.Dispose() : b.Dispose()
    End Sub

    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)
        For i = 0 To TabPages.Count - 1
            If GetTabRect(i).Contains(e.Location) Then
                _overtab = i
            End If
            Invalidate()
        Next
    End Sub
End Class

Public Class VelocityTag
    Inherits Control

    Private _border As Color = FromHex("#2c3e50")
    Public Property Border As Color
        Get
            Return _border
        End Get
        Set(value As Color)
            _border = value
            Invalidate()
        End Set
    End Property

    Sub New()
        DoubleBuffered = True
        BackColor = FromHex("#34495e")
        ForeColor = Color.White
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g As Graphics = e.Graphics
        g.Clear(BackColor)
        g.DrawRectangle(New Pen(_border), 0, 0, Width - 1, Height - 1)
        g.DrawString(Text, Font, New SolidBrush(ForeColor), New Rectangle(0, 0, Width, Height), New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
    End Sub

    Protected Overrides Sub OnTextChanged(e As EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub
End Class
 
Public Class VelocityProgressBar
    Inherits Control

    Private _border As Color = FromHex("#485e75")
    Public Property Border As Color
        Get
            Return _border
        End Get
        Set(value As Color)
            _border = value
            Invalidate()
        End Set
    End Property

    Private _progressColor As Color = FromHex("#2c3e50")
    Public Property ProgressColor As Color
        Get
            Return _progressColor
        End Get
        Set(value As Color)
            _progressColor = value
            Invalidate()
        End Set
    End Property

    Private _val As Integer = 0
    Public Property Value As Integer
        Get
            Return _val
        End Get
        Set(value As Integer)
            _val = value
            ValChanged()
            Invalidate()
        End Set
    End Property

    Private _min As Integer = 0
    Public Property Min As Integer
        Get
            Return _min
        End Get
        Set(value As Integer)
            _min = value
            Invalidate()
        End Set
    End Property

    Private _max As Integer = 100
    Public Property Max As Integer
        Get
            Return _max
        End Get
        Set(value As Integer)
            _max = value
            Invalidate()
        End Set
    End Property

    Private _showPercent As Boolean = False
    Public Property ShowPercent As Boolean
        Get
            Return _showPercent
        End Get
        Set(value As Boolean)
            _showPercent = value
            Invalidate()
        End Set
    End Property

    Private Sub ValChanged()
        If _val > _max Then
            _val = _max
        End If
    End Sub

    Sub New()
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g As Graphics = e.Graphics

        If _showPercent Then
            g.FillRectangle(New SolidBrush(FromHex("#506070")), 0, 0, Width - 35, Height - 1)
            g.FillRectangle(New SolidBrush(_progressColor), New Rectangle(0, 0, _val * (Width - 35) / (_max - _min), Height))
            g.DrawRectangle(New Pen(Color.Black), 0, 0, Width - 35, Height - 1)
            g.DrawString(_val & "%", Font, New SolidBrush(ForeColor), Width - 30, Height / 2 - g.MeasureString(_val & "%", Font).Height / 2 - 1)
        Else
            g.Clear(FromHex("#506070"))
            g.FillRectangle(New SolidBrush(_progressColor), New Rectangle(0, 0, (_val - 0) * (Width - 0) / (_max - _min) + 0, Height))
            g.DrawRectangle(New Pen(Color.Black), 0, 0, Width - 1, Height - 1)
        End If
    End Sub
End Class

Public Class VelocityToggle
    Inherits Control

    Private _checked As Boolean = False
    Public Property Checked As Boolean
        Get
            Return _checked
        End Get
        Set(value As Boolean)
            _checked = value
            Invalidate()
        End Set
    End Property 

    Sub New()
        Size = New Size(50, 23)
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g As Graphics = e.Graphics
        g.Clear(FromHex("#435363"))

        Select Case _checked
            Case True
                g.FillRectangle(Brushes.White, Width - 19, Height - 19, 15, 15)
            Case False
                g.FillRectangle(New SolidBrush(FromHex("#2c3e50")), 4, 4, 15, 15)
        End Select 
    End Sub

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        _checked = Not (_checked)
        Invalidate()
    End Sub
End Class