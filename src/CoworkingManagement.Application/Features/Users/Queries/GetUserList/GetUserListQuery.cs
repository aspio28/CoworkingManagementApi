using CoworkingManagement.Application.Common.Models;
using MediatR;

namespace CoworkingManagement.Application.Features.Users.Commands.GetUserList;

public record GetUserListQuery(
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<PaginatedList<UserDto>>;