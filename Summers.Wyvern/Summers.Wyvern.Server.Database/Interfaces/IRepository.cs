﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Summers.Aelin.Server.Database.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        void Add(TEntity obj);
        Task<TEntity> GetById(Guid id);
        Task<long> CountAll();
        Task<IEnumerable<TEntity>> GetAll();
        void Update(TEntity obj);
        void Remove(Guid id);
    }
}