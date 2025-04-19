using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CosmosOdyssey.REST.Dtos;
using CosmosOdyssey.REST.Models;

namespace REST.Mappers
{
    public static class ProviderMapper
    {
        public static ProviderDto ToProviderDto(this Provider providerModel)
        {
            return new ProviderDto
            {
                Id = providerModel.Id,
                CompanyId = providerModel.CompanyId,
                CompanyName = providerModel.Company.Name,
                Price = providerModel.Price,
                FlightStart = providerModel.FlightStart,
                FlightEnd = providerModel.FlightEnd,
                //LegId = providerModel.LegId
            };
        }

    }
}