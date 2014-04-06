
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
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
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;

namespace KinectWords
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //active sensor
        private KinectSensor sensor;

        //Bitmap
        private WriteableBitmap colorBmp;

        //byte array for intermediate camera data storate
        private byte[] colorPixels;

        // Speech recognition engine using audio data from Kinect.
        private SpeechRecognitionEngine speechEngine;

        // List of all UI span elements used to select recognized text.
        private List<Span> recognitionSpans;

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

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Gets the metadata for the speech recognizer (acoustic model) most suitable to
        /// process audio from Kinect device.
        /// </summary>
        /// <returns>
        /// RecognizerInfo if found, <code>null</code> otherwise.
        /// </returns>
        private static RecognizerInfo GetKinectRecognizer()
        {
            foreach (RecognizerInfo recognizer in SpeechRecognitionEngine.InstalledRecognizers())
            {
                string value;
                recognizer.AdditionalInfo.TryGetValue("Kinect", out value);
                if ("True".Equals(value, StringComparison.OrdinalIgnoreCase) && "en-US".Equals(recognizer.Culture.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return recognizer;
                }
            }

            return null;
        }

    }
}
