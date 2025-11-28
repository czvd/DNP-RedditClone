using ApiContracts.Dtos.CommentDtos;

namespace ApiContracts.Dtos.PostDtos;

public record PostWithCommentsDto(PostWithUsername Post, List<CommentUsername> Comments);