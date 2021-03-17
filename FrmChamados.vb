Imports MySql.Data.MySqlClient

Public Class FrmChamados
    Dim dataCalculo As Date

    Private Sub FrmChamados_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Configura os btns de forma inativa, exceto o btn novo
        btnSalvar.BackgroundImage = My.Resources.DisketInativo
        btnEditar.BackgroundImage = My.Resources.cadernoInativo
        btnRemover.BackgroundImage = My.Resources.binInativo
        Listar()
        'Fecha o menu
        Form1.Hide()
        CarregarCombobox()


    End Sub

    Sub Listar()
        Try
            Abrir()

            'listagem do banco para o DataGrid
            Dim dt As New DataTable
            Dim da As MySqlDataAdapter
            'comando sql de busca
            Dim sql As String = "SELECT * FROM tb_chamados order by titulo asc"
            da = New MySqlDataAdapter(sql, con)

            da.Fill(dt)
            grid.DataSource = dt
            'VERIFICA SE TEM ALGO NO BANCO PARA ENTÃO FORMATAR O GRID
            If grid.Rows.Count > 0 Then
                Formatar()
            End If

        Catch ex As Exception
            MsgBox("Erro no listar---" + ex.Message)
        End Try
        CarregarCombobox()

    End Sub
    Sub CarregarCombobox()
        Try
            Abrir()
            'select do banco de equip.
            Dim sql As String = "SELECT * FROM tb_equipamentos order by nome asc"
            Dim dt As New DataTable
            Dim da As New MySqlDataAdapter(sql, con)

            da.Fill(dt)
            'verifica se há algo no db de equip. 
            If dt.Rows.Count > 0 Then
                cbEquipamento.DataSource = dt
                cbEquipamento.ValueMember = "id"
                cbEquipamento.DisplayMember = "nome"
            Else
                cbEquipamento.Text = "Cadastre um primeiro"
                btnNovo.Enabled = False
                btnNovo.BackgroundImage = My.Resources.Inativo
            End If

        Catch ex As Exception
            MsgBox("Erro no Carregar do combobox" + ex.Message)
        End Try

    End Sub
    Sub Formatar()
        'Formata o datagrid
        'ocultando 
        grid.Columns(0).Visible = False
        grid.Columns(4).Visible = False
        grid.Columns(5).Visible = False

        grid.Columns(1).HeaderText = "Título"
        grid.Columns(2).HeaderText = "Equipamento"
        grid.Columns(3).HeaderText = "Data de abertura"
        'Ajustando largura 
        grid.Columns(1).Width = 266
        grid.Columns(2).Width = 100
        grid.Columns(3).Width = 100
    End Sub

    Sub CalculoDias()
        'dataCaluclo, var global recuperada na função cellclick do grid
        Dim dt1 As Date = dataCalculo
        'dia atual formatado para short também 
        Dim dt2 As Date = Now.ToShortDateString
        'método de calculo
        Dim diferencaDias = DateDiff(DateInterval.Day, dt1.Date, dt2.Date)
        'passando valor para label
        lblDias.Text = diferencaDias

    End Sub

    Private Sub HabilitarCampos()
        'habilita todos os campos
        txtDescricao.Enabled = True
        txtTitulo.Enabled = True
        txtDescricao.Enabled = True
        cbEquipamento.Enabled = True
        dtAbertura.Enabled = True
    End Sub

    Private Sub DesabilitarCampos()
        'desabilita todos os campos
        txtDescricao.Enabled = False
        txtTitulo.Enabled = False
        cbEquipamento.Enabled = False
        dtAbertura.Enabled = False
    End Sub

    Private Sub btnNovo_Click(sender As Object, e As EventArgs) Handles btnNovo.Click
        DesabilitarBtns()

        btnSalvar.BackgroundImage = My.Resources.disket
        btnSalvar.Enabled = True
        HabilitarCampos()
        LimparCampos()

    End Sub

    Private Sub DesabilitarBtns()

        btnSalvar.BackgroundImage = My.Resources.DisketInativo
        btnEditar.BackgroundImage = My.Resources.cadernoInativo
        btnRemover.BackgroundImage = My.Resources.binInativo
        btnSalvar.Enabled = False
        btnEditar.Enabled = False
        btnRemover.Enabled = False
    End Sub

    Private Sub btnSalvar_Click(sender As Object, e As EventArgs) Handles btnSalvar.Click

        If txtTitulo.Text <> "" And txtDescricao.Text <> "" Then
            Try

                'Abrindo conexão
                Abrir()

                'formatação de data
                Dim data As String
                data = dtAbertura.Value.ToString("yyyy-MM-dd")

                'INSERT         
                Dim cmd As MySqlCommand
                Dim sql As String
                'comando sqp para insert
                sql = "INSERT INTO tb_chamados (titulo, equipamento, data, descricao, equipamento_id) VALUES ('" & txtTitulo.Text & "', '" & cbEquipamento.Text & "','" & data & "', '" & txtDescricao.Text & "', '" & cbEquipamento.SelectedValue & "')"
                'executando comando
                cmd = New MySqlCommand(sql, con)
                cmd.ExecuteNonQuery()
                'Listagem da dataGrid
                Listar()

                MsgBox("Salvo com sucesso!!", MsgBoxStyle.Information, "Dados Salvos")
                DesabilitarCampos()
                DesabilitarBtns()
                LimparCampos()
            Catch ex As Exception
                MsgBox("Erro ao salvar" + ex.Message)
            End Try
        Else
            MsgBox("Preencha os campos!!", MsgBoxStyle.Information, "Erro")
            txtTitulo.Focus()
        End If

    End Sub

    Private Sub LimparCampos()
        txtDescricao.Text = ""
        txtTitulo.Text = ""
        lblDias.Text = ""
        dtAbertura.Value = Now

    End Sub

    Private Sub grid_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles grid.CellClick
        'desabilita o btn de salvar(insert) 
        btnSalvar.BackgroundImage = My.Resources.DisketInativo
        btnSalvar.Enabled = False
        HabilitarCampos()

        'Habilitando btns para edição(update) e exclusão(delete)
        btnEditar.Enabled = True
        btnEditar.BackgroundImage = My.Resources.caderno
        btnRemover.Enabled = True
        btnRemover.BackgroundImage = My.Resources.bin

        'puxando dados do datagrid para os campos
        txtCodigo.Text = grid.CurrentRow.Cells(0).Value
        txtCodigo.Visible = False

        txtTitulo.Text = grid.CurrentRow.Cells(1).Value
        txtDescricao.Text = grid.CurrentRow.Cells(4).Value
        cbEquipamento.SelectedValue = grid.CurrentRow.Cells(5).Value
        dtAbertura.Value = grid.CurrentRow.Cells(3).Value
        'recuperando valor da data do banco para variavel global
        dataCalculo = dtAbertura.Value
        CalculoDias()

    End Sub

    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        If txtTitulo.Text <> "" And txtDescricao.Text <> "" Then
            Try

                'Abrindo conexão
                Abrir()

                'comando         
                Dim cmd As MySqlCommand
                Dim sql As String

                'formatação de data
                Dim data As String
                data = dtAbertura.Value.ToString("yyyy-MM-dd")
                'atualizando dados na tabela
                sql = "UPDATE tb_chamados SET titulo='" & txtTitulo.Text & "', equipamento='" & cbEquipamento.Text & "', data='" & data & "', descricao='" & txtDescricao.Text & "', equipamento_id='" & cbEquipamento.SelectedValue & "' WHERE id = '" & txtCodigo.Text & "' "

                cmd = New MySqlCommand(sql, con)
                cmd.ExecuteNonQuery()

                MsgBox("Editado com sucesso!!", MsgBoxStyle.Information, "Dados editados")
                DesabilitarCampos()

                'Listagem da dataGrid
                Listar()

                LimparCampos()

            Catch ex As Exception
                MsgBox("Error editar" + ex.Message)
            End Try
        Else
            MsgBox("Preencha os campos", MsgBoxStyle.Information, "Erro")
            txtTitulo.Focus()
        End If
    End Sub

    Private Sub btnRemover_Click(sender As Object, e As EventArgs) Handles btnRemover.Click
        If MsgBox("Deseja excluir??", vbYesNo, "Escolha a opção") = vbYes Then
            Try
                Abrir()
                'Deletando 

                Dim cmd As MySqlCommand
                Dim sql As String
                'deletando dados da tabela 
                sql = "DELETE FROM tb_chamados WHERE id='" & txtCodigo.Text & "'"
                cmd = New MySqlCommand(sql, con)
                cmd.ExecuteNonQuery()

                MsgBox("Deletado")

                Listar()
                'Limpando
                DesabilitarBtns()
                DesabilitarCampos()
                LimparCampos()

            Catch ex As Exception
                MsgBox("Erro ao deletar")
            End Try

        End If

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'btn de sair abre frm e fecha esse
        Dim form = New Form1
        form.Show()
        Me.Close()

    End Sub


End Class