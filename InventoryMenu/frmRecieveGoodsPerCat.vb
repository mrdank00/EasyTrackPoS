﻿Imports System.Data.SqlClient
Public Class frmRecieveGoodsPerCat
    Dim con As New SqlConnection(My.Settings.PoSConnectionString)
    Dim cmd As New SqlCommand
    Dim adp As New SqlDataAdapter
    Dim dt As New dsGoodsRecieved
    Private Sub CrystalReportViewer1_Load(sender As Object, e As EventArgs) Handles CrystalReportViewer1.Load
        Try
            Dim query = "select * from recieveStock"
            If con.State = ConnectionState.Closed Then
                con.Open()
            End If

            cmd = New SqlCommand(query, con)
            adp.SelectCommand = cmd
            adp.Fill(dt, "recievestock")
            Dim report As New rptRecievedGoodsPerCat
            report.SetDataSource(dt)
            CrystalReportViewer1.ReportSource = report
            CrystalReportViewer1.Refresh()
            cmd.Dispose()
            adp.Dispose()
            con.Close()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
End Class