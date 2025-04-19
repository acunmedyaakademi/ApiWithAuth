using ApiWithAuth.Models.Dtos.User;

namespace ApiWithAuth.Models.Dtos.Comment;

public class CommentDto
{
    public int Id { get; set; }
    public string Body { get; set; }
    public UserDto User { get; set; }
    public DateTime Created { get; set; }
}