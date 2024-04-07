using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using QuoterApp.Services;
using System;

namespace QuoterApp.Endpoints
{
    public class QuoterEndpoints : IEndpoints
    {
        public void RegisterEndpoints(IEndpointRouteBuilder routes)
        {
            routes.MapGet("/quote", GetQuote)
                .Produces<double>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound);

            routes.MapGet("/vwap", GetVwap)
                .Produces<double>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound);
        }

        public IResult GetQuote(IQuoter quoter, string instrumentId, int quantity)
        {
            try
            {
                if (string.IsNullOrEmpty(instrumentId))
                    return Results.BadRequest("Instrument ID can't be null.");

                return Results.Ok(quoter.GetQuote(instrumentId, quantity));
            }
            catch (ArgumentException ex)
            {
                return Results.NotFound(ex.Message);
            }
            catch(BadHttpRequestException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }

        public IResult GetVwap(IQuoter quoter, string instrumentId)
        {
            try
            {
                if (string.IsNullOrEmpty(instrumentId))
                    return Results.BadRequest("Instrument ID can't be null.");

                return Results.Ok(quoter.GetVolumeWeightedAveragePrice(instrumentId));
            }
            catch (ArgumentException ex)
            {
                return Results.NotFound(ex.Message);
            }
        }
    }
}
