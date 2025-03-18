<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSelecionarParada
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSelecionarParada))
        Me.cboArea = New System.Windows.Forms.ComboBox()
        Me.cboEquipamento = New System.Windows.Forms.ComboBox()
        Me.cboComponente = New System.Windows.Forms.ComboBox()
        Me.cboTipoFalha = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lblCod = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.ttxMotivo = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'cboArea
        '
        Me.cboArea.FormattingEnabled = True
        Me.cboArea.Location = New System.Drawing.Point(71, 79)
        Me.cboArea.Name = "cboArea"
        Me.cboArea.Size = New System.Drawing.Size(400, 28)
        Me.cboArea.TabIndex = 0
        '
        'cboEquipamento
        '
        Me.cboEquipamento.FormattingEnabled = True
        Me.cboEquipamento.Location = New System.Drawing.Point(71, 141)
        Me.cboEquipamento.Name = "cboEquipamento"
        Me.cboEquipamento.Size = New System.Drawing.Size(400, 28)
        Me.cboEquipamento.TabIndex = 1
        '
        'cboComponente
        '
        Me.cboComponente.FormattingEnabled = True
        Me.cboComponente.Location = New System.Drawing.Point(71, 198)
        Me.cboComponente.Name = "cboComponente"
        Me.cboComponente.Size = New System.Drawing.Size(400, 28)
        Me.cboComponente.TabIndex = 2
        '
        'cboTipoFalha
        '
        Me.cboTipoFalha.FormattingEnabled = True
        Me.cboTipoFalha.Location = New System.Drawing.Point(71, 253)
        Me.cboTipoFalha.Name = "cboTipoFalha"
        Me.cboTipoFalha.Size = New System.Drawing.Size(400, 28)
        Me.cboTipoFalha.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(71, 53)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(40, 20)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Área"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(71, 118)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(98, 20)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Equipamento"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(71, 175)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(95, 20)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Componente"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(71, 230)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(97, 20)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Tipo de falha"
        '
        'lblCod
        '
        Me.lblCod.AutoSize = True
        Me.lblCod.Location = New System.Drawing.Point(71, 9)
        Me.lblCod.Name = "lblCod"
        Me.lblCod.Size = New System.Drawing.Size(40, 20)
        Me.lblCod.TabIndex = 8
        Me.lblCod.Text = "Área"
        Me.lblCod.Visible = False
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(70, 420)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(400, 51)
        Me.Button1.TabIndex = 9
        Me.Button1.Text = "Confirmar Lançamento"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'ttxMotivo
        '
        Me.ttxMotivo.Location = New System.Drawing.Point(70, 316)
        Me.ttxMotivo.Multiline = True
        Me.ttxMotivo.Name = "ttxMotivo"
        Me.ttxMotivo.Size = New System.Drawing.Size(400, 98)
        Me.ttxMotivo.TabIndex = 10
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(71, 293)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(56, 20)
        Me.Label5.TabIndex = 11
        Me.Label5.Text = "Motivo"
        '
        'frmSelecionarParada
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.SlateGray
        Me.ClientSize = New System.Drawing.Size(587, 502)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.ttxMotivo)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.lblCod)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cboTipoFalha)
        Me.Controls.Add(Me.cboComponente)
        Me.Controls.Add(Me.cboEquipamento)
        Me.Controls.Add(Me.cboArea)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmSelecionarParada"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Selecionar Tipo de Falha"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents cboArea As ComboBox
    Friend WithEvents cboEquipamento As ComboBox
    Friend WithEvents cboComponente As ComboBox
    Friend WithEvents cboTipoFalha As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents lblCod As Label
    Friend WithEvents Button1 As Button
    Friend WithEvents ttxMotivo As TextBox
    Friend WithEvents Label5 As Label
End Class
