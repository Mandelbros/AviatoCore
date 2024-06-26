﻿using AviatoCore.Application.Interfaces;
using AviatoCore.Domain.Entities;
using AviatoCore.Infrastructure.Interfaces;
using AviatoCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AviatoCore.Application.Services
{
    public class FacilityService : IFacilityService
    {
        private readonly IFacilityRepository _facilityRepository;

        public FacilityService(IFacilityRepository facilityRepository)
        {
            _facilityRepository = facilityRepository;
        }

        public async Task<Facility> GetFacilityAsync(int id)
        {
            return await _facilityRepository.GetFacilityAsync(id); 
        }

        public async Task<IEnumerable<Facility>> GetFacilitiesByAirportIdAsync(int airportId)
        {
            return await _facilityRepository.GetFacilitiesByAirportIdAsync(airportId);
        }

        public async Task<IEnumerable<FacilityDto>> GetFacilitiesByAirportIdWithFacTypeAsync(int airportId)
        {
            var allFacilities = await _facilityRepository.GetFacilitiesByAirportIdWithFacTypeAsync();

            var facilities = allFacilities
                .Where(f => f.AirportId == airportId && f.IsDeleted==false)
                .Select(f => new FacilityDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    Description=f.Description,
                    Address = f.Address,
                    ImgUrl = f.ImgUrl,
                    AirportId = f.AirportId,
                    FacilityType = f.FacilityType.Name,
                    IsDeleted = f.IsDeleted
                });
            return facilities;
        }

        public async Task AddFacilityAsync(Facility facility)
        {
             
            await _facilityRepository.AddFacilityAsync(facility);
        }

        public async Task UpdateFacilityAsync(Facility facility)
        {
            await _facilityRepository.UpdateFacilityAsync(facility);
        }

        public async Task DeleteFacilityAsync(int id)
        {
            await _facilityRepository.DeleteFacilityAsync(id);
        }
    }
}
