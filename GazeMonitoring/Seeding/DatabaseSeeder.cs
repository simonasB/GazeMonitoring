using GazeMonitoring.DataAccess;

namespace GazeMonitoring.Seeding
{
    public interface IDatabaseSeeder
    {
        void Seed();
    }

    public class DatabaseSeeder : IDatabaseSeeder
    {
        private readonly IConfigurationRepository _configurationRepository;

        public DatabaseSeeder(IConfigurationRepository configurationRepository)
        {
            _configurationRepository = configurationRepository;
        }

        public void Seed()
        {
            // Create key bindings
            // Default profile
        }
    }
}
