namespace Example.WebApplication.Infrastructure
{
    using System;

    using Example.WebApplication.Services;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class MetricsFilterAttribute : TypeFilterAttribute
    {
        public MetricsFilterAttribute()
            : base(typeof(MetricsActionFilter))
        {
        }

        private class MetricsActionFilter : IActionFilter
        {
            private readonly MetricsManager metricsManager;

            public MetricsActionFilter(MetricsManager metricsManager)
            {
                this.metricsManager = metricsManager;
            }

            public void OnActionExecuting(ActionExecutingContext context)
            {
                metricsManager.Increment();
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {
            }
        }
    }
}
