using Core.Entities;

namespace Core.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IUserRepository UserRepository { get; }
    IVotesRepository VotesRepository { get; }
    IVotingReporitory VotingReporitory { get; }
    INameRepository NameRepository { get; }
    IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
    Task<bool> Complete();
    bool HasChanges();
    void Dispose();
}