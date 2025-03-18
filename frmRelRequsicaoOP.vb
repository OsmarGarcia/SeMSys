

Public Class frmRelRequsicaoOP
    Public NomeRelatorio As String
    Dim dt = New DataTable


    Sub New(dt As DataTable)

        InitializeComponent()
        Me.dt = dt
    End Sub
    Private Sub frmRelRequsicaoOP_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        Me.Text = NomeRelatorio
        Me.ReportViewer1.RefreshReport()

    End Sub
End Class