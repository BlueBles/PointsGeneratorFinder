using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PointsGeneratorFinder
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Canvas area;
        List<Point> points = new List<Point>();
        public MainWindow()
        {
            InitializeComponent();
            area = canva;
            drawPoints();
            new Option_2().ShowDialog();
        }

        void drawPoints()
        {
            
            Random random = new Random();
            for (int i = 0; i < 30; i++)
            {
                points.Add(new Point(random.Next(1, 700), random.Next(1, 400)));
            }

            foreach (var item in points)
            {
                Console.WriteLine(item);
                SolidColorBrush brush = new SolidColorBrush();
                brush.Color = Colors.Blue;
                Ellipse ellipse = new Ellipse() { Height = 4, Width = 4 };
                ellipse.Fill = brush;
                ellipse.StrokeThickness = 2;
                ellipse.Stroke = Brushes.Black;
                ellipse.Margin = new Thickness(item.X-2, item.Y-2, 0,0);
                area.Children.Add(ellipse);
            }

        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            area.Children.Clear();
            points.Clear();
            drawPoints();
        }

       

        private void Find_Click(object sender, RoutedEventArgs e)
        {
            points = points.OrderBy(p => p.X).ThenBy(p => p.Y).ToList();

            for (int i = 0; i < points.Count-1; i++)
            {
                double a = points[i].Y - points[i+1].Y;
                double b = points[i+1].X - points[i].X;
                double c = points[i].X * points[i+1].Y - points[i+1].X * points[i].Y;

                if (b != 0)
                {
                    double newX1 = points[i].X;
                    double newY1 = (-c - a * newX1) / b;

                    double newX2 = points[i+1].X;
                    double newY2 = (-c - a * newX2) / b;
                    var myLine = new Line();
                    myLine.Stroke = System.Windows.Media.Brushes.Green;
                    myLine.X1 = newX1;
                    myLine.X2 = newX2;
                    myLine.Y1 = newY1;
                    myLine.Y2 = newY2;
                    myLine.Opacity = 0.5;
                    myLine.StrokeThickness = 30;
                    area.Children.Add(myLine);
                }
            }
            


           
        }

        private void FindY_Click(object sender, RoutedEventArgs e)
        {
            points = points.OrderBy(p => p.Y).ThenBy(p => p.X).ToList();

            for (int i = 0; i < points.Count - 1; i++)
            {
                double a = points[i].Y - points[i + 1].Y;
                double b = points[i + 1].X - points[i].X;
                double c = points[i].X * points[i + 1].Y - points[i + 1].X * points[i].Y;

                if (b != 0)
                {
                    double newX1 = points[i].X;
                    double newY1 = (-c - a * newX1) / b;

                    double newX2 = points[i + 1].X;
                    double newY2 = (-c - a * newX2) / b;
                    var myLine = new Line();
                    myLine.Stroke = System.Windows.Media.Brushes.Green;
                    myLine.X1 = newX1;
                    myLine.X2 = newX2;
                    myLine.Y1 = newY1;
                    myLine.Y2 = newY2;
                    myLine.Opacity = 0.5;
                    myLine.StrokeThickness = 30;
                    area.Children.Add(myLine);
                }
            }
        }

        private void PickUP_Click(object sender, RoutedEventArgs e)
        {
            new Option_2().Show();
        }
    }
}
