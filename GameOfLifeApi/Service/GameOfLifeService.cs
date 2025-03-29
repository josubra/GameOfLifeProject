using GameOfLifeApi.Data.Repository.Abstraction;
using GameOfLifeApi.Entity;
using GameOfLifeApi.Service.Abstractions;
using System.Linq.Expressions;

namespace GameOfLifeApi.Service
{
    public class GameOfLifeService : IGameOfLifeService
    {
        private readonly IRepository<GameOfLifeBoard> _repository;
        public GameOfLifeService(IRepository<GameOfLifeBoard> repository) 
        {
            _repository = repository;
        }

        public async Task<GameOfLifeBoard> GetGameOfLifeBoardAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _repository.GetByIdAsync(id, cancellationToken);
        }
    }
}
