<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMonitor
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMonitor))
        Me.cboLinha = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.dgvProducao = New System.Windows.Forms.DataGridView()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtNumOP = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.dgvIniciar = New System.Windows.Forms.DataGridView()
        Me.btnIniciar = New System.Windows.Forms.Button()
        Me.btnFinalizar = New System.Windows.Forms.Button()
        CType(Me.dgvProducao, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvIniciar, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cboLinha
        '
        Me.cboLinha.FormattingEnabled = True
        Me.cboLinha.Items.AddRange(New Object() {"Linha 01", "Linha 02", "Linha 03"})
        Me.cboLinha.Location = New System.Drawing.Point(38, 40)
        Me.cboLinha.Name = "cboLinha"
        Me.cboLinha.Size = New System.Drawing.Size(151, 28)
        Me.cboLinha.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(38, 10)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(132, 20)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Linha de Produção"
        '
        'dgvProducao
        '
        Me.dgvProducao.AllowUserToAddRows = False
        Me.dgvProducao.AllowUserToDeleteRows = False
        Me.dgvProducao.AllowUserToResizeRows = False
        Me.dgvProducao.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.dgvProducao.BackgroundColor = System.Drawing.Color.White
        Me.dgvProducao.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.dgvProducao.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvProducao.Location = New System.Drawing.Point(40, 107)
        Me.dgvProducao.Name = "dgvProducao"
        Me.dgvProducao.RowHeadersWidth = 51
        Me.dgvProducao.RowTemplate.Height = 29
        Me.dgvProducao.Size = New System.Drawing.Size(848, 122)
        Me.dgvProducao.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(38, 84)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(97, 20)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Em Produção"
        '
        'txtNumOP
        '
        Me.txtNumOP.Location = New System.Drawing.Point(41, 278)
        Me.txtNumOP.Name = "txtNumOP"
        Me.txtNumOP.Size = New System.Drawing.Size(125, 27)
        Me.txtNumOP.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(39, 255)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(64, 20)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Num OP"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(38, 308)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(97, 20)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Em Produção"
        '
        'dgvIniciar
        '
        Me.dgvIniciar.AllowUserToAddRows = False
        Me.dgvIniciar.AllowUserToDeleteRows = False
        Me.dgvIniciar.AllowUserToResizeRows = False
        Me.dgvIniciar.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.dgvIniciar.BackgroundColor = System.Drawing.Color.White
        Me.dgvIniciar.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.dgvIniciar.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvIniciar.Location = New System.Drawing.Point(40, 331)
        Me.dgvIniciar.Name = "dgvIniciar"
        Me.dgvIniciar.RowHeadersWidth = 51
        Me.dgvIniciar.RowTemplate.Height = 29
        Me.dgvIniciar.Size = New System.Drawing.Size(848, 107)
        Me.dgvIniciar.TabIndex = 7
        '
        'btnIniciar
        '
        Me.btnIniciar.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.btnIniciar.FlatAppearance.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.btnIniciar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnIniciar.Image = Global.SeMSys.My.Resources.Resources.verificar
        Me.btnIniciar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnIniciar.Location = New System.Drawing.Point(177, 277)
        Me.btnIniciar.Name = "btnIniciar"
        Me.btnIniciar.Size = New System.Drawing.Size(188, 29)
        Me.btnIniciar.TabIndex = 9
        Me.btnIniciar.Text = "Iniciar Monitor"
        Me.btnIniciar.UseVisualStyleBackColor = False
        '
        'btnFinalizar
        '
        Me.btnFinalizar.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.btnFinalizar.FlatAppearance.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.btnFinalizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnFinalizar.Image = Global.SeMSys.My.Resources.Resources.download__1_
        Me.btnFinalizar.ImageAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.btnFinalizar.Location = New System.Drawing.Point(388, 277)
        Me.btnFinalizar.Name = "btnFinalizar"
        Me.btnFinalizar.Size = New System.Drawing.Size(208, 29)
        Me.btnFinalizar.TabIndex = 10
        Me.btnFinalizar.Text = "Finalizar Monitor"
        Me.btnFinalizar.UseVisualStyleBackColor = False
        '
        'frmMonitor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.SlateGray
        Me.BackgroundImage = Global.SeMSys.My.Resources.Resources.SM_Cor_2000px_300x
        Me.ClientSize = New System.Drawing.Size(941, 450)
        Me.Controls.Add(Me.btnFinalizar)
        Me.Controls.Add(Me.btnIniciar)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.dgvIniciar)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtNumOP)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.dgvProducao)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cboLinha)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "frmMonitor"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Monitoramento Online"
        CType(Me.dgvProducao, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvIniciar, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents cboLinha As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents dgvProducao As DataGridView
    Friend WithEvents Label2 As Label
    Friend WithEvents txtNumOP As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents dgvIniciar As DataGridView
    Friend WithEvents btnIniciar As Button
    Friend WithEvents btnFinalizar As Button
End Class
