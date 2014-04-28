
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
using System.Drawing;
using WindowsInput;
using Kinect.Toolbox;

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

        private Read newwin = new Read();

        private SwipeGestureDetector gestureDetector;
                
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button2_4_Click(object sender, RoutedEventArgs e)
        {
            newRead("Age 4-6");
        }

        private void button5_6_Click(object sender, RoutedEventArgs e)
        {
            newRead("Age 5 - 6");
        }

        private void buttonRead_Click(object sender, RoutedEventArgs e)
        {
            newRead("Read");
        }

        private void newRead(string title)
        {
            newwin.show(title);
            this.Close();
        }


        private void buttonQuit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
            debugMsg.AppendText("Starting text!");

            this.gestureDetector = new SwipeGestureDetector();
            gestureDetector.MinimalPeriodBetweenGestures = 1000;

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

                //enable skeleton stream
                this.sensor.SkeletonStream.Enable();

                //allocate space in byte array for pixel data
                this.colorPixels = new byte[this.sensor.ColorStream.FramePixelDataLength];

                //create bitmap to be displayed
                this.colorBmp = new WriteableBitmap(this.sensor.ColorStream.FrameWidth, this.sensor.ColorStream.FrameHeight, 96, 96, PixelFormats.Bgr32, null);

                //Point image in xaml to the bitmap above
                this.kinect_Image.Source = this.colorBmp;

                //add event handler for incoming frames
                this.sensor.ColorFrameReady += sensor_ColorFrameReady;

                //event handles for 
                this.sensor.SkeletonFrameReady += this.SensorSkeletonFrameReady;
                this.gestureDetector.OnGestureDetected += on_GestureDetected;

                //add event handler for AllFramesReady
                //this.sensor.AllFramesReady += sensor_AllFramesReady;

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
                this.statusText.Text = "No Kinect Found! \r";
            }

            RecognizerInfo ri = GetKinectRecognizer();
            if (null != ri)
            {
                //recognitionSpans = new List<Span> { pictureSpan };

                this.speechEngine = new SpeechRecognitionEngine(ri.Id);

                // Create a grammar from grammar definition XML file.
                using (var memoryStream = new MemoryStream(Encoding.ASCII.GetBytes(Properties.Resources.SpeechGrammar)))
                {
                    var g = new Grammar(memoryStream);
                    speechEngine.LoadGrammar(g);
                }

                speechEngine.SpeechRecognized += SpeechRecognized;
                speechEngine.SpeechRecognitionRejected += SpeechRejected;

                // For long recognition sessions (a few hours or more), it may be beneficial to turn off adaptation of the acoustic model. 
                // This will prevent recognition accuracy from degrading over time.
                ////speechEngine.UpdateRecognizerSetting("AdaptationOn", 0);

                speechEngine.SetInputToAudioStream(
                    sensor.AudioSource.Start(), new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                speechEngine.RecognizeAsync(RecognizeMode.Multiple);
            }
            else
            {
                this.statusText.Text = "No Speech Engine Found \r";
            }
        }

         private void on_GestureDetected(string gest)
         {
             if (gest == "SwipeToRight")
             {
                 debugMsg.AppendText("Right swipe");
             }

             if (gest == "SwipeToLeft")
             {
                 debugMsg.AppendText("Left swipe");
             }
         }

         private void SensorSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
         {
             using (SkeletonFrame skeletonFrameData = e.OpenSkeletonFrame())
             {
                 if (skeletonFrameData == null)
                 {
                     debugMsg.AppendText("No skeleton!");
                     return;
                 }

                 var allSkeletons = new Skeleton[skeletonFrameData.SkeletonArrayLength];
                 skeletonFrameData.CopySkeletonDataTo(allSkeletons);

                 //loop through skeleton frames
                 foreach (Skeleton sd in allSkeletons)
                 {
                     if (sd.TrackingState == SkeletonTrackingState.Tracked)
                     {
                         gestureDetector.Add(sd.Joints[JointType.HandRight].Position, sensor);
                     }
                     else
                     {
                         continue;
                     }
                 }
             }
         }

         /// <summary>
         /// Handler for recognized speech events.
         /// </summary>
         /// <param name="sender">object sending the event.</param>
         /// <param name="e">event arguments.</param>
         private void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
         {
             // Speech utterance confidence below which we treat speech as if it hadn't been heard
             const double ConfidenceThreshold = 0.3;

             ClearRecognitionHighlights();

             if (e.Result.Confidence >= ConfidenceThreshold)
             {
                 switch (e.Result.Semantics.Value.ToString())
                 {

                     case "AGE2_4":
                         newRead("Age 2 - 4");
                         break;
                     case "AGE5_6":
                         newRead("Age 5 - 6");
                         break;
                     case "READ":
                         newRead("Read");
                         break;
                     case "NEXT":
                         newwin.nextWord();
                         break;
                     case "DONE":
                     case "QUIT":
                         if (null != newwin)
                         {
                             newwin.close();
                         }
                         this.Close();
                         break;
                     case "CAT":
                         if (null != newwin)
                         {
                             newwin.speech("CAT");
                         }
                         break;
                 }
             }
         }

         /// <summary>
         /// Remove any highlighting from recognition instructions.
         /// </summary>
         private void ClearRecognitionHighlights()
         {
             statusText.Text = "not recognized word";
             //      foreach (Span span in recognitionSpans)
             //      {
             //           span.Foreground = (Brush)this.Resources[MediumGreyBrushKey];
             //           span.FontWeight = FontWeights.Normal;
             //      }
             if (null != newwin)
             {
                 newwin.speech("not recognized");
             }
         }

         /// <summary>
         /// Handler for rejected speech events.
         /// </summary>
         /// <param name="sender">object sending the event.</param>
         /// <param name="e">event arguments.</param>
         private void SpeechRejected(object sender, SpeechRecognitionRejectedEventArgs e)
         {
             ClearRecognitionHighlights();
         }
         //Event handler for colorframe
         void sensor_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
         {
             using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
             {
                 if (colorFrame != null)
                 {
                     //copy from frame to temp array
                     colorFrame.CopyPixelDataTo(this.colorPixels);

                     //write pixel data to bitmap
                     this.colorBmp.WritePixels(
                         new Int32Rect(0, 0, this.colorBmp.PixelWidth, this.colorBmp.PixelHeight),
                         this.colorPixels,
                         this.colorBmp.PixelWidth * sizeof(int),
                         0);
                 }
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
