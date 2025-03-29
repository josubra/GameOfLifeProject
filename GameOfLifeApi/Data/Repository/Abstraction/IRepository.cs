﻿using System.Linq.Expressions;

namespace GameOfLifeApi.Data.Repository.Abstraction
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(int id,CancellationToken cancellationToken = default);
    }
}
