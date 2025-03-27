Public Class frmOrdemProducao
    Private Sub btnImprimir_Click(sender As Object, e As EventArgs) Handles btnImprimir.Click
        Dim r As New ClassOrdemProducao
        Dim numop = txtNumOP.Text
        Dim offset = txtOffset.Text

        Try
            r.ImprimirOP(numop, offset)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
End Class