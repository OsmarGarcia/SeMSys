Public Class frmResumoOP
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnPesquisar.Click


        If txtNumOP.Text = "" Then
            MessageBox.Show(Me, "Insira pelo menos uma OP para gerar o relatório", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If


        Dim r As New ClassResumoOP
        Dim numop = txtNumOP.Text

        Try
            r.ResumirOP(numop)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try



    End Sub
End Class