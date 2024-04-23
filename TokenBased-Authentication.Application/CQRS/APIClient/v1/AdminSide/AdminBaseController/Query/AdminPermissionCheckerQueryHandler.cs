
using TokenBased_Authentication.Domain.IRepositories.Role;

namespace TokenBased_Authentication.Application.CQRS.APIClient.v1.AdminSide.AdminBaseController.Query;

public record AdminPermissionCheckerQueryHandler : IRequestHandler<AdminPermissionCheckerQuery, bool>
{
    #region Ctor 

    private readonly IRoleQueryRepository _roleQueryRepository;

    public AdminPermissionCheckerQueryHandler(IRoleQueryRepository roleQueryRepository)
    {
        _roleQueryRepository = roleQueryRepository;
    }

    #endregion

    public async Task<bool> Handle(AdminPermissionCheckerQuery request, CancellationToken cancellationToken)
    {
        var isUserAdmin = await _roleQueryRepository.IsUser_Admin(request.UserId , cancellationToken);
        if (isUserAdmin == false) return false;

        return true;
    }
}
