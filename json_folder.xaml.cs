using System;
using System.Collections.Generic;
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

            var dialog = new CommonOpenFileDialog() {
                EnsurePathExists = true,
                EnsureFileExists = false,
                AllowNonFileSystemItems = true,
                DefaultFileName = "Select Folder",
                Title = "Select The Folder To Process",
            };
            dialog.IsFolderPicker = false;
            CommonFileDialogResult result = dialog.ShowDialog();

            //Console.WriteLine(dialog.FileName);
            
            using (StreamReader r = new StreamReader(dialog.FileName))
            {
                //string path = r.ReadToEnd();

                using (StreamWriter s = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\json_location.txt"))
                {
                    s.Write("");
                    s.WriteLine(dialog.FileName);
                }
            }

        }
    }
}
