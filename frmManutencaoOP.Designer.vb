<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmManutencaoOP
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmManutencaoOP))
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.btnPesquisar = New System.Windows.Forms.Button()
        Me.btnGerarSeparacao = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtdescricao = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtQtd = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtNumOP = New System.Windows.Forms.TextBox()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.btnIniciar = New System.Windows.Forms.Button()
        Me.btnPesquisarOP = New System.Windows.Forms.Button()
        Me.DateTimePicker2 = New System.Windows.Forms.DateTimePicker()
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker()
        Me.DataGridView2 = New System.Windows.Forms.DataGridView()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtCodProd = New System.Windows.Forms.TextBox()
        Me.txtMetodo = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.cboFilial = New System.Windows.Forms.ComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.btnRecalcularReserva = New System.Windows.Forms.Button()
        Me.txtDescReserva = New System.Windows.Forms.TextBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtCodProdReserva = New System.Windows.Forms.TextBox()
        Me.TabPage4 = New System.Windows.Forms.TabPage()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControl1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage1.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.SuspendLayout()
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.AllowUserToResizeRows = False
        Me.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DataGridView1.BackgroundColor = System.Drawing.SystemColors.ControlLightLight
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Location = New System.Drawing.Point(16, 124)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.RowHeadersWidth = 51
        Me.DataGridView1.RowTemplate.Height = 29
        Me.DataGridView1.Size = New System.Drawing.Size(913, 285)
        Me.DataGridView1.TabIndex = 2
        '
        'btnPesquisar
        '
        Me.btnPesquisar.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.btnPesquisar.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaption
        Me.btnPesquisar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPesquisar.Location = New System.Drawing.Point(125, 33)
        Me.btnPesquisar.Name = "btnPesquisar"
        Me.btnPesquisar.Size = New System.Drawing.Size(45, 33)
        Me.btnPesquisar.TabIndex = 14
        Me.btnPesquisar.Text = "..."
        Me.btnPesquisar.UseVisualStyleBackColor = False
        '
        'btnGerarSeparacao
        '
        Me.btnGerarSeparacao.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.btnGerarSeparacao.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaption
        Me.btnGerarSeparacao.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnGerarSeparacao.Location = New System.Drawing.Point(16, 83)
        Me.btnGerarSeparacao.Name = "btnGerarSeparacao"
        Me.btnGerarSeparacao.Size = New System.Drawing.Size(183, 35)
        Me.btnGerarSeparacao.TabIndex = 13
        Me.btnGerarSeparacao.Text = "Pesquisar Formulação"
        Me.btnGerarSeparacao.UseVisualStyleBackColor = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(284, 16)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(62, 20)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "Produto"
        '
        'txtdescricao
        '
        Me.txtdescricao.Enabled = False
        Me.txtdescricao.Location = New System.Drawing.Point(285, 39)
        Me.txtdescricao.MaxLength = 20
        Me.txtdescricao.Name = "txtdescricao"
        Me.txtdescricao.Size = New System.Drawing.Size(306, 27)
        Me.txtdescricao.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(596, 16)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(96, 20)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Qtd. Produzir"
        '
        'txtQtd
        '
        Me.txtQtd.Enabled = False
        Me.txtQtd.Location = New System.Drawing.Point(597, 39)
        Me.txtQtd.MaxLength = 6
        Me.txtQtd.Name = "txtQtd"
        Me.txtQtd.Size = New System.Drawing.Size(95, 27)
        Me.txtQtd.TabIndex = 6
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(16, 16)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(60, 20)
        Me.Label5.TabIndex = 12
        Me.Label5.Text = "NumOP"
        '
        'txtNumOP
        '
        Me.txtNumOP.Location = New System.Drawing.Point(16, 39)
        Me.txtNumOP.MaxLength = 6
        Me.txtNumOP.Name = "txtNumOP"
        Me.txtNumOP.Size = New System.Drawing.Size(103, 27)
        Me.txtNumOP.TabIndex = 8
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Controls.Add(Me.TabPage4)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(1004, 506)
        Me.TabControl1.TabIndex = 15
        '
        'TabPage2
        '
        Me.TabPage2.BackColor = System.Drawing.Color.SlateGray
        Me.TabPage2.Controls.Add(Me.Label8)
        Me.TabPage2.Controls.Add(Me.Label7)
        Me.TabPage2.Controls.Add(Me.btnIniciar)
        Me.TabPage2.Controls.Add(Me.btnPesquisarOP)
        Me.TabPage2.Controls.Add(Me.DateTimePicker2)
        Me.TabPage2.Controls.Add(Me.DateTimePicker1)
        Me.TabPage2.Controls.Add(Me.DataGridView2)
        Me.TabPage2.Location = New System.Drawing.Point(4, 29)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(996, 473)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Iniciar OP"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(172, 3)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(76, 20)
        Me.Label8.TabIndex = 8
        Me.Label8.Text = "Data Final"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(8, 3)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(84, 20)
        Me.Label7.TabIndex = 7
        Me.Label7.Text = "Data Inicial"
        '
        'btnIniciar
        '
        Me.btnIniciar.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.btnIniciar.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaption
        Me.btnIniciar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnIniciar.ForeColor = System.Drawing.SystemColors.WindowText
        Me.btnIniciar.Location = New System.Drawing.Point(487, 19)
        Me.btnIniciar.Name = "btnIniciar"
        Me.btnIniciar.Size = New System.Drawing.Size(130, 38)
        Me.btnIniciar.TabIndex = 5
        Me.btnIniciar.Text = "Iniciar OP's"
        Me.btnIniciar.UseVisualStyleBackColor = False
        '
        'btnPesquisarOP
        '
        Me.btnPesquisarOP.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.btnPesquisarOP.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaption
        Me.btnPesquisarOP.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPesquisarOP.ForeColor = System.Drawing.SystemColors.WindowText
        Me.btnPesquisarOP.Location = New System.Drawing.Point(334, 19)
        Me.btnPesquisarOP.Name = "btnPesquisarOP"
        Me.btnPesquisarOP.Size = New System.Drawing.Size(130, 38)
        Me.btnPesquisarOP.TabIndex = 4
        Me.btnPesquisarOP.Text = "Pesquisar"
        Me.btnPesquisarOP.UseVisualStyleBackColor = False
        '
        'DateTimePicker2
        '
        Me.DateTimePicker2.CustomFormat = "dd/MM/yyyy"
        Me.DateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimePicker2.Location = New System.Drawing.Point(172, 30)
        Me.DateTimePicker2.Name = "DateTimePicker2"
        Me.DateTimePicker2.Size = New System.Drawing.Size(145, 27)
        Me.DateTimePicker2.TabIndex = 3
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.CustomFormat = "dd/MM/yyyy"
        Me.DateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimePicker1.Location = New System.Drawing.Point(11, 30)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.Size = New System.Drawing.Size(154, 27)
        Me.DateTimePicker1.TabIndex = 2
        '
        'DataGridView2
        '
        Me.DataGridView2.AllowUserToAddRows = False
        Me.DataGridView2.AllowUserToDeleteRows = False
        Me.DataGridView2.AllowUserToResizeRows = False
        Me.DataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DataGridView2.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells
        Me.DataGridView2.BackgroundColor = System.Drawing.Color.White
        Me.DataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView2.Location = New System.Drawing.Point(0, 81)
        Me.DataGridView2.Name = "DataGridView2"
        Me.DataGridView2.RowHeadersWidth = 51
        Me.DataGridView2.RowTemplate.Height = 29
        Me.DataGridView2.Size = New System.Drawing.Size(929, 333)
        Me.DataGridView2.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.BackColor = System.Drawing.Color.SlateGray
        Me.TabPage1.Controls.Add(Me.Button1)
        Me.TabPage1.Controls.Add(Me.Label1)
        Me.TabPage1.Controls.Add(Me.Label5)
        Me.TabPage1.Controls.Add(Me.DataGridView1)
        Me.TabPage1.Controls.Add(Me.txtCodProd)
        Me.TabPage1.Controls.Add(Me.btnPesquisar)
        Me.TabPage1.Controls.Add(Me.txtNumOP)
        Me.TabPage1.Controls.Add(Me.btnGerarSeparacao)
        Me.TabPage1.Controls.Add(Me.txtMetodo)
        Me.TabPage1.Controls.Add(Me.txtQtd)
        Me.TabPage1.Controls.Add(Me.Label4)
        Me.TabPage1.Controls.Add(Me.Label2)
        Me.TabPage1.Controls.Add(Me.Label3)
        Me.TabPage1.Controls.Add(Me.txtdescricao)
        Me.TabPage1.Location = New System.Drawing.Point(4, 29)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(996, 473)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Recalcular Itens da OP"
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.Button1.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaption
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button1.Location = New System.Drawing.Point(205, 83)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(183, 35)
        Me.Button1.TabIndex = 15
        Me.Button1.Text = "Recalcular"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(176, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(71, 20)
        Me.Label1.TabIndex = 12
        Me.Label1.Text = "Cod Prod"
        '
        'txtCodProd
        '
        Me.txtCodProd.Location = New System.Drawing.Point(176, 39)
        Me.txtCodProd.MaxLength = 6
        Me.txtCodProd.Name = "txtCodProd"
        Me.txtCodProd.Size = New System.Drawing.Size(103, 27)
        Me.txtCodProd.TabIndex = 8
        '
        'txtMetodo
        '
        Me.txtMetodo.Location = New System.Drawing.Point(699, 39)
        Me.txtMetodo.MaxLength = 6
        Me.txtMetodo.Name = "txtMetodo"
        Me.txtMetodo.Size = New System.Drawing.Size(95, 27)
        Me.txtMetodo.TabIndex = 6
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(698, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(62, 20)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "Metodo"
        '
        'TabPage3
        '
        Me.TabPage3.BackColor = System.Drawing.Color.SlateGray
        Me.TabPage3.Controls.Add(Me.Label10)
        Me.TabPage3.Controls.Add(Me.cboFilial)
        Me.TabPage3.Controls.Add(Me.Label9)
        Me.TabPage3.Controls.Add(Me.btnRecalcularReserva)
        Me.TabPage3.Controls.Add(Me.txtDescReserva)
        Me.TabPage3.Controls.Add(Me.Button2)
        Me.TabPage3.Controls.Add(Me.Label6)
        Me.TabPage3.Controls.Add(Me.txtCodProdReserva)
        Me.TabPage3.Location = New System.Drawing.Point(4, 29)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage3.Size = New System.Drawing.Size(996, 473)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Recalcular Reserva"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.ForeColor = System.Drawing.SystemColors.ButtonFace
        Me.Label10.Location = New System.Drawing.Point(99, 52)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(71, 20)
        Me.Label10.TabIndex = 7
        Me.Label10.Text = "Cod Filial"
        '
        'cboFilial
        '
        Me.cboFilial.FormattingEnabled = True
        Me.cboFilial.Location = New System.Drawing.Point(99, 78)
        Me.cboFilial.Name = "cboFilial"
        Me.cboFilial.Size = New System.Drawing.Size(151, 28)
        Me.cboFilial.TabIndex = 6
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.ForeColor = System.Drawing.SystemColors.ButtonFace
        Me.Label9.Location = New System.Drawing.Point(296, 122)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(74, 20)
        Me.Label9.TabIndex = 5
        Me.Label9.Text = "Descrição"
        '
        'btnRecalcularReserva
        '
        Me.btnRecalcularReserva.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.btnRecalcularReserva.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaption
        Me.btnRecalcularReserva.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRecalcularReserva.Location = New System.Drawing.Point(99, 209)
        Me.btnRecalcularReserva.Name = "btnRecalcularReserva"
        Me.btnRecalcularReserva.Size = New System.Drawing.Size(748, 55)
        Me.btnRecalcularReserva.TabIndex = 4
        Me.btnRecalcularReserva.Text = "Recalcular Reserva"
        Me.btnRecalcularReserva.UseVisualStyleBackColor = False
        '
        'txtDescReserva
        '
        Me.txtDescReserva.Enabled = False
        Me.txtDescReserva.Location = New System.Drawing.Point(296, 154)
        Me.txtDescReserva.Name = "txtDescReserva"
        Me.txtDescReserva.Size = New System.Drawing.Size(551, 27)
        Me.txtDescReserva.TabIndex = 3
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(230, 151)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(60, 35)
        Me.Button2.TabIndex = 2
        Me.Button2.Text = "..."
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.ForeColor = System.Drawing.SystemColors.ButtonFace
        Me.Label6.Location = New System.Drawing.Point(99, 122)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(71, 20)
        Me.Label6.TabIndex = 1
        Me.Label6.Text = "Cod Prod"
        '
        'txtCodProdReserva
        '
        Me.txtCodProdReserva.Location = New System.Drawing.Point(99, 154)
        Me.txtCodProdReserva.Name = "txtCodProdReserva"
        Me.txtCodProdReserva.Size = New System.Drawing.Size(125, 27)
        Me.txtCodProdReserva.TabIndex = 0
        '
        'TabPage4
        '
        Me.TabPage4.Location = New System.Drawing.Point(4, 29)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage4.Size = New System.Drawing.Size(996, 473)
        Me.TabPage4.TabIndex = 3
        Me.TabPage4.Text = "Apontar Produção"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'frmManutencaoOP
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1004, 506)
        Me.Controls.Add(Me.TabControl1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmManutencaoOP"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Manutenção da Ordem de Produção"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents btnPesquisar As Button
    Friend WithEvents btnGerarSeparacao As Button
    Friend WithEvents Label4 As Label
    Friend WithEvents txtdescricao As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtQtd As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents txtNumOP As TextBox
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents Button1 As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents txtCodProd As TextBox
    Friend WithEvents txtMetodo As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents Label8 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents btnIniciar As Button
    Friend WithEvents btnPesquisarOP As Button
    Friend WithEvents DateTimePicker2 As DateTimePicker
    Friend WithEvents DateTimePicker1 As DateTimePicker
    Friend WithEvents DataGridView2 As DataGridView
    Friend WithEvents TabPage3 As TabPage
    Friend WithEvents Label6 As Label
    Friend WithEvents txtCodProdReserva As TextBox
    Friend WithEvents Button2 As Button
    Friend WithEvents txtDescReserva As TextBox
    Friend WithEvents btnRecalcularReserva As Button
    Friend WithEvents Label9 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents cboFilial As ComboBox
    Friend WithEvents TabPage4 As TabPage
End Class
