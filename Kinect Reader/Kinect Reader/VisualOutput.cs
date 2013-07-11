using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Kinect;
using Kinect_Reader;


namespace Kinect_Reader
{
    class VisualOutput
    {
        #region Initialization
        /// <summary>
        /// Width of output drawing
        /// </summary>
        private const float RenderWidth = 640.0f;

        /// <summary>
        /// Height of output drawing
        /// </summary>
        private const float RenderHeight = 480.0f;
        
        /// <summary>
        /// Thickness of drawn joint lines
        /// </summary>
        private const double JointThickness = 3;

        /// <summary>
        /// Thickness of body center ellipse
        /// </summary>
        private const double BodyCenterThickness = 10;

        /// <summary>
        /// Brush used to draw skeleton center point
        /// </summary>
        private static readonly Brush centerPointBrush = Brushes.Blue;

        /// <summary>
        /// Brush used for drawing joints that are currently inferred.
        /// </summary>
        private static readonly Brush inferredJointBrush = Brushes.Yellow;

        /// <summary>
        /// Brush used for drawing joints that are currently tracked.
        /// </summary>
        private static readonly Brush trackedJointBrush = new SolidColorBrush(Color.FromArgb(255, 68, 192, 68));

        /// <summary>
        /// Pen used for drawing bones that are currently tracked.
        /// </summary>
        private static readonly Pen trackedBonePen = new Pen(Brushes.Green, 6);

        /// <summary>
        /// Pen used for drawing bones that are currently inferred
        /// </summary>
        private static readonly Pen inferredBonePen = new Pen(Brushes.Gray, 1);

        #endregion 
       
        #region Methods        
        /// <summary>
        /// Draws a skeleton's bones and joints
        /// </summary>
        /// <param name="skeleton">skeleton to draw</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        /// <param name="timestamp"></param>
        /// <param name="window"></param>
        public static void DrawBonesAndJoints(Skeleton skeleton, DrawingContext drawingContext, long timestamp, MainWindow window)
        {
            drawingContext.DrawImage(window.colorBitmap, new Rect(new Point(0, 0), new Point(window.colorBitmap.PixelWidth, window.colorBitmap.PixelHeight)));
            int i = 12;

            //render bones
            DrawBone(skeleton, drawingContext, JointType.Spine, JointType.HipCenter, window);
            DrawBone(skeleton, drawingContext, JointType.HipCenter, JointType.HipLeft, window);
            DrawBone(skeleton, drawingContext, JointType.HipCenter, JointType.HipRight, window);
            DrawBone(skeleton, drawingContext, JointType.HipLeft, JointType.KneeLeft, window);
            DrawBone(skeleton, drawingContext, JointType.KneeLeft, JointType.AnkleLeft, window);
            DrawBone(skeleton, drawingContext, JointType.AnkleLeft, JointType.FootLeft, window);
            DrawBone(skeleton, drawingContext, JointType.HipRight, JointType.KneeRight, window);
            DrawBone(skeleton, drawingContext, JointType.KneeRight, JointType.AnkleRight, window);
            DrawBone(skeleton, drawingContext, JointType.AnkleRight, JointType.FootRight, window);

            //render joints
            for (i = 12; i < 20; i++)
            {
                Joint j = skeleton.Joints[(JointType)i];
                Brush drawBrush = null;
                if (j.TrackingState == JointTrackingState.Tracked)
                {
                    drawBrush = trackedJointBrush;
                }
                else if (j.TrackingState == JointTrackingState.Inferred)
                {
                    drawBrush = inferredJointBrush;
                }
                if (drawBrush != null)
                {
                    drawingContext.DrawEllipse(drawBrush, null, SkeletonPointToScreen(j.Position, window), JointThickness, JointThickness);
                }
            }
            
        }

        public static Point SkeletonPointToScreen(SkeletonPoint skelPoint, MainWindow window)
        {
            ColorImagePoint depthPoint = window.sensor.MapSkeletonPointToColor(skelPoint, ColorImageFormat.RgbResolution640x480Fps30);
            return new Point(depthPoint.X, depthPoint.Y);
        }

        private static void DrawBone(Skeleton skeleton, DrawingContext dc, JointType jt0, JointType jt1, MainWindow window)
        {
            Joint j0 = skeleton.Joints[jt0];
            Joint j1 = skeleton.Joints[jt1];

            //if we can't find either of these joints, exit
            if (j0.TrackingState == JointTrackingState.NotTracked || j1.TrackingState == JointTrackingState.NotTracked)
            {
                return;
            }

            //Don't draw if both points are inferred
            if (j0.TrackingState == JointTrackingState.Inferred && j1.TrackingState == JointTrackingState.Inferred)
            {
                return;
            }

            //We assume all drawn bones are inferred unles BOTH joints are tracked
            Pen drawPen = inferredBonePen;

            if (j0.TrackingState == JointTrackingState.Tracked && j1.TrackingState == JointTrackingState.Tracked)
            {
                drawPen = trackedBonePen;
                dc.DrawLine(drawPen, SkeletonPointToScreen(j0.Position, window), SkeletonPointToScreen(j1.Position, window));
            }
        }

        public static void skeletonNotNull(MainWindow window, Skeleton skeleton, SkeletonFrame skeletonFrame)
        {
            using (DrawingContext dc = window.drawingGroup.Open())
            {
                //Draw a transparent background to set the render size
                dc.DrawRectangle(Brushes.Black, null, new Rect(0.0, 0.0, RenderWidth, RenderHeight));
                if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                {
                    DrawBonesAndJoints(skeleton, dc, skeletonFrame.Timestamp, window);
                }

                else if (skeleton.TrackingState == SkeletonTrackingState.PositionOnly)
                {
                    dc.DrawEllipse(centerPointBrush, null, SkeletonPointToScreen(skeleton.Position, window), BodyCenterThickness, BodyCenterThickness);
                }

                //prevent drawing outside of the render area
                window.drawingGroup.ClipGeometry = new RectangleGeometry(new Rect(0.0, 0.0, RenderWidth, RenderHeight));
                window.image.Source = window.imageSource;
            }
        }

        public static void skeletonNull(MainWindow window)
        {
            using (DrawingContext dc = window.drawingGroup.Open())
            {
                dc.DrawImage(window.colorBitmap, new Rect(new Point(0, 0), new Point(window.colorBitmap.PixelWidth, window.colorBitmap.PixelHeight)));
            }

            //prevent drawing outside of the render area
            window.drawingGroup.ClipGeometry = new RectangleGeometry(new Rect(0.0, 0.0, RenderWidth, RenderHeight));
            window.image.Source = window.imageSource;
        }

        #endregion
    }
}
