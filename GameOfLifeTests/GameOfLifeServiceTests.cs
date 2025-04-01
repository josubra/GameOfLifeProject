using GameOfLifeApi.Data;
using GameOfLifeApi.Data.Repository;
using GameOfLifeApi.Entity;
using GameOfLifeApi.Service;
using GameOfLifeApi.Util;
using GameOfLifeTests.Fixture;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;


namespace GameOfLifeTests
{
    public class GameOfLifeServiceTests : IClassFixture<DatabaseFixture>, IAsyncLifetime
    {
        private readonly DatabaseFixture _fixture;
        private ApplicationDbContext _dbContext;
        private GameOfLifeService _service;
        private IDbContextTransaction _transaction;

        public GameOfLifeServiceTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        public async Task InitializeAsync()
        {
            var connectionString = _fixture.DbContainer.GetConnectionString();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                .Options;

            _dbContext = new ApplicationDbContext(options);
            await _dbContext.Database.EnsureCreatedAsync();

            _transaction = await _dbContext.Database.BeginTransactionAsync();

            var loggerMock = new Mock<ILogger<GameOfLifeService>>();
            var repository = new Repository<GameOfLifeBoard>(_dbContext);
            _service = new GameOfLifeService(repository, loggerMock.Object);
        }

        public async Task DisposeAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
            }

            await _dbContext.Database.EnsureDeletedAsync();
            await _dbContext.DisposeAsync();
        }

        [Fact]
        public async Task AddGameOfLifeBoardAsync_ValidBoard_ReturnsBoardWithId()
        {
            var boardState = HelperUtil.SampleBoard;

            var result = await _service.AddGameOfLifeBoardAsync(boardState);

            Assert.True(result.Id > 0);
            var savedState = JsonSerializer.Deserialize<bool[][]>(result.LastBoardState);
            Assert.Equal(boardState, savedState);
        }

        [Fact]
        public async Task GetGameOfLifeBoardAsync_ExistingId_ReturnsBoard()
        {
            var boardState = HelperUtil.SampleBoard;
            var addedBoard = await _service.AddGameOfLifeBoardAsync(boardState);
            var id = addedBoard.Id;

            var result = await _service.GetGameOfLifeBoardAsync(id);

            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            var savedState = JsonSerializer.Deserialize<bool[][]>(result.LastBoardState);
            Assert.Equal(boardState, savedState);
        }

        [Fact]
        public async Task GetGameOfLifeBoardAsync_NonExistentId_ReturnsNull()
        {
            var result = await _service.GetGameOfLifeBoardAsync(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAndSaveNextStateAsync_ValidBoard_ReturnsNextState()
        {
            var initialBoard = HelperUtil.SampleBoard;
            var addedBoard = await _service.AddGameOfLifeBoardAsync(initialBoard);
            var id = addedBoard.Id;

            var result = await _service.GetAndSaveNextStateAsync(id);

            Assert.NotNull(result);
            Assert.NotEqual(initialBoard, result);

            var savedBoard = await _service.GetGameOfLifeBoardAsync(id);
            var savedState = JsonSerializer.Deserialize<bool[][]>(savedBoard.LastBoardState);
            Assert.Equal(result, savedState);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(10)]
        public async Task GetAndSaveFutureStateAsync_ValidBoard_ReturnsFutureState(int steps)
        {
            var initialBoard = HelperUtil.SampleBoard;
            var addedBoard = await _service.AddGameOfLifeBoardAsync(initialBoard);
            var id = addedBoard.Id;

            var result = await _service.GetAndSaveFutureStateAsync(id, steps);

            Assert.NotNull(result);

            var savedBoard = await _service.GetGameOfLifeBoardAsync(id);
            var savedState = JsonSerializer.Deserialize<bool[][]>(savedBoard.LastBoardState);
            Assert.Equal(result, savedState);
        }

        [Theory]
        [InlineData(2)]
        public async Task GetAndSaveFinalStateAsync_IfDidntFind_LastState_AfterMaxAttempts_DontSave_And_ReturnNull(int maxAttempts)
        {
            var generatedBoard = HelperUtil.GenerateBoard(4, 3);
            var addedBoard = await _service.AddGameOfLifeBoardAsync(generatedBoard);
            var id = addedBoard.Id;

            var result = await _service.GetAndSaveFinalStateAsync(id, maxAttempts);

            Assert.Null(result);

            var savedBoard = await _service.GetGameOfLifeBoardAsync(id);
            var savedState = JsonSerializer.Deserialize<bool[][]>(savedBoard.LastBoardState);
            Assert.Equal(generatedBoard, savedState);
        }

        [Theory]
        [InlineData(20)]
        public async Task GetAndSaveFinalStateAsync_IfFindFinalState_AfterMaxAttempts_Save_And_ReturnState(int maxAttempts)
        {
            var generatedBoard = HelperUtil.GenerateBoard(4,3);
            var addedBoard = await _service.AddGameOfLifeBoardAsync(generatedBoard);
            var id = addedBoard.Id;

            var result = await _service.GetAndSaveFinalStateAsync(id, maxAttempts);

            Assert.NotNull(result);

            var savedBoard = await _service.GetGameOfLifeBoardAsync(id);
            var savedState = JsonSerializer.Deserialize<bool[][]>(savedBoard.LastBoardState);
            Assert.Equal(result, savedState);
        }

    }
}