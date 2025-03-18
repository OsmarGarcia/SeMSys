Imports Microsoft.SqlServer
Imports Oracle.ManagedDataAccess.Client
Imports System.Data.SqlClient






Module mdlConexaoOracle


#If DEBUG Then
    Public ip As String = "192.168.0.20"
#Else
       Public ip As String = "192.168.0.20" 
#End If
    Public conexaoSQL As New SqlConnection("Initial Catalog=SampaioBD;" &
                                                "Data Source=SRVSQL,49172;User=sa;Password=520Qu31202;MultipleActiveResultSets=true")
    Public SQLCmd As SqlCommand
    Public SQLdr As SqlDataReader
    Public SQLad As SqlDataAdapter
    Public Oratransaction As OracleTransaction
    'Public rs As New ADODB.Recordset
    Public cmd As New OracleCommand
    Public datareader As OracleDataReader
    Public conexao As New OracleConnection
    Public sql As String = ""

    Public strConexao = "Data Source=(DESCRIPTION=" &
                           "(ADDRESS_LIST=" &
                           "(ADDRESS=(PROTOCOL=TCP)(HOST=" & ip & ")(PORT=1521))" &
                           ")" &
                           "(CONNECT_DATA=" &
                           "(SID=WINT)" &
                           ")" &
                           ");User Id=sampaio;Password=samp42o87"

    'M4N4GER98SPLSH BDTESTE

    Sub ConectaOra()



        Try
            If conexao.State() = 1 Then
                Exit Sub
            ElseIf conexao.State() = 0 Then
                conexao.ConnectionString = strConexao
                conexao.Open()
            End If
        Catch ex As Exception
            MessageBox.Show("Erro ao conectar com o banco. " & ip & vbCrLf &
                             "Messagem de erro: " & ex.Message.ToString() & "    O aplicativo será encerrado.")

            Application.Exit()
        End Try



    End Sub



    Sub ConectaSRVSQL()



        Try
            If conexaoSQL.State() = 1 Then
                Exit Sub
            ElseIf conexaoSQL.State() = 0 Then

                conexaoSQL.Open()




            End If
        Catch ex As Exception
            MessageBox.Show("Erro ao conectar com o banco. " &
                             "Messagem de erro: " & ex.Message.ToString() & "    O aplicativo será encerrado.")

            Application.Exit()
        End Try



    End Sub

End Module
