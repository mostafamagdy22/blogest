using blogest.domain.Entities;

namespace blogest.application.DTOs.responses;

public record GetPostResponse(List<CommentDto> Comments,string Content,string Title,DateTime PublishAt,string Publisher);
