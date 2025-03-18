<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMetodoMRP
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMetodoMRP))
        Me.lblCodigo = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblDescricao = New System.Windows.Forms.Label()
        Me.cboMetodos = New System.Windows.Forms.ComboBox()
        Me.btnDefinirMetodo = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lblCodigo
        '
        Me.lblCodigo.AutoSize = True
        Me.lblCodigo.Location = New System.Drawing.Point(12, 9)
        Me.lblCodigo.Name = "lblCodigo"
        Me.lblCodigo.Size = New System.Drawing.Size(53, 20)
        Me.lblCodigo.TabIndex = 0
        Me.lblCodigo.Text = "Label1"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 38)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(53, 20)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Label1"
        '
        'lblDescricao
        '
        Me.lblDescricao.AutoSize = True
        Me.lblDescricao.Location = New System.Drawing.Point(12, 38)
        Me.lblDescricao.Name = "lblDescricao"
        Me.lblDescricao.Size = New System.Drawing.Size(53, 20)
        Me.lblDescricao.TabIndex = 0
        Me.lblDescricao.Text = "Label1"
        '
        'cboMetodos
        '
        Me.cboMetodos.FormattingEnabled = True
        Me.cboMetodos.Location = New System.Drawing.Point(12, 61)
        Me.cboMetodos.Name = "cboMetodos"
        Me.cboMetodos.Size = New System.Drawing.Size(208, 28)
        Me.cboMetodos.TabIndex = 1
        '
        'btnDefinirMetodo
        '
        Me.btnDefinirMetodo.Location = New System.Drawing.Point(238, 61)
        Me.btnDefinirMetodo.Name = "btnDefinirMetodo"
        Me.btnDefinirMetodo.Size = New System.Drawing.Size(94, 29)
        Me.btnDefinirMetodo.TabIndex = 2
        Me.btnDefinirMetodo.Text = "Definir Metodo"
        Me.btnDefinirMetodo.UseVisualStyleBackColor = True
        '
        'frmMetodoMRP
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(455, 108)
        Me.Controls.Add(Me.btnDefinirMetodo)
        Me.Controls.Add(Me.cboMetodos)
        Me.Controls.Add(Me.lblDescricao)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lblCodigo)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmMetodoMRP"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Inserir Metodo"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblCodigo As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents lblDescricao As Label
    Friend WithEvents cboMetodos As ComboBox
    Friend WithEvents btnDefinirMetodo As Button
End Class
