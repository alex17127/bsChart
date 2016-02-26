using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace bsChart
{
    public class Range
    {
        public double max = double.MinValue;
        public double min = double.MaxValue;

        public Range() { }

        public Range(double value)
        {
            Update(value);
        }

        public void Update(double value)
        {
            max = value > max ? value : max;
            min = value < min ? value : min;
        }

        public void Update(Range range)
        {
            max = range.max > max ? range.max : max;
            min = range.min < min ? range.min : min;
        }

        //public double? Size_null()
        //{
        //    if (max == double.MaxValue || min == double.MinValue) return null;
        //    return max - min;
        //}

        public double Size()
        {
            if (max == double.MinValue || min == double.MaxValue) return 0;
            return max - min;
        }

        public double[] Grid()
        {
            if (max == double.MinValue || min == double.MaxValue) return null;

            double diff = max - min;
            if (diff < 1e-100) return null;

            double v = diff;
            int p = 0;
            while (!(v > 0 && v < 1))
            {
                p++;
                v = (diff < 1) ? v * 10 : v / 10;
            }

            double step = 0.1;
            step = diff < 1 ? step / Math.Pow(10, p) : step * Math.Pow(10, p);

            List<double> gr = new List<double>();
            double g_t = Math.Sign(min) * (int)(Math.Abs(min) / step) * step;

            while (g_t < max)
            {
                if (g_t >= min && g_t <= max)
                    gr.Add(g_t);

                g_t = Math.Round(g_t + step, p + 1);
            }

            return gr.ToArray();
        }


        public double[] Grid(int lines_number)
        {
            if (lines_number < 1) return null;
            if (max == double.MinValue || min == double.MaxValue) return null;

            double diff = max - min;
            if (diff < 1e-100) return null;

            double v = diff;
            int p = 0;
            while (!(v > 0 && v < 10))
            {
                p++;
                v = (diff < 1) ? v * 10 : v / 10;
            }

            double step = 0.1 * (Math.Round(v / lines_number / 0.1) + 1);
            step = diff < 1 ? step / Math.Pow(10, p) : step * Math.Pow(10, p);

            double[] g = new double[lines_number];
            double g0 = Math.Sign(min) * (int)(Math.Abs(min) / step) * step;

            for (int i = 0; i < lines_number; i++)
                g[i] = Math.Round(g0 + i * step, p + 1);

            return g;
        }

    }

}
