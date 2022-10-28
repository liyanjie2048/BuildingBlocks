namespace Liyanjie.AspNetCore.Authentication.Code;

/// <summary>
/// 
/// </summary>
public class CodeAuthenticationHandler : AuthenticationHandler<CodeAuthenticationOptions>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    /// <param name="logger"></param>
    /// <param name="encoder"></param>
    /// <param name="clock"></param>
    public CodeAuthenticationHandler(
        IOptionsMonitor<CodeAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock)
    { }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        await Task.CompletedTask;

        if (Context.Request.Headers.TryGetValue("Authorization", out var code) && code[0] == $"Code {Options.ValidCode}")
        {
            return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "_"),
            }, CodeAuthenticationDefaults.AuthenticationScheme)), CodeAuthenticationDefaults.AuthenticationScheme));
        }

        return AuthenticateResult.Fail("Code Authorization Fail");
    }
}
