#region Usings

using TokenBased_Authentication.Infrastructure.Repositories.User;
using Microsoft.Extensions.DependencyInjection;
using TokenBased_Authentication.Application.Common.IUnitOfWork;
using TokenBased_Authentication.Domain.IRepositories.User;
using TokenBased_Authentication.Infrastructure.UnitOfWork;

namespace TokenBased_Authentication.IoC;

#endregion

public static class DependencyContainer
{
    public static void ConfigureDependencies(IServiceCollection services)
    {
        #region Repositories

        //User
        services.AddScoped<IUserCommandRepository, UserCommandRepository>();
        services.AddScoped<IUserQueryRepository, UserQueryRepository>();

        #endregion

        #region Unit Of Work 

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        #endregion
    }
}
