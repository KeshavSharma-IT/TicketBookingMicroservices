using EventServices.Application.IRepository;
using EventServices.Domain.Entities;
using EventServices.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventServices.Infrastructure.Repositiory
{
    public class ShowRepository : IShowRepository
    {
        private readonly EventDbContext _dbContext;
        public ShowRepository(EventDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Show show)
        {
            await _dbContext.Shows.AddAsync(show);
        }

        public async Task<List<Show>> GetAllAsync()
        {
            return await _dbContext.Shows.Where(s => s.IsActive).ToListAsync();
        }

        public async Task<List<Show>> GetShowsByEventAndCityAsync(Guid eventid, string city)
        {
            return await _dbContext.Shows
                .Include(s => s.Screen)
                    .ThenInclude(src => src.venue)
                .Where(s => s.EventId == eventid && s.Screen.venue.City == city)
                .ToListAsync();
        }

        public async Task<Show?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Shows.FindAsync(id);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
