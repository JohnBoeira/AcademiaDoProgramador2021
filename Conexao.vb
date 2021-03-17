Imports MySql.Data.MySqlClient
Module Conexao
    ' Public con As New MySqlConnection("server=localhost; userid=root; password=; database=estoque; port=;")
    Public con As New MySqlConnection("server=mysql743.umbler.com; userid=sistemaestoque; password=nddacademiaestoque; database=estoque; port=41890;")
    Sub Abrir()
        If con.State = 0 Then
            con.Open()
        End If
    End Sub
    Sub Fechar()
        If con.State = 1 Then
            con.Close()
        End If
    End Sub
End Module
