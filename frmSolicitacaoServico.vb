Imports System.Data.SqlClient

Public Class frmSolicitacaoServico


    Private Property Solicitante As String

    Private Sub frmSolicitacaoServico_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        Solicitante = My.Settings.UsuarioWinthor & "_" & My.Settings.NomeWinthor
        txtSolicitante.Text = Solicitante


        cboPrioridade.DropDownStyle = ComboBoxStyle.DropDownList
        cboPrioridade.Items.Add("A")
        cboPrioridade.Items.Add("B")
        cboPrioridade.Items.Add("C")


        cboTipoServico.DropDownStyle = ComboBoxStyle.DropDownList
        cboTipoServico.Items.Add("CORRETIVA")
        cboTipoServico.Items.Add("PREVENTIVA")
        cboTipoServico.Items.Add("MELHORIA")
        cboTipoServico.Items.Add("PREDIAL")


    End Sub



    Private Function GerarSolicitacao(solicitante As String, local As String, equipamento As String, tipo As String, prioridade As String, descricao As String)

        If conexaoSQL.State = 0 Then ConectaSRVSQL()


        ' Validação dos parâmetros
        If String.IsNullOrWhiteSpace(solicitante) OrElse
           String.IsNullOrWhiteSpace(local) OrElse
           String.IsNullOrWhiteSpace(equipamento) OrElse
           String.IsNullOrWhiteSpace(tipo) OrElse
           String.IsNullOrWhiteSpace(prioridade) OrElse
           String.IsNullOrWhiteSpace(descricao) Then
            MessageBox.Show("Todos os campos devem ser preenchidos.", "Erro")
            Return "NOK"
        End If

        ' Tenta se conectar e inserir os dados no banco de dados
        Try


            Dim query As String = "INSERT INTO tblSolicitacaoServico (DTSOLICITACAO2, DESCRICAO, EQUIPAMENTO, LOCAL, SOLICITANTE, STATUS, PRIORIDADE, TIPOSERVICO) " &
                                  "VALUES (@DTSOLICITACAO, @DESCRICAO, @EQUIPAMENTO, @LOCAL, @SOLICITANTE, @STATUS, @PRIORIDADE, @TIPOSERVICO)"

            Using cmd As New SqlCommand(query, conexaoSQL)
                cmd.Parameters.AddWithValue("@DTSOLICITACAO", DateTime.Now)
                cmd.Parameters.AddWithValue("@DESCRICAO", descricao)
                cmd.Parameters.AddWithValue("@EQUIPAMENTO", equipamento)
                cmd.Parameters.AddWithValue("@LOCAL", local)
                cmd.Parameters.AddWithValue("@SOLICITANTE", solicitante)
                cmd.Parameters.AddWithValue("@STATUS", "Aberta")
                cmd.Parameters.AddWithValue("@PRIORIDADE", prioridade)
                cmd.Parameters.AddWithValue("@TIPOSERVICO", tipo)

                cmd.ExecuteNonQuery()
            End Using

            MessageBox.Show("Solicitação gerada com sucesso!", "Sucesso")

            Return "OK"
        Catch ex As Exception
            MessageBox.Show("Erro ao gerar a solicitação: " & ex.Message, "Erro")
            Return "NOK"
        Finally
            cmd.Dispose()
        End Try
    End Function

    Private Sub btnConfirmar_Click(sender As Object, e As EventArgs) Handles btnConfirmar.Click
        Dim solicitante As String = txtSolicitante.Text
        Dim local As String = txtLocal.Text
        Dim equipamento As String = txtEquipamento.Text
        Dim tipo As String = cboTipoServico.Text
        Dim prioridade As String = cboPrioridade.Text
        Dim descricao As String = txtDescricaoServico.Text

        If GerarSolicitacao(solicitante, local, equipamento, tipo, prioridade, descricao) = "OK" Then


            For Each ctrl As Control In Me.Controls
                ' Verifica se o controle é um TextBox
                If TypeOf ctrl Is TextBox Then
                    DirectCast(ctrl, TextBox).Clear()
                End If
            Next

        End If

    End Sub




    Private flagTabPage2Loaded As Boolean = False

    Private Sub TabControl1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControl1.SelectedIndexChanged


        ' Verifica se a aba ativa é a aba desejada (ex: TabPage2)
        If TabControl1.SelectedTab Is TabPage2 Then
            If Not flagTabPage2Loaded Then
                ' Chama a função para popular o DataGridView
                DataGridView1.DataSource = consultarSolicitacoes()
                flagTabPage2Loaded = True
            End If
        Else
            ' Quando sair da aba, resetar o flag
            If TabControl1.SelectedTab IsNot TabPage2 Then
                flagTabPage2Loaded = False
            End If
        End If


    End Sub



    Private Function consultarSolicitacoes() As DataTable
        Dim dataTable As New DataTable()

        Try
            If conexaoSQL.State = 0 Then ConectaSRVSQL()

            Dim query As String = "SELECT 
                                    UPPER(DESCRICAO) AS DESCRICAO,
                                    UPPER(LOCAL) AS LOCAL,
                                    UPPER(EQUIPAMENTO) AS EQUIPAMENTO,
                                    UPPER(SOLICITANTE) AS SOLICITATE,
                                    DTSOLICITACAO2 AS 'DATA SOLICITAÇÃO', 
                                    OBS AS 'OBSERVAÇÕES' 
                                    FROM tblSolicitacaoServico 
                                    WHERE STATUS = @STATUS"
            Using cmd As New SqlCommand(query, conexaoSQL)
                cmd.Parameters.AddWithValue("@STATUS", "Aberta")

                Using adapter As New SqlDataAdapter(cmd)
                    adapter.Fill(dataTable)
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Erro ao consultar solicitações: " & ex.Message, "Erro")
        Finally

        End Try
        cmd.Dispose()
        Return dataTable
    End Function
End Class