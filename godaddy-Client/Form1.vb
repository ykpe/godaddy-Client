Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Text

Public Class godaddyUpdateClient
    Dim godaddyData As GodaddyData
    Dim ipInfoURL As String = "https://api.ipify.org"
    Dim ip6InfoURL As String = "https://api64.ipify.org"
    Dim recordIPv4 As String = "127.0.0.1"
    Dim recordIPv6 As String = "[::1]"
    Dim MODE_IPV4 As String = "4"
    Dim MODE_IPV6 As String = "6"

    Dim recordFileName As String = "tick.tmp"
    Private CloseAllowed As Boolean
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        godaddyData = New GodaddyData()
        Me.Text = "Godaddy Client " + My.Application.Info.Version.ToString
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

        godaddyData.enableIPv6 = CheckBoxIPv6.Checked

        SaveRecord()
        UpdateDomain()
        StartTimer()

    End Sub

    Private Sub StartTimer()
        Timer_updateDNS.Stop()
        Timer_updateDNS.Interval = godaddyData.updateInterval * 60000
        Timer_updateDNS.Start()

        Btn_Submit.Enabled = False
        CheckBoxIPv6.Enabled = False
        Btn_Cancel.Enabled = True
    End Sub

    Private Sub StopTimer()
        Timer_updateDNS.Stop()
        Btn_Submit.Enabled = True
        CheckBoxIPv6.Enabled = True
        Btn_Cancel.Enabled = False
    End Sub

    Private Sub UpdateDomain()

        UpdateGodaddyAsync(MODE_IPV4)

        If CheckBoxIPv6.Checked = True Then

            UpdateGodaddyAsync(MODE_IPV6)

        End If

    End Sub



    Private Async Sub UpdateGodaddyAsync(currentMode As String)

        Try

            Dim uriForIPInfo As Uri
            If currentMode = MODE_IPV4 Then
                uriForIPInfo = New Uri(ipInfoURL)
            Else
                uriForIPInfo = New Uri(ip6InfoURL)
            End If

            Dim clientForIPInfo As New HttpClient()
            Dim rspForIPInfo As HttpResponseMessage
            rspForIPInfo = Await clientForIPInfo.GetAsync(uriForIPInfo)
            rspForIPInfo.EnsureSuccessStatusCode()

            Dim newIP = Await rspForIPInfo.Content.ReadAsStringAsync()

            If currentMode = MODE_IPV4 Then
                If newIP = recordIPv4 Then
                    Label_UpdateState_4.Text = "IPv4: IP Not Changed " + Now
                    Return
                Else
                    recordIPv4 = newIP.ToString
                End If
            Else
                If newIP = recordIPv6 Then
                    Label_UpdateState_6.Text = "IPv6: IP Not Changed " + Now
                    Return
                Else
                    If newIP.Length > 0 Then
                        Dim arr() = newIP.Split(":")
                        If arr.Length > 2 Then
                            recordIPv6 = newIP.ToString
                        Else
                            Label_UpdateState_6.Text = "IPv6: IP Not Detect " + Now
                        End If
                    End If
                End If
            End If

            Dim infoArray(0) As Dictionary(Of String, Object)
            Dim dataStruct As New Dictionary(Of String, Object)
            dataStruct.Add("data", newIP)
            infoArray(0) = dataStruct
            Dim dataForPost As String = System.Text.Json.JsonSerializer.Serialize(infoArray)


            Dim authInfo As String = godaddyData.key + ":" + godaddyData.secret
            Dim apiURL As String = ""
            If currentMode = MODE_IPV4 Then
                Label_UpdateState_4.Text = "IPv4: " + newIP
                apiURL = "https://api.godaddy.com/v1/domains/" + godaddyData.domainName + "/records/A/" + godaddyData.hostname
            Else
                Label_UpdateState_6.Text = "IPv6: " + newIP
                apiURL = "https://api.godaddy.com/v1/domains/" + godaddyData.domainName + "/records/AAAA/" + godaddyData.hostname
            End If

            Dim client As New HttpClient()
            Dim rspn As HttpResponseMessage
            Dim uriWebApi As New Uri(apiURL)
            Dim hContent As HttpContent = New StringContent(dataForPost, Encoding.UTF8, "application/json")
            Dim token As AuthenticationHeaderValue = New AuthenticationHeaderValue("sso-key", authInfo)
            client.DefaultRequestHeaders.Authorization = token
            rspn = Await client.PutAsync(uriWebApi, hContent)
            rspn.EnsureSuccessStatusCode()

            If currentMode = MODE_IPV4 Then
                Label_UpdateState_4.Text = "IPv4: Success " + Now
            Else
                Label_UpdateState_6.Text = "IPv6: Success " + Now
            End If

        Catch ex As Exception
            Dim errorMsg As String = ""
            If ex.Message IsNot Nothing Then
                errorMsg = ex.Message.Substring(0, Math.Min(ex.Message.Length, 40))
            End If
            If currentMode = 4 Then
                Label_UpdateState_4.Text = "IPv4:" + errorMsg + Now
            Else
                Label_UpdateState_6.Text = "IPv6:" + errorMsg + Now
            End If
        Finally

        End Try


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
        CheckBoxIPv6.Checked = godaddyData.enableIPv6
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

    Private Sub CheckBoxIPv6_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxIPv6.CheckedChanged
        If CheckBoxIPv6.CheckState = False Then
            Label_UpdateState_6.Text = "IPv6:"
        End If

    End Sub
End Class

Public Class GodaddyData
    Public key As String
    Public secret As String
    Public domainName As String
    Public hostname As String
    Public updateInterval As String
    Public enableIPv6 As Boolean

    Dim _splitToken As Char = ","
    Dim keyIndex As Integer = 0
    Dim secretIndex As Integer = 1
    Dim domainNameIndex As Integer = 2
    Dim hostNameIndex As Integer = 3
    Dim updateIntervalIndex As Integer = 4
    Dim enableIPv6Index As Integer = 5

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
        stringToWrite.Append(_splitToken)
        stringToWrite.Append(enableIPv6.ToString)
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

        enableIPv6 = datas(enableIPv6Index)

        Return True
    End Function

End Class

