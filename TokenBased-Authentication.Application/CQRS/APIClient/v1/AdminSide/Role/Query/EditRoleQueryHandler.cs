using TokenBased_Authentication.Domain.DTO.AdminSide.Role;
using TokenBased_Authentication.Domain.IRepositories.Role;

namespace TokenBased_Authentication.Application.CQRS.APIClient.v1.AdminSide.Role.Query;

public record EditRoleQueryHandler : IRequestHandler<EditRoleQuery, EditRoleDTO>
{
    #region Ctor

    private readonly IRoleQueryRepository _roleQueryRepository;

    public EditRoleQueryHandler(IRoleQueryRepository roleQueryRepository)
    {
        _roleQueryRepository = roleQueryRepository;
    }

    #endregion

    public async Task<EditRoleDTO?> Handle(EditRoleQuery request, CancellationToken cancellationToken)
    {
        //get Role By Role 
        var role = await _roleQueryRepository.GetByIdAsync(cancellationToken, request.RoleId);
        if (role == null) return null;

        return new EditRoleDTO()
        {
            Id = role.Id,
            RoleUniqueName = role.RoleUniqueName,
            Title = role.Title,
            Permissions = await _roleQueryRepository.GetRolePermissionsIdByRoleId(role.Id , cancellationToken)
        };
    }
}
