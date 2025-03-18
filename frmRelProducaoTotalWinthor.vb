Public Class frmRelProducaoTotalWinthor
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnPesquisar.Click


        If dtpDtIncicio.Text = "" Or dtpDtinal.Text = "" Or
            DateTime.Parse(dtpDtinal.Text) < DateTime.Parse(dtpDtIncicio.Text) Then

            MessageBox.Show(Me, "Insira uma data válida", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)


            Exit Sub
        End If




        Dim r As New ClassProducaoTotalWinthor
        Dim dtincio As String = dtpDtIncicio.Text
        Dim dtfim As String = dtpDtinal.Text

        Try
            r.ConsultarProducaoTotal(dtincio, dtfim)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try



    End Sub

    Private Sub frmRelProducaoTotalWinthor_Load(sender As Object, e As EventArgs) Handles Me.Load

        dtpDtIncicio.Format = DateTimePickerFormat.Custom
        dtpDtIncicio.CustomFormat = "dd/MM/yyyy"


        dtpDtinal.Format = DateTimePickerFormat.Custom
        dtpDtinal.CustomFormat = "dd/MM/yyyy"
    End Sub
End Class