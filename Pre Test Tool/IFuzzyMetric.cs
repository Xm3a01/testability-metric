using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pre_Test_Tool
{
    class IFuzzyMetric
    {
        public static double fuzzication (double coh , double cop , double dep)
        {
            double testEfort= 0.0;
            if ((coh >= 0.0 && coh <= 0.2) && (cop >= 0.0 && cop <= 0.2) || (dep >= 0.0 && dep <= 0.2))
            {
                testEfort= Level.vLow().Max();
            }

            else if ((coh >= 0.1 && coh <= 0.4) && (cop >= 0.1 && cop <= 0.4) || (dep >= 0.1 && dep <= 0.4))
            {
                testEfort= Level.low().Max();
            }

            else if ((coh >= 0.3 && coh <= 0.7) && (cop >= 0.3 && cop <= 0.7) || (dep >= 0.3 && dep <= 0.7))
            {
                testEfort= Level.med().Max();
            }

            else if ((coh >= 0.6 && coh <= 0.9) && (cop >= 0.6 && cop <= 0.9) || (dep >= 0.6 && dep <= 0.9))
            {
                testEfort= Level.high().Max();
            }

            if ((coh >= 0.8 && coh <= 1) && (cop >= 0.8 && cop <= 1) || (dep >= 0.8 && dep <= 1))
            {
                testEfort= Level.vHigh().Max();
            }

            return testEfort;
        }
    }
}
