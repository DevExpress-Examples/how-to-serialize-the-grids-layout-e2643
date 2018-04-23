using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Controls;

namespace GridLayoutSerialization {
    public partial class MainPage : UserControl {
        string fileName = "gridLayout.xml";
        string layoutFolderName = "gridLayout";

        static MainPage() {
            IsLayoutSavedProperty = DependencyProperty.Register("IsLayoutSaved",
                typeof(bool), typeof(MainPage), new PropertyMetadata(null));
        }
        public MainPage() {
            InitializeComponent();
            IsLayoutSaved = CheckGridSaved();
            grid.DataSource = IssueList.GetData();
        }
        public static readonly DependencyProperty IsLayoutSavedProperty;
        public bool IsLayoutSaved {
            get { return (bool)GetValue(MainPage.IsLayoutSavedProperty); }
            set { SetValue(MainPage.IsLayoutSavedProperty, value); }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e) {
            try {
                SaveGridLayoutToIsolatedStorage();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
            IsLayoutSaved = true;
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e) {
            try {
                RestoreGridLayoutFromIsolatedStorage();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        void SaveGridLayoutToIsolatedStorage() {
            IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication();
            if (!file.DirectoryExists(layoutFolderName)) {
                file.CreateDirectory(layoutFolderName);
            }
            string fullPath = System.IO.Path.Combine(layoutFolderName, fileName);
            using (IsolatedStorageFileStream fs = file.CreateFile(fullPath)) {
                grid.SaveLayoutToStream(fs);
            }
        }
        bool CheckGridSaved() {
            IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication();
            string fullPath = System.IO.Path.Combine(layoutFolderName, fileName);
            return file.FileExists(fullPath);

        }
        void RestoreGridLayoutFromIsolatedStorage() {
            IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication();
            string fullPath = System.IO.Path.Combine(layoutFolderName, fileName);
            using (IsolatedStorageFileStream fs = file.OpenFile(fullPath, FileMode.Open, FileAccess.Read)) {
                grid.RestoreLayoutFromStream(fs);
            }
        }
    }
    public class IssueList {
        static public List<IssueDataObject> GetData() {
            List<IssueDataObject> data = new List<IssueDataObject>();
            data.Add(new IssueDataObject() {
                IssueName = "Transaction History",
                IssueType = "Bug",
                IsPrivate = true
            });
            data.Add(new IssueDataObject() {
                IssueName = "Ledger: Inconsistency",
                IssueType = "Bug",
                IsPrivate = false
            });
            data.Add(new IssueDataObject() {
                IssueName = "Data Import",
                IssueType = "Request",
                IsPrivate = false
            });
            data.Add(new IssueDataObject() {
                IssueName = "Data Archiving",
                IssueType = "Request",
                IsPrivate = true
            });
            return data;
        }
    }

    public class IssueDataObject {
        public string IssueName { get; set; }
        public string IssueType { get; set; }
        public bool IsPrivate { get; set; }
    }
}
