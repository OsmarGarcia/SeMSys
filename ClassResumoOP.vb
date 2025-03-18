Imports System.Data.SqlClient
Imports System.Linq.Expressions
Imports System.Text
Imports Microsoft.Reporting.WinForms
Imports Oracle.ManagedDataAccess.Client

Public Class ClassResumoOP


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



    Public Sub ResumirOP(numop)


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
                        Convert.ToDecimal(dr("HORASPRODUTIVAS")))


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

        sql = "SELECT A.NUMOP,A.CODPRODMASTER CODPROD,B.DESCRICAO, A.QTPRODUZIR FROM PCOPC A, PCPRODUT B WHERE A.CODPRODMASTER = B.CODPROD AND " & inClauseOra.ToString()

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


                    End If

                Next
            Loop





        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Exit Sub
        End Try




        Using frm = New frmRelRequsicaoOP(dt)
            frm.ReportViewer1.LocalReport.DataSources.Add(New Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", dt))
            frm.ReportViewer1.SetDisplayMode(DisplayMode.PrintLayout)
            frm.ReportViewer1.ZoomMode = ZoomMode.PageWidth
            frm.NomeRelatorio = "SeMSys.RelResumirOP.rdlc"
            frm.ReportViewer1.LocalReport.ReportEmbeddedResource = "SeMSys.RelResumirOP.rdlc"
            frm.ShowDialog()
        End Using


    End Sub

End Class
