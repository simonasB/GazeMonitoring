using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using GazeMonitoring.Base;
using GazeMonitoring.DataAccess;
using GazeMonitoring.DataAccess.LiteDB;
using GazeMonitoring.Model;

namespace GazeMonitoring.Views
{
    /// <summary>
    /// Interaction logic for ScreenConfigurationWindow.xaml
    /// </summary>
    public partial class ScreenConfigurationWindow : Window
    {
        private readonly IAppLocalContextManager _appLocalContextManager;
        private readonly IConfigurationRepository _configurationRepository;
        private Point _startPoint;
        private Rectangle _rectSelectArea;
        private ContentControl _rectContentControl;
        private bool _activated;
        private MonitoringConfiguration _monitoringConfiguration;
        private AppLocalContext _appLocalContext;

        public const string AreaOfInterestTitle = "AreaOfInterestTitle";

        public ScreenConfigurationWindow(IAppLocalContextManager appLocalContextManager, IConfigurationRepository configurationRepository)
        {
            _appLocalContextManager = appLocalContextManager;
            _configurationRepository = configurationRepository;
            InitializeComponent();

            this.PreviewKeyDown += HandleEsc;
            _appLocalContext = appLocalContextManager.Get();
            _appLocalContext.MonitoringConfigurationId = 8;
            _appLocalContext.ScreenConfigurationId = "76edd958-e418-4c37-9f38-b3972c274926";
        }

        public ScreenConfigurationWindow() : this(new AppLocalContextManager(new LiteDBConfigurationRepository()), new LiteDBConfigurationRepository())
        {
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _monitoringConfiguration = _configurationRepository.Search<MonitoringConfiguration>(_appLocalContext.MonitoringConfigurationId.Value);

            var screenConfiguration =
                _monitoringConfiguration.ScreenConfigurations?.FirstOrDefault(o =>
                    o.Id == _appLocalContext.ScreenConfigurationId);

            screenConfiguration?.AreasOfInterest?.ForEach(areaOfInterest =>
            {
                var grid = new Canvas();
                var deleteButton = new Button
                {
                    Content = "Remove",
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Tag = areaOfInterest.Id
                };
                deleteButton.Click += DeleteButtonOnClick;
                Canvas.SetTop(deleteButton, 20);


                grid.Children.Add(new Rectangle
                {
                    Stroke = Brushes.LightBlue,
                    StrokeThickness = 2,
                    Fill = new SolidColorBrush(Color.FromRgb(100, 120, 130)) {Opacity = 0},
                    IsHitTestVisible = false
                });
                grid.Children.Add(new TextBox { Text = areaOfInterest.Name, Tag = AreaOfInterestTitle});
                grid.Children.Add(deleteButton);

                var rectContentControl = new ContentControl
                {
                    Content = grid,
                    Template = (ControlTemplate)this.FindResource("DesignerItemTemplate"),
                    Tag = areaOfInterest.Id
                };

                Canvas.SetLeft(rectContentControl, areaOfInterest.Left);
                Canvas.SetTop(rectContentControl, areaOfInterest.Top);

                rectContentControl.Width = areaOfInterest.Width;
                rectContentControl.Height = areaOfInterest.Height;

                paintSurface.Children.Add(rectContentControl);
            });
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }

        private void Canvas_MouseDown_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_activated)
            {
                _startPoint = e.GetPosition(paintSurface);

                foreach (UIElement paintSurfaceChild in paintSurface.Children)
                {
                    if (paintSurfaceChild.IsMouseOver)
                    {
                        return;
                    }
                }

                // Initialize the rectangle.
                // Set border color and width
                _rectSelectArea = new Rectangle
                {
                    Stroke = Brushes.LightBlue,
                    StrokeThickness = 2,
                    Fill = new SolidColorBrush(Color.FromRgb(100, 120, 130)) {Opacity = 0},
                    IsHitTestVisible = false
                };

                var uniqueName = Guid.NewGuid().ToString().Replace("-", "_");

                var grid = new Canvas();
                var deleteButton = new Button
                {
                    Content = "Remove", HorizontalAlignment = HorizontalAlignment.Right, Tag = uniqueName
                };
                deleteButton.Click += DeleteButtonOnClick;
                Canvas.SetTop(deleteButton, 20);

                grid.Children.Add(_rectSelectArea);
                grid.Children.Add(new TextBox { Text = "Lorem Ipsum", Tag = AreaOfInterestTitle });
                grid.Children.Add(deleteButton);

                _rectContentControl = new ContentControl
                {
                    Content = grid,
                    Template = (ControlTemplate) this.FindResource("DesignerItemTemplate"),
                    Tag = uniqueName,
                    
                };

                Canvas.SetLeft(_rectContentControl, _startPoint.X);
                Canvas.SetTop(_rectContentControl, _startPoint.Y);
                paintSurface.Children.Add(_rectContentControl);
            }
        }

        private void DeleteButtonOnClick(object sender, RoutedEventArgs e)
        {
            var elementToRemove = paintSurface.Children.Cast<FrameworkElement>().First(o => o.Tag == ((Button)sender).Tag);
            paintSurface.Children.Remove(elementToRemove);
        }

        private void Canvas_MouseMove_1(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (_activated)
            {
                if (e.LeftButton == MouseButtonState.Released || _rectSelectArea == null)
                {
                    _rectContentControl = null;
                    return;
                }

                var pos = e.GetPosition(paintSurface);

                // Set the position of rectangle
                var x = Math.Min(pos.X, _startPoint.X);
                var y = Math.Min(pos.Y, _startPoint.Y);

                // Set the dimenssion of the rectangle
                var w = Math.Max(pos.X, _startPoint.X) - x;
                var h = Math.Max(pos.Y, _startPoint.Y) - y;

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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var areasOfInterest = new List<AreaOfInterest>();

            foreach (UIElement paintSurfaceChild in paintSurface.Children.Cast<UIElement>().Where(o => o.DependencyObjectType.SystemType == typeof(ContentControl)))
            {
                var contentControl = (ContentControl) paintSurfaceChild;
                var title = (TextBox)((Canvas) contentControl.Content).Children.Cast<FrameworkElement>().First(o => (string)o.Tag == AreaOfInterestTitle);
                var areaOfInterest = new AreaOfInterest
                {
                    Height = contentControl.Height,
                    Width = contentControl.Width,
                    Id = (string) contentControl.Tag,
                    Left = Canvas.GetLeft(paintSurfaceChild),
                    Top = Canvas.GetTop(paintSurfaceChild),
                    Name = title.Text
                };

                areasOfInterest.Add(areaOfInterest);
            }

            if (_appLocalContext.ScreenConfigurationId == null)
            {
                _monitoringConfiguration.ScreenConfigurations =
                    _monitoringConfiguration.ScreenConfigurations ?? new List<ScreenConfiguration>();
                var id = Guid.NewGuid().ToString();
                _monitoringConfiguration.ScreenConfigurations.Add(new ScreenConfiguration
                {
                    AreasOfInterest = areasOfInterest,
                    Id = id,
                    Name = "test",
                    Number = 1
                });
                _appLocalContext.ScreenConfigurationId = id;
            }
            else
            {
                var screenConfiguration =
                    _monitoringConfiguration.ScreenConfigurations.First(o =>
                        o.Id == _appLocalContext.ScreenConfigurationId);
                screenConfiguration.AreasOfInterest = areasOfInterest;
            }

            _configurationRepository.Update(_monitoringConfiguration);
        }
    }
}
