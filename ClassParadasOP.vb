Imports System.Data.SqlClient
Imports System.Linq.Expressions
Imports System.Text
Imports Microsoft.Reporting.WinForms
Imports Oracle.ManagedDataAccess.Client

Public Class ClassParadasOP


    Private Property vCodProd As String
    Private Property vDescricao As String
    Private Property vNumOP As String
    Private Property vQtProg As Double
    Private Property vQtProdUnit As Double
    Private Property vQtProdMaster As Double
    Private Property vAtendimento As Double
    Private Property vEficiencia As Double
    Private Property vProdutividade As Double
    Private Property vTempoTotal As Double
    Private Property vHorasParadas As Double
    Private Property vHorasVariacao As Double
    Private Property vHorasProdutivas As Double



    Public Sub ResumirParadasOP(numop)


        Dim numopArray() As String = numop.Split(",")

        Dim inClause As New StringBuilder()
        inClause.Append("X.OP IN (")

        For i As Integer = 0 To numopArray.Length - 1
            inClause.Append("@numop" & i)
            If i < numopArray.Length - 1 Then
                inClause.Append(",")
            End If
        Next

        inClause.Append(")")


        Dim sql = "SELECT
                            X.OP,
                            ROUND(SUM(X.TEMPOTOTAL), 2) TEMPOTOTAL,
                            ROUND(SUM(X.HORASPRODUTIVAS), 2) HORASPRODUTIVAS,
                            (SELECT ROUND(SUM(TEMPOTOTAL), 2) FROM BDADOS_REGISTROS WHERE OP = X.OP AND TIPODEFALHA NOT IN (
                                'HORA PRODUTIVA RODANDO ABAIXO DA NOMINAL',
                                'HORAPRODUTIVA',
                                'VARIACAO DE RITMO'
                            )) HORASPARADAS,
                            (SELECT ROUND(SUM(TEMPOTOTAL), 2) FROM BDADOS_REGISTROS WHERE OP = X.OP AND TIPODEFALHA = 'VARIACAO DE RITMO') VARIACAODERITMO,
                            (SELECT MAX(VALORMEDIDO) - MIN(VALORMEDIDO) FROM TBL_REGISTRO_PROCESSO WHERE IDSENSOR = '5' AND NUMOP = X.OP) PRODUNIT,
                            (SELECT AVG(QTUNITCX) FROM TBL_REGISTRO_PROCESSO WHERE IDSENSOR = '5' AND NUMOP = X.OP) QTUNITCX,
                            (SELECT DISTINCT IDPROCESSO FROM TBL_REGISTRO_PROCESSO WHERE IDSENSOR = '5' AND NUMOP = X.OP) LINHA,
                            (SELECT FORMAT(MIN(DATACRIACAO), 'dd/MM/yyyy HH:mm:ss') FROM TBL_REGISTRO_PROCESSO WHERE NUMOP = X.OP) DATAINICIAL,
                            (SELECT FORMAT(MAX(DATACRIACAO), 'dd/MM/yyyy HH:mm:ss') FROM TBL_REGISTRO_PROCESSO WHERE NUMOP = X.OP) DATAFINAL
                        FROM BDADOS_REGISTROS X
                        WHERE " & inClause.ToString() & "
                        GROUP BY X.OP"



        Dim cmd As New SqlCommand
        Dim dr As SqlDataReader
        'Dim conexaoSQL As New SqlConnection

        If mdlConexaoOracle.conexaoSQL.State = 0 Then ConectaSRVSQL()

        cmd.Connection = conexaoSQL
        cmd.CommandText = sql
        cmd.CommandType = CommandType.Text

        For i As Integer = 0 To numopArray.Length - 1
            cmd.Parameters.Add(New SqlParameter("@NUMOP" & i, SqlDbType.VarChar)).Value = numopArray(i)
        Next

        'cmd.Parameters.Add("NUMOP", numop)
        dr = cmd.ExecuteReader
        Dim dt = New DataTable
        Dim dtocorrencias As New DataTable


        dt.Columns.Add("vCodprod")
        dt.Columns.Add("vDescricao")
        dt.Columns.Add("vNumOP")
        dt.Columns.Add("vQtProg", GetType(Decimal))
        dt.Columns.Add("vQtProdUnit", GetType(Decimal))
        dt.Columns.Add("vQtProdMaster", GetType(Decimal))
        dt.Columns.Add("vAtendimento", GetType(Decimal))
        dt.Columns.Add("vEficiencia", GetType(Decimal))
        dt.Columns.Add("vProdutividade", GetType(Decimal))
        dt.Columns.Add("vTempoTotal", GetType(Decimal))
        dt.Columns.Add("vHorasParadas", GetType(Decimal))
        dt.Columns.Add("vHorasVariacao", GetType(Decimal))
        dt.Columns.Add("vHorasProdutivas", GetType(Decimal))
        dt.Columns.Add("vProdWinthor", GetType(Decimal))
        dt.Columns.Add("vNumLote", GetType(String))
        dt.Columns.Add("vPosicao", GetType(String))
        dt.Columns.Add("vQtUnitCX", GetType(Decimal))
        dt.Columns.Add("vDataInicio", GetType(String))
        dt.Columns.Add("vDataFim", GetType(String))


        Try


            Do While dr.Read

                dt.Rows.Add("", "",
                        dr("OP").ToString(),
                        0,
                        Convert.ToDecimal(dr("PRODUNIT")),
                        Convert.ToDecimal(dr("PRODUNIT")) / Convert.ToDecimal(dr("QTUNITCX")),
                        0,
                        Convert.ToDecimal(dr("HORASPRODUTIVAS")) / Convert.ToDecimal(dr("TEMPOTOTAL")),
                        0,
                        Convert.ToDecimal(dr("TEMPOTOTAL")),
                        Convert.ToDecimal(dr("HORASPARADAS")),
                        Convert.ToDecimal(dr("VARIACAODERITMO")),
                        Convert.ToDecimal(dr("HORASPRODUTIVAS")), 0, "", "",
                        Convert.ToDecimal(dr("QTUNITCX")),
                        dr("DATAINICIAL").ToString,
                        dr("DATAFINAL").ToString)


            Loop

        Catch ex As Exception

            MessageBox.Show(ex.Message)
            Exit Sub
        Finally
            dr.Close()
            cmd.Dispose()
        End Try

        'INICIA BUSCA DOS DADOS NO WINTHOR
        Dim cmdOra = New OracleCommand
        Dim drOra As OracleDataReader
        Dim inClauseOra As New StringBuilder()



        inClauseOra.Append("A.NUMOP IN (")

        For i As Integer = 0 To numopArray.Length - 1
            inClauseOra.Append(":NUMOP" & i)
            If i < numopArray.Length - 1 Then
                inClauseOra.Append(",")
            End If
        Next

        inClauseOra.Append(")")

        sql = "SELECT A.NUMOP,A.CODPRODMASTER CODPROD,B.DESCRICAO, A.QTPRODUZIR, A.QTPRODUZIDA, A.NUMLOTE, A.POSICAO FROM PCOPC A, PCPRODUT B WHERE A.CODPRODMASTER = B.CODPROD AND " & inClauseOra.ToString()

        cmdOra.CommandText = sql
        cmdOra.CommandType = CommandType.Text
        cmdOra.Connection = conexao


        For i As Integer = 0 To numopArray.Length - 1
            cmdOra.Parameters.Add("NUMOP" & i, numopArray(i))
        Next


        Try

            drOra = cmdOra.ExecuteReader

            Do While drOra.Read

                For i = 0 To dt.Rows.Count - 1

                    If dt.Rows(i)("vNumOP").ToString = drOra("NUMOP").ToString Then


                        dt.Rows(i)("vCodprod") = drOra("CODPROD")
                        dt.Rows(i)("vDescricao") = drOra("DESCRICAO")
                        dt.Rows(i)("vQtProg") = drOra("QTPRODUZIR")
                        dt.Rows(i)("vAtendimento") = dt.Rows(i)("vQtProdUnit") / drOra("QTPRODUZIR")
                        dt.Rows(i)("vProdWinthor") = drOra("QTPRODUZIDA")
                        dt.Rows(i)("vNumlote") = drOra("NUMLOTE")

                        If drOra("POSICAO").ToString = "F" Then
                            dt.Rows(i)("vPosicao") = "Fechada"
                        ElseIf drOra("POSICAO").ToString = "L" Then
                            dt.Rows(i)("vPosicao") = "Aguardando Produção"
                        ElseIf drOra("POSICAO").ToString = "P" Then
                            dt.Rows(i)("vPosicao") = "Em Produção"
                        ElseIf drOra("POSICAO").ToString = "C" Then
                            dt.Rows(i)("vPosicao") = "Cancelada"
                        End If
                    End If
                Next
            Loop


            drOra.Close()
            cmdOra.Dispose()



            'INICA A COLETA DAS OCORRENCIAS DE PARADAS NA PRODUÇÃO


            sql = "SELECT
                    HORAINICIAL,
                    HORAFINAL,
                    OPERADOR,
                    ROUND(TEMPOTOTAL,2) TEMPOTOTAL,
                    AREA,
                    EQUIPAMENTO,
                    COMPONENTE,
                    TIPODEFALHA,
                    MOTIVO
                    FROM BDADOS_REGISTROS
                    WHERE OP = @NUMOP
                    AND TIPODEFALHA NOT IN (
						                    'HORA PRODUTIVA',
						                    'HORA PRODUTIVA RODANDO ABAIXO DA NOMINAL',
						                    'VARIACAO DE RITMO')

                    ORDER BY HORAINICIAL"




            dtocorrencias.Columns.Add("vHORAINICIAL")
            dtocorrencias.Columns.Add("vHORAFINAL")
            dtocorrencias.Columns.Add("vOPERADOR")
            dtocorrencias.Columns.Add("vTEMPOTOTAL")
            dtocorrencias.Columns.Add("vAREA")
            dtocorrencias.Columns.Add("vEQUIPAMENTO")
            dtocorrencias.Columns.Add("vCOMPONENTE")
            dtocorrencias.Columns.Add("vTIPODEFALHA")
            dtocorrencias.Columns.Add("vMOTIVO")


            If mdlConexaoOracle.conexaoSQL.State = 0 Then ConectaSRVSQL()

            cmd.Connection = conexaoSQL
            cmd.CommandText = sql
            cmd.CommandType = CommandType.Text


            cmd.Parameters.Add(New SqlParameter("@NUMOP", SqlDbType.Int)).Value = numop
            dr = cmd.ExecuteReader


            Do While dr.Read


                dtocorrencias.Rows.Add(
                dr("HORAINICIAL").ToString,
                dr("HORAFINAL").ToString,
                dr("OPERADOR").ToString,
                Convert.ToDecimal(dr("TEMPOTOTAL")),
                dr("AREA").ToString,
                dr("EQUIPAMENTO").ToString,
                dr("COMPONENTE").ToString,
                dr("TIPODEFALHA").ToString,
                dr("MOTIVO").ToString)



            Loop



        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Exit Sub
        End Try




        Using frm = New frmRelRequsicaoOP(dt)

            Dim usuario As string = My.Settings.UsuarioWinthor.ToString & " - " & My.Settings.NomeWinthor.ToString
            frm.ReportViewer1.LocalReport.DataSources.Add(New Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", dt))
            frm.ReportViewer1.LocalReport.DataSources.Add(New Microsoft.Reporting.WinForms.ReportDataSource("DataSet2", dtocorrencias))
            frm.ReportViewer1.SetDisplayMode(DisplayMode.PrintLayout)
            frm.ReportViewer1.ZoomMode = ZoomMode.PageWidth
            frm.NomeRelatorio = "SeMSys.RelParadasOP.rdlc"
            frm.ReportViewer1.LocalReport.ReportEmbeddedResource = "SeMSys.RelParadasOP.rdlc"
            frm.ReportViewer1.LocalReport.SetParameters(New ReportParameter("parUsuario", usuario))
            frm.ShowDialog()
        End Using


    End Sub

End Class
