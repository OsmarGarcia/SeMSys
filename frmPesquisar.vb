Public Class frmPesquisar

    Private Property form
    Private Property textbox
    Private Property codigo
    Sub New(form, textbox)


        InitializeComponent()

        Me.form = form
        Me.textbox = textbox
    End Sub
    Private Sub frmPesquisar_Load(sender As Object, e As EventArgs) Handles MyBase.Load



        Dim lv As ListView
        Dim item As New ListViewItem
        lv = ListView1

        lv.GridLines = True
        lv.FullRowSelect = True
        lv.View = View.Details
        lv.Columns.Add("Cod Prod").Width() = 80
        lv.Columns.Add("Descricao").Width() = 500





    End Sub

    Private Sub btnPesquisar_Click(sender As Object, e As EventArgs) Handles btnPesquisar.Click
        Dim lv As ListView = ListView1
        'rs.Close()

        sql = "select codprod, descricao 
               from pcprodut where descricao like '%" & txtDescricao.Text.ToUpper & "%'
               and obs2 not in ('FL') 
                and tipomerc in ('PA','SA','MP','EM')"

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

        lv.Items.Clear()


        Do While datareader.Read()

            Dim item As ListViewItem = lv.Items.Add(UCase(datareader("codprod").ToString))
            item.SubItems.Add(UCase(datareader("descricao").ToString))
        Loop


        'rs.Close()


    End Sub



    Private Sub ListView1_DoubleClick(sender As Object, e As EventArgs) Handles ListView1.DoubleClick

        Dim codprod As String = ListView1.SelectedItems.Item(0).Text

        If Me.form.name = "frmProgramarProducao" Then
            form.codprod = codprod
            form.CapturarDadosProduto()
        ElseIf Me.form.name = "frmManutencaoOP" Then
            form.codprodreserva = codprod
            form.txtdescricaoreserva = ListView1.SelectedItems.Item(0).SubItems(1).Text

        End If



        Me.Dispose()


    End Sub


End Class