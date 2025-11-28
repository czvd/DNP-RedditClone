namespace ApiContracts.Dtos.CommentDtos;

public class CommentDto
{
    public int Id { get; set; }
    public string Body { get; set; } = "";
    public int PostId { get; set; }
    public int UserId { get; set; }
}