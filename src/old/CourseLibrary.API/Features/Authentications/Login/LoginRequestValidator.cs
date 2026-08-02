namespace CourseLibrary.API.Features.Authentications.Login;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(author => author.Email).NotNull().NotEmpty().Length(8, 50).EmailAddress();
        RuleFor(author => author.Password).NotNull().NotEmpty().Length(8, 50);
    }
}
