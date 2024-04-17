using TokenBased_Authentication.Domain.DTO.Common;
using TokenBased_Authentication.Domain.IRepositories.Role;

namespace TokenBased_Authentication.Application.CQRS.APIClient.v1.AdminSide.Role.Query;


internal class RoleSelectedListQueryHandler : IRequestHandler<RoleSelectedListQuery, List<SelectListViewModel>>
{
    #region Ctor

    private readonly IRoleQueryRepository _roleQueryRepository;

    public RoleSelectedListQueryHandler(IRoleQueryRepository roleQueryRepository)
    {
        _roleQueryRepository = roleQueryRepository;
    }

    #endregion

    public async Task<List<SelectListViewModel>> Handle(RoleSelectedListQuery request, CancellationToken cancellationToken)
    {
        return await _roleQueryRepository.GetSelectRolesList(cancellationToken);
    }
}
