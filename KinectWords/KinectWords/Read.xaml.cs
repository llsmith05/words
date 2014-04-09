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

namespace KinectWords
{
    /// <summary>
    /// Interaction logic for Read.xaml
    /// </summary>
    public partial class Read : Window
    {
        WordObject cat = new WordObject("CAT", "C", "A", "T");
        public Read()
        {
            InitializeComponent();
        }

        public void show(string title)
        {
            this.Title = title;
            this.text1.FontSize = 72;
            this.text1.Text = cat.getText1();

            this.text2.FontSize = 72;
            this.text2.Text = cat.getText2();

            this.text3.FontSize = 72;
            this.text3.Text = cat.getText3();

            this.Show();

        }


        private void ButtonMenu_Click(object sender, RoutedEventArgs e)
        {
            close();
        }

        public void speech(string label)
        {
            if (label == cat.getWord())
            {
                this.statusText.Text = "Cat was spoken";
            }
            else
            {
                this.statusText.Text = label;
            }
        }

        public void close()
        {
            this.Close();
        }

    }
}
