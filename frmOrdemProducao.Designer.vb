<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOrdemProducao
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOrdemProducao))
        Me.txtNumOP = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnImprimir = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtOffset = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'txtNumOP
        '
        Me.txtNumOP.Location = New System.Drawing.Point(123, 43)
        Me.txtNumOP.Name = "txtNumOP"
        Me.txtNumOP.Size = New System.Drawing.Size(197, 27)
        Me.txtNumOP.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(123, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(64, 20)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Num OP"
        '
        'btnImprimir
        '
        Me.btnImprimir.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.btnImprimir.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaption
        Me.btnImprimir.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.InactiveCaption
        Me.btnImprimir.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnImprimir.Image = Global.SeMSys.My.Resources.Resources.lista_de_papel
        Me.btnImprimir.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnImprimir.Location = New System.Drawing.Point(123, 142)
        Me.btnImprimir.Name = "btnImprimir"
        Me.btnImprimir.Size = New System.Drawing.Size(197, 55)
        Me.btnImprimir.TabIndex = 2
        Me.btnImprimir.Text = "Imprimir OP"
        Me.btnImprimir.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(122, 79)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(82, 20)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Offset Lote"
        '
        'txtOffset
        '
        Me.txtOffset.Location = New System.Drawing.Point(122, 102)
        Me.txtOffset.Name = "txtOffset"
        Me.txtOffset.Size = New System.Drawing.Size(197, 27)
        Me.txtOffset.TabIndex = 3
        '
        'frmOrdemProducao
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.SlateGray
        Me.ClientSize = New System.Drawing.Size(441, 209)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtOffset)
        Me.Controls.Add(Me.btnImprimir)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtNumOP)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmOrdemProducao"
        Me.Text = "Imprimir Ordem de Produção"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtNumOP As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents btnImprimir As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents txtOffset As TextBox
End Class
