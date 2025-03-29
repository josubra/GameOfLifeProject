using GameOfLifeApi.Entity;
using System.Linq.Expressions;

namespace GameOfLifeApi.Service.Abstractions
{
    public interface IGameOfLifeService
    {
        Task<GameOfLifeBoard> GetGameOfLifeBoardAsync(int id, CancellationToken cancellationToken = default);
    }
}
