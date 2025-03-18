

Imports Microsoft.Reporting.WinForms
Imports Oracle.ManagedDataAccess.Client
Imports SeMSys.frmConfig

Public Class frmProgramarProducao

    Public Property cor_L01 As String = My.Settings.Cor_Linha01.ToString
    Public Property cor_L02 As String = My.Settings.Cor_Linha02.ToString
    Public Property cor_L03 As String = My.Settings.Cor_Linha03.ToString
    Public Property cor_L04 As String = My.Settings.Cor_Linha04.ToString
    Public Property cor_L05 As String = My.Settings.Cor_Linha05.ToString

    Public Property txtcodprod As String



    Private Sub AlterarEstadoBotoesPorTag(panel As Panel, tagName As String, isEnabled As Boolean)
        For Each ctrl As Control In panel.Controls
            ' Verifica se o controle é um botão
            If TypeOf ctrl Is Button Then
                Dim button As Button = DirectCast(ctrl, Button)

                ' Verifica se o botão possui a tag desejada dentro de um conjunto de palavras separadas por vírgula
                If button.Tag IsNot Nothing Then
                    Dim tags As String() = button.Tag.ToString().ToLower().Split(","c)
                    If tags.Contains(tagName.ToLower()) Then
                        button.Enabled = isEnabled
                    Else
                        button.Enabled = Not isEnabled
                    End If
                End If
            End If
        Next
    End Sub



    Private Sub frmProgramarProducao_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        Dim settingsList As New List(Of SettingItem)()


        listOrdens2.Columns(6).Width = 0



        ConectaOra()



        DtPickerHoraInicial.Format = DateTimePickerFormat.Custom
        DtPickerHoraInicial.CustomFormat = "dd/MM/yyyy HH:mm:ss"

        DtPickerHoraFinal.Format = DateTimePickerFormat.Custom
        DtPickerHoraFinal.CustomFormat = "dd/MM/yyyy HH:mm:ss"

        cboLinha2.Items.Add("1")
        cboLinha2.Items.Add("2")
        cboLinha2.Items.Add("3")
        cboLinha2.Items.Add("4")
        cboLinha2.Items.Add("5")


        lblFilialEstoque.Text = lblFilialEstoque.Text & My.Settings.CodFilialEstoque
        lblFilialProducao.Text = lblFilialProducao.Text & My.Settings.CodFilialProducao

        AlterarEstadoBotoesPorTag(Me.panelProgramar2, My.Settings.PermissaoUsuario, True)


    End Sub

    Private Sub btnPesquisar_Click(sender As Object, e As EventArgs)
        Dim frm As New frmPesquisar(Me, txtCodProd2)
        frm.Show()

        txtCodProd2.Text = txtcodprod
    End Sub

    Private Sub btnProgramar_Click(sender As Object, e As EventArgs) Handles btnProgramar.Click

        If panelProgramar2.Visible = False Then
            panelProgramar2.Visible = True
        Else
            panelProgramar2.Visible = False
        End If



    End Sub

    Private Sub btnAdicionar_Click()
        Throw New NotImplementedException()
    End Sub

    Dim vHoraFinal As Date
    Private Sub btnIncluir_Click(sender As Object, e As EventArgs) Handles btnIncluir2.Click


        Dim qthorasporTurno = 19
        Dim velocidade_nominal = txtVelocidadeNominal2.Text



        'Valida demais campos se estão preenchidos
        If txtCodProd2.Text = "" _
        Or txtDescricao2.Text = "" _
        Or txtEmbalagem2.Text = "" _
        Or txtQtdOps2.Text = "" _
        Or txtEficiencia.Text = "" _
        Or txtQtProduzir2.Text = "" _
        Or cboLinha2.Text = "" Then

            MessageBox.Show("Preencha todos os campos")
            Exit Sub
        End If

        'Valida se tem um método válido pro produto acabado
        If cboMetodo2.Text = "-" AndAlso txtCodProd2.Text <> "99999" Then
            MessageBox.Show("Insira um método válido para o produto acabado", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If


        'Validar se está sendo inserido um item com OP associada
        If txtNumOP.Text <> "" AndAlso txtNumOP.Text <> "-" AndAlso txtNumLote.Text <> "" AndAlso txtNumLote.Text <> "-" Then


            'Validar se a quantidade inserida é diferente da quantidade programada da OP no Winthor

            Dim sql = "SELECT NVL(A.QTPRODUZIR,0) QTPRODUZIR, A.POSICAO,A.DTPREVINICIO, (SELECT DESCRICAO7 FROM PCPRODUT WHERE PCPRODUT.CODPROD = A.CODPRODMASTER) AS TIPOLOTE FROM PCOPC A WHERE A.NUMOP = :NUMOP"
            Dim dtOP As New DataTable
            Try
                ' Primeira instrução SQL
                Dim cmd As New OracleCommand
                Dim dr As OracleDataReader
                cmd.CommandType = CommandType.Text
                cmd.Connection = conexao
                cmd.CommandText = sql
                cmd.Parameters.Clear()
                cmd.Parameters.Add("NUMOP", OracleDbType.Varchar2).Value = txtNumOP.Text

                dr = cmd.ExecuteReader()
                dtOP.Load(dr)
                dr.Dispose()
                cmd.Dispose()

                If Convert.ToDecimal(dtOP.Rows(0)("QTPRODUZIR")) = 0 Or dtOP.Rows(0)("POSICAO").ToString = "C" Or dtOP.Rows(0)("POSICAO").ToString = "F" Then
                    MessageBox.Show("Não é possível incluir esta OP na programação." & vbCrLf & vbCrLf & "Possíveis causas:" & vbCrLf & "- OP NÃO EXISTENTE" & vbCrLf & "- OP COM QUANTIDADE A PRODUZIR ZERADA" & vbCrLf & "- OP FECHADA OU CANCELADA", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Exit Sub
                End If

                'Valida se a OP já existe na programação
                For i = 0 To listOrdens2.Items.Count - 1
                    If listOrdens2.Items(i).SubItems(9).Text = txtNumOP.Text Then
                        MessageBox.Show("OP já inserida na programação." & vbCrLf & vbCrLf & "Impossível adicionar novamente esta OP.")
                        Exit Sub
                    End If
                Next


                If Convert.ToDecimal(dtOP.Rows(0)("QTPRODUZIR")) <> Convert.ToDecimal(txtQtProduzir2.Text) Or dtOP.Rows(0)("dtprevinicio").ToString() <> DtPickerHoraInicial.Value.ToString() Then
                    ReprogramarOPWinthor(txtNumOP.Text, Convert.ToDouble(txtQtProduzir2.Text), txtNumLote.Text, dtOP.Rows(0)("TIPOLOTE").ToString(), DtPickerHoraInicial.Value.ToString())



                End If

            Catch ex As Exception
                MessageBox.Show("Erro ao inserir OP." & vbCrLf & "Numop: " & txtNumOP.Text & vbCrLf & ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End Try



            sql = "SELECT NVL(A.QTPRODUZIR,0) QTPRODUZIR, A.POSICAO,A.DTPREVINICIO,NUMLOTE FROM PCOPC A WHERE A.NUMOP = :NUMOP"

            Try
                ' Primeira instrução SQL
                Dim cmd As New OracleCommand
                Dim dr As OracleDataReader
                cmd.CommandType = CommandType.Text
                cmd.Connection = conexao
                cmd.CommandText = sql
                cmd.Parameters.Clear()
                cmd.Parameters.Add("NUMOP", OracleDbType.Varchar2).Value = txtNumOP.Text

                dr = cmd.ExecuteReader()
                dtOP.Clear()
                dtOP.Load(dr)
                dr.Dispose()
                cmd.Dispose()

                txtNumLote.Text = dtOP.Rows(0)("NUMLOTE").ToString()

            Catch ex As Exception
                MessageBox.Show("Erro ao recuperar novo lote da OP." & vbCrLf & "Numop: " & txtNumOP.Text & vbCrLf & ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try




        End If



        'Calcula horas de produção

        Dim horas As Double
        Dim qt As Double = Convert.ToDecimal(Replace(txtQtProduzir2.Text, ".", ",")) * 1
        Dim eficiencia As Double = Convert.ToDouble(txtEficiencia.Text.Trim("%")) / 10000

        horas = Math.Round(qt / velocidade_nominal / eficiencia, 2)





        'Adiciona valores de cabeçalho da OP no listview

        Dim lv As Windows.Forms.ListView = listOrdens2
        Dim item As ListViewItem = lv.Items.Add(txtCodProd2.Text)   'ITEM 0
        item.SubItems.Add(txtDescricao2.Text) 'ITEM 1
        item.SubItems.Add(txtEmbalagem2.Text) 'ITEM 2
        item.SubItems.Add(cboMetodo2.Text) 'ITEM 3

        'ITEM 4
        If txtCodProd2.Text = "99999" Then
            item.SubItems.Add("0")
        Else
            item.SubItems.Add(txtQtProduzir2.Text)
        End If




        'Pega o ultimo horario da linha de produção adicionada

        Dim vLinha As String = cboLinha2.Text
        Dim dtLinha As New DataTable

        dtLinha.Columns.Add("HoraFinal", Type.GetType("System.DateTime"))

        For i = 0 To listOrdens2.Items.Count - 2

            If listOrdens2.Items(i).SubItems(13).Text = vLinha Then 'valida se é da mesma linha que está sendo incluida

                dtLinha.Rows.Add(DateTime.Parse(listOrdens2.Items(i).SubItems(7).Text))


            End If
        Next

        dtLinha.DefaultView.Sort = "HoraFinal desc"
        Dim dtlinhaordenada = dtLinha.DefaultView.ToTable()
        dtLinha = dtlinhaordenada

        Dim conttagem As Integer = dtLinha.Rows.Count




        'ITEM 5

        'VERIFICA SE A HORA INICIAL QUE ESTÁ SENDO LANÇADA É MENOR QUE A MENOR HORA DA LISTA JÁ INCLUIDA,
        'SE SIM INICIA PELA NOVA HORA QUE ESTÁ SENDO LANÇADA, SENÃO, LANÇA NO FINAL DA LISTA
        If conttagem > 0 AndAlso DateTime.Parse(dtLinha.Rows(0)("HoraFinal").ToString) < DateTime.Parse(DtPickerHoraInicial.Text) Then
            item.SubItems.Add(DateTime.Parse(dtLinha.Rows(0)("HoraFinal").ToString)) 'Hora Inicial

        Else
            item.SubItems.Add(DtPickerHoraInicial.Text)
        End If


        Dim hora As String = Convert.ToDateTime(DtPickerHoraInicial.Text).ToString("t")


        vHoraFinal = DtPickerHoraInicial.Value.AddHours(horas).ToString 'Data Final
        DtPickerHoraInicial.Text = vHoraFinal


        item.SubItems.Add("1") 'ITEM 7
        item.SubItems.Add(vHoraFinal) 'ITEM 8
        item.SubItems.Add(horas) 'ITEM 9
        item.SubItems.Add(txtNumOP.Text) 'ITEM 10
        item.SubItems.Add(txtNumLote.Text) 'ITEM 11
        item.SubItems.Add("") 'ITEM 12
        item.SubItems.Add(txtQtunitcx.Text) 'ITEM 13
        item.SubItems.Add(cboLinha2.Text) 'ITEM 14

FIM:
        ' reprograma a grid pro caso de ser adicionado um produto com uma data no meio da grid
        ReprogramarOPs(Me, vLinha)

        txtCodProd2.Select()

    End Sub

    Private Sub CalcularHorasProgramadas()

        Dim datainicial As Date = Convert.ToDateTime(listOrdens2.Items(0).SubItems(5).Text)
        Dim horasnecessarias As Double = Convert.ToDouble(listOrdens2.Items(0).SubItems(8).Text)
        Dim tempointervalo As Integer = 0
        Dim qtturnos As Double
        Dim qtDiaInteiro As Double
        Dim dif As Double
        Dim QtDias As Double
        Dim HorasIntervalo As Double = 1
        Dim HorasIntervaloTotal As Double
        Dim HorasIntraturno As Double = 5
        Dim HorasTotalIntraturno As Double




        For i = 0 To listOrdens2.Items.Count - 1

            datainicial = Convert.ToDateTime(listOrdens2.Items(i).SubItems(5).Text)
            horasnecessarias = Convert.ToDouble(listOrdens2.Items(i).SubItems(8).Text)
            qtturnos = horasnecessarias / 8
            QtDias = horasnecessarias / 24
            qtDiaInteiro = Math.Floor(horasnecessarias / 24)
            dif = QtDias - qtDiaInteiro
            HorasIntervaloTotal = HorasIntervalo * qtturnos
            HorasTotalIntraturno = HorasIntraturno * qtDiaInteiro

            horasnecessarias += HorasIntervaloTotal + HorasTotalIntraturno

            listOrdens2.Items(i).SubItems(8).Text = horasnecessarias


        Next


    End Sub

    Public Sub CapturarDadosProduto()
        Dim cod As String = txtCodProd2.Text

        If cod = "" Then
            Exit Sub
        ElseIf cod = "99999" Then
            txtDescricao2.Text = "SEM PROGRAMAÇÃO"
            txtEficiencia.Text = "1000"
            txtVelocidadeNominal2.Text = "1"
            txtEmbalagem2.Text = "-"
            cboMetodo2.Text = "-"
            txtQtunitcx.Text = "0"

            txtQtProduzir2.Focus()

            Exit Sub
        End If

        sql = "select 
                descricao, 
                embalagem, 
                nvl(descricao1,0) descricao1, 
                nvl(descricao2,0) descricao2,
                qtunitcx
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
            txtDescricao2.Text = UCase(datareader("descricao").ToString)
            txtEmbalagem2.Text = UCase(datareader("embalagem").ToString)
            txtQtunitcx.Text = UCase(datareader("qtunitcx").ToString)

            If cboLinha2.Text = "M30" Then
                txtVelocidadeNominal2.Text = datareader("descricao1").ToString
            Else
                txtVelocidadeNominal2.Text = datareader("descricao2").ToString
            End If
        Loop


        cboMetodo2.Items.Clear()

        sql = "select distinct metodo
                 from pccomposicao 
                    where codprodmaster = " & cod & " and codfilial = 4"

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
            cboMetodo2.Items.Add(datareader(0).ToString)
        Loop
    End Sub
    Private Sub txtCodProd2_LostFocus(sender As Object, e As EventArgs) Handles txtCodProd2.LostFocus
        CapturarDadosProduto()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click



        If listOrdens2.Items.Count = 0 Then Exit Sub


        Dim resposta As DialogResult = MessageBox.Show("Deseja realmente excluir este item da programação?", "Excluir Item", MessageBoxButtons.YesNo)

        If resposta = DialogResult.No Then Exit Sub


        Dim linha = listOrdens2.FocusedItem.SubItems(13).Text
        Dim numop = listOrdens2.FocusedItem.SubItems(9).Text




        If listOrdens2.FocusedItem.SubItems(11).Text = "" Then GoTo FIM
        Dim idprograma As Integer = listOrdens2.FocusedItem.SubItems(11).Text

        If ExistePrograma(idprograma) = False Then
            listOrdens2.FocusedItem.Remove()
            ReprogramarOPs(Me, linha)
            Exit Sub
        End If




        'cancela OP na SMPROGRAMAPRODUCAO
        sql = "update smprogramaproducao set status = 'CANCELADA' where idprograma = :IDPROGRAMA"

        cmd.Connection = conexao
        cmd.CommandText = sql
        cmd.CommandType = CommandType.Text
        cmd.Parameters.Add("IDPROGRAMA", idprograma)
        Oratransaction = conexao.BeginTransaction()
        cmd.Transaction = Oratransaction
        Try
            cmd.ExecuteNonQuery()
            Oratransaction.Commit()

        Catch ex As Exception
            MessageBox.Show("Erro ao exlcuir item do programa de produção na SMPROGRAMAPRODUCAO." & vbCrLf & vbCrLf & ex.Message, "Erro")
            Oratransaction.Rollback()
            Exit Sub
        Finally
            cmd.Dispose()
        End Try



        If numop = "" Or numop = Nothing Or numop < 0 Then
            GoTo FIM
        End If


        Dim r As DialogResult = MessageBox.Show("Deseja cancelar a ordem de produção no Winthor?", "Excluir Item", MessageBoxButtons.YesNo)

        If r = DialogResult.No Then GoTo FIM


        'CANCELA OP NA PCOPC

        sql = "UPDATE PCOPC SET POSICAO = 'C' WHERE NUMOP = :NUMOP"


        cmd.Connection = conexao
        cmd.CommandText = sql
        cmd.CommandType = CommandType.Text
        cmd.Parameters.Add("NUMOP", numop)
        Oratransaction = conexao.BeginTransaction()
        cmd.Transaction = Oratransaction
        Try
            cmd.ExecuteNonQuery()
            Oratransaction.Commit()

        Catch ex As Exception
            MessageBox.Show("Erro ao cancelar OP na PCOPC." & vbCrLf & vbCrLf & ex.Message, "Erro")
            Oratransaction.Rollback()
            GoTo FIM
        Finally
            cmd.Dispose()

        End Try

FIM:
        listOrdens2.FocusedItem.Remove()

        If listOrdens2.Items.Count > 0 Then
            ReprogramarOPs(Me, linha)
            SalvarPrograma(txtCodPrograma.Text)
        Else

            SalvarPrograma(txtCodPrograma.Text)
        End If

    End Sub


    Public Sub GerarMRP()

        Dim lv1 As ListView = listOrdens2
        Dim lv2 As ListView = listMateriais2
        Dim dt As New DataTable ' Produtos Acabados (pai)
        Dim dt2 As New DataTable ' Produtos filhos


        If conexao.State = 0 Then ConectaOra()

        dt.Columns.Add("codprod")
        dt.Columns.Add("Qt", Type.GetType("System.Double"))
        dt.Columns.Add("metodo")

        dt2.Columns.Add("codprod")
        dt2.Columns.Add("descricao")
        dt2.Columns.Add("qtnecessidade")
        dt2.Columns.Add("qtestoque")
        dt2.Columns.Add("tipomerc")
        dt2.Columns.Add("metodo")


        'lv2.Items.Clear()


        'ZERAR QUANTIDADES DOS INSUMOS
        For i = 0 To lv2.Items.Count - 1
            lv2.Items(i).SubItems(2).Text = 0
            lv2.Items(i).Checked = False
        Next


        For y = 0 To lv1.Items.Count - 1

            FormularAcabados(lv1.Items(y).Text, lv1.Items(y).SubItems(3).Text, My.Settings.CodFilialEstoque, My.Settings.CodFilialProducao, lv1.Items(y).SubItems(4).Text, lv1.Items(y).SubItems(5).Text)

        Next

Formular:
        For z = 0 To lv2.Items.Count - 1

            If lv2.Items(z).SubItems(4).Text = "SA" And lv2.Items(z).Checked = False Then


                Dim metodo As String = ""

                If lv2.Items(z).SubItems(5).Text = "" Or lv2.Items(z).SubItems(5).Text = "-" Then
                    metodo = DefinirMetodoMRP(lv2.Items(z).Text, lv2.Items(z).SubItems(1).Text)
                    lv2.Items(z).SubItems(5).Text = metodo
                Else
                    metodo = lv2.Items(z).SubItems(5).Text
                End If



                FormularAcabados(lv2.Items(z).Text, metodo, My.Settings.CodFilialEstoque, My.Settings.CodFilialProducao, lv2.Items(z).SubItems(2).Text, lv2.Items(z).SubItems(9).Text)
                lv2.Items(z).Checked = True


            End If

            lv2.Items(z).Checked = True

        Next

        If lv2.CheckedItems.Count < lv2.Items.Count Then
            GoTo Formular
        End If



        For i As Integer = lv2.Items.Count - 1 To 0 Step -1
            Dim item As ListViewItem = lv2.Items(i)

            'Se o item tiver qtd zerada

            If item.SubItems(2).Text = "0" Then

                'se existir uma OP aberta pra este item
                If item.SubItems(6).Text <> "0" And item.SubItems(6).Text <> "" And item.SubItems(6).Text <> "-" Then


                    Dim sql = "UPDATE PCOPC SET POSICAO = 'C' WHERE NUMOP = :NUMOP"


                    Using cmd As New OracleCommand()
                        cmd.Connection = conexao
                        cmd.CommandText = sql
                        cmd.CommandType = CommandType.Text
                        cmd.Parameters.Add("NUMOP", item.SubItems(6).Text)
                        'Oratransaction = conexao.BeginTransaction()
                        'cmd.Transaction = Oratransaction
                        Try
                            cmd.ExecuteNonQuery()
                            'Oratransaction.Commit()

                        Catch ex As Exception
                            MessageBox.Show("Erro ao cancelar OP na PCOPC." & vbCrLf & vbCrLf & ex.Message, "Erro")
                            'Oratransaction.Rollback()

                        End Try
                    End Using

                End If



                lv2.Items.RemoveAt(i)
            End If
        Next





    End Sub


    Public Sub FormularAcabados(codprod As String, metodo As String, filialestoque As String, filialproducao As String, qt As Double, dtinicio As String)


        Dim lv2 As ListView = listMateriais2
        Dim dt As New DataTable ' Produtos Acabados (pai)
        Dim dt2 As New DataTable ' Produtos filhos
        Dim qtproduzir
        Dim numop
        Dim numlote As String
        Dim cmd As New OracleCommand

        If conexao.State = 0 Then ConectaOra()

        dt.Columns.Add("codprod")
        dt.Columns.Add("Qt", Type.GetType("System.Double"))
        dt.Columns.Add("metodo")

        dt2.Columns.Add("codprod")
        dt2.Columns.Add("descricao")
        dt2.Columns.Add("qtnecessidade")
        dt2.Columns.Add("qtestoque")
        dt2.Columns.Add("tipomerc")
        dt2.Columns.Add("metodo")
        dt2.Columns.Add("dtinicio")

        Dim sql As String = "
                                SELECT 
                                A.CODPROD,
                                B.DESCRICAO,
                                A.METODO,
                                SUM(A.QT) QTNECESSIDADE,
                                SUM(C.QTESTGER - C.QTBLOQUEADA - C.QTRESERV) ESTOQUEDISP,
                                B.TIPOMERC
                                FROM PCCOMPOSICAO A, PCPRODUT B, PCEST C
                                WHERE A.CODPROD = B.CODPROD
                                AND A.CODPROD = C.CODPROD
                                AND A.codprodmaster = '" & codprod & "'
                                AND A.METODO = '" & metodo & "'
                                AND C.CODFILIAL = '" & filialestoque & "'
                                AND A.CODFILIAL = '" & filialproducao & "'
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
            Exit Sub
        End Try

        dt2.Rows.Clear()

        Do While datareader.Read

            dt2.Rows.Add(UCase(datareader(0).ToString),
                             UCase(datareader(1).ToString),
                             Math.Round(UCase(datareader(3).ToString) * qt, 6),
                             UCase(datareader(4).ToString),
                             UCase(datareader(5).ToString),
                             UCase(datareader(2).ToString),
                             UCase(dtinicio))

        Loop

        If lv2.Items.Count = 0 Then


            'se o list view estiver vazio

            For x = 0 To dt2.Rows.Count - 1


                Dim item2 As ListViewItem = lv2.Items.Add(dt2.Rows(x)("codprod"))
                item2.SubItems.Add(dt2.Rows(x)("descricao"))
                item2.SubItems.Add(dt2.Rows(x)("qtnecessidade"))
                item2.SubItems.Add(dt2.Rows(x)("qtestoque"))
                item2.SubItems.Add(dt2.Rows(x)("tipomerc"))
                item2.SubItems.Add("")
                item2.SubItems.Add(codprod)
                item2.SubItems.Add("")
                item2.SubItems.Add("")
                item2.SubItems.Add(dtinicio)
                item2.SubItems.Add("")

                item2.Checked = False



            Next

        Else

            'se o list view não estiver vazio
            For x = 0 To dt2.Rows.Count - 1

                If ProcurarNoListView(dt2.Rows(x)("codprod").ToString) = True Then

                    For w = 0 To lv2.Items.Count - 1






                        If lv2.Items(w).Text = dt2.Rows(x)("codprod").ToString Then

                            numop = lv2.Items(w).SubItems(7).Text
                            NumLote = lv2.Items(w).SubItems(8).Text

                            Dim valor, valor2

                            valor = Convert.ToDouble(lv2.Items(w).SubItems(2).Text)
                            valor2 = Convert.ToDouble(dt2.Rows(x).Item(2).ToString)

                            If valor = valor2 Then
                                Exit For
                            End If

                            lv2.Items(w).SubItems(2).Text = valor + valor2
                            qtproduzir = Math.Round(Convert.ToDecimal(lv2.Items(w).SubItems(2).Text), 6)

                            'Existe OP
                            If numop <> "" Then


                                'Consulta a quantidade atual da OP 
                                sql = "SELECT NVL(A.QTPRODUZIR,0) QTPRODUZIR, A.POSICAO,(SELECT DESCRICAO7 FROM PCPRODUT WHERE PCPRODUT.CODPROD = A.CODPRODMASTER) AS TIPOLOTE FROM PCOPC A WHERE A.NUMOP = :NUMOP"
                                Dim dtOP As New DataTable
                                Try
                                    ' Primeira instrução SQL
                                    'Dim cmd As New OracleCommand
                                    Dim dr As OracleDataReader
                                    cmd.CommandType = CommandType.Text
                                    cmd.Connection = conexao
                                    cmd.CommandText = sql
                                    cmd.Parameters.Clear()
                                    cmd.Parameters.Add("NUMOP", OracleDbType.Varchar2).Value = numop

                                    dr = cmd.ExecuteReader()
                                    dtOP.Load(dr)
                                    dr.Dispose()
                                    cmd.Dispose()

                                    If Convert.ToDecimal(dtOP.Rows(0)("QTPRODUZIR")) = qtproduzir Then
                                        Exit For
                                    End If
                                Catch

                                End Try




                                MessageBox.Show("Já existe uma OP associada ao produto " & lv2.Items(w).Text & " - " & lv2.Items(w).SubItems(1).Text & vbCrLf & vbCrLf & "NUMOP: " & numop & vbCrLf & vbCrLf & "A OP será reprogramada para a nova quantidade necessária." & vbCrLf & vbCrLf & "Qtd Anterior: " & valor & vbCrLf & "Qtd Atual: " & qtproduzir, "Atenção!")

                                ReprogramarOPWinthor(numop, qtproduzir, numlote, dtOP.Rows(0)("TIPOLOTE").ToString(), dtinicio)



                            End If

                            Exit For

                        End If




                    Next


                Else

                    Dim item2 As ListViewItem = lv2.Items.Add(dt2.Rows(x)("codprod"))
                    item2.SubItems.Add(dt2.Rows(x)("descricao"))
                    item2.SubItems.Add(dt2.Rows(x)("qtnecessidade"))
                    item2.SubItems.Add(dt2.Rows(x)("qtestoque"))
                    item2.SubItems.Add(dt2.Rows(x)("tipomerc"))
                    item2.SubItems.Add("")
                    item2.SubItems.Add(codprod)
                    item2.SubItems.Add("")
                    item2.SubItems.Add("")
                    item2.SubItems.Add(dtinicio)
                    item2.SubItems.Add("")

                    item2.Checked = False

                End If


            Next



        End If






    End Sub



    Private Function ProcurarNoListView(codprod As String) As Integer
        Dim lv As ListView = listMateriais2
        Dim i As Integer = 0
        For x = 0 To lv.Items.Count - 1
            If lv.Items(x).Text = codprod Then i = i + 1
        Next

        If i > 0 Then
            Return True
        Else
            Return False
        End If

    End Function


    Public Function GerarProgramacao(codprod As String, metodo As String, qtproduzir As Decimal, codfunc As String, dtprevinicio As String) As DataTable


        If conexao.State = 0 Then ConectaOra()
        If dtprevinicio = "" Or dtprevinicio = Nothing Then
            dtprevinicio = Date.Today.ToString()
        End If
        Dim format As Globalization.NumberFormatInfo = New Globalization.NumberFormatInfo()
        format.NumberDecimalSeparator = "."

        Dim numop As String
        Dim numlote As String
        Dim SQL As String
        Dim lvOrdens As ListView = listOrdens2
        Dim lvMateriais As ListView = listMateriais2
        Dim TIPO_LOTE As String
        qtproduzir = Math.Round(qtproduzir, 3)


        cmd.CommandType = CommandType.Text
        cmd = conexao.CreateCommand



        '\\\\\\\\\\\\\\\\\\\\inicia uma nova transação\\\\\\\\\\\\\\\\\\

        '\\\\\\\\\\\\\\\\\\\\NOVA ORDEM DE PRODUÇÃO\\\\\\\\\\\\\\\\\\\\\\\\\\

        Oratransaction = conexao.BeginTransaction()
        cmd.Transaction = Oratransaction


        Try

            'SELECIONA NO BANCO O PROXIMO NUMERO DE OP
            SQL = "SELECT CASE WHEN PROXNUMOP > PROXNUMPROG THEN
                 PROXNUMOP
                 WHEN PROXNUMOP <= PROXNUMPROG  THEN
                 PROXNUMPROG
                 END PROXNUMPROG
                 FROM (SELECT NVL((SELECT MAX(NUMOP) FROM PEPROGOP),1)+1 PROXNUMPROG
               , NVL((SELECT MAX(NUMOP) FROM PCOPC),1)+1 PROXNUMOP
                 FROM DUAL)"

            cmd.CommandText = SQL
            datareader = cmd.ExecuteReader
            datareader.Read()
            numop = datareader("PROXNUMPROG").ToString




            'atualiza parametros do petrah
            SQL = "UPDATE PEPARAMETROS
               SET PROXNUMPROG = " & numop + 1


            cmd.CommandText = SQL
            cmd.ExecuteNonQuery()




            ' atualiza pcconsum com o proximo numero de op
            SQL = "UPDATE PCCONSUM
               SET PROXNUMOP = " & numop + 1


            cmd.CommandText = SQL
            cmd.ExecuteNonQuery()





            'valida parametros de lote estabelecidos no petrah

            SQL = "SELECT NVL(SEQUENCIALOTE,'P') SEQUENCIALOTE
                     , NVL(TRAVARLOTE,'N') TRAVARLOTE
                  FROM PEPARAMETROS
                 WHERE CODFILIAL = '1'"

            cmd.CommandText = SQL
            datareader = cmd.ExecuteReader()
            datareader.Read()




            'seleciona tipo de sequicial de lote e define o numlote
            If datareader("SEQUENCIALOTE").ToString = "P" Then
                'quando lote for por produto, seleciona o proximo lote do produto na pcprodut
                SQL = "SELECT (PREFIXOLOTE||NVL(PROXNUMLOTE,1)) PROXNUMLOTE
                 , NVL(ESTOQUEPORLOTE,'N') ESTOQUEPORLOTE
                 , PREFIXOLOTE
                   FROM PCPRODUT
                   WHERE CODPROD = " & codprod

                cmd.CommandText = SQL
                datareader = cmd.ExecuteReader()
                datareader.Read()
                numlote = datareader("PROXNUMLOTE").ToString
            Else
                'seleciona o proximo lote da pcconsum se for por filial


                SQL = "SELECT DESCRICAO7 AS TIPO_LOTE FROM PCPRODUT WHERE CODPROD = " & codprod
                cmd.CommandText = SQL
                datareader = cmd.ExecuteReader()
                datareader.Read()
                TIPO_LOTE = datareader("TIPO_LOTE").ToString



                SQL = "SELECT FNC_PROXNUMLOTE(" & codprod & ", TO_DATE('" & DateTime.Parse(dtprevinicio).ToString("dd/MM/yyyy") & "', 'DD/MM/YYYY')) AS PROXNUMLOTE FROM DUAL"

                cmd.CommandText = SQL
                datareader = cmd.ExecuteReader()
                datareader.Read()


                numlote = datareader("PROXNUMLOTE").ToString



                'atualiza a pcconsum

                If TIPO_LOTE <> "JULIANO" And TIPO_LOTE <> "TAMPICO" Then

                    SQL = "UPDATE PCCONSUM
                            SET PROXNUMLOTE = " & numlote + 1

                    cmd.CommandText = SQL
                    cmd.ExecuteNonQuery()

                End If


            End If




                'atualiza peprogop
                SQL = "INSERT INTO PEPROGOP
            ( NUMOP
            , CODFILIAL
            , CODPRODMASTER
            , METODO
            , NUMSEQ
            , QTPRODUZIR
            , DTLANC
            , CODFUNCLANC
            , POSICAO
            , ADEQUACAO


            , QTHORAS
            , NUMLOTE
            , DTPREVINICIO)
    VALUES 
            ( '" & numop & "'
            , " & My.Settings.CodFilialProducao & "
            , '" & codprod & "'
            , '" & metodo & "'
            , '1'
            , TO_NUMBER(TRIM(NVL(REPLACE('" & qtproduzir.ToString.Replace(",", ".") & "','.',','),0)))
            , SYSDATE
            , '" & codfunc & "'
            , 'L'
            , NULL
            , 0
            , '" & numlote & "'
            , to_date('" & dtprevinicio & "', 'DD/MM/YYYY HH24:MI:SS'))"

            cmd.CommandText = SQL
            cmd.ExecuteNonQuery()





            'atualiza obs da op
            SQL = "INSERT INTO PCOBSOP
                     ( NUMOP
                     , OBS
                     , ROTINALANC
                     , CODFUNCLANC
                     , DATALANC)
                VALUES 
                     ( '" & numop & "'
                     , 'ORDEM DE PRODUCAO GERADA COM SUCESSO'
                     , 'SAMP_PROD.EXE'
                     , '" & codfunc & "'
                     , SYSDATE)"

            cmd.CommandText = SQL
            cmd.ExecuteNonQuery()





            'atualiza pcopc
            SQL = "INSERT INTO PCOPC
                     ( NUMOP
                     , NUMOPCENTRAL
                     , CODFILIAL
                     , CODPRODMASTER
                     , METODO
                     , QTPRODUZIR
                     , DTLANC
                     , CODFUNCLANC
                     , POSICAO
                     , NUMLOTE
                     , QTORIGINAL
                     , DTPREVINICIO
                     , REPROCESSO)
                VALUES 
                     ( '" & numop & "'
                     , '" & numop & "'
                     , " & My.Settings.CodFilialProducao & "
                     , '" & codprod & "'
                     , '" & metodo & "'
                     , TO_NUMBER(TRIM(NVL(REPLACE('" & qtproduzir.ToString.Replace(",", ".") & "','.',','),0)))
                     , SYSDATE
                     , '" & codfunc & "'
                     , 'L'
                     , '" & numlote & "'
                     , TO_NUMBER(TRIM(NVL(REPLACE('" & qtproduzir.ToString.Replace(",", ".") & "','.',','),0)))
                     , to_date('" & dtprevinicio & "', 'DD/MM/YYYY HH24:MI:SS')
                     , 'N')"

            cmd.CommandText = SQL
            cmd.ExecuteNonQuery()



            '\\\\\\\\\\\\\\\ FAZ A BUSCA DA FORMULAÇÃO DO ITEM//////////////////

            Dim DT As New DataTable

            DT = BuscarFormula(codprod, metodo, My.Settings.CodFilialProducao, qtproduzir)





            '\\\\\\\\\\\\\\\\\\\\GRAVA ITENS DA OP\\\\\\\\\\\\\\\\\\\\\\\\\\


            Dim codMP As String
            Dim numseq As Integer = 1
            Dim qtnecessidade As Double
            ' loop para gravar itens da OP
            For i = 0 To DT.Rows.Count - 1




                codMP = DT.Rows(i).Item(0).ToString
                qtnecessidade = DT.Rows(i).Item(2).ToString
                qtnecessidade = Math.Round(qtnecessidade, 6)
                'Oratransaction = conexao.BeginTransaction()
                'cmd.Transaction = Oratransaction
                'INSERI OS ITENS DA OPS NAS DEVIDAS TABELAS
                'Try

                'peprogitens
                SQL = "INSERT INTO PEPROGITENS
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
                cmd.CommandText = SQL
                cmd.ExecuteNonQuery()


                'pcopi
                SQL = "INSERT INTO PCOPI
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
                cmd.CommandText = SQL
                cmd.ExecuteNonQuery()

                'pccomposicaofracao
                SQL = "INSERT INTO PCCOMPOSICAOFRACAO
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

                cmd.CommandText = SQL
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

            Oratransaction.Commit()
            MessageBox.Show("OP GERADA COM SUCESSO" & vbCrLf & "OP: " & numop & vbCrLf & "LOTE: " & numlote)

            Dim dtretorno As New DataTable
            dtretorno.Columns.Add("numop")
            dtretorno.Columns.Add("numlote")
            dtretorno.Rows.Add(numop, numlote)

            Return dtretorno
        Catch ex As Exception
            Oratransaction.Rollback()
            MessageBox.Show(ex.Message, "Erro ao gravar OP")
            Exit Function

        End Try





    End Function

    Public Function BuscarFormula(codprod As String, metodo As String, filial As String, qt As Double) As DataTable

        'RETORNA A FORMULAÇÃO DE UM PRODUTO ACABADO OU SEMIACABADO


        Dim dt2 As New DataTable ' Produtos filhos
        If conexao.State = 0 Then ConectaOra()


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
                                ROUND(SUM(A.QT),3) QTNECESSIDADE,
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
    Private Sub btnRequisitarMateriais_Click(sender As Object, e As EventArgs) Handles btnRequisitarMateriais.Click


        If listOrdens2.Items.Count <= 0 Then
            MessageBox.Show("Pelo menos um produto precisa ser incluido na programação antes de simular os materiais.")
            Exit Sub
        End If



        GerarMRP()



        ColorirListaMateriais(Me)
        SalvarPrograma(txtCodPrograma.Text)


    End Sub

    Private Sub btnImprimirInsumos_Click(sender As Object, e As EventArgs) Handles btnImprimirInsumos.Click


        If listOrdens2.Items.Count = 0 Then
            MessageBox.Show("Não existem produtos a imprimir")
            Exit Sub
        End If


        Dim dt = ImprimirInsumos()

        Using frm = New frmRelRequsicaoOP(dt)
            frm.ReportViewer1.LocalReport.DataSources.Add(New Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", dt))
            frm.ReportViewer1.SetDisplayMode(DisplayMode.PrintLayout)
            frm.ReportViewer1.ZoomMode = ZoomMode.PageWidth
            frm.NomeRelatorio = "SeMSys.RelRequisicaoOP.rdlc"
            frm.ReportViewer1.LocalReport.ReportEmbeddedResource = "SeMSys.RelRequisicaoOP.rdlc"
            frm.ShowDialog()
        End Using
    End Sub

    Private Function ImprimirSemiAcabado() As DataTable
        Dim dt = New DataTable
        dt.Columns.Add("vCodigo")
        dt.Columns.Add("vDescricao")
        dt.Columns.Add("vQuantidade", GetType(Decimal))
        dt.Columns.Add("vNumOP")
        dt.Columns.Add("vLote")


        Dim lv As ListView = listMateriais2

        For i = 0 To lv.Items.Count - 1
            If lv.Items(i).SubItems(5).Text = "" Then GoTo proximo
            dt.Rows.Add(lv.Items(i).Text,
                        lv.Items(i).SubItems(1).Text,
                        FormatNumber(lv.Items(i).SubItems(2).Text, 4),
                        lv.Items(i).SubItems(7).Text,
                        lv.Items(i).SubItems(8).Text)
proximo:
        Next

        Return dt
    End Function

    Private Function ImprimirInsumos() As DataTable
        Dim dt = New DataTable
        dt.Columns.Add("vCodigo")
        dt.Columns.Add("vDescricao")
        dt.Columns.Add("vQuantidade", GetType(Decimal))
        dt.Columns.Add("vEstoque", GetType(Decimal))

        Dim lv As ListView = listMateriais2

        For i = 0 To lv.Items.Count - 1

            dt.Rows.Add(lv.Items(i).Text,
                        lv.Items(i).SubItems(1).Text,
                        FormatNumber(lv.Items(i).SubItems(2).Text, 4),
                        FormatNumber(lv.Items(i).SubItems(3).Text, 4))

        Next

        Return dt
    End Function

    Private Sub btnProgramar2_Click(sender As Object, e As EventArgs) Handles btnProgramar2.Click
        Dim CodProd As String
        Dim Metodo As String
        Dim QtProduzir As Double
        Dim codfunc As String = Form1.CodUser
        Dim DtPrevInicio As String
        Dim list As ListView = listOrdens2
        Dim dtretorno As DataTable
        Dim codprograma As String = txtCodPrograma.Text
        'Dim numop, numlote As String

        If list.Items.Count = 0 Then Exit Sub


        For x = 0 To list.Items.Count - 1


            CodProd = list.Items(x).Text
            Metodo = list.Items(x).SubItems(3).Text
            QtProduzir = list.Items(x).SubItems(4).Text
            DtPrevInicio = list.Items(x).SubItems(5).Text
            'numop = list.Items(x).SubItems(9).Text
            'numlote = list.Items(x).SubItems(10).Text

            dtretorno = New DataTable

            If list.Items(x).SubItems(9).Text <> "" Or list.Items(x).Text = "99999" Then
                GoTo proximo
            End If

            dtretorno = GerarProgramacao(CodProd, Metodo, QtProduzir, codfunc, DtPrevInicio)

            For i = 0 To dtretorno.Rows.Count - 1
                list.Items(x).SubItems(9).Text = dtretorno.Rows(i)("numop").ToString
                list.Items(x).SubItems(10).Text = dtretorno.Rows(i)("numlote").ToString
            Next




proximo:
        Next


        If ExistePrograma(codprograma) = True Then
            SalvarPrograma(codprograma)
        Else
            SalvarPrograma(codprograma)
        End If


    End Sub

    Public Function ExistePrograma(codprograma) As Boolean


        cmd.Connection = conexao
        cmd.CommandType = CommandType.Text
        sql = "select count(programa) contagem from smprogramaproducao where programa = " & codprograma
        cmd.CommandText = sql

        Try

            datareader = cmd.ExecuteReader
            datareader.Read()


            If Convert.ToDouble(datareader("contagem").ToString) = 0 Then
                Return False
            Else Return True
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erro")
            Return False
            Exit Function
        End Try





    End Function

    Public Function contarItensPrograma(codprograma) As Integer


        cmd.Connection = conexao
        cmd.CommandType = CommandType.Text
        sql = "select count(programa) contagem from smprogramaproducao where programa = " & codprograma & " and status is null AND TIPO IS NULL"
        cmd.CommandText = sql

        Try

            datareader = cmd.ExecuteReader
            datareader.Read()


            Return Convert.ToDouble(datareader("contagem").ToString)


        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erro")
            Return 0
            Exit Function

        End Try





    End Function

    Public Function contarSubItensPrograma(codprograma) As Integer


        cmd.Connection = conexao
        cmd.CommandType = CommandType.Text
        sql = "select count(programa) contagem from smprogramaproducao where programa = " & codprograma & " and status is null AND TIPO = 'MP'"
        cmd.CommandText = sql

        Try

            datareader = cmd.ExecuteReader
            datareader.Read()


            Return Convert.ToDouble(datareader("contagem").ToString)


        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erro")
            Return 0
            Exit Function

        End Try





    End Function

    Public Sub SalvarPrograma(codprograma As String)




        Dim sql As String
        Dim descricao As String
        Dim codprod As String
        Dim qtproduzir As String
        Dim numop As String
        Dim numlote As String
        Dim horainicial As String
        Dim horafinal As String
        Dim tempototal As String
        Dim metodo As String
        Dim qtunitcx As Integer
        Dim linha As String

        Oratransaction = conexao.BeginTransaction()
        cmd.Transaction = Oratransaction
        cmd.Connection = conexao
        cmd.CommandType = CommandType.Text


        Try




            sql = "DELETE FROM SMPROGRAMAPRODUCAO WHERE PROGRAMA = " & codprograma

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()



            Oratransaction.Commit()


        Catch ex As Exception
            MessageBox.Show("Erro ao salvar o programa." & vbCrLf & ex.Message, "Erro")
            Oratransaction.Rollback()
            Exit Sub
        End Try



        Oratransaction = conexao.BeginTransaction()
        cmd.Transaction = Oratransaction
        cmd.Connection = conexao
        cmd.CommandType = CommandType.Text



        Try

            ' Salva os itens do programa
            For i = 0 To listOrdens2.Items.Count - 1



                codprod = listOrdens2.Items(i).Text
                descricao = listOrdens2.Items(i).SubItems(1).Text
                Dim embalagem = listOrdens2.Items(i).SubItems(2).Text
                qtproduzir = Replace(listOrdens2.Items(i).SubItems(4).Text, ",", ".")
                numop = listOrdens2.Items(i).SubItems(9).Text
                numlote = listOrdens2.Items(i).SubItems(10).Text
                horainicial = Convert.ToDateTime(listOrdens2.Items(i).SubItems(5).Text)
                horafinal = Convert.ToDateTime(listOrdens2.Items(i).SubItems(7).Text)
                tempototal = Replace(Convert.ToDecimal(listOrdens2.Items(i).SubItems(8).Text), ",", ".")
                metodo = listOrdens2.Items(i).SubItems(3).Text
                If listOrdens2.Items(i).SubItems(12).Text = "" Then
                    qtunitcx = 0
                Else
                    qtunitcx = Convert.ToInt32(listOrdens2.Items(i).SubItems(12).Text)
                End If
                linha = listOrdens2.Items(i).SubItems(13).Text



                sql = "insert into smprogramaproducao
                    (codprod
                    ,descricao
                    ,qtproduzir
                    ,numop
                    ,numlote
                    ,horainicial
                    ,horafinal
                    ,tempototal
                    ,programa
                    ,metodo
                    ,qtunitcx
                    ,linha, embalagem)
                    values
                    (
                        '" & codprod & "',
                        '" & descricao & "',
                        " & qtproduzir & ",
                        '" & numop & "',
                        '" & numlote & "',
                        to_date('" & horainicial & "','DD/MM/YYYY HH24:MI:SS'),
                        to_date('" & horafinal & "','DD/MM/YYYY HH24:MI:SS'),
                        " & tempototal & ",
                        '" & codprograma & "',
                        '" & metodo & "',
                        " & qtunitcx & ",
                        '" & linha & "',
                        '" & embalagem & "'
                                        )"

                cmd.CommandText = sql
                cmd.ExecuteNonQuery()


            Next



            If listMateriais2.Items.Count = 0 Then GoTo Fim
            'Salva os subitens do programa
            For i = 0 To listMateriais2.Items.Count - 1



                codprod = listMateriais2.Items(i).Text
                descricao = listMateriais2.Items(i).SubItems(1).Text
                qtproduzir = Replace(listMateriais2.Items(i).SubItems(2).Text, ",", ".")
                numop = listMateriais2.Items(i).SubItems(7).Text
                numlote = listMateriais2.Items(i).SubItems(8).Text
                metodo = listMateriais2.Items(i).SubItems(5).Text



                sql = "insert into smprogramaproducao
                    (codprod
                    ,descricao
                    ,qtproduzir
                    ,numop
                    ,numlote
                    ,programa
                    ,metodo
                    ,tipo)
                    values
                    (
                        '" & codprod & "',
                        '" & descricao & "',
                        " & qtproduzir & ",
                        '" & numop & "',
                        '" & numlote & "',
                        '" & codprograma & "',
                        '" & metodo & "',
                        'MP'
                        )"

                cmd.CommandText = sql
                cmd.ExecuteNonQuery()


            Next
Fim:
            Oratransaction.Commit()
            MessageBox.Show("Programa salvo")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erro")
            Oratransaction.Rollback()
            Exit Sub
        End Try



    End Sub

    Public Sub SalvarItem(codprograma As String, i As Integer)

        Dim sql As String
        Dim descricao As String
        Dim codprod As String
        Dim qtproduzir As String
        Dim numop As String
        Dim numlote As String
        Dim horainicial As Date
        Dim horafinal As Date
        Dim tempototal As Double
        Dim metodo As String
        Dim qtunitcx As Integer
        Dim linha As String
        Dim embalagem As String

        Dim cmd = New OracleCommand
        Oratransaction = conexao.BeginTransaction()
        cmd.Transaction = Oratransaction
        cmd.Connection = conexao
        cmd.CommandType = CommandType.Text

        Try




            codprod = listOrdens2.Items(i).Text
            descricao = listOrdens2.Items(i).SubItems(1).Text
            embalagem = listOrdens2.Items(i).SubItems(2).Text
            qtproduzir = Replace(listOrdens2.Items(i).SubItems(4).Text, ",", ".")
            numop = listOrdens2.Items(i).SubItems(9).Text
            numlote = listOrdens2.Items(i).SubItems(10).Text
            horainicial = Convert.ToDateTime(listOrdens2.Items(i).SubItems(5).Text)
            horafinal = Convert.ToDateTime(listOrdens2.Items(i).SubItems(7).Text)
            tempototal = Convert.ToDouble(listOrdens2.Items(i).SubItems(8).Text)
            metodo = listOrdens2.Items(i).SubItems(3).Text
            If listOrdens2.Items(i).SubItems(12).Text = "" Then
                qtunitcx = 0
            Else
                qtunitcx = Convert.ToInt32(listOrdens2.Items(i).SubItems(12).Text)
            End If
            linha = listOrdens2.Items(i).SubItems(13).Text




            sql = "insert into smprogramaproducao
                    (codprod
                    ,descricao
                    ,qtproduzir
                    ,numop
                    ,numlote
                    ,horainicial
                    ,horafinal
                    ,tempototal
                    ,programa
                    ,metodo
                    ,qtunitcx
                    ,linha)
                    values
                    (
                        '" & codprod & "',
                        '" & descricao & "',
                        " & qtproduzir & ",
                        '" & numop & "',
                        '" & numlote & "',
                        TO_DATE('" & horainicial & "','DD/MM/YYYY HH24:MI:SS'),
                        TO_DATE('" & horafinal & "','DD/MM/YYYY HH24:MI:SS'),
                        :TEMPOTOTAL,
                        '" & codprograma & "',
                        '" & metodo & "',
                        " & qtunitcx & ",
                        '" & linha & "'
                                            )"

            cmd.Parameters.Add("TEMPOTOTAL", tempototal)
            cmd.CommandText = sql
            cmd.ExecuteNonQuery()




            Oratransaction.Commit()
            'MessageBox.Show("Item salvo com sucesso")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erro")
            Oratransaction.Rollback()
            Exit Sub
        End Try
    End Sub

    Public Sub SalvarSubItem(codprograma As String, i As Integer)

        Dim sql As String
        Dim descricao As String
        Dim codprod As String
        Dim qtproduzir As String
        Dim numop As String
        Dim numlote As String
        Dim dtpreviniciosa As String
        Dim metodo As String



        Oratransaction = conexao.BeginTransaction()
        cmd.Transaction = Oratransaction
        cmd.Connection = conexao
        cmd.CommandType = CommandType.Text

        Try




            codprod = listMateriais2.Items(i).Text
            descricao = listMateriais2.Items(i).SubItems(1).Text
            qtproduzir = Replace(listMateriais2.Items(i).SubItems(2).Text, ",", ".")
            numop = listMateriais2.Items(i).SubItems(7).Text
            numlote = listMateriais2.Items(i).SubItems(8).Text
            dtpreviniciosa = listMateriais2.Items(i).SubItems(9).Text
            metodo = listMateriais2.Items(i).SubItems(5).Text




            sql = "insert into smprogramaproducao
                    (codprod
                    ,descricao
                    ,qtproduzir
                    ,numop
                    ,numlote
                    ,programa
                    ,metodo
                    ,dtpreviniciosa
                    ,tipo)
                    values
                    (
                        '" & codprod & "',
                        '" & descricao & "',
                        " & qtproduzir & ",
                        '" & numop & "',
                        '" & numlote & "',
                        '" & codprograma & "',
                        '" & metodo & "',
                        TO_DATE('" & dtpreviniciosa & "','DD/MM/YYYY HH24:MI:SS'),
                        'MP')"

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = "Select max(idprograma) a from smprogramaproducao"
            cmd.CommandText = sql
            datareader = cmd.ExecuteReader()
            datareader.Read()
            Dim idsubitem$ = datareader("a").ToString()
            listMateriais2.Items(i).SubItems(10).Text = idsubitem

            Oratransaction.Commit()
            'MessageBox.Show("Item salvo com sucesso")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erro")
            Oratransaction.Rollback()
            Exit Sub
        End Try
    End Sub

    Public Sub AlterarPrograma(codprograma)


        Dim sql As String
        Dim descricao As String
        Dim codprod As String
        Dim qtproduzir As String
        Dim numop As String
        Dim numlote As String
        Dim horainicial As Date
        Dim horafinal As Date
        Dim tempototal As Double
        Dim metodo As String
        Dim iditem As String
        Dim qtunitcx As Integer
        Dim linha As String






        For x = 0 To listOrdens2.Items.Count - 1


            If listOrdens2.Items(x).SubItems(11).Text = "" Or listOrdens2.Items(x).SubItems(11).Text = 0 Then

                SalvarItem(codprograma, listOrdens2.Items(x).Index)

            End If




        Next




        For x = 0 To listMateriais2.Items.Count - 1


            If listMateriais2.Items(x).SubItems(10).Text = "" Then

                SalvarSubItem(codprograma, listMateriais2.Items(x).Index)

            End If




        Next





        Dim cmd = New OracleCommand
        Oratransaction = conexao.BeginTransaction()
        cmd.Transaction = Oratransaction
        cmd.Connection = conexao
        cmd.CommandType = CommandType.Text


        ' faz o update da programaçao no banco
        Try
            For i = 0 To listOrdens2.Items.Count - 1


                If listOrdens2.Items(i).SubItems(11).Text = Nothing Or listOrdens2.Items(i).SubItems(11).Text = "" Then GoTo semID

                codprod = listOrdens2.Items(i).Text
                descricao = listOrdens2.Items(i).SubItems(1).Text
                qtproduzir = Replace(listOrdens2.Items(i).SubItems(4).Text, ",", ".")
                numop = listOrdens2.Items(i).SubItems(9).Text
                numlote = listOrdens2.Items(i).SubItems(10).Text
                horainicial = Convert.ToDateTime(listOrdens2.Items(i).SubItems(5).Text)
                horafinal = Convert.ToDateTime(listOrdens2.Items(i).SubItems(7).Text)
                tempototal = Convert.ToDouble(listOrdens2.Items(i).SubItems(8).Text)
                metodo = listOrdens2.Items(i).SubItems(3).Text
                iditem = listOrdens2.Items(i).SubItems(11).Text
                If listOrdens2.Items(i).SubItems(12).Text = "" Then
                    qtunitcx = 0
                Else
                    qtunitcx = Convert.ToInt32(listOrdens2.Items(i).SubItems(12).Text)
                End If
                linha = listOrdens2.Items(i).SubItems(13).Text



                sql = "update smprogramaproducao set
                    codprod = '" & codprod & "'
                    ,descricao = '" & descricao & "'
                    ,qtproduzir = " & qtproduzir & "
                    ,numop = '" & numop & "'
                    ,numlote = '" & numlote & "'
                    ,horainicial = TO_DATE('" & horainicial & "','DD/MM/YYYY HH24:MI:SS')
                    ,horafinal = TO_DATE('" & horafinal & "','DD/MM/YYYY HH24:MI:SS')
                    ,tempototal = :TEMPOTOTAL
                    ,programa = '" & codprograma & "'
                    ,metodo = '" & metodo & "'
                    ,qtunitcx = " & qtunitcx & "
                    ,linha = '" & linha & "'
                    
                    where programa = '" & codprograma & "' and idprograma = " & iditem


                cmd.CommandText = sql
                cmd.Parameters.Clear()
                cmd.Parameters.Add("TEMPOTOTAL", tempototal)
                cmd.ExecuteNonQuery()

semID:

            Next


            If listMateriais2.Items.Count = 0 Then GoTo Fim
            ' faz o update dos itens do programa no banco

            For i = 0 To listMateriais2.Items.Count - 1



                codprod = listMateriais2.Items(i).Text
                descricao = listMateriais2.Items(i).SubItems(1).Text
                qtproduzir = Replace(listMateriais2.Items(i).SubItems(2).Text, ",", ".")
                numop = listMateriais2.Items(i).SubItems(7).Text
                numlote = listMateriais2.Items(i).SubItems(8).Text

                metodo = listMateriais2.Items(i).SubItems(5).Text
                iditem = listMateriais2.Items(i).SubItems(10).Text




                sql = "update smprogramaproducao set
                    codprod = '" & codprod & "'
                    ,descricao = '" & descricao & "'
                    ,qtproduzir = " & qtproduzir & "
                    ,numop = '" & numop & "'
                    ,numlote = '" & numlote & "'
                    ,programa = '" & codprograma & "'
                    ,metodo = '" & metodo & "'
                    where programa = '" & codprograma & "' and idprograma = " & iditem

                cmd.CommandText = sql
                cmd.ExecuteNonQuery()


            Next


Fim:
            Oratransaction.Commit()
            MessageBox.Show("Programa alterado com sucesso")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erro")
            Oratransaction.Rollback()
            Exit Sub
        End Try



    End Sub

    Private Sub btnPesquisar_Click_1(sender As Object, e As EventArgs) Handles btnPesquisar.Click

        Dim FRM As New frmPesquisar(Me, txtCodProd2)
        FRM.ShowDialog()

        txtCodProd2.Text = txtcodprod

    End Sub

    Private Sub btnImprimirPrograma_Click(sender As Object, e As EventArgs) Handles btnImprimirPrograma.Click

        'GerarMRP()

        If txtCodPrograma.Text = "" Then
            MessageBox.Show("Não existem produtos a imprimir")
            Exit Sub
        End If

        'Dim ParPrograma As New ReportParameter

        'ParPrograma.Name = "ParProgramaCodigo"
        'ParPrograma.Values.Add(txtCodPrograma.Text)


        Dim dt = ImprimirPrograma(txtCodPrograma.Text)
        Dim dt2 = ImprimirSemiAcabado()

        Using frm = New frmRelRequsicaoOP(dt)


            frm.ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("DataSet1", dt))
            frm.ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("DataSet2", dt2))
            frm.NomeRelatorio = "Rel Programa de Produção"
            frm.ReportViewer1.SetDisplayMode(DisplayMode.PrintLayout)
            frm.ReportViewer1.ZoomMode = ZoomMode.PageWidth
            frm.ReportViewer1.LocalReport.ReportEmbeddedResource = "SeMSys.RelProgramaProducao.rdlc"
            frm.ReportViewer1.LocalReport.SetParameters(New ReportParameter("ParProgramaCodigo", txtCodPrograma.Text))
            frm.ShowDialog()
        End Using
    End Sub

    Private Function ImprimirPrograma(codprograma) As DataTable
        Dim dt = New DataTable
        dt.Columns.Add("vCodigo")
        dt.Columns.Add("vDescricao")
        dt.Columns.Add("vQuantidade", GetType(Decimal))
        dt.Columns.Add("vMetodo")
        dt.Columns.Add("vhrInicial")
        dt.Columns.Add("vHrFinal")
        dt.Columns.Add("vHoraNecessaria", GetType(Decimal))
        dt.Columns.Add("vNumOP")
        dt.Columns.Add("vNumlote")
        dt.Columns.Add("vPrograma")
        dt.Columns.Add("vQtMaster", GetType(Decimal))
        dt.Columns.Add("vLinha")
        dt.Columns.Add("vQtPalete", GetType(Decimal))
        Dim qtmaster As Decimal
        Dim qtpalete As Decimal

        Dim lv As ListView = listOrdens2

        For i = 0 To lv.Items.Count - 1

            If lv.Items(i).Text = "99999" Then
                qtmaster = 0
                qtpalete = 0
            Else
                qtmaster = Convert.ToDecimal(lv.Items(i).SubItems(4).Text) / Convert.ToDecimal(lv.Items(i).SubItems(12).Text)
                sql = "select qttotpal from pcprodut where codprod = " & lv.Items(i).Text

                cmd.Connection = conexao
                cmd.CommandType = CommandType.Text
                cmd.CommandText = sql


                datareader = cmd.ExecuteReader
                datareader.Read()
                qtpalete = Convert.ToDecimal(datareader("qttotpal"))
                qtpalete = FormatNumber(qtmaster / qtpalete, 0)
            End If







            dt.Rows.Add(lv.Items(i).Text,
                        lv.Items(i).SubItems(1).Text,
                        FormatNumber(lv.Items(i).SubItems(4).Text, 4),
                        lv.Items(i).SubItems(3).Text,
                        lv.Items(i).SubItems(5).Text,
                        lv.Items(i).SubItems(7).Text,
                        lv.Items(i).SubItems(8).Text,
                        lv.Items(i).SubItems(9).Text,
                        lv.Items(i).SubItems(10).Text,
                                           codprograma,
                       FormatNumber(qtmaster, 0),
                        lv.Items(i).SubItems(13).Text,
                        qtpalete)
proximo:
        Next

        Return dt
    End Function

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        txtCodProd2.Enabled = True
        Button4.Enabled = False




        Dim sql As String = "SELECT DFSEQ_NOVO_SMPROGRAMAPRODUCAO.nextval FROM DUAL"

        cmd.Connection = conexao
        cmd.CommandText = sql
        cmd.CommandType = CommandType.Text
        Try
            datareader = cmd.ExecuteReader
            datareader.Read()
            txtCodPrograma.Text = datareader("nextval").ToString
            txtCodPrograma.Enabled = False

            If listOrdens2.Items.Count > 0 Then
                listOrdens2.Items.Clear()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erro")
            Exit Sub
        End Try




    End Sub

    Private Sub btnCarregarPrograma_Click(sender As Object, e As EventArgs) Handles btnCarregarPrograma.Click

        If txtCodPrograma.Text = "" Then Exit Sub


        Dim codprograma As String = txtCodPrograma.Text
        Dim lista As ListView = listOrdens2
        Dim ListaSubItens As ListView = listMateriais2
        Dim dt As New DataTable
        Dim dt2 As New DataTable

        lista.Items.Clear()
        ListaSubItens.Items.Clear()

        dt.Columns.Add("codigo")
        dt.Columns.Add("descricao")
        dt.Columns.Add("embalagem")
        dt.Columns.Add("metodo")
        dt.Columns.Add("qtproduzir")
        dt.Columns.Add("datainicial")
        dt.Columns.Add("qtdOps")
        dt.Columns.Add("horafinal")
        dt.Columns.Add("horanecessaria")
        dt.Columns.Add("numop")
        dt.Columns.Add("numlote")
        dt.Columns.Add("idprograma")
        dt.Columns.Add("qtunitcx")
        dt.Columns.Add("linha")

        sql = "select * from smprogramaproducao where programa =" & codprograma & " and status IS NULL AND TIPO IS NULL order by linha,horainicial"

        cmd.Connection = conexao
        cmd.CommandType = CommandType.Text
        cmd.CommandText = sql

        Try

            datareader = cmd.ExecuteReader





            Do While datareader.Read



                dt.Rows.Add(datareader("codprod").ToString,
                                    datareader("descricao").ToString,
                                    "",
                                    datareader("metodo").ToString,
                                    datareader("qtproduzir").ToString,
                                    Convert.ToDateTime(datareader("horainicial")),
                                    "1",
                                    Convert.ToDateTime(datareader("horafinal")),
                                    Convert.ToDecimal(datareader("tempototal").ToString),
                                    datareader("numop").ToString,
                                    datareader("numlote").ToString,
                                    datareader("idprograma").ToString,
                                    datareader("qtunitcx").ToString,
                                    datareader("linha").ToString)

            Loop


            For x = 0 To dt.Rows.Count - 1


                Dim item2 As ListViewItem = lista.Items.Add(dt.Rows(x)("codigo"))
                item2.SubItems.Add(dt.Rows(x)("descricao"))
                item2.SubItems.Add(dt.Rows(x)("embalagem"))
                item2.SubItems.Add(dt.Rows(x)("metodo"))
                item2.SubItems.Add(dt.Rows(x)("qtproduzir"))
                item2.SubItems.Add(dt.Rows(x)("datainicial"))
                item2.SubItems.Add(dt.Rows(x)("qtdOps"))
                item2.SubItems.Add(dt.Rows(x)("horafinal"))
                item2.SubItems.Add(dt.Rows(x)("horanecessaria"))
                item2.SubItems.Add(dt.Rows(x)("numop"))
                item2.SubItems.Add(dt.Rows(x)("numlote"))
                item2.SubItems.Add(dt.Rows(x)("idprograma"))
                item2.SubItems.Add(dt.Rows(x)("qtunitcx"))
                item2.SubItems.Add(dt.Rows(x)("linha"))

                item2.Checked = False



            Next




            '/////  CARREGA OS DADOS DOS SUBITENS DO PROGRAMA

            dt2.Clear()


            dt2.Columns.Add("codigo")
            dt2.Columns.Add("descricao")
            dt2.Columns.Add("metodo")
            dt2.Columns.Add("qtproduzir")
            dt2.Columns.Add("numop")
            dt2.Columns.Add("numlote")
            dt2.Columns.Add("dtpreviniciosa")
            dt2.Columns.Add("idprograma")
            dt2.Columns.Add("qtestoque")


            sql = "select * from smprogramaproducao where programa =" & codprograma & " and status IS NULL AND TIPO IS NOT NULL order by idprograma"

            cmd.Connection = conexao
            cmd.CommandType = CommandType.Text
            cmd.CommandText = sql
            datareader = cmd.ExecuteReader


            Do While datareader.Read

                dt2.Rows.Add(datareader("codprod").ToString,
                                datareader("descricao").ToString,
                                datareader("metodo").ToString,
                                datareader("qtproduzir").ToString,
                                datareader("numop").ToString,
                                datareader("numlote").ToString,
                                datareader("dtpreviniciosa").ToString,
                                datareader("idprograma").ToString)

            Loop


            For x = 0 To dt2.Rows.Count - 1


                Dim item2 As ListViewItem = ListaSubItens.Items.Add(dt2.Rows(x)("codigo"))
                item2.SubItems.Add(dt2.Rows(x)("descricao"))
                item2.SubItems.Add(dt2.Rows(x)("qtproduzir"))
                item2.SubItems.Add("-")
                item2.SubItems.Add("-")
                item2.SubItems.Add(dt2.Rows(x)("metodo"))
                item2.SubItems.Add("-")
                item2.SubItems.Add(dt2.Rows(x)("numop"))
                item2.SubItems.Add(dt2.Rows(x)("numlote"))
                item2.SubItems.Add(dt2.Rows(x)("dtpreviniciosa"))
                item2.SubItems.Add(dt2.Rows(x)("idprograma"))


                item2.Checked = False



            Next


            'ATUALIZAR OS ESTOQUES DOS SUBITEMS

            For x As Integer = 0 To ListaSubItens.Items.Count - 1

                Dim codprod As Double = Convert.ToDouble(ListaSubItens.Items(x).Text)


                Dim sql As String = "SELECT PKG_ESTOQUE.ESTOQUE_DISPONIVEL(:CODPROD, :CODFILIAL, 'V') AS QTDISP FROM DUAL"

                If Not datareader Is Nothing AndAlso Not datareader.IsClosed Then datareader.Close()

                ' Cria um novo comando para cada iteração
                Dim cmd As New OracleCommand()
                cmd.Connection = conexao
                cmd.CommandType = CommandType.Text
                cmd.CommandText = sql

                ' Adiciona parâmetros com tipos de dados e valores
                cmd.Parameters.Add("CODPROD", codprod)
                cmd.Parameters.Add("CODFILIAL", My.Settings.CodFilialEstoque)

                ' Executa o comando e processa o resultado
                datareader = cmd.ExecuteReader()
                If datareader.Read() Then
                    ListaSubItens.Items(x).SubItems(3).Text = datareader("QTDISP").ToString
                End If

                ' Fecha o DataReader
                datareader.Close()
            Next


            For x = 0 To ListaSubItens.Items.Count - 1

                Dim codprod As Double = Convert.ToDouble(ListaSubItens.Items(x).Text)
                Dim sql As String = "SELECT TIPOMERC FROM PCPRODUT WHERE CODPROD =:CODPROD"
                Dim cmd As New OracleCommand()
                cmd.Connection = conexao
                cmd.CommandType = CommandType.Text
                cmd.CommandText = sql

                ' Adiciona parâmetros com tipos de dados e valores
                cmd.Parameters.Add("CODPROD", CodProd)


                ' Executa o comando e processa o resultado
                datareader = cmd.ExecuteReader()
                If datareader.Read() Then
                    ListaSubItens.Items(x).SubItems(4).Text = datareader("TIPOMERC").ToString
                End If

                ' Fecha o DataReader
                datareader.Close()

            Next

            ColorirListaProdutosAcabados(Me)
            ColorirListaMateriais(Me)



        Catch ex As Exception
            MessageBox.Show("Erro ao abrir programa" + vbCrLf + vbCrLf + ex.Message)
            Exit Sub
        End Try






        txtCodProd2.Enabled = True



    End Sub

    Private Sub btnSalvarPrograma_Click(sender As Object, e As EventArgs) Handles btnSalvarPrograma.Click

        Dim codprograma = txtCodPrograma.Text


        Dim sql As String

        If codprograma = "" Then
            MessageBox.Show("Primeiro crie um novo programa clicando no botão + na parte superior da tela.", "Erro")
            Exit Sub
        End If
        Oratransaction = conexao.BeginTransaction()
        cmd.Transaction = Oratransaction
        cmd.Connection = conexao
        cmd.CommandType = CommandType.Text

        Try




            sql = "DELETE FROM SMPROGRAMAPRODUCAO WHERE PROGRAMA = " & codprograma

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()



            Oratransaction.Commit()

            SalvarPrograma(codprograma)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erro")
            Oratransaction.Rollback()
            Exit Sub
        End Try








    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles btnAdicionarIntervalo.Click
        If listOrdens2.Items.Count = 0 Or listOrdens2.SelectedItems.Count = 0 Then Exit Sub


        Dim resposta As DialogResult = MessageBox.Show("Deseja realmente adicionar 1hr de a mais neste item da programação?" & vbCrLf & "A hora final será recalculada.", "Adicionar Hora parada no processo", MessageBoxButtons.YesNo)

        If resposta = DialogResult.No Then Exit Sub





        listOrdens2.FocusedItem.SubItems(8).Text = Convert.ToDouble(listOrdens2.FocusedItem.SubItems(8).Text) + 1

        If listOrdens2.Items.Count > 0 Then
            'ReprogramarOPs(Me)
            AlterarPrograma(txtCodPrograma.Text)
        Else

            AlterarPrograma(txtCodPrograma.Text)
        End If
    End Sub

    Private Sub btnProgramarSA_Click(sender As Object, e As EventArgs) Handles btnProgramarSA.Click
        Dim CodProd As String
        Dim Metodo As String
        Dim QtProduzir As Decimal
        Dim codfunc As String = My.Settings.UsuarioWinthor
        Dim DtPrevInicio As String
        Dim list As ListView = listMateriais2
        Dim dtretorno As DataTable
        Dim codprograma As String = txtCodPrograma.Text
        Dim numop As String
        'Dim numop, numlote As String

        If list.Items.Count = 0 Then Exit Sub


        For x = 0 To list.Items.Count - 1

            Metodo = list.Items(x).SubItems(5).Text
            Dim format As Globalization.NumberFormatInfo = New System.Globalization.NumberFormatInfo()
            format.NumberDecimalSeparator = "."
            QtProduzir = Math.Round(Convert.ToDecimal(list.Items(x).SubItems(2).Text), 3)
            DtPrevInicio = list.Items(x).SubItems(9).Text
            numop = list.Items(x).SubItems(7).Text
            CodProd = list.Items(x).Text

            If numop <> "" Then
                GoTo proximo
            End If

            If list.Items(x).SubItems(5).Text = "" Then
                GoTo proximo
            End If

            dtretorno = New DataTable


            Try


                dtretorno = GerarProgramacao(CodProd, Metodo, QtProduzir, codfunc, DtPrevInicio)

                If dtretorno Is Nothing Then
                    MessageBox.Show("Erro ao gravar OP para o produto:" & vbCrLf & "Codprod: " & CodProd & vbCrLf & vbCrLf & "Nenhuma OP ou Lote foi gerada para o produto.")
                    GoTo proximo
                End If

                For i = 0 To dtretorno.Rows.Count - 1
                    list.Items(x).SubItems(7).Text = dtretorno.Rows(i)("numop").ToString
                    list.Items(x).SubItems(8).Text = dtretorno.Rows(i)("numlote").ToString
                Next

            Catch ex As Exception
                'MessageBox.Show("Erro ao gerar programação do semiacabado" & vbCrLf & "Cod Prod: " & CodProd, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try


proximo:
        Next


        If ExistePrograma(codprograma) = True Then
            SalvarPrograma(codprograma)
        End If

    End Sub


    Friend Sub ReprogramarOPWinthor(numop As String, novaQtProduzir As Double, numlote As String, TipoLote As String, dtprevinicio As String)


        Dim sql = "SELECT NVL(QTPRODUZIR,0) QTPRODUZIR, POSICAO,CODPRODMASTER,DTPREVINICIO FROM PCOPC WHERE NUMOP = :NUMOP"
        Dim dtOP As New DataTable


        ' Primeira instrução SQL
        Using cmd As New OracleCommand
            Dim dr As OracleDataReader
            cmd.CommandType = CommandType.Text
            cmd.Connection = conexao
            cmd.CommandText = sql
            cmd.Parameters.Clear()
            cmd.Parameters.Add("NUMOP", OracleDbType.Varchar2).Value = numop

            dr = cmd.ExecuteReader()
            dtOP.Load(dr)
            dr.Dispose()
            cmd.Dispose()
            If Convert.ToDecimal(dtOP.Rows(0)("QTPRODUZIR")) = novaQtProduzir Then

                If TipoLote = "TAMPICO" Or TipoLote = "JULIANO" Then
                    GoTo Seguir
                End If


                Exit Sub
            End If
Seguir:
            If Convert.ToDecimal(dtOP.Rows(0)("QTPRODUZIR")) = 0 Or dtOP.Rows(0)("POSICAO").ToString = "C" Or dtOP.Rows(0)("POSICAO").ToString = "F" Then


                MessageBox.Show("Não é possível incluir esta OP na programação." & vbCrLf & vbCrLf & "Possíveis causas:" & vbCrLf & "- OP NÃO EXISTENTE" & vbCrLf & "- OP COM QUANTIDADE A PRODUZIR ZERADA" & vbCrLf & "- OP FECHADA OU CANCELADA", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub


            End If
        End Using


        If TipoLote = "TAMPICO" Or TipoLote = "JULIANO" Then


            sql = "SELECT FNC_PROXNUMLOTE(" & dtOP.Rows(0)("CODPRODMASTER").ToString() & ", TO_DATE('" & DateTime.Parse(dtprevinicio).ToString("dd/MM/yyyy") & "', 'DD/MM/YYYY')) AS PROXNUMLOTE FROM DUAL"

            Using cmd As New OracleCommand()
                cmd.Connection = conexao
                cmd.CommandType = CommandType.Text


                Try
                    ' Primeira instrução SQL
                    cmd.CommandText = sql
                    datareader = cmd.ExecuteReader
                    datareader.Read()
                    numlote = datareader("PROXNUMLOTE").ToString


                Catch ex As Exception
                    MessageBox.Show("Erro ao reprogramar OP." & vbCrLf & "Numop: " & numop & vbCrLf & ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)

                    Exit Sub
                End Try
            End Using




        End If

        Dim sql1 As String = "DECLARE
                                    v_status VARCHAR2(32767);
                                BEGIN
                                    v_status := Reprogramar_OP_Func(:NUMOP, :NOVAQT, :NUMLOTE, :DTPREVINICIO);
                                    DBMS_OUTPUT.PUT_LINE(v_status);
                                    :v_status := v_status;
                                END;"


        Using cmd As New OracleCommand()
            cmd.Connection = conexao
            cmd.CommandType = CommandType.Text

            ' Iniciar transação
            conexao.Commit()
            Dim trans As OracleTransaction = conexao.BeginTransaction()
            cmd.Transaction = trans

            Try
                ' Primeira instrução SQL
                cmd.CommandText = sql1
                cmd.Parameters.Clear()
                cmd.Parameters.Add("NUMOP", OracleDbType.Varchar2).Value = numop
                cmd.Parameters.Add("NOVAQT", OracleDbType.Decimal).Value = novaQtProduzir
                cmd.Parameters.Add("NUMLOTE", OracleDbType.Varchar2).Value = numlote
                cmd.Parameters.Add("DTPREVINICIO", OracleDbType.Varchar2).Value = dtprevinicio
                cmd.Parameters.Add(New OracleParameter("v_status", OracleDbType.Varchar2, 32767)).Direction = ParameterDirection.Output
                cmd.ExecuteNonQuery()

                Dim pRetorno As String = cmd.Parameters("v_status").Value.ToString()

                If pRetorno <> "SUCESSO" Then
                    MessageBox.Show("Erro ao reprogramar OP." & vbCrLf & vbCrLf & pRetorno, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    trans.Rollback()
                    Exit Sub
                End If


                ' Commit da transação
                trans.Commit()
                MessageBox.Show("OP reprogramada!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Catch ex As Exception
                MessageBox.Show("Erro ao reprogramar OP." & vbCrLf & "Numop: " & numop & vbCrLf & ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
                trans.Rollback()
                Exit Sub
            End Try
        End Using






    End Sub

    Private Sub txtQtProduzir2_Enter(sender As Object, e As EventArgs) Handles txtQtProduzir2.LostFocus

        Try


            Dim horas As Double
            Dim velocidade_nominal = txtVelocidadeNominal2.Text
            Dim qt As Double = Convert.ToDecimal(Replace(txtQtProduzir2.Text, ".", ",")) * 1
            Dim eficiencia As Double = Convert.ToDouble(txtEficiencia.Text.Trim("%")) / 10000

            horas = Math.Round(qt / velocidade_nominal / eficiencia, 2)

            Dim hora As String = Convert.ToDateTime(DtPickerHoraInicial.Text).ToString("t")


            Dim HoraFinal = DtPickerHoraInicial.Value.AddHours(horas).ToString 'Data Final
            DtPickerHoraFinal.Text = HoraFinal

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try




    End Sub


End Class

