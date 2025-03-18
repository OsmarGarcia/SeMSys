Imports Microsoft.Reporting.WinForms
Imports Oracle.ManagedDataAccess.Client

Public Class ClassProducaoTotalWinthor


    Private Property sql As String = "SELECT
                                      TO_CHAR(M.DTMOV,'DD/MM/YYYY') DTMOV,
                                      TO_CHAR(M.CODPROD) CODPROD,
                                      TO_CHAR(S.CODSEC) CODSEC,
                                      S.DESCRICAO AS SECAO,
                                      TO_CHAR(D.CODEPTO) CODEPTO,
                                      D.DESCRICAO AS DEPTO,
                                      M.CODFILIAL,
                                      P.DESCRICAO,
                                      P.UNIDADE,
                                      P.EMBALAGEM,
                                      P.DESCRICAO1 AS NOMINAL,
                                      O.QTPRODUZIR AS QTPROGRAMADA,
                                      TO_CHAR(M.DTMOVLOG,'DD/MM/YYYY') DTMOV,
                                      TO_CHAR(M.CODFUNCREQ) CODFUNCREQ,
                                      (SELECT
                                        PCEMPR.NOME FROM
                                        PCEMPR WHERE
                                        PCEMPR.MATRICULA = M.CODFUNCREQ) AS NOME,
                                      M.QT,
                                      P.QTUNITCX AS QTUNITCX,
                                      --M.QT / CASE WHEN P.DESCRICAO1 = 0 THEN 1 ELSE P.DESCRICAO1 END  AS HORAPRODUTIVA,
                                      M.PUNIT,
                                      O.NUMOP,
                                      M.NUMLOTE,
                                      M.CODOPER,
                                      CASE
                                        WHEN M.CODOPER = 'SP'
                                        THEN 'ESTORNO DE APONTAMENTO'
                                        WHEN M.CODOPER = 'EP'
                                        THEN 'APONTAMENTO DE PRODUÇÃO'
                                        WHEN M.QT < 0
                                        THEN 'CANCELAMENTO DE PRODUÇÃO'
                                      END AS OPERACAO
                                    FROM
                                      PCMOV M,
                                      PCPRODUT P,
                                      PCOPC O,
                                      PCSECAO S,
                                      PCDEPTO D
                                    WHERE
  	                                  P.CODPROD = M.CODPROD
                                      AND M.CODPROD = O.CODPRODMASTER
                                      AND M.CODFILIAL = O.CODFILIAL
                                      AND P.CODSEC = S.CODSEC
                                      AND P.CODEPTO = D.CODEPTO
                                      AND M.NUMOP = O.NUMOP
                                      AND M.CODFILIAL IN (1,4)
                                      AND M.DTMOV BETWEEN TO_DATE(:DTINICIO,'DD/MM/YYYY') AND TO_DATE(:DTFIM,'DD/MM/YYYY')
                                      AND D.CODEPTO IN ('30','40')
                                      AND M.CODOPER IN ('EP','SP')
                                      --AND M.CODPROD = 801
                                    ORDER BY
                                      S.DESCRICAO,
                                      M.NUMLOTE,
                                      M.DTMOVLOG"




    Public Sub ConsultarProducaoTotal(dtinicio As String, dtfim As String)

        Dim cmdOra = New OracleCommand
        Dim drOra As OracleDataReader

        cmdOra.Parameters.Add("DTINICIO", dtinicio)
        cmdOra.Parameters.Add("DTFIM", dtfim)
        cmdOra.CommandText = sql
        cmdOra.CommandType = CommandType.Text
        cmdOra.Connection = conexao


        Dim dt As New DataTable

        Try
            drOra = cmdOra.ExecuteReader
            dt.Load(drOra)

            dt.Columns.Add("QTMASTER", GetType(Decimal))

            For i = 0 To dt.Rows.Count - 1

                dt.Rows(i)("QTMASTER") = dt.Rows(i)("QT") / dt.Rows(i)("QTUNITCX")


            Next



        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try



        Using frm = New frmRelRequsicaoOP(dt)

            Dim reportusuario As String = My.Settings.UsuarioWinthor.ToString & " - " & My.Settings.NomeWinthor.ToString


            frm.ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("DataSet1", dt))
            frm.ReportViewer1.SetDisplayMode(DisplayMode.PrintLayout)
            frm.ReportViewer1.ZoomMode = ZoomMode.PageWidth
            frm.NomeRelatorio = "SeMSys.RelProducaoTotalWinthor.rdlc"
            frm.ReportViewer1.LocalReport.ReportEmbeddedResource = "SeMSys.RelProducaoTotalWinthor.rdlc"

            frm.ReportViewer1.LocalReport.SetParameters(New ReportParameter("parUsuario", reportusuario))
            frm.ReportViewer1.LocalReport.SetParameters(New ReportParameter("parDtInicio", dtinicio))
            frm.ReportViewer1.LocalReport.SetParameters(New ReportParameter("parDtFim", dtfim))
            frm.ShowDialog()
        End Using


    End Sub


End Class
