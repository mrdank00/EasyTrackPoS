﻿Imports System.Data.SqlClient
Public Class frmSales
    Dim Con As New SqlConnection(My.Settings.PoSConnectionString)
    Dim dr As SqlDataReader
    Dim cmd As SqlCommand
    Dim builder As SqlCommandBuilder
    Dim da As SqlDataAdapter
    Dim dt As New dsSalesTranx
    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        'Dim f2 As New frmSalesMenu
        'f2.Show()
        Me.Hide()

    End Sub

    Private Sub frmSales_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Display()
        ckCashDisc.Checked = True
        txtCashDisc.Enabled = True
        ckPerDisc.Checked = False
        txtPerDisc.Enabled = False

        If Con.State = ConnectionState.Closed Then
            Con.Open()
        End If
        Dim que = "select * from userlogs"
        cmd = New SqlCommand(que, Con)
        Dim da As New SqlDataAdapter(cmd)
        Dim table As New DataTable
        da.Fill(table)
        If table.Rows.Count = 0 Then
        Else
            Dim index = table.Rows.Count() - 1
            Activeuser.Text = table.Rows(index)(1).ToString
        End If

        Con.Close()


        ckrollPaper.Checked = True
        ckA5Paper.Checked = False
        ckA4.Checked = False
        ShowConfig()
        Me.MaximumSize = Screen.FromRectangle(Me.Bounds).WorkingArea.Size
        WindowState = FormWindowState.Maximized
        Timer1.Enabled = True

        cbCreditCustName.Visible = False
        lblCreditCust.Visible = False
        ShowBand()
        clear()
        cbSaleType.SelectedIndex = 0
        txtProdname.Focus()


    End Sub
    Private Sub clear()
        txtCashPaid.Text = ""
        txtProdcode.Text = ""
        txtPrice.Text = ""
        cbProdName.Text = ""
        txtAmt.Text = ""
        txtQty.Text = ""
        txtCat.Text = ""
        lblCustNo.Text = ""
        lblChange.Text = ""
        lblActualStock.Text = ""
        lblNewBal.Text = ""
        lblOldBal.Text = ""
        cbCreditCustName.Text = ""
        txtCat.Text = ""
        txtColour.Text = ""
        txtProdline.Text = ""
        txtSize.Text = ""
        txtBuyerName.Text = ""
        txtBuyerTel.Text = ""
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Dim sum As Decimal = 0
        Dim payable As Decimal = 0
        Dim DiscAmt As Decimal = 0
        If lblProdName.Text = "" Or txtQty.Text = "" Or txtPrice.Text = "" Then
            MsgBox("Select Item or Input Quantity", vbCritical)
        Else
            If Con.State = ConnectionState.Closed Then
                Con.Open()
            End If

            Dim query = "Select * from StockMast where Prodcode='" + txtProdcode.Text + "'"
            cmd = New SqlCommand(query, Con)
            dr = cmd.ExecuteReader
            While dr.Read
                If dr.Item("ProdQty") Is Nothing Then
                    lblActualStock.Text = 0
                Else
                    lblActualStock.Text = dr.Item("ProdQty")
                End If
                For Each row As DataGridViewRow In gvSales.Rows
                    If txtProdcode.Text = row.Cells(5).Value Then
                        row.Cells(1).Value += Val(txtQty.Text)
                        row.Cells(3).Value = row.Cells(1).Value * row.Cells(2).Value
                        row.Cells(8).Value = Val(lblActualStock.Text) - row.Cells(1).Value
                        row.Cells(12).Value = row.Cells(3).Value
                        'For discount percentage
                        If row.Cells(13).Value = "%" Then
                            Dim Discprice As Decimal
                            Discprice = (Val(txtPerDisc.Text) / 100) * row.Cells(3).Value
                            row.Cells(10).Value = txtPerDisc.Text
                            row.Cells(11).Value = Discprice
                            row.Cells(12).Value = row.Cells(3).Value - Discprice

                        End If
                        'for Money Discount
                        'If row.Cells(13).Value = "$" Then
                        'Dim Discprice As Decimal
                        'Discprice = row.Cells(3).Value - Val(txtCashDisc.Text)
                        'row.Cells(10).Value = txtCashDisc.Text
                        ' row.Cells(12).Value = Discprice
                        'row.Cells(11).Value = row.Cells(3).Value - Discprice

                        'End If

                        If ckPerDisc.Checked = True Then
                            For k = 0 To gvSales.RowCount - 1
                                If row.Cells(10).Value = "" Then
                                    row.Cells(12).Value = row.Cells(3).Value
                                    row.Cells(11).Value = 0
                                End If
                                sum += gvSales.Rows(k).Cells(3).Value
                                payable += gvSales.Rows(k).Cells(12).Value
                                DiscAmt += gvSales.Rows(k).Cells(11).Value
                            Next
                            lblTotal.Text = sum
                            lblPayable.Text = payable
                            lblDiscAmt.Text = DiscAmt
                        End If

                        Con.Close()
                        clear()
                        dr.Close()

                        Try
                            For k = 0 To gvSales.RowCount - 1
                                If gvSales.Rows(k).Cells(10).ToString = "" Then
                                    gvSales.Rows(k).Cells(12).Value = gvSales.Rows(k).Cells(3).Value
                                    gvSales.Rows(k).Cells(11).Value = "0"
                                End If
                                sum += gvSales.Rows(k).Cells(3).Value
                                payable += gvSales.Rows(k).Cells(12).Value
                                DiscAmt += gvSales.Rows(k).Cells(11).Value
                            Next
                            lblTotal.Text = sum
                            lblPayable.Text = payable
                            lblDiscAmt.Text = DiscAmt
                        Catch ex As Exception
                            MsgBox(ex.ToString)
                        End Try
                        Exit Sub

                    End If

                Next

                Dim a = Val(txtQty.Text)
                Dim newstock As New Integer
                Dim c = Val(lblActualStock.Text)
                newstock = CInt(c - a)
                gvSales.Rows.Add(lblProdName.Text, txtQty.Text, txtPrice.Text, txtAmt.Text, txtCat.Text, txtProdcode.Text, txtSize.Text, txtProdline.Text, newstock, lblRecieptNo.Text, "0", "0", txtAmt.Text, "", txtColour.Text, lblActualStock.Text)

            End While
            dr.Close()

        End If

        txtCashPaid.Text = ""
        txtProdcode.Text = ""
        txtPrice.Text = ""
        cbProdName.Text = ""
        txtAmt.Text = ""
        txtQty.Text = ""
        txtCat.Text = ""
        lblCustNo.Text = ""
        lblChange.Text = ""
        lblActualStock.Text = ""
        lblNewBal.Text = ""
        lblOldBal.Text = ""
        cbCreditCustName.Text = ""
        txtCat.Text = ""
        txtColour.Text = ""
        txtProdline.Text = ""
        txtSize.Text = ""
        If cbSaleType.Text = "" Or cbSaleType.Text = "Credit Sale" Then
            cbCreditCustName.Visible = True
            lblCreditCust.Visible = True
        Else
            cbCreditCustName.Visible = False
            lblCreditCust.Visible = False
        End If

    End Sub
    Public Sub Feel(valueTosearch As String)
        Try
            If Con.State = ConnectionState.Closed Then
                Con.Open()
            End If
            If cbCatSort.SelectedIndex = -1 And cbProdlineSort.SelectedIndex = -1 Then
                Dim query = "select ProdName,ProdQty,retailprice,Prodsize,ProdCat,ProdColour,Prodline,ProdCode from StockMast where concat(ProdName,ProdCode) like '%" + valueTosearch + "%'"
                cmd = New SqlCommand(query, Con)
                Dim adapter As New SqlDataAdapter(cmd)
                Dim table As New DataTable()
                adapter.Fill(table)
                gvStock.DataSource = table
                'Dim itemavailable As Boolean
                'itemavailable = table.Rows(0)(0).ToString = ""
                'If itemavailable Then
                'Else
                'If table.Rows(0)(0).ToString = txtProdname.Text And table.Rows.Count = 1 Then
                'cbProdName.Text = table.Rows(0)(7).ToString()
                'Else
                'MsgBox("No Record")
                ''txtProdName.Text = ""
                'End If
                'End If

            End If
            If cbCatSort.SelectedIndex <> -1 And cbProdlineSort.SelectedIndex = -1 Then
                Dim query = "select ProdName,ProdQty,retailprice,Prodsize,ProdCat,ProdColour,Prodline,ProdCode from StockMast where concat(ProdName,ProdCode) like '%" + valueTosearch + "%' and prodcat like '%" + cbCatSort.Text + "%'"
                cmd = New SqlCommand(query, Con)
                Dim adapter As New SqlDataAdapter(cmd)
                Dim table As New DataTable()
                adapter.Fill(table)
                gvStock.DataSource = table
                'Dim itemavailable As Boolean
                'itemavailable = table.Rows(0)(0).ToString = ""
                'If itemavailable Then
                'Else
                'If table.Rows(0)(0).ToString = txtProdname.Text And table.Rows.Count = 1 Then
                'cbProdName.Text = table.Rows(0)(7).ToString()
                'Else
                'MsgBox("No Record")
                ''txtProdName.Text = ""
                'End If
                'End If

            End If
            If cbCatSort.SelectedIndex = -1 And cbProdlineSort.SelectedIndex <> -1 Then
                Dim query = "select ProdName,ProdQty,retailprice,Prodsize,ProdCat,ProdColour,Prodline,ProdCode from StockMast where concat(ProdName,ProdCode) like '%" + valueTosearch + "%' and Prodline like '%" + cbProdlineSort.Text + "%'"
                cmd = New SqlCommand(query, Con)
                Dim adapter As New SqlDataAdapter(cmd)
                Dim table As New DataTable()
                adapter.Fill(table)
                gvStock.DataSource = table
                'Dim itemavailable As Boolean
                'itemavailable = table.Rows(0)(0).ToString = ""
                'If itemavailable Then
                'Else
                'If table.Rows(0)(0).ToString = txtProdname.Text And table.Rows.Count = 1 Then
                'cbProdName.Text = table.Rows(0)(7).ToString()
                'Else
                'MsgBox("No Record")
                ''txtProdName.Text = ""
                'End If
                'End If

            End If

            If cbCatSort.SelectedIndex <> -1 And cbProdlineSort.SelectedIndex <> -1 Then
                Dim query = "select ProdName,ProdQty,retailprice,Prodsize,ProdCat,ProdColour,Prodline,ProdCode from StockMast where concat(ProdName,ProdCode) like '%" + valueTosearch + "%' and prodcat like '%" + cbCatSort.Text + "%' and Prodline like '%" + cbProdlineSort.Text + "%'"
                cmd = New SqlCommand(query, Con)
                Dim adapter As New SqlDataAdapter(cmd)
                Dim table As New DataTable()
                adapter.Fill(table)
                gvStock.DataSource = table
                'Dim itemavailable As Boolean
                'itemavailable = table.Rows(0)(0).ToString = ""
                'If itemavailable Then
                'Else
                'If table.Rows(0)(0).ToString = txtProdname.Text And table.Rows.Count = 1 Then
                'cbProdName.Text = table.Rows(0)(7).ToString()
                'Else
                'MsgBox("No Record")
                ''txtProdName.Text = ""
                'End If
                'End If

            End If




            Con.Close()
        Catch ex As Exception

        End Try
    End Sub
    Public Sub Search(valuetosearch As String)
        Try
            If Con.State = ConnectionState.Closed Then
                Con.Open()
            End If
            Dim query = "select * from customer where concat(customername,Customerno) like '%" + valuetosearch + "%'"
            cmd = New SqlCommand(query, Con)
            Dim adapter As New SqlDataAdapter(cmd)
            Dim tbl As New DataTable()
            adapter.Fill(tbl)
            lblOldBal.Text = tbl.Rows(0)(10).ToString
            lblCustNo.Text = tbl.Rows(0)(0).ToString
            Dim newbal = Val(lblOldBal.Text) + Val(lblTotal.Text)
            lblNewBal.Text = newbal
            Con.Close()
        Catch ex As Exception
            'MsgBox(ex.ToString)
        End Try


    End Sub
    Private Sub Display()
        If Con.State = ConnectionState.Closed Then
            Con.Open()
        End If
        Dim query = "select ProdName,ProdQty,retailprice,Prodsize,ProdCat,ProdColour,Prodline,ProdCode from StockMast"
        ''ProdName,ProdQty,retailprice,Prodsize,ItemNo,ProdCat,ProdColour,Prodline,ProdCode
        cmd = New SqlCommand(query, Con)
        da = New SqlDataAdapter(cmd)
        Dim tbl As New DataTable
        da.Fill(tbl)
        gvStock.DataSource = tbl
        Con.Close()



    End Sub

    Private Sub cbProdName_TextUpdate(sender As Object, e As EventArgs) Handles cbProdName.TextUpdate
        'Feel(txtProdname.Text)

    End Sub



    Private Sub gvStock_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        Try
            Dim row As DataGridViewRow = gvStock.Rows(e.RowIndex)
            cbProdName.Text = row.Cells(1).Value.ToString()
            lblProdName.Text = row.Cells(1).Value.ToString()
            txtPrice.Text = row.Cells(7).Value.ToString()
            txtProdcode.Text = row.Cells(0).Value.ToString()
            lblActualStock.Text = row.Cells(6).Value.ToString()
            txtProdline.Text = row.Cells(2).Value.ToString()
            txtCat.Text = row.Cells(5).Value.ToString()
            txtSize.Text = row.Cells(3).Value.ToString()
            txtColour.Text = row.Cells(4).Value.ToString()

        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try

        ''txtProdName.Text = ""


    End Sub
    Private Sub ShowBand()
        If Con.State = ConnectionState.Closed Then
            Con.Open()
        End If

        Dim query = "select * from PriceBand"
        cmd = New SqlCommand(query, Con)
        Dim adapter As New SqlDataAdapter(cmd)
        Dim tbl As New DataTable()
        adapter.Fill(tbl)
        gvPriceBand.DataSource = tbl
        Con.Close()
    End Sub
    Private Sub txtQty_TextChanged(sender As Object, e As EventArgs) Handles txtQty.TextChanged
        If txtQty.Text = "" Then
            Exit Sub
        End If
        'Price Bands
        'For Each row As DataGridViewRow In gvPriceBand.Rows
        '    If row.Cells(1).Value = lblProdName.Text And row.Cells(3).Value <= CDbl(txtQty.Text) And row.Cells(4).Value >= CDbl(txtQty.Text) Then
        '        txtPrice.Text = row.Cells(5).Value
        '    Else
        '        'txtPrice.Text = lblOPrice.Text
        '    End If
        '    'MsgBox(row.Cells(5).Value)
        'Next

        Dim amt As Decimal
        amt = Val(txtQty.Text) * Val(txtPrice.Text)
        txtAmt.Text = amt
    End Sub

    Private Sub gvSales_RowsAdded(sender As Object, e As DataGridViewRowsAddedEventArgs)

        Try
            Dim sum As Decimal = 0
            For k = 0 To gvSales.RowCount - 1
                sum += gvSales.Rows(k).Cells(3).Value

            Next
            lblTotal.Text = sum
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try

    End Sub

    Private Sub txtCashPaid_TextChanged(sender As Object, e As EventArgs) Handles txtCashPaid.TextChanged

        Dim CashPaid As Double = 0
        Dim change As Double = 0
        change = Val(txtCashPaid.Text) - Val(lblPayable.Text)
        lblChange.Text = change



    End Sub

    Private Sub reciept()

        Con.Open()
        Dim sql = "insert into Recieptconfig(Salesperson,recieptid,date) values('" + Activeuser.Text + "','" + lblRecieptNo.Text + "','" + lblDate.Text + "') "
        cmd = New SqlCommand(sql, Con)
        cmd.ExecuteNonQuery()
        Con.Close()

    End Sub
    Private Sub ShowConfig()
        If Con.State = ConnectionState.Closed Then
            Con.Open()
        End If
        Dim recieptcount As String
        Dim nextreciept As String
        Dim que = "select * from recieptconfig"
        cmd = New SqlCommand(que, Con)
        Dim da As New SqlDataAdapter(cmd)
        Dim table As New DataTable
        da.Fill(table)
        If table.Rows.Count() = 0 Then
            lblRecieptNo.Text = "10001"
        Else

            Dim index = table.Rows.Count() - 1
            Dim reciept = table.Rows(index)(0).ToString
            nextreciept = reciept + 1
            recieptcount = nextreciept.Count.ToString
            Select Case recieptcount
                Case "1"
                    lblRecieptNo.Text = "1000" + nextreciept
                Case "2"
                    lblRecieptNo.Text = "100" + nextreciept
                Case "3"
                    lblRecieptNo.Text = "10" + nextreciept
                Case "4"
                    lblRecieptNo.Text = "1" + nextreciept
                Case "5"
                    lblRecieptNo.Text = nextreciept
                Case Else
                    lblRecieptNo.Text = nextreciept

            End Select
        End If


        Con.Close()


    End Sub


    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        lblTime.Text = Date.Now.ToString("hh:mm:ss")
        lblDate.Text = Date.Now.ToString("dd-MMM-yy")
    End Sub

    Private Sub cbSaleType_TextChanged(sender As Object, e As EventArgs) Handles cbSaleType.TextChanged, cbCreditCustName.TextChanged
        Search(cbCreditCustName.Text)
    End Sub

    Private Sub cbSaleType_DropDownClosed(sender As Object, e As EventArgs) Handles cbSaleType.DropDownClosed

        If (cbSaleType.SelectedIndex = 1) Then
            txtBuyerName.Enabled = False
            txtCashPaid.Enabled = False
            txtBuyerTel.Enabled = False
            cbLocation.Enabled = False
            cbPaymode.Enabled = False
            cbCreditCustName.Visible = True
            lblCreditCust.Visible = True
            txtCashPaid.Text = 0

        ElseIf (cbSaleType.SelectedIndex = 0) Then
            cbPaymode.Enabled = True
            txtBuyerName.Enabled = True
            txtCashPaid.Enabled = True
            txtBuyerTel.Enabled = True
            cbLocation.Enabled = True
            cbCreditCustName.Visible = False
            lblCreditCust.Visible = False
        End If
    End Sub

    Private Sub cbProdName_TextChanged(sender As Object, e As EventArgs) Handles cbProdName.TextChanged

    End Sub

    Private Sub cbProdName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbProdName.SelectedIndexChanged


    End Sub

    Private Sub txtProdName_KeyUp(sender As Object, e As KeyEventArgs)
        Feel(txtProdname.Text)

    End Sub

    Private Sub lblTotal_TextChanged(sender As Object, e As EventArgs) Handles lblTotal.TextChanged
        Dim newbal As New Decimal
        newbal = Val(lblOldBal.Text) + Val(lblTotal.Text)
        lblNewBal.Text = newbal
    End Sub

    Private Sub cbCreditCustName_DropDownClosed(sender As Object, e As EventArgs) Handles cbCreditCustName.DropDownClosed
        Search(cbCreditCustName.Text)
    End Sub

    Private Sub cbCreditCustName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbCreditCustName.SelectedIndexChanged
        Search(cbCreditCustName.Text)
    End Sub

    Private Sub txtPrice_TextChanged(sender As Object, e As EventArgs) Handles txtPrice.TextChanged
        Dim amt As Decimal
        amt = Val(txtQty.Text) * Val(txtPrice.Text)
        txtAmt.Text = amt
    End Sub

    Private Sub cbCreditCustName_TextUpdate(sender As Object, e As EventArgs) Handles cbCreditCustName.TextUpdate
        Search(cbCreditCustName.Text)
    End Sub

    Private Sub txtQty_KeyUp(sender As Object, e As KeyEventArgs) Handles txtQty.KeyUp

    End Sub

    Private Sub txtQty_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtQty.KeyPress
        If Not Char.IsNumber(e.KeyChar) And Not e.KeyChar = Chr(Keys.Delete) And Not e.KeyChar = Chr(Keys.Back) And Not e.KeyChar = Chr(Keys.Space) Then
            e.Handled = True
            'MsgBox("This field will accept numbers only")
        End If
    End Sub

    Private Sub txtCashPaid_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCashPaid.KeyPress
        If Not Char.IsNumber(e.KeyChar) And Not e.KeyChar = Chr(Keys.Delete) And Not e.KeyChar = Chr(Keys.Back) And Not e.KeyChar = Chr(Keys.Space) Then
            e.Handled = True
            'MsgBox("This field will accept numbers only")
        End If
    End Sub

    Private Sub txtProdname_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtProdname.KeyPress
        Feel(txtProdname.Text)
    End Sub

    Private Sub txtProdname_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtProdname.Validating
        Itemfill()
    End Sub
    Sub Itemfill()
        If txtProdname.Text = "" Then
            txtPrice.Text = ""
            txtProdcode.Text = ""
            lblActualStock.Text = ""
            txtProdline.Text = ""
            txtCat.Text = ""
            txtSize.Text = ""
            txtColour.Text = ""
            lblProdName.Text = ""
        End If
    End Sub

    Private Sub txtProdname_Validated(sender As Object, e As EventArgs) Handles txtProdname.Validated
        Itemfill()
    End Sub

    Private Sub txtProdname_TextChanged(sender As Object, e As EventArgs) Handles txtProdname.TextChanged
        Itemfill()
    End Sub

    Private Sub gvStock_CellClick_1(sender As Object, e As DataGridViewCellEventArgs) Handles gvStock.CellClick
        Try

            Dim row As DataGridViewRow = gvStock.Rows(e.RowIndex)
            cbProdName.Text = row.Cells(0).Value.ToString()
            'lblProdName.Text = row.Cells(0).Value.ToString()
            txtPrice.Text = row.Cells(2).Value.ToString()
            lblActualStock.Text = row.Cells(1).Value.ToString()
            txtProdcode.Text = row.Cells(7).Value.ToString()
            txtProdline.Text = row.Cells(6).Value.ToString()
            txtCat.Text = row.Cells(4).Value.ToString()
            txtSize.Text = row.Cells(3).Value.ToString()
            txtColour.Text = row.Cells(5).Value.ToString()
            lblOPrice.Text = row.Cells(2).Value.ToString()
            lblProdName.Text = row.Cells(0).Value.ToString() + "-" + txtProdline.Text + ""
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try

        ''txtProdName.Text = ""
    End Sub

    Private Sub PictureBox3_Click(sender As Object, e As EventArgs) Handles PictureBox3.Click
        Me.Hide()
        PfrmSalesMgmt.btnOpenSession.Visible = False
    End Sub

    Private Sub PictureBox4_Click(sender As Object, e As EventArgs) Handles PictureBox4.Click
        If WindowState = FormWindowState.Normal Then
            WindowState = FormWindowState.Maximized
        ElseIf WindowState = FormWindowState.Maximized Then
            WindowState = FormWindowState.Normal
        End If
    End Sub

    Private Sub gvSales_RowsAdded_1(sender As Object, e As DataGridViewRowsAddedEventArgs) Handles gvSales.RowsAdded
        Try
            Dim sum As Decimal = 0
            Dim payable As Decimal = 0
            Dim discamt As Decimal = 0
            For k = 0 To gvSales.RowCount - 1
                If gvSales.Rows(k).Cells(10).ToString = "" Then
                    gvSales.Rows(k).Cells(12).Value = gvSales.Rows(k).Cells(3).Value
                    gvSales.Rows(k).Cells(11).Value = "0"
                End If
                sum += gvSales.Rows(k).Cells(3).Value
                payable += gvSales.Rows(k).Cells(12).Value
                DiscAmt += gvSales.Rows(k).Cells(11).Value
            Next
            lblTotal.Text = sum
            lblPayable.Text = payable
            lblDiscAmt.Text = discamt
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub BunifuThinButton21_Click(sender As Object, e As EventArgs) Handles BunifuThinButton21.Click
        ShowConfig()
        If Val(txtCashPaid.Text) < Val(lblPayable.Text) And cbSaleType.SelectedIndex = 0 Then
            MsgBox("Cash paid not enough", vbCritical)
            Exit Sub
        End If
        If ckrollPaper.Checked = False And ckA5Paper.Checked = False And ckA4.Checked = False Then
            MsgBox("Select Print format")
            Exit Sub
        End If

        If cbSaleType.Text = "" Or gvSales.RowCount = 0 Then
            MsgBox("Select Sale Type or Fill sale Cart", vbCritical)
            Exit Sub
        Else
            Try
                If (cbSaleType.SelectedIndex = 0 Or cbSaleType.SelectedItem = "Walk-In Sale") Then
                    'reciept()

                    ' cbCreditCustName.Visible = False
                    ' lblCreditCust.Visible = False
                    Try

                        If gvSales.Rows.Count > 0 Then
                            For Each row As DataGridViewRow In gvSales.Rows

                                If Con.State = ConnectionState.Closed Then
                                    Con.Open()
                                End If
                                Dim query = "insert into salestranx (ItemCode,ItemName,ProdLine,ProdCat,ItemSize,ItemColour,QtySold,DateSold,TimeSold,BuyerName,BuyerTel,BuyerLocation,NewStock,RetailPrice,SaleType,CredCustName,Amount,Soldby,RecieptNo,AmtPaid,Balance,DiscountRate,DiscountAmount,AmountPayable,TotalDiscount) values(@ItemCode,@ItemName,@ProdLine,@ProdCat,@ItemSize,@ItemColour,@QtySold,@DateSold,@TimeSold,@BuyerName,@BuyerTel,@BuyerLocation,@NewStock,@RetailPrice,@SaleType,@CreditCustomerName,@Amount,'" + Activeuser.Text + "', '" + lblRecieptNo.Text + "','" + txtCashPaid.Text + "','" + lblChange.Text + "',@Discount,@DiscAmt,@Amtpayable,'" + lblDiscAmt.Text + "')"
                                cmd = New SqlCommand(query, Con)
                                With cmd

                                    .Parameters.AddWithValue("@ItemCode", SqlDbType.NVarChar).Value = row.Cells(5).Value
                                    .Parameters.AddWithValue("@Itemname", row.Cells(0).Value)
                                    .Parameters.AddWithValue("@ProdLine", row.Cells(7).Value)
                                    .Parameters.AddWithValue("@ProdCat", row.Cells(4).Value)
                                    .Parameters.AddWithValue("@ItemSize", row.Cells(6).Value)
                                    .Parameters.AddWithValue("@ItemColour", row.Cells(4).Value)
                                    .Parameters.AddWithValue("@QtySold", row.Cells(1).Value)
                                    .Parameters.AddWithValue("@DateSold", lblDate.Text)
                                    .Parameters.AddWithValue("@TimeSold", lblTime.Text)
                                    .Parameters.AddWithValue("@BuyerName", txtBuyerName.Text)
                                    .Parameters.AddWithValue("@BuyerTel", txtBuyerTel.Text)
                                    .Parameters.AddWithValue("@BuyerLocation", cbLocation.Text)
                                    .Parameters.AddWithValue("@NewStock", row.Cells(8).Value)
                                    .Parameters.AddWithValue("@RetailPrice", row.Cells(2).Value)
                                    .Parameters.AddWithValue("@Saletype", cbSaleType.Text)
                                    .Parameters.AddWithValue("@CreditCustomerName", cbCreditCustName.Text)
                                    .Parameters.AddWithValue("@Amount", row.Cells(3).Value)
                                    .Parameters.AddWithValue("@Discount", row.Cells(10).Value)
                                    .Parameters.AddWithValue("@DiscAmt", row.Cells(11).Value)
                                    .Parameters.AddWithValue("@AmtPayable", value:=(lblPayable.Text))
                                    .ExecuteNonQuery()
                                End With



                            Next




                            'MsgBox("Record Saved")
                        End If
                    Finally
                        For k = 0 To gvSales.RowCount - 1
                            If Con.State = ConnectionState.Closed Then
                                Con.Open()
                            End If
                            Dim sqll = "Select * from StockMast where Prodcode='" + gvSales.Rows(k).Cells(5).Value + "'"
                            cmd = New SqlCommand(sqll, Con)
                            dr = cmd.ExecuteReader
                            While dr.Read

                                Dim query = "update StockMast set prodqty = '" & dr.Item("ProdQty") - gvSales.Rows(k).Cells(1).Value & "' where Prodcode= @itemcode"
                                cmd = New SqlCommand(query, Con)
                                With cmd
                                    .Parameters.AddWithValue("@ItemCode", SqlDbType.NVarChar).Value = gvSales.Rows(k).Cells(5).Value
                                    .ExecuteNonQuery()
                                End With
                                cmd.ExecuteNonQuery()
                            End While
                        Next
                        For Each row As DataGridViewRow In gvSales.Rows
                            Dim quer = "insert into InventoryLedger (ItemCode,itemname,tranxtype,TranxSource,TranxGroup,oldqty,qtyIssued,StockBalance,Userid,RetailPrice,CostPrice,RetailAmt,CostAmt,time,date) values(@ItemCode,@Itemname,@Tranxtype,@tranxsource,@TranxGroup,@oldqty,@qtyIssued,@balance,@userid,@Rprice,@cprice,@ramt,@camt,@time,@date)"
                            cmd = New SqlCommand(quer, Con)
                            With cmd
                                .Parameters.AddWithValue("@ItemCode", SqlDbType.NVarChar).Value = row.Cells(5).Value
                                .Parameters.AddWithValue("@Itemname", row.Cells(0).Value)
                                .Parameters.AddWithValue("@tranxtype", "Issued")
                                .Parameters.AddWithValue("@tranxsource", "Sales")
                                .Parameters.AddWithValue("@tranxgroup", row.Cells(4).Value)
                                .Parameters.AddWithValue("@oldqty", row.Cells(15).Value)
                                .Parameters.AddWithValue("@qtyIssued", row.Cells(1).Value)
                                .Parameters.AddWithValue("@Balance", row.Cells(8).Value)
                                .Parameters.AddWithValue("@Rprice", row.Cells(2).Value)
                                .Parameters.AddWithValue("@Cprice", row.Cells(2).Value)
                                .Parameters.AddWithValue("@Ramt", row.Cells(5).Value)
                                .Parameters.AddWithValue("@Camt", row.Cells(5).Value)
                                .Parameters.AddWithValue("@userid", Activeuser.Text)
                                .Parameters.AddWithValue("@Date", lblDate.Text)
                                .Parameters.AddWithValue("@Time", lblTime.Text)
                                .ExecuteNonQuery()
                            End With

                        Next
                        Con.Close()
                        'MsgBox("StockMast Updated")
                        reciept()

                        If ckA5Paper.Checked = True Then
                            PrintRecieptA5(lblRecieptNo.Text)
                        ElseIf ckrollPaper.Checked = True Then
                            RollReciept(lblRecieptNo.Text)
                        ElseIf ckA4.Checked = True Then
                            PrintRecieptA4(lblRecieptNo.Text)
                        End If

                        lblTotal.Text = ""
                        clear()
                        gvSales.Rows.Clear()
                        Display()

                    End Try
                ElseIf (cbSaleType.SelectedIndex = 1) Then
                    'cbCreditCustName.Visible = True
                    'lblCreditCust.Visible = True
                    Dim a = Val(lblTotal.Text)
                    Dim b = Val(lblOldBal.Text)
                    Dim sum = b + a
                    lblNewBal.Text = sum
                    If cbCreditCustName.Text = "" Then
                        MsgBox("Choose customer")
                    Else
                        Try
                            'reciept()
                            'Dim i As Integer

                            For Each row As DataGridViewRow In gvSales.Rows

                                If Con.State = ConnectionState.Closed Then
                                    Con.Open()
                                End If
                                Dim query = "insert into salestranx (ItemCode,ItemName,ProdLine,ProdCat,ItemSize,ItemColour,QtySold,DateSold,TimeSold,BuyerName,BuyerTel,BuyerLocation,NewStock,RetailPrice,SaleType,CredCustName,Amount,Soldby,RecieptNo,AmtPaid,Balance,DiscountRate,DiscountAmount,AmountPayable,TotalDiscount) values(@ItemCode,@ItemName,@ProdLine,@ProdCat,@ItemSize,@ItemColour,@QtySold,@DateSold,@TimeSold,@BuyerName,@BuyerTel,@BuyerLocation,@NewStock,@RetailPrice,@SaleType,@CreditCustomerName,@Amount,'" + Activeuser.Text + "', '" + lblRecieptNo.Text + "','" + txtCashPaid.Text + "','" + lblChange.Text + "',@Discount,@DiscAmt,@Amtpayable,'" + lblDiscAmt.Text + "')"
                                cmd = New SqlCommand(query, Con)
                                With cmd

                                    .Parameters.AddWithValue("@ItemCode", SqlDbType.NVarChar).Value = row.Cells(5).Value
                                    .Parameters.AddWithValue("@Itemname", row.Cells(0).Value)
                                    .Parameters.AddWithValue("@ProdLine", row.Cells(7).Value)
                                    .Parameters.AddWithValue("@ProdCat", row.Cells(4).Value)
                                    .Parameters.AddWithValue("@ItemSize", row.Cells(6).Value)
                                    .Parameters.AddWithValue("@ItemColour", row.Cells(5).Value)
                                    .Parameters.AddWithValue("@QtySold", row.Cells(1).Value)
                                    .Parameters.AddWithValue("@DateSold", lblDate.Text)
                                    .Parameters.AddWithValue("@TimeSold", lblTime.Text)
                                    .Parameters.AddWithValue("@BuyerName", txtBuyerName.Text)
                                    .Parameters.AddWithValue("@BuyerTel", txtBuyerTel.Text)
                                    .Parameters.AddWithValue("@BuyerLocation", cbLocation.Text)
                                    .Parameters.AddWithValue("@NewStock", row.Cells(8).Value)
                                    .Parameters.AddWithValue("@RetailPrice", row.Cells(2).Value)
                                    .Parameters.AddWithValue("@Saletype", cbSaleType.Text)
                                    .Parameters.AddWithValue("@CreditCustomerName", cbCreditCustName.Text)
                                    .Parameters.AddWithValue("@Amount", row.Cells(3).Value)
                                    .Parameters.AddWithValue("@Discount", row.Cells(10).Value)
                                    .Parameters.AddWithValue("@DiscAmt", row.Cells(11).Value)
                                    .Parameters.AddWithValue("@AmtPayable", value:=(lblPayable.Text))
                                    .ExecuteNonQuery()
                                End With



                            Next



                        Finally
                            If Con.State = ConnectionState.Closed Then
                                Con.Open()
                            End If
                            Dim quer = "Insert into CustomerLedger(CustomerName,oldbal,Newbal,CreditRecieved,DateRecieved,TimeRecieved,CustomerNo)Values('" + cbCreditCustName.Text + "','" + lblOldBal.Text + "','" + lblNewBal.Text + "','" + lblTotal.Text + "','" + lblDate.Text + "','" + lblTime.Text + "','" + lblCustNo.Text + "')"
                            cmd = New SqlCommand(quer, Con)
                            cmd.ExecuteNonQuery()

                            Dim que = "update Customer set CurrentBalance=@CurBal where CustomerNo = " + lblCustNo.Text + ""
                            cmd = New SqlCommand(que, Con)
                            With cmd

                                .Parameters.AddWithValue("@CurBal", CDbl(lblNewBal.Text))
                                .ExecuteNonQuery()

                            End With


                            'MsgBox("Customer Updated")
                            Con.Close()

                          
                            For k = 0 To gvSales.RowCount - 1
                                If Con.State = ConnectionState.Closed Then
                                    Con.Open()
                                End If
                                Dim sqll = "Select * from StockMast where Prodcode='" + gvSales.Rows(k).Cells(5).Value + "'"
                                cmd = New SqlCommand(sqll, Con)
                                dr = cmd.ExecuteReader
                                While dr.Read

                                    Dim query = "update StockMast set prodqty = '" & dr.Item("ProdQty") - gvSales.Rows(k).Cells(1).Value & "' where Prodcode= " & gvSales.Rows(k).Cells(5).Value & ""
                                    cmd = New SqlCommand(query, Con)
                                    cmd.ExecuteNonQuery()
                                End While
                            Next
                            For Each row As DataGridViewRow In gvSales.Rows
                                Dim sql = "insert into InventoryLedger (ItemCode,itemname,tranxtype,TranxSource,TranxGroup,oldqty,qtyIssued,StockBalance,Userid,RetailPrice,CostPrice,RetailAmt,CostAmt,time,date) values(@ItemCode,@Itemname,@Tranxtype,@tranxsource,@TranxGroup,@oldqty,@qtyIssued,@balance,@userid,@Rprice,@cprice,@ramt,@camt,@time,@date)"
                                cmd = New SqlCommand(sql, Con)
                                With cmd
                                    .Parameters.AddWithValue("@ItemCode", row.Cells(5).Value)
                                    .Parameters.AddWithValue("@Itemname", row.Cells(0).Value)
                                    .Parameters.AddWithValue("@tranxtype", "Issued")
                                    .Parameters.AddWithValue("@tranxsource", "Sales")
                                    .Parameters.AddWithValue("@tranxgroup", row.Cells(4).Value)
                                    .Parameters.AddWithValue("@oldqty", row.Cells(15).Value)
                                    .Parameters.AddWithValue("@qtyIssued", row.Cells(1).Value)
                                    .Parameters.AddWithValue("@Balance", row.Cells(8).Value)
                                    .Parameters.AddWithValue("@Rprice", row.Cells(2).Value)
                                    .Parameters.AddWithValue("@Cprice", row.Cells(2).Value)
                                    .Parameters.AddWithValue("@Ramt", row.Cells(5).Value)
                                    .Parameters.AddWithValue("@Camt", row.Cells(5).Value)
                                    .Parameters.AddWithValue("@userid", Activeuser.Text)
                                    .Parameters.AddWithValue("@Date", lblDate.Text)
                                    .Parameters.AddWithValue("@Time", lblTime.Text)
                                    .ExecuteNonQuery()
                                End With

                            Next
                            Con.Close()

                            ShowConfig()
                            'MsgBox("StockMast Updated")
                            reciept()

                            If ckA5Paper.Checked = True Then
                                PrintRecieptA5(lblRecieptNo.Text)
                            ElseIf ckrollPaper.Checked = True Then
                                RollReciept(lblRecieptNo.Text)
                            ElseIf ckA4.Checked = True Then
                                PrintRecieptA4(lblRecieptNo.Text)
                            End If
                            gvSales.Rows.Clear()
                            lblTotal.Text = ""
                            clear()
                            Display()

                            lblTotal.Text = ""
                        End Try
                    End If
                End If
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
        End If

        ShowConfig()
        txtProdname.Focus()
    End Sub

    Private Sub ckrollPaper_Click(sender As Object, e As EventArgs) Handles ckrollPaper.Click
        If ckrollPaper.Checked = False Then
            ckA5Paper.Checked = True
            ckA4.Checked = False
        Else
            ckA5Paper.Checked = False
            ckA4.Checked = False
        End If
        If ckA5Paper.Checked = False And ckrollPaper.Checked = False Then
            ckA4.Checked = True
        End If
    End Sub

    Private Sub ckA5Paper_Click(sender As Object, e As EventArgs) Handles ckA5Paper.Click
        If ckA5Paper.Checked = False Then
            ckrollPaper.Checked = True
            ckA4.Checked = False
        Else
            ckrollPaper.Checked = False
            ckA4.Checked = False
        End If
        If ckA5Paper.Checked = False And ckrollPaper.Checked = False Then
            ckA4.Checked = True
        End If
    End Sub

    Private Sub BunifuThinButton22_Click(sender As Object, e As EventArgs) Handles BunifuThinButton22.Click
        gvSales.Rows.Clear()
    End Sub

    Private Sub gvSales_RowsRemoved(sender As Object, e As DataGridViewRowsRemovedEventArgs) Handles gvSales.RowsRemoved
        Try
            Dim sum As Decimal = 0
            Dim payable As Decimal = 0
            Dim discamt As Decimal = 0
            For k = 0 To gvSales.RowCount - 1
                If gvSales.Rows(k).Cells(10).ToString = "" Then
                    gvSales.Rows(k).Cells(12).Value = gvSales.Rows(k).Cells(3).Value
                    gvSales.Rows(k).Cells(11).Value = "0"
                End If
                sum += gvSales.Rows(k).Cells(3).Value
                payable += gvSales.Rows(k).Cells(12).Value
                discamt += gvSales.Rows(k).Cells(11).Value
            Next
            lblTotal.Text = sum
            lblPayable.Text = payable
            lblDiscAmt.Text = discamt
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub gvSales_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles gvSales.CellClick
        Dim row As DataGridViewRow = gvSales.Rows(e.RowIndex)
        txtDiscName.Text = row.Cells(0).Value.ToString()
        lbldiscCode.Text = row.Cells(5).Value.ToString()
    End Sub

    Private Sub BunifuCheckBox2_Click(sender As Object, e As EventArgs) Handles ckPerDisc.Click
        If ckPerDisc.Checked = False Then
            ckCashDisc.Checked = True
            txtCashDisc.Enabled = True
        Else
            ckCashDisc.Checked = False
            txtCashDisc.Enabled = False
        End If

        If ckCashDisc.Checked = False Then
            txtCashDisc.Enabled = False
            txtPerDisc.Enabled = True
        Else
            txtCashDisc.Enabled = True
            txtPerDisc.Enabled = False
        End If

    End Sub

    Private Sub BunifuCheckBox1_Click(sender As Object, e As EventArgs) Handles ckCashDisc.Click
        If ckCashDisc.Checked = False Then
            ckPerDisc.Checked = True

        Else
            ckPerDisc.Checked = False

        End If

        If ckPerDisc.Checked = False Then
            txtPerDisc.Enabled = False
            txtCashDisc.Enabled = True
        Else
            txtPerDisc.Enabled = True
            txtCashDisc.Enabled = False
        End If

    End Sub

    Private Sub BunifuThinButton23_Click(sender As Object, e As EventArgs) Handles BunifuThinButton23.Click

        If lbldiscCode.Text = "" Then
            MsgBox("Select Item to Discount")
            Exit Sub
        End If


        Dim sum As Decimal = 0
        Dim payable As Decimal = 0
        Dim DiscAmt As Decimal = 0
        If ckPerDisc.Checked = True Then
            For Each row As DataGridViewRow In gvSales.Rows

                If lbldiscCode.Text = row.Cells(5).Value Then
                    If row.Cells(10).Value = 0 Then
                        Dim Discprice As Decimal
                        Discprice = (Val(txtPerDisc.Text) / 100) * row.Cells(3).Value
                        row.Cells(10).Value = txtPerDisc.Text
                        row.Cells(11).Value = Discprice
                        row.Cells(12).Value = row.Cells(3).Value - Discprice
                        row.Cells(13).Value = "%"
                    Else
                        MsgBox("Discount Already Added", vbCritical)
                        lbldiscCode.Text = ""
                        txtDiscName.Text = ""
                        Exit Sub
                    End If
                End If
            Next
            For k = 0 To gvSales.RowCount - 1
                If gvSales.Rows(k).Cells(10).ToString = "" Then
                    gvSales.Rows(k).Cells(12).Value = gvSales.Rows(k).Cells(3).Value
                    gvSales.Rows(k).Cells(11).Value = "0"
                End If
                sum += gvSales.Rows(k).Cells(3).Value
                payable += gvSales.Rows(k).Cells(12).Value
                DiscAmt += gvSales.Rows(k).Cells(11).Value
            Next
            lblTotal.Text = sum
            lblPayable.Text = payable
            lblDiscAmt.Text = DiscAmt
            MsgBox("Discount Added")

        End If
        If ckCashDisc.Checked = True Then
            For Each row As DataGridViewRow In gvSales.Rows
                If lbldiscCode.Text = row.Cells(5).Value Then
                    If row.Cells(10).Value = 0 Then
                        Dim Discprice As Decimal
                        Discprice = row.Cells(3).Value - Val(txtCashDisc.Text)
                        row.Cells(10).Value = txtCashDisc.Text
                        row.Cells(12).Value = Discprice
                        row.Cells(11).Value = row.Cells(3).Value - Discprice
                        row.Cells(13).Value = "$"
                    Else

                        MsgBox("Discount Already Added", vbCritical)
                        lbldiscCode.Text = ""
                        txtDiscName.Text = ""
                        Exit Sub
                    End If
                End If

            Next
            For k = 0 To gvSales.RowCount - 1
                If gvSales.Rows(k).Cells(10).Value = "" Then
                    gvSales.Rows(k).Cells(12).Value = gvSales.Rows(k).Cells(3).Value
                    gvSales.Rows(k).Cells(11).Value = "0"
                End If
                sum += gvSales.Rows(k).Cells(3).Value
                payable += gvSales.Rows(k).Cells(12).Value
                DiscAmt += gvSales.Rows(k).Cells(11).Value
            Next
            lblTotal.Text = sum
            lblPayable.Text = payable
            lblDiscAmt.Text = DiscAmt
            MsgBox("Discount Added")
        End If

        lbldiscCode.Text = ""
        txtDiscName.Text = ""
    End Sub

    Private Sub BunifuThinButton24_Click(sender As Object, e As EventArgs)
    End Sub

    Private Sub BunifuThinButton25_Click(sender As Object, e As EventArgs) Handles BunifuThinButton25.Click
        If gvSales.Rows.Count = 0 Then
            MsgBox("Make a sale to Discount")
            Exit Sub
        End If

        Dim sum As Decimal = 0
        Dim payable As Decimal = 0
        Dim DiscAmt As Decimal = 0

        If ckPerDisc.Checked = True Then
            Dim ask As MsgBoxResult
            ask = MsgBox("Would you like to discount Cart by " + txtPerDisc.Text + "%?", MsgBoxStyle.YesNo, "")
            If ask = MsgBoxResult.Yes Then
                For Each row As DataGridViewRow In gvSales.Rows

                    If True Then
                        If row.Cells(10).Value = 0 Then
                            Dim Discprice As Decimal
                            Discprice = (Val(txtPerDisc.Text) / 100) * row.Cells(3).Value
                            row.Cells(10).Value = txtPerDisc.Text
                            row.Cells(11).Value = Discprice
                            row.Cells(12).Value = row.Cells(3).Value - Discprice
                            row.Cells(13).Value = "%"
                        Else
                            MsgBox("Discount Already Added", vbCritical)
                            lbldiscCode.Text = ""
                            txtDiscName.Text = ""
                            Exit Sub
                        End If
                    End If
                Next
                For k = 0 To gvSales.RowCount - 1
                    If gvSales.Rows(k).Cells(10).ToString = "" Then
                        gvSales.Rows(k).Cells(12).Value = gvSales.Rows(k).Cells(3).Value
                        gvSales.Rows(k).Cells(11).Value = "0"
                    End If
                    sum += gvSales.Rows(k).Cells(3).Value
                    payable += gvSales.Rows(k).Cells(12).Value
                    DiscAmt += gvSales.Rows(k).Cells(11).Value
                Next
                lblTotal.Text = sum
                lblPayable.Text = payable
                lblDiscAmt.Text = DiscAmt
                MsgBox("Discount Added")

            End If
        End If
        If ckCashDisc.Checked = True Then
            Dim ask As MsgBoxResult
            ask = MsgBox("Would you like to discount Cart by ¢" + txtCashDisc.Text + "?", MsgBoxStyle.YesNo, "")
            If ask = MsgBoxResult.Yes Then
                For Each row As DataGridViewRow In gvSales.Rows
                    If True Then

                        If row.Cells(10).Value = 0 Then
                            Dim Discprice As Decimal
                            Discprice = row.Cells(3).Value - Val(txtCashDisc.Text)
                            row.Cells(10).Value = txtCashDisc.Text
                            row.Cells(11).Value = Val(row.Cells(3).Value - Discprice) / Val(gvSales.Rows.Count)
                            row.Cells(12).Value = row.Cells(3).Value - row.Cells(11).Value
                            row.Cells(13).Value = "$"
                        Else

                            MsgBox("Discount Already Added", vbCritical)
                            lbldiscCode.Text = ""
                            txtDiscName.Text = ""
                            Exit Sub
                        End If
                    End If

                Next
                For k = 0 To gvSales.RowCount - 1
                    If gvSales.Rows(k).Cells(10).Value = "" Then
                        gvSales.Rows(k).Cells(12).Value = gvSales.Rows(k).Cells(3).Value
                        gvSales.Rows(k).Cells(11).Value = "0"
                    End If
                    sum += gvSales.Rows(k).Cells(3).Value
                    payable += gvSales.Rows(k).Cells(12).Value
                    DiscAmt += gvSales.Rows(k).Cells(11).Value
                Next
                lblTotal.Text = sum
                lblPayable.Text = payable
                lblDiscAmt.Text = Math.Round(DiscAmt)
                MsgBox("Discount Added")
            End If
        End If
        lbldiscCode.Text = ""
        txtDiscName.Text = ""

    End Sub
    Public Sub FillSale(valueTosearch As String)
        Try
            If Con.State = ConnectionState.Closed Then
                Con.Open()
            End If

            Dim query = "select ProdName,ProdQty,retailprice,Prodsize,ProdCat,ProdColour,Prodline,ProdCode from StockMast where concat(ProdName,ProdCode) like '%" + valueTosearch + "%'"
            cmd = New SqlCommand(query, Con)
            Dim adapter As New SqlDataAdapter(cmd)
            Dim table As New DataTable()
            adapter.Fill(table)
            gvStock.DataSource = table


            cbProdName.Text = table.Rows(0)(0).ToString
            lblProdName.Text = table.Rows(0)(0).ToString
            'lblProdName.Text = table.Rows(0)(0).ToString + " " + "(" + txtProdline.Text + ")"
            txtPrice.Text = table.Rows(0)(2).ToString
            lblActualStock.Text = table.Rows(0)(1).ToString
            txtProdcode.Text = table.Rows(0)(7).ToString
            txtProdline.Text = table.Rows(0)(6).ToString
            txtCat.Text = table.Rows(0)(4).ToString
            txtSize.Text = table.Rows(0)(3).ToString
            txtColour.Text = table.Rows(0)(5).ToString


            Con.Close()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub txtProdname_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles txtProdname.SelectionChangeCommitted
        FillSale(txtProdname.SelectedItem)
    End Sub

    Private Sub txtProdname_SelectedIndexChanged(sender As Object, e As EventArgs) Handles txtProdname.SelectedIndexChanged
        FillSale(txtProdname.SelectedItem)
    End Sub

    Private Sub BunifuThinButton26_Click(sender As Object, e As EventArgs)
    End Sub

    Private Sub txtQty_MouseLeave(sender As Object, e As EventArgs) Handles txtQty.MouseLeave
        If txtQty.Text = "" Then
            Exit Sub
        End If
        For Each row As DataGridViewRow In gvPriceBand.Rows
            If row.Cells(1).Value = lblProdName.Text And row.Cells(3).Value <= CDbl(txtQty.Text) And row.Cells(4).Value >= CDbl(txtQty.Text) Then
                txtPrice.Text = row.Cells(5).Value
            Else
                'txtPrice.Text = lblOPrice.Text
            End If
            'MsgBox(row.Cells(5).Value)
        Next

        Dim amt As Decimal
        amt = Val(txtQty.Text) * Val(txtPrice.Text)
        txtAmt.Text = amt
    End Sub

    Private Sub BunifuThinButton27_Click(sender As Object, e As EventArgs) Handles BunifuThinButton27.Click
        frmSales_Load(e, e)
    End Sub

    Private Sub ckA4_Click(sender As Object, e As EventArgs) Handles ckA4.Click
        If ckA4.Checked = True Then
            ckA5Paper.Checked = False
            ckrollPaper.Checked = False
        Else
            ckA4.Checked = True
        End If
    End Sub

    Private Sub txtProdname_KeyDown(sender As Object, e As KeyEventArgs) Handles txtProdname.KeyDown
        If e.KeyCode = Keys.Enter Then
            FillSale(txtProdname.Text)
            txtQty.Focus()
        End If
    End Sub

    Private Sub txtQty_KeyDown(sender As Object, e As KeyEventArgs) Handles txtQty.KeyDown
        If txtQty.Text Is Nothing Then
            MsgBox("Enter Qty")
            Exit Sub
        End If
        If e.KeyCode = Keys.Enter Then
            PictureBox1_Click(Nothing, Nothing)
            txtProdname.Focus()
        End If
    End Sub

    Private Sub txtCashPaid_KeyDown(sender As Object, e As KeyEventArgs) Handles txtCashPaid.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtBuyerName.Focus()
        End If
    End Sub

    Private Sub txtBuyerName_KeyDown(sender As Object, e As KeyEventArgs) Handles txtBuyerName.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtBuyerTel.Focus()
        End If
    End Sub

    Private Sub txtBuyerTel_KeyDown(sender As Object, e As KeyEventArgs) Handles txtBuyerTel.KeyDown
        If e.KeyCode = Keys.Enter Then
            BunifuThinButton21_Click(Nothing, Nothing)
        End If
    End Sub

    Sub RollReciept(valuetosearch As String)
        Try
            If Con.State = ConnectionState.Closed Then
                Con.Open()
            End If

            Dim que = "select * from recieptconfig"
            cmd = New SqlCommand(que, Con)
            da = New SqlDataAdapter(cmd)
            Dim table As New DataTable
            da.Fill(table)
            Dim query = "select * from SalesTranx where recieptno='" + valuetosearch + "'"
            cmd = New SqlCommand(query, Con)
            dt.Tables("salesTranx").Rows.Clear()
            da.SelectCommand = cmd
            da.Fill(dt, "salesTranx")

            Dim sql = "select * from ClientReg"
            dt.Tables("ClientReg").Rows.Clear()
            cmd = New SqlCommand(sql, Con)
            da.SelectCommand = cmd
            da.Fill(dt, "ClientReg")

            Dim report As New rptSalesReciept
            report.SetDataSource(dt)
            If ckprint.Checked = True Then
                report.PrintToPrinter(1, True, 0, 0)
            End If
            If ckprintpreview.Checked = True Then
                frmSupplierReport.Show()
                frmSupplierReport.CrystalReportViewer1.ReportSource = report
                frmSupplierReport.CrystalReportViewer1.Refresh()
            End If
            cmd.Dispose()
            da.Dispose()
            Con.Close()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Sub PrintRecieptA4(valuetosearch As String)
        Try
            If Con.State = ConnectionState.Closed Then
                Con.Open()
            End If

            Dim que = "select * from recieptconfig"
            cmd = New SqlCommand(que, Con)
            da = New SqlDataAdapter(cmd)
            Dim table As New DataTable
            da.Fill(table)
            Dim query = "select * from SalesTranx where recieptno='" + valuetosearch + "'"
            cmd = New SqlCommand(query, Con)
            dt.Tables("salesTranx").Rows.Clear()
            da.SelectCommand = cmd
            da.Fill(dt, "salesTranx")

            Dim sql = "select * from ClientReg"
            dt.Tables("ClientReg").Rows.Clear()
            cmd = New SqlCommand(sql, Con)
            da.SelectCommand = cmd
            da.Fill(dt, "ClientReg")

            Dim report As New rptSalesRecieptA4
            report.SetDataSource(dt)
            If ckprint.Checked = True Then
                report.PrintToPrinter(1, True, 0, 0)
            End If
            If ckprintpreview.Checked = True Then
                frmSupplierReport.Show()
                frmSupplierReport.CrystalReportViewer1.ReportSource = report
                frmSupplierReport.CrystalReportViewer1.Refresh()
            End If

            cmd.Dispose()
            da.Dispose()
            Con.Close()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Sub PrintRecieptA5(valuetosearch As String)
        Try
            If Con.State = ConnectionState.Closed Then
                Con.Open()
            End If

            Dim que = "select * from recieptconfig"
            cmd = New SqlCommand(que, Con)
            da = New SqlDataAdapter(cmd)
            Dim table As New DataTable
            da.Fill(table)
            Dim query = "select * from SalesTranx where recieptno='" + valuetosearch + "'"
            cmd = New SqlCommand(query, Con)
            dt.Tables("salesTranx").Rows.Clear()
            da.SelectCommand = cmd
            da.Fill(dt, "salesTranx")

            Dim sql = "select * from ClientReg"
            dt.Tables("ClientReg").Rows.Clear()
            cmd = New SqlCommand(sql, Con)
            da.SelectCommand = cmd
            da.Fill(dt, "ClientReg")

            Dim report As New rptSalesRecieptA5
            report.SetDataSource(dt)
            If ckprint.Checked = True Then
                report.PrintToPrinter(1, True, 0, 0)
            End If
            If ckprintpreview.Checked = True Then
                frmSupplierReport.Show()
                frmSupplierReport.CrystalReportViewer1.ReportSource = report
                frmSupplierReport.CrystalReportViewer1.Refresh()
            End If
            cmd.Dispose()
            da.Dispose()
            Con.Close()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Sub Sortcat(valuetosearch As String)
        If Con.State = ConnectionState.Closed Then
            Con.Open()
        End If
        Dim query = "select ProdName,ProdQty,retailprice,Prodsize,ProdCat,ProdColour,Prodline,ProdCode from StockMast where ProdCat like '%" + valuetosearch + "%'"
        cmd = New SqlCommand(query, Con)
        Dim adapter As New SqlDataAdapter(cmd)
        Dim table As New DataTable()
        adapter.Fill(table)
        gvStock.DataSource = table
        Con.Close()

    End Sub

    Sub Sortpline(valuetosearch As String)
        If Con.State = ConnectionState.Closed Then
            Con.Open()
        End If
        Dim query = "select ProdName,ProdQty,retailprice,Prodsize,ProdCat,ProdColour,Prodline,ProdCode from StockMast where prodline like '%" + valuetosearch + "%'"
        cmd = New SqlCommand(query, Con)
        Dim adapter As New SqlDataAdapter(cmd)
        Dim table As New DataTable()
        adapter.Fill(table)
        gvStock.DataSource = table
        Con.Close()

    End Sub

    Private Sub BunifuThinButton28_Click(sender As Object, e As EventArgs) Handles BunifuThinButton28.Click
        If ckA5Paper.Checked = True Then
            PrintRecieptA5(txtReprintNo.Text)
        ElseIf ckrollPaper.Checked = True Then
            RollReciept(txtReprintNo.Text)
        ElseIf ckA4.Checked = True Then
            PrintRecieptA4(txtReprintNo.Text)
        End If
    End Sub

    Private Sub cbCatSort_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cbCatSort.SelectionChangeCommitted
        Sortcat(cbCatSort.Text)
        'cbProdlineSort.SelectedIndex = -1
    End Sub

    Private Sub cbProdlineSort_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cbProdlineSort.SelectionChangeCommitted
        Sortpline(cbProdlineSort.Text)
        'cbCatSort.SelectedIndex = -1
    End Sub

    Private Sub BunifuThinButton29_Click(sender As Object, e As EventArgs) Handles BunifuThinButton29.Click
        cbCatSort.SelectedIndex = -1
        cbProdlineSort.SelectedIndex = -1
        Display()
    End Sub

    Private Sub cbProdlineSort_Click(sender As Object, e As EventArgs) Handles cbProdlineSort.Click
        If Con.State = ConnectionState.Closed Then
            Con.Open()
        End If
        cbProdlineSort.Items.Clear()
        Dim pli = "select productline from productline"
        cmd = New SqlCommand(pli, Con)
        dr = cmd.ExecuteReader
        While dr.Read
            cbProdlineSort.Items.Add(dr(0))
        End While
        Con.Close()
    End Sub

    Private Sub cbCatSort_Click(sender As Object, e As EventArgs) Handles cbCatSort.Click
        If Con.State = ConnectionState.Closed Then
            Con.Open()
        End If
        cbCatSort.Items.Clear()
        Dim sqll = "select category from Category"
        cmd = New SqlCommand(sqll, Con)
        dr = cmd.ExecuteReader
        While dr.Read
            cbCatSort.Items.Add(dr(0))
        End While
        Con.Close()
    End Sub

    Private Sub cbCreditCustName_Click(sender As Object, e As EventArgs) Handles cbCreditCustName.Click
        If Con.State = ConnectionState.Closed Then
            Con.Open()
        End If
        cbCreditCustName.Items.Clear()
        Dim sql = "select * from Customer"
        cmd = New SqlCommand(sql, Con)
        dr = cmd.ExecuteReader
        While dr.Read
            cbCreditCustName.Items.Add(dr(1))

        End While
        Con.Close()
    End Sub

    Private Sub txtProdname_Enter(sender As Object, e As EventArgs) Handles txtProdname.Enter
        If Con.State = ConnectionState.Closed Then
            Con.Open()
        End If
        txtProdname.Items.Clear()
        Dim query = "select * from Stockmast"
        cmd = New SqlCommand(query, Con)
        dr = cmd.ExecuteReader
        While dr.Read
            txtProdname.Items.Add(dr(1))
        End While
        Con.Close()
    End Sub
End Class