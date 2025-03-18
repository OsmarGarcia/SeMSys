

Imports Microsoft.Web.WebView2.WinForms

Public Class FrmMonitorOnline


    Public pagina As String
    Private Sub NovaPagina()
        'Página
        Dim _uri As New Uri(pagina)


        WebView21.Source = _uri
        WebView21.ZoomFactor = 0.6

    End Sub
    Private Sub FrmMonitorOnline_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.pagina = "http://192.168.0.165:3008/d/b3228a06-5b11-4621-b817-ef6dd6b35c64/dashboard-l03?orgId=1&refresh=5s&from=now-1h&to=now&kiosk"

        NovaPagina()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        WebView21.ZoomFactor += 0.1
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        WebView21.ZoomFactor -= 0.1
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.pagina = "https://app.powerbi.com/Redirect?action=OpenReport&appId=211c636b-b82d-4597-94c9-15baeb7ce580&reportObjectId=85954871-5a03-492a-b8e0-1274d4a461ec&ctid=90689b1e-e545-45f4-b50c-9b23da755a8c&reportPage=ReportSectiond2f5e4e37230582dc282&pbi_source=appShareLink&portalSessionId=c0158c64-cf17-4bcb-b849-68accaebc2a4"

        NovaPagina()
    End Sub
End Class