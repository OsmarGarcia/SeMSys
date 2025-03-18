<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmRelProducaoTotalWinthor
    Inherits System.Windows.Forms.Form

    'Descartar substituições de formulário para limpar a lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Exigido pelo Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'OBSERVAÇÃO: o procedimento a seguir é exigido pelo Windows Form Designer
    'Pode ser modificado usando o Windows Form Designer.  
    'Não o modifique usando o editor de códigos.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRelProducaoTotalWinthor))
        Me.btnPesquisar = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.dtpDtIncicio = New System.Windows.Forms.DateTimePicker()
        Me.dtpDtinal = New System.Windows.Forms.DateTimePicker()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'btnPesquisar
        '
        Me.btnPesquisar.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.btnPesquisar.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaption
        Me.btnPesquisar.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.InactiveCaption
        Me.btnPesquisar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPesquisar.Image = Global.SeMSys.My.Resources.Resources.lista_de_papel
        Me.btnPesquisar.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnPesquisar.Location = New System.Drawing.Point(399, 104)
        Me.btnPesquisar.Name = "btnPesquisar"
        Me.btnPesquisar.Size = New System.Drawing.Size(197, 55)
        Me.btnPesquisar.TabIndex = 1
        Me.btnPesquisar.Text = "Gerar Relatorio"
        Me.btnPesquisar.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label1.Location = New System.Drawing.Point(21, 57)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(80, 20)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Data Incial"
        '
        'dtpDtIncicio
        '
        Me.dtpDtIncicio.Location = New System.Drawing.Point(21, 80)
        Me.dtpDtIncicio.Name = "dtpDtIncicio"
        Me.dtpDtIncicio.Size = New System.Drawing.Size(250, 27)
        Me.dtpDtIncicio.TabIndex = 3
        '
        'dtpDtinal
        '
        Me.dtpDtinal.Location = New System.Drawing.Point(21, 144)
        Me.dtpDtinal.Name = "dtpDtinal"
        Me.dtpDtinal.Size = New System.Drawing.Size(250, 27)
        Me.dtpDtinal.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label2.Location = New System.Drawing.Point(21, 121)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(76, 20)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Data Final"
        '
        'frmRelProducaoTotalWinthor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.SlateGray
        Me.ClientSize = New System.Drawing.Size(641, 236)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.dtpDtinal)
        Me.Controls.Add(Me.dtpDtIncicio)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnPesquisar)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmRelProducaoTotalWinthor"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Rel Resumo por OP"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnPesquisar As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents dtpDtIncicio As DateTimePicker
    Friend WithEvents dtpDtinal As DateTimePicker
    Friend WithEvents Label2 As Label
End Class
