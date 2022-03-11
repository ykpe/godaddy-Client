Imports System.IO
Imports System.Net
Imports System.Runtime.Serialization.Formatters.Binary

Public Class godaddyUpdateClient
    Dim godaddyData As GodaddyData
    Dim ipInfoURL As String = "https://api.ipify.org"
    Dim ip6InfoURL As String = "https://api64.ipify.org"
    Dim recordFileName As String = "tick.tmp"
    Private CloseAllowed As Boolean
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        godaddyData = New GodaddyData()

        'Update UI language
        Btn_Submit.Text = "Start"
        Btn_Cancel.Text = "Stop"
        Label_API_Key.Text = "API Key"
        Label_API_Secret.Text = "API Secret"
        Label_Domain.Text = "Domain"
        Label_Interval.Text = "Update (Mins)"
        Label_Hostname.Text = "Host Name"
        Label_UpdateState_4.Text = "IPv4:"
        Label_UpdateState_6.Text = "IPv6:"

        'Load records from file
        LoadRecord()

        'Display data if record exist
        UpdateUI()

        'if interval has been set
        If godaddyData.updateInterval > 0 Then
            UpdateDomain()
            StartTimer()
        Else
            StopTimer()
        End If
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Not CloseAllowed And e.CloseReason <> CloseReason.WindowsShutDown Then
            Me.Hide()
            e.Cancel = True
            NotifyIcon1.Visible = True
        End If
    End Sub

    Private Sub notifyIcon1_DoubleClick(Sender As Object, e As EventArgs) Handles NotifyIcon1.DoubleClick
        ' Show the form when the user double clicks on the notify icon.
        Me.Show()
        ' Set the WindowState to normal if the form is minimized.
        If (Me.WindowState = FormWindowState.Minimized) Then _
            Me.WindowState = FormWindowState.Normal

        ' Activate the form.
        Me.Activate()
    End Sub

    Private Sub Btn_Submit_Click(sender As Object, e As EventArgs) Handles Btn_Submit.Click
        If TextBox_API_Key.Text.Length > 0 Then
            godaddyData.key = TextBox_API_Key.Text
        Else
            Return
        End If
        If TextBox_API_Secret.Text.Length > 0 Then
            godaddyData.secret = TextBox_API_Secret.Text
        Else
            Return
        End If

        If TextBox_Domain.Text.Length > 0 Then
            godaddyData.domainName = TextBox_Domain.Text
        Else
            Return
        End If

        If TextBox_Hostname.Text.Length > 0 Then
            godaddyData.hostname = TextBox_Hostname.Text
        Else
            Return
        End If
        If TextBox_Interval.Text.Length > 0 Then
            godaddyData.updateInterval = Convert.ToInt32(TextBox_Interval.Text)
            If godaddyData.updateInterval > 35790 Then
                godaddyData.updateInterval = 35790
                TextBox_Interval.Text = "35790"
            End If
        Else
            Return
        End If

        SaveRecord()
        UpdateDomain()
        StartTimer()
    End Sub

    Private Sub StartTimer()
        Timer_updateDNS.Stop()
        Timer_updateDNS.Interval = godaddyData.updateInterval * 60000
        Timer_updateDNS.Start()

        Btn_Submit.Enabled = False
        Btn_Cancel.Enabled = True
    End Sub

    Private Sub StopTimer()
        Timer_updateDNS.Stop()
        Btn_Submit.Enabled = True
        Btn_Cancel.Enabled = False
    End Sub

    Private Sub UpdateDomain()
        Dim clientIPv4 As WebClient = New WebClient()
        AddHandler clientIPv4.DownloadDataCompleted, AddressOf DownloadDataCallback
        Dim uri As Uri = New Uri(ipInfoURL)
        clientIPv4.DownloadDataAsync(uri, 4)


        Dim clientIPv6 As WebClient = New WebClient()
        AddHandler clientIPv6.DownloadDataCompleted, AddressOf DownloadDataCallback
        Dim uri6 As Uri = New Uri(ip6InfoURL)
        clientIPv6.DownloadDataAsync(uri6, 6)

    End Sub

    Private Sub DownloadDataCallback(ByVal sender As Object, ByVal e As DownloadDataCompletedEventArgs)

        Dim currentIP As Integer = CType(e.UserState, Integer)

        Try
            If e.Cancelled = False AndAlso e.Error Is Nothing Then

                Dim infoArray(0) As Dictionary(Of String, Object)
                Dim dataStruct As New Dictionary(Of String, Object)
                Dim dataInfo() As Byte = CType(e.Result, Byte())

                dataStruct.Add("data", System.Text.Encoding.UTF8.GetString(dataInfo))
                infoArray(0) = dataStruct

                Dim authInfo As String = "sso-key " + godaddyData.key + ":" + godaddyData.secret
                Dim apiURL As String = ""
                If currentIP = 4 Then
                    Label_UpdateState_4.Text = "IPv4: " + System.Text.Encoding.UTF8.GetString(dataInfo)
                    apiURL = "https://api.godaddy.com/v1/domains/" + godaddyData.domainName + "/records/A/" + godaddyData.hostname
                Else
                    Label_UpdateState_6.Text = "IPv6: " + System.Text.Encoding.UTF8.GetString(dataInfo)
                    apiURL = "https://api.godaddy.com/v1/domains/" + godaddyData.domainName + "/records/AAAA/" + godaddyData.hostname
                End If

                Dim web As New System.Net.WebClient()
                web.Headers("accept") = "application/json"
                web.Headers("Content-Type") = "application/json"
                web.Headers("Authorization") = authInfo

                Dim dataForPost As String = System.Text.Json.JsonSerializer.Serialize(infoArray)
                AddHandler web.UploadStringCompleted, AddressOf UploadStringCallback2

                Dim uri As Uri = New Uri(apiURL)
                web.UploadStringAsync(uri, "PUT", dataForPost, currentIP)

            End If
        Catch ex As WebException
            If currentIP = 4 Then
                StopTimer()
                Label_UpdateState_4.Text = "IPv4:" + ex.Message + Now
            Else
                Label_UpdateState_6.Text = "IPv6:" + ex.Message + Now
            End If

        End Try
    End Sub

    Private Sub UploadStringCallback2(ByVal sender As Object, ByVal e As UploadStringCompletedEventArgs)
        Dim currentIP As Integer = CType(e.UserState, Integer)
        If currentIP = 4 Then
            Label_UpdateState_4.Text = "IPv4: Success " + Now
        Else
            Label_UpdateState_6.Text = "IPv6: Success " + Now
        End If

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer_updateDNS.Tick
        UpdateDomain()
    End Sub

    Private Sub UpdateUI()
        TextBox_API_Key.Text = godaddyData.key
        TextBox_API_Secret.Text = godaddyData.secret
        TextBox_Domain.Text = godaddyData.domainName
        TextBox_Hostname.Text = godaddyData.hostname
        TextBox_Interval.Text = godaddyData.updateInterval
    End Sub

    Private Sub LoadRecord()
        If My.Computer.FileSystem.FileExists(recordFileName) Then
            Dim rawString As String = My.Computer.FileSystem.ReadAllText(recordFileName)
            godaddyData.LoadDataFromString(rawString)
            System.Console.WriteLine(rawString)
            System.Console.WriteLine(godaddyData.secret)
        End If
    End Sub

    Private Sub SaveRecord()
        If My.Computer.FileSystem.FileExists(recordFileName) = True Then
            My.Computer.FileSystem.DeleteFile(recordFileName)
        End If
        My.Computer.FileSystem.WriteAllText(recordFileName, godaddyData.StringForWrite(), True)
    End Sub

    Private Sub SettingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SettingToolStripMenuItem.Click
        Me.Show()
    End Sub

    Private Sub ExitToolStripMenuItem_Click_1(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        CloseAllowed = True
        Me.Close()
    End Sub

    Private Sub Btn_Cancel_Click(sender As Object, e As EventArgs) Handles Btn_Cancel.Click
        StopTimer()
    End Sub

    Private Sub TextBox_Interval_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox_Interval.KeyPress
        If Char.IsDigit(e.KeyChar) Or Char.IsControl(e.KeyChar) Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub
End Class

Public Class GodaddyData
    Public key As String
    Public secret As String
    Public domainName As String
    Public hostname As String
    Public updateInterval As String

    Dim _splitToken As Char = ","
    Dim keyIndex As Integer = 0
    Dim secretIndex As Integer = 1
    Dim domainNameIndex As Integer = 2
    Dim hostNameIndex As Integer = 3
    Dim updateIntervalIndex As Integer = 4

    Public Function StringForWrite() As String
        Dim stringToWrite As New System.Text.StringBuilder()
        stringToWrite.Append(key)
        stringToWrite.Append(_splitToken)
        stringToWrite.Append(secret)
        stringToWrite.Append(_splitToken)
        stringToWrite.Append(domainName)
        stringToWrite.Append(_splitToken)
        stringToWrite.Append(hostname)
        stringToWrite.Append(_splitToken)
        stringToWrite.Append(updateInterval)
        Return stringToWrite.ToString()
    End Function

    Public Function LoadDataFromString(input As String)
        Dim datas() As String = input.Split(_splitToken)
        key = datas(keyIndex)
        secret = datas(secretIndex)
        domainName = datas(domainNameIndex)
        hostname = datas(hostNameIndex)
        updateInterval = datas(updateIntervalIndex)
        If updateInterval > 35790 Then
            updateInterval = 35790
        End If
        Return True
    End Function

End Class

