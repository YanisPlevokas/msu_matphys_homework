using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FirstSemesterLib;
using Microsoft.Win32;
using System.Threading;
using Grid = FirstSemesterLib.Grid2D;

namespace SecondSemesterLab_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static RoutedCommand AddCustomCollectionCommand = new RoutedCommand("AddCustomCollection", typeof(MainWindow));
        private V4MainCollection MainColl;

        public MainWindow()
        {
            InitializeComponent();
            MainColl = new V4MainCollection();
            GetCollectionBuilder().SetMainCollection(MainColl);
            DataContext = MainColl;
        }
        private void FilterDataOnGrid(object c, FilterEventArgs e) => e.Accepted = e.Item is V4DataOnGrid;
        private void FilterDataCollection(object c, FilterEventArgs e) => e.Accepted = e.Item is V4DataCollection;
        
        private bool Save()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.InitialDirectory = Directory.GetCurrentDirectory();
            dialog.RestoreDirectory = true;
            dialog.Filter = "dat files (*.dat)|*.dat|All files (*.*)|*.*";
            if (dialog.ShowDialog() == true)
            {
                MainColl.Save(dialog.FileName);
                return true;
            }
            return false;
        }

        private Second_Lab_class GetCollectionBuilder()
        {
            return Resources["CollectionBuilder"] as Second_Lab_class;
        }

        private bool Open()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = Directory.GetCurrentDirectory();
            dialog.RestoreDirectory = true;
            dialog.Filter = "dat files (*.dat)|*.dat|All files (*.*)|*.*";
            if (dialog.ShowDialog() == true)
            {

                V4MainCollection coll = new V4MainCollection();
                coll.Load(dialog.FileName);
                MainColl = coll;
                DataContext = MainColl;
                GetCollectionBuilder().SetMainCollection(MainColl);
                //SetMainCollection(coll);
                return true;
            }
            return false;
        }

        private bool SuggestSave()
        {
            if (!MainColl.IfChangedCollection)
                return true;

            string caption = "Несохранённые изменения";
            string message = "Данные были изменены. Вы хотите сохранить их?";
            MessageBoxButton buttons = MessageBoxButton.YesNoCancel;
            MessageBoxResult res = MessageBox.Show(message, caption, buttons);
            if (res == MessageBoxResult.No)
                return true;
            if (res == MessageBoxResult.Cancel)
                return false;
            if (res == MessageBoxResult.Yes)
                try
                {
                    return Save();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }
            return false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("EN-US");
            MainColl = new V4MainCollection();
            DataContext = MainColl;
            GetCollectionBuilder().SetMainCollection(MainColl);
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (!SuggestSave())
                e.Cancel = true;
        }
        private void Item_New_Click(object sender, RoutedEventArgs e)
        {
            if (SuggestSave())
                MainColl = new V4MainCollection();
            DataContext = MainColl;
        }
        private void Item_Open_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SuggestSave())
                    Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Item_SaveAs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void Item_Defaults_Click(object sender, RoutedEventArgs e)
        {
            if (MainColl == null)
                MessageBox.Show("Нет объекта");
            else
            {
                try { 
                    MainColl.AddDefaults();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Add Defaults problem \n" + ex.Message);
                }

            }
        }

        private void Item_DefaultColl_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
            var rand = new Random();
            V4DataCollection coll = new V4DataCollection("def_col", 12.3);
            coll.InitRandom(4, (float)2.0, (float)2.0, 1.0, 1.0);
            MainColl.Add(coll);
            }
            catch (Exception ex)
                {
                MessageBox.Show("Item_DefaultColl_Click \n" + ex.Message);
            }
        }

        private void Item_DefaultGrid_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            var rand = new Random();
            V4DataOnGrid coll = new V4DataOnGrid("default DataOnGrid", 12.3, new Grid2D((float)rand.NextDouble() * 5, 4, (float)rand.NextDouble() * 5, 4));
            coll.InitRandom(0, 1);
            MainColl.Add(coll);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Item_DefaultGrid_Click \n" + ex.Message);
            }
        }

        private void Item_AddFromFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = Directory.GetCurrentDirectory();
            dialog.RestoreDirectory = true;
            dialog.Filter = "dat files (*.dat)|*.dat|All files (*.*)|*.*";
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    V4DataCollection coll = new V4DataCollection(dialog.FileName);
                    MainColl.Add(coll);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Item_Remove_Click(object sender, RoutedEventArgs e)
        {
            if (listBox_Main.SelectedItem != null)
            {
                V4Data data = (V4Data)listBox_Main.SelectedItem;
                MainColl.Remove(data.MInfo, data.FInfo);
            }
        }



        private void OpenCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                if (SuggestSave())
                    Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void CanSaveCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = MainColl != null && MainColl.IfChangedCollection;
        }

        private void SaveCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CanDeleteCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = listBox_Main?.SelectedItem != null;
        }
        private void DeleteCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (listBox_Main.SelectedItem != null)
            {
                V4Data data = (V4Data)listBox_Main.SelectedItem;
                MainColl.Remove(data.MInfo, data.FInfo);
            }
        }


        private void CanAddCustomCollectionCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = GetCollectionBuilder() != null && GetCollectionBuilder().Error == null;
        }

        private void AddCustomCollectionCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            GetCollectionBuilder().Adding();
        }


    }
}
