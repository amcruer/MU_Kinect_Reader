using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using System.IO;
using System.Windows;
using System.Windows.Media;
using Kinect_Reader;

namespace Kinect_Reader
{
    class Tracker
    {
        private Skeleton[] skeletons = null;
        private MainWindow window;
        private KinectSensor sensor;
        public CSVWrite outputFile;
        

        public Tracker(KinectSensor sn, MainWindow win, string fileName)
        {
            window = win;
            outputFile = new CSVWrite(fileName);
            
            sensor = sn;
            try
            {
                sensor.Start();
            }

            catch (IOException)
            {
                sensor = null;
                MessageBox.Show("No Kinect sensor found. Please connect one and restart the application.", "*****ERROR*****");
                return;
            }

            sensor.SkeletonFrameReady += SensorSkeletonFrameReady;
            sensor.ColorFrameReady += SensorColorFrameReady;
            sensor.SkeletonStream.Enable();
            sensor.ColorStream.Enable();
        }

        public void StopTracker()
        {
            sensor.SkeletonStream.Disable();
            sensor.ColorStream.Disable();
            outputFile.closeFiles();
            
        }

        public void RestartTracker(string newFileName)
        {
            outputFile = new CSVWrite(newFileName);
            sensor.SkeletonStream.Enable();
            sensor.ColorStream.Enable();
        }

        private void SensorSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    if (this.skeletons == null)
                    {
                        this.skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    }

                    skeletonFrame.CopySkeletonDataTo(this.skeletons);

                    Skeleton skeleton = this.skeletons.Where(s => s.TrackingState == SkeletonTrackingState.Tracked).FirstOrDefault();
                    if (skeleton != null)
                    {
                        Joint leftFoot = skeleton.Joints[JointType.FootLeft];
                        Joint rightFoot = skeleton.Joints[JointType.FootRight];
                        Joint leftAnkle = skeleton.Joints[JointType.AnkleLeft];
                        Joint rightAnkle = skeleton.Joints[JointType.AnkleRight];
                        Joint leftKnee = skeleton.Joints[JointType.KneeLeft];
                        Joint rightKnee = skeleton.Joints[JointType.KneeRight];
                        Joint leftHip = skeleton.Joints[JointType.HipLeft];
                        Joint rightHip = skeleton.Joints[JointType.HipRight];
                        Joint centerHip = skeleton.Joints[JointType.HipCenter];


                        if (window.buttonStart.IsEnabled == false && window.buttonPause.IsEnabled == true)
                        {
                            outputFile.PrintSkeleton(skeleton, skeletonFrame.Timestamp);
                            //dataFile
                        }

                        Calculations.kneeAnkleRatio(leftKnee, rightKnee, leftAnkle, rightAnkle, window);

                        //make call to Visual Output here
                        VisualOutput.skeletonNotNull(window, skeleton, skeletonFrame);

                    }

                    else if (skeleton == null)
                    {
                        //make call to Visual Output here as well
                        VisualOutput.skeletonNull(window);
                    }

                    
                }
            }
        }

        private void SensorColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
            {
                if (colorFrame != null)
                {
                    //Copy the pixel data from the image to a temporary array
                    colorFrame.CopyPixelDataTo(window.colorPixels);

                    //write the pixel data into our bitmap
                    window.colorBitmap.WritePixels(new Int32Rect(0, 0, window.colorBitmap.PixelWidth, window.colorBitmap.PixelHeight),
                        window.colorPixels,
                        window.colorBitmap.PixelWidth * sizeof(int),
                        0);
                }
            }
        }
    }
}
