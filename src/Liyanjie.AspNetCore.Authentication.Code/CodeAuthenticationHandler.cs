namespace Liyanjie.AspNetCore.Authentication.Code;

/// <summary>
/// 
/// </summary>
/// <param name="options"></param>
/// <param name="logger"></param>
/// <param name="encoder"></param>
#if NET6_0
/// <param name="clock"></param>
#endif
public class CodeAuthenticationHandler(
    IOptionsMonitor<CodeAuthenticationOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder
#if NET6_0
   , ISystemClock clock
#endif
    )
    : AuthenticationHandler<CodeAuthenticationOptions>(options, logger, encoder
#if NET6_0
        , clock
#endif
        )
{

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
