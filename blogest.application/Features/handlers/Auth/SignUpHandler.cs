using AutoMapper;
using blogest.application.Interfaces.services;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace blogest.application.Features.handlers.Auth
{
    public class SignUpHandler : IRequestHandler<SignUpCommand, SignUpResponseDto>
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        public SignUpHandler(IUsersRepository usersRepository, IAuthService authService, IMapper mapper)
        {
            _usersRepository = usersRepository;
            _authService = authService;
            _mapper = mapper;
        }
        public async Task<SignUpResponseDto> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            var failures = new List<ValidationFailure>();
            if (await _usersRepository.IsEmailExit(request.Email))
                failures.Add(new ValidationFailure("Email", "Email is already using!"));
            if (await _usersRepository.IsUserNameExit(request.UserName))
                failures.Add(new ValidationFailure("UserName", "User name is already taken!"));

            if (failures.Count > 0)
                throw new ValidationException(failures);

            User user = _mapper.Map<User>(request);

            return await _authService.SignUp(user);
        }
    }
}