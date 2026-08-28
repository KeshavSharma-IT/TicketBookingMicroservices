using EventServices.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventServices.Application.IRepository
{
    public interface IVenueRepository
    {
        Task AddAsync(Venue venue);
        Task<List<Venue>> GetAllAsync();
        Task<List<Venue>> GetByCityAsync(string city);
        Task<Venue?> GetByIdAsync(Guid id);
        Task SaveChangesAsync();
    }

}
