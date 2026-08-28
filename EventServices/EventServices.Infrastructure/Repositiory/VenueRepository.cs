using EventServices.Application.IRepository;
using EventServices.Domain.Entities;
using EventServices.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventServices.Infrastructure.Repositiory
{
    public class VenueRepository : IVenueRepository
    {
        private readonly EventDbContext _dbContext;

        public VenueRepository(EventDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(Venue venue)
        {
            await _dbContext.Venues.AddAsync(venue);
        }

        public async Task<List<Venue>> GetAllAsync()
        {
            return await _dbContext.Venues.ToListAsync();
        }

        public async Task<List<Venue>> GetByCityAsync(string city)
        {
            return await _dbContext.Venues.Where(a => a.City == city).ToListAsync();
        }

        public async Task<Venue?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Venues
                .Include(v => v.Screens)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
