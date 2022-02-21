using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PointsGeneratorFinder
{
    /// <summary>
    /// Logika interakcji dla klasy Option_2.xaml
    /// </summary>
    public partial class Option_2 : Window
    {
        List<Point> points = new List<Point>();
        Canvas area;
        const int SUCKERS = 5;
        public Option_2()
        {
            InitializeComponent();
            area = canva;
            drawPoints(7, 10);
        }
       
        async void drawPoints(int x, int y)
        {
            #region generate points
            area.Height = y * 30;
            area.Width = x * 30 ;
            Random random = new Random();
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    points.Add(new Point(i * 30 + 10, j * 30 + 10));
                }

            }
            #endregion
            foreach (var item in points)
            {
                Console.WriteLine(item);
                SolidColorBrush brush = new SolidColorBrush();
                brush.Color = Colors.Blue;

                //Ellipse dot in xy field area
                Ellipse ellipse = new Ellipse() { Height = 4, Width = 4 };
                ellipse.Fill = brush;
                ellipse.StrokeThickness = 2;
                ellipse.Stroke = Brushes.Black;
                ellipse.Margin = new Thickness(item.X - 2, item.Y - 2, 0, 0);
                ellipse.Tag = item;

                //Label to see points X,Y value
                Label label = new Label();
                label.Content = "X" + item.X + " Y" + item.Y;
                label.Margin = new Thickness(item.X - 10, item.Y - 15, 0, 0);
                label.FontSize = 5;
                label.Style = Resources["DefaultStyleKey"] as Style;
                area.Children.Add(ellipse);
                area.Children.Add(label);
                await Task.Delay(5);
            }

        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            int y = (int)Rows.Value;
            int x = (int)Columns.Value;
            area.Children.Clear();
            points.Clear();
            drawPoints(x, y);
        }
        //Sorting by X
        private void Find_Click(object sender, RoutedEventArgs e)
        {
            //X
            List<Ellipse> ellipses = new List<Ellipse>();
            points = points.OrderBy(p => p.X).ToList();
            DataPoints.ItemsSource = points;
            foreach (var item in area.Children)
            {
                if (item is Ellipse)
                {
                    ellipses.Add(item as Ellipse);
                }
            }
            ellipses = ellipses.OrderBy(p => ((Point)p.Tag).X).ToList();
            for (int i = 0; i < ellipses.Count; i++)
            {

                if (i % 2 == 0) //even
                {
                    Ellipse ellipse = ellipses[i];
                    ellipse.Stroke = Brushes.Green;

                }
                else //odd
                {
                    Ellipse ellipse = ellipses[i];
                    ellipse.Stroke = Brushes.Red;
                }

            }
        }
        //Sorting by Y
        private void FindY_Click(object sender, RoutedEventArgs e)
        {
            List<Ellipse> ellipses = new List<Ellipse>(); //list for ellipses
            points = points.OrderBy(p => p.Y).ToList(); //sort points to match later sort of ellipse
            DataPoints.ItemsSource = points;
            foreach (var item in area.Children) //take out ellipses from canva
            {
                if (item is Ellipse)
                {
                    ellipses.Add(item as Ellipse);
                }
            }
            ellipses = ellipses.OrderBy(p => ((Point)p.Tag).Y).ToList(); //sort ellipses by points
            for (int i = 0; i < ellipses.Count; i++)
            {

                if (i % 2 == 0) //even
                {
                    Ellipse ellipse = ellipses[i];
                    ellipse.Stroke = Brushes.Green;
                }
                else //odd
                {
                    Ellipse ellipse = ellipses[i];
                    ellipse.Stroke = Brushes.Red;
                }

            }
        }
    }
}
