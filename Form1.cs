﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graphics6
{
    public partial class Form1 : Form
    {
        private Graphics perspective_g;
        private Bitmap   perspective_bmp;

        private Graphics orthographic_g;
        private Bitmap   orthographic_bmp;

        private Primitive cur_primitive;



        private Transform get_perpective_transform()
        {
            switch(PerspectiveComboBox.SelectedItem.ToString())
            {
                case "Перспективная":
                    {
                        return Transform.PerspectiveProjection();
                    }
                case "Изометрическая":
                    {
                        return Transform.IsometricProjection();
                    }
                default:
                    {
                        return Transform.PerspectiveProjection();
                    }
            }
        }


        //Рисует координатные оси и фигуру
        private void DrawAxis(Graphics g, Transform t, int width, int height)
        {
            List<Primitive> p = new List<Primitive>();
            XYZPoint a = new XYZPoint(0, 0, 0);
            XYZPoint b = new XYZPoint(0.8, 0, 0);
            XYZPoint c = new XYZPoint(0, 0.8, 0);
            XYZPoint d = new XYZPoint(0, 0, 0.8);

            p.Add(a);
            p.Add(b);
            p.Add(c);
            p.Add(d);

            p.Add(new XYZLine(a, b));
            p.Add(new XYZLine(a, c));
            p.Add(new XYZLine(a, d));

            p.Add(cur_primitive);

            foreach(Primitive x in p)
            {
                x.Draw(g, t, width, height);
            }
        }

        private void GetPrimitive()
        {
            switch(PrimitiveComboBox.SelectedItem.ToString())
            {
                case "Тетраэдр":
                    {
                        cur_primitive = new Tetrahedron(0.5);
                        break;
                    }
                case "Гексаэдр":
                    {
                        cur_primitive = new Hexahedron(0.5);
                        break;
                    }
                case "Октаэдр":
                    {
                        cur_primitive = new Octahedron(0.5);
                        break;
                    }
                case "Икосаэдр":
                    {
                        cur_primitive = new Icosahedron(0.5);
                        break;
                    }
                case "Додекаэдр":
                    {
                        cur_primitive = new Dodecahedron(0.5);
                        break;
                    }
                default:
                    {
                        cur_primitive = new Tetrahedron(0.5);
                        break;
                    }
            }
        }

        private void Clear()
        {
            perspective_bmp = new Bitmap(PerspectiveBox.Width, PerspectiveBox.Height);
            perspective_g = Graphics.FromImage(perspective_bmp);
            PerspectiveBox.Image = perspective_bmp;

        }
        public Form1()
        {
            InitializeComponent();

            //Создаем Bitmap и Graphics для двух PictureBox
            perspective_bmp = new Bitmap(PerspectiveBox.Width, PerspectiveBox.Height);
            perspective_g = Graphics.FromImage(perspective_bmp);
            PerspectiveBox.Image = perspective_bmp;
            //Инициализируем ComboBox для отображения проекций
            PerspectiveComboBox.SelectedItem = PerspectiveComboBox.Items[1];
            PrimitiveComboBox.SelectedItem = PrimitiveComboBox.Items[0];
            ReflectionComboBox.SelectedItem = ReflectionComboBox.Items[0];

            GetPrimitive();
            DrawAxis(perspective_g, get_perpective_transform(), PerspectiveBox.Width, PerspectiveBox.Height);
        }

        private void ApplyPerspective_Click(object sender, EventArgs e)
        {
            perspective_bmp = new Bitmap(PerspectiveBox.Width, PerspectiveBox.Height);
            perspective_g = Graphics.FromImage(perspective_bmp);
            PerspectiveBox.Image = perspective_bmp;
            DrawAxis(perspective_g, get_perpective_transform(), PerspectiveBox.Width, PerspectiveBox.Height);
        }

        private void ApplyPrimitive_Click(object sender, EventArgs e)
        {
            Clear();
            GetPrimitive();
            DrawAxis(perspective_g, get_perpective_transform(), PerspectiveBox.Width, PerspectiveBox.Height);
        }

        //Смещение
        private void Translate()
        {
            double X = (double)numericUpDown1.Value;
            double Y = (double)numericUpDown2.Value;
            double Z = (double)numericUpDown3.Value;
            cur_primitive.Apply(Transform.Translate(X, Y, Z));
        }

        //Поворот
        private void Rotate()
        {
            double X = (double)numericUpDown4.Value / 180 * Math.PI;
            double Y = (double)numericUpDown5.Value / 180 * Math.PI;
            double Z = (double)numericUpDown6.Value / 180 * Math.PI;
            cur_primitive.Apply(Transform.RotateX(X) * Transform.RotateY(Y) * Transform.RotateZ(Z));
        }

        //Масштаб
        private void Scale()
        {
            double X = (double)numericUpDown7.Value;
            double Y = (double)numericUpDown8.Value;
            double Z = (double)numericUpDown9.Value;
            cur_primitive.Apply(Transform.Scale(X, Y, Z));

        }

        //Отражение
        private void Reflect()
        {
            switch(ReflectionComboBox.SelectedItem.ToString())
            {
                case "Отражение по X":
                    {
                        cur_primitive.Apply(Transform.ReflectX());
                        break;
                    }
                case "Отражение по Y":
                    {
                        cur_primitive.Apply(Transform.ReflectY());
                        break;
                    }
                case "Отражение по Z":
                    {
                        cur_primitive.Apply(Transform.ReflectZ());
                        break;
                    }
                default:
                    {
                        cur_primitive.Apply(Transform.ReflectX());
                        break;
                    }
            }
        }

        //Масштабирование относительно центра
        private void ScaleCenter()
        {
            double C = (double)numericUpDown10.Value;
            cur_primitive.Apply(Transform.Scale(C, C, C));
        }

        private void RotateCenter()
        {
            double X = (double)numericUpDown11.Value / 180 * Math.PI;
            double Y = (double)numericUpDown12.Value / 180 * Math.PI;
            double Z = (double)numericUpDown13.Value / 180 * Math.PI;
            cur_primitive.Apply(Transform.RotateX(X) * Transform.RotateY(Y) * Transform.RotateZ(Z));
        }

        private void RotateLine()
        {
            double X1 = (double)numericUpDown14.Value;
            double Y1 = (double)numericUpDown15.Value;
            double Z1 = (double)numericUpDown16.Value;

            double X2 = (double)numericUpDown17.Value;
            double Y2 = (double)numericUpDown18.Value;
            double Z2 = (double)numericUpDown19.Value;

            XYZLine l = new XYZLine(new XYZPoint(X1, Y1, Z1), new XYZPoint(X2, Y2, Z2));

            double ang = (double)numericUpDown20.Value / 180 * Math.PI;

            cur_primitive.Apply(Transform.RotateLine(l, ang));
        }

        private void ApplyAffin_Click(object sender, EventArgs e)
        {
            Clear();
            Translate();
            Rotate();
            Scale();

            DrawAxis(perspective_g, get_perpective_transform(), PerspectiveBox.Width, PerspectiveBox.Height);
        }

        private void ApplyReflection_Click(object sender, EventArgs e)
        {
            Clear();
            Reflect();

            DrawAxis(perspective_g, get_perpective_transform(), PerspectiveBox.Width, PerspectiveBox.Height);
        }

        private void ApplyScaleCenter_Click(object sender, EventArgs e)
        {
            Clear();
            ScaleCenter();

            DrawAxis(perspective_g, get_perpective_transform(), PerspectiveBox.Width, PerspectiveBox.Height);
        }

        private void ApplyRotationCenter_Click(object sender, EventArgs e)
        {
            Clear();
            RotateCenter();

            DrawAxis(perspective_g, get_perpective_transform(), PerspectiveBox.Width, PerspectiveBox.Height);
        }

        private void ApplyLineRotation_Click(object sender, EventArgs e)
        {
            Clear();
            RotateLine();

            DrawAxis(perspective_g, get_perpective_transform(), PerspectiveBox.Width, PerspectiveBox.Height);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Object Files(*.obj)| *.obj | Text files(*.txt) | *.txt | All files(*.*) | *.* ";
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string info = "";
                    List<XYZPoint> points = cur_primitive.Points;
                    foreach (XYZPoint point in points)
                    {
                        info += "v ";
                        info += point.X + " ";
                        info += point.Y + " ";
                        info += point.Z;
                        info += "\r\n";
                    }

                    foreach (Verge f in cur_primitive.Verges)
                    {
                        info += "f ";
                        foreach (var point in f.Points)
                        {
                            var index = points.IndexOf(point) + 1;
                            info += index + "/" + index + "/" + index + " ";
                        }
                        info += "\r\n";
                    }

                    System.IO.File.WriteAllText(saveDialog.FileName, info);
                }
                catch
                {
                    DialogResult rezult = MessageBox.Show("Невозможно сохранить файл",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog loadDialog = new OpenFileDialog();
            loadDialog.Filter = "Object Files(*.obj)|*.obj|Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (loadDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    List<Verge> verges = new List<Verge>();
                    List<XYZPoint> points = new List<XYZPoint>();
                    string str = System.IO.File.ReadAllText(loadDialog.FileName).Replace("\r\n", "!");
                    string[] info = str.Split('!');

                    List<Verge> Verges = new List<Verge>();

                    foreach (var verge in info)
                    {
                        if (string.IsNullOrWhiteSpace(verge)) continue; // Пропуск пустых строк

                        string[] pIndex = verge.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (pIndex.Length == 0) continue;

                        if (pIndex[0] == "v") // Обработка точки
                        {
                            if (pIndex.Length < 4) continue; // недостаточно координат
                            if (float.TryParse(pIndex[1], out float x) &&
                                float.TryParse(pIndex[2], out float y) &&
                                float.TryParse(pIndex[3], out float z))
                            {
                                points.Add(new XYZPoint(x, y, z));
                            }
                        }
                        else if (pIndex[0] == "f") // Обработка грани
                        {
                            List<XYZPoint> pts = new List<XYZPoint>();
                            for (int i = 1; i < pIndex.Length; i++) // пропускаем f
                            {
                                string[] nums = pIndex[i].Split('/');
                                if (nums.Length > 0 && int.TryParse(nums[0], out int ind1))
                                {
                                    if (ind1 > 0 && ind1 <= points.Count)
                                    {
                                        pts.Add(points[ind1 - 1]); // Индексы начинаются с 1
                                    }
                                }
                            }
                            if (pts.Count > 0)
                            {
                                Verges.Add(new Verge(pts));
                            }
                        }
                    }

                    // Создание и отрисовка фигуры
                    Clear();
                    
                    cur_primitive = new Tetrahedron(Verges, points);
                    DrawAxis(perspective_g, get_perpective_transform(), PerspectiveBox.Width, PerspectiveBox.Height);
                }
                catch
                {
                    DialogResult rezult = MessageBox.Show("Невозможно отобразить файл",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
    }
}
