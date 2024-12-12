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

        public XYZPoint Normal { get; private set; } // Нормаль к грани
        XYZPoint viewDirection = new XYZPoint(1.01, 1.01, 1.01);
        public Verge() { }

        public Verge(IList<XYZPoint> points)
        {
            this.points = points;
            double length = Math.Sqrt(viewDirection.X * viewDirection.X + viewDirection.Y * viewDirection.Y + viewDirection.Z * viewDirection.Z);
            viewDirection.X /= length;
            viewDirection.Y /= length;
            viewDirection.Z /= length;
            ComputeNormal();
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

        public void Draw(Graphics g, int count, Transform projection, int width, int height, XYZPoint center_poly, Pen pen = null)
        {
            ComputeNormal();
            EnsureNormalDirectionOutward(center_poly);
            if (!IsVisible(viewDirection))
                return; // Пропускаем невидимые грани
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
        private void ComputeNormal()
        {
            if (Points.Count < 3)
                return;

            // Вычисляем векторы для нормали
            var v1 = new XYZPoint(
                Points[1].X - Points[0].X,
                Points[1].Y - Points[0].Y,
                Points[1].Z - Points[0].Z);

            var v2 = new XYZPoint(
                Points[2].X - Points[0].X,
                Points[2].Y - Points[0].Y,
                Points[2].Z - Points[0].Z);

            // Векторное произведение для нахождения нормали
            Normal = new XYZPoint(
                v1.Y * v2.Z - v1.Z * v2.Y,
                v1.Z * v2.X - v1.X * v2.Z,
                v1.X * v2.Y - v1.Y * v2.X);

            // Нормализуем нормаль
            double length = Math.Sqrt(Normal.X * Normal.X + Normal.Y * Normal.Y + Normal.Z * Normal.Z);
            Normal.X /= length;
            Normal.Y /= length;
            Normal.Z /= length;
        }
        public bool IsVisible(XYZPoint viewDirection)
        {
            double dotProduct = Normal.X * viewDirection.X + Normal.Y * viewDirection.Y + Normal.Z * viewDirection.Z;
            return dotProduct < -1e-6; // Более строгая проверка
        }
        public void EnsureNormalDirectionOutward(XYZPoint objectCenter)
        {
            if (Normal == null)
                return;

            var toCenter = new XYZPoint(
                objectCenter.X - Points[0].X,
                objectCenter.Y - Points[0].Y,
                objectCenter.Z - Points[0].Z
            );

            double dotProduct = Normal.X * toCenter.X + Normal.Y * toCenter.Y + Normal.Z * toCenter.Z;

            if (dotProduct > 0) // Нормаль направлена внутрь, инвертируем её
            {
                Normal.X = -Normal.X;
                Normal.Y = -Normal.Y;
                Normal.Z = -Normal.Z;
            }
        }
    }
}
