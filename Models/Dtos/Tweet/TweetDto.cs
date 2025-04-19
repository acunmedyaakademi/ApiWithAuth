using ApiWithAuth.Models.Dtos.User;

namespace ApiWithAuth.Models.Dtos.Tweet;

public class TweetDto
{
    public int Id { get; set; }
    public string Body { get; set; }
    public UserDto User { get; set; }
    public DateTime Created { get; set; }
}