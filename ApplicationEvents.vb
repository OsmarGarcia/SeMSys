Imports System.Configuration
Imports Microsoft.VisualBasic.ApplicationServices

Namespace My
    ' The following events are available for MyApplication:
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.

    ' **NEW** ApplyApplicationDefaults: Raised when the application queries default values to be set for the application.

    ' Example:
    ' Private Sub MyApplication_ApplyApplicationDefaults(sender As Object, e As ApplyApplicationDefaultsEventArgs) Handles Me.ApplyApplicationDefaults
    '
    '   ' Setting the application-wide default Font:
    '   e.Font = New Font(FontFamily.GenericSansSerif, 12, FontStyle.Regular)
    '
    '   ' Setting the HighDpiMode for the Application:
    '   e.HighDpiMode = HighDpiMode.PerMonitorV2
    '
    '   ' If a splash dialog is used, this sets the minimum display time:
    '   e.MinimumSplashScreenDisplayTime = 4000
    ' End Sub

    Partial Friend Class MyApplication
        Private Sub MyApplication_Startup(sender As Object, e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) Handles Me.Startup


            Dim currentVersion As String = ConfigurationManager.AppSettings("CurrentVersion")
            'currentVersion = My.Settings.CurrentVersion
            If currentVersion Is Nothing OrElse currentVersion <> My.Application.Info.Version.ToString() Then
                ' Atualize as configurações
                My.Settings.Reload()
                My.Settings("CurrentVersion") = My.Application.Info.Version.ToString()
                My.Settings.Save()
            End If



        End Sub


        Private Sub MyApplication_UnhandledException(sender As Object, e As UnhandledExceptionEventArgs) Handles Me.UnhandledException



            MessageBox.Show("Ocorreu um erro fora do aplicativo: " & e.Exception.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)


            ' Impedir que a aplicação desligue após uma exceção não tratada
            e.ExitApplication = False
        End Sub
    End Class



End Namespace
