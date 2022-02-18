Imports System.IO
Imports System.Net
Imports System.Runtime.Serialization.Formatters.Binary

Public Class godaddyUpdateClient
    Dim godaddyData As GodaddyData
    Dim ipInfoURL As String = "https://api.ipify.org"
    Dim recordFileName As String = "tick.tmp"

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        godaddyData = New GodaddyData()

        'Update UI language
        Btn_Submit.Text = "Save"
        Btn_Cancel.Text = "Cancel"
        Label_API_Key.Text = "API KEY"
        Label_API_Secret.Text = "API Secret"
        Label_Domain.Text = "Domain"
        Label_Interval.Text = "Update Interval"
        Label_Hostname.Text = "Host Name"

        'Load records from file
        LoadRecord()

        'Display data if record exist
        UpdateUI()

        'if interval has been set
        If godaddyData.updateInterval > 0 Then
            updateTimerSetting()
        End If
    End Sub

    Private Sub Btn_Submit_Click(sender As Object, e As EventArgs) Handles Btn_Submit.Click
        godaddyData.key = TextBox_API_Key.Text
        godaddyData.secret = TextBox_API_Secret.Text
        godaddyData.domainName = TextBox_Domain.Text
        godaddyData.hostname = TextBox_Hostname.Text
        godaddyData.updateInterval = TextBox_Interval.Text
        SaveRecord()
        UpdateDomain()
        updateTimerSetting()
    End Sub

    Private Sub updateTimerSetting()
        Timer_updateDNS.Stop()
        Timer_updateDNS.Interval = godaddyData.updateInterval * 6000
        Timer_updateDNS.Start()
    End Sub
    Private Sub UpdateDomain()
        Dim client As WebClient = New WebClient()
        AddHandler client.DownloadDataCompleted, AddressOf DownloadDataCallback
        Dim uri As Uri = New Uri(ipInfoURL)
        client.DownloadDataAsync(uri, godaddyData)

    End Sub


    Private Shared Sub DownloadDataCallback(ByVal sender As Object, ByVal e As DownloadDataCompletedEventArgs)

        Dim dataPassIn As GodaddyData = CType(e.UserState, GodaddyData)

        Try
            If e.Cancelled = False AndAlso e.Error Is Nothing Then


                Dim data() As Byte = CType(e.Result, Byte())
                Dim ipInfo As String = System.Text.Encoding.UTF8.GetString(data)

                Dim authInfo As String = "sso-key " + dataPassIn.key + ":" + dataPassIn.secret
                Dim apiURL As String = "https://api.godaddy.com/v1/domains/" + dataPassIn.domainName + "/records/A/" + dataPassIn.hostname

                Dim web As New System.Net.WebClient()

                web.Headers.Add("Content-Type", "application/json")
                web.Headers.Add("Authorization", authInfo)

                Dim dictionary As New Dictionary(Of String, String)
                dictionary.Add("data", ipInfo)

                Dim dataForPost As Byte() = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(dictionary)

                Dim res As Byte() = web.UploadData(apiURL, "POST", dataForPost)

                MsgBox(System.Text.Encoding.UTF8.GetString(res))
            End If
        Catch ex As WebException

            If ex.Status = WebExceptionStatus.ProtocolError Then

                If (CType(ex.Response, HttpWebResponse)).StatusCode = HttpStatusCode.NotFound Then

                    'handle the 404 here
                    MsgBox("404")
                End If
            ElseIf ex.Status = WebExceptionStatus.NameResolutionFailure Then

                'handle name resolution failure

            End If


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
        Return True
    End Function

End Class

