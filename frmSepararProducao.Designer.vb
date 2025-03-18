<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSepararProducao
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSepararProducao))
        Me.listCabecalhoOPs = New System.Windows.Forms.ListView()
        Me.NumOP = New System.Windows.Forms.ColumnHeader()
        Me.CodFilial = New System.Windows.Forms.ColumnHeader()
        Me.CodProd = New System.Windows.Forms.ColumnHeader()
        Me.Descricao = New System.Windows.Forms.ColumnHeader()
        Me.TipoMerc = New System.Windows.Forms.ColumnHeader()
        Me.Unidade = New System.Windows.Forms.ColumnHeader()
        Me.Embalagem = New System.Windows.Forms.ColumnHeader()
        Me.Metodo = New System.Windows.Forms.ColumnHeader()
        Me.QtProduzir = New System.Windows.Forms.ColumnHeader()
        Me.QtProduzida = New System.Windows.Forms.ColumnHeader()
        Me.DtLanc = New System.Windows.Forms.ColumnHeader()
        Me.Posicao = New System.Windows.Forms.ColumnHeader()
        Me.QtHoras = New System.Windows.Forms.ColumnHeader()
        Me.FinalizaProducao = New System.Windows.Forms.ColumnHeader()
        Me.NumLote = New System.Windows.Forms.ColumnHeader()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.listItensOPs = New System.Windows.Forms.ListView()
        Me.CodProdMP = New System.Windows.Forms.ColumnHeader()
        Me.DescricaoMP = New System.Windows.Forms.ColumnHeader()
        Me.QtNecessidade = New System.Windows.Forms.ColumnHeader()
        Me.QtEstoque = New System.Windows.Forms.ColumnHeader()
        Me.btnPesquisar = New System.Windows.Forms.Button()
        Me.btnSimularItens = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.txtDataInicial = New System.Windows.Forms.MaskedTextBox()
        Me.txtDataFinal = New System.Windows.Forms.MaskedTextBox()
        Me.SuspendLayout()
        '
        'listCabecalhoOPs
        '
        Me.listCabecalhoOPs.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.NumOP, Me.CodFilial, Me.CodProd, Me.Descricao, Me.TipoMerc, Me.Unidade, Me.Embalagem, Me.Metodo, Me.QtProduzir, Me.QtProduzida, Me.DtLanc, Me.Posicao, Me.QtHoras, Me.FinalizaProducao, Me.NumLote})
        Me.listCabecalhoOPs.GridLines = True
        Me.listCabecalhoOPs.HideSelection = False
        Me.listCabecalhoOPs.Location = New System.Drawing.Point(29, 139)
        Me.listCabecalhoOPs.Name = "listCabecalhoOPs"
        Me.listCabecalhoOPs.Size = New System.Drawing.Size(1023, 209)
        Me.listCabecalhoOPs.TabIndex = 0
        Me.listCabecalhoOPs.UseCompatibleStateImageBehavior = False
        Me.listCabecalhoOPs.View = System.Windows.Forms.View.Details
        '
        'NumOP
        '
        Me.NumOP.Text = "NumOP"
        Me.NumOP.Width = 90
        '
        'CodFilial
        '
        Me.CodFilial.Text = "Filial"
        '
        'CodProd
        '
        Me.CodProd.Text = "Codigo"
        Me.CodProd.Width = 90
        '
        'Descricao
        '
        Me.Descricao.Text = "Descricao"
        Me.Descricao.Width = 300
        '
        'TipoMerc
        '
        Me.TipoMerc.Text = "TipoMerc"
        Me.TipoMerc.Width = 90
        '
        'Unidade
        '
        Me.Unidade.Text = "Unidade"
        Me.Unidade.Width = 90
        '
        'Embalagem
        '
        Me.Embalagem.Text = "Embalagem"
        Me.Embalagem.Width = 100
        '
        'Metodo
        '
        Me.Metodo.Text = "Metodo"
        '
        'QtProduzir
        '
        Me.QtProduzir.Text = "QtProduzir"
        Me.QtProduzir.Width = 90
        '
        'QtProduzida
        '
        Me.QtProduzida.Text = "QtProduzida"
        Me.QtProduzida.Width = 90
        '
        'DtLanc
        '
        Me.DtLanc.Text = "DtLanc"
        Me.DtLanc.Width = 100
        '
        'Posicao
        '
        Me.Posicao.Text = "Posicao"
        '
        'QtHoras
        '
        Me.QtHoras.Text = "QtHoras"
        '
        'FinalizaProducao
        '
        Me.FinalizaProducao.Text = "FinalizaProducao"
        '
        'NumLote
        '
        Me.NumLote.Text = "NumLote"
        Me.NumLote.Width = 90
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(29, 53)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(87, 20)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Data Inicial"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(168, 53)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(79, 20)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Data Final"
        '
        'listItensOPs
        '
        Me.listItensOPs.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.CodProdMP, Me.DescricaoMP, Me.QtNecessidade, Me.QtEstoque})
        Me.listItensOPs.GridLines = True
        Me.listItensOPs.HideSelection = False
        Me.listItensOPs.Location = New System.Drawing.Point(29, 368)
        Me.listItensOPs.Name = "listItensOPs"
        Me.listItensOPs.Size = New System.Drawing.Size(695, 209)
        Me.listItensOPs.TabIndex = 5
        Me.listItensOPs.UseCompatibleStateImageBehavior = False
        Me.listItensOPs.View = System.Windows.Forms.View.Details
        '
        'CodProdMP
        '
        Me.CodProdMP.Text = "Codigo"
        Me.CodProdMP.Width = 120
        '
        'DescricaoMP
        '
        Me.DescricaoMP.Text = "Descricao"
        Me.DescricaoMP.Width = 300
        '
        'QtNecessidade
        '
        Me.QtNecessidade.Text = "Qt Necessaria"
        Me.QtNecessidade.Width = 150
        '
        'QtEstoque
        '
        Me.QtEstoque.Text = "Estoque Fisico"
        Me.QtEstoque.Width = 120
        '
        'btnPesquisar
        '
        Me.btnPesquisar.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.btnPesquisar.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnPesquisar.FlatAppearance.CheckedBackColor = System.Drawing.Color.DarkGray
        Me.btnPesquisar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkGray
        Me.btnPesquisar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray
        Me.btnPesquisar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPesquisar.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.btnPesquisar.ForeColor = System.Drawing.Color.Black
        Me.btnPesquisar.Location = New System.Drawing.Point(299, 79)
        Me.btnPesquisar.Name = "btnPesquisar"
        Me.btnPesquisar.Size = New System.Drawing.Size(116, 32)
        Me.btnPesquisar.TabIndex = 6
        Me.btnPesquisar.Text = "Pesquisar"
        Me.btnPesquisar.UseVisualStyleBackColor = False
        '
        'btnSimularItens
        '
        Me.btnSimularItens.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.btnSimularItens.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnSimularItens.FlatAppearance.CheckedBackColor = System.Drawing.Color.DarkGray
        Me.btnSimularItens.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkGray
        Me.btnSimularItens.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray
        Me.btnSimularItens.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSimularItens.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.btnSimularItens.ForeColor = System.Drawing.Color.Black
        Me.btnSimularItens.Location = New System.Drawing.Point(421, 79)
        Me.btnSimularItens.Name = "btnSimularItens"
        Me.btnSimularItens.Size = New System.Drawing.Size(116, 32)
        Me.btnSimularItens.TabIndex = 7
        Me.btnSimularItens.Text = "Simular Itens"
        Me.btnSimularItens.UseVisualStyleBackColor = False
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.Button1.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.Button1.FlatAppearance.CheckedBackColor = System.Drawing.Color.DarkGray
        Me.Button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkGray
        Me.Button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button1.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.Button1.ForeColor = System.Drawing.Color.Black
        Me.Button1.Location = New System.Drawing.Point(543, 79)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(178, 32)
        Me.Button1.TabIndex = 9
        Me.Button1.Text = "Imprimir Requisição"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'txtDataInicial
        '
        Me.txtDataInicial.Location = New System.Drawing.Point(29, 80)
        Me.txtDataInicial.Mask = "00/00/0000"
        Me.txtDataInicial.Name = "txtDataInicial"
        Me.txtDataInicial.Size = New System.Drawing.Size(125, 27)
        Me.txtDataInicial.TabIndex = 10
        '
        'txtDataFinal
        '
        Me.txtDataFinal.Location = New System.Drawing.Point(168, 81)
        Me.txtDataFinal.Mask = "00/00/0000"
        Me.txtDataFinal.Name = "txtDataFinal"
        Me.txtDataFinal.Size = New System.Drawing.Size(125, 27)
        Me.txtDataFinal.TabIndex = 11
        '
        'frmSepararProducao
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.SlateGray
        Me.ClientSize = New System.Drawing.Size(1064, 588)
        Me.Controls.Add(Me.txtDataFinal)
        Me.Controls.Add(Me.txtDataInicial)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.btnSimularItens)
        Me.Controls.Add(Me.btnPesquisar)
        Me.Controls.Add(Me.listItensOPs)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.listCabecalhoOPs)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "frmSepararProducao"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Separar Itens para Produção"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents listCabecalhoOPs As ListView
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents listItensOPs As ListView
    Friend WithEvents btnPesquisar As Button
    Friend WithEvents btnSimularItens As Button
    Friend WithEvents NumOP As ColumnHeader
    Friend WithEvents CodFilial As ColumnHeader
    Friend WithEvents CodProd As ColumnHeader
    Friend WithEvents Descricao As ColumnHeader
    Friend WithEvents TipoMerc As ColumnHeader
    Friend WithEvents Unidade As ColumnHeader
    Friend WithEvents Embalagem As ColumnHeader
    Friend WithEvents Metodo As ColumnHeader
    Friend WithEvents QtProduzir As ColumnHeader
    Friend WithEvents QtProduzida As ColumnHeader
    Friend WithEvents DtLanc As ColumnHeader
    Friend WithEvents Posicao As ColumnHeader
    Friend WithEvents QtHoras As ColumnHeader
    Friend WithEvents FinalizaProducao As ColumnHeader
    Friend WithEvents NumLote As ColumnHeader
    Friend WithEvents CodProdMP As ColumnHeader
    Friend WithEvents DescricaoMP As ColumnHeader
    Friend WithEvents QtNecessidade As ColumnHeader
    Friend WithEvents Button1 As Button
    Friend WithEvents QtEstoque As ColumnHeader
    Friend WithEvents txtDataInicial As MaskedTextBox
    Friend WithEvents txtDataFinal As MaskedTextBox
End Class
