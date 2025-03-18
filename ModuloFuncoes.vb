

Imports System.Transactions
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports System.Xml
Imports ADODB
Imports Microsoft.Reporting.WinForms.Internal.Soap.ReportingServices2005.Execution
Imports Oracle.ManagedDataAccess.Client

Module ModuloFuncoes


    Public metodoescolhido As String = ""
    Public Class Datas

        Public Property Data() As Date


        Public Sub New()
        End Sub

        Public Sub New(ByVal Data As Date)
            Me.Data = Data
        End Sub

    End Class


    Public Sub RecalcularReserva(codprod, codfilial)


        Dim sql As String = "DECLARE
      vENTRADA PKG_ANALISAR_ESTOQUE.TP_ENTRADA;
    BEGIN
      vENTRADA.CODFILIAL              := " & codfilial & ";
      vENTRADA.LISTA_DE_DEPOSITOS     := 0;
      vENTRADA.LISTA_DE_PRODUTOS      := " & codprod & ";
      vENTRADA.LISTA_DE_DEPARTAMENTOS := 0;
      vENTRADA.LISTA_DE_FORNECEDORES  := 0;
      vENTRADA.LISTA_DE_SECOES        := 0;
      vENTRADA.LISTA_DE_CATEGORIAS    := 0;
      vENTRADA.LISTA_DE_SUBCATEGORIAS := 0;
      vENTRADA.LISTA_DE_MARCAS        := 0;

      PKG_ANALISAR_ESTOQUE.PRC_RESERVADO(vENTRADA);
    END;"
        Dim cmd As New OracleCommand
        Dim transaction As OracleTransaction = conexao.BeginTransaction()
        Try



            cmd.Connection = conexao
            cmd.Transaction = transaction
            cmd.CommandText = sql


            cmd.ExecuteNonQuery()


            transaction.Commit()

            MessageBox.Show("Reserva recalculada com sucesso", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information)


        Catch ex As Exception
            MessageBox.Show("Erro ao recalcular saldo reservado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            transaction.Rollback()
        End Try
    End Sub


    Public Sub ReprogramarOPs(frmProgramarProducao As frmProgramarProducao, linha As String)

        'GoTo FIM
        ' cria um datatable pra armazenar os dados do listview
        Dim dt = New DataTable


        dt.Columns.Add("Codprod")
        dt.Columns.Add("Descricao")
        dt.Columns.Add("Embalagem")
        dt.Columns.Add("metodo")
        dt.Columns.Add("qtproduzir")
        dt.Columns.Add("Datainicial", Type.GetType("System.DateTime"))
        dt.Columns.Add("qtdops")
        dt.Columns.Add("Datafinal", Type.GetType("System.DateTime"))
        dt.Columns.Add("horasnecessarias")
        dt.Columns.Add("numop")
        dt.Columns.Add("numlote")
        dt.Columns.Add("idprograma", Type.GetType("System.Double"))
        dt.Columns.Add("qtunitcx")
        dt.Columns.Add("linha")








        ' preenche o datatable com os itens do listview somente com os dados da linha que está sendo tratada

        For i = 0 To frmProgramarProducao.listOrdens2.Items.Count - 1


            'valida se é da linha a ser tratada
            If frmProgramarProducao.listOrdens2.Items(i).SubItems(13).Text <> linha Then GoTo PROXIMO
            If frmProgramarProducao.listOrdens2.Items(i).SubItems(11).Text = "" Then frmProgramarProducao.listOrdens2.Items(i).SubItems(11).Text = "0"


            Dim datat As Date = DateTime.Parse(frmProgramarProducao.listOrdens2.Items(i).SubItems(5).Text.ToString)


            dt.Rows.Add(frmProgramarProducao.listOrdens2.Items(i).Text, frmProgramarProducao.listOrdens2.Items(i).SubItems(1).Text, frmProgramarProducao.listOrdens2.Items(i).SubItems(2).Text, frmProgramarProducao.listOrdens2.Items(i).SubItems(3).Text, frmProgramarProducao.listOrdens2.Items(i).SubItems(4).Text, datat, frmProgramarProducao.listOrdens2.Items(i).SubItems(6).Text, frmProgramarProducao.listOrdens2.Items(i).SubItems(7).Text, frmProgramarProducao.listOrdens2.Items(i).SubItems(8).Text, frmProgramarProducao.listOrdens2.Items(i).SubItems(9).Text, frmProgramarProducao.listOrdens2.Items(i).SubItems(10).Text, Convert.ToDouble(frmProgramarProducao.listOrdens2.Items(i).SubItems(11).Text), frmProgramarProducao.listOrdens2.Items(i).SubItems(12).Text, frmProgramarProducao.listOrdens2.Items(i).SubItems(13).Text)

            'frmProgramarProducao.listOrdens2.Items(i).Remove()   'exclui o item do list view
PROXIMO:
        Next


        ' exlcui os itens no listview
        Try
            ' Iterar de trás para frente para evitar problemas de índice
            For i As Integer = frmProgramarProducao.listOrdens2.Items.Count - 1 To 0 Step -1
                ' Valida se é da linha a ser tratada
                If frmProgramarProducao.listOrdens2.Items(i).SubItems(13).Text <> linha Then
                    Continue For ' pula para o próximo item
                End If

                ' Exclui o item do ListView
                frmProgramarProducao.listOrdens2.Items(i).Remove()
            Next
        Catch ex As ArgumentOutOfRangeException
            MessageBox.Show("Erro de índice: " & ex.Message)
        Catch ex As Exception
            MessageBox.Show("Erro: " & ex.Message)
        End Try




        'ordena pela data de menor pra maior
        dt.DefaultView.Sort = "Datainicial asc"
        dt = dt.DefaultView.ToTable


        'PEGA O PRIMEIRO HORARIO DA LINHA DE PRODUÇÃO

        Dim horainicial As Date
        If dt.Rows.Count > 0 Then
            horainicial = DateTime.Parse(dt.Rows(0)("Datainicial").ToString)
        End If



        'recalcular horas


        For x = 0 To dt.Rows.Count - 1
            If dt.Rows(x)("linha") <> linha Then GoTo pula

            Dim horas As Double = dt.Rows(x)("horasnecessarias")



            Dim horafinal As Date = horainicial.AddHours(horas)
            dt.Rows(x)("Datafinal") = horafinal
            dt.Rows(x)("Datainicial") = horainicial
            horainicial = horafinal

pula:
        Next


        For a = 0 To dt.Rows.Count - 1
            Dim item As ListViewItem = frmProgramarProducao.listOrdens2.Items.Add(dt.Rows(a)("codprod"))
            item.SubItems.Add(dt.Rows(a)("descricao"))
            item.SubItems.Add(dt.Rows(a)("embalagem"))
            item.SubItems.Add(dt.Rows(a)("metodo"))
            item.SubItems.Add(dt.Rows(a)("qtproduzir"))
            item.SubItems.Add(dt.Rows(a)("Datainicial"))
            item.SubItems.Add(dt.Rows(a)("qtdops"))
            item.SubItems.Add(dt.Rows(a)("Datafinal"))
            item.SubItems.Add(dt.Rows(a)("horasnecessarias"))
            item.SubItems.Add(dt.Rows(a)("numop"))
            item.SubItems.Add(dt.Rows(a)("numlote"))
            item.SubItems.Add(dt.Rows(a)("idprograma"))
            item.SubItems.Add(dt.Rows(a)("qtunitcx"))
            item.SubItems.Add(dt.Rows(a)("linha"))

        Next

        ColorirListaProdutosAcabados(frmProgramarProducao)

FIM:

    End Sub






    Public Sub ColorirListaMateriais(frmProgramarProducao As frmProgramarProducao)

        For i = 0 To frmProgramarProducao.listMateriais2.Items.Count - 1

            If Convert.ToDouble(frmProgramarProducao.listMateriais2.Items(i).SubItems(2).Text) > Convert.ToDouble(frmProgramarProducao.listMateriais2.Items(i).SubItems(3).Text) Then
                frmProgramarProducao.listMateriais2.Items(i).BackColor = Color.FromArgb(255, 255, 170, 170)

            End If
        Next


    End Sub


    Public Sub ColorirListaProdutosAcabados(frmProgramarProducao As frmProgramarProducao)
        On Error Resume Next

        Dim colors(4) As Color



        Dim corL01 As Color = Color.FromArgb(Convert.ToInt32(frmProgramarProducao.cor_L01.Substring(0, 2), 16),
                                             Convert.ToInt32(frmProgramarProducao.cor_L01.Substring(2, 2), 16),
                                             Convert.ToInt32(frmProgramarProducao.cor_L01.Substring(4, 2), 16),
                                             Convert.ToInt32(frmProgramarProducao.cor_L01.Substring(6, 2), 16))



        Dim corL02 As Color = Color.FromArgb(Convert.ToInt32(frmProgramarProducao.cor_L02.Substring(0, 2), 16), Convert.ToInt32(frmProgramarProducao.cor_L02.Substring(2, 2), 16), Convert.ToInt32(frmProgramarProducao.cor_L02.Substring(4, 2), 16), Convert.ToInt32(frmProgramarProducao.cor_L02.Substring(6, 2), 16))


        Dim corL03 As Color = Color.FromArgb(Convert.ToInt32(frmProgramarProducao.cor_L03.Substring(0, 2), 16), Convert.ToInt32(frmProgramarProducao.cor_L03.Substring(2, 2), 16), Convert.ToInt32(frmProgramarProducao.cor_L03.Substring(4, 2), 16), Convert.ToInt32(frmProgramarProducao.cor_L03.Substring(6, 2), 16))


        Dim corL04 As Color = Color.FromArgb(Convert.ToInt32(frmProgramarProducao.cor_L04.Substring(0, 2), 16), Convert.ToInt32(frmProgramarProducao.cor_L04.Substring(2, 2), 16), Convert.ToInt32(frmProgramarProducao.cor_L04.Substring(4, 2), 16), Convert.ToInt32(frmProgramarProducao.cor_L04.Substring(6, 2), 16))


        Dim corL05 As Color = Color.FromArgb(Convert.ToInt32(frmProgramarProducao.cor_L05.Substring(0, 2), 16), Convert.ToInt32(frmProgramarProducao.cor_L05.Substring(2, 2), 16), Convert.ToInt32(frmProgramarProducao.cor_L05.Substring(4, 2), 16), Convert.ToInt32(frmProgramarProducao.cor_L05.Substring(6, 2), 16))


        colors(0) = corL01
        colors(1) = corL02
        colors(2) = corL03
        colors(3) = corL04
        colors(4) = corL05




        For i = 0 To frmProgramarProducao.listOrdens2.Items.Count - 1


            frmProgramarProducao.listOrdens2.Items(i).BackColor = colors(Convert.ToDouble(frmProgramarProducao.listOrdens2.Items(i).SubItems(13).Text) - 1)


        Next


    End Sub



    Public Function DefinirMetodoMRP(Codprod, descricao)

        frmMetodoMRP.lblCodigo.Text = Codprod
        frmMetodoMRP.lblDescricao.Text = descricao

        Dim sql As String = ""

        sql = "select distinct metodo from pccomposicao where codprodmaster = '" & Codprod & "' and codfilial = '1'"




        cmd.Connection = conexao
        cmd.CommandText = sql
        cmd.CommandType = CommandType.Text
        Try
            datareader = cmd.ExecuteReader

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erro")
            Exit Function
        End Try

        Do While datareader.Read
            frmMetodoMRP.cboMetodos.Items.Add(datareader.Item(0).ToString)
        Loop


        frmMetodoMRP.ShowDialog()


        Return metodoescolhido


    End Function



End Module
