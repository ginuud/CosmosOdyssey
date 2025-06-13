using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CosmosOdyssey.REST.Data;
using CosmosOdyssey.REST.Dtos;
using CosmosOdyssey.REST.Models;
using Microsoft.EntityFrameworkCore;

namespace REST.Mappers
{
    public static class RouteInfoMapper
    {
        public static RouteInfoDto ToRouteInfoDto(this RouteInfo routeInfoModel)
        {
            return new RouteInfoDto
            {
                Id = routeInfoModel.Id,
                From = routeInfoModel.From,
                To = routeInfoModel.To,
                Distance = routeInfoModel.Distance
            };
        }

    }
}