Public Class Form1
    Private Sub btnEquipa_Click(sender As Object, e As EventArgs) Handles btnEquipa.Click
        Dim form = New FrmEquipamento
        form.Show()
        MsgBox("Para voltar para o MENU CLIQUE na LOGO. Salve as alterações clicando no icone de edição", MsgBoxStyle.Information, "Dicas!!")
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FrmChamados.Close()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim form = New FrmChamados
        form.Show()
        MsgBox("Veja os dias que o chamado está aberto clicando na tabela(grid), para voltar para o MENU CLIQUE na LOGO. O primeiro icone (novo) só é habilitado se houver equipamento cadatrado. Salve as alterações clicando no icone de edição", MsgBoxStyle.Information, "Dicas!!")
    End Sub


End Class