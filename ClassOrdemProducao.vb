Imports Microsoft.Reporting.WinForms
Imports Oracle.ManagedDataAccess.Client
Imports System.Data.SqlClient
Imports System.Text

Public Class ClassOrdemProducao





    Private Property NUMOP As String
    Private Property NUMLOTE As String
    Private Property COD128 As String
    Private Property CODPRODMASTER As String
    Private Property DATA_VALIDADE As String
    Private Property DESCRICAO As String
    Private Property EMBALAGEM As Double
    Private Property QTTOTPAL As Double
    Private Property LASTROPAL As Double
    Private Property ALTURAPAL As Double
    Private Property PRAZOVAL As Double
    Private Property QTPRODUZIR As Double
    Private Property QTNECESSIDADE As Double
    Private Property CODPROD As String
    Private Property DESCRICAOMP As Double


    Public Sub ImprimirOP(numop, offset)

        Dim dt As New DataTable
        Dim dtitens As New DataTable


        Dim sql = "SELECT
                        NUMOP,
                        NUMLOTE,
                        '010'||LPAD(CODAUXILIAR,13,0)||'12'||DT_VAL||'11'||DT_FAB||'3100'||LPAD(QTTOTPAL, 6, '0')||'10'||NUMLOTE AS COD128,
                        CODPRODMASTER,
                        DATA_VALIDADE,
                        DESCRICAO,
                        EMBALAGEM,
                        QTTOTPAL,
                        LASTROPAL,
                        ALTURAPAL,
                        PRAZOVAL,
                        QTPRODUZIR FROM (

                        SELECT
                          A.CODPRODMASTER,
                          TO_CHAR(SYSDATE) AS DATA_FABRICACAO,
                          TO_CHAR(SYSDATE,'YYMMDD') AS DT_FAB,
                          A.NUMOP,
                          TO_CHAR(CASE WHEN B.DESCRICAO6 = 'MESES' THEN
                          ADD_MONTHS(SYSDATE,B.PRAZOVAL)
                          ELSE 
                          SYSDATE + B.PRAZOVAL
                          END,'YYMMDD') AS DT_VAL,
                          TO_CHAR(CASE WHEN B.DESCRICAO6 = 'MESES' THEN
                          ADD_MONTHS(SYSDATE,B.PRAZOVAL)
                          ELSE 
                          SYSDATE + B.PRAZOVAL
                          END) AS DATA_VALIDADE ,
                          A.DTFECHA,
                          A.QTPRODUZIR,
                          B.DESCRICAO,
                          B.EMBALAGEM,
                          B.QTTOTPAL,
                          B.LASTROPAL,
                          B.ALTURAPAL,
                          B.PRAZOVAL,
                          B.MODULO,
                          B.RUA,
                          B.APTO,
                          B.LARGURAM3,
                          B.ALTURAM3,
                          B.COMPRIMENTOM3,
                          B.CODAUXILIAR,
                          CASE WHEN B.DESCRICAO7 = 'JULIANO' THEN
                          ( TO_CHAR(MOD(TO_NUMBER(TO_CHAR(SYSDATE, 'YYYY')), 10)) || 
                            TO_CHAR(SYSDATE, 'DDD'))
                            WHEN B.DESCRICAO7 = 'TAMPICO' THEN
                            '318-' || (SELECT 
                            TO_CHAR(MOD(TO_NUMBER(TO_CHAR(SYSDATE, 'YYYY')), 10)) || 
                            TO_CHAR(SYSDATE, 'DDD')
                            FROM DUAL)
                          ELSE
                          TO_CHAR(nvl(A.NUMLOTE,0) + " & offset & ") END  AS NUMLOTE,
                          B.QTUNITCX,
                          TO_DATE(SYSDATE,'DD/MM/YYYY'),
                          A.CODPRODMASTER ||'*'||TO_CHAR(SYSDATE,'DDMMYYYY') TESTE
                        FROM
                          PCOPC A
                          INNER JOIN PCPRODUT B ON A.CODPRODMASTER = B.CODPROD
                        WHERE
                          A.CODFILIAL = '1'
                          AND A.NUMOP = " & numop & ")"


        Using command As New OracleCommand(sql, conexao)


            'Oratransaction = conexao.BeginTransaction()
            'command.Transaction = Oratransaction
            'command.Parameters.Clear()
            'cmd.Parameters.Add(New OracleParameter(":NUMOP", OracleDbType.Varchar2)).Value = numop
            'cmd.Parameters.Add(New OracleParameter(":OFFSET", OracleDbType.Int32)).Value = offset
            'command.Parameters.Add(New OracleParameter(":QTPROD", OracleDbType.Decimal)).Value = QtProduzir
            'command.Parameters.Add(New OracleParameter(":QTREQ", OracleDbType.Decimal)).Value = qt
            command.CommandType = CommandType.Text

            Using dr As OracleDataReader = command.ExecuteReader()
                dt.Clear()
                dt.Load(dr)
            End Using
        End Using















        Using frm = New frmRelRequsicaoOP(dt)

            Dim usuario As String = My.Settings.UsuarioWinthor.ToString & " - " & My.Settings.NomeWinthor.ToString
            frm.ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("DataSet1", dt))
            frm.ReportViewer1.LocalReport.DataSources.Add(New Microsoft.Reporting.WinForms.ReportDataSource("DataSet2", dtitens))
            frm.ReportViewer1.SetDisplayMode(DisplayMode.PrintLayout)
            frm.ReportViewer1.ZoomMode = ZoomMode.PageWidth
            frm.NomeRelatorio = "SeMSys.RelOrdemProducao.rdlc"
            frm.ReportViewer1.LocalReport.ReportEmbeddedResource = "SeMSys.RelOrdemProducao.rdlc"
            'frm.ReportViewer1.LocalReport.SetParameters(New ReportParameter("Usuario", usuario))
            frm.ShowDialog()
        End Using


    End Sub



End Class
