using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Clippin_Fillin_And_Killin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Declaration Section

        private Random _rand = new Random();

        private Brush _subjectBack = new SolidColorBrush(ColorFromHex("30427FCF"));
        private Brush _subjectBorder = new SolidColorBrush(ColorFromHex("427FCF"));
        private Brush _clipBack = new SolidColorBrush(ColorFromHex("30D65151"));
        private Brush _clipBorder = new SolidColorBrush(ColorFromHex("D65151"));
        private Brush _intersectBack = new SolidColorBrush(ColorFromHex("609F18CC"));
        private Brush _intersectBorder = new SolidColorBrush(ColorFromHex("9F18CC"));
        //Default Convex
        private List<Point> recordedPoly = new List<Point> { new Point(50, 150),
                                                       new Point(200, 50),
                                                       new Point(350, 150),
                                                       new Point(350, 300),
                                                       new Point(250, 300),
                                                       new Point(200, 250),
                                                       new Point(150, 350),
                                                       new Point(100, 250),
                                                       new Point(100, 200)};
        private bool Record = false;
        //default rectangle
        List<Point> Rectangle = new List<Point> { new Point(100, 100), new Point(300, 100), new Point(300, 300), new Point(100, 300) };


        List<Edge> allEdges = new List<Edge>();
        List<Bucket> ET = new List<Bucket>();
        List<Bucket> AET = new List<Bucket>();

        #endregion

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();

            
        }

        #endregion
        #region Event Listeners

        private void btnTriRect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double width = canvas.ActualWidth;
                double height = canvas.ActualHeight;

                List<Point> poly1 = new List<Point> {
                    new Point(_rand.NextDouble() * width, _rand.NextDouble() * height),
                    new Point(_rand.NextDouble() * width, _rand.NextDouble() * height),
                    new Point(_rand.NextDouble() * width, _rand.NextDouble() * height) };

                Point rectPoint = new Point(_rand.NextDouble() * (width * .75d), _rand.NextDouble() * (height * .75d));		//	don't let it start all the way at the bottom right
                Rect rect = new Rect(
                    rectPoint,
                    new Size(_rand.NextDouble() * (width - rectPoint.X), _rand.NextDouble() * (height - rectPoint.Y)));

                List<Point> poly2 = new List<Point> { rect.TopLeft, rect.TopRight, rect.BottomRight, rect.BottomLeft };

                List<Point> intersect = SutherlandHodgman.GetIntersectedPolygon(poly1, poly2);

                canvas.Children.Clear();
                ShowPolygon(poly1, _subjectBack, _subjectBorder, 1d);
                ShowPolygon(poly2, _clipBack, _clipBorder, 1d);
                ShowPolygon(intersect, _intersectBack, _intersectBorder, 3d);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnConvex_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                List<Point> intersect = SutherlandHodgman.GetIntersectedPolygon(recordedPoly, Rectangle);

                canvas.Children.Clear();
                ShowPolygon(recordedPoly, _subjectBack, _subjectBorder, 1d);
                ShowPolygon(Rectangle, _clipBack, _clipBorder, 1d);
                ShowPolygon(intersect, _intersectBack, _intersectBorder, 3d);
                Record = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Private Methods

        private void ShowPolygon(List<Point> points, Brush background, Brush border, double thickness)
        {
            if (points == null || points.Count == 0)
            {
                return;
            }

            Polygon polygon = new Polygon();
            polygon.Fill = background;
            polygon.Stroke = border;
            polygon.StrokeThickness = thickness;

            foreach (Point point in points)
            {
                polygon.Points.Add(point);
            }

            canvas.Children.Add(polygon);
        }

        /// <summary>
        /// This is just a wrapper to the color converter (why can't they have a method off the color class with all
        /// the others?)
        /// </summary>
        private static Color ColorFromHex(string hexValue)
        {
            if (hexValue.StartsWith("#"))
            {
                return (Color)ColorConverter.ConvertFromString(hexValue);
            }
            else
            {
                return (Color)ColorConverter.ConvertFromString("#" + hexValue);
            }
        }

        #endregion

        private void recordButton_Click(object sender, RoutedEventArgs e)
        {
                
                canvas.Children.Clear();
                recordedPoly = new List<Point>();
                List<Edge> allEdges = new List<Edge>();
                List<Bucket> ET = new List<Bucket>();
                List<Bucket> AET = new List<Bucket>();
                List<Point> Base = new List<Point> {new Point(0,0),new Point(canvas.ActualWidth, 0), new Point(canvas.ActualWidth, canvas.ActualHeight),new Point(0, canvas.ActualHeight) };
                ShowPolygon(Base, Brushes.White, Brushes.White,1d);
                
                Record = true;
            
        }


        public void GetAllEdges(List<Point> points)
        {
            for(int i =0; i< points.Count - 2; i++)
            {
                allEdges.Add(new Edge(points[i], points[i + 1]));
            }

        }
        public void initialiseET(List<Edge> edges)
        {
            foreach(Edge edge in edges)
            {
                ET.Add(new Bucket(edge));
            }
        }



        private void fillButton_Click(object sender, RoutedEventArgs e)
        {
            ShowPolygon(recordedPoly, Brushes.White, Brushes.Black, 2d);
            Record = false;
            GetAllEdges(recordedPoly);
            initialiseET(allEdges);

        
        }

        public double findTheSmallestY(List<Bucket> bucketList)
        {
            double y = bucketList[0].yMin;
            foreach(Bucket bucket in bucketList)
            {
                y = Math.Min(y, bucket.yMin);
            }

            return y;
        }

        public Bucket findTheSmallestYOwner(List<Bucket> bucketList,double y)
        {
           
            foreach (Bucket bucket in bucketList)
            {
                if(bucket.yMin == y )
                {
                    return bucket;
                }
            }

            return bucketList[0];
            
        }

        

        public void fillPolygon()
        {
            double y = findTheSmallestY(ET);
            Bucket smallestYowner = findTheSmallestYOwner(ET, y);
            double x = smallestYowner.xMin;

            while (AET.Count > 0 | ET.Count>0 )
            {
                AET.Add(smallestYowner);// move buckket
                ET.Remove(smallestYowner);//move bucket
                AET.OrderBy(o => o.xMin);//sort by x value
               
                foreach(Bucket bucket in AET)
                {
                    x += smallestYowner.slopeInverse; 
                }
              
            }


        }

        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(Record == true)
            {
                Point currentPoint = e.GetPosition(TheGrid);
                Point startingPoint = new Point(0,0);
                recordedPoly.Add(currentPoint);
                if(recordedPoly.Count>1)
                {
                    startingPoint = recordedPoly[0];
                    DrawLine(recordedPoly[recordedPoly.Count - 2], recordedPoly[recordedPoly.Count - 1]);

                    
                }
            }
        }
        private void DrawLine(Point point1, Point point2)
        {
            Line line = new Line();
            line.Stroke = Brushes.Black;

            line.X1 = point1.X;
            line.X2 = point2.X;
            line.Y1 = point1.Y;
            line.Y2 = point2.Y;

            line.StrokeThickness = 2;
            canvas.Children.Add(line);
        }

        private void showRectangle_Click(object sender, RoutedEventArgs e)
        {
            double width = canvas.ActualWidth;
            double height = canvas.ActualHeight;
            Rectangle = new List<Point> { new Point(width / 2 - 150, height / 2 - 100), new Point(width / 2 + 150, height / 2 - 100), new Point(width / 2 + 150, height / 2 + 100), new Point(width / 2 - 150, height / 2 + 100) };
            ShowPolygon(Rectangle, _clipBack, _clipBorder, 1d);

        }
    }
}

