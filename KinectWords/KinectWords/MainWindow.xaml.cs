
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
using System.IO;

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
                //Start items on window load
         private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var potentialSensor in KinectSensor.KinectSensors)
            {
                if (potentialSensor.Status == KinectStatus.Connected)
                {
                    this.sensor = potentialSensor;
                    break;
                }
            }

            if (null != this.sensor)
            {
                //Turn on color stream
                this.sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);

                //allocate space in byte array for pixel data
                this.colorPixels = new byte[this.sensor.ColorStream.FramePixelDataLength];

                //create bitmap to be displayed
                this.colorBmp = new WriteableBitmap(this.sensor.ColorStream.FrameWidth, this.sensor.ColorStream.FrameHeight, 96, 96, PixelFormats.Bgr32, null);

                //Point image in xaml to the bitmap above
                //this.imgCanvas.Source = this.colorBmp;

                //add event handler for incoming frames
                //this.sensor.ColorFrameReady += sensor_ColorFrameReady;

                //start the sensor
                try
                {
                    this.statusText.Text = "Hello";
                    this.sensor.Start();
                }
                catch (IOException)
                {
                    this.sensor = null;
                }
            }

            if (null == this.sensor)
            {
                this.statusText.Text = "No Kinect Found!";
            }

            RecognizerInfo ri = GetKinectRecognizer();
            if (null != ri)
            {
                //recognitionSpans = new List<Span> { pictureSpan };

                this.speechEngine = new SpeechRecognitionEngine(ri.Id);

            }
            else
            {
                this.statusText.Text = "No Speech Engine Found";
            }
        }

 
  
         void StopKinect(KinectSensor sensor)
         {
             if (sensor != null)
             {
                 sensor.Stop();
                 sensor.AudioSource.Stop();
             }
         }

        //Shutdown sensor
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.sensor.Stop();
        }
    }
}
