﻿Imports System.IO
Imports System.Net
Imports System.Runtime.Serialization.Formatters.Binary

Public Class godaddyUpdateClient
    Dim godaddyData As GodaddyData
    Dim ipInfoURL As String = "https://api.ipify.org"
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

        'Load records from file
        LoadRecord()

        'Display data if record exist
        UpdateUI()

        'if interval has been set
        If godaddyData.updateInterval > 0 Then
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
        godaddyData.key = TextBox_API_Key.Text
        godaddyData.secret = TextBox_API_Secret.Text
        godaddyData.domainName = TextBox_Domain.Text
        godaddyData.hostname = TextBox_Hostname.Text
        godaddyData.updateInterval = TextBox_Interval.Text
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
        Dim client As WebClient = New WebClient()
        AddHandler client.DownloadDataCompleted, AddressOf DownloadDataCallback
        Dim uri As Uri = New Uri(ipInfoURL)
        client.DownloadDataAsync(uri, godaddyData)

    End Sub

    Private Shared Sub DownloadDataCallback(ByVal sender As Object, ByVal e As DownloadDataCompletedEventArgs)

        Dim dataPassIn As GodaddyData = CType(e.UserState, GodaddyData)

        Try
            If e.Cancelled = False AndAlso e.Error Is Nothing Then

                Dim infoArray(0) As Dictionary(Of String, Object)
                Dim dataStruct As New Dictionary(Of String, Object)
                Dim dataInfo() As Byte = CType(e.Result, Byte())

                dataStruct.Add("data", System.Text.Encoding.UTF8.GetString(dataInfo))
                infoArray(0) = dataStruct

                Dim authInfo As String = "sso-key " + dataPassIn.key + ":" + dataPassIn.secret
                Dim apiURL As String = "https://api.godaddy.com/v1/domains/" + dataPassIn.domainName + "/records/A/" + dataPassIn.hostname

                Dim web As New System.Net.WebClient()
                web.Headers("accept") = "application/json"
                web.Headers("Content-Type") = "application/json"
                web.Headers("Authorization") = authInfo

                Dim dataForPost As String = System.Text.Json.JsonSerializer.Serialize(infoArray)
                Dim res As String = web.UploadString(apiURL, "PUT", dataForPost)
            End If
        Catch ex As WebException
            MsgBox(ex.Message)
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

