using System;
using System.Drawing;
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

namespace KinectWords
{
    /// <summary>
    /// Interaction logic for Age2_4.xaml
    /// </summary>
    public partial class Age2_4 : Window
    {
        public Age2_4()
        {
            InitializeComponent();
            Uri uri = new Uri("cat.gif", UriKind.Absolute);
            ImageSource imgSource = new BitmapImage(uri);
            topImg.Source = imgSource;

        }
    }
}
