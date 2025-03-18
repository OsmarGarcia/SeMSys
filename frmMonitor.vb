Imports System.Data.SqlClient
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class frmMonitor

    Private Sub frmMonitor_Load(sender As Object, e As EventArgs) Handles Me.Load

    End Sub

    Private Sub cboLinha_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboLinha.SelectedIndexChanged

        Dim linha$ = cboLinha.SelectedItem
        Dim sql$ = "s"
        Dim dt As New DataTable

        dt.Columns.Add("NUMOP")
        dt.Columns.Add("NUMLOTE")
        dt.Columns.Add("CODPROD")
        dt.Columns.Add("DESCRICAO")
        dt.Columns.Add("QTPRODUZIR")
        dt.Columns.Add("QTPRODUZIDA")




        If linha = "Linha 01" Then

            linha = "1"
        ElseIf linha = "Linha 02" Then
            linha = "2"

        ElseIf linha = "Linha 03" Then
            linha = "3"
        End If



        Dim numop$ = ConsultarOP(linha)


        sql = "SELECT 
                A.NUMOP,
                A.NUMLOTE,
                A.CODPRODMASTER CODPROD,
                B.DESCRICAO,
                A.QTPRODUZIR QTPROGRAMADA
                FROM PCOPC A, PCPRODUT B
                WHERE A.CODPRODMASTER = B.CODPROD
                AND A.NUMOP = " & numop

        cmd.Connection = conexao
        cmd.CommandText = sql
        cmd.CommandType = CommandType.Text
        Try
            datareader = cmd.ExecuteReader
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erro")
            Exit Sub
        End Try

        If datareader.FieldCount = 0 Then
            Exit Sub

        End If


        Do While datareader.Read()

            dt.Rows.Add(
                datareader("numop").ToString,
                datareader("numlote").ToString,
                datareader("codprod").ToString,
                datareader("descricao").ToString,
                datareader("qtprogramada").ToString)
        Loop


        dgvProducao.DataSource = dt
        dgvProducao.Refresh()



    End Sub



    Public Function ConsultarOP(linha$) As String
        sql = "select distinct numop from TBL_EMPRODUCAO where NUMOP IS NOT NULL and linha = " & linha

        Using cmd = conexaoSQL.CreateCommand

            cmd.CommandText = sql

            SQLdr = cmd.ExecuteReader

            Dim numop$ = "semOP"

            While SQLdr.Read

                numop = SQLdr(0).ToString()

            End While

            Return numop
        End Using


    End Function

    Private Sub btnIniciar_Click(sender As Object, e As EventArgs) Handles btnIniciar.Click
        Dim numop$ = txtNumOP.Text
        Dim linha$ = cboLinha.Text
        Dim sql$ = ""
        Dim R As DialogResult = MessageBox.Show("Realmente deseja inicar o monitoramento da OP: " & numop & " na " & linha & " ?" & vbCrLf & vbCrLf & "Confirmar?", "Iniciar OP", CType(vbYesNo, MessageBoxButtons))

        If R = DialogResult.No Then
            Exit Sub
        End If


        'VALIDA SE A ORDEM DE PRODUÇÃO EXISTE NO WINTHOR

        sql = "SELECT 
                CASE
                WHEN NUMOP IS NOT NULL THEN 'S'
                END NUMOP
                FROM PCOPC WHERE NUMOP = " & numop & " AND POSICAO NOT IN ('F','C')"

        cmd = conexao.CreateCommand
        cmd.CommandText = sql
        datareader = cmd.ExecuteReader
        datareader.Read()


        If datareader.HasRows = False Then
            MessageBox.Show("OP inválida, cancelada ou fechada no Winthor." + vbCrLf + vbCrLf + "OP não iniciada no monitoramento online.")
            Exit Sub
        End If



        'VALIDA SE A ORDEM DE PRODUÇÃO JÁ RODOU NO MONITORAMENTO

        'sql = "SELECT
        '        CASE WHEN NUMOP IS NOT NULL THEN 'S' 
        '        ELSE 'N' END STATUS 
        '        FROM TBL_REGISTRO_PROCESSO WHERE NUMOP = " + numop

        'Dim dr As SqlDataReader
        'SQLCmd = conexaoSQL.CreateCommand
        'SQLCmd.CommandTimeout = 240
        'SQLCmd.CommandText = sql
        'dr = SQLCmd.ExecuteReader
        'dr.Read()


        'If dr.HasRows = True Then
        '    Dim Result As MsgBoxResult = MessageBox.Show("OP já rodou anteriormente." + vbCrLf + vbCrLf + "Reiniciar indevidamente pode gerar erro de calculos na eficiência do turno." + vbCrLf + vbCrLf +
        '                    "Deseja realmente reiniciar esta OP?", "Iniciar OP", MessageBoxButtons.YesNo)
        '    If Result = MsgBoxResult.No Then

        '        If dr.IsClosed = False Then dr.Close()
        '        Exit Sub
        '    End If

        'End If

        'If dr.IsClosed = False Then dr.Close()





        'ATUALIZA ORDEM DE PRODUÇÃO NO SERVIDOR

        If linha = "Linha 01" Then
            linha = "1"
        ElseIf linha = "Linha 02" Then
            linha = "2"
        ElseIf linha = "Linha 03" Then
            linha = "3"
        End If


        sql = "UPDATE TBL_EMPRODUCAO SET NUMOP = " & numop & " WHERE LINHA = " & linha


        'Dim tras As SqlTransaction = conexaoSQL.BeginTransaction
        SQLCmd = conexaoSQL.CreateCommand
        SQLCmd.CommandText = sql
        'SQLCmd.Transaction = tras



        Try

            SQLCmd.ExecuteNonQuery()
            'tras.Commit()
            MessageBox.Show("OP iniciada no servidor de monitoramento." & vbCrLf & vbCrLf & "Apartir de agora todos os dados de monitoramento serão associados a OP: " & numop)

            'dgvProducao.ItemsSource = ConsultarOP(linha).defautView

        Catch ex As Exception

            MessageBox.Show("Erro ao iniciar OP no servidor." & vbCrLf & vbCrLf & ex.Message)
            'tras.Rollback()
        End Try

    End Sub

    Private Sub btnFinalizar_Click(sender As Object, e As EventArgs) Handles btnFinalizar.Click
        Dim numop$ = txtNumOP.Text
        Dim linha$ = cboLinha.Text
        Dim sql$ = ""
        Dim R As DialogResult = MessageBox.Show("Realmente deseja finalizar o monitoramento da OP: " & numop & " na " & linha & " ?" & vbCrLf & vbCrLf & "Finalizar?", "Finalizar OP", CType(vbYesNo, MessageBoxButtons))

        If R = DialogResult.No Then
            Exit Sub
        End If




        'ATUALIZA ORDEM DE PRODUÇÃO NO SERVIDOR

        If linha = "Linha 01" Then
            linha = "1"
        ElseIf linha = "Linha 02" Then
            linha = "2"
        ElseIf linha = "Linha 03" Then
            linha = "3"
        End If


        sql = "UPDATE TBL_EMPRODUCAO SET NUMOP = 0 WHERE LINHA = " & linha

        If conexaoSQL.State = ConnectionState.Closed Then
            conexaoSQL.Open()
        End If

        'Dim tras As SqlTransaction = conexaoSQL.BeginTransaction(IsolationLevel.ReadCommitted)
        SQLCmd = conexaoSQL.CreateCommand
        SQLCmd.CommandText = sql
        'SQLCmd.Transaction = tras



        Try

            SQLCmd.ExecuteNonQuery()
            'tras.Commit()
            MessageBox.Show("OP finalizada no servidor de monitoramento." & vbCrLf & vbCrLf & "Apartir de agora não será efetuado monitoramento da Linha 0" & linha)

        Catch ex As Exception

            MessageBox.Show("Erro ao finalizar OP no servidor." & vbCrLf & vbCrLf & ex.Message)
            'tras.Rollback()
        End Try

    End Sub
End Class