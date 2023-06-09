﻿using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    private readonly DataBaseContext _dataBaseContext;

    public GenericRepository(DataBaseContext dataBaseContext)
    {
        _dataBaseContext = dataBaseContext;
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _dataBaseContext.Set<T>().FindAsync(id);
    }
    

    public async Task<IReadOnlyList<T>> ListAllAsync()
    {
        return await _dataBaseContext.Set<T>().ToListAsync();
    }

    public async Task<T> GetEntityWithSpec(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).ToListAsync();
    }

    public async Task<int> CountAsync(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).CountAsync();
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
        return SpecificationEvaluator<T>.GetQuery(_dataBaseContext.Set<T>().AsQueryable(), spec);
    }

    public void Add(T entity)
    {
        _dataBaseContext.Set<T>().Add(entity);
    }

    public void Update(T entity)
    {
        _dataBaseContext.Set<T>().Attach(entity);
        _dataBaseContext.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(T entity)
    {
        _dataBaseContext.Set<T>().Remove(entity);
    }
}