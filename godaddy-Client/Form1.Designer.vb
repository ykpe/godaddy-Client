<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class godaddyUpdateClient
    Inherits System.Windows.Forms.Form

    'Form 覆寫 Dispose 以清除元件清單。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    '為 Windows Form 設計工具的必要項
    Private components As System.ComponentModel.IContainer

    '注意: 以下為 Windows Form 設計工具所需的程序
    '可以使用 Windows Form 設計工具進行修改。
    '請勿使用程式碼編輯器進行修改。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(godaddyUpdateClient))
        Me.TextBox_Domain = New System.Windows.Forms.TextBox()
        Me.Label_Domain = New System.Windows.Forms.Label()
        Me.Label_API_Key = New System.Windows.Forms.Label()
        Me.TextBox_API_Key = New System.Windows.Forms.TextBox()
        Me.TextBox_API_Secret = New System.Windows.Forms.TextBox()
        Me.TextBox_Interval = New System.Windows.Forms.TextBox()
        Me.Label_API_Secret = New System.Windows.Forms.Label()
        Me.Label_Interval = New System.Windows.Forms.Label()
        Me.Btn_Submit = New System.Windows.Forms.Button()
        Me.Btn_Cancel = New System.Windows.Forms.Button()
        Me.Timer_updateDNS = New System.Windows.Forms.Timer(Me.components)
        Me.TextBox_Hostname = New System.Windows.Forms.TextBox()
        Me.Label_Hostname = New System.Windows.Forms.Label()
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SettingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Label_UpdateState_4 = New System.Windows.Forms.Label()
        Me.Label_UpdateState_6 = New System.Windows.Forms.Label()
        Me.CheckBoxIPv6 = New System.Windows.Forms.CheckBox()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TextBox_Domain
        '
        Me.TextBox_Domain.Location = New System.Drawing.Point(115, 38)
        Me.TextBox_Domain.Name = "TextBox_Domain"
        Me.TextBox_Domain.Size = New System.Drawing.Size(336, 22)
        Me.TextBox_Domain.TabIndex = 0
        '
        'Label_Domain
        '
        Me.Label_Domain.AutoSize = True
        Me.Label_Domain.Location = New System.Drawing.Point(26, 38)
        Me.Label_Domain.Name = "Label_Domain"
        Me.Label_Domain.Size = New System.Drawing.Size(37, 12)
        Me.Label_Domain.TabIndex = 1
        Me.Label_Domain.Text = "Label1"
        '
        'Label_API_Key
        '
        Me.Label_API_Key.AutoSize = True
        Me.Label_API_Key.Location = New System.Drawing.Point(26, 97)
        Me.Label_API_Key.Name = "Label_API_Key"
        Me.Label_API_Key.Size = New System.Drawing.Size(37, 12)
        Me.Label_API_Key.TabIndex = 2
        Me.Label_API_Key.Text = "Label2"
        '
        'TextBox_API_Key
        '
        Me.TextBox_API_Key.Location = New System.Drawing.Point(115, 97)
        Me.TextBox_API_Key.Name = "TextBox_API_Key"
        Me.TextBox_API_Key.Size = New System.Drawing.Size(336, 22)
        Me.TextBox_API_Key.TabIndex = 3
        '
        'TextBox_API_Secret
        '
        Me.TextBox_API_Secret.Location = New System.Drawing.Point(115, 147)
        Me.TextBox_API_Secret.Name = "TextBox_API_Secret"
        Me.TextBox_API_Secret.Size = New System.Drawing.Size(336, 22)
        Me.TextBox_API_Secret.TabIndex = 4
        '
        'TextBox_Interval
        '
        Me.TextBox_Interval.Location = New System.Drawing.Point(115, 243)
        Me.TextBox_Interval.Name = "TextBox_Interval"
        Me.TextBox_Interval.Size = New System.Drawing.Size(336, 22)
        Me.TextBox_Interval.TabIndex = 10
        '
        'Label_API_Secret
        '
        Me.Label_API_Secret.AutoSize = True
        Me.Label_API_Secret.Location = New System.Drawing.Point(26, 150)
        Me.Label_API_Secret.Name = "Label_API_Secret"
        Me.Label_API_Secret.Size = New System.Drawing.Size(37, 12)
        Me.Label_API_Secret.TabIndex = 6
        Me.Label_API_Secret.Text = "Label3"
        '
        'Label_Interval
        '
        Me.Label_Interval.AutoSize = True
        Me.Label_Interval.Location = New System.Drawing.Point(26, 243)
        Me.Label_Interval.Name = "Label_Interval"
        Me.Label_Interval.Size = New System.Drawing.Size(37, 12)
        Me.Label_Interval.TabIndex = 7
        Me.Label_Interval.Text = "Label4"
        '
        'Btn_Submit
        '
        Me.Btn_Submit.Location = New System.Drawing.Point(84, 306)
        Me.Btn_Submit.Name = "Btn_Submit"
        Me.Btn_Submit.Size = New System.Drawing.Size(75, 23)
        Me.Btn_Submit.TabIndex = 8
        Me.Btn_Submit.Text = "Button1"
        Me.Btn_Submit.UseVisualStyleBackColor = True
        '
        'Btn_Cancel
        '
        Me.Btn_Cancel.Location = New System.Drawing.Point(344, 306)
        Me.Btn_Cancel.Name = "Btn_Cancel"
        Me.Btn_Cancel.Size = New System.Drawing.Size(75, 23)
        Me.Btn_Cancel.TabIndex = 9
        Me.Btn_Cancel.Text = "Button2"
        Me.Btn_Cancel.UseVisualStyleBackColor = True
        '
        'Timer_updateDNS
        '
        '
        'TextBox_Hostname
        '
        Me.TextBox_Hostname.Location = New System.Drawing.Point(115, 194)
        Me.TextBox_Hostname.Name = "TextBox_Hostname"
        Me.TextBox_Hostname.Size = New System.Drawing.Size(336, 22)
        Me.TextBox_Hostname.TabIndex = 5
        '
        'Label_Hostname
        '
        Me.Label_Hostname.AutoSize = True
        Me.Label_Hostname.Location = New System.Drawing.Point(26, 197)
        Me.Label_Hostname.Name = "Label_Hostname"
        Me.Label_Hostname.Size = New System.Drawing.Size(37, 12)
        Me.Label_Hostname.TabIndex = 11
        Me.Label_Hostname.Text = "Label1"
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.ContextMenuStrip = Me.ContextMenuStrip1
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Godaddy Client"
        Me.NotifyIcon1.Visible = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SettingToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(115, 48)
        '
        'SettingToolStripMenuItem
        '
        Me.SettingToolStripMenuItem.Name = "SettingToolStripMenuItem"
        Me.SettingToolStripMenuItem.Size = New System.Drawing.Size(114, 22)
        Me.SettingToolStripMenuItem.Text = "Setting"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(114, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'Label_UpdateState_4
        '
        Me.Label_UpdateState_4.AutoSize = True
        Me.Label_UpdateState_4.Location = New System.Drawing.Point(49, 335)
        Me.Label_UpdateState_4.Name = "Label_UpdateState_4"
        Me.Label_UpdateState_4.Size = New System.Drawing.Size(37, 12)
        Me.Label_UpdateState_4.TabIndex = 12
        Me.Label_UpdateState_4.Text = "Label1"
        '
        'Label_UpdateState_6
        '
        Me.Label_UpdateState_6.AutoSize = True
        Me.Label_UpdateState_6.Location = New System.Drawing.Point(49, 360)
        Me.Label_UpdateState_6.Name = "Label_UpdateState_6"
        Me.Label_UpdateState_6.Size = New System.Drawing.Size(37, 12)
        Me.Label_UpdateState_6.TabIndex = 13
        Me.Label_UpdateState_6.Text = "Label1"
        '
        'CheckBoxIPv6
        '
        Me.CheckBoxIPv6.AutoSize = True
        Me.CheckBoxIPv6.Location = New System.Drawing.Point(28, 359)
        Me.CheckBoxIPv6.Name = "CheckBoxIPv6"
        Me.CheckBoxIPv6.Size = New System.Drawing.Size(15, 14)
        Me.CheckBoxIPv6.TabIndex = 14
        Me.CheckBoxIPv6.UseVisualStyleBackColor = True
        '
        'godaddyUpdateClient
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(517, 399)
        Me.Controls.Add(Me.CheckBoxIPv6)
        Me.Controls.Add(Me.Label_UpdateState_6)
        Me.Controls.Add(Me.Label_UpdateState_4)
        Me.Controls.Add(Me.Label_Hostname)
        Me.Controls.Add(Me.TextBox_Hostname)
        Me.Controls.Add(Me.Btn_Cancel)
        Me.Controls.Add(Me.Btn_Submit)
        Me.Controls.Add(Me.Label_Interval)
        Me.Controls.Add(Me.Label_API_Secret)
        Me.Controls.Add(Me.TextBox_Interval)
        Me.Controls.Add(Me.TextBox_API_Secret)
        Me.Controls.Add(Me.TextBox_API_Key)
        Me.Controls.Add(Me.Label_API_Key)
        Me.Controls.Add(Me.Label_Domain)
        Me.Controls.Add(Me.TextBox_Domain)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "godaddyUpdateClient"
        Me.Text = "Godaddy Client"
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TextBox_Domain As TextBox
    Friend WithEvents Label_Domain As Label
    Friend WithEvents Label_API_Key As Label
    Friend WithEvents TextBox_API_Key As TextBox
    Friend WithEvents TextBox_API_Secret As TextBox
    Friend WithEvents TextBox_Interval As TextBox
    Friend WithEvents Label_API_Secret As Label
    Friend WithEvents Label_Interval As Label
    Friend WithEvents Btn_Submit As Button
    Friend WithEvents Btn_Cancel As Button
    Friend WithEvents Timer_updateDNS As Timer
    Friend WithEvents TextBox_Hostname As TextBox
    Friend WithEvents Label_Hostname As Label
    Friend WithEvents NotifyIcon1 As NotifyIcon
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents SettingToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents Label_UpdateState_4 As Label
    Friend WithEvents Label_UpdateState_6 As Label
    Friend WithEvents CheckBoxIPv6 As CheckBox
End Class
