using blogest.application.DTOs.responses;
using MediatR;

namespace blogest.application.Features.commands;
public record TokenRequestCommand(string refreshToken) : IRequest<RefreshTokenResponse>;
