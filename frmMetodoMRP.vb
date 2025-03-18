Public Class frmMetodoMRP
    Private Sub btnDefinirMetodo_Click(sender As Object, e As EventArgs) Handles btnDefinirMetodo.Click


        If cboMetodos.Text = "" Then
            MessageBox.Show("Escolha um método válido")
            Exit Sub
        End If

        metodoescolhido = cboMetodos.Text

        Me.Dispose()


    End Sub
End Class