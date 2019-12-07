Imports System.Net
Imports System.IO
Imports System.Web.Script.Serialization
Public Class MainApp
    Function Numeric2Bytes(ByVal b As Double) As String
        Dim bSize(8) As String
        Dim i As Integer

        bSize(0) = "Bytes"
        bSize(1) = "KB" 'Kilobytes
        bSize(2) = "MB" 'Megabytes
        bSize(3) = "GB" 'Gigabytes
        bSize(4) = "TB" 'Terabytes
        bSize(5) = "PB" 'Petabytes
        bSize(6) = "EB" 'Exabytes
        bSize(7) = "ZB" 'Zettabytes
        bSize(8) = "YB" 'Yottabytes

        b = CDbl(b) ' Make sure var is a Double (not just
        ' variant)
        For i = UBound(bSize) To 0 Step -1
            If b >= (1024 ^ i) Then
                Numeric2Bytes = ThreeNonZeroDigits(b / (1024 ^
                i)) & " " & bSize(i)
                Exit For
            End If
        Next
    End Function

    ' Return the value formatted to include at most three
    ' non-zero digits and at most two digits after the
    ' decimal point. Examples:
    '         1
    '       123
    '        12.3
    '         1.23
    '         0.12
    Private Function ThreeNonZeroDigits(ByVal value As Double) _
    As String
        If value >= 100 Then
            ' No digits after the decimal.
            Return Format$(CInt(value))
        ElseIf value >= 10 Then
            ' One digit after the decimal.
            Return Format$(value, "0.0")
        Else
            ' Two digits after the decimal.
            Return Format$(value, "0.00")
        End If
    End Function
    Public Function APIRequest(ByVal userid As String)
        Try

            Dim webClient As New System.Net.WebClient
            Dim result As String = webClient.DownloadString("https://api.chewey-bot.ga/analytics/getlatest/" & userid)
            Dim jss As JavaScriptSerializer = New JavaScriptSerializer()
            Dim dict As Object = jss.Deserialize(Of Dictionary(Of String, Object))(result)
            channels.Text = (dict("channels"))
            users.Text = (dict("users"))
            receivedmsg.Text = (dict("received_messages"))
            sentmsg.Text = (dict("sent_messages"))
            ramused.Text = Numeric2Bytes(dict("ram_used"))
            servers.Text = (dict("servers"))


            Dim date1 As String = dict("created")
            Dim parsedDate = DateTime.Parse(date1)

            statsdate.Text = "Data received on " & parsedDate
        Catch ex As Exception
            Try
                Dim webClient As New System.Net.WebClient
                Dim result As String = webClient.DownloadString("https://api.chewey-bot.ga/analytics/getlatest/" & userid)
                Dim jss As JavaScriptSerializer = New JavaScriptSerializer()
                Dim dict As Object = jss.Deserialize(Of Dictionary(Of String, Object))(result)
                Errortext.Text = dict("error")
            Catch ex1 As Exception
                Errortext.Text = "Possibly Downtime or Error Code 429"

            End Try

            PictureBox7.Enabled = False
            TabControl1.SelectTab(CooldownPage)
            Timer1.Start()
            Timer2.Start()

        End Try
    End Function
    Private Sub BunifuFlatButton1_Click(sender As Object, e As EventArgs)
        My.Computer.FileSystem.WriteAllText(Application.StartupPath() + "\userid.txt", BunifuMetroTextbox2.Text, False)

        APIRequest(BunifuMetroTextbox2.Text)

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Try

            Dim text As String = My.Computer.FileSystem.ReadAllText(Application.StartupPath() + "\userid.txt")
            BunifuMetroTextbox2.Text = text
        Catch ex As Exception

        End Try

        APIRequest(BunifuMetroTextbox2.Text)



    End Sub

    Private Sub PictureBox10_Click(sender As Object, e As EventArgs) Handles PictureBox10.Click

        APIRequest(BunifuMetroTextbox2.Text)

    End Sub
#Region " Move Form "

    ' [ Move Form ]
    '
    ' // By Elektro 

    Public MoveForm As Boolean
    Public MoveForm_MousePosition As Point

    Public Sub MoveForm_MouseDown(sender As Object, e As MouseEventArgs) Handles BunifuGradientPanel2.MouseDown
        ' Add more handles here (Example: PictureBox1.MouseDown)

        If e.Button = MouseButtons.Left Then
            MoveForm = True
            Me.Cursor = Cursors.NoMove2D
            MoveForm_MousePosition = e.Location
        End If

    End Sub

    Public Sub MoveForm_MouseMove(sender As Object, e As MouseEventArgs) Handles BunifuGradientPanel2.MouseMove
        ' Add more handles here (Example: PictureBox1.MouseMove)

        If MoveForm Then
            Me.Location = Me.Location + (e.Location - MoveForm_MousePosition)
        End If

    End Sub

    Public Sub MoveForm_MouseUp(sender As Object, e As MouseEventArgs) Handles BunifuGradientPanel2.MouseUp
        ' Add more handles here (Example: PictureBox1.MouseUp)

        If e.Button = MouseButtons.Left Then
            MoveForm = False
            Me.Cursor = Cursors.Default
        End If

    End Sub

#End Region
    Private Sub PictureBox11_Click(sender As Object, e As EventArgs) Handles PictureBox11.Click
        Close()
    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click
        TabControl1.SelectTab(SettingsPage)
    End Sub

    Private Sub PictureBox7_Click(sender As Object, e As EventArgs) Handles PictureBox7.Click
        TabControl1.SelectTab(IndexPage)

    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Dim webAddress As String = "https://www.patreon.com/CheweyZ"
        Process.Start(webAddress)
    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click
        TabControl1.SelectTab(FAQPage)

    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        Dim webAddress1 As String = "https://discordapp.com/invite/PhcYY57"
        Process.Start(webAddress1)
    End Sub

    Private Sub LinkLabel3_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel3.LinkClicked
        Dim webAddress1 As String = "https://ko-fi.com/lxibot"
        Process.Start(webAddress1)
    End Sub

    Private Sub Label41_Click(sender As Object, e As EventArgs) Handles Label41.Click
        TabControl1.SelectTab(ChartPage)

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        TabControl1.SelectTab(IndexPage)
        Timer1.Stop()
        Timer2.Stop()
        BunifuProgressBar1.Value = 0
        PictureBox7.Enabled = True

        If servers.Text = "Loading" Then
            APIRequest(BunifuMetroTextbox2.Text)
        End If
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        BunifuProgressBar1.Value = BunifuProgressBar1.Value + 1
    End Sub

    Private Sub PictureBox19_Click(sender As Object, e As EventArgs) Handles PictureBox19.Click
        
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub BunifuSeparator12_Load(sender As Object, e As EventArgs) Handles BunifuSeparator12.Load

    End Sub

    Private Sub Label46_Click(sender As Object, e As EventArgs) Handles Label46.Click
        TabControl1.SelectTab(ChangelogPage)

    End Sub
End Class
