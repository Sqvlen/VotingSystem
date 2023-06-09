﻿using System.Collections;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Repositories;

namespace Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataBaseContext _dataBaseContext;
    private readonly IMapper _mapper;
    private Hashtable _repositories;
    
    public UnitOfWork(DataBaseContext dataBaseContext, IMapper mapper)
    {
        _dataBaseContext = dataBaseContext;
        _mapper = mapper;
    }

    public IUserRepository UserRepository => new UserRepository(_dataBaseContext, _mapper);
    public IVotesRepository VotesRepository => new VotesRepository(_dataBaseContext);
    public IVotingReporitory VotingReporitory => new VotingRepository(_dataBaseContext);
    public INameRepository NameRepository => new NameRepository(_dataBaseContext);

    public async Task<bool> Complete()
    {
        return await _dataBaseContext.SaveChangesAsync() > 0;
    }

    public bool HasChanges()
    {
        return _dataBaseContext.ChangeTracker.HasChanges();
    }

    public void Dispose()
    {
        _dataBaseContext.Dispose();
    }

    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
    {
        if(_repositories == null) 
            _repositories = new Hashtable();
    
        var type = typeof(TEntity).Name;
    
        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(GenericRepository<>);
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _dataBaseContext);
    
            _repositories.Add(type, repositoryInstance);
        }
    
        return (IGenericRepository<TEntity>) _repositories[type];
    }
}