Imports StreamScaleIP
Public Class Form1
    Dim WithEvents StreamingScale As StreamScaleIP.StreamingScale
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub button1_Click(sender As Object, e As EventArgs) Handles button1.Click
        If StreamingScale IsNot Nothing Then
            StreamingScale.Disconnect()
            StreamingScale.Dispose()
            StreamingScale = Nothing
            tmrUpdate.Stop()
        Else
            StreamingScale = New StreamingScale("Truck Scale ", txtAddress.Text, numPort.Value)
            StreamingScale.Connect(500)
            tmrUpdate.Start()
        End If

    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If StreamingScale IsNot Nothing Then
            StreamingScale.Disconnect()
            StreamingScale.Dispose()
            StreamingScale = Nothing
        End If
    End Sub

    Private Sub tmrUpdate_Tick(sender As Object, e As EventArgs) Handles tmrUpdate.Tick
        If StreamingScale IsNot Nothing Then
            lblWeight.Text = StreamingScale.CurrentScaleData.CurWeight.ToString()
            lblStatus.Text = StreamingScale.CurrentScaleData.CurrentStatus.ToString()

            lblStatchar.Text = StreamingScale.CurrentScaleData.CurrentStatusChar
            lblMotion.Visible = StreamingScale.CurrentScaleData.Motion
        End If
    End Sub
End Class
