using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GazeMonitoring.Views
{
    /// <summary>
    /// Interaction logic for ScreenConfigurationWindow.xaml
    /// </summary>
    public partial class ScreenConfigurationWindow : Window
    {
        private Point startPoint;
        private Rectangle rectSelectArea;
        private ContentControl _rectContentControl;
        private bool _activated = false;


        public ScreenConfigurationWindow()
        {
            InitializeComponent();
        }

        private void Canvas_MouseDown_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_activated)
            {
                startPoint = e.GetPosition(paintSurface);

                foreach (UIElement paintSurfaceChild in paintSurface.Children)
                {
                    if (paintSurfaceChild.IsMouseOver)
                    {
                        return;
                    }
                }

                // Initialize the rectangle.
                // Set border color and width
                rectSelectArea = new Rectangle
                {
                    Stroke = Brushes.LightBlue,
                    StrokeThickness = 2,
                    Fill = new SolidColorBrush(Color.FromRgb(100, 120, 130)) {Opacity = 0},
                    IsHitTestVisible = false
                };

                var grid = new Canvas();
                grid.Children.Add(rectSelectArea);
                grid.Children.Add(new TextBox { Text = "Lorem Ipsum" });

                _rectContentControl = new ContentControl();
                _rectContentControl.Content = grid;
                _rectContentControl.Template = (ControlTemplate) this.FindResource("DesignerItemTemplate");

                Canvas.SetLeft(_rectContentControl, startPoint.X);
                Canvas.SetTop(_rectContentControl, startPoint.Y);
                paintSurface.Children.Add(_rectContentControl);
            }
        }

        private void Canvas_MouseMove_1(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (_activated)
            {
                if (e.LeftButton == MouseButtonState.Released || rectSelectArea == null)
                {
                    _rectContentControl = null;
                    return;
                }

                var pos = e.GetPosition(paintSurface);

                // Set the position of rectangle
                var x = Math.Min(pos.X, startPoint.X);
                var y = Math.Min(pos.Y, startPoint.Y);

                // Set the dimenssion of the rectangle
                var w = Math.Max(pos.X, startPoint.X) - x;
                var h = Math.Max(pos.Y, startPoint.Y) - y;

                _rectContentControl.Width = w;
                _rectContentControl.Height = h;

                Canvas.SetLeft(_rectContentControl, x);
                Canvas.SetTop(_rectContentControl, y);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _activated = !_activated;
        }

        private void PaintSurface_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_activated)
            {
                _activated = false;
            }
        }
    }
}
