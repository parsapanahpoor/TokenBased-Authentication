#region Usings

using TokenBased_Authentication.Infrastructure.Repositories.User;
using Microsoft.Extensions.DependencyInjection;
using TokenBased_Authentication.Application.Common.IUnitOfWork;
using TokenBased_Authentication.Domain.IRepositories.User;
using TokenBased_Authentication.Infrastructure.UnitOfWork;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using TokenBased_Authentication.Domain.IRepositories.Role;
using TokenBased_Authentication.Infrastructure.Repositories.Role;

namespace TokenBased_Authentication.IoC;

#endregion

public static class API_DependencyContainer
{
    public static void ConfigureDependencies(IServiceCollection services)
    {
        //User
        services.AddScoped<IUserCommandRepository, UserCommandRepository>();
        services.AddScoped<IUserQueryRepository, UserQueryRepository>();

        //Role
        services.AddScoped<IRoleCommandRepository, RoleCommandRepository>();
        services.AddScoped<IRoleQueryRepository, RoleQueryRepository>();

        //Unit Of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
