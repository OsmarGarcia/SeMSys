Public Class frmTrocaNF
    Private Sub PesquisarNFs()

        Dim dt As New DataTable

        dt.Columns.Add("CODFILIAL")
        dt.Columns.Add("UF")
        dt.Columns.Add("ESPECIE")
        dt.Columns.Add("SERIE")
        dt.Columns.Add("NUMNOTA")
        dt.Columns.Add("NUMBONUS")
        dt.Columns.Add("DTEMISSAO")
        dt.Columns.Add("DTENT")
        dt.Columns.Add("CODFORNEC")
        dt.Columns.Add("FORNECEDOR")
        dt.Columns.Add("VLTOTAL")
        dt.Columns.Add("FUNCIONARIO")
        dt.Columns.Add("NOME")
        dt.Columns.Add("HORALANC")
        dt.Columns.Add("MINUTOLANC")
        dt.Columns.Add("ROTINA_ALTER")
        dt.Columns.Add("EQUIP_ALTER")
        dt.Columns.Add("FUNC_ALTER")


        Dim SQL As String = "SELECT 
                                A.CODFILIAL,
                                A.UF,
                                A.ESPECIE,
                                A.SERIE,
                                A.NUMNOTA,
                                A.NUMBONUS,
                                A.DTEMISSAO,
                                A.DTENT,
                                A.CODFORNEC,
                                B.FORNECEDOR FORNECEDOR,
                                A.VLTOTAL,
                                A.CODFUNCLANC FUNCIONARIO,
                                C.NOME,
                                A.HORALANC,
                                A.MINUTOLANC,
                                A.ROTINALANC ROTINA_ALTER,
                                A.EQUIPLANC EQUIP_ALTER,
                                A.FUNCLANC FUNC_ALTER
                                FROM PCNFENT A, PCFORNEC B, PCEMPR C
                                WHERE A.CODFORNEC = B.CODFORNEC
                                AND A.CODFUNCLANC = C.MATRICULA
                                AND A.NUMNOTA in ( " & txtNumNota.Text & ")"


        cmd.Connection = conexao
        cmd.CommandText = SQL
        cmd.CommandType = CommandType.Text
        Try
            datareader = cmd.ExecuteReader

            Do While datareader.Read

                dt.Rows.Add(datareader(0).ToString,
                            datareader(1).ToString,
                            datareader(2).ToString,
                            datareader(3).ToString,
                            datareader(4).ToString,
                            datareader(5).ToString,
                            datareader(6).ToString,
                            datareader(7).ToString,
                            datareader(8).ToString,
                            datareader(9).ToString,
                            datareader(10).ToString,
                            datareader(11).ToString,
                            datareader(12).ToString,
                            datareader(13).ToString,
                            datareader(14).ToString,
                            datareader(15).ToString,
                            datareader(16).ToString,
                            datareader(17).ToString)

            Loop


            dgvNF.DataSource = dt
            dgvNF.Update()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erro")
            Exit Sub
        End Try




    End Sub

    Private Sub btnPesquisar_Click(sender As Object, e As EventArgs) Handles btnPesquisar.Click
        PesquisarNFs()
    End Sub



    Private Sub dgvNF_DoubleClick(sender As Object, e As EventArgs) Handles dgvNF.DoubleClick
        Try
            Dim colecao As DataGridViewSelectedRowCollection = dgvNF.SelectedRows
            Dim numnota = colecao(0).Cells(4).Value
            Dim fornec = colecao(0).Cells(8).Value

            Dim form As New frmTipoNF
            form.ShowDialog()


            Dim especie = My.Settings.Item("confEspecieSelecionada").ToString



            Dim ResultadoGerar As DialogResult = MessageBox.Show("Deseja alterar a espécie para " & especie & "?", "Alterar Especie", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If ResultadoGerar = DialogResult.No Then Exit Sub

            sql = "update pcnfent set especie = '" & especie & "' where numnota = '" & numnota & "' and codfornec = '" & fornec & "'"



            Oratransaction = conexao.BeginTransaction(IsolationLevel.ReadCommitted)
            cmd.Connection = conexao
            cmd.CommandText = sql
            cmd.CommandType = CommandType.Text
            cmd.Transaction = Oratransaction
            Try
                datareader = cmd.ExecuteReader()
                Oratransaction.Commit()
                PesquisarNFs()



                MessageBox.Show("Executado com sucesso!", "Sucesso")

            Catch ex As Exception
                MessageBox.Show("Erro ao conectar com o banco. " &
                                 "Messagem de erro: " & ex.Message.ToString())
                Oratransaction.Rollback()
                Exit Sub
            End Try





        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub frmTrocaNF_Load(sender As Object, e As EventArgs) Handles Me.Load
        ConectaOra()
    End Sub
End Class