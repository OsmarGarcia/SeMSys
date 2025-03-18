<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmParadasOP
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmParadasOP))
        Me.txtNumOP = New System.Windows.Forms.TextBox()
        Me.btnPesquisar = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'txtNumOP
        '
        Me.txtNumOP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtNumOP.Location = New System.Drawing.Point(24, 118)
        Me.txtNumOP.MaxLength = 6
        Me.txtNumOP.Name = "txtNumOP"
        Me.txtNumOP.PlaceholderText = "Insira apenas uma OP"
        Me.txtNumOP.Size = New System.Drawing.Size(352, 27)
        Me.txtNumOP.TabIndex = 0
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
        Me.Label1.Location = New System.Drawing.Point(24, 89)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(143, 20)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Ordem de Produção"
        '
        'frmParadasOP
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.SlateGray
        Me.ClientSize = New System.Drawing.Size(641, 236)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnPesquisar)
        Me.Controls.Add(Me.txtNumOP)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmParadasOP"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Rel Resumo por OP"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtNumOP As TextBox
    Friend WithEvents btnPesquisar As Button
    Friend WithEvents Label1 As Label
End Class
