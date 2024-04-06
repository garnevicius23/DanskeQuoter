using Microsoft.AspNetCore.Routing;
using System;
using System.Linq;
using System.Reflection;

namespace QuoterApp.Endpoints
{
    public static class EndpointsExtension
    {
        public static void MapDefinedEndpoints(this IEndpointRouteBuilder routes)
        {
            var currentAssembly = Assembly.GetExecutingAssembly();

            var definedEndpoint = currentAssembly.ExportedTypes
                .Where(x => typeof(IEndpoints).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(Activator.CreateInstance).Cast<IEndpoints>();

            foreach (var e in definedEndpoint)
                e.RegisterEndpoints(routes);
        }
    }
}
