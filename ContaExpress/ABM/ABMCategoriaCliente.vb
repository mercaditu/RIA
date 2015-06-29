﻿Imports System.Windows
Imports System.Data.SqlClient
Imports Osuna.Utiles.Conectividad
Public Class ABMCategoriaCliente
    Dim Nuevo As Integer
    Private sqlc As New SqlConnection
    Private myTrans As SqlTransaction
    Private ser As New BDConexión.BDConexion
    Private cmd As New SqlCommand
    Private sql As String
    Dim CodGlobal As Integer
    Dim f As New Funciones.Funciones
    Dim UltimoCodigo As Integer
    Dim UltimoNumero As Integer
    Dim CodNumero As Integer
    Dim permiso As Double

    Private Sub ABMCategoriaCliente_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        permiso = PermisoAplicado(UsuCodigo, 175)
        If permiso = 0 Then
            MessageBox.Show("Usted no tiene permiso para abrir esta ventana", "Pos Express")
            Me.Close()
        End If

        Me.CATEGORIACLIENTETableAdapter.Fill(Me.DsCategoriaCliente.CATEGORIACLIENTE)
        Deshabilitar()
        lblCategoria.Text = "Categorias"
    End Sub


    Private Sub dgvCategoriaCliente_SelectionChanged(sender As Object, e As System.EventArgs) Handles dgvCategoriaCliente.SelectionChanged
        If (dgvCategoriaCliente.Rows.Count > 0) Then
            If dgvCategoriaCliente.CurrentRow.Cells("CODCATEGORIACLIENTE").Value Then
                Dim categoria As Integer = dgvCategoriaCliente.CurrentRow.Cells("CODCATEGORIACLIENTE").Value


                txtCategorizacion.Text = dgvCategoriaCliente.CurrentRow.Cells("DESCATEGORIACLIENTE").Value
            End If
        End If
    End Sub

    Private Sub btnGuardar_Click(sender As System.Object, e As System.EventArgs) Handles BtnGuardar.Click
        Try

            '====================Guardar Categoria Cliente================================

            If Nuevo = 1 Then
                UltimoCodigo = f.Maximo("CODCATEGORIACLIENTE", "CATEGORIACLIENTE")
                CodGlobal = UltimoCodigo + 1
                UltimoNumero = f.Maximo("NUMCATEGORIACLIENTE", "CATEGORIACLIENTE")
                CodNumero = UltimoNumero + 1
                '=====================Insertar Categoria Cliente==========================
                ser.Abrir(sqlc)
                myTrans = sqlc.BeginTransaction(IsolationLevel.ReadCommitted, "Insertar")
                cmd.Connection = sqlc
                cmd.Transaction = myTrans
                sql = ""
                sql = "INSERT INTO CATEGORIACLIENTE (CODCATEGORIACLIENTE,CODUSUARIO,CODEMPRESA,NUMCATEGORIACLIENTE,DESCATEGORIACLIENTE,FECGRA) values(" & CodGlobal & "," & UsuCodigo & "," & EmpCodigo & "," & CodNumero & ",'" & txtCategorizacion.Text & "', convert(datetime, '" & Today() & " 0:00:00', 103))"
                cmd.CommandText = sql
                cmd.ExecuteNonQuery()
                myTrans.Commit()
                sqlc.Close()
                '=====================Llevar al focus de la grilla============================
                Dim codCatTemp As String
                codCatTemp = CodGlobal
                Me.CATEGORIACLIENTETableAdapter.Fill(Me.DsCategoriaCliente.CATEGORIACLIENTE)
                For i = 0 To dgvCategoriaCliente.RowCount - 1
                    If dgvCategoriaCliente.Rows(i).Cells("CODCATEGORIACLIENTE").Value = codCatTemp Then
                        CATEGORIACLIENTEBindingSource.Position = i
                    End If
                Next
            ElseIf Nuevo = 2 Then
                '===================Actualizar Categoria Cliente================================
                ser.Abrir(sqlc)
                myTrans = sqlc.BeginTransaction(IsolationLevel.ReadCommitted, "Actualizar")
                cmd.Connection = sqlc
                cmd.Transaction = myTrans
                sql = ""
                sql = "UPDATE CATEGORIACLIENTE SET DESCATEGORIACLIENTE='" & txtCategorizacion.Text & "' WHERE CODCATEGORIACLIENTE=" & dgvCategoriaCliente.CurrentRow.Cells("CODCATEGORIACLIENTE").Value
                cmd.CommandText = sql
                cmd.ExecuteNonQuery()
                myTrans.Commit()
                sqlc.Close()
                '=====================Llevar al focus de la grilla============================
                Dim codCatTemp As String
                codCatTemp = dgvCategoriaCliente.CurrentRow.Cells("CODCATEGORIACLIENTE").Value
                Me.CATEGORIACLIENTETableAdapter.Fill(Me.DsCategoriaCliente.CATEGORIACLIENTE)
                For i = 0 To dgvCategoriaCliente.RowCount - 1
                    If dgvCategoriaCliente.Rows(i).Cells("CODCATEGORIACLIENTE").Value = codCatTemp Then
                        CATEGORIACLIENTEBindingSource.Position = i
                    End If
                Next
            End If
            Deshabilitar()
            lblCategoria.Text = "Categorias"
        Catch ex As Exception
            myTrans.Rollback()
            MessageBox.Show("Ocurrió un Error Durante la Operación" + ex.Message, "POS EXPRESS", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub btnNuevo_Click(sender As System.Object, e As System.EventArgs) Handles btnNuevo.Click
        permiso = PermisoAplicado(UsuCodigo, 176)
        If permiso = 0 Then
            MessageBox.Show("Usted no tiene permiso para Crear en esta ventana", "Pos Express")
            Exit Sub
        End If
        Nuevo = 1
        Habilitar()
        txtCategorizacion.Text = ""
        txtCategorizacion.Focus()
        txtCategorizacion.BackColor = Color.LightSteelBlue
        lblCategoria.Text = "Ingrese la descripcion de la Categoria"
        btnNuevo.Image = My.Resources.NewActive
    End Sub

    Private Sub btnModificar_Click(sender As System.Object, e As System.EventArgs) Handles BtnModificar.Click
        permiso = PermisoAplicado(UsuCodigo, 178)
        If permiso = 0 Then
            MessageBox.Show("Usted no tiene permiso para Modificar datos de este registro", "Pos Express")
            Exit Sub
        End If
        If Trim(txtCategorizacion.Text) = "Categoría Genérica" Then
            MessageBox.Show("La categoría 'Categoría Genérica' no se puede Modificar", "POSEXPRESS", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        Nuevo = 2

        Habilitar()
        txtCategorizacion.Focus()
        txtCategorizacion.BackColor = Color.LightSteelBlue
        BtnModificar.Image = My.Resources.EditActive
    End Sub

    Private Sub btnEliminar_Click(sender As System.Object, e As System.EventArgs) Handles BtnEliminar.Click
        permiso = PermisoAplicado(UsuCodigo, 177)
        If permiso = 0 Then
            MessageBox.Show("Usted no tiene permiso para Eliminar en esta ventana", "Pos Express")
            Exit Sub
        End If
        Try
            If MessageBox.Show("¿Esta seguro que quiere eliminar la Categoria del Cliente?", "PosExpress", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = MsgBoxResult.No Then
                Exit Sub
            End If
            txtCategorizacion.Enabled = False

            Nuevo = 3
            If Trim(dgvCategoriaCliente.CurrentRow.Cells("DESCATEGORIACLIENTE").Value) = "Categoría Genérica" Then
                MessageBox.Show("La categoría 'Categoría Genérica' no se puede eliminar", "POSEXPRESS", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If


            If Nuevo = 3 Then
                '==================Eliminar Categoria Cliente================================
                    ser.Abrir(sqlc)
                    myTrans = sqlc.BeginTransaction(IsolationLevel.ReadCommitted, "Eliminar")
                    cmd.Connection = sqlc
                    cmd.Transaction = myTrans
                    sql = ""
                    sql = "DELETE FROM CATEGORIACLIENTE WHERE CODCATEGORIACLIENTE=" & dgvCategoriaCliente.CurrentRow.Cells("CODCATEGORIACLIENTE").Value
                    cmd.CommandText = sql
                    cmd.ExecuteNonQuery()
                    myTrans.Commit()
                sqlc.Close()
            End If
          
            Dim codCatTemp As String
            codCatTemp = dgvCategoriaCliente.CurrentRow.Cells("CODCATEGORIACLIENTE").Value
            Me.CATEGORIACLIENTETableAdapter.Fill(Me.DsCategoriaCliente.CATEGORIACLIENTE)
            For i = 0 To dgvCategoriaCliente.RowCount - 1
                If dgvCategoriaCliente.Rows(i).Cells("CODCATEGORIACLIENTE").Value = codCatTemp Then
                    CATEGORIACLIENTEBindingSource.Position = i
                End If
            Next
        Catch ex As SqlException
            Try
                myTrans.Rollback("")
            Catch

            End Try

            Dim NroError As Integer = ex.Number
            Dim Mensaje As String = ex.Message
            If NroError = 547 Then
                MessageBox.Show("La categoria que desea eliminar tiene relaciones con otros datos del sistema, y por tanto no se podra eliminar", "POSEXPRESS", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("Error al Eliminar la Categoria del Cliente: " + ex.Message, "POSEXPRESS", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If

        Finally
            sqlc.Close()

        End Try
        Deshabilitar()
        lblCategoria.Text = "Categorias"
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As System.EventArgs) Handles btnCancelar.Click
        Try


            If dgvCategoriaCliente.Rows.Count > 0 Then
                Dim codCatTemp As String
                codCatTemp = dgvCategoriaCliente.CurrentRow.Cells("CODCATEGORIACLIENTE").Value

                '===========Cancelar y no perder el foco en linea y refresar la grilla====================
                Me.CATEGORIACLIENTEBindingSource.CancelEdit()
                Me.CATEGORIACLIENTETableAdapter.Fill(Me.DsCategoriaCliente.CATEGORIACLIENTE)

                For i = 0 To dgvCategoriaCliente.RowCount - 1
                    If dgvCategoriaCliente.Rows(i).Cells("CODCATEGORIACLIENTE").Value = codCatTemp Then
                        CATEGORIACLIENTEBindingSource.Position = i
                    End If
                Next
                txtCategorizacion.Text = dgvCategoriaCliente.CurrentRow.Cells("DESCATEGORIACLIENTE").Value

            End If
            Deshabilitar()
            lblCategoria.Text = "Categorias"
        Catch ex As Exception
            myTrans.Rollback()
            MessageBox.Show("Ocurrió un Error Durante la Operación" + ex.Message, "POS EXPRESS", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub txtbuscacliente_GotFocus(sender As Object, e As System.EventArgs) Handles txtbuscacliente.GotFocus
        txtbuscacliente.BackColor = Color.LightSteelBlue
    End Sub

    Private Sub txtbuscacliente_LostFocus(sender As Object, e As System.EventArgs) Handles txtbuscacliente.LostFocus
        txtbuscacliente.BackColor = Color.DimGray
    End Sub

    
    Private Sub txtbuscacliente_TextChanged(sender As Object, e As System.EventArgs) Handles txtbuscacliente.TextChanged
        Me.CATEGORIACLIENTEBindingSource.Filter = "DESCATEGORIACLIENTE like '%" & txtbuscacliente.Text & "%'"
    End Sub
    Private Sub Habilitar()
        btnNuevo.Image = My.Resources.NewOff
        btnNuevo.Enabled = False
        btnNuevo.Cursor = Cursors.Arrow
        BtnEliminar.Enabled = False
        BtnEliminar.Image = My.Resources.DeleteOff
        BtnEliminar.Cursor = Cursors.Arrow
        BtnModificar.Enabled = False
        BtnModificar.Image = My.Resources.EditOff
        BtnModificar.Cursor = Cursors.Arrow
        txtCategorizacion.Enabled = True
        btnCancelar.Enabled = True
        btnCancelar.Image = My.Resources.SaveCancel
        btnCancelar.Cursor = Cursors.Hand
        BtnGuardar.Enabled = True
        BtnGuardar.Image = My.Resources.Save
        BtnGuardar.Cursor = Cursors.Hand

        dgvCategoriaCliente.Enabled = False
        txtbuscacliente.Enabled = False
    End Sub
    Private Sub Deshabilitar()
        btnNuevo.Image = My.Resources.New_
        btnNuevo.Enabled = True
        btnNuevo.Cursor = Cursors.Hand

        BtnEliminar.Enabled = True
        BtnEliminar.Image = My.Resources.Delete
        BtnEliminar.Cursor = Cursors.Hand
        BtnModificar.Enabled = True
        BtnModificar.Image = My.Resources.Edit
        BtnModificar.Cursor = Cursors.Hand

        btnCancelar.Enabled = True
        btnCancelar.Image = My.Resources.SaveCancelOff
        btnCancelar.Cursor = Cursors.Arrow
        BtnGuardar.Enabled = False
        BtnGuardar.Image = My.Resources.SaveOff
        BtnGuardar.Cursor = Cursors.Arrow
        txtCategorizacion.Enabled = False

        dgvCategoriaCliente.Enabled = True
        txtbuscacliente.Enabled = True
    End Sub
End Class