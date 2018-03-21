using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Serialization;
using Autofac;
using Autofac.Configuration;
using GazeMonitoring.Common;
using GazeMonitoring.Common.Entities;
using Microsoft.Extensions.Configuration;
using MockMonitoring;
using TheEyeTribeMonitoring;
using TobiiCoreMonitoring;

namespace GazeMonitoring
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private GazeDataMonitor _gazeDataMonitor;
        private static IContainer _container;
        private static ILifetimeScope _lifetimeScope;

        public MainWindow() {
            InitializeComponent();

            FileStream fileStream =
                File.Create(Path.Combine(Directory.GetCurrentDirectory(), "test.xml"));
            XmlWriter writer = XmlWriter.Create(fileStream);

            writer.WriteStartDocument();

            writer.WriteStartElement("GazeData");

            writer.WriteStartElement("SubjectInfo");

            writer.WriteElementString(nameof(SubjectInfo.Age), "1");
            writer.WriteElementString(nameof(SubjectInfo.Name), "Name");
            writer.WriteElementString(nameof(SubjectInfo.Details), "Details");
            
            writer.WriteEndElement();

            writer.WriteStartElement("Saccades");

            var saccade = new Saccade {
                Amplitude = 1,
                Direction = 2,
                EndTimeStamp = 4,
                StartTimeStamp = 3,
                Velocity = 5
            };

            var serializer = new XmlSerializer(saccade.GetType(), new XmlRootAttribute("Saccade"));
            serializer.Serialize(writer, saccade);

            writer.WriteEndElement();
            writer.WriteEndElement();

            writer.WriteEndDocument();

            writer.Dispose();
            fileStream.Dispose();

            EyeTribeInitializer.Init();
            TobiiCoreInitializer.Init();
            MockInitializer.Init();


            var config = new ConfigurationBuilder();

            config.AddJsonFile("config.json");
            var configurationRoot = config.Build();
            if (!bool.TryParse(configurationRoot["autoDiscover"], out var autoDiscover)) {
                // log info message
            }

            var builder = new ContainerBuilder();
            var module = new ConfigurationModule(configurationRoot);
            builder.RegisterModule<CommonModule>();
            builder.RegisterModule(module);

            if (autoDiscover) {
                var discoveryManager = new TrackerDiscoveryManager();
                discoveryManager.Discover(builder);
            }

            CmbDataStreams.ItemsSource = Enum.GetValues(typeof(DataStream)).Cast<DataStream>();

            _container = builder.Build();
        }

        private void CmbDataStreams_SelectionChanged(object sender, SelectionChangedEventArgs e) {
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e) {
            _lifetimeScope = _container.BeginLifetimeScope();

            var subjectInfo = new SubjectInfo {
                Age = 10,
                Details = "testing",
                Name = "default",
            };

            _gazeDataMonitor = _lifetimeScope.Resolve<GazeDataMonitor>(
                new NamedParameter(Constants.DataStreamParameterName, CmbDataStreams.SelectedItem),
                new NamedParameter(Constants.SubjectInfoParameterName, subjectInfo));

            _gazeDataMonitor.Start();

            BtnStart.IsEnabled = false;
            CmbDataStreams.IsEnabled = false;
            BtnStop.IsEnabled = true;
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e) {
            _gazeDataMonitor.Stop();
            _lifetimeScope.Dispose();

            BtnStart.IsEnabled = true;
            CmbDataStreams.IsEnabled = true;
            BtnStop.IsEnabled = false;
        }
    }
}
