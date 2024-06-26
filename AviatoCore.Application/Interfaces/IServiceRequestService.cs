﻿using AviatoCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AviatoCore.Application.Interfaces
{
    public interface IServiceRequestService
    {
        Task<ServiceRequest> GetServiceRequestAsync(int id); // New method
        Task AddServiceRequestAsync(ServiceRequest serviceRequest);
        Task<IEnumerable<ServiceRequest>> GetServiceRequestsByAirportIdAsync(int airportId);
    }
}
