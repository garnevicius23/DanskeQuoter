using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using QuoterApp.Services;
using System;
using System.Threading.Tasks;

namespace QuoterApp.Endpoints
{
    public class QuoterEndpoints : IEndpoints
    {
        public void RegisterEndpoints(IEndpointRouteBuilder routes)
        {
            routes.MapGet("/quote/{instrumentId}/{quantity}", GetQuote)
                .Produces<double>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            routes.MapGet("/vwap/{instrumentId}", GetVwap)
                .Produces<double>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);
        }

        public async Task<IResult> GetQuote(IQuoter quoter, string instrumentId, int quantity)
        {
            try
            {
                return Results.Ok(quoter.GetQuote(instrumentId, quantity));
            }
            catch (ArgumentException ex)
            {
                return Results.NotFound(ex.Message);
            }
        }

        public async Task<IResult> GetVwap(IQuoter quoter, string instrumentId)
        {
            try
            {
                return Results.Ok(quoter.GetVolumeWeightedAveragePrice(instrumentId));
            }
            catch (ArgumentException ex)
            {
                return Results.NotFound(ex.Message);
            }
        }
    }
}
