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
        List<List<Ellipse>> all_ellipses = new List<List<Ellipse>>();
        List<Point> points = new List<Point>();
        Canvas area;
        const int SUCKERS = 5;
        int amount_rows = 0;
        int amount_cols = 0;
        public Option_2()
        {
            InitializeComponent();
            area = canva;
        }

        async void drawPoints(int x, int y)
        {
            #region generate points
            area.Height = y * 30;
            area.Width = x * 30;
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
            amount_cols = x;
            amount_rows = y;
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

        private async void FindS_Click(object sender, RoutedEventArgs e)
        {
            if (all_ellipses.Count != 0)
            {
                all_ellipses.Clear();
            }
            //SORT by Y means columns, X is rows
            List<Ellipse> ellipses = new List<Ellipse>(); //list for ellipses
            if (amount_cols >= amount_rows)
            {
                points = points.OrderBy(p => p.Y).ToList(); //sort points to match later sort of ellipse
            }
            else
            {
                points = points.OrderBy(p => p.X).ToList(); //sort points to match later sort of ellipse
            }
            DataPoints.ItemsSource = points;
            foreach (var item in area.Children) //take out ellipses from canva
            {
                if (item is Ellipse)
                {
                    ellipses.Add(item as Ellipse);
                }
            }
            if (amount_cols >= amount_rows)
            {
                ellipses = ellipses.OrderBy(p => ((Point)p.Tag).Y).ToList(); //sort ellipses by points
                int f = 0;
                List<Ellipse> tmp = new List<Ellipse>();
                foreach (var item in ellipses)
                {
                    f++;
                    tmp.Add(item);
                    if (f == amount_cols)
                    {
                        all_ellipses.Add(tmp);
                        tmp = new List<Ellipse>();
                        f = 0;
                    }
                }
            }
            else
            {
                ellipses = ellipses.OrderBy(p => ((Point)p.Tag).X).ToList(); //sort ellipses by points
                int f = 0;
                List<Ellipse> tmp = new List<Ellipse>();
                foreach (var item in ellipses)
                {
                    f++;
                    tmp.Add(item);
                    if (f == amount_rows)
                    {
                        all_ellipses.Add(tmp);
                        tmp = new List<Ellipse>();
                        f = 0;
                    }
                }
            }

            Brush brush = Brushes.Red;
            bool change = false;
            int g = 0;
            //color just to see diffrence
            foreach (var item in all_ellipses)
            {
                foreach (var eli in item)
                {

                    Ellipse ellipse = eli;
                    ellipse.Stroke = brush;
                }
                if (change)
                {
                    brush = Brushes.Red;
                    change = !change;
                }
                else
                {
                    brush = Brushes.Green;
                    change = !change;

                }
                await Task.Delay(50);
            }

        }

        private async Task take_out(List<List<Ellipse>> ellipses, bool recursive = false)
        {
            int leftovers = 0;
            if (recursive)
            {
                leftovers = ellipses[0].Count % 5;
            }
            else
            {
                foreach (var item in ellipses) //get row or column (bigger one)
                {

                    if (item.Count % 5 == 0) //if it's multiplication of 5 then we can take 5,5,5
                    {
                        //TODO: Mode to take odd and even
                        int p = item.Count / 5;
                        for (int i = 0; i < p; i++)
                        {
                            List<Ellipse> el = item.GetRange(i * 5, 5);
                            foreach (var ellipse in el)
                            {
                                ellipse.Opacity = 0.2;
                            }
                            await Task.Delay(500);
                        }
                    }
                    else if (item.Count < 5) //if there is less than 5 we can take all directly
                    {
                        foreach (var el in item)
                        {
                            Ellipse ellipse = el;
                            ellipse.Opacity = 0.2;
                        }
                    }
                    else //there is more than 5, ex. 12,7,9,34
                    {
                        leftovers = item.Count % 5; //rest
                        int p = item.Count / 5; //full 5
                        for (int i = 0; i < p; i++)
                        {
                            List<Ellipse> el = item.GetRange(i * 5, 5);
                            foreach (var ellipse in el)
                            {
                                ellipse.Opacity = 0.2;
                            }
                            await Task.Delay(500);
                        }
                    }
                    await Task.Delay(500);


                }
            }
            if (leftovers != 0)
            {
                Leftovers(leftovers);
            }
        }
        private async void start_Click(object sender, RoutedEventArgs e)
        {
            await take_out(all_ellipses);
        }
        async void Leftovers(int amount)
        {
            List<List<Ellipse>> subellipses = new List<List<Ellipse>>();
            foreach (var item in all_ellipses)
            {
                List<Ellipse> tmp = new List<Ellipse>();
                for (int i = item.Count - 1; i >= 0; i--)
                {
                    tmp = item.Skip(Math.Max(0, item.Count() - amount)).ToList();
                }
                subellipses.Add(tmp);
            }
            subellipses = subellipses.SelectMany(inner => inner.Select((item, index) => new { item, index })) //transpose of matrix
                                                                .GroupBy(i => i.index, i => i.item)
                                                                .Select(g => g.ToList())
                                                                .ToList();
            await take_out(subellipses);
        }

    }
}
