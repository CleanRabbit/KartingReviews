using System;
using System.Threading.Tasks;

namespace Summers.Aelin.Server.Database.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> Commit();
    }
}