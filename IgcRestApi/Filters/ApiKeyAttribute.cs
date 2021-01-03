using Microsoft.AspNetCore.Mvc;
using System;

namespace IgcRestApi.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAttribute : TypeFilterAttribute
    {
        public ApiKeyAttribute(string realm = @"My Realm") : base(typeof(ApiKeyFilter))
        {
            Arguments = new object[] { realm };
        }
    }
}
