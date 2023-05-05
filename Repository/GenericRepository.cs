﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using BankListAPI.VsCode.Contracts;
using BankListAPI.VsCode.Data;
using BankListAPI.VsCode.Models;
using Microsoft.EntityFrameworkCore;

namespace BankListAPI.VsCode.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T:class
    {
        private readonly BankListDbContext _context;
        private readonly IMapper _mapper;

        public GenericRepository(BankListDbContext context, IMapper mapper) { 
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetAsync(id);
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Exists(int id)
        {
            var entity = await GetAsync(id);
            return entity != null;
        }

        public async Task<List<T>> GetAllAsync()
        {
           return await _context.Set<T>().ToListAsync();
        }

        public async Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters)
        {
            var totalSize = await _context.Set<T>().CountAsync();
            var items = await _context.Set<T>()
                                .Skip(queryParameters.StartIndex)
                                .Take(queryParameters.PageSize)
                                .ProjectTo<TResult>(_mapper.ConfigurationProvider)  //exact column that should take
                                .ToListAsync();
            return new PagedResult<TResult> {
                Items = items,
                PageNumber = queryParameters.PageNumber,
                RecordNumber= queryParameters.PageSize,
                TotalCount= totalSize
            };
        }

        public async Task<T> GetAsync(int? id)
        {
            if(id is null)
            {
                return null;
            }
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _context.Update(entity);
           await _context.SaveChangesAsync();
           return entity;
        }
    }
}
