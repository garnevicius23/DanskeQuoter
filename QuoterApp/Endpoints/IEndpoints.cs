using Microsoft.AspNetCore.Routing;

namespace QuoterApp.Endpoints
{
    public interface IEndpoints
    {
        void RegisterEndpoints(IEndpointRouteBuilder routes);
    }
}
