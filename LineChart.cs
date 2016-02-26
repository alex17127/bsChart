using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace bsChart
{
    public class LineChart : FrameworkElement
    {
        private VisualCollection _children;
        public List<Point> Seriese = new List<Point>();
        public Brush brush = Brushes.Blue;
        public Pen pen = new Pen(Brushes.Blue, 1);


        public LineChart()
        {
            _children = new VisualCollection(this);
        }

        // Provide a required override for the VisualChildrenCount property.
        protected override int VisualChildrenCount
        {
            get { return _children.Count; }
        }

        // Provide a required override for the GetVisualChild method.
        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= _children.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            return _children[index];
        }

        public DrawingVisual Draw()
        {
            DrawingVisual dv = new DrawingVisual();
            DrawingContext dc = dv.RenderOpen();


            dc.Close();
            return dv;
        }
    }
}
