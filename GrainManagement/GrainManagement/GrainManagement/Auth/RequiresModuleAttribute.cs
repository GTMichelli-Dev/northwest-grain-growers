using GrainManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace GrainManagement.Auth;

/// <summary>
/// Action/controller filter that returns 404 when the named module is disabled.
/// Usage: [RequiresModule("Seed")] or [RequiresModule("WarehouseIntake")]
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public sealed class RequiresModuleAttribute : Attribute, IFilterFactory
{
    public string ModuleName { get; }
    public bool IsReusable => true;

    public RequiresModuleAttribute(string moduleName)
    {
        ModuleName = moduleName;
    }

    public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
    {
        var options = serviceProvider.GetRequiredService<IOptions<ModuleOptions>>();
        return new RequiresModuleFilter(options.Value, ModuleName);
    }

    private sealed class RequiresModuleFilter : IAuthorizationFilter
    {
        private readonly ModuleOptions _options;
        private readonly string _moduleName;

        public RequiresModuleFilter(ModuleOptions options, string moduleName)
        {
            _options = options;
            _moduleName = moduleName;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!_options.IsEnabled(_moduleName))
            {
                context.Result = new NotFoundResult();
            }
        }
    }
}
