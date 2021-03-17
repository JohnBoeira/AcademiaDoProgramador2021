Imports MySql.Data.MySqlClient

Public Class FrmEquipamento
    Private Sub FrmEquipamento_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        btnSalvar.BackgroundImage = My.Resources.DisketInativo
        btnEditar.BackgroundImage = My.Resources.cadernoInativo
        btnRemover.BackgroundImage = My.Resources.binInativo
        Listar()
        'Fecha o menu
        Form1.Hide()

    End Sub

    Sub Listar()
        Try
            Abrir()

            'listagem do banco para o DataGrid
            Dim dt As New DataTable
            Dim da As MySqlDataAdapter
            'comando sql de busca
            Dim sql As String = "SELECT * FROM tb_equipamentos order by nome asc"
            da = New MySqlDataAdapter(sql, con)
            'VERIFICA SE TEM ALGO NO BANCO PARA ENTÃO FORMATAR O GRID
            da.Fill(dt)
            grid.DataSource = dt
            If grid.Rows.Count > 0 Then
                Formatar()
            End If

        Catch ex As Exception
            MsgBox("Erro no listar---" + ex.Message)
        End Try

    End Sub

    Sub Formatar()
        'Formata o datagrid
        'ocultando id, preco e data
        grid.Columns(5).Visible = False
        grid.Columns(1).Visible = False
        grid.Columns(2).Visible = False

        grid.Columns(0).HeaderText = "Nome"
        grid.Columns(4).HeaderText = "Número de série"
        grid.Columns(3).HeaderText = "Fabricante"
        'Ajustando largura 
        grid.Columns(0).Width = 190
        grid.Columns(4).Width = 176
        grid.Columns(3).Width = 100
    End Sub

    Private Sub HabilitarCampos()
        'habilita todos os campos
        txtNome.Enabled = True
        txtNumero.Enabled = True
        txtFabricante.Enabled = True
        txtPreco.Enabled = True
    End Sub

    Private Sub DesabilitarCampos()
        'desabilita todos os campos
        txtNome.Enabled = False
        txtNumero.Enabled = False
        txtFabricante.Enabled = False
        txtPreco.Enabled = False
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

        If txtNome.Text <> "" And txtFabricante.Text <> "" And txtPreco.Text <> "" And txtNumero.Text <> "" Then
            Try

                'Abrindo conexão
                Abrir()

                'formatação de data
                Dim data As String
                data = dtFabricante.Value.ToString("yyyy-MM-dd")

                'INSERT         
                Dim cmd As MySqlCommand
                Dim sql As String
                'comando sqp para insert
                sql = "INSERT INTO tb_equipamentos (nome, data, preco, fabricante, numero) VALUES ('" & txtNome.Text & "', '" & data & "','" & txtPreco.Text.Replace(",", ".") & "', '" & txtFabricante.Text & "','" & txtNumero.Text & "')"
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
            txtNome.Focus()
        End If

    End Sub

    Private Sub LimparCampos()
        txtNome.Text = ""
        txtFabricante.Text = ""
        txtNumero.Text = ""
        txtPreco.Text = ""
        dtFabricante.Value = Now

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
        txtCodigo.Text = grid.CurrentRow.Cells(5).Value
        txtCodigo.Visible = False

        txtFabricante.Text = grid.CurrentRow.Cells(3).Value
        txtNome.Text = grid.CurrentRow.Cells(0).Value
        txtPreco.Text = grid.CurrentRow.Cells(2).Value
        txtNumero.Text = grid.CurrentRow.Cells(4).Value
        dtFabricante.Value = grid.CurrentRow.Cells(1).Value
    End Sub

    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        If txtNome.Text <> "" Then
            Try

                'Abrindo conexão
                Abrir()

                'comando         
                Dim cmd As MySqlCommand
                Dim sql As String

                'formatação de data
                Dim data As String
                data = dtFabricante.Value.ToString("yyyy-MM-dd")

                sql = "UPDATE tb_equipamentos SET nome='" & txtNome.Text & "', data='" & data & "', preco='" & txtPreco.Text.Replace(",", ".") & "', fabricante='" & txtFabricante.Text & "', numero='" & txtNumero.Text & "' WHERE id = '" & txtCodigo.Text & "' "

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
            txtNome.Focus()
        End If
    End Sub

    Private Sub btnRemover_Click(sender As Object, e As EventArgs) Handles btnRemover.Click
        If MsgBox("Deseja excluir??", vbYesNo, "Escolha a opção") = vbYes Then
            Try
                Abrir()
                'Deletando 

                Dim cmd As MySqlCommand
                Dim sql As String

                sql = "DELETE FROM tb_equipamentos WHERE id='" & txtCodigo.Text & "'"
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

    Private Sub Button1_Click(sender As Object, e As EventArgs)


    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        'abre form do menu e fecha esse
        Dim form = New Form1
        Form1.Show()
        Me.Close()
    End Sub
End Class