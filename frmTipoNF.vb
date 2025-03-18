Public Class frmTipoNF
    Public especie As String


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        My.Settings.Item("confEspecieSelecionada") = ConfirmarEspecie()
        Me.Dispose()
    End Sub


    Private Sub PesquisarEspecies()

        Dim dt As New DataTable

        'dt.Columns.Add("ESPECIE")



        Dim SQL As String = "SELECT DISTINCT ESPECIE FROM PCNFENT"


        cmd.Connection = conexao
        cmd.CommandText = SQL
        cmd.CommandType = CommandType.Text
        Try
            datareader = cmd.ExecuteReader

            Do While datareader.Read

                ComboBox1.Items.Add(datareader(0).ToString)

            Loop



            ComboBox1.Update()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erro")
            Exit Sub
        End Try
    End Sub



    Public Function ConfirmarEspecie() As String

        especie = ComboBox1.Text

        Return especie
    End Function

    Private Sub frmTipoNF_Load(sender As Object, e As EventArgs) Handles Me.Load
        PesquisarEspecies()
    End Sub

    Private Sub ComboBox1_LostFocus(sender As Object, e As EventArgs) Handles ComboBox1.LostFocus

        ConfirmarEspecie()

    End Sub
End Class