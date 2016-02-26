using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace bsChart
{

    public class DrawingVisual_Ext : DrawingVisual
    {
        public String Name;
    }

    public class ChartVisual : FrameworkElement
    {
        private VisualCollection _children;
        public Serieses serieses = new Serieses();
        ScaleXY scale;


        public int min_grid_size = 10;


        public ChartVisual()
        {
            _children = new VisualCollection(this);

            scale = new ScaleXY(this);

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

        

        DrawingVisual SeriesDrawing;

        public void Draw()
        {
            scale.Update(serieses.xRange().min, serieses.yRange().min,
                         serieses.xRange().Size(), serieses.yRange().Size());

            DrawFrame(scale);
            DrawSerieses(scale);
            DrawYGrid(serieses.yRange().Grid() ,scale);

            //if (SeriesDrawing == null)
            //    _children.Add(DrawSerieses(scale));
            //else
            //    DrawSerieses(scale); 
        }

        private void ChartVisual_LayoutUpdated(object sender, EventArgs e)
        {
            //Draw();
        }

        void DrawSerieses(ScaleXY scale)
        {
            if(SeriesDrawing==null)
            {
                SeriesDrawing = new DrawingVisual();
                _children.Add(SeriesDrawing);
            }
            DrawingContext dc = SeriesDrawing.RenderOpen();
            serieses.Draw(dc, scale);
            dc.Close();
        }

        DrawingVisual FrameDrawing;

        void DrawFrame(ScaleXY scale)
        {
            if (FrameDrawing == null)
            {
                FrameDrawing = new DrawingVisual();
                _children.Add(FrameDrawing);
            }
                
            Pen pen = new Pen(Brushes.Black, 2);
            DrawingContext dc = FrameDrawing.RenderOpen();
            dc.DrawRectangle(null, pen, scale.FrameRect());
            dc.Close();
        }

        DrawingVisual YGridDrawing;
        public void DrawYGrid(double[] grid,ScaleXY scale)
        {
            if (grid == null) return;
            if (YGridDrawing == null)
            {
                YGridDrawing = new DrawingVisual();
                _children.Add(YGridDrawing);
            }

            Pen pen = new Pen(Brushes.Gray, 1)
            {
                DashStyle = DashStyles.Dash
            };

            Rect fr = scale.FrameRect();

            DrawingContext dc = YGridDrawing.RenderOpen();
            for (int i = 0; i < grid.Count();i++)
                dc.DrawLine(pen, new Point(fr.Left, scale.TransferY(grid[i])),
                                new Point(fr.Right, scale.TransferY(grid[i])));

            dc.Close();
        }

        Point scale_xy()
        {
            return new Point( ActualWidth / serieses.xRange().Size(),
                              ActualHeight / serieses.yRange().Size());
        }

        Size size_pt()
        {
            return new Size(serieses.xRange().Size() / ActualWidth,
                            serieses.yRange().Size() / ActualHeight);
        }

    }
}
