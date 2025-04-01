using Testcontainers.MySql;

namespace GameOfLifeTests.Fixture
{
    public  class DatabaseFixture : IAsyncLifetime
    {
        public MySqlContainer DbContainer { get; }
        public DatabaseFixture() => DbContainer = new MySqlBuilder()
                .WithImage("mysql:latest")
                .WithPortBinding(3306)
                .WithUsername("root")
                .WithPassword("password")
                .Build();

        public async Task InitializeAsync()
        {
            await DbContainer.StartAsync();
        }

        public async Task DisposeAsync()
        {
            await DbContainer.DisposeAsync();
        }
    }
}
