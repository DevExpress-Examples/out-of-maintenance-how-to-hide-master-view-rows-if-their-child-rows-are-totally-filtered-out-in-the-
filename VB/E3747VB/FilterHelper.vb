Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.Data
Imports System.Data
Imports System.ComponentModel
Imports DevExpress.Data.Filtering.Helpers
Imports System.Collections

Namespace FilterMasterDetailGrid
	Public Class FilterHelper
		Private _parentView As GridView
		Private _childView As GridView

		Public Sub New(ByVal parentView As GridView, ByVal childView As GridView)
			_parentView = parentView
			AddHandler _parentView.CustomRowFilter, AddressOf _view_CustomRowFilter
			_childView = childView
		End Sub

		Private Sub _view_CustomRowFilter(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.RowFilterEventArgs)
			Dim view As GridView = (TryCast(sender, GridView))
			Dim controller As BaseGridController = view.DataController
			Dim pdc As PropertyDescriptorCollection = Nothing
				Dim source As IList = CType(view.DataSource, IList)
			If TypeOf view.DataSource Is ITypedList Then
				pdc = (TryCast(view.DataSource, ITypedList)).GetItemProperties(Nothing)
			Else
				pdc = TypeDescriptor.GetProperties(CType(source, Object).GetType().GetProperty("Item").PropertyType)
			End If
			Dim ev As New ExpressionEvaluator(pdc, controller.FilterCriteria)
			e.Visible = (Not IsEmptyDetail(e.ListSourceRow, controller)) AndAlso ev.Fit(source(e.ListSourceRow))
			e.Handled = True
		End Sub


		Private Function IsEmptyDetail(ByVal listSourceRow As Integer, ByVal controller As BaseGridController) As Boolean
			Dim detail As IList = CType(controller.GetDetailList(listSourceRow, 0), IList)
			Dim pdc As PropertyDescriptorCollection = Nothing
			If TypeOf detail Is ITypedList Then
				pdc = (TryCast(detail, ITypedList)).GetItemProperties(Nothing)
			Else
				pdc = TypeDescriptor.GetProperties(CType(detail, Object).GetType().GetProperty("Item").PropertyType)
			End If
			Dim detailEvaluator As New ExpressionEvaluator(pdc, _childView.DataController.FilterCriteria)
			For Each o As Object In detail
				If detailEvaluator.Fit(o) Then
					Return False
				End If
			Next o
			Return True
		End Function
	End Class
End Namespace
