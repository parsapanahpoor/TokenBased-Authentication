using TokenBased_Authentication.Domain.DTO.AdminSide.Role;
using TokenBased_Authentication.Domain.IRepositories.Role;

namespace TokenBased_Authentication.Application.CQRS.APIClient.v1.AdminSide.Role.Query;

public record FilterRolesQueryHandler : IRequestHandler<FilterRolesQuery, FilterRolesDTO>
{
    #region Ctor

    private readonly IRoleQueryRepository _roleQueryRepository;

    public FilterRolesQueryHandler(IRoleQueryRepository roleQueryRepository)
    {
        _roleQueryRepository = roleQueryRepository;
    }

    #endregion

    public async Task<FilterRolesDTO> Handle(FilterRolesQuery request, CancellationToken cancellationToken)
    {
        return await _roleQueryRepository.FilterRoles(new FilterRolesDTO()
        {
            RoleTitle = request.RoleTitle,
        },
        cancellationToken
        );
    }
}
