Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Windows.Forms
Imports System.ComponentModel

Namespace FilterMasterDetailGrid
    Partial Public Class Form1
        Inherits Form

        Public Sub New()
            InitializeComponent()
            Dim bs As New BindingSource()
            bs.DataSource = CreateObjectSource(3, 5)
            gridControl1.DataSource = bs
            Dim tempVar As New FilterHelper(gridView1, gridView2)

        End Sub

        Private Function CreateObjectSource(ByVal master As Integer, ByVal detail As Integer) As BindingList(Of Record)
            Dim list As New BindingList(Of Record)()
            For i As Integer = 0 To master - 1
                Dim rec As New Record()
                rec.Value = "Master " & i
                rec.Detail = New BindingList(Of Record)()
                For j As Integer = 0 To detail - 1
                    Dim childRec As New Record()
                    childRec.Value = "Child " & i & " " & + j
                    rec.Detail.Add(childRec)
                Next j
                list.Add(rec)
            Next i
            Return list
        End Function

        Private Class Record
            Private _value As String
            Private _detail As BindingList(Of Record)
            Public Property Value() As String
                Get
                    Return _value
                End Get
                Set(ByVal value As String)
                    _value = value
                End Set
            End Property

            Public Property Detail() As BindingList(Of Record)
                Get
                    Return _detail
                End Get
                Set(ByVal value As BindingList(Of Record))
                    _detail = value
                End Set
            End Property
        End Class


        Private Function CreateTable(ByVal RowCount As Integer) As DataTable
            Dim tbl As New DataTable("Parent")
            tbl.Columns.Add("Name", GetType(String))
            tbl.Columns.Add("Number", GetType(Integer))
            tbl.Columns.Add("Date", GetType(Date))
            tbl.Columns.Add("DETAILID", GetType(Integer))
            For i As Integer = 0 To RowCount - 1
                tbl.Rows.Add(New Object() { String.Format("Name{0}", i), 3 - i, Date.Now.AddDays(i), i })
            Next i
            Return tbl
        End Function

        Private Function CreateDet1Table(ByVal RowCount As Integer) As DataTable
            Dim tbl As New DataTable("Det1")
            tbl.Columns.Add("Name", GetType(String))
            tbl.Columns.Add("ID", GetType(Integer))
            For j As Integer = 0 To 4
                For i As Integer = 0 To RowCount - 1
                    tbl.Rows.Add(New Object() { String.Format("Name{0}", j + (i Mod 2) * 5), i })
                Next i
            Next j
            Return tbl
        End Function

        Private Function GetMasterDetail() As DataSet
            Dim ds As New DataSet("TestDS")
            ds.Tables.Add(CreateTable(20))
            ds.Tables.Add(CreateDet1Table(20))
            Dim parentColumn As DataColumn = ds.Tables("Parent").Columns("DETAILID")
            Dim childColumn As DataColumn = ds.Tables("Det1").Columns("ID")
            ds.Relations.Add(New DataRelation("Detail", parentColumn, childColumn))
            Return ds
        End Function

        Private Sub gridView2_ColumnFilterChanged(ByVal sender As Object, ByVal e As EventArgs) Handles gridView2.ColumnFilterChanged
            gridView1.DataController.RefreshData()
        End Sub

        Private Sub simpleButton1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles simpleButton1.Click
            gridView2.ActiveFilter.Clear()
        End Sub

        Private Sub simpleButton2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles simpleButton2.Click
            gridControl1.DataSource = GetMasterDetail().Tables("Parent")
            gridView1.PopulateColumns()
            gridView2.PopulateColumns()
        End Sub

        Private Sub simpleButton3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles simpleButton3.Click
            gridControl1.DataSource = CreateObjectSource(3, 5)
            gridView1.PopulateColumns()
            gridView2.PopulateColumns()
        End Sub

        Private Sub simpleButton4_Click(ByVal sender As Object, ByVal e As EventArgs) Handles simpleButton4.Click
            Dim bs As New BindingSource()
            bs.DataSource = CreateObjectSource(3, 5)
            gridControl1.DataSource = bs
            gridView1.PopulateColumns()
            gridView2.PopulateColumns()
        End Sub
    End Class
End Namespace
