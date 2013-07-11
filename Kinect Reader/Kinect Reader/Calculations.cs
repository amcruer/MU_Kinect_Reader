using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace Kinect_Reader
{
    class Calculations
    {
        public static double[] CalculateAngle(Skeleton skeleton, double ts)
        {
            double leftKneeFlexion = kneeFlexion(skeleton.Joints[JointType.HipLeft], skeleton.Joints[JointType.KneeLeft], skeleton.Joints[JointType.AnkleLeft]);
            double rightKneeFlexion = kneeFlexion(skeleton.Joints[JointType.HipRight], skeleton.Joints[JointType.KneeRight], skeleton.Joints[JointType.AnkleRight]);
            double leftKneeValgus = kneeValgus(skeleton.Joints[JointType.KneeLeft], skeleton.Joints[JointType.AnkleLeft]);
            double rightKneeValgus = kneeValgus(skeleton.Joints[JointType.KneeRight], skeleton.Joints[JointType.AnkleRight]);
            double[] returnValues = new double[5];
            returnValues[0] = leftKneeFlexion;
            returnValues[1] = leftKneeValgus * 2;
            returnValues[2] = rightKneeFlexion;
            returnValues[3] = -rightKneeValgus * 2;
            returnValues[4] = ts;
            return returnValues;
        }

        public static double kneeFlexion(Joint hipJoint, Joint kneeJoint, Joint ankleJoint)
        {
            float v1x = hipJoint.Position.X - kneeJoint.Position.X;         //Compute the vectors
            float v1y = hipJoint.Position.Z - kneeJoint.Position.Z;
            float v1z = hipJoint.Position.Y - kneeJoint.Position.Y;
            float v2x = ankleJoint.Position.X - kneeJoint.Position.X;         //Compute the vectors
            float v2y = ankleJoint.Position.Z - kneeJoint.Position.Z;
            float v2z = ankleJoint.Position.Y - kneeJoint.Position.Y;

            double v1m = Math.Sqrt(v1x * v1x + v1y * v1y + v1z * v1z);    //Vector mangitudes
            double v2m = Math.Sqrt(v2x * v2x + v2y * v2y + v2z * v2z);
            float dotp = v1x * v2x + v1y * v2y + v1z * v2z;         // Dot product of the vectors
            double cosa = dotp / (v1m * v2m);                      // Get  cos(theta)
            double angle = 180.0 * Math.Acos(cosa) / 3.14159;       //Get theta in degrees
            return angle;
        }

        public static double kneeValgus(Joint kneeJoint, Joint ankleJoint)
        {
            double v1x = ankleJoint.Position.X - kneeJoint.Position.X;         //Compute the vectors
            double v1z = ankleJoint.Position.Y - kneeJoint.Position.Y;
            double v2x = 0;
            double v2z = v1z;
            double v1m = Math.Sqrt(v1x * v1x + v1z * v1z);     //Vector magintudes
            double v2m = Math.Sqrt(v2x * v2x + v2z * v2z);
            double dotp = v1x * v2x + v1z * v2z;
            double cosa = dotp / (v1m * v2m);
            double angle = 180.0 * Math.Acos(cosa) / 3.14159;        //Get theta in degrees
            if (v1x < 0)
                angle = -angle;

            //System.Diagnostics.Debug.Print(angle+" " + cosa + " " + dotp + " " + v1x + " " + v1z + " " + v2z + " ");
            return angle;
        }

        public static void kneeAnkleRatio(Joint leftKnee, Joint rightKnee, Joint leftAnkle, Joint rightAnkle, MainWindow window)
        {

            double leftKneePos = leftKnee.Position.X;
            double rightKneePos = rightKnee.Position.X;
            double leftAnklePos = leftAnkle.Position.X;
            double rightAnklePos = rightAnkle.Position.X;

            double kneeDistance = Math.Abs(rightKneePos - leftKneePos);
            double ankleDistance = Math.Abs(rightAnklePos - leftAnklePos);

            //window.AnkleDistance.Text = Convert.ToString(ankleDistance);
            //window.KneeDistance.Text = Convert.ToString(kneeDistance);

        }
    }
}
