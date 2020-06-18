Imports System.Data.SqlTypes

Public Class Form1
    Private Class clMove
        Public Life As Integer = 100
        Public Life2 As Integer = 100
        Public cx As Integer = 0
        Public cy As Integer = 0
        Public walk_vx = 0
        Public jump As Integer = 0
        Public jump_vx As Integer = 0
        Public jump_vy As Double = 0
        Public jump_time = 0
        Public gravity As Double = 0.5
        Public sonic_x As Integer = 1000
        Public sonic_r As Integer = 0
        Public state As Integer = 0
        Public next_state As Integer = 0
        Public state_time As Integer = 0
        Public tech_flag As Integer = 0
        Public tech_time As Integer = 0
    End Class
    Dim Time As Integer = 99
    Dim frame As Integer = 0
    Dim c1 As clMove = New clMove()
    Dim c2 As clMove = New clMove()
    Dim ryu_state = 1
    Dim hadou_x = 1000
    Dim img_0 As Image = Image.FromFile("..\..\アニメ素材\ガイル絵本風立ち.png")
    Dim img_1 As Image = Image.FromFile("..\..\アニメ素材\ガイル絵本風パンチ.png")
    Dim img_2 As Image = Image.FromFile("..\..\アニメ素材\ガイル絵本風ムエタイキック.png")
    Dim img_3 As Image = Image.FromFile("..\..\アニメ素材\ガイル絵本風構え.png")
    Dim img_4 As Image = Image.FromFile("..\..\アニメ素材\ガイル絵本風ソニックブーム.png")
    Dim img_sonic As Image = Image.FromFile("..\..\アニメ素材\ソニックブーム.png")
    Dim img_sonic2 As Image = Image.FromFile("..\..\アニメ素材\ソニックブーム２.png")
    Dim img_r1 As Image = Image.FromFile("..\..\アニメ素材\リュウ立ち.png")
    Dim img_r2 As Image = Image.FromFile("..\..\アニメ素材\リュウ構え.png")
    Dim img_r3 As Image = Image.FromFile("..\..\アニメ素材\リュウ波動拳.png")
    Dim img_r4 As Image = Image.FromFile("..\..\アニメ素材\リュウジャンプ.png")
    Dim img_hadou As Image = Image.FromFile("..\..\アニメ素材\波動拳.png")
    Dim img_back As Image = Image.FromFile("..\..\アニメ素材\背景.png")
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        frame = frame + 1
        If frame Mod 100 = 0 Then
            Time = Time - 1
        End If
        PictureBox1.BackColor = Color.Black
        Dim canvas As New Bitmap(PictureBox1.Width, PictureBox1.Height)
        Dim g As Graphics = Graphics.FromImage(canvas)
        g.DrawImage(img_back, 0, 0, canvas.Width, canvas.Height)
        ProgressFrame(c1)
        Dim img As Image
        If (c1.state = 1) Then
            img = img_1
            SetNextFrame(c1, 0, 9)
        ElseIf (c1.state = 2) Then
            img = img_2
            SetNextFrame(c1, 0, 14)
        ElseIf (c1.state = 3) Then
            img = img_3
            SetNextFrame(c1, 4, 10)
        ElseIf (c1.state = 4) Then
            img = img_4
            If c1.tech_flag = 0 Then
                c1.sonic_x = 0
            End If
            SetNextFrame(c1, 0, 36)
        Else
            img = img_0
            c1.tech_flag = 0
        End If
        DrawTime(g)
        JumpCalc(c1)
        JumpCalc(c2)
        g.DrawImage(img, 20 + c1.cx, 220 - c1.cy, 200, 200)
        If c1.sonic_x <= canvas.Width Then
            If c1.sonic_x Mod 50 = 0 Then
                c1.sonic_r = c1.sonic_r + 1
            End If
            If c1.sonic_r Mod 2 = 0 Then
                g.DrawImage(img_sonic, 20 + c1.cx + c1.sonic_x, 220, 200, 200)
            Else
                g.DrawImage(img_sonic2, 20 + c1.cx + c1.sonic_x, 220, 200, 200)
            End If
            c1.sonic_x = c1.sonic_x + 5
        End If
        If (ryu_state = 1) Then
            img = img_r1
        ElseIf ryu_state = 2 Then
            img = img_r2
        ElseIf ryu_state = 3 Then
            img = img_r3
        End If
        If c2.cy > 0 Then
            Dim center_x As Integer = 400 + c2.cx + 100
            Dim center_y As Integer = 220 - c2.cy + 100
            Dim r As Double = 100 * Math.Sqrt(2)
            If c2.jump_vx < 0 Then
                Dim angle1 As Double = (135 + c2.jump_time / 19 * 360) * 3.14 / 180
                Dim angle2 As Double = (45 + c2.jump_time / 19 * 360) * 3.14 / 180
                Dim angle3 As Double = (225 + c2.jump_time / 19 * 360) * 3.14 / 180
                Dim dp() As Point = New Point() {New Point(center_x + r * Math.Sin(angle1), center_y + r * Math.Cos(angle1)),
                New Point(center_x + r * Math.Sin(angle2), center_y + r * Math.Cos(angle2)),
                New Point(center_x + r * Math.Sin(angle3), center_y + r * Math.Cos(angle3))}
                g.DrawImage(img_r4, dp)
            ElseIf c2.jump_vx > 0 Then
                Dim angle1 As Double = (135 - c2.jump_time / 19 * 360) * 3.14 / 180
                Dim angle2 As Double = (45 - c2.jump_time / 19 * 360) * 3.14 / 180
                Dim angle3 As Double = (225 - c2.jump_time / 19 * 360) * 3.14 / 180
                Dim dp() As Point = New Point() {New Point(center_x + r * Math.Sin(angle1), center_y + r * Math.Cos(angle1)),
                New Point(center_x + r * Math.Sin(angle2), center_y + r * Math.Cos(angle2)),
                New Point(center_x + r * Math.Sin(angle3), center_y + r * Math.Cos(angle3))}
                g.DrawImage(img_r4, dp)
            Else
                g.DrawImage(img_r4, 400 + c2.cx, 220 - c2.cy, 200, 200)
            End If
        Else
            g.DrawImage(img, 400 + c2.cx, 220 - c2.cy, 200, 200)
        End If

        If hadou_x <= canvas.Width Then
            g.DrawImage(img_hadou, 360 + c2.cx + hadou_x, 220, 200, 200)
            hadou_x = hadou_x - 5
        End If
        g.Dispose()
        PictureBox1.Image = canvas
    End Sub
    Private Function RotateMove(ByRef bmp As Bitmap, ByVal angle As Double, ByVal x As Integer, ByVal y As Integer) As Bitmap
        Dim bmp2 As Bitmap = New Bitmap(bmp.Width, bmp.Height)
        Dim g As Graphics = Graphics.FromImage(bmp2)
        g.Clear(Color.Blue)
        g.TranslateTransform(-x, -y)
        g.RotateTransform(angle, System.Drawing.Drawing2D.MatrixOrder.Append)
        g.TranslateTransform(x, y, System.Drawing.Drawing2D.MatrixOrder.Append)
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear
        g.DrawImageUnscaled(bmp, 0, 0)
        g.Dispose()
        bmp2.MakeTransparent(Color.Blue)
        Return bmp2
    End Function
    Private Sub JumpCalc(ByRef c As clMove)
        If c.jump = 1 Then
            c.jump_time = c.jump_time + 1
            Dim j As Integer = -20
            Dim t As Double = c.jump_time / 4
            c.jump_vy = 15 - c.gravity * t * t
            c.cx = c.cx + c.jump_vx
            c.cy = c.cy + c.jump_vy
            If c.cy < 0 Then
                c.jump = 0
                c.jump_time = 0
                c.jump_vx = 0
                c.jump_vy = 0
                c.cy = 0
            End If
        Else
            c.cx = c.cx + c.walk_vx
        End If
    End Sub
    Private Sub DrawTime(ByRef g As Graphics)
        Dim f As Font = New Font("MSゴシック", 20)
        Dim b As Brush = Brushes.White
        Dim p As Point = New Point(PictureBox1.Width / 2 - 30, 10)
        g.DrawString(Time.ToString, f, b, p)
        Dim r1 As Rectangle = New Rectangle(10, 10, 200, 30)
        g.FillRectangle(Brushes.Red, r1)
        Dim r2 As Rectangle = New Rectangle(210 - c1.Life * 2, 10, c1.Life * 2, 30)
        g.FillRectangle(Brushes.Yellow, r2)
        Dim a As Integer = PictureBox1.Width
        Dim r3 As Rectangle = New Rectangle(a - 210, 10, 200, 30)
        g.FillRectangle(Brushes.Red, r3)
        Dim r4 As Rectangle = New Rectangle(a - 210, 10, c2.Life * 2, 30)
        g.FillRectangle(Brushes.Yellow, r4)
    End Sub
    Private Sub SetNextFrame(ByRef c As clMove, ByVal ns As Integer, ByVal st As Integer)
        If c.tech_flag = 0 Then
            c.next_state = ns
            c.state_time = st
            c.tech_flag = 1
        End If
    End Sub
    Private Sub ProgressFrame(ByRef c As clMove)
        If c.tech_flag = 0 Then
            c.tech_time = 0
        Else
            c.tech_time = c.tech_time + 1
            If c.state_time = c.tech_time Then
                c.state = c.next_state
                c.tech_flag = 0
            End If
        End If
    End Sub
    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        If c1.jump = 0 Then
            c1.jump = 1
            c1.jump_time = 0
            c1.jump_vy = 0
        End If
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        If c1.jump = 0 Then
            c1.jump = 1
            c1.jump_time = 0
            c1.jump_vy = 0
            c1.jump_vx = -5
        End If
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        If c1.jump = 0 Then
            c1.jump = 1
            c1.jump_time = 0
            c1.jump_vy = 0
            c1.jump_vx = 5
        End If
    End Sub
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        c1.state = 0
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        c1.state = 1
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        c1.state = 1
    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        c1.state = 3
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        c1.state = 2
    End Sub
    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        c1.state = 2
    End Sub
    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        c1.state = 2
    End Sub
    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        Dim t As String = TextBox1.Text.ToString
        If t = "" Then
            c1.Life = 0
        Else
            c1.Life = t
        End If
        If c1.Life < 0 Then
            c1.Life = 0
        End If
        If c1.Life > 100 Then
            c1.Life = 100
        End If
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        Dim t As String = TextBox2.Text.ToString
        If t = "" Then
            c2.Life = 0
        Else
            c2.Life = t
        End If
        If c2.Life2 < 0 Then
            c2.Life2 = 0
        End If
        If c2.Life2 > 100 Then
            c2.Life2 = 100
        End If
    End Sub

    Private Sub Button6_MouseDown(sender As Object, e As MouseEventArgs) Handles Button6.MouseDown
        c1.walk_vx = 5
    End Sub
    Private Sub Button4_MouseDown(sender As Object, e As MouseEventArgs) Handles Button4.MouseDown
        c1.walk_vx = -5
    End Sub
    Private Sub Button6_MouseUp(sender As Object, e As MouseEventArgs) Handles Button6.MouseUp
        c1.walk_vx = 0
    End Sub
    Private Sub Button4_MouseUp(sender As Object, e As MouseEventArgs) Handles Button4.MouseUp
        c1.walk_vx = 0
    End Sub

    Private Sub Button21_MouseDown(sender As Object, e As EventArgs) Handles Button21.MouseDown
        c2.walk_vx = 5
    End Sub

    Private Sub Button19_MouseDown(sender As Object, e As EventArgs) Handles Button19.MouseDown
        c2.walk_vx = -5
    End Sub
    Private Sub Button21_MouseUp(sender As Object, e As MouseEventArgs) Handles Button21.MouseUp
        c2.walk_vx = 0
    End Sub
    Private Sub Button19_MouseUp(sender As Object, e As MouseEventArgs) Handles Button19.MouseUp
        c2.walk_vx = 0
    End Sub

    Private Sub Button27_Click(sender As Object, e As EventArgs) Handles Button27.Click
        ryu_state = ryu_state + 1
        If ryu_state = 3 Then
            hadou_x = 0
        End If
        If ryu_state = 4 Then
            ryu_state = 1
        End If
    End Sub

    Private Sub Button22_Click(sender As Object, e As EventArgs) Handles Button22.Click
        If c2.jump = 0 Then
            c2.jump = 1
            c2.jump_time = 0
            c2.jump_vy = 0
            c2.jump_vx = -5
        End If
    End Sub

    Private Sub Button23_Click(sender As Object, e As EventArgs) Handles Button23.Click
        If c2.jump = 0 Then
            c2.jump = 1
            c2.jump_time = 0
            c2.jump_vy = 0
        End If

    End Sub

    Private Sub Button24_Click(sender As Object, e As EventArgs) Handles Button24.Click
        If c2.jump = 0 Then
            c2.jump = 1
            c2.jump_time = 0
            c2.jump_vy = 0
            c2.jump_vx = 5
        End If
    End Sub
End Class
