using EventServices.Application.DTO.Event;
using EventServices.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventServices.Application.IRepository
{
    public interface IEventRepository 
    {
        Task AddAsync(Event eventObj);
        Task<List<Event>> GetAllAsync();
        Task<Event?> GetByIdAsync(Guid id);
        Task SaveAsync();
    }
}
