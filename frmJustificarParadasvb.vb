Imports System.Data.SqlClient
Imports System.Linq.Expressions

Public Class frmJustificarParadasvb
    Private Sub frmJustificarParadasvb_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If conexaoSQL.State = 0 Then ConectaSRVSQL()

        dtpDataFinal.Format = DateTimePickerFormat.Custom
        dtpDataFinal.CustomFormat = "dd/MM/yyyy HH:mm:ss"

        dtpDataIncial.Format = DateTimePickerFormat.Custom
        dtpDataIncial.CustomFormat = "dd/MM/yyyy HH:mm:ss"
    End Sub

    Private Sub btnCarregarParadaTurno_Click(sender As Object, e As EventArgs) 
        Dim sql = "SELECT
                    TIPODEFALHA AS 'TIPO DE FALHA',
                    FORMAT(DATA, 'dd/MM/yyyy') DATA,
                    FORMAT(HORAINICIAL,'HH:mm') AS 'HR INICIAL',
                    FORMAT(HORAFINAL,'HH:mm') AS 'HR FINAL',
                    FLOOR(DATEDIFF(S,HORAINICIAL,HORAFINAL) * 0.0166666667) AS  'MIN PARADO',
                    ROUND(DATEDIFF(s,HORAINICIAL,HORAFINAL) * 0.0002777777,2) AS 'HORAS PARADAS',
                    Codigo
                    FROM BDADOS_REGISTROS 
                    WHERE LINHA = 'M40'
                    --AND OP IN (SELECT NUMOP FROM TBL_EMPRODUCAO WHERE LINHA = 3)
                    AND CASE
                    WHEN DATEPART(HOUR,DATA) >= 6 AND DATEPART(HOUR,DATA)  <= 13 THEN '1 TURNO'
                    WHEN DATEPART(HOUR,DATA) >= 14 AND DATEPART(HOUR,DATA)  <= 21 THEN '2 TURNO'
                    WHEN DATEPART(HOUR,DATA) >= 22 AND DATEPART(HOUR,DATA)  <= 05 THEN '3 TURNO'
                    ELSE '4 TURNO'
                    END = (SELECT CASE
                    WHEN DATEPART(HOUR,sysdatetime()) >= 6 AND DATEPART(HOUR,sysdatetime())  <= 13 THEN '1 TURNO'
                    WHEN DATEPART(HOUR,sysdatetime()) >= 14 AND DATEPART(HOUR,sysdatetime())  <= 21 THEN '2 TURNO'
                    WHEN DATEPART(HOUR,sysdatetime()) >= 22 AND DATEPART(HOUR,sysdatetime())  <= 05 THEN '3 TURNO'
                    ELSE '4 TURNO'
                    END)
                    AND FORMAT(HORAINICIAL, 'dd/MM/yyyy') = FORMAT(SYSDATETIME(), 'dd/MM/yyyy')
                    ORDER BY DATEDIFF(SECOND,HORAINICIAL,HORAFINAL) DESC"




        Dim dt = New DataTable
        dt.Columns.Add("tipodefalha")
        dt.Columns.Add("data")
        dt.Columns.Add("horainicio")
        dt.Columns.Add("horafim")
        dt.Columns.Add("minuto")
        dt.Columns.Add("hora")
        dt.Columns.Add("Codigo")



        Dim cmd = conexaoSQL.CreateCommand
        cmd.CommandText = sql


        dt.Rows.Clear()

        SQLdr = cmd.ExecuteReader


        If SQLdr.HasRows Then


            Do While SQLdr.Read()

                dt.Rows.Add(
                    SQLdr("TIPO DE FALHA").ToString(),
                    SQLdr("DATA").ToString(),
                    SQLdr("HR INICIAL").ToString(),
                    SQLdr("HR FINAL").ToString(),
                    SQLdr("MIN PARADO").ToString(),
                    SQLdr("HORAS PARADAS").ToString(),
                    SQLdr("Codigo").ToString())




            Loop


            DataGridView1.DataSource = dt
            DataGridView1.Update()



        End If

        SQLdr.Close()

    End Sub
    Public Sub PesquisarOutroTurno(datainicial, datafinal)

        Dim cmd = New SqlCommand
        Dim Sqldr As SqlDataReader


        Dim sql = "SELECT
                    CODIGO,
                    HORAINICIAL,
                    HORAFINAL,
                    OPERADOR,
                    ROUND(TEMPOTOTAL,2) TEMPOTOTAL,
                    AREA,
                    EQUIPAMENTO,
                    COMPONENTE,
                    TIPODEFALHA,
                    MOTIVO
                    FROM BDADOS_REGISTROS
                    WHERE
                    HORAINICIAL BETWEEN CONVERT(datetime,@datainicial, 103) 
                                        AND CONVERT(datetime,@datafinal, 103)
                    AND TIPODEFALHA NOT IN (
						                    'HORA PRODUTIVA',
						                    'HORA PRODUTIVA RODANDO ABAIXO DA NOMINAL',
						                    'VARIACAO DE RITMO')

                    ORDER BY HORAINICIAL DESC"




        Dim dt = New DataTable


        cmd = conexaoSQL.CreateCommand
        cmd.CommandType = CommandType.Text
        cmd.CommandText = sql
        cmd.Parameters.Add(New SqlParameter("@datainicial", SqlDbType.VarChar)).Value = datainicial
        cmd.Parameters.Add(New SqlParameter("@datafinal", SqlDbType.VarChar)).Value = datafinal


        Try

            Sqldr = cmd.ExecuteReader

            dt.Rows.Clear()

            dt.Load(Sqldr)

            DataGridView1.DataSource = dt
            DataGridView1.Update()

        Catch ex As Exception

            MessageBox.Show(ex.Message)
        Finally

            If Sqldr.IsClosed = 0 Then Sqldr.Close()
        End Try


    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim datainicial = dtpDataIncial.Text
        Dim datafinal = dtpDataFinal.Text

        PesquisarOutroTurno(datainicial, datafinal)
    End Sub

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        Dim sel As DataGridViewSelectedRowCollection = DataGridView1.SelectedRows
        Dim codigo = sel(0).Cells(0).Value.ToString

        Dim frm As New frmSelecionarParada(codigo)
        frm.ShowDialog()

    End Sub


    Private Sub CarregarParadasNaoJustificadas()

        Dim sql = "SELECT
                    TIPODEFALHA AS 'TIPO DE FALHA',
                    FORMAT(DATA, 'dd/MM/yyyy') DATA,
                    FORMAT(HORAINICIAL,'HH:mm') AS 'HR INICIAL',
                    FORMAT(HORAFINAL,'HH:mm') AS 'HR FINAL',
                    FLOOR(DATEDIFF(S,HORAINICIAL,HORAFINAL) * 0.0166666667) AS  'MIN PARADO',
                    ROUND(DATEDIFF(s,HORAINICIAL,HORAFINAL) * 0.0002777777,2) AS 'HORAS PARADAS',
                    Codigo
                    FROM BDADOS_REGISTROS 
                    WHERE LINHA = 'M40'
                    AND OP IN (SELECT NUMOP FROM TBL_EMPRODUCAO WHERE LINHA = 3)
                    AND CASE
                    WHEN DATEPART(HOUR,DATA) >= 6 AND DATEPART(HOUR,DATA)  <= 13 THEN '1 TURNO'
                    WHEN DATEPART(HOUR,DATA) >= 14 AND DATEPART(HOUR,DATA)  <= 21 THEN '2 TURNO'
                    WHEN DATEPART(HOUR,DATA) >= 22 AND DATEPART(HOUR,DATA)  <= 05 THEN '3 TURNO'
                    ELSE '4 TURNO'
                    END = (SELECT CASE
                    WHEN DATEPART(HOUR,sysdatetime()) >= 6 AND DATEPART(HOUR,sysdatetime())  <= 13 THEN '1 TURNO'
                    WHEN DATEPART(HOUR,sysdatetime()) >= 14 AND DATEPART(HOUR,sysdatetime())  <= 21 THEN '2 TURNO'
                    WHEN DATEPART(HOUR,sysdatetime()) >= 22 AND DATEPART(HOUR,sysdatetime())  <= 05 THEN '3 TURNO'
                    ELSE '4 TURNO'
                    END)
                    AND FORMAT(HORAINICIAL, 'dd/MM/yyyy') = FORMAT(SYSDATETIME(), 'dd/MM/yyyy')
                    AND TIPODEFALHA = 'PARADA NAO DEFINIDA'
                    ORDER BY DATEDIFF(SECOND,HORAINICIAL,HORAFINAL) DESC"




        Dim dt = New DataTable
        dt.Columns.Add("tipodefalha")
        dt.Columns.Add("data")
        dt.Columns.Add("horainicio")
        dt.Columns.Add("horafim")
        dt.Columns.Add("minuto")
        dt.Columns.Add("hora")
        dt.Columns.Add("Codigo")



        Dim cmd = conexaoSQL.CreateCommand
        cmd.CommandText = sql


        dt.Rows.Clear()

        SQLdr = cmd.ExecuteReader


        If SQLdr.HasRows Then


            Do While SQLdr.Read()

                dt.Rows.Add(
                    SQLdr("TIPO DE FALHA").ToString(),
                    SQLdr("DATA").ToString(),
                    SQLdr("HR INICIAL").ToString(),
                    SQLdr("HR FINAL").ToString(),
                    SQLdr("MIN PARADO").ToString(),
                    SQLdr("HORAS PARADAS").ToString(),
                    SQLdr("Codigo").ToString())




            Loop


            DataGridView1.DataSource = dt
            DataGridView1.Update()



        End If

        SQLdr.Close()

    End Sub



    Private Sub CarregarParadasJustificadas()

        Dim sql = "SELECT
                    TIPODEFALHA AS 'TIPO DE FALHA',
                    FORMAT(DATA, 'dd/MM/yyyy') DATA,
                    FORMAT(HORAINICIAL,'HH:mm') AS 'HR INICIAL',
                    FORMAT(HORAFINAL,'HH:mm') AS 'HR FINAL',
                    FLOOR(DATEDIFF(S,HORAINICIAL,HORAFINAL) * 0.0166666667) AS  'MIN PARADO',
                    ROUND(DATEDIFF(s,HORAINICIAL,HORAFINAL) * 0.0002777777,2) AS 'HORAS PARADAS',
                    Codigo
                    FROM BDADOS_REGISTROS 
                    WHERE LINHA = 'M40'
                    AND OP IN (SELECT NUMOP FROM TBL_EMPRODUCAO WHERE LINHA = 3)
                    AND CASE
                    WHEN DATEPART(HOUR,DATA) >= 6 AND DATEPART(HOUR,DATA)  <= 13 THEN '1 TURNO'
                    WHEN DATEPART(HOUR,DATA) >= 14 AND DATEPART(HOUR,DATA)  <= 21 THEN '2 TURNO'
                    WHEN DATEPART(HOUR,DATA) >= 22 AND DATEPART(HOUR,DATA)  <= 05 THEN '3 TURNO'
                    ELSE '4 TURNO'
                    END = (SELECT CASE
                    WHEN DATEPART(HOUR,sysdatetime()) >= 6 AND DATEPART(HOUR,sysdatetime())  <= 13 THEN '1 TURNO'
                    WHEN DATEPART(HOUR,sysdatetime()) >= 14 AND DATEPART(HOUR,sysdatetime())  <= 21 THEN '2 TURNO'
                    WHEN DATEPART(HOUR,sysdatetime()) >= 22 AND DATEPART(HOUR,sysdatetime())  <= 05 THEN '3 TURNO'
                    ELSE '4 TURNO'
                    END)
                    AND FORMAT(HORAINICIAL, 'dd/MM/yyyy') = FORMAT(SYSDATETIME(), 'dd/MM/yyyy')
                    AND TIPODEFALHA NOT IN ('PARADA NAO DEFINIDA','HORA PRODUTIVA','VARIACAO DE RITMO')
                    ORDER BY DATEDIFF(SECOND,HORAINICIAL,HORAFINAL) DESC"




        Dim dt = New DataTable
        dt.Columns.Add("tipodefalha")
        dt.Columns.Add("data")
        dt.Columns.Add("horainicio")
        dt.Columns.Add("horafim")
        dt.Columns.Add("minuto")
        dt.Columns.Add("hora")
        dt.Columns.Add("Codigo")



        Dim cmd = conexaoSQL.CreateCommand
        cmd.CommandText = sql


        dt.Rows.Clear()

        SQLdr = cmd.ExecuteReader


        If SQLdr.HasRows Then


            Do While SQLdr.Read()

                dt.Rows.Add(
                    SQLdr("TIPO DE FALHA").ToString(),
                    SQLdr("DATA").ToString(),
                    SQLdr("HR INICIAL").ToString(),
                    SQLdr("HR FINAL").ToString(),
                    SQLdr("MIN PARADO").ToString(),
                    SQLdr("HORAS PARADAS").ToString(),
                    SQLdr("Codigo").ToString())




            Loop


            DataGridView1.DataSource = dt
            DataGridView1.Update()



        End If

        SQLdr.Close()

    End Sub



    Private Sub CarregarVariacaoDeRitmo()

        Dim sql = "SELECT
                    TIPODEFALHA AS 'TIPO DE FALHA',
                    FORMAT(DATA, 'dd/MM/yyyy') DATA,
                    FORMAT(HORAINICIAL,'HH:mm') AS 'HR INICIAL',
                    FORMAT(HORAFINAL,'HH:mm') AS 'HR FINAL',
                    FLOOR(DATEDIFF(S,HORAINICIAL,HORAFINAL) * 0.0166666667) AS  'MIN PARADO',
                    ROUND(DATEDIFF(s,HORAINICIAL,HORAFINAL) * 0.0002777777,2) AS 'HORAS PARADAS',
                    Codigo
                    FROM BDADOS_REGISTROS 
                    WHERE LINHA = 'M40'
                    AND OP IN (SELECT NUMOP FROM TBL_EMPRODUCAO WHERE LINHA = 3)
                    AND CASE
                    WHEN DATEPART(HOUR,DATA) >= 6 AND DATEPART(HOUR,DATA)  <= 13 THEN '1 TURNO'
                    WHEN DATEPART(HOUR,DATA) >= 14 AND DATEPART(HOUR,DATA)  <= 21 THEN '2 TURNO'
                    WHEN DATEPART(HOUR,DATA) >= 22 AND DATEPART(HOUR,DATA)  <= 05 THEN '3 TURNO'
                    ELSE '4 TURNO'
                    END = (SELECT CASE
                    WHEN DATEPART(HOUR,sysdatetime()) >= 6 AND DATEPART(HOUR,sysdatetime())  <= 13 THEN '1 TURNO'
                    WHEN DATEPART(HOUR,sysdatetime()) >= 14 AND DATEPART(HOUR,sysdatetime())  <= 21 THEN '2 TURNO'
                    WHEN DATEPART(HOUR,sysdatetime()) >= 22 AND DATEPART(HOUR,sysdatetime())  <= 05 THEN '3 TURNO'
                    ELSE '4 TURNO'
                    END)
                    AND FORMAT(HORAINICIAL, 'dd/MM/yyyy') = FORMAT(SYSDATETIME(), 'dd/MM/yyyy')
                    AND TIPODEFALHA = 'VARIACAO DE RITMO'
                    ORDER BY DATEDIFF(SECOND,HORAINICIAL,HORAFINAL) DESC"




        Dim dt = New DataTable
        dt.Columns.Add("tipodefalha")
        dt.Columns.Add("data")
        dt.Columns.Add("horainicio")
        dt.Columns.Add("horafim")
        dt.Columns.Add("minuto")
        dt.Columns.Add("hora")
        dt.Columns.Add("Codigo")



        Dim cmd = conexaoSQL.CreateCommand
        cmd.CommandText = sql


        dt.Rows.Clear()

        SQLdr = cmd.ExecuteReader


        If SQLdr.HasRows Then


            Do While SQLdr.Read()

                dt.Rows.Add(
                    SQLdr("TIPO DE FALHA").ToString(),
                    SQLdr("DATA").ToString(),
                    SQLdr("HR INICIAL").ToString(),
                    SQLdr("HR FINAL").ToString(),
                    SQLdr("MIN PARADO").ToString(),
                    SQLdr("HORAS PARADAS").ToString(),
                    SQLdr("Codigo").ToString())




            Loop


            DataGridView1.DataSource = dt
            DataGridView1.Update()



        End If

        SQLdr.Close()

    End Sub


    Private Sub CarregarHoraProdutiva()

        Dim sql = "SELECT
                    TIPODEFALHA AS 'TIPO DE FALHA',
                    FORMAT(DATA, 'dd/MM/yyyy') DATA,
                    FORMAT(HORAINICIAL,'HH:mm') AS 'HR INICIAL',
                    FORMAT(HORAFINAL,'HH:mm') AS 'HR FINAL',
                    FLOOR(DATEDIFF(S,HORAINICIAL,HORAFINAL) * 0.0166666667) AS  'MIN PARADO',
                    ROUND(DATEDIFF(s,HORAINICIAL,HORAFINAL) * 0.0002777777,2) AS 'HORAS PARADAS',
                    Codigo
                    FROM BDADOS_REGISTROS 
                    WHERE LINHA = 'M40'
                    AND OP IN (SELECT NUMOP FROM TBL_EMPRODUCAO WHERE LINHA = 3)
                    AND CASE
                    WHEN DATEPART(HOUR,DATA) >= 6 AND DATEPART(HOUR,DATA)  <= 13 THEN '1 TURNO'
                    WHEN DATEPART(HOUR,DATA) >= 14 AND DATEPART(HOUR,DATA)  <= 21 THEN '2 TURNO'
                    WHEN DATEPART(HOUR,DATA) >= 22 AND DATEPART(HOUR,DATA)  <= 05 THEN '3 TURNO'
                    ELSE '4 TURNO'
                    END = (SELECT CASE
                    WHEN DATEPART(HOUR,sysdatetime()) >= 6 AND DATEPART(HOUR,sysdatetime())  <= 13 THEN '1 TURNO'
                    WHEN DATEPART(HOUR,sysdatetime()) >= 14 AND DATEPART(HOUR,sysdatetime())  <= 21 THEN '2 TURNO'
                    WHEN DATEPART(HOUR,sysdatetime()) >= 22 AND DATEPART(HOUR,sysdatetime())  <= 05 THEN '3 TURNO'
                    ELSE '4 TURNO'
                    END)
                    AND FORMAT(HORAINICIAL, 'dd/MM/yyyy') = FORMAT(SYSDATETIME(), 'dd/MM/yyyy')
                    AND TIPODEFALHA in ('HORA PRODUTIVA','HORA PRODUTIVA RODANDO ABAIXO DA NOMINAL')
                    ORDER BY DATEDIFF(SECOND,HORAINICIAL,HORAFINAL) DESC"




        Dim dt = New DataTable
        dt.Columns.Add("tipodefalha")
        dt.Columns.Add("data")
        dt.Columns.Add("horainicio")
        dt.Columns.Add("horafim")
        dt.Columns.Add("minuto")
        dt.Columns.Add("hora")
        dt.Columns.Add("Codigo")



        Dim cmd = conexaoSQL.CreateCommand
        cmd.CommandText = sql


        dt.Rows.Clear()

        SQLdr = cmd.ExecuteReader


        If SQLdr.HasRows Then


            Do While SQLdr.Read()

                dt.Rows.Add(
                    SQLdr("TIPO DE FALHA").ToString(),
                    SQLdr("DATA").ToString(),
                    SQLdr("HR INICIAL").ToString(),
                    SQLdr("HR FINAL").ToString(),
                    SQLdr("MIN PARADO").ToString(),
                    SQLdr("HORAS PARADAS").ToString(),
                    SQLdr("Codigo").ToString())




            Loop


            DataGridView1.DataSource = dt
            DataGridView1.Update()



        End If

        SQLdr.Close()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)
        CarregarParadasNaoJustificadas()

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) 
        CarregarParadasJustificadas()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) 
        CarregarVariacaoDeRitmo()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) 
        CarregarHoraProdutiva()
    End Sub


End Class