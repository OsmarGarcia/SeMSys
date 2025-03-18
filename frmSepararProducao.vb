Imports Microsoft.Reporting.WinForms

Public Class frmSepararProducao
    Private Sub btnCarregarItens_Click(sender As Object, e As EventArgs) Handles btnPesquisar.Click
        Dim lv As ListView = listCabecalhoOPs
        If conexao.State = 0 Then
            ConectaOra()
        End If


        sql =
"      SELECT C.NUMOP
     , C.CODFILIAL
     , C.CODPRODMASTER
     , P.DESCRICAO
     , P.TIPOMERC
     , P.UNIDADE
     , P.EMBALAGEM
     , C.METODO
     , NVL(C.QTPRODUZIR,0)
     , NVL(C.QTPRODUZIDA,0)
     , C.DTLANC
     , C.POSICAO
     , C.QTHORAS
     , C.FINALIZAPRODUCAO
     , C.NUMLOTE
  FROM PEPROGOP C
     , PCPRODUT P
 WHERE C.CODPRODMASTER = P.CODPROD
   AND C.CODFILIAL = :CODFILIAL
   AND C.POSICAO IN ('L','P','F')
   AND C.DTPREVINICIO BETWEEN TO_DATE('" & txtDataInicial.Text & "','DD/MM/YYYY')
   AND TO_DATE('" & txtDataFinal.Text & "','DD/MM/YYYY')
 ORDER BY C.NUMOP DESC
"
        cmd.Parameters.Add("CODFILIAL", My.Settings.CodFilialProducao)
        cmd.Connection = conexao
        cmd.CommandText = sql
        cmd.CommandType = CommandType.Text
        Try
            datareader = cmd.ExecuteReader
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erro")
            Exit Sub
        End Try


        lv.Items.Clear()

        Do While datareader.Read

            Dim item As ListViewItem = lv.Items.Add(UCase(datareader(0).ToString))
            item.SubItems.Add(UCase(datareader(1).ToString))
            item.SubItems.Add(UCase(datareader(2).ToString))
            item.SubItems.Add(UCase(datareader(3).ToString))
            item.SubItems.Add(UCase(datareader(4).ToString))
            item.SubItems.Add(UCase(datareader(5).ToString))
            item.SubItems.Add(UCase(datareader(6).ToString))
            item.SubItems.Add(UCase(datareader(7).ToString))
            item.SubItems.Add(datareader(8).ToString)
            item.SubItems.Add(datareader(9).ToString)
            item.SubItems.Add(UCase(datareader(10).ToString))
            item.SubItems.Add(UCase(datareader(11).ToString))
            item.SubItems.Add(UCase(datareader(12).ToString))
            item.SubItems.Add(UCase(datareader(13).ToString))
            item.SubItems.Add(UCase(datareader(14).ToString))

        Loop

        datareader.Close()



    End Sub

    Private Sub frmSepararProducao_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        listCabecalhoOPs.CheckBoxes = True

    End Sub

    Private Sub btnSimularItens_Click(sender As Object, e As EventArgs) Handles btnSimularItens.Click

        Dim lv1 As ListView = listCabecalhoOPs
        Dim lv2 As ListView = listItensOPs
        Dim vContagem As Integer = lv1.CheckedItems.Count - 1
        Dim vItemsConcat As String = ""
        Dim colecao = lv1.CheckedItems
        Dim vItems = New List(Of String)

        If conexao.State = 0 Then
            ConectaOra()
        End If




        For i = 0 To vContagem
            vItems.Add(colecao.Item(i).Text)
        Next




        vItemsConcat = String.Join(",", vItems)

        Dim sql As String = "
                                SELECT 
                                I.CODPROD,
                                P.DESCRICAO,
                                ROUND(AVG(E.QTESTGER),2) QTESTOQUE,
                                ROUND(SUM(I.QTNECESSIDADE),2) QTNECESSIDADE
                                FROM PEPROGITENS I, PCPRODUT P, PCEST E
                                WHERE I.CODPROD = P.CODPROD
                                AND I.CODPROD = E.CODPROD
                                AND I.NUMOP IN (" & vItemsConcat & ")
                                AND I.CODOPER = 'SP'
                                AND E.CODFILIAL = '1'
                                GROUP BY I.CODPROD,P.DESCRICAO  
                                ORDER BY P.DESCRICAO
                            "

        cmd.Connection = conexao
        cmd.CommandText = sql
        cmd.CommandType = CommandType.Text
        Try
            datareader = cmd.ExecuteReader

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erro")
            Exit Sub
        End Try

        lv2.Items.Clear()

        Do While datareader.Read
            Dim item As ListViewItem = lv2.Items.Add(UCase(datareader(0).ToString))
            item.SubItems.Add(UCase(datareader(1).ToString))
            item.SubItems.Add(UCase(datareader(3).ToString))
            item.SubItems.Add(UCase(datareader(2).ToString))


        Loop

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim dt = ObterDados()

        Using frm = New frmRelRequsicaoOP(dt)
            frm.ReportViewer1.LocalReport.DataSources.Add(New Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", dt))
            frm.ReportViewer1.SetDisplayMode(DisplayMode.PrintLayout)
            frm.ReportViewer1.ZoomMode = ZoomMode.PageWidth
            frm.NomeRelatorio = "SeMSys.RelRequisicaoOP.rdlc"
            frm.ReportViewer1.LocalReport.ReportEmbeddedResource = "SeMSys.RelRequisicaoOP.rdlc"
            frm.ShowDialog()
        End Using
    End Sub

    Private Function ObterDados() As DataTable
        Dim dt = New DataTable
        dt.Columns.Add("vCodigo")
        dt.Columns.Add("vDescricao")
        dt.Columns.Add("vQuantidade", GetType(Decimal))
        dt.Columns.Add("vEstoque", GetType(Decimal))

        Dim lv As ListView = listItensOPs

        For i = 0 To lv.Items.Count - 1

            dt.Rows.Add(lv.Items(i).Text,
                        lv.Items(i).SubItems(1).Text,
                        lv.Items(i).SubItems(2).Text,
                        lv.Items(i).SubItems(3).Text)

        Next


        Return dt

    End Function
End Class