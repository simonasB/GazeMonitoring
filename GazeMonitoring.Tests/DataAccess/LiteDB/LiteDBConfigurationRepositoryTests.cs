using System;
using GazeMonitoring.Common;
using GazeMonitoring.DataAccess.LiteDB;
using GazeMonitoring.Model;
using NUnit.Framework;

namespace GazeMonitoring.Tests.DataAccess.LiteDB
{
    [TestFixture]
    public class LiteDBConfigurationRepositoryTests
    {
        private readonly LiteDBConfigurationRepository _configurationRepository = new LiteDBConfigurationRepository(new AppDataHelper());

        [Test]
        public void A()
        {
            /*var id = _configurationRepository.Save(new MonitoringConfiguration
            {
                Name = "test"
            });*/

            var a = _configurationRepository.Search<MonitoringConfiguration>(8);
            a.ScreenConfigurations.ForEach(o => Console.WriteLine(o.Id));
        }
    }
}
