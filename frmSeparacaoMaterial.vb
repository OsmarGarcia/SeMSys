Imports Microsoft.Reporting.WinForms
Imports Oracle.ManagedDataAccess.Client
Imports Oracle.ManagedDataAccess.Types

Public Class frmSeparacaoMaterial

    Private Sub btnPesquisar_Click(sender As Object, e As EventArgs) Handles btnPesquisar.Click
        Dim frm As New frmPesquisarOP(Me)
        frm.Show()
    End Sub

    Private Function DividirOP(numop As String, divisor As Double, qt As String) As DataTable

        Dim dt As New DataTable
        Dim dt2 As New DataTable


        dt2.Columns.Add("NUMOP")
        dt2.Columns.Add("CODPROD")
        dt2.Columns.Add("DESCRICAO")
        dt2.Columns.Add("NUMLOTE")
        dt2.Columns.Add("QT", GetType(Decimal))


        Try

            Dim qtd As Double = Convert.ToDecimal(qt)
            Dim qt_vezes As Double = Math.Floor(qtd / divisor)
            dt2.Rows.Clear()


            dt = PesquisarItensOP(numop)


            For x = 0 To dt.Rows.Count - 1
                dt2.Rows.Add(
                    numop,
                    dt.Rows(x)("CODPROD").ToString,
                    dt.Rows(x)("DESCRICAO").ToString,
                    dt.Rows(x)("NUMLOTE").ToString,
                    Math.Round((Convert.ToDecimal(dt.Rows(x)("QT")) * divisor) / qtd, 3)
                    )

            Next



            Return dt2

        Catch ex As Exception
            MessageBox.Show("Erro ao realizar divisão da OP." & vbCrLf & vbCrLf & ex.Message)
            Return Nothing
        End Try

    End Function
    Private Function PesquisarItensOP(numop As String) As DataTable
        Dim sql As String = "SELECT 
                            TO_CHAR(A.CODPROD) AS CODPROD,
                            B.DESCRICAO,
                            '1' AS NUMLOTE,
                            ROUND(A.QTNECESSIDADE,(SELECT NVL(NUMCASASDECESTOQUE,1) FROM PCCONSUM)) AS QTNECESSIDADE,
                            ROUND(C.CUSTOREAL,(SELECT NVL(NUMCASASDECESTOQUE,1) FROM PCCONSUM)) AS CUSTOREAL,
                            ROUND(C.CUSTOFIN,(SELECT NVL(NUMCASASDECESTOQUE,1) FROM PCCONSUM)) AS CUSTOFIN,
                            ROUND(C.CUSTOCONT,(SELECT NVL(NUMCASASDECESTOQUE,1) FROM PCCONSUM)) AS CUSTOCONT,
                            ROUND(C.VALORULTENT,(SELECT NVL(NUMCASASDECESTOQUE,1) FROM PCCONSUM)) AS VALORULTENT,
                            ROUND(C.CUSTOULTENT,(SELECT NVL(NUMCASASDECESTOQUE,1) FROM PCCONSUM)) AS CUSTOULTENT,
                            'N' AS ESTOQUEPORLOTE
                            FROM PCOPI A, PCPRODUT B , PCEST C
                            WHERE A.CODPROD = B.CODPROD
                            AND A.CODPROD = C.CODPROD
                            AND C.CODFILIAL = " & My.Settings.CodFilialEstoque & "
                            AND NVL(B.ESTOQUEPORLOTE,'N') = 'N'
                            AND A.NUMOP = " & numop & "

        UNION ALL

                            SELECT 
                            TO_CHAR(A.CODPROD) CODPROD,
                            B.DESCRICAO,
                            A.NUMLOTE,
                            ROUND(A.QT,(SELECT NVL(NUMCASASDECESTOQUE,1) FROM PCCONSUM)) AS QTNECESSIDADE,
                            ROUND(C.CUSTOREAL,(SELECT NVL(NUMCASASDECESTOQUE,1) FROM PCCONSUM)) AS CUSTOREAL,
                            ROUND(C.CUSTOFIN,(SELECT NVL(NUMCASASDECESTOQUE,1) FROM PCCONSUM)) AS CUSTOFIN,
                            ROUND(C.CUSTOCONT,(SELECT NVL(NUMCASASDECESTOQUE,1) FROM PCCONSUM)) AS CUSTOCONT,
                            ROUND(C.VALORULTENT,(SELECT NVL(NUMCASASDECESTOQUE,1) FROM PCCONSUM)) AS VALORULTENT,
                            ROUND(C.CUSTOULTENT,(SELECT NVL(NUMCASASDECESTOQUE,1) FROM PCCONSUM)) AS CUSTOULTENT,
                            'S' AS ESTOQUEPORLOTE
                            FROM PCOPILOTE A,PCPRODUT B , PCEST C
                            WHERE A.CODPROD = B.CODPROD 
                            AND A.CODPROD = C.CODPROD
                            AND C.CODFILIAL = " & My.Settings.CodFilialEstoque & "
                            AND A.NUMOP = " & numop & "
                            AND NVL(B.ESTOQUEPORLOTE,'N') = 'S'
                            "


        If conexao.State = 0 Then ConectaOra()
        Dim cmd As New OracleCommand

        Dim dt As New DataTable
        cmd.CommandType = CommandType.Text
        cmd.Parameters.Add(New OracleParameter("NUMOP", OracleDbType.Int32)).Value = Convert.ToInt32(numop)
        cmd.Parameters.Add(New OracleParameter("CODFILIAL", OracleDbType.Varchar2)).Value = My.Settings.CodFilialEstoque
        cmd.CommandText = sql
        cmd.Connection = conexao

        Dim da As New OracleDataAdapter(cmd)

        Try

            da.Fill(dt)

            Return dt

        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Return Nothing
        End Try

    End Function

    Private Function PesquisarCabecalhoOP(numop As Double) As DataTable
        Dim sql As String = "SELECT 
                                to_char(A.CODPRODMASTER) AS CODPROD,
                                B.DESCRICAO,'' AS NUMLOTE,
                                ROUND(A.QTPRODUZIR,2) AS QT,
                                CASE WHEN (SELECT DISTINCT(MODOPREPARO) FROM PCCOMPOSICAO WHERE CODPRODMASTER = A.CODPRODMASTER AND METODO = A.METODO AND CODFILIAL = A.CODFILIAL) IS NULL
                                THEN 'N/A'
                                ELSE (SELECT DISTINCT(MODOPREPARO) FROM PCCOMPOSICAO WHERE CODPRODMASTER = A.CODPRODMASTER AND METODO = A.METODO AND CODFILIAL = A.CODFILIAL) END KIT,
                                POSICAO
                                FROM PCOPC A, PCPRODUT B 
                                WHERE A.CODPRODMASTER = B.CODPROD
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

    Private Function PesquisarApontamentos(numop)

        Dim dt As New DataTable

        sql = "SELECT
                DTMOV,
                NVL(NUMSEQ,0) SEQ_APONTAMENTO,
                CODPROD,
                DESCRICAO,
                QT,
                CODFILIAL,
                CODOPER,
                NUMTRANSVENDA,
                (SELECT MATRICULA || ' - ' || NOME FROM PCEMPR WHERE MATRICULA = PCMOV.CODUSUR) AS FUNCIONARIO
                FROM PCMOV WHERE NUMOP = " & numop & "
                ORDER BY NUMTRANSVENDA"

        Try


            Using cmd As New OracleCommand(sql, conexao)

                cmd.CommandType = CommandType.Text

                Using dr As OracleDataReader = cmd.ExecuteReader()
                    dt.Clear()
                    dt.Load(dr)
                End Using
            End Using

            Return dt
        Catch ex As Exception
            MessageBox.Show("Erro ao pesquisar apontamentos da OP.")
            Return Nothing
        End Try

    End Function

    Private Sub txtNumOP_LostFocus(sender As Object, e As EventArgs) Handles txtNumOP.LostFocus


        If txtNumOP.Text = "" Then Exit Sub


        Dim dt As New DataTable
        Dim dtlancamentos As New DataTable

        dt = PesquisarCabecalhoOP(Convert.ToDecimal(txtNumOP.Text))



        If dt.Rows(0)("POSICAO").ToString() <> "P" Then
            MessageBox.Show("Ordem de producao inválida.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
            'Exit Sub
        End If

        If dt IsNot Nothing Then
            txtdescricao.Text = dt.Rows(0)("DESCRICAO").ToString
            txtQtd.Text = dt.Rows(0)("QT").ToString
        End If


        DataGridView1.DataSource = PesquisarItensOP(txtNumOP.Text)

        If dtlancamentos IsNot Nothing Then
            dgvApontamentos.DataSource = PesquisarApontamentos(Convert.ToDecimal(txtNumOP.Text))
        End If


    End Sub


    Private Function RequisitarInsumos(numop As Decimal, qt As Decimal) As DataTable

        If conexao.State = 0 Then ConectaOra()


        Dim dt As New DataTable
        Dim dtlote As New DataTable
        Dim dtclone As New DataTable
        Dim dtItensOP As New DataTable
        Dim QtProduzir As Decimal
        Dim SQL As String
        Dim numtransvenda As Integer
        Dim numtransitem As Integer
        Dim NUMSEQ As Integer


        Dim Oratransaction As OracleTransaction = conexao.BeginTransaction()
        'VALIDAR SE A OP ESTÁ INICIADA
        SQL = "SELECT POSICAO,QTPRODUZIR FROM PCOPC WHERE NUMOP = :NUMOP"

        Using cmd As New OracleCommand(SQL, conexao)


            cmd.Transaction = Oratransaction

            cmd.Parameters.Add(New OracleParameter(":NUMOP", OracleDbType.Varchar2)).Value = numop
            cmd.CommandType = CommandType.Text

            Using dr As OracleDataReader = cmd.ExecuteReader()
                dt.Clear()
                dt.Load(dr)
            End Using

            QtProduzir = Convert.ToDecimal(dt.Rows(0)("QTPRODUZIR"))

            If dt.Rows.Count > 0 AndAlso dt.Rows(0)("POSICAO").ToString() <> "P" Then
                MessageBox.Show("Ordem de Produção não está em posição para requisição de materiais." & vbCrLf & vbCrLf & "A operação será cancelada.", "Posição da OP", MessageBoxButtons.OK, MessageBoxIcon.Information)

                Oratransaction.Rollback()
                Return Nothing
            End If

            Oratransaction.Commit()

        End Using

        'CONSULTAR DISPONIBILIDADE DOS LOTES NO WINTHOR E WMS E GERAR EXCESSÃO CASO NÃO HAJAM LOTES

        SQL = "
            SELECT A.CODPROD,
                C.DESCRICAO,
                C.USAWMS,
                NVL(ROUND(A.QTNECESSIDADE,3),0) AS QTNECESSIDADE,
                NVL(ROUND(A.QTREQUISITADO,3),0) as QTREQUISITADO,
                NVL(ROUND(A.QTRESERVATUAL,3),0) as QTRESERVATUAL,
                NVL(ROUND(B.QTESTGER - B.QTBLOQUEADA,3),0) QT_DISP_WINTHOR,
                NVL((SELECT ROUND(SUM(nvl(QT,0)) - SUM(nvl(QTPENDSAIDA,0)),3) FROM PCESTENDERECO WHERE PCESTENDERECO.CODPROD = A.CODPROD),0) QT_DISP_WMS,
                NVL(B.CUSTOREAL,0) as CUSTOREAL,
                NVL(B.CUSTOCONT,0) as CUSTOCONT,
                NVL(B.CUSTOFIN,0) as CUSTOFIN,
                NVL(B.VALORULTENT,0) as VALORULTENT,
                NVL(B.CUSTOULTENT,0) as CUSTOULTENT 
                FROM PCOPI A,PCEST B,PCPRODUT C
                WHERE A.CODPROD = B.CODPROD
                AND A.CODPROD = C.CODPROD
                AND B.CODFILIAL = " & My.Settings.CodFilialEstoque & "
                AND A.NUMOP = " & numop


        Dim erroEstoque As Boolean = False

        Try
            Using command As New OracleCommand(SQL, conexao)
                Oratransaction = conexao.BeginTransaction()
                command.Transaction = Oratransaction
                command.Parameters.Clear()
                'cmd.Parameters.Add(New OracleParameter(":NUMOP", OracleDbType.Int32)).Value = numop
                'cmd.Parameters.Add(New OracleParameter(":CODFILIAL", OracleDbType.Varchar2)).Value = My.Settings.CodFilialProducao
                'command.Parameters.Add(New OracleParameter(":QTPROD", OracleDbType.Decimal)).Value = QtProduzir
                'command.Parameters.Add(New OracleParameter(":QTREQ", OracleDbType.Decimal)).Value = qt
                command.CommandType = CommandType.Text

                Using dr As OracleDataReader = command.ExecuteReader()
                    dt.Clear()
                    dt.Load(dr)
                End Using
            End Using

            dt.Columns("QTNECESSIDADE").ReadOnly = False


            For i = 0 To dt.Rows.Count - 1

                dt.Rows(i)("QTNECESSIDADE") = Math.Round(Convert.ToDecimal(dt.Rows(i)("QTNECESSIDADE")) * qt / QtProduzir, 3)

            Next



            Dim mensagem = "Os seguintes produtos não têm estoque suficiente para essa movimentação: " & vbCrLf & vbCrLf
            Dim msg = mensagem

            For i = 0 To dt.Rows.Count - 1
                If dt.Rows(i)("USAWMS") = "S" Then


                    If Convert.ToDecimal(dt.Rows(i)("QTNECESSIDADE")) > Convert.ToDecimal(dt.Rows(i)("QT_DISP_WINTHOR")) OrElse
                   Convert.ToDecimal(dt.Rows(i)("QTNECESSIDADE")) > Convert.ToDecimal(dt.Rows(i)("QT_DISP_WMS")) Then

                        msg &= $"{dt.Rows(i)("CODPROD")}: {dt.Rows(i)("DESCRICAO")}{vbCrLf}" &
                       $"Qtd Neces: {dt.Rows(i)("QTNECESSIDADE")}{vbCrLf}" &
                       $"Qtd Winthor: {dt.Rows(i)("QT_DISP_WINTHOR")}{vbCrLf}" &
                       $"Qtd WMS: {dt.Rows(i)("QT_DISP_WMS")}{vbCrLf}{vbCrLf}"

                        erroEstoque = True ' Marca que o erro ocorreu
                    End If

                End If
            Next

            If erroEstoque Then
                MessageBox.Show(msg, "Estoque Insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Throw New Exception("Estoque insuficiente para iniciar OP") ' Força o Catch para chamar o Rollback corretamente
            End If

            Oratransaction.Commit() ' Se não teve erro, confirma a transação

        Catch ex As Exception

            MessageBox.Show("Erro ao processar requisição: " & ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Oratransaction.Rollback()
            Return Nothing
        Finally
            Oratransaction.Dispose() ' Garante que a transação será descartada corretamente

        End Try




        '********* INICIA O PROCESSO DE REQUISIÇÃO APÓS AS VALIDAÇOES E PERMISSÕES ******************************'




        ' Inicia uma nova transação
        Oratransaction = conexao.BeginTransaction()


        Try

            'SELECIONA PROXNUMTRANSVENDA NA PCCONSUM

            SQL = "SELECT NVL(PROXNUMTRANSVENDA,1) AS PROXNUMTRANSVENDA FROM PCCONSUM"
            Using cmd As New OracleCommand(SQL, conexao)

                cmd.CommandType = CommandType.Text

                Dim result As Object = cmd.ExecuteScalar()

                If result IsNot DBNull.Value AndAlso result IsNot Nothing Then
                    numtransvenda = Convert.ToInt32(result)
                End If

            End Using

            SQL = "UPDATE PCCONSUM SET PROXNUMTRANSVENDA = NVL(PROXNUMTRANSVENDA,1) + 1 "
            Using cmd As New OracleCommand(SQL, conexao)

                cmd.CommandType = CommandType.Text

                cmd.ExecuteNonQuery()

            End Using

            SQL = "SELECT MAX(NVL(NUMSEQ,1))+1 AS NUMSEQ FROM PCMOV WHERE NUMOP = " & numop
            Using cmd As New OracleCommand(SQL, conexao)

                cmd.CommandType = CommandType.Text

                Dim result As Object = cmd.ExecuteScalar()

                If result IsNot DBNull.Value AndAlso result IsNot Nothing Then
                    NUMSEQ = Convert.ToInt32(result)
                Else
                    NUMSEQ = 1
                End If



            End Using



            SQL = "SELECT 
                            TO_CHAR(A.CODPROD) AS CODPROD,
                            B.DESCRICAO,
                            ROUND(NVL(A.QTNECESSIDADE,0),(SELECT NVL(NUMCASASDECESTOQUE,1) FROM PCCONSUM)) AS QTNECESSIDADE,
                            ROUND(NVL(C.CUSTOREAL,0),(SELECT NVL(NUMCASASDECESTOQUE,1) FROM PCCONSUM)) AS CUSTOREAL,
                            ROUND(NVL(C.CUSTOFIN,0),(SELECT NVL(NUMCASASDECESTOQUE,1) FROM PCCONSUM)) AS CUSTOFIN,
                            ROUND(NVL(C.CUSTOCONT,0),(SELECT NVL(NUMCASASDECESTOQUE,1) FROM PCCONSUM)) AS CUSTOCONT,
                            ROUND(NVL(C.VALORULTENT,0),(SELECT NVL(NUMCASASDECESTOQUE,1) FROM PCCONSUM)) AS VALORULTENT,
                            ROUND(NVL(C.CUSTOULTENT,0),(SELECT NVL(NUMCASASDECESTOQUE,1) FROM PCCONSUM)) AS CUSTOULTENT,
                            NVL(B.ESTOQUEPORLOTE,'N') AS ESTOQUEPORLOTE
                            FROM PCOPI A, PCPRODUT B , PCEST C
                            WHERE A.CODPROD = B.CODPROD
                            AND A.CODPROD = C.CODPROD
                            AND C.CODFILIAL = " & My.Settings.CodFilialEstoque & "
                            AND A.NUMOP = " & numop & "
                            ORDER BY NVL(B.ESTOQUEPORLOTE,'N')"


            Using cmd As New OracleCommand(SQL, conexao)

                cmd.CommandType = CommandType.Text

                Using dr As OracleDataReader = cmd.ExecuteReader()
                    dt.Clear()
                    dt.Load(dr)
                    dt.Columns.Add("FALTAREQUISITAR")
                End Using
            End Using



            For i = 0 To dt.Rows.Count - 1

                dt.Rows(i)("QTNECESSIDADE") = Math.Round(Convert.ToDecimal(dt.Rows(i)("QTNECESSIDADE")) * qt / QtProduzir, 3)
                dt.Rows(i)("FALTAREQUISITAR") = dt.Rows(i)("QTNECESSIDADE")


            Next

            Dim SQL_SELECT_NUMTRANSITEM As String
            Dim SQL_INSERT_PCMOV As String
            Dim dtLotes As New DataTable
            Dim qtdisponivelLote As Decimal
            Dim qtrequisitar As Decimal
            Dim ContagemItens As Integer
            Dim NUMLOTE As String
REVALIDAR:

            For i = 0 To dt.Rows.Count - 1    'Passando em todos os itens da OP

                If dt.Rows(i)("FALTAREQUISITAR") = 0 Then Continue For

                ' 1. Obter o próximo numtransitem
                SQL_SELECT_NUMTRANSITEM = "SELECT DFSEQ_PCMOVCOMPLE.NEXTVAL NUMTRANSITEM from dual"
                Using cmd As New OracleCommand(SQL_SELECT_NUMTRANSITEM, conexao)

                    cmd.CommandType = CommandType.Text

                    Dim result As Object = cmd.ExecuteScalar()
                    If result IsNot DBNull.Value AndAlso result IsNot Nothing Then
                        numtransitem = Convert.ToInt32(result)
                    End If
                End Using


                ' CASO USE CONTROLE POR LOTE, AVALIA O LOTE A SER REQUISITADO

                If dt.Rows(i)("ESTOQUEPORLOTE").ToString() = "S" Then

                    'RETORNAR LOTES QUE AINDA POSSUEM SALDO NA OP PRA REQUISITAR


                    SQL = "SELECT 
                                    TO_CHAR(A.CODPROD) CODPROD,
                                    B.DESCRICAO,
                                    NVL(A.NUMLOTE,'1') AS NUMLOTE,
                                    ROUND(A.QT,3) AS QTNECESSIDADE,
                                    ROUND(A.QTREQUISITADO,3) AS QTREQUISITADO,
                                    NVL(ROUND(C.QTESTGER - C.QTBLOQUEADA - C.QTRESERV,3),0) QT_DISP_WINTHOR,
                                    NVL((SELECT ROUND(SUM(nvl(QT,0)) - SUM(nvl(QTPENDSAIDA,0)),3) FROM PCESTENDERECO WHERE PCESTENDERECO.CODPROD = A.CODPROD),0) QT_DISP_WMS,
                                    ROUND(C.CUSTOREAL,3) AS CUSTOREAL,
                                    ROUND(C.CUSTOFIN,3) AS CUSTOFIN,
                                    ROUND(C.CUSTOCONT,3) AS CUSTOCONT,
                                    ROUND(C.VALORULTENT,3) AS VALORULTENT,
                                    ROUND(C.CUSTOULTENT,3) AS CUSTOULTENT,
                                    (SELECT NVL(DTVALIDADE,TO_DATE('01/01/1900','DD/MM/YYYY')) FROM PCLOTE WHERE PCLOTE.CODFILIAL = " & My.Settings.CodFilialEstoque & " AND CODPROD = A.CODPROD AND PCLOTE.NUMLOTE = A.NUMLOTE) AS DTVALIDADE
                                    FROM PCOPILOTE A,PCPRODUT B , PCEST C
                                    WHERE A.CODPROD = B.CODPROD 
                                    AND A.CODPROD = C.CODPROD
                                    AND C.CODFILIAL = " & My.Settings.CodFilialEstoque & "
                                    AND A.NUMOP = " & numop & "
                                    AND NVL(B.ESTOQUEPORLOTE,'N') = 'S'
                                    AND A.CODPROD = " & Convert.ToInt32(dt.Rows(i)("CODPROD").ToString()) & "
                                    AND QT > QTREQUISITADO
                                    ORDER BY (SELECT NVL(DTVALIDADE,TO_DATE('01/01/1900','DD/MM/YYYY')) FROM PCLOTE WHERE PCLOTE.CODFILIAL = " & My.Settings.CodFilialEstoque & " AND CODPROD = A.CODPROD AND PCLOTE.NUMLOTE = A.NUMLOTE), QTREQUISITADO DESC"

                    Using cmd As New OracleCommand(SQL, conexao)

                        cmd.CommandType = CommandType.Text

                        cmd.ExecuteNonQuery()

                        Using dr As OracleDataReader = cmd.ExecuteReader()
                            dtLotes.Clear()
                            dtLotes.Load(dr)
                        End Using

                    End Using

                    ' Caso não retorne nenhum registro, lança a excessão
                    If dtLotes Is Nothing Then
                        Throw New Exception("Erro na movimentação de estoque (Consulta PCOPILOTE): Produto usa controle de lotes e não há registros na tabela de lotes da OP ou todos os lotes já foram requisitados. " & vbCrLf & "Codprod: " & dt.Rows(i)("CODPROD").ToString() & " - " & dt.Rows(i)("DESCRICAO").ToString())
                    End If




                    qtdisponivelLote = Convert.ToDecimal(dtLotes.Rows(0)("QTNECESSIDADE")) - Convert.ToDecimal(dtLotes.Rows(0)("QTREQUISITADO"))

                    If Convert.ToDecimal(dt.Rows(i)("QTNECESSIDADE")) <= qtdisponivelLote Then

                        NUMLOTE = dtLotes.Rows(0)("NUMLOTE").ToString()
                        qtrequisitar = Convert.ToDecimal(dt.Rows(i)("QTNECESSIDADE"))
                        dt.Rows(i)("FALTAREQUISITAR") -= qtrequisitar

                    ElseIf Convert.ToDecimal(dt.Rows(i)("QTNECESSIDADE")) > qtdisponivelLote Then

                        NUMLOTE = dtLotes.Rows(0)("NUMLOTE").ToString()
                        qtrequisitar = qtdisponivelLote
                        dt.Rows(i)("FALTAREQUISITAR") -= qtrequisitar

                    End If




                Else
                    qtrequisitar = Convert.ToDecimal(dt.Rows(i)("QTNECESSIDADE"))
                    dt.Rows(i)("FALTAREQUISITAR") -= qtrequisitar
                    NUMLOTE = "1"

                End If


                ' 2. Inserir na PCMOV
                SQL_INSERT_PCMOV = "INSERT INTO PCMOV             
                (DTMOV, CODPROD, CODOPER, QT, PUNIT, CUSTOREAL, CUSTOFIN, CUSTOCONT, VALORULTENT, 
                 CUSTOULTENT, CODFILIAL, STATUS, NUMLOTE, NUMOP, CODFUNCLANC, CODFUNCREQ, 
                 NUMTRANSVENDA, CODUSUR, NUMTRANSITEM,NUMPED,NUMSEQ)
                VALUES
                (:DTMOV, :CODPROD, :CODOPER, :QT, :PUNIT, :CUSTOREAL, :CUSTOFIN, :CUSTOCONT, 
                 :VALORULTENT, :CUSTOULTENT, :CODFILIAL, :STATUS, :NUMLOTE, :NUMOP, :CODFUNCLANC,
                 :CODFUNCREQ, :NUMTRANSVENDA, :CODUSUR, :NUMTRANSITEM,:NUMPED,:NUMSEQ)"

                Using cmd As New OracleCommand(SQL_INSERT_PCMOV, conexao)

                    cmd.CommandType = CommandType.Text

                    ' Adicionar parâmetros (preencher com valores reais)
                    cmd.Parameters.Add(":DTMOV", OracleDbType.Date).Value = New DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
                    'cmd.Parameters.Add(":DTMOVLOG", OracleDbType.Date).Value = DateTime.Now
                    cmd.Parameters.Add(":CODPROD", OracleDbType.Varchar2).Value = dt.Rows(i)("CODPROD").ToString()
                    cmd.Parameters.Add(":CODOPER", OracleDbType.Varchar2).Value = "SP"
                    cmd.Parameters.Add(":QT", OracleDbType.Decimal).Value = qtrequisitar
                    cmd.Parameters.Add(":CUSTOREAL", OracleDbType.Decimal).Value = Convert.ToDecimal(dt.Rows(i)("CUSTOREAL"))
                    cmd.Parameters.Add(":PUNIT", OracleDbType.Decimal).Value = Convert.ToDecimal(dt.Rows(i)("CUSTOREAL"))
                    cmd.Parameters.Add(":CUSTOFIN", OracleDbType.Decimal).Value = Convert.ToDecimal(dt.Rows(i)("CUSTOFIN"))
                    cmd.Parameters.Add(":CUSTOCONT", OracleDbType.Decimal).Value = Convert.ToDecimal(dt.Rows(i)("CUSTOCONT"))
                    cmd.Parameters.Add(":VALORULTENT", OracleDbType.Decimal).Value = Convert.ToDecimal(dt.Rows(i)("VALORULTENT"))
                    cmd.Parameters.Add(":CUSTOULTENT", OracleDbType.Decimal).Value = Convert.ToDecimal(dt.Rows(i)("CUSTOULTENT"))
                    cmd.Parameters.Add(":CODFILIAL", OracleDbType.Varchar2).Value = My.Settings.CodFilialProducao
                    cmd.Parameters.Add(":STATUS", OracleDbType.Varchar2).Value = "AB"
                    cmd.Parameters.Add(":NUMLOTE", OracleDbType.Varchar2).Value = NUMLOTE
                    cmd.Parameters.Add(":NUMOP", OracleDbType.Varchar2).Value = numop
                    cmd.Parameters.Add(":CODFUNCLANC", OracleDbType.Varchar2).Value = My.Settings.UsuarioWinthor
                    cmd.Parameters.Add(":CODFUNCREQ", OracleDbType.Varchar2).Value = My.Settings.UsuarioWinthor
                    cmd.Parameters.Add(":NUMTRANSVENDA", OracleDbType.Int32).Value = numtransvenda
                    cmd.Parameters.Add(":CODUSUR", OracleDbType.Varchar2).Value = My.Settings.UsuarioWinthor
                    cmd.Parameters.Add(":NUMTRANSITEM", OracleDbType.Int32).Value = numtransitem
                    cmd.Parameters.Add(":NUMPED", OracleDbType.Int32).Value = numop

                    cmd.Parameters.Add(":NUMSEQ", OracleDbType.Int32).Value = NUMSEQ

                    cmd.ExecuteNonQuery()
                End Using


                Dim SQL_INSERT_PCMOVCOMPLE = "INSERT INTO PCMOVCOMPLE    
                                             ( NUMTRANSITEM        
                                             , DTREGISTRO          
                                             , CODAGREGACAO) 
                                        VALUES                     
                                             ( " & numtransitem & "       
                                             , SYSDATE             
                                             , '0')"

                Using cmd As New OracleCommand(SQL_INSERT_PCMOVCOMPLE, conexao)

                    cmd.CommandType = CommandType.Text
                    cmd.ExecuteNonQuery()

                End Using


            Next


            'exlcui da tabela temporaria os produtos totalmente requisitados
            For i As Integer = dt.Rows.Count - 1 To 0 Step -1
                If dt.Rows(i)("FALTAREQUISITAR").ToString() <= 0 Then
                    dt.Rows(i).Delete()
                End If
            Next
            dt.AcceptChanges()

            'valida se há mais algum item a requisitar
            ContagemItens = dt.Rows.Count
            If ContagemItens > 0 Then
                GoTo REVALIDAR
            End If


            ' 3. Chama a PKG para movimentação de estoque
            Dim SQL_EXECUTE_PKG As String = "BEGIN
                      :RETORNO := PKG_ESTOQUE.VENDAS_SAIDA(" & numtransvenda & ", 'N', :MSGRETORNO);
                    END;"

            Using cmd As New OracleCommand(SQL_EXECUTE_PKG, conexao)

                cmd.CommandType = CommandType.Text

                ' Entrada: Número da transação
                'cmd.Parameters.Add(":NUMTRANSVENDA", OracleDbType.Int32).Value = numtransvenda

                ' Saída: Retorno numérico da função
                Dim paramRetorno As New OracleParameter(":RETORNO", OracleDbType.Decimal)
                paramRetorno.Direction = ParameterDirection.Output
                cmd.Parameters.Add(paramRetorno)

                ' Saída: Mensagem de retorno
                Dim paramMsgRetorno As New OracleParameter(":MSGRETORNO", OracleDbType.Varchar2, 1000)
                paramMsgRetorno.Direction = ParameterDirection.Output
                cmd.Parameters.Add(paramMsgRetorno)

                ' 🚀 Executar o comando corretamente
                cmd.ExecuteNonQuery()

                ' Capturar valores de saída de forma segura
                Dim retorno As Integer = If(IsDBNull(paramRetorno.Value), -1, CType(paramRetorno.Value, OracleDecimal).ToInt32())

                Dim msgRetorno As String = If(paramMsgRetorno.Value IsNot Nothing, paramMsgRetorno.Value.ToString(), "Sem mensagem de retorno")

                ' Valida o retorno
                If retorno <= 0 OrElse msgRetorno <> "OK" Then
                    Throw New Exception("Erro na movimentação de estoque (Chamada da PKG_ESTOQUE): " & msgRetorno)
                End If
            End Using





            SQL = "SELECT 
                    CODPROD,
                    DESCRICAO,
                    QT,
                    NUMLOTE,
                    CODFILIAL,
                    NUMOP,
                    NUMTRANSVENDA
                    FROM PCMOV 
                    WHERE NUMTRANSVENDA = :NUMTRANSVENDA
                    "




            Using cmd As New OracleCommand(SQL, conexao)
                'cmd.Transaction = Oratransaction

                cmd.Parameters.Add(New OracleParameter(":NUMTRANSVENDA", OracleDbType.Int32)).Value = numtransvenda

                cmd.CommandType = CommandType.Text

                Using dr As OracleDataReader = cmd.ExecuteReader()
                    dt.Clear()
                    dt.Load(dr)
                End Using
            End Using





            For i = 0 To dt.Rows.Count - 1



                ' 4. Atualiza os dados de giro na PCEST
                SQL = "UPDATE PCEST                                                              
                      SET  
                         QTVENDMES = QTVENDMES + :QTVENDMES                                
                      ,  QTVENDDIA = QTVENDDIA + :QTVENDDIA                                 
                      ,  QTVENDSEMANA = QTVENDSEMANA + :QTVENDSEMANA                          
                      ,  DTULTSAIDA = TRUNC(SYSDATE)                                           
                      WHERE  CODPROD = :CODPROD                                       
                      AND    CODFILIAL = :CODFILIAL"

                Using cmd As New OracleCommand(SQL, conexao)
                    'cmd.Transaction = Oratransaction
                    cmd.CommandType = CommandType.Text
                    cmd.Parameters.Add(":QTVENDMES", OracleDbType.Decimal).Value = Convert.ToDecimal(dt.Rows(i)("QT"))
                    cmd.Parameters.Add(":QTVENDDIA", OracleDbType.Decimal).Value = Convert.ToDecimal(dt.Rows(i)("QT"))
                    cmd.Parameters.Add(":QTVENDSEMANA", OracleDbType.Decimal).Value = Convert.ToDecimal(dt.Rows(i)("QT"))
                    cmd.Parameters.Add(":CODPROD", OracleDbType.Varchar2).Value = dt.Rows(i)("CODPROD").ToString()
                    cmd.Parameters.Add(":CODFILIAL", OracleDbType.Varchar2).Value = dt.Rows(i)("CODFILIAL").ToString()
                    cmd.ExecuteNonQuery()
                End Using

                ' 5. Atualiza PCOPI
                SQL = "UPDATE PCOPI                                                              
                 SET QTRESERVATUAL = NVL(QTRESERVATUAL,0) - :QTRESERVATUAL              
                  ,QTREQUISITADO   = NVL(QTREQUISITADO,0) + :QTREQUISITADO              
                  WHERE  CODPROD = :CODPROD                                       
                  AND  NUMOP = :NUMOP"

                Using cmd As New OracleCommand(SQL, conexao)
                    cmd.Transaction = Oratransaction
                    cmd.CommandType = CommandType.Text
                    cmd.Parameters.Add(":QTRESERVATUAL", OracleDbType.Decimal).Value = Convert.ToDecimal(dt.Rows(i)("QT"))
                    cmd.Parameters.Add(":QTREQUISITADO", OracleDbType.Decimal).Value = Convert.ToDecimal(dt.Rows(i)("QT"))
                    cmd.Parameters.Add(":CODPROD", OracleDbType.Varchar2).Value = dt.Rows(i)("CODPROD").ToString()
                    cmd.Parameters.Add(":NUMOP", OracleDbType.Varchar2).Value = dt.Rows(i)("NUMOP").ToString()
                    cmd.ExecuteNonQuery()
                End Using

                ' 6. Atualiza PCOPILOTE
                SQL = "UPDATE PCOPILOTE                                              
                    SET QTREQUISITADO = NVL (QTREQUISITADO, 0) + :QT   
                   WHERE NUMOP = :NUMOP                                      
                        AND NUMLOTE = :NUMLOTE                                  
                        AND FRACAOUMIDA = 'A'                          
                        AND CODPROD = :CODPROD                                  
                        "

                Using cmd As New OracleCommand(SQL, conexao)
                    cmd.Transaction = Oratransaction
                    cmd.CommandType = CommandType.Text
                    cmd.Parameters.Add(":QT", OracleDbType.Decimal).Value = Convert.ToDecimal(dt.Rows(i)("QT"))
                    cmd.Parameters.Add(":NUMOP", OracleDbType.Varchar2).Value = dt.Rows(i)("NUMOP").ToString()
                    cmd.Parameters.Add(":NUMLOTE", OracleDbType.Varchar2).Value = dt.Rows(i)("NUMLOTE").ToString()
                    cmd.Parameters.Add(":CODPROD", OracleDbType.Varchar2).Value = dt.Rows(i)("CODPROD").ToString()
                    cmd.ExecuteNonQuery()
                End Using



            Next

            ' Commit da transação
            Oratransaction.Commit()


            MessageBox.Show("Requisição realizada com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Return dt

        Catch ex As Exception
            Oratransaction.Rollback()
            MessageBox.Show("Erro na transação: " & vbCrLf & vbCrLf & ex.Message & vbCrLf & "A requisição não foi efetuada.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return Nothing
        Finally
            If Oratransaction IsNot Nothing Then Oratransaction.Dispose()
        End Try



    End Function

    Private Function ApontarPA()

    End Function

    Private Sub btnGerarSeparacao_Click(sender As Object, e As EventArgs) Handles btnGerarSeparacao.Click

        If txtNumOP.Text = "" Or txtDivisor.Text = "" Then
            MessageBox.Show("Preencha todos os campos.")
            Exit Sub
        End If
        Dim dt As New DataTable

        dt = RequisitarInsumos(Convert.ToDecimal(txtNumOP.Text), Convert.ToDecimal(txtDivisor.Text))

        If dt Is Nothing Then Exit Sub

        'Dim res As DialogResult = MessageBox.Show("Deseja gerar a entrada do Produto Acabado?", "Gerar Requisição", MessageBoxButtons.YesNo)

        'If res = DialogResult.Yes Then
        'ApontarPA()
        'End If


        Dim resposta As DialogResult = MessageBox.Show("Deseja imprimir a requisição?", "Imprimir documento", MessageBoxButtons.YesNo)

        If resposta = DialogResult.No Then Exit Sub

        Dim dt2 As New DataTable


        dt2 = PesquisarCabecalhoOP(Convert.ToDecimal(txtNumOP.Text))




        Dim qt As String = Convert.ToString(Math.Ceiling(Convert.ToDecimal(txtQtd.Text) / Convert.ToDecimal(txtDivisor.Text)))

        If dt Is Nothing Then
            MessageBox.Show("Não foi possivel carregar os dados. O Relatório não será impresso.")
            Exit Sub
        End If


        Using frm = New frmRelRequsicaoOP(dt)

            Dim reportusuario As String = My.Settings.UsuarioWinthor.ToString & " - " & My.Settings.NomeWinthor.ToString


            frm.ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("DataSet1", dt))
            frm.ReportViewer1.SetDisplayMode(DisplayMode.PrintLayout)
            frm.ReportViewer1.ZoomMode = ZoomMode.PageWidth
            frm.NomeRelatorio = "Separação de Material por OP"
            frm.ReportViewer1.LocalReport.ReportEmbeddedResource = "SeMSys.RelRequisicaoOPDividida.rdlc"

            frm.ReportViewer1.LocalReport.SetParameters(New ReportParameter("parUsuario", reportusuario))
            frm.ReportViewer1.LocalReport.SetParameters(New ReportParameter("parNumOP", txtNumOP.Text))
            frm.ReportViewer1.LocalReport.SetParameters(New ReportParameter("parDescricao", txtdescricao.Text))
            frm.ReportViewer1.LocalReport.SetParameters(New ReportParameter("parQt", qt))
            frm.ReportViewer1.LocalReport.SetParameters(New ReportParameter("parVolume", txtDivisor.Text))
            frm.ReportViewer1.LocalReport.SetParameters(New ReportParameter("parModoPreparo", dt2.Rows(0)("KIT").ToString))

            frm.ShowDialog()
        End Using
    End Sub


End Class