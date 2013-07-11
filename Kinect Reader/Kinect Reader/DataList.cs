using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinect_Reader
{
    class DataList
    {
        public DataList(string[] values)
        {
            Console.WriteLine("(" + values[0] + ")" + values[1]);
            Console.WriteLine(values[0].GetType());
            //long.Parse(values[0])
            Frame = long.Parse(values[0]);
            HlState = int.Parse(values[1]);
            HLX = double.Parse(values[2]);
            HLY = double.Parse(values[3]);
            HLZ = double.Parse(values[4]);
            KlState = int.Parse(values[5]);
            KLX = double.Parse(values[6]);
            KLY = double.Parse(values[7]);
            KLZ = double.Parse(values[8]);
            AlState = int.Parse(values[9]);
            ALX = double.Parse(values[10]);
            ALY = double.Parse(values[11]);
            ALZ = double.Parse(values[12]);
            FlState = int.Parse(values[13]);
            FLX = double.Parse(values[14]);
            FLY = double.Parse(values[15]);
            FLZ = double.Parse(values[16]);
            HrState = int.Parse(values[17]);
            HRX = double.Parse(values[18]);
            HRY = double.Parse(values[19]);
            HRZ = double.Parse(values[20]);
            KrState = int.Parse(values[21]);
            KRX = double.Parse(values[22]);
            KRY = double.Parse(values[23]);
            KRZ = double.Parse(values[24]);
            ArState = int.Parse(values[25]);
            ARX = double.Parse(values[26]);
            ARY = double.Parse(values[27]);
            ARZ = double.Parse(values[28]);
            FrState = int.Parse(values[29]);
            FRX = double.Parse(values[30]);
            FRY = double.Parse(values[31]);
            FRZ = double.Parse(values[32]);
            LKFX = double.Parse(values[33]);
            LKVG = double.Parse(values[34]);
            RKFX = double.Parse(values[35]);
            RKVG = double.Parse(values[36]);
        }

        public long Frame { get; set; }
        public int HlState { get; set; }
        public double HLX { get; set; }
        public double HLY { get; set; }
        public double HLZ { get; set; }
        public int KlState { get; set; }
        public double KLX { get; set; }
        public double KLY { get; set; }
        public double KLZ { get; set; }
        public int AlState { get; set; }
        public double ALX { get; set; }
        public double ALY { get; set; }
        public double ALZ { get; set; }
        public int FlState { get; set; }
        public double FLX { get; set; }
        public double FLY { get; set; }
        public double FLZ { get; set; }
        public int HrState { get; set; }
        public double HRX { get; set; }
        public double HRY { get; set; }
        public double HRZ { get; set; }
        public int KrState { get; set; }
        public double KRX { get; set; }
        public double KRY { get; set; }
        public double KRZ { get; set; }
        public int ArState { get; set; }
        public double ARX { get; set; }
        public double ARY { get; set; }
        public double ARZ { get; set; }
        public int FrState { get; set; }
        public double FRX { get; set; }
        public double FRY { get; set; }
        public double FRZ { get; set; }
        public double LKFX { get; set; }
        public double LKVG { get; set; }
        public double RKFX { get; set; }
        public double RKVG { get; set; }
    }
}
