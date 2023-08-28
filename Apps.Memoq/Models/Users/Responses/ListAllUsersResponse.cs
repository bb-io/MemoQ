using Apps.Memoq.Models.Dto;

namespace Apps.Memoq.Models.Users.Responses;

public class ListAllUsersResponse
{
    public UserDto[] Users { get; set; }
}