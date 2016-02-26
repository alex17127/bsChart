using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Windows;
using System.Windows.Media;


namespace bsChart
{
    public enum DrawModes
    {
        point,
        line
    };

    public class Series
    {
        public string Name;
        public bool isTimeSeries = true;
        public int RenderBlockSize = 500;
        public Brush brush = Brushes.Red;
        public Pen pen = new Pen(Brushes.DarkBlue, 3);

        public Range xRange = new Range();
        public Range yRange = new Range();

        object sync = new object();
        //ConcurrentBag<IDataObject> series = new ConcurrentBag<IDataObject>();
        List<IDataObject> series = new List<IDataObject>();

        //Action<DrawingContext, Series, ScaleXY> Draw_func;
        Action<DrawingContext, ScaleXY> DrawPoint;
        Action<DrawingContext, ScaleXY> DrawLine;

        public DrawModes mode;

        public Series(DrawModes mode)
        {
            this.mode = mode;
            switch (mode)
            {
                case DrawModes.point:
                    DrawPoint = (dc, scale) => drawPoint(dc, scale);
                    break;
                case DrawModes.line:
                    DrawLine = (dc, scale) => drawLine(dc,scale);
                    break;
            }
        }

        public void Clear()
        {
            lock(sync)
                series.Clear();
            xRange = new Range();
            yRange = new Range();
            firstBlockIndex = 0;
        }

        public void Add(IDataObject obj)
        {
            lock(sync)
                series.Add(obj);
            xRange.Update(obj.xRange());
            yRange.Update(obj.yRange());
        }

        public void Draw(DrawingContext dc,ScaleXY scale)
        {
            switch (mode)
            {
                case DrawModes.point:
                    if (DrawPoint == null) return;
                    foreach (var o in series)
                    drawPoint(dc , scale);
                    break;
                case DrawModes.line:
                    if (series?.Count < 2) return;
                    DrawLine(dc , scale);
                    break;
            }                             
        }

        void drawPoint(DrawingContext dc, ScaleXY scale)
        {
            int Mark_sz = 3;
            foreach (var point in series)
            {
                PointXY p = (PointXY)point;
                Rect rect = new Rect(scale.TransferX(p.x) - Mark_sz, 
                                     scale.TransferY(p.y) - Mark_sz,
                                     Mark_sz * 2, Mark_sz * 2);
                dc.DrawRectangle(brush, pen, rect);
            }
        }

        void drawLineXY(DrawingContext dc, ScaleXY scale)
        {
            Point p0, p1;
            int sz = series.Count;
            if (sz < 2) return;
            int k = 1;
            while (k < sz)
            {
                k--;
                p0 = ((PointXY)series.ElementAt(k++)).point(scale);
                p1 = ((PointXY)series.ElementAt(k++)).point(scale);
                dc.DrawLine(pen, p0, p1);
            }
        }


        int firstBlockIndex = 0;
        void drawLine(DrawingContext dc, ScaleXY scale)
        {
            Point p0, p1;
            int sz = series.Count;
            if (sz < 2) return;
            int k = 1;
            while (k < sz)
            {
                k--;
                bool isZoomed = false;
                
                p0 = ((PointXY)series.ElementAt(k++)).point(scale);
                p1 = ((PointXY)series.ElementAt(k++)).point(scale);

                double yMin = p1.Y;
                double yMax = p1.Y;
                
                while (isTimeSeries && Math.Abs(p1.X-p0.X)<2 && k<sz)
                {
                    p1 = ((PointXY)series.ElementAt(k++)).point(scale);
                    if (p1.Y < yMin) yMin = p1.Y;
                    if (p1.Y > yMax) yMax = p1.Y;
                    isZoomed = true;
                    if (k == sz) break;
                }
                    
                if(isZoomed)
                    dc.DrawLine(pen, new Point(p0.X,yMin), new Point(p0.X, yMax));
                else
                    dc.DrawLine(pen, p0, p1);

            }
        }

    }
}
