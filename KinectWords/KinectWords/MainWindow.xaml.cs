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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KinectWords
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button2_4_Click(object sender, RoutedEventArgs e)
        {
            Age2_4 newwin = new Age2_4();
            newwin.Show();
        }

        private void button5_6_Click(object sender, RoutedEventArgs e)
        {
            Age5_6 newwin = new Age5_6();
            newwin.Show();
        }

        private void buttonRead_Click(object sender, RoutedEventArgs e)
        {
            Read newwin = new Read();
            newwin.Show();
        }
    }
}
