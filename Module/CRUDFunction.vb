﻿'Imports MySql.Data.MySqlClient
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine.ReportDocument
Module CRUDFunction
    Public result As String
    Public cmd As New SqlCommand
    Public da As New SqlDataAdapter
    Public dt As New DataTable
    Public ds As New DataSet

#Region "Report"
    Public Sub reports(ByVal sql As String, ByVal rptname As String, ByVal crystalRpt As Object)
        '
        Try
            If Poscon.State = ConnectionState.Closed Then
                Poscon.Open()
            End If

            Dim reportname As String
            With cmd
                .Connection = Poscon
                .CommandText = sql
            End With
            ds = New DataSet
            da = New SqlDataAdapter(sql, Poscon)
            da.Fill(ds)
            reportname = rptname
            Dim reportdoc = "" 'As New CrystalDecisions.CrystalReports.Engine.ReportDocument
            Dim strReportPath As String
            strReportPath = Application.StartupPath & "\SalesMenu\" & reportname & ".rpt"
            '    "K:\Daakye\FoodApplication\SalesMenu\rptProformaA4.rpt"

            With reportdoc
                '.Load(strReportPath)
                '.SetDataSource(ds.Tables(0))

            End With
            With crystalRpt
                .ShowRefreshButton = False
                .ShowCloseButton = False
                .ShowGroupTreeButton = False
                .ReportSource = reportdoc
            End With
            frmSupplierReport.Show()
        Catch ex As Exception
            MsgBox(ex.Message & "No Crystal Reports have been Installed")
        Finally
            If Poscon.State = ConnectionState.Open Then
                Poscon.Close()
                da.Dispose()
            End If
        End Try
        'poscon.Close()
        'da.Dispose()
    End Sub

    'THIS METHOD IS FOR INSERTING DATA IN THE DATABASE
    Public Sub create(ByVal sql As String)
        Try
            If Poscon.State = ConnectionState.Closed Then
                Poscon.Open()
            End If
            'HOLDS THE DATA TO BE EXECUTED
            With cmd
                .Connection = Poscon
                .CommandText = sql
                'EXECUTE THE DATA
                result = cmd.ExecuteNonQuery
                'CHECKING IF THE DATA HAS EXECUTED OR NOT AND THEN THE POP UP MESSAGE WILL APPEAR
                If result = 0 Then
                    MessageBox.Show("Data is failed to insert.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Else
                    'MessageBox.Show("Data has been inserted in the database.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End With
            Poscon.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            Poscon.Close()
        End Try
    End Sub

    Public Sub createLogged(ByVal sql As String)
        Try
            If Poscon.State = ConnectionState.Closed Then
                Poscon.Open()
            End If
            'HOLDS THE DATA TO BE EXECUTED
            With cmd
                .Connection = Poscon
                .CommandText = sql
                'EXECUTE THE DATA
                result = cmd.ExecuteNonQuery
                'CHECKING IF THE DATA HAS EXECUTED OR NOT AND THEN THE POP UP MESSAGE WILL APPEAR
                If result = 0 Then
                    'MessageBox.Show("Data is failed to insert.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Else
                    'MessageBox.Show("Data has been inserted in the database.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End With
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            Poscon.Close()
        End Try

    End Sub

    '    'THIS METHOD IS FOR RETRIEVING DATA IN THE DATABASE
    Public Sub reload(ByVal sql As String, ByVal DTG As Object)
        Try
            If Poscon.State = ConnectionState.Closed Then
                Poscon.Open()
            End If
            dt = New DataTable
            With cmd
                .Connection = Poscon
                .CommandText = sql
            End With
            da.SelectCommand = cmd
            da.Fill(dt)
            DTG.DataSource = dt
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            Poscon.Close()
            da.Dispose()
        End Try
    End Sub
#End Region
    Public Sub reloadtxt(ByVal sql As String)
        Try
            If Poscon.State = ConnectionState.Closed Then
                Poscon.Open()
            End If
            With cmd
                .Connection = Poscon
                .CommandText = sql
            End With
            dt = New DataTable
            da = New SqlDataAdapter(sql, Poscon)
            da.Fill(dt)

        Catch ex As Exception
            MsgBox(ex.Message & "reloadtxt")
        Finally
            If Poscon.State = ConnectionState.Open Then
                Poscon.Close()
                da.Dispose()
            End If
        End Try
    End Sub

    'THIS METHOD IS FOR UPDATING THE DATA IN THE DATABASE.
    Public Sub updates(ByVal sql As String)
        Try
            If Poscon.State = ConnectionState.Closed Then
                Poscon.Open()
            End If
            With cmd
                .Connection = Poscon
                .CommandText = sql
                result = cmd.ExecuteNonQuery
                If result = 0 Then
                    MessageBox.Show("Data is failed to updated", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Else
                    'MessageBox.Show("Data has been updated in the database.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End With
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            Poscon.Close()
        End Try
    End Sub

    'Mother UPDATE phone
    Public Sub StudentUpdatePhone(ByVal sql As String)
        Try
            If Poscon.State = ConnectionState.Closed Then
                Poscon.Open()
            End If
            With cmd
                .Connection = Poscon
                .CommandText = sql
                result = cmd.ExecuteNonQuery
                If result = 0 Then
                    MessageBox.Show("Student information is failed to Saved", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Else
                    MessageBox.Show("Student has been added to parent list.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End With
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            Poscon.Close()
        End Try
    End Sub

    Public Sub UpdatePhone(ByVal sql As String)
        Try
            If Poscon.State = ConnectionState.Closed Then
                Poscon.Open()
            End If
            With cmd
                .Connection = Poscon
                .CommandText = sql
                result = cmd.ExecuteNonQuery
            End With
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            Poscon.Close()
        End Try
    End Sub
    'INSERT SMS 
    Public Sub CreateSMS(ByVal sql As String)
        Try
            If Poscon.State = ConnectionState.Closed Then
                Poscon.Open()
            End If
            With cmd
                .Connection = Poscon
                .CommandText = sql
                result = cmd.ExecuteNonQuery
            End With
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            Poscon.Close()
        End Try
    End Sub

    Public Sub updatesLogged(ByVal sql As String)
        Try
            If Poscon.State = ConnectionState.Closed Then
                Poscon.Open()
            End If
            With cmd
                .Connection = Poscon
                .CommandText = sql
                result = cmd.ExecuteNonQuery
                If result = 0 Then
                    ' MessageBox.Show("Data is failed to updated", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Else
                    ' MessageBox.Show("Data has been updated in the database.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End With
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            Poscon.Close()

        End Try

    End Sub
    'THIS METHOD IS FOR DELETING THE DATA IN THE DATABASE
    Public Sub delete(ByVal sql As String)
        Try
            If Poscon.State = ConnectionState.Closed Then
                Poscon.Open()
            End If
            With cmd
                .Connection = Poscon
                .CommandText = sql
                result = cmd.ExecuteNonQuery
                If result = 0 Then
                    MessageBox.Show("Data is failed to delete", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Else
                    MessageBox.Show("Data has been deleted in the database.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End With
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            Poscon.Close()
        End Try
    End Sub

    Public Sub findRecords(sql As String, dtg As DataGridView)
        Try
            If Poscon.State = ConnectionState.Closed Then
                Poscon.Open()
            End If
            cmd = New SqlCommand
            With cmd
                .Connection = Poscon
                .CommandText = sql
            End With
            da = New SqlDataAdapter
            da.SelectCommand = cmd
            dt = New DataTable
            da.Fill(dt)
            dtg.DataSource = dt
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            Poscon.Close()
            da.Dispose()
        End Try
    End Sub
End Module
