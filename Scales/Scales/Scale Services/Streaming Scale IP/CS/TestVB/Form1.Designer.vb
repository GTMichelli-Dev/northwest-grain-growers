<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.button1 = New System.Windows.Forms.Button()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.label3 = New System.Windows.Forms.Label()
        Me.lblWeight = New System.Windows.Forms.Label()
        Me.label2 = New System.Windows.Forms.Label()
        Me.label1 = New System.Windows.Forms.Label()
        Me.numPort = New System.Windows.Forms.NumericUpDown()
        Me.txtAddress = New System.Windows.Forms.TextBox()
        Me.tmrUpdate = New System.Windows.Forms.Timer(Me.components)
        Me.lblStatchar = New System.Windows.Forms.Label()
        Me.lblMotion = New System.Windows.Forms.Label()
        CType(Me.numPort, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'button1
        '
        Me.button1.Location = New System.Drawing.Point(509, 209)
        Me.button1.Name = "button1"
        Me.button1.Size = New System.Drawing.Size(256, 76)
        Me.button1.TabIndex = 15
        Me.button1.Text = "Connect"
        Me.button1.UseVisualStyleBackColor = True
        '
        'lblStatus
        '
        Me.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblStatus.Location = New System.Drawing.Point(0, 391)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(783, 51)
        Me.lblStatus.TabIndex = 14
        Me.lblStatus.Text = "Status"
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'label3
        '
        Me.label3.AutoSize = True
        Me.label3.Location = New System.Drawing.Point(51, 146)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(167, 29)
        Me.label3.TabIndex = 13
        Me.label3.Text = "Scale Weight"
        '
        'lblWeight
        '
        Me.lblWeight.BackColor = System.Drawing.Color.White
        Me.lblWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 22.125!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWeight.ForeColor = System.Drawing.Color.Black
        Me.lblWeight.Location = New System.Drawing.Point(48, 209)
        Me.lblWeight.Name = "lblWeight"
        Me.lblWeight.Size = New System.Drawing.Size(355, 76)
        Me.lblWeight.TabIndex = 12
        Me.lblWeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'label2
        '
        Me.label2.AutoSize = True
        Me.label2.Location = New System.Drawing.Point(500, 11)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(61, 29)
        Me.label2.TabIndex = 11
        Me.label2.Text = "Port"
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(51, 11)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(109, 29)
        Me.label1.TabIndex = 10
        Me.label1.Text = "Address"
        '
        'numPort
        '
        Me.numPort.Location = New System.Drawing.Point(496, 69)
        Me.numPort.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.numPort.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.numPort.Name = "numPort"
        Me.numPort.Size = New System.Drawing.Size(229, 35)
        Me.numPort.TabIndex = 9
        Me.numPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.numPort.Value = New Decimal(New Integer() {10001, 0, 0, 0})
        '
        'txtAddress
        '
        Me.txtAddress.Location = New System.Drawing.Point(50, 68)
        Me.txtAddress.Margin = New System.Windows.Forms.Padding(7, 6, 7, 6)
        Me.txtAddress.Name = "txtAddress"
        Me.txtAddress.Size = New System.Drawing.Size(353, 35)
        Me.txtAddress.TabIndex = 8
        Me.txtAddress.Text = "172.30.35.226"
        '
        'tmrUpdate
        '
        Me.tmrUpdate.Interval = 200
        '
        'lblStatchar
        '
        Me.lblStatchar.Location = New System.Drawing.Point(311, 146)
        Me.lblStatchar.Name = "lblStatchar"
        Me.lblStatchar.Size = New System.Drawing.Size(54, 38)
        Me.lblStatchar.TabIndex = 16
        Me.lblStatchar.Text = " "
        '
        'lblMotion
        '
        Me.lblMotion.AutoSize = True
        Me.lblMotion.ForeColor = System.Drawing.Color.Red
        Me.lblMotion.Location = New System.Drawing.Point(95, 301)
        Me.lblMotion.Name = "lblMotion"
        Me.lblMotion.Size = New System.Drawing.Size(92, 29)
        Me.lblMotion.TabIndex = 17
        Me.lblMotion.Text = "Motion"
        Me.lblMotion.Visible = False
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(15.0!, 29.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(783, 442)
        Me.Controls.Add(Me.lblMotion)
        Me.Controls.Add(Me.lblStatchar)
        Me.Controls.Add(Me.button1)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.label3)
        Me.Controls.Add(Me.lblWeight)
        Me.Controls.Add(Me.label2)
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.numPort)
        Me.Controls.Add(Me.txtAddress)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.Name = "Form1"
        Me.Text = "Form1"
        CType(Me.numPort, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private WithEvents button1 As Button
    Private WithEvents lblStatus As Label
    Private WithEvents label3 As Label
    Private WithEvents lblWeight As Label
    Private WithEvents label2 As Label
    Private WithEvents label1 As Label
    Private WithEvents numPort As NumericUpDown
    Private WithEvents txtAddress As TextBox
    Friend WithEvents tmrUpdate As Timer
    Friend WithEvents lblStatchar As Label
    Private WithEvents lblMotion As Label
End Class
