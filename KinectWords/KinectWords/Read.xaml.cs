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
        List<WordObject> Words = new List<WordObject>();
        int count;
        public Read()
        {
            InitializeComponent();
            WordObject cat = new WordObject("CAT", "C", "A", "T");
            WordObject sad = new WordObject("SAD", "S", "A", "D");
            WordObject bed = new WordObject("BED", "B", "E", "D");
            this.Words.Add(cat);
            this.Words.Add(sad);
            this.Words.Add(bed);
            this.count = 0;
        }

        public void show(string title)
        {
            this.Title = title;
            show();

        }

        public void show()
        {
            this.text1.FontSize = 72;
            this.text1.Text = Words[count].getText1();

            this.text2.FontSize = 72;
            this.text2.Text = Words[count].getText2();

            this.text3.FontSize = 72;
            this.text3.Text = Words[count].getText3();

            this.Show();

        }


        private void ButtonMenu_Click(object sender, RoutedEventArgs e)
        {
            close();
        }

        private void ButtonMenu_Next(object sender, RoutedEventArgs e)
        {
            nextWord();
        }

        public void nextWord()
        {
            count = count + 1;
            if (count < Words.Count)
            {
                 //this.statusText.Text = count.ToString() + " " + Words.Count.ToString();
                if (count + 1 == Words.Count)
                {
                    buttonMenu_Next.Visibility = Visibility.Hidden;
                } 
                show();
            }
            else
            {
                this.statusText.Text = "No more Words";
            }
            
        }

        public void speech(string label)
        {
            if (label == Words[count].getWord())
            {
                this.statusText.Text = "Good Job";
            }
            else
            {
                this.statusText.Text = "Try Again";
            }
        }

        public void close()
        {
            this.Close();
        }



    }
}
