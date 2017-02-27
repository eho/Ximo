using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ximo.DependencyInjection
{
    /// <summary>
    ///     Represents an application module. The module class is responsible for registering the correct service definitions
    ///     and their implementation in each application module.
    /// </summary>
    public interface IModule
    {
        /// <summary>
        ///     Sets the configuration required for initializing the module.
        /// </summary>
        IConfiguration Configuration { set; }

        /// <summary>
        ///     Initializes the current module instance into a specified service collection.
        /// </summary>
        /// <param name="serviceCollection">The service collection used to register services.</param>
        void Initialize(IServiceCollection serviceCollection);
    }
}