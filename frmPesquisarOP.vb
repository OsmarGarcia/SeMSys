Imports System.Data.SqlClient
Imports Oracle.ManagedDataAccess.Client

Public Class frmPesquisarOP

    Dim frm
    Sub New(frm)

        ' Esta chamada é requerida pelo designer.
        InitializeComponent()


        Me.frm = frm

    End Sub
    Private Sub frmPesquisarOP_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        Dim sql As String = "SELECT A.NUMOP,A.NUMLOTE,A.CODPRODMASTER,B.DESCRICAO, A.QTPRODUZIR, A.POSICAO
                            FROM PCOPC A, PCPRODUT B WHERE A.CODPRODMASTER = B.CODPROD
                            AND A.POSICAO IN ('P')
                            ORDER BY A.NUMOP"


        If conexao.State = 0 Then ConectaOra()
        Dim cmd As New OracleCommand
        Dim da As New OracleDataAdapter(cmd)
        Dim dt As New DataTable
        cmd.CommandType = CommandType.Text
        cmd.CommandText = sql
        cmd.Connection = conexao

        Try

            da.Fill(dt)

            DataGridView1.DataSource = dt

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try





    End Sub

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick

        Dim sel As DataGridViewSelectedRowCollection = DataGridView1.SelectedRows
        Dim codigo = sel(0).Cells(0).Value.ToString
        Dim descricao = sel(0).Cells(3).Value.ToString
        Dim qt = sel(0).Cells(4).Value.ToString

        frm.txtNumOP.Text = codigo
        frm.txtdescricao.Text = descricao
        frm.txtQtd.Text = qt
        frm.txtNumOP.Select()
        Me.Hide()
    End Sub
End Class