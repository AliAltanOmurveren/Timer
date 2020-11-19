using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;

namespace MyTimer
{
    /// <summary>
    /// Interaction logic for json_folder.xaml
    /// </summary>
    public partial class json_folder : Window
    {
        public json_folder()
        {
            InitializeComponent();


            

            tbox_glass_ml.Text = Properties.Settings.Default.Glass_ml.ToString();
            tbox_json_location.Text = Properties.Settings.Default.Durations_file;



            //CommonFileDialogResult result = dialog.ShowDialog();

            //Console.WriteLine(dialog.FileName);
            
            /*
            using (StreamReader r = new StreamReader(dialog.FileName))
            {
                //string path = r.ReadToEnd();

                using (StreamWriter s = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\json_location.txt"))
                {
                    s.Write("");
                    s.WriteLine(dialog.FileName);
                }
            }

            */

        }
        private void tbox_glass_ml_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

        }

        private void open_folder_btn_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = Color.FromRgb(74, 118, 82);
            open_folder_btn.Background = brush;
        }

        private void open_folder_btn_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = Color.FromRgb(128, 191, 140);
            open_folder_btn.Background = brush;

        }

        private void open_folder_btn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string filePath;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "JSON files (*.json)|*.json";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;
                DialogResult result = openFileDialog.ShowDialog();


                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                }
            }

            /*
            var dialog = new CommonOpenFileDialog()
            {
                EnsurePathExists = true,
                EnsureFileExists = false,
                AllowNonFileSystemItems = true,
                Title = "Select The Folder To Process",
                DefaultExtension = ".json"
            };
            dialog.IsFolderPicker = false;

            dialog.ShowDialog();
            */
        }

        private void save_btn_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Glass_ml = int.Parse(tbox_glass_ml.Text);
            Properties.Settings.Default.Durations_file = tbox_json_location.Text;
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void cancel_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
