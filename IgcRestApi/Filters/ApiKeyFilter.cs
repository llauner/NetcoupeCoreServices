using IgcRestApi.Exceptions;
using IgcRestApi.Services;
using IgcRestApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace IgcRestApi.Filters
{
    public class ApiKeyFilter : IAuthorizationFilter
    {
        private readonly string _realm;
        private readonly IApiKeyService _apiKeyService;


        public ApiKeyFilter(string realm, IApiKeyService apiKeyService)
        {
            _realm = realm;
            if (string.IsNullOrWhiteSpace(_realm))
            {
                throw new ArgumentNullException(nameof(realm), @"Please provide a non-empty realm value.");
            }

            _apiKeyService = apiKeyService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                var isapiKeyFound = context.HttpContext.Request.Query.ContainsKey("apiKey");
                if (isapiKeyFound)
                {
                    var apiKeyValue = context.HttpContext.Request.Query["apiKey"];

                    if (!string.IsNullOrEmpty(apiKeyValue) && IsAuthorized(context, apiKeyValue))
                    {
                        return;
                    }
                    
                }

                ReturnUnauthorizedResult(context);
            }
            catch (FormatException)
            {
                ReturnUnauthorizedResult(context);
            }
        }

        public bool IsAuthorized(AuthorizationFilterContext context, string apiKeyValue)
        {
            var isApiKeyOK = _apiKeyService.IsAuthorized(apiKeyValue);
            return isApiKeyOK;
        }

        private void ReturnUnauthorizedResult(AuthorizationFilterContext context)
        {
            throw new CoreApiException(System.Net.HttpStatusCode.Unauthorized, "Wrong or invalid apiKey");
        }
    }
}
