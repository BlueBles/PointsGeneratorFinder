using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Window = System.Windows.Window;

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
        const int SUCKERS = 4;
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
            area.Height = y * 65; //rows
            area.Width = x * 55; //columns
            Random random = new Random();
            for (int i = 0; i < x; i++) //i columns
            {
                for (int j = 0; j < y; j++) //j rows
                {
                    points.Add(new Point(i * 55  , j * 65 )); //columns , rows
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

        /// <summary>
        /// Find the best rows and columns and sort them.
        /// </summary>
        /// <remarks>
        /// We have to know the exact amount of rows and columns to make sorting of points correctly
        /// </remarks>
        private async void FindS_Click(object sender, RoutedEventArgs e)
        {
            bool more_cols = false;
            if (amount_cols >= amount_rows)
            {
                more_cols = true;
            }
            //clear at begin to make sure we looking in list of points
            if (all_ellipses.Count != 0)
            {
                all_ellipses.Clear();
            }
            //SORT by Y means columns, X is rows, for square matrix it goes columns
            List<Ellipse> ellipses = new List<Ellipse>(); //list for ellipses
            if (more_cols)
            {
                points = points.OrderBy(p => p.Y).ToList(); //sort points to match later sort of ellipse
            }
            else
            {
                points = points.OrderBy(p => p.X).ToList(); //sort points to match later sort of ellipse
            }
            DataPoints.ItemsSource = points; //show points to confirm positions
            foreach (var item in area.Children) //take out only ellipses from canva
            {
                if (item is Ellipse)
                {
                    ellipses.Add(item as Ellipse);
                }
            }
            if (more_cols)
            {
                ellipses = ellipses.OrderBy(p => ((Point)p.Tag).Y).ToList(); //sort ellipses by points Y
                int f = 0; //variable that track that there is good amount of points in one row/col
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
                ellipses = ellipses.OrderBy(p => ((Point)p.Tag).X).ToList(); //sort ellipses by points X
                int f = 0; //variable that track that there is good amount of points in one row/col
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
            #region VISUALISATION
            Brush brush = Brushes.Red;
            bool change = false;
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
            #endregion

        }

        private async Task take_out(List<List<Ellipse>> ellipses)
        {
            int leftovers = 0;

            foreach (var item in ellipses) //get one row or column (bigger one)
            {
                if (item.Count % SUCKERS == 0 && item.Count > SUCKERS)
                {
                    int p = item.Count / SUCKERS;
                    int p_odd = p % 2;

                    for (int i = 0; i < p - p_odd; i += 2)
                    {
                        if (p_odd == 1) 
                        {
                            List<Ellipse> el_odd = item.Skip(i * SUCKERS).Take(SUCKERS).ToList();
                            el_odd.ForEach(x => x.Opacity = 0.2);
                            await Task.Delay(500);
                            i += 1;
                        }
                        if(i >= p - p_odd) //if there is only elements in amount of suckers
                        {
                            break;
                        }
                        List<Ellipse> el = item.Skip(i * SUCKERS).Take(SUCKERS*2).ToList();
                        List<Ellipse> even = new List<Ellipse>();
                        List<Ellipse> odd = new List<Ellipse>();
                        for (int h = 0; h < el.Count; h++)
                        {
                            if (h % 2 == 0)
                            {
                                //even
                                even.Add(el[h]);
                            }
                            else
                            {
                                odd.Add(el[h]);
                                //odd
                            }
                        }
                        even.ForEach(x => x.Opacity = 0.2); //shorter than loop foreach with brackets
                        await Task.Delay(500);
                        odd.ForEach(x => x.Opacity = 0.2); //shorter than loop foreach with brackets
                        await Task.Delay(500);
                    }
                }
        
                else if (item.Count < SUCKERS) //if there is less than amount suckers we can take all directly
                {
                    item.ForEach(x => x.Opacity = 0.2);
                }
                else //there is more than suckers amount, ex. 12,7,9,34
                {
                    leftovers = item.Count % SUCKERS; //rest
                    int p = item.Count / SUCKERS; //full grip (p = what we can take at once)
                    for (int i = 0; i < p; i++)
                    {
                        List<Ellipse> el = item.GetRange(i * SUCKERS, SUCKERS);
                        foreach (var ellipse in el)
                        {
                            ellipse.Opacity = 0.2;
                        }
                        await Task.Delay(500);
                    }
                }
                await Task.Delay(500);

            }

            if (leftovers != 0)
            {
                Leftovers(leftovers);
            }

            //TEST AND INFO IF ALL IS GOOD
            #region test
            bool ok = true;
            foreach (var item in area.Children) //take out only ellipses from canva
            {
                if (item is Ellipse)
                {
                    Ellipse el = item as Ellipse;
                    if(el.Opacity != 0.2)
                    {
                        ok = false;
                    }
                }
            }
            info.Content = ok ? "It's good" : "Something missing";
            #endregion
        }
        private async void start_Click(object sender, RoutedEventArgs e)
        {
            info.Content = "";
            await take_out(all_ellipses);
        }
        /// <summary>
        /// Take points that left in matrix <c>subellipses</c>
        /// </summary>
        /// <example>
        /// We got 7%5 it gaves us 2, so in this function we are taking 2 last col/rows and then we transpose cols->rows or rows->cols
        /// </example>
        /// <remarks>
        /// After the iteration if the number rows or cols was not a multiplicity of 5.
        /// We take off from all cols/rows the amount that left, and then we do a matrix transpose
        /// </remarks>
        /// <param name="amount">Quantity of not touched points in cols/rows</param>
        async void Leftovers(int amount)
        {
            //init new list that will contains what left from matrix
            List<List<Ellipse>> subellipses = new List<List<Ellipse>>();
            foreach (var item in all_ellipses) //loop throught points
            {
                List<Ellipse> tmp = new List<Ellipse>();
                for (int i = item.Count - 1; i >= 0; i--)
                {
                    //Take only values that we need, from the end of list (skip doesn't change order)
                    tmp = item.Skip(Math.Max(0, item.Count() - amount)).ToList();
                }
                //create new 
                subellipses.Add(tmp);
            }
            //We have to transpose the matrix to find optimal moves
            //Optimal -> as much as possible 
            subellipses = subellipses.SelectMany(inner => inner.Select((item, index) => new { item, index })) //transpose of matrix
                                                                .GroupBy(i => i.index, i => i.item)
                                                                .Select(g => g.ToList())
                                                                .ToList();
            //TODO:uncomment under
            all_ellipses = subellipses; //this makes that it works in third recursion where there is on two(col/row) less than 5 elements
            await take_out(subellipses);
        }

    }
}
