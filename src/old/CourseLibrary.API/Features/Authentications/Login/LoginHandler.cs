using CourseLibrary.Authentication;
using CourseLibrary.Domain.Abstraction.Repositories;
using CourseLibrary.Domain.Entities;
using FastEndpoints;

namespace CourseLibrary.API.Features.Authentications.Login;

public class LoginHandler : ICommandHandler<LoginRequest, LoginResponse?>
{
    private readonly IJwtTokenService jwtTokenService;
    private readonly IUserRepository userRepository;
    public LoginHandler(
        IJwtTokenService jwtTokenService,
        IUserRepository userRepository)
    {
        this.jwtTokenService = jwtTokenService;
        this.userRepository = userRepository;
    }
    public async Task<LoginResponse?> ExecuteAsync(LoginRequest command, CancellationToken ct)
    {
        User? user = await this.userRepository.GetUserAsync(command.Email, command.Password);

        if (user is null)
        {
            return null;
        }

        string? token = this.jwtTokenService.GetToken(user);

        if (token is null)
        {
            return null;

        }

        return new LoginResponse(token);
    }
}
