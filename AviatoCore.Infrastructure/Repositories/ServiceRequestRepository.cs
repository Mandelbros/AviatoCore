﻿using AviatoCore.Domain.Entities;
using AviatoCore.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AviatoCore.Infrastructure.Repositories
{
    public class ServiceRequestRepository : IServiceRequestRepository
    {
        private readonly AviatoDbContext _context;

        public ServiceRequestRepository(AviatoDbContext context)
        {
            _context = context;
        }
          
        public async Task AddServiceRequestAsync(ServiceRequest serviceRequest)
        {
            await _context.Set<ServiceRequest>().AddAsync(serviceRequest);
            await _context.SaveChangesAsync();
        }

        public async Task<ServiceRequest> GetServiceRequestAsync(int id)
        {
            var serviceRequest = await _context.Set<ServiceRequest>().FindAsync(id);

            if (serviceRequest == null)
            {
                throw new KeyNotFoundException("Service Request not found");
            }

            return serviceRequest;
        }

        public async Task<IEnumerable<ServiceRequest>> GetAllServiceRequestsAsync()
        {
            
            return await _context.Set<ServiceRequest>().ToListAsync();
           
        }
    }
}