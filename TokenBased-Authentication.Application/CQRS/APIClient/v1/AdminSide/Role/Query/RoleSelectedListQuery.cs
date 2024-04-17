using TokenBased_Authentication.Domain.DTO.Common;

namespace TokenBased_Authentication.Application.CQRS.APIClient.v1.AdminSide.Role.Query;

public record RoleSelectedListQuery : IRequest<List<SelectListViewModel>>
{
}
