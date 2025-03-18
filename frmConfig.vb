Imports System.Configuration
Imports System.Collections.Generic
Imports System.Windows.Forms.VisualStyles
Imports System.ComponentModel

Public Class frmConfig
    Private Sub frmConfig_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        On Error Resume Next
        ' Criar uma lista para armazenar os itens de configuração
        Dim settingsList As New List(Of SettingItem)()

        ' Popular a lista com os itens de configuração
        For Each settingProperty As SettingsProperty In My.Settings.Properties
            If settingProperty.Name <> "UsuarioWinthor" AndAlso settingProperty.Name <> "NomeWinthor" _
                AndAlso settingProperty.Name <> "PermissaoUsuario" AndAlso settingProperty.Name <> "CurrentVersion" _
                AndAlso settingProperty.Name <> "UpgradeRequired" Then
                Dim settingItem As New SettingItem()
                settingItem.Name = settingProperty.Name
                settingItem.Value = My.Settings(settingProperty.Name)
                settingsList.Add(settingItem)
            End If
        Next




        ' Atribuir a lista ao DataGridView
        DataGridView1.DataSource = settingsList
        DataGridView1.Sort(DataGridView1.Columns(0), ListSortDirection.Ascending)



        For i = 0 To DataGridView1.RowCount

            If DataGridView1.Rows(i).Cells(0).Value.ToString.Substring(0, 3) = "Cor" Then

                Dim colorstring = DataGridView1.Rows(i).Cells(1).Value.ToString
                If colorstring.Length = 8 Then

                    ' Extrai componentes ARGB da string
                    Dim a As Integer = Convert.ToInt32(colorstring.Substring(0, 2), 16)
                    Dim r As Integer = Convert.ToInt32(colorstring.Substring(2, 2), 16)
                    Dim g As Integer = Convert.ToInt32(colorstring.Substring(4, 2), 16)
                    Dim b As Integer = Convert.ToInt32(colorstring.Substring(6, 2), 16)

                    DataGridView1.Rows(i).Cells(1).Style.ForeColor = Color.FromArgb(a, r, g, b)

                End If
            End If

        Next
    End Sub


    Private Sub DataGridView1_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellEndEdit, DataGridView1.CellValueChanged
        ' Obter o nome e o novo valor da configuração
        Dim settingName As String = DataGridView1.Rows(e.RowIndex).Cells("Name").Value.ToString()
        Dim settingValue As Object = DataGridView1.Rows(e.RowIndex).Cells("Value").Value

        ' Atualizar My.Settings com o novo valor
        My.Settings(settingName) = settingValue
        My.Settings.Save()
    End Sub
    Public Class SettingItem
        Public Property Name As String
        Public Property Value As Object
    End Class



    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        ' Check if the clicked cell is in the Color column
        If e.ColumnIndex = DataGridView1.Columns("Value").Index AndAlso DataGridView1.Rows(e.RowIndex).Cells(0).Value.ToString.Substring(0, 3) = "Cor" Then
            ' Open ColorDialog
            If ColorDialog1.ShowDialog() = DialogResult.OK Then
                ' Get selected color
                Dim selectedColor As Color = ColorDialog1.Color
                ' Convert color to ARGB format
                Dim argbColor As String = selectedColor.ToArgb().ToString("X")
                ' Update the cell with the ARGB color value
                DataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = argbColor
                DataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex).Style.ForeColor = selectedColor
            End If
        End If
    End Sub

End Class




