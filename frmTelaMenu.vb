Imports System.Reflection

Public Class frmTelaMenu
    Public versao As String = Application.ProductVersion.ToString & vbCrLf &
        System.Reflection.Assembly.GetEntryAssembly().GetName.Version.ToString & vbCrLf &
        System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetEntryAssembly().Location).FileVersion.ToString & vbCrLf &
        System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetEntryAssembly().Location).ProductVersion.ToString




    Private Sub AlterarVisibilidadePorTag(parent As ToolStripItem, tagName As String, isVisible As Boolean)
        ' Verifica todos os itens de menu no dropdown do item atual, se houver
        If TypeOf parent Is ToolStripMenuItem Then
            For Each item As ToolStripItem In DirectCast(parent, ToolStripMenuItem).DropDownItems
                ' Verifica se o item é um ToolStripMenuItem
                If TypeOf item Is ToolStripMenuItem Then
                    ' Se for, chama recursivamente para os itens do dropdown
                    Dim dropDownItem As ToolStripMenuItem = DirectCast(item, ToolStripMenuItem)
                    AlterarVisibilidadePorTag(dropDownItem, tagName, isVisible)
                End If

                ' Verifica se o item possui a tag desejada dentro de um conjunto de palavras separadas por vírgula
                If item.Tag IsNot Nothing Then
                    Dim tags As String() = item.Tag.ToString().ToLower().Split(","c)
                    If tags.Contains(tagName.ToLower()) Then
                        item.Visible = isVisible
                    End If
                End If
            Next
        End If

        ' Verifica se o item principal (primeiro nível) possui a tag desejada dentro de um conjunto de palavras separadas por vírgula
        If parent.Tag IsNot Nothing Then
            Dim tags As String() = parent.Tag.ToString().ToLower().Split(","c)
            If tags.Contains(tagName.ToLower()) Then
                parent.Visible = isVisible
            End If
        End If
    End Sub

    ' Método para chamar a função principal passando o MenuStrip
    Private Sub AlterarVisibilidadePorTag(menuStrip As MenuStrip, tagName As String, isVisible As Boolean)
        For Each item As ToolStripItem In menuStrip.Items
            If TypeOf item Is ToolStripMenuItem Then
                Dim menuItem As ToolStripMenuItem = DirectCast(item, ToolStripMenuItem)
                AlterarVisibilidadePorTag(menuItem, tagName, isVisible)
            End If

            ' Verifica se o item principal (primeiro nível) possui a tag desejada dentro de um conjunto de palavras separadas por vírgula
            If item.Tag IsNot Nothing Then
                Dim tags As String() = item.Tag.ToString().ToLower().Split(","c)
                If tags.Contains(tagName.ToLower()) Then
                    item.Visible = isVisible
                End If
            End If
        Next
    End Sub





    Private Sub SepararInsumosParaProduçãoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ts13.Click

        Dim frmSepararProducao As New frmSeparacaoMaterial
        frmSepararProducao.Show()
    End Sub

    Private Sub frmTelaMenu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblUsuario.Text = My.Settings.UsuarioWinthor & " - " & My.Settings.NomeWinthor
        versao = Assembly.GetExecutingAssembly().GetName().Name & " _ " & My.Application.Info.Version.ToString()
        lblVersao.Text = versao

        Me.Text = Me.Text & "              " & lblUsuario.Text




        AlterarVisibilidadePorTag(MenuStrip1, My.Settings.PermissaoUsuario, True)



    End Sub

    Private Sub ProgramarProduçãoToolStripMenuItem_Click(sender As Object, e As EventArgs)

        frmProgramarProducao.Text = frmProgramarProducao.Text & "          " & lblUsuario.Text
        frmProgramarProducao.Show()
    End Sub

    Private Sub frmTelaMenu_Closed(sender As Object, e As EventArgs) Handles Me.Closed



        Application.Exit()


    End Sub

    Private Sub MonitoramentoOnlineToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ts15.Click

        Dim frm As New FrmMonitorOnline

        frm.Show()

    End Sub

    Private Sub JustificarParadasDeLinhaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ts16.Click
        Dim FRM = New frmJustificarParadasvb

        FRM.ShowDialog()
    End Sub

    Private Sub RequisiçaoDeMateriaisDeConsumoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ts3.Click
        Form2.Show()
    End Sub

    Private Sub IniciarMonitoramentoOnlineToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ts14.Click
        Dim r As New frmMonitor
        r.ShowDialog()
    End Sub


    Private Sub ts41_Click(sender As Object, e As EventArgs) Handles ts41.Click
        Dim frm As New frmProgramarProducao
        frm.Show()

    End Sub

    Private Sub ConfiguraçõesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConfiguraçõesToolStripMenuItem.Click
        Dim frm As New frmConfig
        frm.show()
    End Sub

    Private Sub RelResumoDeOPsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RelResumoDeOPsToolStripMenuItem.Click


        Dim frm As New frmResumoOP
        frm.Show()

    End Sub

    Private Sub ts174_Click(sender As Object, e As EventArgs) Handles ts174.Click
        Dim frm As New frmParadasOP
        frm.Show()
    End Sub

    Private Sub ts171_Click(sender As Object, e As EventArgs) Handles ts171.Click
        Dim frm As New frmRelProducaoTotalWinthor
        frm.Show()
    End Sub

    Private Sub MaintSystemToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MaintSystemToolStripMenuItem.Click
        Dim frm As New frmSolicitacaoServico
        frm.Show()
    End Sub

    Private Sub ts47_Click(sender As Object, e As EventArgs) Handles ts47.Click
        Dim frm As New frmManutencaoOP
        frm.Show()
    End Sub

    Private Sub TROToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TROToolStripMenuItem.Click
        Dim frm As New frmTrocaNF
        frm.Show()
    End Sub

    Private Sub RelOrdemDeProduçãoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RelOrdemDeProduçãoToolStripMenuItem.Click

        Dim frm As New frmOrdemProducao
        frm.Show()
    End Sub
End Class