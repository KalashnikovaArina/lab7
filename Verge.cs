using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace Graphics6
{
    class Verge
    {

        private IList<XYZPoint> points = new List<XYZPoint>();

        public IList<XYZPoint> Points { get { return points; } set { points = value; } }

        public Verge() { }

        public Verge(IList<XYZPoint> points)
        {
            this.points = points;
        }

        public void AddPoint(XYZPoint p)
        {
            points.Add(p);
        }

        public void Apply(Transform t)
        {
            foreach (var point in Points)
                point.Apply(t);
        }
        List<Color> colors = new List<Color>() { Color.Brown, Color.DarkSeaGreen, Color.Green, Color.Coral, Color.DarkOrange, Color.DarkGray, Color.Purple, Color.IndianRed, Color.YellowGreen, Color.Violet, Color.HotPink, Color.DarkOrchid, Color.OrangeRed, Color.DarkViolet, Color.Pink, Color.MediumVioletRed, Color.DarkKhaki, Color.Red, Color.Plum, Color.DarkOrange, Color.DarkSalmon };

        public void Draw(Graphics g, int count, Transform projection,  int width, int height, Pen pen = null)
        {
            if (pen == null)
                pen = Pens.Black;
            Pen br = new Pen(colors[count]);

            if (Points.Count == 1)
                Points[0].Draw(g, projection, width, height);
            else
            {
                for (int i = 0; i < Points.Count - 1; ++i)
                {
                    var line = new XYZLine(Points[i], Points[i + 1]);
                    line.Draw(g, projection, width, height, br);
                }
                (new XYZLine(Points[Points.Count - 1], Points[0])).Draw(g, projection, width, height);
            }
        }
    }
}
