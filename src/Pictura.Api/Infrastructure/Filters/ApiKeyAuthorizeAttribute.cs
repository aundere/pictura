using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Pictura.Api.Infrastructure.Options;

namespace Pictura.Api.Infrastructure.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private const string HeaderName = "X-Api-Key";
        
        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var options = context.HttpContext.RequestServices.GetRequiredService<IOptions<ApiAuthOptions>>().Value;

            if (options.AuthEndpoints == ApiAuthOptions.RequiredAuthEndpoints.None)
            {
                return Task.CompletedTask;
            }
            
            var isModifying = context.HttpContext.Request.Method is "POST" or "PUT" or "PATCH" or "DELETE";
            
            var isAuthorizedEndpoint =
                (isModifying && options.AuthEndpoints == ApiAuthOptions.RequiredAuthEndpoints.Modifying) ||
                options.AuthEndpoints == ApiAuthOptions.RequiredAuthEndpoints.All;
            
            var isHeaderMatches = context.HttpContext.Request.Headers[HeaderName] == options.SecretKey;

            if (!isHeaderMatches && isAuthorizedEndpoint)
            {
                context.Result = new UnauthorizedResult();
            }
            
            return Task.CompletedTask;
        }
    }
}
