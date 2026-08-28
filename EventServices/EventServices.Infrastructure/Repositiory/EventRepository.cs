using EventServices.Application.IRepository;
using EventServices.Domain.Entities;
using EventServices.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventServices.Infrastructure.Repositiory
{
    public class EventRepository : IEventRepository
    {
        private readonly EventDbContext _dbContext;

        public EventRepository(EventDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task AddAsync(Event eventObj)
        {
            await _dbContext.Events.AddAsync(eventObj);
        }

        public async Task<List<Event>> GetAllAsync()
        {
            return await _dbContext.Events.Where(e => e.IsActive).ToListAsync();
        }

        public async Task<Event?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Events.FindAsync(id);
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
