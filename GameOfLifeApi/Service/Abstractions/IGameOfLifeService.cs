using GameOfLifeApi.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GameOfLifeApi.Service.Abstractions
{
    public interface IGameOfLifeService
    {
        Task<GameOfLifeBoard> GetGameOfLifeBoardAsync(int id, CancellationToken cancellationToken = default);

        Task<GameOfLifeBoard> AddGameOfLifeBoardAsync(bool[][] boardState, CancellationToken cancellationToken = default);

        Task<bool[][]?> GetAndSaveNextStateAsync(int id);

        Task<bool[][]?> GetAndSaveFutureStateAsync(int id, int steps);

        Task<bool[][]?> GetAndSaveFinalStateAsync(int id, int maxAttempts);
    }
}
