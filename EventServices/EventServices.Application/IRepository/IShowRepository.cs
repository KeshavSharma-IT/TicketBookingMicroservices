using EventServices.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventServices.Application.IRepository
{
    public interface IShowRepository
    {
        Task AddAsync(Show show);
        Task<List<Show>> GetAllAsync();
        Task<List<Show>> GetShowsByEventAndCityAsync(Guid eventid, string city);
        Task<Show?> GetByIdAsync(Guid id);
        Task SaveChangesAsync();
    }
}
