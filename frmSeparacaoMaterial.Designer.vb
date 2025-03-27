<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSeparacaoMaterial
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSeparacaoMaterial))
        Me.txtNumOP = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnGerarSeparacao = New System.Windows.Forms.Button()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.txtDivisor = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnPesquisar = New System.Windows.Forms.Button()
        Me.txtQtd = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtdescricao = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.dgvApontamentos = New System.Windows.Forms.DataGridView()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvApontamentos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtNumOP
        '
        Me.txtNumOP.Location = New System.Drawing.Point(23, 67)
        Me.txtNumOP.MaxLength = 6
        Me.txtNumOP.Name = "txtNumOP"
        Me.txtNumOP.Size = New System.Drawing.Size(125, 27)
        Me.txtNumOP.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(23, 44)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(60, 20)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "NumOP"
        '
        'btnGerarSeparacao
        '
        Me.btnGerarSeparacao.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.btnGerarSeparacao.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaption
        Me.btnGerarSeparacao.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnGerarSeparacao.Location = New System.Drawing.Point(973, 63)
        Me.btnGerarSeparacao.Name = "btnGerarSeparacao"
        Me.btnGerarSeparacao.Size = New System.Drawing.Size(140, 35)
        Me.btnGerarSeparacao.TabIndex = 2
        Me.btnGerarSeparacao.Text = "Gerar Requisição"
        Me.btnGerarSeparacao.UseVisualStyleBackColor = False
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DataGridView1.BackgroundColor = System.Drawing.Color.White
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Location = New System.Drawing.Point(23, 129)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.RowHeadersWidth = 51
        Me.DataGridView1.RowTemplate.Height = 29
        Me.DataGridView1.Size = New System.Drawing.Size(1090, 220)
        Me.DataGridView1.TabIndex = 3
        '
        'txtDivisor
        '
        Me.txtDivisor.Location = New System.Drawing.Point(805, 67)
        Me.txtDivisor.MaxLength = 6
        Me.txtDivisor.Name = "txtDivisor"
        Me.txtDivisor.Size = New System.Drawing.Size(142, 27)
        Me.txtDivisor.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(805, 44)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(144, 20)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Requistar para (Qtd)"
        '
        'btnPesquisar
        '
        Me.btnPesquisar.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.btnPesquisar.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaption
        Me.btnPesquisar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPesquisar.Location = New System.Drawing.Point(154, 61)
        Me.btnPesquisar.Name = "btnPesquisar"
        Me.btnPesquisar.Size = New System.Drawing.Size(45, 33)
        Me.btnPesquisar.TabIndex = 4
        Me.btnPesquisar.Text = "..."
        Me.btnPesquisar.UseVisualStyleBackColor = False
        '
        'txtQtd
        '
        Me.txtQtd.Enabled = False
        Me.txtQtd.Location = New System.Drawing.Point(624, 67)
        Me.txtQtd.MaxLength = 2
        Me.txtQtd.Name = "txtQtd"
        Me.txtQtd.Size = New System.Drawing.Size(152, 27)
        Me.txtQtd.TabIndex = 0
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(624, 44)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(145, 20)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "Qtd. Total a Produzir"
        '
        'txtdescricao
        '
        Me.txtdescricao.Enabled = False
        Me.txtdescricao.Location = New System.Drawing.Point(205, 67)
        Me.txtdescricao.MaxLength = 2
        Me.txtdescricao.Name = "txtdescricao"
        Me.txtdescricao.Size = New System.Drawing.Size(409, 27)
        Me.txtdescricao.TabIndex = 0
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(205, 44)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(62, 20)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "Produto"
        '
        'dgvApontamentos
        '
        Me.dgvApontamentos.AllowUserToAddRows = False
        Me.dgvApontamentos.AllowUserToDeleteRows = False
        Me.dgvApontamentos.AllowUserToOrderColumns = True
        Me.dgvApontamentos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.dgvApontamentos.BackgroundColor = System.Drawing.Color.White
        Me.dgvApontamentos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvApontamentos.Location = New System.Drawing.Point(23, 387)
        Me.dgvApontamentos.Name = "dgvApontamentos"
        Me.dgvApontamentos.RowHeadersWidth = 51
        Me.dgvApontamentos.RowTemplate.Height = 29
        Me.dgvApontamentos.Size = New System.Drawing.Size(1090, 466)
        Me.dgvApontamentos.TabIndex = 5
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(23, 106)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(84, 20)
        Me.Label5.TabIndex = 6
        Me.Label5.Text = "Itens da OP"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(23, 364)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(151, 20)
        Me.Label6.TabIndex = 7
        Me.Label6.Text = "Apontamentos da OP"
        '
        'frmSeparacaoMaterial
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.SlateGray
        Me.ClientSize = New System.Drawing.Size(1136, 953)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.dgvApontamentos)
        Me.Controls.Add(Me.btnPesquisar)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.btnGerarSeparacao)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtdescricao)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtQtd)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtDivisor)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtNumOP)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "frmSeparacaoMaterial"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Separação de Material"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvApontamentos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtNumOP As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents btnGerarSeparacao As Button
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents txtDivisor As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents btnPesquisar As Button
    Friend WithEvents txtQtd As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtdescricao As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents dgvApontamentos As DataGridView
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
End Class
