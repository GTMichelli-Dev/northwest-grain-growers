Module Module1

    Sub Main()
        Dim f As New System.IO.Ports.SerialPort
        f.NewLine = Chr(13)
        Dim Test As String
        Dim InString As String
        Dim AllString = " 1    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5540    001    5540    001    5540    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    001    5560    "

        Dim WTString As Array = AllString.Split(Chr(2))
        For OneWt = WTString.Length - 1 To 0 Step -1
            InString = WTString(OneWt)
            InString = InString.Replace(Chr(13), "")
            InString = InString.Replace(Chr(10), "")
            If InString.Length > 14 Then
                Dim WeightArray As Array = InString.ToCharArray
            End If
        Next
        Test = WTString.Length.ToString()

        If InString <> "" And InString > 12 Then
            InString = Mid(InString, 6, 6)

            Dim WeightArray As Array = InString.ToCharArray


        End If
    End Sub

End Module
