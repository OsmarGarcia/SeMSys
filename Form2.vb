Imports System.Drawing.Text
Imports System.Security.Cryptography

Public Class Form2
    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'ControlExtension.Draggable(Button1, True)
        'ControlExtension.Draggable(Button2, True)


    End Sub



    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim control = New TextBox
        Dim point = New Point
        Dim x As Random = New Random


        Dim mycontrol = New Panel
        mycontrol.Size = New Size(x.Next(1, 128), 22)
        mycontrol.BackColor = Color.FromArgb(x.Next(1, 255), x.Next(1, 255), x.Next(1, 255), x.Next(1, 255))
        mycontrol.BorderStyle = BorderStyle.FixedSingle

        FlowLayoutPanel1.Controls.Add(mycontrol)


    End Sub
End Class