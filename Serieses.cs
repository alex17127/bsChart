using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Windows.Media;
using System.Windows;


namespace bsChart
{
    public interface ISeries
    {
        void Draw(DrawingContext dc);
    }

    public class Serieses
    {
        ConcurrentBag<Series> serieses = new ConcurrentBag<Series>();

        public void Add(Series series)
        {
            serieses.Add(series);
        } 

        public void Draw(DrawingContext dc,ScaleXY scale)
        {
            foreach(var s in serieses)
            {
                if (s != null)
                {
                    s.Draw(dc,scale);
                }
                    
            }
        }

        public Range xRange()
        {
            if (serieses.Count == 0) return null;
            Range r = new Range();
            foreach(var s in serieses)
                r.Update(s.xRange);
            return r;
        }

        public Range yRange()
        {
            if (serieses.Count == 0) return null;
            Range r = new Range();
            foreach (var s in serieses)
                r.Update(s.yRange);
            return r;
        }
    }
}
