using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pre_Test_Tool
{
    class Level
    {
        private static List<double> v_low = new List<double>();
        private static List<double> Low = new List<double>();
        private static List<double> Med = new List<double>();
        private static List<double> High = new List<double>();
        private static List<double> v_high = new List<double>();


        public static List<double> vLow()
        {
            for (double i = 0.0; i <= 0.2; i += 0.01)
            {
                v_low.Add(i);
            }

            return v_low;
        }

        public static List<double> vHigh()
        {
            for (double i = 0.8; i <= 1; i += 0.01)
            {
                v_high.Add(i);
            }

            return v_high;
        }

        public static List<double> low()
        {
            for (double i = 0.1; i <= 0.4; i += 0.01)
            {
                Low.Add(i);
            }

            return Low;
        }


        public static List<double> med()
        {
            for (double i = 0.3; i <= 0.7; i += 0.01)
            {
                Med.Add(i);
            }

            return Med;
        }

        public static List<double> high()
        {
            for (double i = 0.6; i <= 0.9; i += 0.01)
            {
                High.Add(i);
            }

            return High;
        }
    }
}
