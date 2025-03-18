Public Class frmSelecionarParada

    Sub New(cod$)

        InitializeComponent()
        Me.lblCod.Text = cod
    End Sub
    Private Sub frmSelecionarParada_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ConectaSRVSQL()

        Dim cmd As New SqlClient.SqlCommand("SELECT Codigo,CODID FROM GERAL WHERE Identificador = 'AREA'", conexaoSQL)
        Dim da As New SqlClient.SqlDataAdapter(cmd)
        Dim dt As New DataTable
        da.Fill(dt)

        cboArea.DataSource = dt
        cboArea.DisplayMember = "CODID"
        cboArea.ValueMember = "Codigo"
        cboArea.Text = "Selecione a área"

        Dim cmd1 As New SqlClient.SqlCommand("SELECT Codigo,CODID FROM GERAL WHERE Identificador = 'PARADA'", conexaoSQL)
        Dim da1 As New SqlClient.SqlDataAdapter(cmd1)
        Dim dt1 As New DataTable
        da1.Fill(dt1)

        cboTipoFalha.DataSource = dt1
        cboTipoFalha.DisplayMember = "CODID"
        cboTipoFalha.ValueMember = "Codigo"
        cboTipoFalha.Text = "Selecione a falha"


    End Sub


    Private Sub cboEquipamento_TextChanged(sender As Object, e As EventArgs) Handles cboEquipamento.TextChanged


        Dim cmd As New SqlClient.SqlCommand("SELECT Codigo,CODID FROM GERAL WHERE Identificador = 'COMPONENTE' AND IDTIPO2 = @EQUIPAMENTO", conexaoSQL)
        cmd.Parameters.AddWithValue("EQUIPAMENTO", cboEquipamento.Text)
        Dim da As New SqlClient.SqlDataAdapter(cmd)
        Dim dt As New DataTable
        da.Fill(dt)

        cboComponente.DataSource = dt
        cboComponente.DisplayMember = "CODID"
        cboComponente.ValueMember = "Codigo"
        cboComponente.Text = "Selecione o componente"

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim cmd As New SqlClient.SqlCommand("UPDATE BDADOS_REGISTROS SET AREA = @AREA, EQUIPAMENTO = @EQUIPAMENTO, COMPONENTE = @COMPONENTE, TIPODEFALHA = @TIPODEFALHA, MOTIVO = @MOTIVO WHERE CODIGO = @CODIGO", conexaoSQL)
        cmd.Parameters.AddWithValue("AREA", cboArea.Text)
        cmd.Parameters.AddWithValue("EQUIPAMENTO", cboEquipamento.Text)
        cmd.Parameters.AddWithValue("COMPONENTE", cboComponente.Text)
        cmd.Parameters.AddWithValue("TIPODEFALHA", cboTipoFalha.Text)
        cmd.Parameters.AddWithValue("MOTIVO", ttxMotivo.Text)
        cmd.Parameters.AddWithValue("CODIGO", lblCod.Text)



        Try
            cmd.ExecuteNonQuery()
            MessageBox.Show("Alteração realizada com sucesso.")
            Me.Hide()

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try




    End Sub


    Private Sub cboArea_TextChanged(sender As Object, e As EventArgs) Handles cboArea.TextChanged

        Dim cmd As New SqlClient.SqlCommand("SELECT Codigo,CODID FROM GERAL WHERE Identificador = 'EQUIPAMENTO' AND IDTIPO2 = @AREA", conexaoSQL)
        cmd.Parameters.AddWithValue("AREA", cboArea.Text)
        Dim da As New SqlClient.SqlDataAdapter(cmd)
        Dim dt As New DataTable
        da.Fill(dt)

        cboEquipamento.DataSource = dt
        cboEquipamento.DisplayMember = "CODID"
        cboEquipamento.ValueMember = "Codigo"
        cboEquipamento.Text = "Selecione o equipamento"


    End Sub
End Class