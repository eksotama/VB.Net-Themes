Imports System.Windows.Forms
Imports System.Drawing.Drawing2D
Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports System.Drawing

'.::Tweety Theme::.
'Author:   UnReLaTeDScript
'Converted to C# by: Delirium™ @ HackForums.Net
'Credits:  Aeonhack [Themebase]
'Version:  1.0
MustInherit Class Theme
	Inherits ContainerControl

	#Region " Initialization "

	Protected G As Graphics
	Public Sub New()
		SetStyle(CType(139270, ControlStyles), True)
	End Sub

	Private ParentIsForm As Boolean
	Protected Overrides Sub OnHandleCreated(e As EventArgs)
		Dock = DockStyle.Fill
		ParentIsForm = TypeOf Parent Is Form
		If ParentIsForm Then
			If Not (_TransparencyKey = Color.Empty) Then
				ParentForm.TransparencyKey = _TransparencyKey
			End If
			ParentForm.FormBorderStyle = FormBorderStyle.None
		End If
		MyBase.OnHandleCreated(e)
	End Sub

	Public Overrides Property Text() As String
		Get
			Return MyBase.Text
		End Get
		Set
			MyBase.Text = value
			Invalidate()
		End Set
	End Property
	#End Region

	#Region " Sizing and Movement "

	Private _Resizable As Boolean = True
	Public Property Resizable() As Boolean
		Get
			Return _Resizable
		End Get
		Set
			_Resizable = value
		End Set
	End Property

	Private _MoveHeight As Integer = 24
	Public Property MoveHeight() As Integer
		Get
			Return _MoveHeight
		End Get
		Set
			_MoveHeight = value
			Header = New Rectangle(7, 7, Width - 14, _MoveHeight - 7)
		End Set
	End Property

	Private Flag As IntPtr
	Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
		If Not (e.Button = MouseButtons.Left) Then
			Return
		End If
		If ParentIsForm Then
			If ParentForm.WindowState = FormWindowState.Maximized Then
				Return
			End If
		End If

		If Header.Contains(e.Location) Then
			Flag = New IntPtr(2)
		ElseIf Current.Position = 0 Or Not _Resizable Then
			Return
		Else
			Flag = New IntPtr(Current.Position)
		End If

		Capture = False
		Dim m As Message = Message.Create(Parent.Handle, 161, Flag, IntPtr.Zero)
		DefWndProc(m)

		MyBase.OnMouseDown(e)
	End Sub

	Private Structure Pointer
		Public ReadOnly Cursor As Cursor
		Public ReadOnly Position As Byte
		Public Sub New(c As Cursor, p As Byte)
			Cursor = c
			Position = p
		End Sub
	End Structure

	Private F1 As Boolean
	Private F2 As Boolean
	Private F3 As Boolean
	Private F4 As Boolean
	Private PTC As Point
	Private Function GetPointer() As Pointer
		PTC = PointToClient(MousePosition)
		F1 = PTC.X < 7
		F2 = PTC.X > Width - 7
		F3 = PTC.Y < 7
		F4 = PTC.Y > Height - 7

		If F1 And F3 Then
			Return New Pointer(Cursors.SizeNWSE, 13)
		End If
		If F1 And F4 Then
			Return New Pointer(Cursors.SizeNESW, 16)
		End If
		If F2 And F3 Then
			Return New Pointer(Cursors.SizeNESW, 14)
		End If
		If F2 And F4 Then
			Return New Pointer(Cursors.SizeNWSE, 17)
		End If
		If F1 Then
			Return New Pointer(Cursors.SizeWE, 10)
		End If
		If F2 Then
			Return New Pointer(Cursors.SizeWE, 11)
		End If
		If F3 Then
			Return New Pointer(Cursors.SizeNS, 12)
		End If
		If F4 Then
			Return New Pointer(Cursors.SizeNS, 15)
		End If
		Return New Pointer(Cursors.[Default], 0)
	End Function

	Private Current As Pointer
	Private Pending As Pointer
	Private Sub SetCurrent()
		Pending = GetPointer()
		If Current.Position = Pending.Position Then
			Return
		End If
		Current = GetPointer()
		Cursor = Current.Cursor
	End Sub

	Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
		If _Resizable Then
			SetCurrent()
		End If
		MyBase.OnMouseMove(e)
	End Sub

	Protected Header As Rectangle
	Protected Overrides Sub OnSizeChanged(e As EventArgs)
		If Width = 0 OrElse Height = 0 Then
			Return
		End If
		Header = New Rectangle(7, 7, Width - 14, _MoveHeight - 7)
		Invalidate()
		MyBase.OnSizeChanged(e)
	End Sub

	#End Region

	#Region " Convienence "

	Public MustOverride Sub PaintHook()
	Protected Overrides NotOverridable Sub OnPaint(e As PaintEventArgs)
		If Width = 0 OrElse Height = 0 Then
			Return
		End If
		G = e.Graphics
		PaintHook()
	End Sub

	Private _TransparencyKey As Color
	Public Property TransparencyKey() As Color
		Get
			Return _TransparencyKey
		End Get
		Set
			_TransparencyKey = value
			Invalidate()
		End Set
	End Property

	Private _Image As Image
	Public Property Image() As Image
		Get
			Return _Image
		End Get
		Set
			_Image = value
			Invalidate()
		End Set
	End Property
	Public ReadOnly Property ImageWidth() As Integer
		Get
			If _Image Is Nothing Then
				Return 0
			End If
			Return _Image.Width
		End Get
	End Property

	Private _Size As Size
	Private _Rectangle As Rectangle
	Private _Gradient As LinearGradientBrush

	Private _Brush As SolidBrush
	Protected Sub DrawCorners(c As Color, rect As Rectangle)
		_Brush = New SolidBrush(c)
		G.FillRectangle(_Brush, rect.X, rect.Y, 1, 1)
		G.FillRectangle(_Brush, rect.X + (rect.Width - 1), rect.Y, 1, 1)
		G.FillRectangle(_Brush, rect.X, rect.Y + (rect.Height - 1), 1, 1)
		G.FillRectangle(_Brush, rect.X + (rect.Width - 1), rect.Y + (rect.Height - 1), 1, 1)
	End Sub

	Protected Sub DrawBorders(p1 As Pen, p2 As Pen, rect As Rectangle)
		G.DrawRectangle(p1, rect.X, rect.Y, rect.Width - 1, rect.Height - 1)
		G.DrawRectangle(p2, rect.X + 1, rect.Y + 1, rect.Width - 3, rect.Height - 3)
	End Sub

	Protected Sub DrawText(a As HorizontalAlignment, c As Color, x As Integer)
		DrawText(a, c, x, 0)
	End Sub
	Protected Sub DrawText(a As HorizontalAlignment, c As Color, x As Integer, y As Integer)
		If String.IsNullOrEmpty(Text) Then
			Return
		End If
		_Size = G.MeasureString(Text, Font).ToSize()
		_Brush = New SolidBrush(c)

		Select Case a
			Case HorizontalAlignment.Left
				G.DrawString(Text, Font, _Brush, x, _MoveHeight \ 2 - _Size.Height \ 2 + y)
				Exit Select
			Case HorizontalAlignment.Right
				G.DrawString(Text, Font, _Brush, Width - _Size.Width - x, _MoveHeight \ 2 - _Size.Height \ 2 + y)
				Exit Select
			Case HorizontalAlignment.Center
				G.DrawString(Text, Font, _Brush, Width \ 2 - _Size.Width \ 2 + x, _MoveHeight \ 2 - _Size.Height \ 2 + y)
				Exit Select
		End Select
	End Sub

	Protected Sub DrawIcon(a As HorizontalAlignment, x As Integer)
		DrawIcon(a, x, 0)
	End Sub
	Protected Sub DrawIcon(a As HorizontalAlignment, x As Integer, y As Integer)
		If _Image Is Nothing Then
			Return
		End If
		Select Case a
			Case HorizontalAlignment.Left
				G.DrawImage(_Image, x, _MoveHeight \ 2 - _Image.Height \ 2 + y)
				Exit Select
			Case HorizontalAlignment.Right
				G.DrawImage(_Image, Width - _Image.Width - x, _MoveHeight \ 2 - _Image.Height \ 2 + y)
				Exit Select
			Case HorizontalAlignment.Center
				G.DrawImage(_Image, Width \ 2 - _Image.Width \ 2, _MoveHeight \ 2 - _Image.Height \ 2)
				Exit Select
		End Select
	End Sub

	Protected Sub DrawGradient(c1 As Color, c2 As Color, x As Integer, y As Integer, width As Integer, height As Integer, _
		angle As Single)
		_Rectangle = New Rectangle(x, y, width, height)
		_Gradient = New LinearGradientBrush(_Rectangle, c1, c2, angle)
		G.FillRectangle(_Gradient, _Rectangle)
	End Sub

	#End Region

End Class
NotInheritable Class Draw
	Private Sub New()
	End Sub
	Public Shared Function RoundRect(Rectangle As Rectangle, Curve As Integer) As GraphicsPath
		Dim P As New GraphicsPath()
		Dim ArcRectangleWidth As Integer = Curve * 2
		P.AddArc(New Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90)
		P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90)
		P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 0, 90)
		P.AddArc(New Rectangle(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 90, 90)
		P.AddLine(New Point(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y), New Point(Rectangle.X, Curve + Rectangle.Y))
		Return P
	End Function
	'Public Function RoundRect(ByVal X As Integer, ByVal Y As Integer, ByVal Width As Integer, ByVal Height As Integer, ByVal Curve As Integer) As GraphicsPath
	'    Dim Rectangle As Rectangle = New Rectangle(X, Y, Width, Height)
	'    Dim P As GraphicsPath = New GraphicsPath()
	'    Dim ArcRectangleWidth As Integer = Curve * 2
	'    P.AddArc(New Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90)
	'    P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90)
	'    P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 0, 90)
	'    P.AddArc(New Rectangle(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 90, 90)
	'    P.AddLine(New Point(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y), New Point(Rectangle.X, Curve + Rectangle.Y))
	'    Return P
	'End Function
End Class
MustInherit Class ThemeControl
	Inherits Control

	#Region " Initialization "

	Protected G As Graphics
	Protected B As Bitmap
	Public Sub New()
		SetStyle(CType(139270, ControlStyles), True)
		B = New Bitmap(1, 1)
		G = Graphics.FromImage(B)
	End Sub

	Public Sub AllowTransparent()
		SetStyle(ControlStyles.Opaque, False)
		SetStyle(ControlStyles.SupportsTransparentBackColor, True)
	End Sub

	Public Overrides Property Text() As String
		Get
			Return MyBase.Text
		End Get
		Set
			MyBase.Text = value
			Invalidate()
		End Set
	End Property
	#End Region

	#Region " Mouse Handling "

	Protected Enum State As Byte
		MouseNone = 0
		MouseOver = 1
		MouseDown = 2
	End Enum

	Protected MouseState As State
	Protected Overrides Sub OnMouseLeave(e As EventArgs)
		ChangeMouseState(State.MouseNone)
		MyBase.OnMouseLeave(e)
	End Sub
	Protected Overrides Sub OnMouseEnter(e As EventArgs)
		ChangeMouseState(State.MouseOver)
		MyBase.OnMouseEnter(e)
	End Sub
	Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
		ChangeMouseState(State.MouseOver)
		MyBase.OnMouseUp(e)
	End Sub
	Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
		If e.Button = MouseButtons.Left Then
			ChangeMouseState(State.MouseDown)
		End If
		MyBase.OnMouseDown(e)
	End Sub

	Private Sub ChangeMouseState(e As State)
		MouseState = e
		Invalidate()
	End Sub

	#End Region

	#Region " Convienence "

	Public MustOverride Sub PaintHook()
	Protected Overrides NotOverridable Sub OnPaint(e As PaintEventArgs)
		If Width = 0 OrElse Height = 0 Then
			Return
		End If
		PaintHook()
		e.Graphics.DrawImage(B, 0, 0)
	End Sub

	Protected Overrides Sub OnSizeChanged(e As EventArgs)
		If Not (Width = 0) AndAlso Not (Height = 0) Then
			B = New Bitmap(Width, Height)
			G = Graphics.FromImage(B)
			Invalidate()
		End If
		MyBase.OnSizeChanged(e)
	End Sub

	Private _NoRounding As Boolean
	Public Property NoRounding() As Boolean
		Get
			Return _NoRounding
		End Get
		Set
			_NoRounding = value
			Invalidate()
		End Set
	End Property

	Private _Image As Image
	Public Property Image() As Image
		Get
			Return _Image
		End Get
		Set
			_Image = value
			Invalidate()
		End Set
	End Property
	Public ReadOnly Property ImageWidth() As Integer
		Get
			If _Image Is Nothing Then
				Return 0
			End If
			Return _Image.Width
		End Get
	End Property
	Public ReadOnly Property ImageTop() As Integer
		Get
			If _Image Is Nothing Then
				Return 0
			End If
			Return Height \ 2 - _Image.Height \ 2
		End Get
	End Property

	Private _Size As Size
	Private _Rectangle As Rectangle
	Private _Gradient As LinearGradientBrush

	Private _Brush As SolidBrush
	Protected Sub DrawCorners(c As Color, rect As Rectangle)
		If _NoRounding Then
			Return
		End If

		B.SetPixel(rect.X, rect.Y, c)
		B.SetPixel(rect.X + (rect.Width - 1), rect.Y, c)
		B.SetPixel(rect.X, rect.Y + (rect.Height - 1), c)
		B.SetPixel(rect.X + (rect.Width - 1), rect.Y + (rect.Height - 1), c)
	End Sub

	Protected Sub DrawBorders(p1 As Pen, p2 As Pen, rect As Rectangle)
		G.DrawRectangle(p1, rect.X, rect.Y, rect.Width - 1, rect.Height - 1)
		G.DrawRectangle(p2, rect.X + 1, rect.Y + 1, rect.Width - 3, rect.Height - 3)
	End Sub

	Protected Sub DrawText(a As HorizontalAlignment, c As Color, x As Integer)
		DrawText(a, c, x, 0)
	End Sub
	Protected Sub DrawText(a As HorizontalAlignment, c As Color, x As Integer, y As Integer)
		If String.IsNullOrEmpty(Text) Then
			Return
		End If
		_Size = G.MeasureString(Text, Font).ToSize()
		_Brush = New SolidBrush(c)

		Select Case a
			Case HorizontalAlignment.Left
				G.DrawString(Text, Font, _Brush, x, Height \ 2 - _Size.Height \ 2 + y)
				Exit Select
			Case HorizontalAlignment.Right
				G.DrawString(Text, Font, _Brush, Width - _Size.Width - x, Height \ 2 - _Size.Height \ 2 + y)
				Exit Select
			Case HorizontalAlignment.Center
				G.DrawString(Text, Font, _Brush, Width \ 2 - _Size.Width \ 2 + x, Height \ 2 - _Size.Height \ 2 + y)
				Exit Select
		End Select
	End Sub

	Protected Sub DrawIcon(a As HorizontalAlignment, x As Integer)
		DrawIcon(a, x, 0)
	End Sub
	Protected Sub DrawIcon(a As HorizontalAlignment, x As Integer, y As Integer)
		If _Image Is Nothing Then
			Return
		End If
		Select Case a
			Case HorizontalAlignment.Left
				G.DrawImage(_Image, x, Height \ 2 - _Image.Height \ 2 + y)
				Exit Select
			Case HorizontalAlignment.Right
				G.DrawImage(_Image, Width - _Image.Width - x, Height \ 2 - _Image.Height \ 2 + y)
				Exit Select
			Case HorizontalAlignment.Center
				G.DrawImage(_Image, Width \ 2 - _Image.Width \ 2, Height \ 2 - _Image.Height \ 2)
				Exit Select
		End Select
	End Sub

	Protected Sub DrawGradient(c1 As Color, c2 As Color, x As Integer, y As Integer, width As Integer, height As Integer, _
		angle As Single)
		_Rectangle = New Rectangle(x, y, width, height)
		_Gradient = New LinearGradientBrush(_Rectangle, c1, c2, angle)
		G.FillRectangle(_Gradient, _Rectangle)
	End Sub
	#End Region

End Class
MustInherit Class ThemeContainerControl
	Inherits ContainerControl

	#Region " Initialization "

	Protected G As Graphics
	Protected B As Bitmap
	Public Sub New()
		SetStyle(CType(139270, ControlStyles), True)
		B = New Bitmap(1, 1)
		G = Graphics.FromImage(B)
	End Sub

	Public Sub AllowTransparent()
		SetStyle(ControlStyles.Opaque, False)
		SetStyle(ControlStyles.SupportsTransparentBackColor, True)
	End Sub

	#End Region
	#Region " Convienence "

	Public MustOverride Sub PaintHook()
	Protected Overrides NotOverridable Sub OnPaint(e As PaintEventArgs)
		If Width = 0 OrElse Height = 0 Then
			Return
		End If
		PaintHook()
		e.Graphics.DrawImage(B, 0, 0)
	End Sub

	Protected Overrides Sub OnSizeChanged(e As EventArgs)
		If Not (Width = 0) AndAlso Not (Height = 0) Then
			B = New Bitmap(Width, Height)
			G = Graphics.FromImage(B)
			Invalidate()
		End If
		MyBase.OnSizeChanged(e)
	End Sub

	Private _NoRounding As Boolean
	Public Property NoRounding() As Boolean
		Get
			Return _NoRounding
		End Get
		Set
			_NoRounding = value
			Invalidate()
		End Set
	End Property

	Private _Rectangle As Rectangle

	Private _Gradient As LinearGradientBrush
	Protected Sub DrawCorners(c As Color, rect As Rectangle)
		If _NoRounding Then
			Return
		End If
		B.SetPixel(rect.X, rect.Y, c)
		B.SetPixel(rect.X + (rect.Width - 1), rect.Y, c)
		B.SetPixel(rect.X, rect.Y + (rect.Height - 1), c)
		B.SetPixel(rect.X + (rect.Width - 1), rect.Y + (rect.Height - 1), c)
	End Sub

	Protected Sub DrawBorders(p1 As Pen, p2 As Pen, rect As Rectangle)
		G.DrawRectangle(p1, rect.X, rect.Y, rect.Width - 1, rect.Height - 1)
		G.DrawRectangle(p2, rect.X + 1, rect.Y + 1, rect.Width - 3, rect.Height - 3)
	End Sub

	Protected Sub DrawGradient(c1 As Color, c2 As Color, x As Integer, y As Integer, width As Integer, height As Integer, _
		angle As Single)
		_Rectangle = New Rectangle(x, y, width, height)
		_Gradient = New LinearGradientBrush(_Rectangle, c1, c2, angle)
		G.FillRectangle(_Gradient, _Rectangle)
	End Sub
	#End Region

End Class

Class TxtBox
	Inherits ThemeControl
	#Region "lol"
	Private txtbox As New TextBox()
	Private _passmask As Boolean = False
	Public Property UseSystemPasswordChar() As Boolean
		Get
			Return _passmask
		End Get
		Set
			txtbox.UseSystemPasswordChar = UseSystemPasswordChar
			_passmask = value
			Invalidate()
		End Set
	End Property
	Private _maxchars As Integer = 32767
	Public Property MaxLength() As Integer
		Get
			Return _maxchars
		End Get
		Set
			_maxchars = value
			txtbox.MaxLength = MaxLength
			Invalidate()
		End Set
	End Property
	Private _align As HorizontalAlignment
	Public Property TextAlignment() As HorizontalAlignment
		Get
			Return _align
		End Get
		Set
			_align = value
			Invalidate()
		End Set
	End Property

	Protected Overrides Sub OnPaintBackground(pevent As System.Windows.Forms.PaintEventArgs)
	End Sub
	Protected Overrides Sub OnTextChanged(e As System.EventArgs)
		MyBase.OnTextChanged(e)
		Invalidate()
	End Sub
	Protected Overrides Sub OnBackColorChanged(e As System.EventArgs)
		MyBase.OnBackColorChanged(e)
		txtbox.BackColor = BackColor
		Invalidate()
	End Sub
	Protected Overrides Sub OnForeColorChanged(e As System.EventArgs)
		MyBase.OnForeColorChanged(e)
		txtbox.ForeColor = ForeColor
		Invalidate()
	End Sub
	Protected Overrides Sub OnFontChanged(e As System.EventArgs)
		MyBase.OnFontChanged(e)
		txtbox.Font = Font
	End Sub
	Protected Overrides Sub OnGotFocus(e As System.EventArgs)
		MyBase.OnGotFocus(e)
		txtbox.Focus()
	End Sub
	Public Sub TextChngTxtBox()
	' ERROR: Handles clauses are not supported in C#
		Text = txtbox.Text
	End Sub
	Public Sub TextChng()
	' ERROR: Handles clauses are not supported in C#
		txtbox.Text = Text
	End Sub

	#End Region

	Protected Overrides Sub WndProc(ByRef m As Message)
		Select Case m.Msg
			Case 15
				Invalidate()
				MyBase.WndProc(m)
				Me.PaintHook()
				Exit Select
			Case Else
				' TODO: might not be correct. Was : Exit Select
				MyBase.WndProc(m)
				Exit Select
			' TODO: might not be correct. Was : Exit Select
		End Select
	End Sub

	Public Sub New()
		MyBase.New()

		Controls.Add(txtbox)
		If True Then
			txtbox.Multiline = False
			txtbox.BackColor = Color.FromArgb(0, 0, 0)
			txtbox.ForeColor = ForeColor
			txtbox.Text = String.Empty
			txtbox.TextAlign = HorizontalAlignment.Center
			txtbox.BorderStyle = BorderStyle.None
			txtbox.Location = New Point(5, 8)
			txtbox.Font = New Font("Arial", 8.25F, FontStyle.Bold)
			txtbox.Size = New Size(Width - 8, Height - 11)
			txtbox.UseSystemPasswordChar = UseSystemPasswordChar
		End If

		Text = ""

		DoubleBuffered = True
	End Sub

	Public Overrides Sub PaintHook()
		Me.BackColor = Color.White
		G.Clear(Parent.BackColor)
		Dim p As New Pen(Color.FromArgb(204, 204, 204), 1)
		Dim o As New Pen(Color.FromArgb(249, 249, 249), 8)
		G.FillPath(Brushes.White, Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 2))
		G.DrawPath(o, Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 2))
		G.DrawPath(p, Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 2))
		Height = txtbox.Height + 16
		Dim drawFont As New Font("Tahoma", 9, FontStyle.Regular)
		If True Then
			txtbox.Width = Width - 12
			txtbox.ForeColor = Color.FromArgb(72, 72, 72)
			txtbox.Font = drawFont
			txtbox.TextAlign = TextAlignment
			txtbox.UseSystemPasswordChar = UseSystemPasswordChar
		End If
		DrawCorners(Parent.BackColor, ClientRectangle)
	End Sub
End Class

Class PanelBox
	Inherits ThemeContainerControl
	Public Sub New()
		AllowTransparent()
	End Sub
	Public Overrides Sub PaintHook()
		Me.Font = New Font("Tahoma", 10)
		Me.ForeColor = Color.FromArgb(40, 40, 40)
		G.SmoothingMode = SmoothingMode.AntiAlias
		G.FillRectangle(New SolidBrush(Color.FromArgb(235, 235, 235)), New Rectangle(2, 0, Width, Height))
		G.FillRectangle(New SolidBrush(Color.FromArgb(249, 249, 249)), New Rectangle(1, 0, Width - 3, Height - 4))
		G.DrawRectangle(New Pen(Color.FromArgb(214, 214, 214)), 0, 0, Width - 2, Height - 3)
	End Sub
End Class
Class GroupDropBox
	Inherits ThemeContainerControl
	Private _Checked As Boolean
	Private X As Integer
	Private y As Integer
	Private _OpenedSize As Size
	Public Property Checked() As Boolean
		Get
			Return _Checked
		End Get
		Set
			_Checked = value
			Invalidate()
		End Set
	End Property
	Public Property OpenSize() As Size
		Get
			Return _OpenedSize
		End Get
		Set
			_OpenedSize = value
			Invalidate()
		End Set
	End Property
	Public Sub New()

		AllowTransparent()
		Size = New Size(90, 30)
		MinimumSize = New Size(5, 30)
		_Checked = True
		AddHandler Me.Resize, New EventHandler(AddressOf GroupDropBox_Resize)
		AddHandler Me.MouseDown, New MouseEventHandler(AddressOf GroupDropBox_MouseDown)
	End Sub
	Public Overrides Sub PaintHook()
		Me.Font = New Font("Tahoma", 10)
		Me.ForeColor = Color.FromArgb(40, 40, 40)
		If _Checked = True Then
			G.SmoothingMode = SmoothingMode.AntiAlias
			G.Clear(Color.FromArgb(245, 245, 245))
			G.FillRectangle(New SolidBrush(Color.FromArgb(231, 231, 231)), New Rectangle(0, 0, Width, 30))
			G.DrawLine(New Pen(Color.FromArgb(233, 238, 240)), 1, 1, Width - 2, 1)
			G.DrawRectangle(New Pen(Color.FromArgb(214, 214, 214)), 0, 0, Width - 1, Height - 1)
			G.DrawRectangle(New Pen(Color.FromArgb(214, 214, 214)), 0, 0, Width - 1, 30)
			Me.Size = _OpenedSize
			G.DrawString("t", New Font("Marlett", 12), New SolidBrush(Me.ForeColor), Width - 25, 5)
		Else
			G.SmoothingMode = SmoothingMode.AntiAlias
			G.Clear(Color.FromArgb(245, 245, 245))
			G.FillRectangle(New SolidBrush(Color.FromArgb(231, 231, 231)), New Rectangle(0, 0, Width, 30))
			G.DrawLine(New Pen(Color.FromArgb(231, 236, 238)), 1, 1, Width - 2, 1)
			G.DrawRectangle(New Pen(Color.FromArgb(214, 214, 214)), 0, 0, Width - 1, Height - 1)
			G.DrawRectangle(New Pen(Color.FromArgb(214, 214, 214)), 0, 0, Width - 1, 30)
			Me.Size = New Size(Width, 30)
			G.DrawString("u", New Font("Marlett", 12), New SolidBrush(Me.ForeColor), Width - 25, 5)
		End If
		G.DrawString(Text, Font, New SolidBrush(Me.ForeColor), 7, 6)
	End Sub

	Private Sub GroupDropBox_Resize(sender As Object, e As System.EventArgs)
		If _Checked = True Then
			_OpenedSize = Me.Size
		End If
	End Sub


	Protected Overrides Sub OnMouseMove(e As System.Windows.Forms.MouseEventArgs)
		MyBase.OnMouseMove(e)
		X = e.X
		y = e.Y
		Invalidate()
	End Sub


	Private Sub GroupDropBox_MouseDown(sender As Object, e As MouseEventArgs)

		If X >= Width - 22 Then
			If y <= 30 Then
				Select Case Checked
					Case True
						Checked = False
						Exit Select
					Case False
						Checked = True
						Exit Select
				End Select
			End If
		End If
	End Sub
End Class
Class GroupPanelBox
	Inherits ThemeContainerControl
	Public Sub New()
		AllowTransparent()
	End Sub
	Public Overrides Sub PaintHook()
		Me.Font = New Font("Tahoma", 10)
		Me.ForeColor = Color.FromArgb(40, 40, 40)
		G.SmoothingMode = SmoothingMode.AntiAlias
		G.Clear(Color.FromArgb(245, 245, 245))
		G.FillRectangle(New SolidBrush(Color.FromArgb(231, 231, 231)), New Rectangle(0, 0, Width, 30))
		G.DrawLine(New Pen(Color.FromArgb(233, 238, 240)), 1, 1, Width - 2, 1)
		G.DrawRectangle(New Pen(Color.FromArgb(214, 214, 214)), 0, 0, Width - 1, Height - 1)
		G.DrawRectangle(New Pen(Color.FromArgb(214, 214, 214)), 0, 0, Width - 1, 30)
		G.DrawString(Text, Font, New SolidBrush(Me.ForeColor), 7, 6)
	End Sub
End Class

Class ButtonGreen
	Inherits ThemeControl
	Public Overrides Sub PaintHook()
		Me.Font = New Font("Arial", 10)
		G.Clear(Me.BackColor)
		G.SmoothingMode = SmoothingMode.HighQuality
		Select Case MouseState
			Case State.MouseNone
				Dim p1 As New Pen(Color.FromArgb(120, 159, 22), 1)
				Dim x1 As New LinearGradientBrush(ClientRectangle, Color.FromArgb(157, 209, 57), Color.FromArgb(130, 181, 18), LinearGradientMode.Vertical)
				G.FillPath(x1, Draw.RoundRect(ClientRectangle, 4))
				G.DrawPath(p1, Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 3))
				G.DrawLine(New Pen(Color.FromArgb(190, 232, 109)), 2, 1, Width - 3, 1)
				DrawText(HorizontalAlignment.Center, Color.FromArgb(240, 240, 240), 0)
				Exit Select
			Case State.MouseDown
				Dim p2 As New Pen(Color.FromArgb(120, 159, 22), 1)
				Dim x2 As New LinearGradientBrush(ClientRectangle, Color.FromArgb(125, 171, 25), Color.FromArgb(142, 192, 40), LinearGradientMode.Vertical)
				G.FillPath(x2, Draw.RoundRect(ClientRectangle, 4))
				G.DrawPath(p2, Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 3))
				G.DrawLine(New Pen(Color.FromArgb(142, 172, 30)), 2, 1, Width - 3, 1)
				DrawText(HorizontalAlignment.Center, Color.FromArgb(250, 250, 250), 1)
				Exit Select
			Case State.MouseOver
				Dim p3 As New Pen(Color.FromArgb(120, 159, 22), 1)
				Dim x3 As New LinearGradientBrush(ClientRectangle, Color.FromArgb(165, 220, 59), Color.FromArgb(137, 191, 18), LinearGradientMode.Vertical)
				G.FillPath(x3, Draw.RoundRect(ClientRectangle, 4))
				G.DrawPath(p3, Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 3))
				G.DrawLine(New Pen(Color.FromArgb(190, 232, 109)), 2, 1, Width - 3, 1)
				DrawText(HorizontalAlignment.Center, Color.FromArgb(240, 240, 240), -1)
				Exit Select
		End Select
		Me.Cursor = Cursors.Hand
	End Sub
End Class
Class ButtonBlue
	Inherits ThemeControl
	Public Overrides Sub PaintHook()
		Me.Font = New Font("Arial", 10)
		G.Clear(Me.BackColor)
		G.SmoothingMode = SmoothingMode.HighQuality
		Select Case MouseState
			Case State.MouseNone
				Dim p As New Pen(Color.FromArgb(34, 112, 171), 1)
				Dim x As New LinearGradientBrush(ClientRectangle, Color.FromArgb(51, 159, 231), Color.FromArgb(33, 128, 206), LinearGradientMode.Vertical)
				G.FillPath(x, Draw.RoundRect(ClientRectangle, 4))
				G.DrawPath(p, Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 3))
				G.DrawLine(New Pen(Color.FromArgb(131, 197, 241)), 2, 1, Width - 3, 1)
				DrawText(HorizontalAlignment.Center, Color.FromArgb(240, 240, 240), 0)
				Exit Select
			Case State.MouseDown
				Dim p1 As New Pen(Color.FromArgb(34, 112, 171), 1)
				Dim x1 As New LinearGradientBrush(ClientRectangle, Color.FromArgb(37, 124, 196), Color.FromArgb(53, 153, 219), LinearGradientMode.Vertical)
				G.FillPath(x1, Draw.RoundRect(ClientRectangle, 4))
				G.DrawPath(p1, Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 3))

				DrawText(HorizontalAlignment.Center, Color.FromArgb(250, 250, 250), 1)
				Exit Select
			Case State.MouseOver
				Dim p2 As New Pen(Color.FromArgb(34, 112, 171), 1)
				Dim x2 As New LinearGradientBrush(ClientRectangle, Color.FromArgb(54, 167, 243), Color.FromArgb(35, 165, 217), LinearGradientMode.Vertical)
				G.FillPath(x2, Draw.RoundRect(ClientRectangle, 4))
				G.DrawPath(p2, Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 3))
				G.DrawLine(New Pen(Color.FromArgb(131, 197, 241)), 2, 1, Width - 3, 1)
				DrawText(HorizontalAlignment.Center, Color.FromArgb(240, 240, 240), -1)
				Exit Select
		End Select
		Me.Cursor = Cursors.Hand
	End Sub
End Class