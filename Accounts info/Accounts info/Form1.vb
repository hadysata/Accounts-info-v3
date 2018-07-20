Imports System.Net
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Text

Public Class Form1

    Private isMouseDown As Boolean = False
    Private mouseOffset As Point
    Dim list() As String = IO.File.ReadAllLines("list.txt")
    Dim max_followers As Integer
    Dim max_following As Integer
    Dim max_posts As Integer
    Dim lst As New ListBox
    Dim done As Integer = 0

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        End
    End Sub

    Private Sub Panel1_MouseDown(sender As Object, e As MouseEventArgs) Handles Panel1.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Left Then
            mouseOffset = New Point(-e.X, -e.Y)
            isMouseDown = True
        End If
    End Sub

    Private Sub Panel1_MouseMove(sender As Object, e As MouseEventArgs) Handles Panel1.MouseMove
        If isMouseDown Then
            Dim mousePos As Point = Control.MousePosition
            mousePos.Offset(mouseOffset.X, mouseOffset.Y)
            Me.Location = mousePos
        End If
    End Sub

    Private Sub Panel1_MouseUp(sender As Object, e As MouseEventArgs) Handles Panel1.MouseUp
        If e.Button = Windows.Forms.MouseButtons.Left Then
            isMouseDown = False
        End If
    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click
        Timer1.Start()

    End Sub

    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click
        Me.WindowState = FormWindowState.Minimized

    End Sub
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Dim RSV As Integer
        RSV = "-" & Me.Size.Height
        If Me.Top > 1080 Then
            Timer1.Enabled = False
            End
        Else
            Me.Top += 50
        End If
    End Sub

    Public Sub download(ByVal username_number As Integer)
        Try
            Dim user, pass
            Try
                user = Split(list(username_number), ":") : pass = user(1) : user = user(0)
            Catch ex As Exception
                user = list(username_number)
            End Try

            If CheckBox2.Checked = True Then
                Dim lv2 As New ListViewItem(user.ToString)
                lv2.SubItems.Add("0")
                lv2.SubItems.Add(pass)
                ListView1.Invoke(New MethodInvoker(Sub() ListView1.Items.Add(lv2)))
            Else
                Dim w As New WebClient
                ServicePointManager.DefaultConnectionLimit = 500
                ServicePointManager.UseNagleAlgorithm = False
                ServicePointManager.Expect100Continue = False
                ServicePointManager.CheckCertificateRevocationList = False
                w.Headers.Add("Upgrade-Insecure-Requests: " & rann())
                w.Headers.Add("User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.115 Safari/537.36")
                w.Headers.Add("Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8")
                w.Headers.Add("Cookie: mid=" & rans() & "; csrftoken=" & rans() & "; ds_user_id=" & rann() & "; sessionid=IGSC7ff91bb642e07146e26ab66e17e08743a4d88876f04f2d231df0966ece4da166%3Ap9Ju1wNB1GjYb9kH768s0d5AL4raNp7K%3A%7B%22_auth_user_id%22%3A8154662816%2C%22_auth_user_backend%22%3A%22accounts.backends.CaseInsensitiveModelBackend%22%2C%22_auth_user_hash%22%3A%22%22%2C%22_platform%22%3A4%2C%22_token_ver%22%3A2%2C%22_token%22%3A%228154662816%3AxNaOCMISLJQZ67xCUxtbHOFaSCCYV3Ox%3A78af1e22128309252c195a802a2943c4a9cea7823d1f56f18b2be2f9c6f66408%22%2C%22last_refreshed%22%3A1530762152.5963754654%7D")

                '  w.Proxy = Nothing
                Dim h As String = w.DownloadString("https://www.instagram.com/" & user & "/?__a=1")
                Dim f = Split(h, "followed_by"":{""count"":") : f = Split(f(1), "}")
                Dim m = Split(h, "follow"":{""count"":") : m = Split(m(1), "}")

                Dim p = Split(h, "media"":{""count"":") : p = Split(p(1), ",""")


                If f(0) >= max_followers Then

                    If m(0) >= max_following Then

                        If p(0) >= max_posts Then


                            Dim lvi As New ListViewItem(user.ToString)
                            lvi.SubItems.Add(f(0))
                            lvi.SubItems.Add(m(0))
                            lvi.SubItems.Add(p(0))

                            If CheckBox2.Checked = True Then
                                lvi.SubItems.Add(pass)
                            Else
                                lvi.SubItems.Add("******")
                            End If
                            ListView1.Invoke(New MethodInvoker(Sub() ListView1.Items.Add(lvi)))

                        End If

                    End If


                End If






            End If


            GC.Collect()
            done += 1
        Catch ex As Exception
            MsgBox(ex.ToString & vbCrLf & username_number)
        End Try
    End Sub

    Public Function rann() As Integer
        Dim Generator As System.Random = New System.Random()
        Return Generator.Next(1000, 6000)
    End Function

    Function rans()
        Dim s As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"
        Static r As New Random
        Dim chactersInString As Integer = r.Next(5, 11)
        Dim sb As New StringBuilder
        For i As Integer = 1 To chactersInString
            Dim idx As Integer = r.Next(0, s.Length)
            sb.Append(s.Substring(idx, 1))
        Next
        Return sb.ToString()
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim th As New Thread(AddressOf work) : th.Start()
    End Sub

    Private Sub work()
        Try
            For i As Integer = 0 To list.Length - 1
                Dim th As New Thread(AddressOf download) : th.Start(i)
                Thread.Sleep(rann)
            Next
        Catch ex As Exception

        End Try
    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged
        max_followers = NumericUpDown1.Value
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim SetSave As SaveFileDialog = New SaveFileDialog
        Dim i As Integer
        SetSave.Title = "Save SET.txt"
        SetSave.Filter = "SET.txt File (*.txt)|*.txt"

        If SetSave.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim s As New IO.StreamWriter(SetSave.FileName, True)
            For i = 0 To lst.Items.Count - 1
                s.WriteLine(lst.Items.Item(i))
            Next
            s.Close()
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim sfile As New SaveFileDialog
        With sfile
            .Title = ""
            .InitialDirectory = "C:\"
            .Filter = ("(*.txt) | *.txt")
        End With

        If sfile.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Dim Write As New IO.StreamWriter(sfile.FileName)
            Dim k As ListView.ColumnHeaderCollection = ListView1.Columns
            For Each x As ListViewItem In ListView1.Items
                Dim StrLn As String = ""
                For i = 0 To x.SubItems.Count - 1
                    If i = 2 Then
                        StrLn += x.SubItems(i).Text
                    Else
                        StrLn += x.SubItems(i).Text + ":"

                    End If
                Next
                Write.WriteLine(StrLn)
            Next
            Write.Close()
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim result As Integer = MessageBox.Show("هل تريد حفظ اللسته قبل الايقاف ؟", "Accounts info", MessageBoxButtons.YesNoCancel)
        If result = DialogResult.Cancel Then

        ElseIf result = DialogResult.No Then
            Timer1.Start()
        ElseIf result = DialogResult.Yes Then
            Dim sfile As New SaveFileDialog
            With sfile
                .Title = ""
                .InitialDirectory = "C:\"
                .Filter = ("(*.txt) | *.txt")
            End With

            If sfile.ShowDialog() = Windows.Forms.DialogResult.OK Then
                Dim Write As New IO.StreamWriter(sfile.FileName)
                Dim k As ListView.ColumnHeaderCollection = ListView1.Columns
                For Each x As ListViewItem In ListView1.Items
                    Dim StrLn As String = ""
                    For i = 0 To x.SubItems.Count - 1
                        If i = 2 Then
                            StrLn += x.SubItems(i).Text
                        Else
                            StrLn += x.SubItems(i).Text + ":"

                        End If
                    Next
                    Write.WriteLine(StrLn)
                Next
                Write.Close()
            End If
            Timer1.Start()
        End If
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        Label6.Invoke(New MethodInvoker(Sub() Label6.Text = done))

    End Sub

 
    Private Sub NumericUpDown2_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown2.ValueChanged
        max_following = NumericUpDown2.Value
    End Sub

    Private Sub NumericUpDown3_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown3.ValueChanged
        max_posts = NumericUpDown3.Value
    End Sub
End Class
