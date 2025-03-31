using FastEndpoints;

namespace CourseLibrary.API.Features.Authentications.Login;


public sealed class LoginEndpoint : Endpoint<LoginRequest, LoginResponse>
{
    public override void Configure()
    {
        this.Post("api/authentication/login");
        this.Version(1);
        this.AllowAnonymous();
        this.DontThrowIfValidationFails();
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        LoginResponse? response = await req.ExecuteAsync(ct);
        if (response is null)
        {
            await SendUnauthorizedAsync(cancellation: ct);
        }
        else
        {
            await SendAsync(response, cancellation: ct);

        }
       
    }
}
