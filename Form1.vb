

Imports System.Data.SqlClient

Public Class Form1
    Public nome As String
    Public codigoUsuario As String
    Public CodUser As String
    Public NameUser As String




    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Application.Exit()
    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'On Error Resume Next

        Dim Usuario1 As String = ""
        Dim Senha1 As String
        Dim Usuario2 As String = ""
        Dim Senha2 As String = ""
        'Dim Nome As Integer
        'Dim MaintSystem = Form_MaintSystem()
        ' Codigo As Integer

        Usuario1 = UCase(Me.TextBox1.Text)
        Senha1 = Me.TextBox2.Text.ToUpper

        If Usuario1 <> "" And Senha1 <> "" Then

            sql = "SELECT DECRYPT(SENHABD, USUARIOBD) PASSWORD, MATRICULA, USUARIOBD, NOME
                      FROM PCEMPR
                     WHERE USUARIOBD = '" & Usuario1 & "'"


            cmd.Connection = conexao
            cmd.CommandText = sql
            cmd.CommandType = CommandType.Text



            Try
                datareader = cmd.ExecuteReader()

            Catch ex As Exception
                MessageBox.Show("Erro ao conectar com o banco. " &
                             "Messagem de erro: " & ex.Message.ToString())
                Exit Sub
            End Try

            Do While datareader.Read

                Usuario2 = datareader("USUARIOBD").ToString
                Senha2 = datareader("PASSWORD").ToString
                nome = datareader("NOME").ToString
                codigoUsuario = datareader("MATRICULA").ToString



            Loop




            If Usuario1 = Usuario2 Then

                If Senha1 = Senha2 Then
                    ' MessageBox.Show("Acesso Autorizado.")
                    CodUser = codigoUsuario
                    NameUser = nome
                    My.Settings.UsuarioWinthor = CodUser
                    My.Settings.NomeWinthor = NameUser
                    My.Settings.PermissaoUsuario = ""





                    Dim cmdsql As New SqlCommand
                    cmdsql.CommandType = CommandType.Text
                    cmdsql.CommandText = "SELECT PERMISSAO FROM USUARIOS WHERE CODWINTHOR = @COD"
                    cmdsql.Parameters.AddWithValue("@COD", CodUser)
                    If conexaoSQL.State = 0 Then ConectaSRVSQL()
                    cmdsql.Connection = conexaoSQL
                        Try
                            Dim sqldr As SqlDataReader = cmdsql.ExecuteReader
                        sqldr.Read()
                        My.Settings.PermissaoUsuario = sqldr("PERMISSAO").ToString
                    Catch ex As Exception
                        MessageBox.Show("Você não tem permissões definidas." & vbCrLf & vbCrLf & "Para obter acesso a demais funcionalidades, solicite acesso aos gestores do sistema." & vbCrLf & vbCrLf & "Você acessará com limitação as funcionalidades do sistema.")
                    End Try








                    Me.Hide()
                    'MaintSystem.Show()
                    frmTelaMenu.Show()
                Else

                    MessageBox.Show("Não foi possível efetuar o login. Senha inválida.", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            Else
                MessageBox.Show("Usuário não encontrado.", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Else

            'Dim MsgBoxEmBranco As DialogResult = MessageBox.Show("Preencha todos os campos.", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information)
            MessageBox.Show("Preencha todos os campos.", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If


    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label4.Text = "Versao: " & System.Reflection.Assembly.GetEntryAssembly().GetName.Version.ToString
        ConectaOra()
    End Sub


End Class
