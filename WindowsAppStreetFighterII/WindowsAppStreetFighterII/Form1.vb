Public Class Form1
    Dim Time As Integer = 99
    Dim Life As Integer = 100
    Dim Life2 As Integer = 100
    Dim frame As Integer = 0
    Dim cx As Integer = 0
    Dim cy As Integer = 0
    Dim jump As Integer = 0
    Dim jump_vx As Integer = 0
    Dim jump_vy As Double = 0
    Dim gravity As Double = 0.5
    Dim jump_time = 0
    Dim state As Integer = 0
    Dim img_0 As Image = Image.FromFile("..\..\アニメ素材\ガイル絵本風立ち.png")
    Dim img_1 As Image = Image.FromFile("..\..\アニメ素材\ガイル絵本風パンチ.png")
    Dim img_2 As Image = Image.FromFile("..\..\アニメ素材\ガイル絵本風ムエタイキック.png")
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        cx = cx - 5
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        cx = cx + 5
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        frame = frame + 1
        If frame Mod 100 = 0 Then
            Time = Time - 1
        End If
        PictureBox1.BackColor = Color.Black
        Dim canvas As New Bitmap(PictureBox1.Width, PictureBox1.Height)
        Dim g As Graphics = Graphics.FromImage(canvas)
        Dim img As Image
        If (state = 1) Then
            img = img_1
        ElseIf (state = 2) Then
            img = img_2
        Else
            img = img_0
        End If
        If jump = 1 Then
            jump_time = jump_time + 1
            Dim j As Integer = -20
            Dim t As Double = jump_time / 4
            jump_vy = 15 - gravity * t * t
            cx = cx + jump_vx
            cy = cy + jump_vy
            If cy < 0 Then
                jump = 0
                jump_time = 0
                jump_vx = 0
                jump_vy = 0
                cy = 0
            End If
        End If
        DrawTime(g)
        g.DrawImage(img, 20 + cx, 220 - cy, 200, 200)
        g.Dispose()
        PictureBox1.Image = canvas
    End Sub
    Private Sub DrawTime(ByRef g As Graphics)
        Dim f As Font = New Font("MSゴシック", 20)
        Dim b As Brush = Brushes.White
        Dim p As Point = New Point(PictureBox1.Width / 2 - 30, 10)
        g.DrawString(Time.ToString, f, b, p)
        Dim r1 As Rectangle = New Rectangle(10, 10, 200, 30)
        g.FillRectangle(Brushes.Red, r1)
        Dim r2 As Rectangle = New Rectangle(210 - Life * 2, 10, Life * 2, 30)
        g.FillRectangle(Brushes.Yellow, r2)
        Dim a As Integer = PictureBox1.Width
        Dim r3 As Rectangle = New Rectangle(a - 210, 10, 200, 30)
        g.FillRectangle(Brushes.Red, r3)
        Dim r4 As Rectangle = New Rectangle(a - 210, 10, Life2 * 2, 30)
        g.FillRectangle(Brushes.Yellow, r4)
    End Sub
    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        If jump = 0 Then
            jump = 1
            jump_time = 0
            jump_vy = 0
        End If
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        If jump = 0 Then
            jump = 1
            jump_time = 0
            jump_vy = 0
            jump_vx = -5
        End If
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        If jump = 0 Then
            jump = 1
            jump_time = 0
            jump_vy = 0
            jump_vx = 5
        End If
    End Sub
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        state = 0
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        state = 1
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        state = 1
    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        state = 1
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        state = 2
    End Sub
    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        state = 2
    End Sub
    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        state = 2
    End Sub
    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        Dim t As String = TextBox1.Text.ToString
        If t = "" Then
            Life = 0
        Else
            Life = t
        End If
        If Life < 0 Then
            Life = 0
        End If
        If Life > 100 Then
            Life = 100
        End If
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        Dim t As String = TextBox2.Text.ToString
        If t = "" Then
            Life2 = 0
        Else
            Life2 = t
        End If
        If Life2 < 0 Then
            Life2 = 0
        End If
        If Life2 > 100 Then
            Life2 = 100
        End If

    End Sub
End Class
