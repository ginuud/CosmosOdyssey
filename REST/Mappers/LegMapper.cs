using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CosmosOdyssey.REST.Dtos;
using CosmosOdyssey.REST.Models;

namespace REST.Mappers
{
    public static class LegMapper
    {
        public static LegDto ToLegDto(this Leg legModel)
        {
            return new LegDto
            {
                Id = legModel.Id,
                PriceListId = legModel.PriceListId,
                Providers = legModel.Providers.Select(p => p.ToProviderDto()).ToList()
            };
        }

    }
}