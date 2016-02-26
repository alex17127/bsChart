using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace bsChart
{
    public class ScaleXY
    {
        FrameworkElement fe;
        Rect DataRect;

        public double x_scale = 1;
        public double y_scale = 1;
        public double Width = 1;
        public double Height = 1;
        public double xMin;
        public double yMin;


        public ScaleXY(FrameworkElement fe)
        {
            this.fe = fe;
        }

        public double border_frame_shift = 4;

        public void Update(double xMin, double yMin, double dataWidth, double dataHeight)
        {
            this.xMin = xMin;
            this.yMin = yMin;
            this.Width = fe.ActualWidth - 2* border_frame_shift;
            this.Height = fe.ActualHeight - 2 * border_frame_shift;
            x_scale = Width / dataWidth;
            y_scale = Height / dataHeight;
        }

        public double TransferX(double x)
        {
            return border_frame_shift + (x-xMin) * x_scale;
        }

        public double TransferY(double y)
        {
            return   fe.ActualHeight - border_frame_shift - (y- yMin) * y_scale;//
        }

        public Rect FrameRect()
        {
            int shift = 1;
            return new Rect(shift, shift,
                (fe.ActualWidth - shift <= 0) ? 1 : fe.ActualWidth - shift,
                (fe.ActualHeight - shift <= 0) ? 1 : fe.ActualHeight - shift);
        }

        public double[] scaleX()
        {
            return null;
        }

        

    }
}
