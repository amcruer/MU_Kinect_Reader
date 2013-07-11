#region using statements
using Kinect_Reader;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
#endregion

namespace Kinect_Reader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Initialization
        
        /// <summary>
        /// Bitmap that will hold color information
        /// </summary>
        public WriteableBitmap colorBitmap;
                
        /// <summary>
        /// Intermediate storage for the color data received from the camera
        /// </summary>
        public byte[] colorPixels;               
                
        ///<summary>
        ///Initialize Kinect sensor
        /// </summary>
        public KinectSensor sensor = KinectSensor.KinectSensors.Where(s => s.Status == KinectStatus.Connected).FirstOrDefault();

        /// <summary>
        /// Drawing Group for skeleton rendering output
        /// </summary>
        public DrawingGroup drawingGroup;

        /// <summary>
        /// Drawing image that we will display
        /// </summary>
        public DrawingImage imageSource;
        
        ///<summary>
        ///initializes a new instance of the Tracker class.
        /// </summary>
        private Tracker tracker;

        /// <summary>
        /// boolean to check if file was created
        /// </summary>
        public bool fileCheck = false;

        /// <summary>
        /// for the CSV output file path
        /// </summary>
        public string inputFilePath = null;

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }
        #endregion
        
        #region Button Events

        /// <summary>
        /// logic for Start buton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            
            // Configure save file dialog box
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Default"; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text file (.txt)|*.txt"; // Filter files by extension
            
            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            inputFilePath = dlg.FileName;
            buttonStart.IsEnabled = false;
            buttonPause.IsEnabled = true;
            buttonResume.IsEnabled = false;
            fileCheck = true;

            //Create drawing group
            this.drawingGroup = new DrawingGroup();

            tracker = new Tracker(sensor, this, dlg.FileName);

            //Create image source for WPF control
            this.imageSource = new DrawingImage(this.drawingGroup);
            colorBitmap = new WriteableBitmap(640, 480, 96, 96, PixelFormats.Bgr32, null);
            colorPixels = new byte[sensor.ColorStream.FramePixelDataLength];           
        }

        /// <summary>
        /// logic for Exit button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit?", "Exiting", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                sensor.Stop();
                if (fileCheck == true)
                {
                    tracker.outputFile.closeFiles();

                    if (chkSQLite.IsChecked == true)
                    {
                        if (txtTableBox.Text == "")
                        {
                            MessageBox.Show("Please enter a table name.");
                        }
                        else
                        {
                            string tableName = txtTableBox.Text;
                                                        
                            DatabaseWriter dataWriter = new DatabaseWriter(tableName, inputFilePath);                            
                        }
                    }
                }

                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Logic to launch AboutBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAbout_Click(object sender, RoutedEventArgs e)
        {
            Window AboutBox = new AboutBox();
            AboutBox.Show();
        }

        /// <summary>
        /// pauses file output and fills text blocks with dashes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonPause_Click(object sender, RoutedEventArgs e)
        {
            buttonPause.IsEnabled = false;
            buttonResume.IsEnabled = true;
        }

        /// <summary>
        /// resumes output and file writing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonResume_Click(object sender, RoutedEventArgs e)
        {
            buttonResume.IsEnabled = false;
            buttonPause.IsEnabled = true;
        }

        /// <summary>
        /// stops "old" file and graph streams, closes file, launches dialog for new subject and starts
        /// stream up again
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonNextSubject_Click(object sender, RoutedEventArgs e)
        {
            //Ask the user if they're sure they want to stop the current stream
            if (MessageBox.Show("Are you sure you want to stop the stream and start a new one?", "New Subject", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                if (fileCheck == false)
                {
                    return;
                }
            }

            tracker.StopTracker();

            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Subject"; //Default file name
            dlg.DefaultExt = ".txt"; //Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; //Filter files by extension

            //Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();
            buttonStart.IsEnabled = false;
            buttonPause.IsEnabled = true;
            buttonResume.IsEnabled = false;
            fileCheck = true;

            tracker.RestartTracker(dlg.FileName);
        }
        #endregion
               
        private void image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
        }

        private void chkSQLite_Checked(object sender, RoutedEventArgs e)
        {
            txtTableBox.IsEnabled = true;
        }  
    }
}