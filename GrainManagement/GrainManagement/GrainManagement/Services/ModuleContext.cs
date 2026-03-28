using Microsoft.Extensions.Options;

namespace GrainManagement.Services;

/// <summary>
/// Injectable service that lets controllers, views, and tag helpers
/// check whether a module is enabled for this deployment.
/// </summary>
public interface IModuleContext
{
    bool IsEnabled(string moduleName);
    string DeploymentMode { get; }
    bool IsCentral { get; }
    bool IsRemote { get; }
    ModuleOptions Options { get; }
}

public sealed class ModuleContext : IModuleContext
{
    private readonly ModuleOptions _options;

    public ModuleContext(IOptions<ModuleOptions> options)
    {
        _options = options.Value;
    }

    public bool IsEnabled(string moduleName) => _options.IsEnabled(moduleName);
    public string DeploymentMode => _options.DeploymentMode;
    public bool IsCentral => _options.DeploymentMode.Equals("Central", StringComparison.OrdinalIgnoreCase);
    public bool IsRemote => _options.DeploymentMode.Equals("Remote", StringComparison.OrdinalIgnoreCase);
    public ModuleOptions Options => _options;
}
