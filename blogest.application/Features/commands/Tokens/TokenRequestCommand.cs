using MediatR;

namespace blogest.application.Features.commands.Tokens;
public record TokenRequestCommand(string refreshToken) : IRequest<RefreshTokenResponse>;
