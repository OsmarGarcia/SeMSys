<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmProgramarProducao
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmProgramarProducao))
        Me.Button3 = New System.Windows.Forms.Button()
        Me.panelMenu = New System.Windows.Forms.Panel()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.btnRelatorios = New System.Windows.Forms.Button()
        Me.btnApontar = New System.Windows.Forms.Button()
        Me.btnManutencao = New System.Windows.Forms.Button()
        Me.btnProgramar = New System.Windows.Forms.Button()
        Me.panelProgramar2 = New System.Windows.Forms.Panel()
        Me.DtPickerHoraFinal = New System.Windows.Forms.DateTimePicker()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.lblFilialProducao = New System.Windows.Forms.Label()
        Me.lblFilialEstoque = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.btnAdicionarIntervalo = New System.Windows.Forms.Button()
        Me.txtQtunitcx = New System.Windows.Forms.TextBox()
        Me.txtEficiencia = New System.Windows.Forms.MaskedTextBox()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.btnSalvarPrograma = New System.Windows.Forms.Button()
        Me.btnCarregarPrograma = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.txtCodPrograma = New System.Windows.Forms.TextBox()
        Me.btnPesquisar = New System.Windows.Forms.Button()
        Me.DtPickerHoraInicial = New System.Windows.Forms.DateTimePicker()
        Me.listMateriais2 = New System.Windows.Forms.ListView()
        Me.CodProdMateriais = New System.Windows.Forms.ColumnHeader()
        Me.DescricaoMateriais = New System.Windows.Forms.ColumnHeader()
        Me.QtNecessariaMateriais = New System.Windows.Forms.ColumnHeader()
        Me.QtEstoqueMateriais = New System.Windows.Forms.ColumnHeader()
        Me.Tipomerc = New System.Windows.Forms.ColumnHeader()
        Me.MetodoSA = New System.Windows.Forms.ColumnHeader()
        Me.codprodmaster = New System.Windows.Forms.ColumnHeader()
        Me.OP = New System.Windows.Forms.ColumnHeader()
        Me.Lote = New System.Windows.Forms.ColumnHeader()
        Me.DataPrevInicio = New System.Windows.Forms.ColumnHeader()
        Me.CodProgMateriais = New System.Windows.Forms.ColumnHeader()
        Me.listOrdens2 = New System.Windows.Forms.ListView()
        Me.CodProd = New System.Windows.Forms.ColumnHeader()
        Me.Descricao = New System.Windows.Forms.ColumnHeader()
        Me.Embalagem = New System.Windows.Forms.ColumnHeader()
        Me.Metodo = New System.Windows.Forms.ColumnHeader()
        Me.QtProduzir = New System.Windows.Forms.ColumnHeader()
        Me.DtPrevInicio = New System.Windows.Forms.ColumnHeader()
        Me.QtOps = New System.Windows.Forms.ColumnHeader()
        Me.HoraFinal = New System.Windows.Forms.ColumnHeader()
        Me.HorasNecessarias = New System.Windows.Forms.ColumnHeader()
        Me.NumOP = New System.Windows.Forms.ColumnHeader()
        Me.NumLote = New System.Windows.Forms.ColumnHeader()
        Me.codprograma = New System.Windows.Forms.ColumnHeader()
        Me.qtunitcx = New System.Windows.Forms.ColumnHeader()
        Me.Linha = New System.Windows.Forms.ColumnHeader()
        Me.cboLinha2 = New System.Windows.Forms.ComboBox()
        Me.cboMetodo2 = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.txtQtProduzir2 = New System.Windows.Forms.TextBox()
        Me.txtVelocidadeNominal2 = New System.Windows.Forms.TextBox()
        Me.txtQtdOps2 = New System.Windows.Forms.TextBox()
        Me.txtNumLote = New System.Windows.Forms.TextBox()
        Me.txtNumOP = New System.Windows.Forms.TextBox()
        Me.txtEmbalagem2 = New System.Windows.Forms.TextBox()
        Me.txtDescricao2 = New System.Windows.Forms.TextBox()
        Me.txtCodProd2 = New System.Windows.Forms.TextBox()
        Me.btnImprimirPrograma = New System.Windows.Forms.Button()
        Me.btnImprimirInsumos = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.btnRequisitarMateriais = New System.Windows.Forms.Button()
        Me.btnProgramarSA = New System.Windows.Forms.Button()
        Me.btnProgramar2 = New System.Windows.Forms.Button()
        Me.btnIncluir2 = New System.Windows.Forms.Button()
        Me.panelMenu.SuspendLayout()
        Me.panelProgramar2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button3
        '
        Me.Button3.BackColor = System.Drawing.Color.SlateGray
        Me.Button3.BackgroundImage = Global.SeMSys.My.Resources.Resources.SM_Cor_2000px_300x
        Me.Button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.Button3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Button3.Enabled = False
        Me.Button3.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.Button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button3.ForeColor = System.Drawing.SystemColors.WindowFrame
        Me.Button3.Location = New System.Drawing.Point(0, 677)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(248, 138)
        Me.Button3.TabIndex = 12
        Me.Button3.UseVisualStyleBackColor = False
        '
        'panelMenu
        '
        Me.panelMenu.BackColor = System.Drawing.Color.SlateGray
        Me.panelMenu.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.panelMenu.Controls.Add(Me.Button1)
        Me.panelMenu.Controls.Add(Me.btnRelatorios)
        Me.panelMenu.Controls.Add(Me.btnApontar)
        Me.panelMenu.Controls.Add(Me.btnManutencao)
        Me.panelMenu.Controls.Add(Me.btnProgramar)
        Me.panelMenu.Controls.Add(Me.Button3)
        Me.panelMenu.Dock = System.Windows.Forms.DockStyle.Left
        Me.panelMenu.Location = New System.Drawing.Point(0, 0)
        Me.panelMenu.Name = "panelMenu"
        Me.panelMenu.Size = New System.Drawing.Size(250, 817)
        Me.panelMenu.TabIndex = 20
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.Button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.Button1.FlatAppearance.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.Button1.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ActiveCaption
        Me.Button1.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button1.Font = New System.Drawing.Font("Century Gothic", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.Button1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(25, Byte), Integer), CType(CType(25, Byte), Integer))
        Me.Button1.Image = Global.SeMSys.My.Resources.Resources.settings
        Me.Button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button1.Location = New System.Drawing.Point(-1, 144)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(250, 65)
        Me.Button1.TabIndex = 24
        Me.Button1.Text = "CONFIGURAÇÕES"
        Me.Button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button1.UseVisualStyleBackColor = False
        Me.Button1.Visible = False
        '
        'btnRelatorios
        '
        Me.btnRelatorios.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.btnRelatorios.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.btnRelatorios.FlatAppearance.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.btnRelatorios.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ActiveCaption
        Me.btnRelatorios.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.btnRelatorios.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRelatorios.Font = New System.Drawing.Font("Century Gothic", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.btnRelatorios.ForeColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(25, Byte), Integer), CType(CType(25, Byte), Integer))
        Me.btnRelatorios.Image = Global.SeMSys.My.Resources.Resources.estatisticas
        Me.btnRelatorios.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRelatorios.Location = New System.Drawing.Point(0, 401)
        Me.btnRelatorios.Name = "btnRelatorios"
        Me.btnRelatorios.Size = New System.Drawing.Size(250, 65)
        Me.btnRelatorios.TabIndex = 24
        Me.btnRelatorios.Text = "RELATORIOS"
        Me.btnRelatorios.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnRelatorios.UseVisualStyleBackColor = False
        Me.btnRelatorios.Visible = False
        '
        'btnApontar
        '
        Me.btnApontar.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.btnApontar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.btnApontar.FlatAppearance.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.btnApontar.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ActiveCaption
        Me.btnApontar.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.btnApontar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnApontar.Font = New System.Drawing.Font("Century Gothic", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.btnApontar.ForeColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(25, Byte), Integer), CType(CType(25, Byte), Integer))
        Me.btnApontar.Image = Global.SeMSys.My.Resources.Resources.calendario__1_
        Me.btnApontar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnApontar.Location = New System.Drawing.Point(0, 319)
        Me.btnApontar.Name = "btnApontar"
        Me.btnApontar.Size = New System.Drawing.Size(250, 65)
        Me.btnApontar.TabIndex = 23
        Me.btnApontar.Text = "APONTAR"
        Me.btnApontar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnApontar.UseVisualStyleBackColor = False
        Me.btnApontar.Visible = False
        '
        'btnManutencao
        '
        Me.btnManutencao.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.btnManutencao.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.btnManutencao.FlatAppearance.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.btnManutencao.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ActiveCaption
        Me.btnManutencao.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.btnManutencao.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnManutencao.Font = New System.Drawing.Font("Century Gothic", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.btnManutencao.ForeColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(25, Byte), Integer), CType(CType(25, Byte), Integer))
        Me.btnManutencao.Image = Global.SeMSys.My.Resources.Resources.calendario__1_
        Me.btnManutencao.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnManutencao.Location = New System.Drawing.Point(0, 237)
        Me.btnManutencao.Name = "btnManutencao"
        Me.btnManutencao.Size = New System.Drawing.Size(250, 65)
        Me.btnManutencao.TabIndex = 22
        Me.btnManutencao.Text = "MANUTENCAO"
        Me.btnManutencao.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnManutencao.UseVisualStyleBackColor = False
        Me.btnManutencao.Visible = False
        '
        'btnProgramar
        '
        Me.btnProgramar.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.btnProgramar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.btnProgramar.FlatAppearance.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.btnProgramar.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ActiveCaption
        Me.btnProgramar.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.btnProgramar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnProgramar.Font = New System.Drawing.Font("Century Gothic", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.btnProgramar.ForeColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(25, Byte), Integer), CType(CType(25, Byte), Integer))
        Me.btnProgramar.Image = Global.SeMSys.My.Resources.Resources.calendario__1_
        Me.btnProgramar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnProgramar.Location = New System.Drawing.Point(0, 73)
        Me.btnProgramar.Name = "btnProgramar"
        Me.btnProgramar.Size = New System.Drawing.Size(250, 65)
        Me.btnProgramar.TabIndex = 13
        Me.btnProgramar.Text = "PROGRAMAR"
        Me.btnProgramar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnProgramar.UseVisualStyleBackColor = False
        '
        'panelProgramar2
        '
        Me.panelProgramar2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.panelProgramar2.AutoSize = True
        Me.panelProgramar2.BackColor = System.Drawing.Color.SlateGray
        Me.panelProgramar2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.panelProgramar2.Controls.Add(Me.DtPickerHoraFinal)
        Me.panelProgramar2.Controls.Add(Me.Label6)
        Me.panelProgramar2.Controls.Add(Me.lblFilialProducao)
        Me.panelProgramar2.Controls.Add(Me.lblFilialEstoque)
        Me.panelProgramar2.Controls.Add(Me.TextBox1)
        Me.panelProgramar2.Controls.Add(Me.btnAdicionarIntervalo)
        Me.panelProgramar2.Controls.Add(Me.txtQtunitcx)
        Me.panelProgramar2.Controls.Add(Me.txtEficiencia)
        Me.panelProgramar2.Controls.Add(Me.Button5)
        Me.panelProgramar2.Controls.Add(Me.btnSalvarPrograma)
        Me.panelProgramar2.Controls.Add(Me.btnCarregarPrograma)
        Me.panelProgramar2.Controls.Add(Me.Button4)
        Me.panelProgramar2.Controls.Add(Me.txtCodPrograma)
        Me.panelProgramar2.Controls.Add(Me.btnPesquisar)
        Me.panelProgramar2.Controls.Add(Me.DtPickerHoraInicial)
        Me.panelProgramar2.Controls.Add(Me.listMateriais2)
        Me.panelProgramar2.Controls.Add(Me.listOrdens2)
        Me.panelProgramar2.Controls.Add(Me.cboLinha2)
        Me.panelProgramar2.Controls.Add(Me.cboMetodo2)
        Me.panelProgramar2.Controls.Add(Me.Label2)
        Me.panelProgramar2.Controls.Add(Me.Label3)
        Me.panelProgramar2.Controls.Add(Me.Label19)
        Me.panelProgramar2.Controls.Add(Me.Label18)
        Me.panelProgramar2.Controls.Add(Me.Label17)
        Me.panelProgramar2.Controls.Add(Me.Label21)
        Me.panelProgramar2.Controls.Add(Me.Label20)
        Me.panelProgramar2.Controls.Add(Me.Label16)
        Me.panelProgramar2.Controls.Add(Me.Label5)
        Me.panelProgramar2.Controls.Add(Me.Label4)
        Me.panelProgramar2.Controls.Add(Me.Label15)
        Me.panelProgramar2.Controls.Add(Me.Label14)
        Me.panelProgramar2.Controls.Add(Me.Label1)
        Me.panelProgramar2.Controls.Add(Me.Label13)
        Me.panelProgramar2.Controls.Add(Me.txtQtProduzir2)
        Me.panelProgramar2.Controls.Add(Me.txtVelocidadeNominal2)
        Me.panelProgramar2.Controls.Add(Me.txtQtdOps2)
        Me.panelProgramar2.Controls.Add(Me.txtNumLote)
        Me.panelProgramar2.Controls.Add(Me.txtNumOP)
        Me.panelProgramar2.Controls.Add(Me.txtEmbalagem2)
        Me.panelProgramar2.Controls.Add(Me.txtDescricao2)
        Me.panelProgramar2.Controls.Add(Me.txtCodProd2)
        Me.panelProgramar2.Controls.Add(Me.btnImprimirPrograma)
        Me.panelProgramar2.Controls.Add(Me.btnImprimirInsumos)
        Me.panelProgramar2.Controls.Add(Me.Button2)
        Me.panelProgramar2.Controls.Add(Me.btnRequisitarMateriais)
        Me.panelProgramar2.Controls.Add(Me.btnProgramarSA)
        Me.panelProgramar2.Controls.Add(Me.btnProgramar2)
        Me.panelProgramar2.Controls.Add(Me.btnIncluir2)
        Me.panelProgramar2.Location = New System.Drawing.Point(252, 0)
        Me.panelProgramar2.Name = "panelProgramar2"
        Me.panelProgramar2.Size = New System.Drawing.Size(1393, 1172)
        Me.panelProgramar2.TabIndex = 22
        '
        'DtPickerHoraFinal
        '
        Me.DtPickerHoraFinal.CustomFormat = "dd/MM/yyyy HH:mm:ss"
        Me.DtPickerHoraFinal.Enabled = False
        Me.DtPickerHoraFinal.Location = New System.Drawing.Point(423, 131)
        Me.DtPickerHoraFinal.Name = "DtPickerHoraFinal"
        Me.DtPickerHoraFinal.Size = New System.Drawing.Size(196, 27)
        Me.DtPickerHoraFinal.TabIndex = 39
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label6.Location = New System.Drawing.Point(423, 102)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(85, 20)
        Me.Label6.TabIndex = 40
        Me.Label6.Text = "Dt Prev Fim"
        '
        'lblFilialProducao
        '
        Me.lblFilialProducao.AutoSize = True
        Me.lblFilialProducao.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.lblFilialProducao.Location = New System.Drawing.Point(336, 8)
        Me.lblFilialProducao.Name = "lblFilialProducao"
        Me.lblFilialProducao.Size = New System.Drawing.Size(114, 20)
        Me.lblFilialProducao.TabIndex = 38
        Me.lblFilialProducao.Text = "Filial Produção: "
        '
        'lblFilialEstoque
        '
        Me.lblFilialEstoque.AutoSize = True
        Me.lblFilialEstoque.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.lblFilialEstoque.Location = New System.Drawing.Point(190, 8)
        Me.lblFilialEstoque.Name = "lblFilialEstoque"
        Me.lblFilialEstoque.Size = New System.Drawing.Size(104, 20)
        Me.lblFilialEstoque.TabIndex = 38
        Me.lblFilialEstoque.Text = "Filial Estoque: "
        '
        'TextBox1
        '
        Me.TextBox1.BackColor = System.Drawing.Color.SlateGray
        Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox1.Font = New System.Drawing.Font("Segoe UI", 8.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point)
        Me.TextBox1.ForeColor = System.Drawing.SystemColors.Control
        Me.TextBox1.Location = New System.Drawing.Point(1021, 426)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(353, 113)
        Me.TextBox1.TabIndex = 37
        Me.TextBox1.Text = "Para adicionar um período sem programação explicitamente no programa, utilize o c" &
    "odigo de produto '99999' e na quantidade a produzir, coloque o número de horas p" &
    "aradas com a eficiência em 99,99%"
        '
        'btnAdicionarIntervalo
        '
        Me.btnAdicionarIntervalo.AccessibleName = ""
        Me.btnAdicionarIntervalo.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.btnAdicionarIntervalo.FlatAppearance.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.btnAdicionarIntervalo.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ActiveCaption
        Me.btnAdicionarIntervalo.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.btnAdicionarIntervalo.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAdicionarIntervalo.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.btnAdicionarIntervalo.Image = Global.SeMSys.My.Resources.Resources.plus
        Me.btnAdicionarIntervalo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAdicionarIntervalo.Location = New System.Drawing.Point(1021, 369)
        Me.btnAdicionarIntervalo.Name = "btnAdicionarIntervalo"
        Me.btnAdicionarIntervalo.Size = New System.Drawing.Size(164, 32)
        Me.btnAdicionarIntervalo.TabIndex = 36
        Me.btnAdicionarIntervalo.Tag = "admin"
        Me.btnAdicionarIntervalo.Text = "Adicionar Intervalo"
        Me.btnAdicionarIntervalo.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnAdicionarIntervalo.UseVisualStyleBackColor = False
        Me.btnAdicionarIntervalo.Visible = False
        '
        'txtQtunitcx
        '
        Me.txtQtunitcx.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtQtunitcx.Location = New System.Drawing.Point(1090, 60)
        Me.txtQtunitcx.Multiline = True
        Me.txtQtunitcx.Name = "txtQtunitcx"
        Me.txtQtunitcx.Size = New System.Drawing.Size(70, 29)
        Me.txtQtunitcx.TabIndex = 31
        Me.txtQtunitcx.Visible = False
        '
        'txtEficiencia
        '
        Me.txtEficiencia.Location = New System.Drawing.Point(11, 131)
        Me.txtEficiencia.Mask = "##,##%"
        Me.txtEficiencia.Name = "txtEficiencia"
        Me.txtEficiencia.Size = New System.Drawing.Size(71, 27)
        Me.txtEficiencia.TabIndex = 6
        Me.txtEficiencia.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Button5
        '
        Me.Button5.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.Button5.FlatAppearance.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.Button5.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ActiveCaption
        Me.Button5.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.Button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button5.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Button5.Location = New System.Drawing.Point(114, 3)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(70, 29)
        Me.Button5.TabIndex = 29
        Me.Button5.Tag = "admin"
        Me.Button5.Text = "Limpar"
        Me.Button5.UseVisualStyleBackColor = False
        '
        'btnSalvarPrograma
        '
        Me.btnSalvarPrograma.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.btnSalvarPrograma.FlatAppearance.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.btnSalvarPrograma.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ActiveCaption
        Me.btnSalvarPrograma.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.btnSalvarPrograma.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSalvarPrograma.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.btnSalvarPrograma.Image = Global.SeMSys.My.Resources.Resources.opcao_de_salvar_arquivo
        Me.btnSalvarPrograma.Location = New System.Drawing.Point(79, 2)
        Me.btnSalvarPrograma.Name = "btnSalvarPrograma"
        Me.btnSalvarPrograma.Size = New System.Drawing.Size(29, 29)
        Me.btnSalvarPrograma.TabIndex = 29
        Me.btnSalvarPrograma.Tag = "admin"
        Me.btnSalvarPrograma.UseVisualStyleBackColor = False
        '
        'btnCarregarPrograma
        '
        Me.btnCarregarPrograma.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.btnCarregarPrograma.FlatAppearance.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.btnCarregarPrograma.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ActiveCaption
        Me.btnCarregarPrograma.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.btnCarregarPrograma.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCarregarPrograma.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.btnCarregarPrograma.Location = New System.Drawing.Point(45, 2)
        Me.btnCarregarPrograma.Name = "btnCarregarPrograma"
        Me.btnCarregarPrograma.Size = New System.Drawing.Size(29, 29)
        Me.btnCarregarPrograma.TabIndex = 29
        Me.btnCarregarPrograma.Text = "..."
        Me.btnCarregarPrograma.UseVisualStyleBackColor = False
        '
        'Button4
        '
        Me.Button4.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.Button4.FlatAppearance.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.Button4.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ActiveCaption
        Me.Button4.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.Button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Button4.Image = Global.SeMSys.My.Resources.Resources.plus
        Me.Button4.Location = New System.Drawing.Point(11, 3)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(28, 28)
        Me.Button4.TabIndex = 28
        Me.Button4.Tag = "admin"
        Me.Button4.UseVisualStyleBackColor = False
        '
        'txtCodPrograma
        '
        Me.txtCodPrograma.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtCodPrograma.Location = New System.Drawing.Point(11, 60)
        Me.txtCodPrograma.Multiline = True
        Me.txtCodPrograma.Name = "txtCodPrograma"
        Me.txtCodPrograma.Size = New System.Drawing.Size(80, 29)
        Me.txtCodPrograma.TabIndex = 1
        '
        'btnPesquisar
        '
        Me.btnPesquisar.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.btnPesquisar.FlatAppearance.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.btnPesquisar.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ActiveCaption
        Me.btnPesquisar.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.btnPesquisar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPesquisar.Image = Global.SeMSys.My.Resources.Resources.search
        Me.btnPesquisar.Location = New System.Drawing.Point(211, 60)
        Me.btnPesquisar.Name = "btnPesquisar"
        Me.btnPesquisar.Size = New System.Drawing.Size(51, 29)
        Me.btnPesquisar.TabIndex = 1
        Me.btnPesquisar.Tag = "admin"
        Me.btnPesquisar.UseVisualStyleBackColor = False
        '
        'DtPickerHoraInicial
        '
        Me.DtPickerHoraInicial.CustomFormat = "dd/MM/yyyy HH:mm:ss"
        Me.DtPickerHoraInicial.Location = New System.Drawing.Point(221, 131)
        Me.DtPickerHoraInicial.Name = "DtPickerHoraInicial"
        Me.DtPickerHoraInicial.Size = New System.Drawing.Size(196, 27)
        Me.DtPickerHoraInicial.TabIndex = 8
        '
        'listMateriais2
        '
        Me.listMateriais2.CheckBoxes = True
        Me.listMateriais2.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.CodProdMateriais, Me.DescricaoMateriais, Me.QtNecessariaMateriais, Me.QtEstoqueMateriais, Me.Tipomerc, Me.MetodoSA, Me.codprodmaster, Me.OP, Me.Lote, Me.DataPrevInicio, Me.CodProgMateriais})
        Me.listMateriais2.FullRowSelect = True
        Me.listMateriais2.GridLines = True
        Me.listMateriais2.HideSelection = False
        Me.listMateriais2.Location = New System.Drawing.Point(14, 618)
        Me.listMateriais2.Name = "listMateriais2"
        Me.listMateriais2.Size = New System.Drawing.Size(1001, 325)
        Me.listMateriais2.TabIndex = 24
        Me.listMateriais2.UseCompatibleStateImageBehavior = False
        Me.listMateriais2.View = System.Windows.Forms.View.Details
        '
        'CodProdMateriais
        '
        Me.CodProdMateriais.Text = "Cod Prod"
        Me.CodProdMateriais.Width = 80
        '
        'DescricaoMateriais
        '
        Me.DescricaoMateriais.Text = "Descricao"
        Me.DescricaoMateriais.Width = 300
        '
        'QtNecessariaMateriais
        '
        Me.QtNecessariaMateriais.Text = "Qt Necessidade"
        Me.QtNecessariaMateriais.Width = 120
        '
        'QtEstoqueMateriais
        '
        Me.QtEstoqueMateriais.Text = "Qt Estoque"
        Me.QtEstoqueMateriais.Width = 100
        '
        'Tipomerc
        '
        Me.Tipomerc.Text = "Tipo Merc"
        Me.Tipomerc.Width = 80
        '
        'MetodoSA
        '
        Me.MetodoSA.Text = "Metodo SA"
        Me.MetodoSA.Width = 80
        '
        'codprodmaster
        '
        Me.codprodmaster.Text = "CodProdMaster"
        Me.codprodmaster.Width = 1
        '
        'OP
        '
        Me.OP.Text = "Ordem de Produção"
        Me.OP.Width = 100
        '
        'Lote
        '
        Me.Lote.Text = "Lote"
        Me.Lote.Width = 100
        '
        'DataPrevInicio
        '
        Me.DataPrevInicio.Text = "Data Inicio"
        '
        'CodProgMateriais
        '
        Me.CodProgMateriais.Text = "Cod Programa"
        Me.CodProgMateriais.Width = 20
        '
        'listOrdens2
        '
        Me.listOrdens2.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.CodProd, Me.Descricao, Me.Embalagem, Me.Metodo, Me.QtProduzir, Me.DtPrevInicio, Me.QtOps, Me.HoraFinal, Me.HorasNecessarias, Me.NumOP, Me.NumLote, Me.codprograma, Me.qtunitcx, Me.Linha})
        Me.listOrdens2.FullRowSelect = True
        Me.listOrdens2.GridLines = True
        Me.listOrdens2.HideSelection = False
        Me.listOrdens2.Location = New System.Drawing.Point(14, 220)
        Me.listOrdens2.Name = "listOrdens2"
        Me.listOrdens2.Size = New System.Drawing.Size(1001, 392)
        Me.listOrdens2.TabIndex = 23
        Me.listOrdens2.UseCompatibleStateImageBehavior = False
        Me.listOrdens2.View = System.Windows.Forms.View.Details
        '
        'CodProd
        '
        Me.CodProd.Text = "Codigo"
        Me.CodProd.Width = 80
        '
        'Descricao
        '
        Me.Descricao.Text = "Descricao"
        Me.Descricao.Width = 250
        '
        'Embalagem
        '
        Me.Embalagem.Text = "Embalagem"
        Me.Embalagem.Width = 80
        '
        'Metodo
        '
        Me.Metodo.Text = "Metodo"
        Me.Metodo.Width = 70
        '
        'QtProduzir
        '
        Me.QtProduzir.Text = "Qt Produzir"
        Me.QtProduzir.Width = 90
        '
        'DtPrevInicio
        '
        Me.DtPrevInicio.Text = "Data Inicio"
        Me.DtPrevInicio.Width = 130
        '
        'QtOps
        '
        Me.QtOps.Text = "Qtd OP's"
        Me.QtOps.Width = 0
        '
        'HoraFinal
        '
        Me.HoraFinal.Text = "Hora Final"
        Me.HoraFinal.Width = 130
        '
        'HorasNecessarias
        '
        Me.HorasNecessarias.Text = "Hr Necess."
        Me.HorasNecessarias.Width = 100
        '
        'NumOP
        '
        Me.NumOP.Text = "Num OP"
        Me.NumOP.Width = 100
        '
        'NumLote
        '
        Me.NumLote.Text = "Num Lote"
        Me.NumLote.Width = 100
        '
        'codprograma
        '
        Me.codprograma.Text = "codprograma"
        Me.codprograma.Width = 30
        '
        'qtunitcx
        '
        Me.qtunitcx.Text = "qtunitcx"
        '
        'Linha
        '
        Me.Linha.Text = "Linha"
        '
        'cboLinha2
        '
        Me.cboLinha2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cboLinha2.FormattingEnabled = True
        Me.cboLinha2.Location = New System.Drawing.Point(923, 60)
        Me.cboLinha2.Name = "cboLinha2"
        Me.cboLinha2.Size = New System.Drawing.Size(93, 28)
        Me.cboLinha2.TabIndex = 5
        '
        'cboMetodo2
        '
        Me.cboMetodo2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cboMetodo2.FormattingEnabled = True
        Me.cboMetodo2.ItemHeight = 20
        Me.cboMetodo2.Location = New System.Drawing.Point(728, 60)
        Me.cboMetodo2.Name = "cboMetodo2"
        Me.cboMetodo2.Size = New System.Drawing.Size(93, 28)
        Me.cboMetodo2.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label2.Location = New System.Drawing.Point(10, 101)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(72, 20)
        Me.Label2.TabIndex = 21
        Me.Label2.Text = "Eficiencia"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label3.Location = New System.Drawing.Point(1089, 33)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(62, 20)
        Me.Label3.TabIndex = 21
        Me.Label3.Text = "qtunitcx"
        Me.Label3.Visible = False
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label19.Location = New System.Drawing.Point(1026, 33)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(57, 20)
        Me.Label19.TabIndex = 21
        Me.Label19.Text = "Qt OP's"
        Me.Label19.Visible = False
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label18.Location = New System.Drawing.Point(221, 101)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(97, 20)
        Me.Label18.TabIndex = 21
        Me.Label18.Text = "Dt Prev Inicio"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label17.Location = New System.Drawing.Point(99, 101)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(93, 20)
        Me.Label17.TabIndex = 21
        Me.Label17.Text = "Qtd Produzir"
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label21.Location = New System.Drawing.Point(924, 33)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(44, 20)
        Me.Label21.TabIndex = 21
        Me.Label21.Text = "Linha"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label20.Location = New System.Drawing.Point(828, 33)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(83, 20)
        Me.Label20.TabIndex = 21
        Me.Label20.Text = "Velocidade"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label16.Location = New System.Drawing.Point(729, 34)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(62, 20)
        Me.Label16.TabIndex = 21
        Me.Label16.Text = "Metodo"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label5.Location = New System.Drawing.Point(755, 101)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(70, 20)
        Me.Label5.TabIndex = 21
        Me.Label5.Text = "NumLote"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label4.Location = New System.Drawing.Point(629, 101)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(60, 20)
        Me.Label4.TabIndex = 21
        Me.Label4.Text = "NumOP"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label15.Location = New System.Drawing.Point(617, 34)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(89, 20)
        Me.Label15.TabIndex = 21
        Me.Label15.Text = "Embalagem"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label14.Location = New System.Drawing.Point(268, 34)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(74, 20)
        Me.Label14.TabIndex = 21
        Me.Label14.Text = "Descricao"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label1.Location = New System.Drawing.Point(11, 34)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(71, 20)
        Me.Label1.TabIndex = 21
        Me.Label1.Text = "Cod Prog"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label13.Location = New System.Drawing.Point(99, 34)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(71, 20)
        Me.Label13.TabIndex = 21
        Me.Label13.Text = "Cod Prod"
        '
        'txtQtProduzir2
        '
        Me.txtQtProduzir2.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtQtProduzir2.Location = New System.Drawing.Point(99, 131)
        Me.txtQtProduzir2.Multiline = True
        Me.txtQtProduzir2.Name = "txtQtProduzir2"
        Me.txtQtProduzir2.Size = New System.Drawing.Size(109, 27)
        Me.txtQtProduzir2.TabIndex = 7
        '
        'txtVelocidadeNominal2
        '
        Me.txtVelocidadeNominal2.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtVelocidadeNominal2.Location = New System.Drawing.Point(827, 60)
        Me.txtVelocidadeNominal2.Multiline = True
        Me.txtVelocidadeNominal2.Name = "txtVelocidadeNominal2"
        Me.txtVelocidadeNominal2.Size = New System.Drawing.Size(90, 29)
        Me.txtVelocidadeNominal2.TabIndex = 4
        '
        'txtQtdOps2
        '
        Me.txtQtdOps2.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtQtdOps2.Enabled = False
        Me.txtQtdOps2.Location = New System.Drawing.Point(1026, 61)
        Me.txtQtdOps2.Multiline = True
        Me.txtQtdOps2.Name = "txtQtdOps2"
        Me.txtQtdOps2.Size = New System.Drawing.Size(58, 29)
        Me.txtQtdOps2.TabIndex = 8
        Me.txtQtdOps2.Text = "1"
        Me.txtQtdOps2.Visible = False
        '
        'txtNumLote
        '
        Me.txtNumLote.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtNumLote.Location = New System.Drawing.Point(755, 131)
        Me.txtNumLote.Multiline = True
        Me.txtNumLote.Name = "txtNumLote"
        Me.txtNumLote.Size = New System.Drawing.Size(106, 27)
        Me.txtNumLote.TabIndex = 20
        '
        'txtNumOP
        '
        Me.txtNumOP.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtNumOP.Location = New System.Drawing.Point(629, 131)
        Me.txtNumOP.Multiline = True
        Me.txtNumOP.Name = "txtNumOP"
        Me.txtNumOP.Size = New System.Drawing.Size(106, 27)
        Me.txtNumOP.TabIndex = 20
        '
        'txtEmbalagem2
        '
        Me.txtEmbalagem2.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtEmbalagem2.Location = New System.Drawing.Point(616, 60)
        Me.txtEmbalagem2.Multiline = True
        Me.txtEmbalagem2.Name = "txtEmbalagem2"
        Me.txtEmbalagem2.Size = New System.Drawing.Size(106, 29)
        Me.txtEmbalagem2.TabIndex = 20
        '
        'txtDescricao2
        '
        Me.txtDescricao2.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtDescricao2.Location = New System.Drawing.Point(268, 60)
        Me.txtDescricao2.Multiline = True
        Me.txtDescricao2.Name = "txtDescricao2"
        Me.txtDescricao2.Size = New System.Drawing.Size(342, 29)
        Me.txtDescricao2.TabIndex = 2
        '
        'txtCodProd2
        '
        Me.txtCodProd2.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtCodProd2.Location = New System.Drawing.Point(99, 60)
        Me.txtCodProd2.Multiline = True
        Me.txtCodProd2.Name = "txtCodProd2"
        Me.txtCodProd2.Size = New System.Drawing.Size(106, 29)
        Me.txtCodProd2.TabIndex = 2
        '
        'btnImprimirPrograma
        '
        Me.btnImprimirPrograma.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.btnImprimirPrograma.FlatAppearance.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.btnImprimirPrograma.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ActiveCaption
        Me.btnImprimirPrograma.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.btnImprimirPrograma.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnImprimirPrograma.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.btnImprimirPrograma.Image = Global.SeMSys.My.Resources.Resources.estatisticas
        Me.btnImprimirPrograma.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnImprimirPrograma.Location = New System.Drawing.Point(1021, 324)
        Me.btnImprimirPrograma.Name = "btnImprimirPrograma"
        Me.btnImprimirPrograma.Size = New System.Drawing.Size(164, 32)
        Me.btnImprimirPrograma.TabIndex = 15
        Me.btnImprimirPrograma.Text = "Imprimir Programa"
        Me.btnImprimirPrograma.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnImprimirPrograma.UseVisualStyleBackColor = False
        '
        'btnImprimirInsumos
        '
        Me.btnImprimirInsumos.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.btnImprimirInsumos.FlatAppearance.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.btnImprimirInsumos.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ActiveCaption
        Me.btnImprimirInsumos.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.btnImprimirInsumos.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnImprimirInsumos.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.btnImprimirInsumos.Image = Global.SeMSys.My.Resources.Resources.estatisticas
        Me.btnImprimirInsumos.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnImprimirInsumos.Location = New System.Drawing.Point(1026, 675)
        Me.btnImprimirInsumos.Name = "btnImprimirInsumos"
        Me.btnImprimirInsumos.Size = New System.Drawing.Size(164, 32)
        Me.btnImprimirInsumos.TabIndex = 15
        Me.btnImprimirInsumos.Tag = "admin"
        Me.btnImprimirInsumos.Text = "Imprimir Insumos"
        Me.btnImprimirInsumos.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnImprimirInsumos.UseVisualStyleBackColor = False
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.Button2.FlatAppearance.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.Button2.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ActiveCaption
        Me.Button2.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.Button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Button2.Image = Global.SeMSys.My.Resources.Resources.cruz
        Me.Button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button2.Location = New System.Drawing.Point(1028, 119)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(136, 39)
        Me.Button2.TabIndex = 10
        Me.Button2.Tag = "admin"
        Me.Button2.Text = "Excluir"
        Me.Button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button2.UseVisualStyleBackColor = False
        '
        'btnRequisitarMateriais
        '
        Me.btnRequisitarMateriais.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.btnRequisitarMateriais.FlatAppearance.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.btnRequisitarMateriais.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ActiveCaption
        Me.btnRequisitarMateriais.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.btnRequisitarMateriais.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRequisitarMateriais.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.btnRequisitarMateriais.Image = Global.SeMSys.My.Resources.Resources.lista
        Me.btnRequisitarMateriais.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRequisitarMateriais.Location = New System.Drawing.Point(1021, 220)
        Me.btnRequisitarMateriais.Name = "btnRequisitarMateriais"
        Me.btnRequisitarMateriais.Size = New System.Drawing.Size(164, 39)
        Me.btnRequisitarMateriais.TabIndex = 11
        Me.btnRequisitarMateriais.Tag = "admin"
        Me.btnRequisitarMateriais.Text = "Nec. Materiais"
        Me.btnRequisitarMateriais.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnRequisitarMateriais.UseVisualStyleBackColor = False
        '
        'btnProgramarSA
        '
        Me.btnProgramarSA.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.btnProgramarSA.FlatAppearance.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.btnProgramarSA.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ActiveCaption
        Me.btnProgramarSA.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.btnProgramarSA.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnProgramarSA.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.btnProgramarSA.Image = Global.SeMSys.My.Resources.Resources.download__1_
        Me.btnProgramarSA.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnProgramarSA.Location = New System.Drawing.Point(1026, 618)
        Me.btnProgramarSA.Name = "btnProgramarSA"
        Me.btnProgramarSA.Size = New System.Drawing.Size(164, 39)
        Me.btnProgramarSA.TabIndex = 12
        Me.btnProgramarSA.Tag = "admin"
        Me.btnProgramarSA.Text = "Programar SA's"
        Me.btnProgramarSA.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnProgramarSA.UseVisualStyleBackColor = False
        '
        'btnProgramar2
        '
        Me.btnProgramar2.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.btnProgramar2.FlatAppearance.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.btnProgramar2.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ActiveCaption
        Me.btnProgramar2.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.btnProgramar2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnProgramar2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.btnProgramar2.Image = Global.SeMSys.My.Resources.Resources.download__1_
        Me.btnProgramar2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnProgramar2.Location = New System.Drawing.Point(1021, 274)
        Me.btnProgramar2.Name = "btnProgramar2"
        Me.btnProgramar2.Size = New System.Drawing.Size(164, 39)
        Me.btnProgramar2.TabIndex = 12
        Me.btnProgramar2.Tag = "admin"
        Me.btnProgramar2.Text = "Programar"
        Me.btnProgramar2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnProgramar2.UseVisualStyleBackColor = False
        '
        'btnIncluir2
        '
        Me.btnIncluir2.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.btnIncluir2.FlatAppearance.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.btnIncluir2.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ActiveCaption
        Me.btnIncluir2.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.btnIncluir2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnIncluir2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.btnIncluir2.Image = Global.SeMSys.My.Resources.Resources.verificar
        Me.btnIncluir2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnIncluir2.Location = New System.Drawing.Point(877, 119)
        Me.btnIncluir2.Name = "btnIncluir2"
        Me.btnIncluir2.Size = New System.Drawing.Size(136, 39)
        Me.btnIncluir2.TabIndex = 9
        Me.btnIncluir2.Tag = "admin"
        Me.btnIncluir2.Text = "Incluir "
        Me.btnIncluir2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnIncluir2.UseVisualStyleBackColor = False
        '
        'frmProgramarProducao
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Gainsboro
        Me.ClientSize = New System.Drawing.Size(1636, 817)
        Me.Controls.Add(Me.panelProgramar2)
        Me.Controls.Add(Me.panelMenu)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmProgramarProducao"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Programar Produção"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.panelMenu.ResumeLayout(False)
        Me.panelProgramar2.ResumeLayout(False)
        Me.panelProgramar2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button3 As Button
    Friend WithEvents panelMenu As Panel
    Friend WithEvents btnProgramar As Button

    Friend WithEvents btnIniciar As Button
    Friend WithEvents btnRelatorios As Button
    Friend WithEvents btnApontar As Button
    Friend WithEvents btnManutencao As Button
    Friend WithEvents panelProgramar2 As Panel
    Friend WithEvents btnIncluir2 As Button
    Friend WithEvents listOrdens2 As ListView
    Friend WithEvents cboMetodo2 As ComboBox
    Friend WithEvents Label13 As Label
    Friend WithEvents txtQtProduzir2 As TextBox
    Friend WithEvents txtEmbalagem2 As TextBox
    Friend WithEvents txtDescricao2 As TextBox
    Friend WithEvents Label19 As Label
    Friend WithEvents Label18 As Label
    Friend WithEvents Label17 As Label
    Friend WithEvents Label16 As Label
    Friend WithEvents Label15 As Label
    Friend WithEvents Label14 As Label
    Friend WithEvents txtQtdOps2 As TextBox
    Friend WithEvents CodProd As ColumnHeader
    Friend WithEvents Descricao As ColumnHeader
    Friend WithEvents Embalagem As ColumnHeader
    Friend WithEvents Metodo As ColumnHeader
    Friend WithEvents QtProduzir As ColumnHeader
    Friend WithEvents DtPrevInicio As ColumnHeader
    Friend WithEvents QtOps As ColumnHeader
    Friend WithEvents btnRequisitarMateriais As Button
    Friend WithEvents btnProgramar2 As Button
    Friend WithEvents listMateriais2 As ListView
    Friend WithEvents CodProdMateriais As ColumnHeader
    Friend WithEvents DescricaoMateriais As ColumnHeader
    Friend WithEvents QtNecessariaMateriais As ColumnHeader
    Friend WithEvents QtEstoqueMateriais As ColumnHeader
    Friend WithEvents txtVelocidadeNominal2 As TextBox
    Friend WithEvents DtPickerHoraInicial As DateTimePicker
    Friend WithEvents HoraFinal As ColumnHeader
    Friend WithEvents HorasNecessarias As ColumnHeader
    Friend WithEvents Button2 As Button
    Friend WithEvents cboLinha2 As ComboBox
    Friend WithEvents Label21 As Label
    Friend WithEvents Label20 As Label
    Friend WithEvents Tipomerc As ColumnHeader
    Friend WithEvents MetodoSA As ColumnHeader
    Friend WithEvents btnImprimirInsumos As Button
    Friend WithEvents codprodmaster As ColumnHeader
    Friend WithEvents NumOP As ColumnHeader
    Friend WithEvents NumLote As ColumnHeader
    Friend WithEvents Button1 As Button
    Friend WithEvents btnPesquisar As Button
    Friend WithEvents btnImprimirPrograma As Button
    Friend WithEvents Button4 As Button
    Friend WithEvents txtCodPrograma As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents btnCarregarPrograma As Button
    Friend WithEvents Button5 As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents txtEficiencia As MaskedTextBox
    Friend WithEvents btnSalvarPrograma As Button
    Friend WithEvents codprograma As ColumnHeader
    Friend WithEvents qtunitcx As ColumnHeader
    Friend WithEvents txtQtunitcx As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Linha As ColumnHeader
    Friend WithEvents btnAdicionarIntervalo As Button
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents OP As ColumnHeader
    Friend WithEvents Lote As ColumnHeader
    Friend WithEvents btnProgramarSA As Button
    Friend WithEvents DataPrevInicio As ColumnHeader
    Friend WithEvents CodProgMateriais As ColumnHeader
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents txtNumLote As TextBox
    Friend WithEvents txtNumOP As TextBox
    Friend WithEvents lblFilialProducao As Label
    Friend WithEvents lblFilialEstoque As Label
    Friend WithEvents DtPickerHoraFinal As DateTimePicker
    Friend WithEvents Label6 As Label
    Public WithEvents txtCodProd2 As TextBox
End Class
