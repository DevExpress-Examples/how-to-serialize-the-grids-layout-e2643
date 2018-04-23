Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.IO.IsolatedStorage
Imports System.Windows
Imports System.Windows.Controls

Namespace GridLayoutSerialization
	Partial Public Class MainPage
		Inherits UserControl
		Private fileName As String = "gridLayout.xml"
		Private layoutFolderName As String = "gridLayout"

		Shared Sub New()
			IsLayoutSavedProperty = DependencyProperty.Register("IsLayoutSaved", GetType(Boolean), GetType(MainPage), New PropertyMetadata(Nothing))
		End Sub
		Public Sub New()
			InitializeComponent()
			IsLayoutSaved = CheckGridSaved()
			grid.DataSource = IssueList.GetData()
		End Sub
		Public Shared ReadOnly IsLayoutSavedProperty As DependencyProperty
		Public Property IsLayoutSaved() As Boolean
			Get
				Return CBool(GetValue(MainPage.IsLayoutSavedProperty))
			End Get
			Set(ByVal value As Boolean)
				SetValue(MainPage.IsLayoutSavedProperty, value)
			End Set
		End Property

		Private Sub SaveButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			Try
				SaveGridLayoutToIsolatedStorage()
			Catch ex As Exception
				MessageBox.Show(ex.Message)
			End Try
			IsLayoutSaved = True
		End Sub

		Private Sub LoadButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			Try
				RestoreGridLayoutFromIsolatedStorage()
			Catch ex As Exception
				MessageBox.Show(ex.Message)
			End Try
		End Sub

		Private Sub SaveGridLayoutToIsolatedStorage()
			Dim file As IsolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication()
			If (Not file.DirectoryExists(layoutFolderName)) Then
				file.CreateDirectory(layoutFolderName)
			End If
			Dim fullPath As String = System.IO.Path.Combine(layoutFolderName, fileName)
			Using fs As IsolatedStorageFileStream = file.CreateFile(fullPath)
				grid.SaveLayoutToStream(fs)
			End Using
		End Sub
		Private Function CheckGridSaved() As Boolean
			Dim file As IsolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication()
			Dim fullPath As String = System.IO.Path.Combine(layoutFolderName, fileName)
			Return file.FileExists(fullPath)

		End Function
		Private Sub RestoreGridLayoutFromIsolatedStorage()
			Dim file As IsolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication()
			Dim fullPath As String = System.IO.Path.Combine(layoutFolderName, fileName)
			Using fs As IsolatedStorageFileStream = file.OpenFile(fullPath, FileMode.Open, FileAccess.Read)
				grid.RestoreLayoutFromStream(fs)
			End Using
		End Sub
	End Class
	Public Class IssueList
		Public Shared Function GetData() As List(Of IssueDataObject)
			Dim data As New List(Of IssueDataObject)()
			data.Add(New IssueDataObject() With {.IssueName = "Transaction History", .IssueType = "Bug", .IsPrivate = True})
			data.Add(New IssueDataObject() With {.IssueName = "Ledger: Inconsistency", .IssueType = "Bug", .IsPrivate = False})
			data.Add(New IssueDataObject() With {.IssueName = "Data Import", .IssueType = "Request", .IsPrivate = False})
			data.Add(New IssueDataObject() With {.IssueName = "Data Archiving", .IssueType = "Request", .IsPrivate = True})
			Return data
		End Function
	End Class

	Public Class IssueDataObject
		Private privateIssueName As String
		Public Property IssueName() As String
			Get
				Return privateIssueName
			End Get
			Set(ByVal value As String)
				privateIssueName = value
			End Set
		End Property
		Private privateIssueType As String
		Public Property IssueType() As String
			Get
				Return privateIssueType
			End Get
			Set(ByVal value As String)
				privateIssueType = value
			End Set
		End Property
		Private privateIsPrivate As Boolean
		Public Property IsPrivate() As Boolean
			Get
				Return privateIsPrivate
			End Get
			Set(ByVal value As Boolean)
				privateIsPrivate = value
			End Set
		End Property
	End Class
End Namespace
