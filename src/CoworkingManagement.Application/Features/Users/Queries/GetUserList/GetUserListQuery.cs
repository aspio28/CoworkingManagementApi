using System.Text.Json.Serialization;
using CoworkingManagement.Application.Common.Interfaces;
using CoworkingManagement.Application.Common.Models;
using MediatR;

namespace CoworkingManagement.Application.Features.Users.Commands.GetUserList;

public record GetUserListQuery(
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<PaginatedList<UserDto>>, ICacheableQuery
{
    [JsonIgnore]
    public string CacheKey => $"GetUsers_{PageNumber}_{PageSize}";

    [JsonIgnore]
    public string CacheTag => "Users";

    [JsonIgnore]
    public TimeSpan? Expiration => TimeSpan.FromMinutes(10);
}