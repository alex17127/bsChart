using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace bsChart
{
    public class PointXY : IDataObject
    {
        public double x;
        public double y;

        public PointXY(double x,double y)
        {
            
            this.x = x;
            this.y = y;
        }

        public Range xRange()
        {
            return new Range(x);
        }

        public Range yRange()
        {
            return new Range(y);
        }

        public Point point(ScaleXY scale)
        {
            return new Point(scale.TransferX(x),scale.TransferY(y));
        }
    }
}
