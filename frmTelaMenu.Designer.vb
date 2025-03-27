<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmTelaMenu
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTelaMenu))
        Me.lblVersao = New System.Windows.Forms.Label()
        Me.lblUsuario = New System.Windows.Forms.Label()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.ts1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ts11 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ts12 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ts13 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ts14 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ts15 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ts16 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ts17 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ts171 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ts172 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ts173 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ts174 = New System.Windows.Forms.ToolStripMenuItem()
        Me.RelResumoDeOPsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RelOrdemDeProduçãoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ts2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ts21 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ts22 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ts23 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ts3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ts4 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ts41 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ts42 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ts43 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ts44 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ts45 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ts46 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ts47 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MaintSystemToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConfiguraçõesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TROToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblVersao
        '
        Me.lblVersao.AutoSize = True
        Me.lblVersao.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblVersao.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.lblVersao.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.lblVersao.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblVersao.Location = New System.Drawing.Point(0, 586)
        Me.lblVersao.Name = "lblVersao"
        Me.lblVersao.Size = New System.Drawing.Size(98, 17)
        Me.lblVersao.TabIndex = 0
        Me.lblVersao.Text = "Versao x.x.x"
        '
        'lblUsuario
        '
        Me.lblUsuario.AutoSize = True
        Me.lblUsuario.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblUsuario.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.lblUsuario.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.lblUsuario.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblUsuario.Location = New System.Drawing.Point(0, 569)
        Me.lblUsuario.Name = "lblUsuario"
        Me.lblUsuario.Size = New System.Drawing.Size(74, 17)
        Me.lblUsuario.TabIndex = 1
        Me.lblUsuario.Text = "Usuario: "
        '
        'MenuStrip1
        '
        Me.MenuStrip1.BackColor = System.Drawing.Color.SlateGray
        Me.MenuStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ts1, Me.ts2, Me.ts3, Me.ts4, Me.MaintSystemToolStripMenuItem, Me.ConfiguraçõesToolStripMenuItem, Me.TROToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1473, 28)
        Me.MenuStrip1.TabIndex = 2
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'ts1
        '
        Me.ts1.BackColor = System.Drawing.Color.SlateGray
        Me.ts1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ts11, Me.ts12, Me.ts13, Me.ts14, Me.ts15, Me.ts16, Me.ts17})
        Me.ts1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.ts1.Image = Global.SeMSys.My.Resources.Resources.fabrica__1_
        Me.ts1.Name = "ts1"
        Me.ts1.Size = New System.Drawing.Size(177, 24)
        Me.ts1.Tag = "admin,supervisor,user"
        Me.ts1.Text = "Gestão da Produção"
        Me.ts1.Visible = False
        '
        'ts11
        '
        Me.ts11.Name = "ts11"
        Me.ts11.Size = New System.Drawing.Size(318, 26)
        Me.ts11.Tag = "admin"
        Me.ts11.Text = "Iniciar Produção"
        Me.ts11.Visible = False
        '
        'ts12
        '
        Me.ts12.Name = "ts12"
        Me.ts12.Size = New System.Drawing.Size(318, 26)
        Me.ts12.Tag = "admin,supervisor"
        Me.ts12.Text = "Apontar Produção"
        Me.ts12.Visible = False
        '
        'ts13
        '
        Me.ts13.Name = "ts13"
        Me.ts13.Size = New System.Drawing.Size(318, 26)
        Me.ts13.Tag = "admin,supervisor,user"
        Me.ts13.Text = "Requisitar insumos para produção"
        '
        'ts14
        '
        Me.ts14.Name = "ts14"
        Me.ts14.Size = New System.Drawing.Size(318, 26)
        Me.ts14.Tag = "admin,supervisor"
        Me.ts14.Text = "Iniciar Monitoramento Online"
        Me.ts14.Visible = False
        '
        'ts15
        '
        Me.ts15.Name = "ts15"
        Me.ts15.Size = New System.Drawing.Size(318, 26)
        Me.ts15.Tag = "admin,supervisor,user"
        Me.ts15.Text = "Visualizar monitoramento Online"
        Me.ts15.Visible = False
        '
        'ts16
        '
        Me.ts16.Name = "ts16"
        Me.ts16.Size = New System.Drawing.Size(318, 26)
        Me.ts16.Tag = "admin"
        Me.ts16.Text = "Justificar Paradas de Linha"
        Me.ts16.Visible = False
        '
        'ts17
        '
        Me.ts17.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ts171, Me.ts172, Me.ts173, Me.ts174, Me.RelResumoDeOPsToolStripMenuItem, Me.RelOrdemDeProduçãoToolStripMenuItem})
        Me.ts17.Name = "ts17"
        Me.ts17.Size = New System.Drawing.Size(318, 26)
        Me.ts17.Tag = "admin,supervisor,user"
        Me.ts17.Text = "Relatorios"
        Me.ts17.Visible = False
        '
        'ts171
        '
        Me.ts171.Name = "ts171"
        Me.ts171.Size = New System.Drawing.Size(351, 26)
        Me.ts171.Text = "Rel Produção Consolidada por período"
        '
        'ts172
        '
        Me.ts172.Name = "ts172"
        Me.ts172.Size = New System.Drawing.Size(351, 26)
        Me.ts172.Text = "Rel Perdas do processo por Periodo"
        Me.ts172.Visible = False
        '
        'ts173
        '
        Me.ts173.Name = "ts173"
        Me.ts173.Size = New System.Drawing.Size(351, 26)
        Me.ts173.Text = "Rel Registro de produto não conforme"
        Me.ts173.Visible = False
        '
        'ts174
        '
        Me.ts174.Name = "ts174"
        Me.ts174.Size = New System.Drawing.Size(351, 26)
        Me.ts174.Text = "Rel Paradas de Processo por OP"
        '
        'RelResumoDeOPsToolStripMenuItem
        '
        Me.RelResumoDeOPsToolStripMenuItem.Name = "RelResumoDeOPsToolStripMenuItem"
        Me.RelResumoDeOPsToolStripMenuItem.Size = New System.Drawing.Size(351, 26)
        Me.RelResumoDeOPsToolStripMenuItem.Text = "Rel Resumo de OPs"
        '
        'RelOrdemDeProduçãoToolStripMenuItem
        '
        Me.RelOrdemDeProduçãoToolStripMenuItem.Name = "RelOrdemDeProduçãoToolStripMenuItem"
        Me.RelOrdemDeProduçãoToolStripMenuItem.Size = New System.Drawing.Size(351, 26)
        Me.RelOrdemDeProduçãoToolStripMenuItem.Text = "Rel Ordem de Produção"
        '
        'ts2
        '
        Me.ts2.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ts21, Me.ts22, Me.ts23})
        Me.ts2.Image = Global.SeMSys.My.Resources.Resources.reducao
        Me.ts2.Name = "ts2"
        Me.ts2.Size = New System.Drawing.Size(230, 24)
        Me.ts2.Tag = "admin"
        Me.ts2.Text = "Gerenciar Custos - Produção"
        Me.ts2.Visible = False
        '
        'ts21
        '
        Me.ts21.Name = "ts21"
        Me.ts21.Size = New System.Drawing.Size(393, 26)
        Me.ts21.Tag = "admin"
        Me.ts21.Text = "Associação de centros de custo"
        Me.ts21.Visible = False
        '
        'ts22
        '
        Me.ts22.Name = "ts22"
        Me.ts22.Size = New System.Drawing.Size(393, 26)
        Me.ts22.Tag = "admin"
        Me.ts22.Text = "Simulação de custo unitário"
        Me.ts22.Visible = False
        '
        'ts23
        '
        Me.ts23.Name = "ts23"
        Me.ts23.Size = New System.Drawing.Size(393, 26)
        Me.ts23.Tag = "admin"
        Me.ts23.Text = "Fechamento de competência - custo/produto"
        Me.ts23.Visible = False
        '
        'ts3
        '
        Me.ts3.Image = Global.SeMSys.My.Resources.Resources.lista_de_papel
        Me.ts3.Name = "ts3"
        Me.ts3.Size = New System.Drawing.Size(288, 24)
        Me.ts3.Tag = "dev"
        Me.ts3.Text = "Requisiçao de Materiais de Consumo"
        Me.ts3.Visible = False
        '
        'ts4
        '
        Me.ts4.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ts41, Me.ts42, Me.ts43, Me.ts44, Me.ts45, Me.ts46, Me.ts47})
        Me.ts4.Image = Global.SeMSys.My.Resources.Resources.algoritmo__1_
        Me.ts4.Name = "ts4"
        Me.ts4.Size = New System.Drawing.Size(68, 24)
        Me.ts4.Tag = "admin,supervisor"
        Me.ts4.Text = "PCP"
        Me.ts4.Visible = False
        '
        'ts41
        '
        Me.ts41.Image = Global.SeMSys.My.Resources.Resources.settings
        Me.ts41.Name = "ts41"
        Me.ts41.Size = New System.Drawing.Size(365, 26)
        Me.ts41.Tag = "admin,supervisor"
        Me.ts41.Text = "Programar Produção"
        Me.ts41.Visible = False
        '
        'ts42
        '
        Me.ts42.Name = "ts42"
        Me.ts42.Size = New System.Drawing.Size(365, 26)
        Me.ts42.Tag = "admin,supervisor"
        Me.ts42.Text = "Iniciar Produção"
        Me.ts42.Visible = False
        '
        'ts43
        '
        Me.ts43.Name = "ts43"
        Me.ts43.Size = New System.Drawing.Size(365, 26)
        Me.ts43.Tag = "admin,supervisor"
        Me.ts43.Text = "Apontar Produção"
        Me.ts43.Visible = False
        '
        'ts44
        '
        Me.ts44.Name = "ts44"
        Me.ts44.Size = New System.Drawing.Size(365, 26)
        Me.ts44.Tag = "admin"
        Me.ts44.Text = "Cancelar Produção"
        Me.ts44.Visible = False
        '
        'ts45
        '
        Me.ts45.Name = "ts45"
        Me.ts45.Size = New System.Drawing.Size(365, 26)
        Me.ts45.Tag = "admin"
        Me.ts45.Text = "Gerenciar Materiais"
        Me.ts45.Visible = False
        '
        'ts46
        '
        Me.ts46.Name = "ts46"
        Me.ts46.Size = New System.Drawing.Size(365, 26)
        Me.ts46.Tag = "admin,supervisor"
        Me.ts46.Text = "Simulação de consumo de matéria prima"
        Me.ts46.Visible = False
        '
        'ts47
        '
        Me.ts47.Name = "ts47"
        Me.ts47.Size = New System.Drawing.Size(365, 26)
        Me.ts47.Tag = "admin,supervisor"
        Me.ts47.Text = "Manutenção da Ordem de Produção"
        Me.ts47.Visible = False
        '
        'MaintSystemToolStripMenuItem
        '
        Me.MaintSystemToolStripMenuItem.Image = Global.SeMSys.My.Resources.Resources.settings
        Me.MaintSystemToolStripMenuItem.Name = "MaintSystemToolStripMenuItem"
        Me.MaintSystemToolStripMenuItem.Size = New System.Drawing.Size(189, 24)
        Me.MaintSystemToolStripMenuItem.Tag = ""
        Me.MaintSystemToolStripMenuItem.Text = "Solicitação de Serviço"
        '
        'ConfiguraçõesToolStripMenuItem
        '
        Me.ConfiguraçõesToolStripMenuItem.Image = Global.SeMSys.My.Resources.Resources.contexto
        Me.ConfiguraçõesToolStripMenuItem.Name = "ConfiguraçõesToolStripMenuItem"
        Me.ConfiguraçõesToolStripMenuItem.Size = New System.Drawing.Size(138, 24)
        Me.ConfiguraçõesToolStripMenuItem.Text = "Configurações"
        '
        'TROToolStripMenuItem
        '
        Me.TROToolStripMenuItem.Image = Global.SeMSys.My.Resources.Resources.lista_de_papel
        Me.TROToolStripMenuItem.Name = "TROToolStripMenuItem"
        Me.TROToolStripMenuItem.Size = New System.Drawing.Size(165, 24)
        Me.TROToolStripMenuItem.Tag = "financeiro"
        Me.TROToolStripMenuItem.Text = "Troca Natureza NF"
        '
        'frmTelaMenu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(120.0!, 120.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.SlateGray
        Me.BackgroundImage = Global.SeMSys.My.Resources.Resources.SEMSYS_SEM_FUNDO_11
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.ClientSize = New System.Drawing.Size(1473, 603)
        Me.Controls.Add(Me.lblUsuario)
        Me.Controls.Add(Me.lblVersao)
        Me.Controls.Add(Me.MenuStrip1)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.MaximizeBox = False
        Me.Name = "frmTelaMenu"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SeMSys - Gestão da Produção"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblVersao As Label
    Friend WithEvents lblUsuario As Label
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents ts1 As ToolStripMenuItem
    Friend WithEvents ts11 As ToolStripMenuItem
    Friend WithEvents ts12 As ToolStripMenuItem
    Friend WithEvents ts17 As ToolStripMenuItem
    Friend WithEvents ts171 As ToolStripMenuItem
    Friend WithEvents ts172 As ToolStripMenuItem
    Friend WithEvents ts173 As ToolStripMenuItem
    Friend WithEvents ts2 As ToolStripMenuItem
    Friend WithEvents ts21 As ToolStripMenuItem
    Friend WithEvents ts22 As ToolStripMenuItem
    Friend WithEvents ts23 As ToolStripMenuItem
    Friend WithEvents ts3 As ToolStripMenuItem
    Friend WithEvents ts13 As ToolStripMenuItem
    Friend WithEvents ts15 As ToolStripMenuItem
    Friend WithEvents ts16 As ToolStripMenuItem
    Friend WithEvents ts4 As ToolStripMenuItem
    Friend WithEvents ts41 As ToolStripMenuItem
    Friend WithEvents ts14 As ToolStripMenuItem
    Friend WithEvents ts42 As ToolStripMenuItem
    Friend WithEvents ts43 As ToolStripMenuItem
    Friend WithEvents ts44 As ToolStripMenuItem
    Friend WithEvents ts45 As ToolStripMenuItem
    Friend WithEvents ts46 As ToolStripMenuItem
    Friend WithEvents ts47 As ToolStripMenuItem
    Friend WithEvents ts174 As ToolStripMenuItem
    Friend WithEvents MaintSystemToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ConfiguraçõesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RelResumoDeOPsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TROToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RelOrdemDeProduçãoToolStripMenuItem As ToolStripMenuItem
End Class
