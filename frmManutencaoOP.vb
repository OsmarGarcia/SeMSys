Imports System.Configuration
Imports System.Transactions
Imports Oracle.ManagedDataAccess.Client



Public Class frmManutencaoOP

    Public codprodReserva As String
    Public txtdescricaoreserva As String

    Private Sub btnPesquisar_Click(sender As Object, e As EventArgs) Handles btnPesquisar.Click
        Dim frm As New frmPesquisarOP(Me)
        frm.Show()
    End Sub
    Private Function PesquisarCabecalhoOP(numop As Double) As DataTable
        Dim sql As String = "SELECT to_char(A.CODPRODMASTER) AS CODPROD,B.DESCRICAO,'' AS NUMLOTE,ROUND                               (A.QTPRODUZIR,2) AS QT, A.METODO
                            FROM PCOPC A, PCPRODUT B WHERE A.CODPRODMASTER = B.CODPROD
                            AND A.NUMOP = :NUMOP
                            "


        If conexao.State = 0 Then ConectaOra()
        Dim cmd As New OracleCommand
        Dim da As New OracleDataAdapter(cmd)
        Dim dt As New DataTable
        cmd.CommandType = CommandType.Text
        cmd.Parameters.Add(New OracleParameter("NUMOP", OracleDbType.Decimal)).Value = numop
        cmd.CommandText = sql
        cmd.Connection = conexao

        Try

            da.Fill(dt)

            Return dt

        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Return Nothing
        End Try
    End Function


    Private Sub txtNumOP_LostFocus(sender As Object, e As EventArgs) Handles txtNumOP.LostFocus, txtCodProd.LostFocus

        If txtNumOP.Text = "" Then
            MessageBox.Show("Preencha a OP desejada.")
            Exit Sub
        End If


        DataGridView1.DataSource = PesquisarItensOP(txtNumOP.Text)

        Dim dt As New DataTable

        dt = PesquisarCabecalhoOP(Convert.ToDecimal(txtNumOP.Text))

        If dt IsNot Nothing Then
            txtCodProd.Text = dt.Rows(0)("CODPROD").ToString
            txtdescricao.Text = dt.Rows(0)("DESCRICAO").ToString
            txtQtd.Text = dt.Rows(0)("QT").ToString
            txtMetodo.Text = dt.Rows(0)("METODO").ToString
        End If


    End Sub


    Private Function PesquisarItensOP(numop As String) As DataTable
        Dim sql As String = "SELECT to_char(A.CODPROD) AS CODPROD,B.DESCRICAO,'' AS NUMLOTE,ROUND                                           (A.QTNECESSIDADE,2) AS QT
                            FROM PCOPI A, PCPRODUT B WHERE A.CODPROD = B.CODPROD
                            AND A.NUMOP = :NUMOP
                            "


        If conexao.State = 0 Then ConectaOra()
        Dim cmd As New OracleCommand
        Dim da As New OracleDataAdapter(cmd)
        Dim dt As New DataTable
        cmd.CommandType = CommandType.Text
        cmd.Parameters.Add(New OracleParameter("NUMOP", OracleDbType.Varchar2)).Value = numop
        cmd.CommandText = sql
        cmd.Connection = conexao

        Try

            da.Fill(dt)

            Return dt

        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Return Nothing
        End Try

    End Function

    Private Sub btnGerarSeparacao_Click(sender As Object, e As EventArgs) Handles btnGerarSeparacao.Click

        If txtNumOP.Text = "" Or txtMetodo.Text = "" Then
            MessageBox.Show("Preencha todos os campos.")
            Exit Sub
        End If

        Try
            DataGridView1.DataSource = BuscarFormula(txtCodProd.Text, txtMetodo.Text, My.Settings.CodFilialProducao, txtQtd.Text)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try




    End Sub

    Public Function BuscarFormula(codprod As String, metodo As String, filial As String, qt As Double) As DataTable

        'RETORNA A FORMULAÇÃO DE UM PRODUTO ACABADO OU SEMIACABADO


        Dim dt2 As New DataTable ' Produtos filhos



        dt2.Columns.Add("codprod")
        dt2.Columns.Add("descricao")
        dt2.Columns.Add("qtnecessidade")
        dt2.Columns.Add("qtestoque")
        dt2.Columns.Add("tipomerc")
        dt2.Columns.Add("metodo")

        Dim sql As String = "
                                SELECT 
                                A.CODPROD,
                                B.DESCRICAO,
                                A.METODO,
                                ROUND(SUM(A.QT),6) QTNECESSIDADE,
                                SUM(C.QTESTGER - C.QTBLOQUEADA - C.QTRESERV) ESTOQUEDISP,
                                B.TIPOMERC
                                FROM PCCOMPOSICAO A, PCPRODUT B, PCEST C
                                WHERE A.CODPROD = B.CODPROD
                                AND A.CODPROD = C.CODPROD
                                AND A.codprodmaster = '" & codprod & "'
                                AND A.METODO = '" & metodo & "'
                                AND C.CODFILIAL = '" & filial & "'
                                AND A.CODFILIAL = '" & filial & "'
                                GROUP BY 
                                A.CODPROD,
                                B.DESCRICAO,
                                A.METODO,
                                B.TIPOMERC
                            "

        cmd.Connection = conexao
        cmd.CommandText = sql
        cmd.CommandType = CommandType.Text
        Try
            datareader = cmd.ExecuteReader

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erro")
            Exit Function
        End Try

        dt2.Rows.Clear()

        Do While datareader.Read

            dt2.Rows.Add(UCase(datareader(0).ToString),
                             UCase(datareader(1).ToString),
                             Math.Round(UCase(datareader(3).ToString) * qt, 3),
                             UCase(datareader(4).ToString),
                             UCase(datareader(5).ToString))

        Loop

        Return dt2


    End Function

    Private Sub RecalcularItensOP()


        Dim cmd = New OracleCommand
        cmd.CommandType = CommandType.Text
        cmd = conexao.CreateCommand
        Oratransaction = conexao.BeginTransaction()
        cmd.Transaction = Oratransaction

        Try

            Dim sql = "DELETE FROM PCOPI WHERE NUMOP = " & txtNumOP.Text


            cmd.CommandText = sql
            datareader = cmd.ExecuteReader
            datareader.Read()


            sql = "DELETE FROM PCCOMPOSICAOFRACAO WHERE NUMOP = " & txtNumOP.Text


            cmd.CommandText = sql
            datareader = cmd.ExecuteReader
            datareader.Read()


            Dim DT As New DataTable

            DT = BuscarFormula(txtCodProd.Text, txtMetodo.Text, My.Settings.CodFilialProducao, txtQtd.Text)

            Dim codMP As String
            Dim numseq As Integer = 1
            Dim qtnecessidade As Double
            Dim numop = txtNumOP.Text
            Dim codfunc = My.Settings.UsuarioWinthor
            Dim codprod = txtCodProd.Text

            For i = 0 To DT.Rows.Count - 1




                codMP = DT.Rows(i).Item(0).ToString
                qtnecessidade = Math.Round(Convert.ToDouble(DT.Rows(i).Item(2).ToString), 3)

                'Oratransaction = conexao.BeginTransaction()
                'cmd.Transaction = Oratransaction
                'INSERI OS ITENS DA OPS NAS DEVIDAS TABELAS
                'Try

                'peprogitens
                sql = "INSERT INTO PEPROGITENS
             ( NUMOP
             , CODPROD
             , NUMSEQ
             , QTNECESSIDADE
             , DTLANC
             , CODOPER
             , CODFUNCLANC)
        VALUES
             ( '" & numop & "'
             , '" & codMP & "'
             , '" & numseq & "'
             , '" & qtnecessidade & "'
             , SYSDATE
             , 'SP'
             , '" & codfunc & "')"
                cmd.CommandText = sql
                cmd.ExecuteNonQuery()


                'pcopi
                sql = "INSERT INTO PCOPI
                     ( NUMOP
                     , CODPROD
                     , QTNECESSIDADE
                     , QTREQUISITADO
                     , FRACAOUMIDA
                     , ACEITAREQACIMAPREV)
                VALUES
                     ( '" & numop & "'
                     , '" & codMP & "'
                     , '" & qtnecessidade & "'
                     , 0
                     , 'A'
                     , 'N')
                "
                cmd.CommandText = sql
                cmd.ExecuteNonQuery()

                'pccomposicaofracao
                sql = "INSERT INTO PCCOMPOSICAOFRACAO
                     ( NUMOP
                     , CODPROD
                     , CODPRODMASTER
                     , QTNECESSIDADE
                     , QTREQUISITADO
                     , ACEITAREQACIMAPREV
                     , NUMETAPA
                     , FRACAOUMIDA)
                VALUES 
                     ( '" & numop & "'
                     , '" & codMP & "'
                     ,  '" & codprod & "'
                     , '" & qtnecessidade & "'
                     , 0
                     , 'N'
                     , 0
                     , 'A')"

                cmd.CommandText = sql
                cmd.ExecuteNonQuery()
                'Oratransaction.Commit()
                numseq += 1
                '    Catch ex As Exception
                '    Oratransaction.Rollback()
                '    MessageBox.Show(ex.Message, "Erro ao gravar cabeçalho da OP")
                '    Exit Function
                '    Return False
                'End Try



            Next

            cmd.CommandText = "UPDATE PCOPC SET METODO = '" & txtMetodo.Text & "' WHERE NUMOP = '" & numop & "'"
            cmd.ExecuteNonQuery()

            Oratransaction.Commit()
            MessageBox.Show("OP recalculada com sucesso!")



        Catch ex As Exception
            Oratransaction.Rollback()
            MessageBox.Show(ex.Message, "Erro ao recalcular OP")
            Exit Sub
        End Try

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        RecalcularItensOP()
    End Sub

    Private Sub btnPesquisarOP_Click(sender As Object, e As EventArgs) Handles btnPesquisarOP.Click
        PesquisarOPsIniciar()
    End Sub

    Private Sub PesquisarOPsIniciar()


        Try

            Dim cmd As New OracleCommand
            Dim dr As OracleDataReader
            Dim dt As New DataTable

            cmd.Connection = conexao
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "SELECT
                            A.NUMOP,
                            A.NUMLOTE,
                            A.CODPRODMASTER,
                            B.DESCRICAO,
                            A.QTPRODUZIR,
                            A.DTPREVINICIO
                            FROM PCOPC A,PCPRODUT B
                            WHERE A.CODPRODMASTER = B.CODPROD
                            AND A.POSICAO = 'L'
                            AND NVL(A.QTPRODUZIDA, 0) <= NVL(A.QTPRODUZIR, 0)
                            AND A.CODFILIAL = " & My.Settings.CodFilialProducao &
                            "AND DTPREVINICIO BETWEEN TO_DATE(:DTINICIO,'DD/MM/YYYY') AND TO_DATE(:DTFIM,'DD/MM/YYYY')
                            ORDER BY A.NUMOP"


            cmd.Parameters.Add(New OracleParameter("DTINICIO", OracleDbType.Varchar2)).Value = DateTimePicker1.Text
            cmd.Parameters.Add(New OracleParameter("DTFIM", OracleDbType.Varchar2)).Value = DateTimePicker2.Text
            dr = cmd.ExecuteReader


            dt.Load(dr)
            DataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            DataGridView2.DataSource = dt

            'dt.Clear()
            'COLORIR OS ITENS COM INSUMOS FALTANTES
            Dim dt2 As New DataTable

            For i = 0 To DataGridView2.Rows.Count - 1




                Dim SQL As String = "
                SELECT A.CODPROD, 
                A.QTNECESSIDADE, 
                (SELECT PKG_ESTOQUE.ESTOQUE_DISPONIVEL(A.CODPROD, B.CODFILIAL, 'V') FROM DUAL) AS ESTOQUE, 
                (SELECT TIPOMERC FROM PCPRODUT WHERE PCPRODUT.CODPROD = A.CODPROD) TIPOMERC, 
                (SELECT NVL(PCPRODUT.ESTOQUEPORLOTE, 'N')  FROM PCPRODUT WHERE CODPROD = A.CODPROD) ESTOQUEPORLOTE
                FROM PCOPI A, PCOPC B WHERE A.NUMOP = B.NUMOP AND A.NUMOP = '" & DataGridView2.Rows(i).Cells(0).Value & "'"

                cmd.CommandText = SQL
                dr = cmd.ExecuteReader()
                dt2.Load(dr)

                DataGridView2.Rows(i).DefaultCellStyle.ForeColor = Color.Black

                For x = 0 To dt2.Rows.Count - 1
                    Dim qtnecessidade = Math.Round(Convert.ToDecimal(dt2.Rows(x)("QTNECESSIDADE")), 2)
                    Dim qtestoque = Math.Round(Convert.ToDecimal(dt2.Rows(x)("ESTOQUE")), 2)
                    Dim tipomerc = dt2.Rows(x)("TIPOMERC").ToString()

                    If qtnecessidade > qtestoque AndAlso tipomerc <> "SA" Then
                        DataGridView2.Rows(i).DefaultCellStyle.ForeColor = Color.Red
                        Exit For
                    End If
                Next


            Next

        Catch ex As Exception
            MessageBox.Show("Erro ao carregar OP" & vbCrLf & "Erro: " & ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub btnIniciar_Click(sender As Object, e As EventArgs) Handles btnIniciar.Click


        If DataGridView2.SelectedRows.Count = 0 Then
            MessageBox.Show("Selecione uma OP na grid para continuar.")
            Exit Sub
        End If


        Dim selectedRow As DataGridViewRow = DataGridView2.SelectedRows(0)

        Dim numop As String = selectedRow.Cells(0).Value.ToString()

        For i = 0 To DataGridView2.SelectedRows.Count - 1

            selectedRow = DataGridView2.SelectedRows(i)
            numop = selectedRow.Cells(0).Value.ToString()
            IniciarOP(numop)

        Next



        PesquisarOPsIniciar()


    End Sub

    Private Sub IniciarOP(numop As String)

        If conexao.State = 0 Then ConectaOra()

        Dim dr As OracleDataReader
        Dim dt As New DataTable
        Dim cmd As New OracleCommand

        'Dim tras As OracleTransaction = conexao.BeginTransaction(IsolationLevel.ReadCommitted)

        Dim dtlote As New DataTable
        Dim qtdisponivel As Decimal
        Dim dtclone As New DataTable


        Oratransaction = conexao.BeginTransaction()
        cmd.Transaction = Oratransaction
        cmd.Connection = conexao
        'cmd = conexao.CreateCommand



        Try

            ' VALIDA SE TODOS OS INSUMOS TEM ESTOQUE DISPONÍVEL
            cmd.CommandType = CommandType.Text


            Dim SQL As String = "
                SELECT A.CODPROD, 
                A.QTNECESSIDADE, 
                (SELECT PKG_ESTOQUE.ESTOQUE_DISPONIVEL(A.CODPROD, B.CODFILIAL, 'V') FROM DUAL) AS ESTOQUE, 
                (SELECT TIPOMERC FROM PCPRODUT WHERE PCPRODUT.CODPROD = A.CODPROD) TIPOMERC, 
                (SELECT NVL(PCPRODUT.ESTOQUEPORLOTE, 'N')  FROM PCPRODUT WHERE CODPROD = A.CODPROD) ESTOQUEPORLOTE
                FROM PCOPI A, PCOPC B WHERE A.NUMOP = B.NUMOP AND A.NUMOP = " & numop

            cmd.CommandText = SQL
            dr = cmd.ExecuteReader()
            dt.Load(dr)

            Dim msg As String = "Insumos a seguir sem estoque disponível para iniciar o processo:" & vbCrLf & vbCrLf

            For i = 0 To dt.Rows.Count - 1
                Dim qtnecessidade = Math.Round(Convert.ToDecimal(dt.Rows(i)("QTNECESSIDADE")), 6)
                Dim qtestoque = Math.Round(Convert.ToDecimal(dt.Rows(i)("ESTOQUE")), 6)
                Dim tipomerc = dt.Rows(i)("TIPOMERC").ToString()

                If qtnecessidade > qtestoque AndAlso tipomerc <> "SA" Then
                    msg &= "CODPROD: " & dt.Rows(i)("CODPROD").ToString() & vbCrLf & "QTNECES: " & qtnecessidade & vbCrLf & "QT ESTOQUE: " & qtestoque & vbCrLf & vbCrLf
                End If
            Next

            If msg <> "Insumos a seguir sem estoque disponível para iniciar o processo:" & vbCrLf & vbCrLf Then
                MessageBox.Show("Erro ao iniciar OP." & vbCrLf & msg, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Oratransaction.Rollback()
                Exit Sub
            End If


            'INICIA O PROCESSO DE GRAVAÇÃO DO INICIO DA OP
            For i = 0 To dt.Rows.Count - 1
                Dim qtnecessidade = Math.Round(Convert.ToDecimal(dt.Rows(i)("QTNECESSIDADE")), 3)
                Dim qtestoque = Math.Round(Convert.ToDecimal(dt.Rows(i)("ESTOQUE")), 3)
                Dim codprod = dt.Rows(i)("CODPROD").ToString()

                If qtnecessidade <= qtestoque Then


                    'UPDATE PCOPI PRA RESERVA
                    cmd.CommandText = "UPDATE PCOPI SET QTRESERVALTERAR = QTNECESSIDADE WHERE NUMOP = " & numop & " AND CODPROD = " & codprod & " AND NOT QTNECESSIDADE < 0"
                    cmd.ExecuteNonQuery()


                    cmd.CommandText = "UPDATE PCOPI SET RESERVALIBERADA = 'N' WHERE NUMOP = " & numop
                    cmd.ExecuteNonQuery()




                    'SE O PRODUTO CONTROLAR LOTE, FAZ A OPERAÇÃO DE GRAVAR OS LOTES DA VEZ NA PCOPILOTE
                    If dt.Rows(i)("ESTOQUEPORLOTE").ToString() = "S" Then

                        'PEGA OS LOTES DA VEZ
                        cmd.CommandText = "SELECT PCLOTE.CODPROD                        
                                          , PCPRODUT.DESCRICAO                                                                                                      
                                         , ROUND((NVL(PCLOTE.QT, 0) - NVL(PCLOTE.QTBLOQUEADA, 0) - NVL(PCLOTE.QTRESERV, 0) - NVL(PCLOTE.QTTEMPINDUSTRIA, 0)),6) QTDISPONIVEL  
                                         , nvl(QTTEMPINDUSTRIA,0) QTTEMPINDUSTRIA                                                                                                       
                                         , DTVALIDADE                                                                                                              
                                         , PCLOTE.NUMLOTE                                                                                                          
                                      FROM PCLOTE                                                                                                                  
                                         , PCPRODUT                                                                                                                
                                     WHERE PCLOTE.CODPROD = PCPRODUT.CODPROD                                                                                       
                                       AND PCLOTE.CODFILIAL = " & My.Settings.CodFilialProducao & "                                                                                          
                                       AND PCLOTE.CODPROD = " & codprod & "                                                                                               
                                       AND PCLOTE.DTEXCLUSAO IS NULL                                                                                               
                                       AND (NVL(PCLOTE.QT, 0) - NVL(PCLOTE.QTBLOQUEADA, 0) - NVL(PCLOTE.QTRESERV, 0)) > 0                                          
                                       AND (NVL(PCLOTE.QT, 0) - NVL(PCLOTE.QTBLOQUEADA, 0) - NVL(PCLOTE.QTRESERV, 0)) > NVL(PCLOTE.QTTEMPINDUSTRIA, 0)             
                                       ORDER BY DTVALIDADE ASC "

                        dr = cmd.ExecuteReader
                        dtclone.Clear()
                        dtclone.Load(dr)

                        dtlote = dtclone.Clone()

                        ' Tornar todas as colunas editáveis no clone
                        For Each col As DataColumn In dtlote.Columns
                            col.ReadOnly = False
                        Next

                        ' Importar todas as linhas da tabela original para a clonada
                        For Each row As DataRow In dtclone.Rows
                            dtlote.ImportRow(row)
                        Next



                        For x = 0 To dtlote.Rows.Count - 1
                            qtdisponivel = Convert.ToDecimal(dtlote.Rows(x)("QTDISPONIVEL"))
                            If qtdisponivel >= qtnecessidade Then
                                dtlote.Rows(x)("QTTEMPINDUSTRIA") = qtnecessidade
                                Exit For
                            Else
                                dtlote.Rows(x)("QTTEMPINDUSTRIA") = dtlote.Rows(x)("QTDISPONIVEL")
                                qtnecessidade = Math.Round(qtnecessidade - Convert.ToDecimal(dtlote.Rows(x)("QTDISPONIVEL")), 3)
                            End If

                        Next

                        For x = 0 To dtlote.Rows.Count - 1
                            If Convert.ToDecimal(dtlote.Rows(x)("QTTEMPINDUSTRIA")) > 0 Then

                                cmd.CommandText = "UPDATE PCLOTE                            
                                                      SET QTTEMPINDUSTRIA = '" & Convert.ToDecimal(dtlote.Rows(x)("QTTEMPINDUSTRIA")) & "'     
                                                    WHERE CODPROD = '" & codprod & "'                 
                                                      AND NUMLOTE = '" & dtlote.Rows(x)("NUMLOTE") & "'
                                                    "
                                cmd.ExecuteNonQuery()


                                cmd.CommandText = "INSERT INTO PCOPILOTE          
                                                      ( CODPROD              
                                                      , NUMLOTEORI           
                                                      , NUMLOTE              
                                                      , QT                   
                                                      , QTREQUISITADO        
                                                      , NUMOP                
                                                      , NUMSEQ               
                                                      , DTVALIDADE           
                                                      , FRACAOUMIDA    )     
                                 
                                              VALUES                         
                                 
                                                     (  '" & codprod & "'             
                                                      , '" & dtlote.Rows(x)("NUMLOTE") & "'          
                                                      , '" & dtlote.Rows(x)("NUMLOTE") & "'           
                                                      , '" & dtlote.Rows(x)("QTTEMPINDUSTRIA") & "'               
                                                      , 0       
                                                      , '" & numop & "'               
                                                      , '1'              
                                                      , '" & Convert.ToDateTime(dtlote.Rows(x)("DTVALIDADE")) & "'          
                                                      , 'A' )
                                            "

                                cmd.ExecuteNonQuery()


                                cmd.CommandText = "UPDATE PCLOTE                    
                                                     SET QTTEMPINDUSTRIA = 0           
                                                    WHERE NUMLOTE = '" & dtlote.Rows(x)("NUMLOTE") & "' 
                                                    AND CODPROD = '" & codprod & "'"
                                cmd.ExecuteNonQuery()
                            End If
                        Next
                    End If

                    'FAZ A RESERVA EFETIVAMENTE USANDO PKG_ESTOQUE ATUAL DO BANCO DE DADOS
                    cmd.CommandText = "DECLARE " &
                              "  vRETORNO VARCHAR2(1); " &
                              "  psMSG_RETORNO VARCHAR2(32767); " &
                              "BEGIN " &
                              "  vRETORNO := PKG_ESTOQUE.RESERVA_INCLUIR(" &
                              "    :pnIDENTIFICADOR, " &
                              "    :pnCODPROD, " &
                              "    :pnNUMSEQ, " &
                              "    SYS.DIUTIL.INT_TO_BOOL(:pbPEDIDO), " &
                              "    :psOPERACAO, " &
                              "    psMSG_RETORNO => psMSG_RETORNO " &
                              "  ); " &
                              "  :P_RETORNO := vRETORNO; " &
                              "  :psMSG_RETORNO := psMSG_RETORNO; " &
                              "END;"

                    cmd.Parameters.Clear()
                    cmd.Parameters.Add(New OracleParameter("pnIDENTIFICADOR", OracleDbType.Varchar2)).Value = numop
                    cmd.Parameters.Add(New OracleParameter("pnCODPROD", OracleDbType.Varchar2)).Value = codprod
                    cmd.Parameters.Add(New OracleParameter("pnNUMSEQ", OracleDbType.Varchar2)).Value = "1"
                    cmd.Parameters.Add(New OracleParameter("pbPEDIDO", OracleDbType.Int32)).Value = 0
                    cmd.Parameters.Add(New OracleParameter("psOPERACAO", OracleDbType.Varchar2)).Value = "II"
                    cmd.Parameters.Add(New OracleParameter("P_RETORNO", OracleDbType.Varchar2, 1)).Direction = ParameterDirection.Output
                    cmd.Parameters.Add(New OracleParameter("psMSG_RETORNO", OracleDbType.Varchar2, 32767)).Direction = ParameterDirection.InputOutput

                    cmd.ExecuteNonQuery()

                    Dim pRetorno As String = cmd.Parameters("P_RETORNO").Value.ToString()
                    Dim msgRetorno As String = cmd.Parameters("psMSG_RETORNO").Value.ToString()

                    If msgRetorno <> "OK" Then
                        MessageBox.Show("Erro ao gerar reserva." & vbCrLf & vbCrLf & msgRetorno, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Oratransaction.Rollback()
                        Exit Sub
                    End If




                End If

            Next

            'ATUALIZA POSICAO DA OP NA PCOPC
            cmd.CommandText = "UPDATE PCOPC SET POSICAO = 'P', DTINICIO = TRUNC(SYSDATE), CODFUNCINICIO = " & My.Settings.UsuarioWinthor & " WHERE NUMOP = " & numop
            cmd.ExecuteNonQuery()

            'ATUALIZA BAIXA NA PCOPI
            cmd.CommandText = "UPDATE PCOPI SET BAIXAVIRTUAL = 'N' WHERE NUMOP = " & numop
            cmd.ExecuteNonQuery()


            Oratransaction.Commit()

            MessageBox.Show("Ordem de produção iniciada com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("Erro ao iniciar OP." & vbCrLf & "Erro: " & ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Oratransaction.Rollback()

            Exit Sub
        End Try


    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim frm As New frmPesquisar(Me, txtCodProdReserva)
        frm.ShowDialog()

        txtCodProdReserva.Text = codprodReserva
        txtDescReserva.Text = txtdescricaoreserva
    End Sub



    Public Sub CapturarDadosProduto() Handles txtCodProdReserva.LostFocus
        Dim cod As String = txtCodProdReserva.Text

        If cod = "" Then
            Exit Sub
        End If

        sql = "select 
                descricao
                from pcprodut where codprod = " & cod

        cmd.Connection = conexao
        cmd.CommandText = sql
        cmd.CommandType = CommandType.Text
        Try
            datareader = cmd.ExecuteReader
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erro")
            Exit Sub
        End Try

        Do While datareader.Read
            txtDescReserva.Text = UCase(datareader("descricao").ToString)

        Loop
    End Sub

    Private Sub btnRecalcularReserva_Click(sender As Object, e As EventArgs) Handles btnRecalcularReserva.Click
        RecalcularReserva(txtCodProdReserva.Text, cboFilial.Text)
    End Sub

    Private Sub frmManutencaoOP_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Dim cod As String = txtCodProdReserva.Text



        sql = "SELECT DISTINCT CODIGO FROM PCFILIAL ORDER BY CODIGO"

        cmd.Connection = conexao
        cmd.CommandText = sql
        cmd.CommandType = CommandType.Text
        Try

            datareader = cmd.ExecuteReader

            Do While datareader.Read
                cboFilial.Items.Add(UCase(datareader("CODIGO").ToString))
            Loop


        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erro")
            Exit Sub
        End Try


    End Sub

    Private Sub TabControl1_DrawItem(sender As Object, e As DrawItemEventArgs) Handles TabControl1.DrawItem
        Dim g As Graphics = e.Graphics
        Dim tabPage As TabPage = TabControl1.TabPages(e.Index)
        Dim tabBounds As Rectangle = TabControl1.GetTabRect(e.Index)

        If e.State = DrawItemState.Selected Then
            g.FillRectangle(Brushes.LightBlue, tabBounds)
        Else
            g.FillRectangle(Brushes.SlateGray, tabBounds)
        End If

        Dim tabText As String = tabPage.Text
        Dim sf As New StringFormat()
        sf.Alignment = StringAlignment.Center
        sf.LineAlignment = StringAlignment.Center
        g.DrawString(tabText, TabControl1.Font, Brushes.Black, tabBounds, sf)
    End Sub
End Class