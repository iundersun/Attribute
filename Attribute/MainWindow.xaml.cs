using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;

namespace Attribute
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            RadioButtonEnabled(false);
        }

        bool[] attributes = new bool[6];
        //[0] - readonly
        //[1] - system
        //[2] - notContentIndexed
        //[3] - hiden
        //[4] - arhive
        //[5] - normal

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(path.Text))
            {
                try
                {
                    FileAttributes fa = new FileAttributes();
                    File.SetAttributes(path.Text, FileAttributes.Normal);
                    if (readOnly.IsChecked == true)
                        fa |= FileAttributes.ReadOnly;
                    if (system.IsChecked == true)
                        fa |= FileAttributes.System;
                    if (notContentIndexed.IsChecked == true)
                        fa |= FileAttributes.NotContentIndexed;
                    if (hidden.IsChecked == true)
                        fa |= FileAttributes.Hidden;
                    if (archive.IsChecked == true)
                        fa |= FileAttributes.Archive;
                    if (normal.IsChecked == true)
                        fa = FileAttributes.Normal;
                    File.SetAttributes(path.Text, fa);
                    MessageBox.Show("File attributes changed");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "File attributes doesn't changed");
                }
            }
            else
            {
                MessageBox.Show("File doesn't exists");
            }
            RadioButtonEnabled(false);
            CheckBoxEnabled(false);
        }

        private void Open_file_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "All Files|*.*";
            file.ShowDialog();
            path.Text = file.FileName;
            if (File.Exists(path.Text))
                GetFileAttributes();
        }

        private void GetFileAttributes()
        {
            FileAttributes fa = File.GetAttributes(path.Text);
            string buffer, attrib = fa.ToString();
            int startIndex;
            for (int i = 0; i < 6; i++)
                attributes[i] = false;
            while ((startIndex=attrib.LastIndexOf(',')) != -1)
            {
                buffer = attrib.Substring(startIndex+2,attrib.Length-startIndex-2);
                attrib = attrib.Remove(startIndex);
                switch (buffer)
                {
                    case "ReadOnly":
                        attributes[0] = true;
                        break;
                    case "System":
                        attributes[1] = true;
                        break;
                    case "NotContentIndexed":
                        attributes[2] = true;
                        break;
                    case "Hidden":
                        attributes[3] = true;
                        break;
                    case "Archive":
                        attributes[4] = true;
                        break;
                    default:
                        attributes[5] = true;
                        break;
                }
            }
            switch (attrib)
            {
                case "ReadOnly":
                    attributes[0] = true;
                    break;
                case "System":
                    attributes[1] = true;
                    break;
                case "NotContentIndexed":
                    attributes[2] = true;
                    break;
                case "Hidden":
                    attributes[3] = true;
                    break;
                case "Archive":
                    attributes[4] = true;
                    break;
                default:
                    attributes[5] = true;
                    break;
            }
            RadioButtonEnabled(true);
            manual.IsChecked = true;
            def.IsChecked = true;
        }

        private void path_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (path.Text.Length != 0)
                if (!File.Exists(path.Text))
                {
                    path.BorderBrush = Brushes.Red;
                    apply.IsEnabled = false;
                    RadioButtonEnabled(false);
                    CheckBoxEnabled(false);
                }
                else
                {
                    path.BorderBrush = Brushes.Green;
                    RadioButtonEnabled(true);
                    manual.IsChecked = true;
                    def.IsChecked = true;
                }
        }

        private void path_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Return" && File.Exists(path.Text))
                GetFileAttributes();
        }

        private void default_Checked(object sender, RoutedEventArgs e)
        {
            apply.IsEnabled = true;
            CheckBoxEnabled(true);
            readOnly.IsChecked = attributes[0];
            system.IsChecked = attributes[1];
            notContentIndexed.IsChecked = attributes[2];
            hidden.IsChecked = attributes[3];
            archive.IsChecked = attributes[4];
            normal.IsChecked = attributes[5];
            CheckBoxEnabled(false);
        }

        private void manual_Checked(object sender, RoutedEventArgs e)
        {
            CheckBoxEnabled(true);
        }

        private void clear_Checked(object sender, RoutedEventArgs e)
        {
            apply.IsEnabled = true;

            CheckBoxEnabled(true);
            readOnly.IsChecked = false;
            system.IsChecked = false;
            notContentIndexed.IsChecked = false;
            hidden.IsChecked = false;
            archive.IsChecked = false;
            normal.IsChecked = true;
            CheckBoxEnabled(false);
        }

        private void normal_Checked(object sender, RoutedEventArgs e)
        {
            clear.IsChecked = true;
        }

        private void RadioButtonEnabled(bool value)
        {
            def.IsEnabled = value;
            clear.IsEnabled = value;
            manual.IsEnabled = value;
        }

        private void CheckBoxEnabled(bool value)
        {
            normal.IsEnabled = value;
            archive.IsEnabled = value;
            notContentIndexed.IsEnabled = value;
            readOnly.IsEnabled = value;
            hidden.IsEnabled = value;
            system.IsEnabled = value;
        }
    }
}
