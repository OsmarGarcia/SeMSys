<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTrocaNF
    Inherits System.Windows.Forms.Form

    'Descartar substituições de formulário para limpar a lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTrocaNF))
        Me.btnPesquisar = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtNumNota = New System.Windows.Forms.TextBox()
        Me.dgvNF = New System.Windows.Forms.DataGridView()
        CType(Me.dgvNF, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnPesquisar
        '
        Me.btnPesquisar.Location = New System.Drawing.Point(303, 42)
        Me.btnPesquisar.Name = "btnPesquisar"
        Me.btnPesquisar.Size = New System.Drawing.Size(172, 29)
        Me.btnPesquisar.TabIndex = 7
        Me.btnPesquisar.Text = "Pesquisar"
        Me.btnPesquisar.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(12, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(78, 20)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Num Nota"
        '
        'txtNumNota
        '
        Me.txtNumNota.Location = New System.Drawing.Point(12, 43)
        Me.txtNumNota.Name = "txtNumNota"
        Me.txtNumNota.Size = New System.Drawing.Size(273, 27)
        Me.txtNumNota.TabIndex = 5
        '
        'dgvNF
        '
        Me.dgvNF.AllowUserToAddRows = False
        Me.dgvNF.AllowUserToDeleteRows = False
        Me.dgvNF.AllowUserToOrderColumns = True
        Me.dgvNF.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight
        Me.dgvNF.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvNF.Location = New System.Drawing.Point(9, 86)
        Me.dgvNF.Name = "dgvNF"
        Me.dgvNF.ReadOnly = True
        Me.dgvNF.RowHeadersWidth = 51
        Me.dgvNF.RowTemplate.Height = 29
        Me.dgvNF.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvNF.Size = New System.Drawing.Size(1164, 435)
        Me.dgvNF.TabIndex = 4
        '
        'frmTrocaNF
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ControlDarkDark
        Me.ClientSize = New System.Drawing.Size(1185, 533)
        Me.Controls.Add(Me.btnPesquisar)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtNumNota)
        Me.Controls.Add(Me.dgvNF)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmTrocaNF"
        Me.Text = "frmTrocaNF"
        CType(Me.dgvNF, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnPesquisar As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents txtNumNota As TextBox
    Friend WithEvents dgvNF As DataGridView
End Class
