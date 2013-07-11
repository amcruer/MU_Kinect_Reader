using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace Kinect_Reader
{
    class CSVWrite
    {
        private static System.IO.StreamWriter file;

        public CSVWrite(string fileName)
        {
            file = new System.IO.StreamWriter(fileName);
            //file.Write("Time,HLState,HLX,HLY,HLZ,KLState,KLX,KLY,KLZ,ALState,ALX,ALY,ALZ,FLState,FLX,FLY,FLZ,HRState,HRX,HRY,HRZ,KRState,KRX,KRY,KRZ,ARState,ARX,ARY,ARZ,FRState,FRX,FRY,FRZ,lknfx,lknvg,rknfx,rknvg\r\n");
        }

        public void closeFiles()
        {
            file.Close();
            file.Dispose();
            file = null;
        }

        public void PrintSkeleton(Skeleton skeleton, double timeStamp)
        {
            if (file != null)
            {
                int i;
                file.Write(timeStamp);

                for (i = 12; i < 20; i++)
                {
                    if (skeleton.Joints[(JointType)i].TrackingState == JointTrackingState.NotTracked)
                    {
                        file.Write(",0,0,0,0");
                    }
                    else if (skeleton.Joints[(JointType)i].TrackingState == JointTrackingState.Tracked)
                    {
                        file.Write(",2," + skeleton.Joints[(JointType)i].Position.X + "," +
                                        skeleton.Joints[(JointType)i].Position.Y + "," +
                                        skeleton.Joints[(JointType)i].Position.Z);
                    }
                    else if (skeleton.Joints[(JointType)i].TrackingState == JointTrackingState.Inferred)
                    {
                        file.Write(",1," + skeleton.Joints[(JointType)i].Position.X + "," +
                                        skeleton.Joints[(JointType)i].Position.Y + "," +
                                        skeleton.Joints[(JointType)i].Position.Z);
                    }
                }

                double[] angles = Calculations.CalculateAngle(skeleton, timeStamp);
                file.Write("," + angles[0] + "," + angles[1] + "," + angles[2] + "," + angles[3]);
                file.WriteLine("\r");
            }
        }
    }
}
