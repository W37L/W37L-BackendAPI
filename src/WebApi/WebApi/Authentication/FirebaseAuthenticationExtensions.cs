using System.Security.Claims;
using System.Text.Encodings.Web;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

public static class FirebaseAuthenticationExtensions {
    public static AuthenticationBuilder AddFirebaseAuthentication(this IServiceCollection services,
        string firebaseProjectId) {
        FirebaseApp.Create(new AppOptions {
            Credential = GoogleCredential.GetApplicationDefault(),
            ProjectId = firebaseProjectId
        });

        services
            .AddAuthentication("Firebase")
            .AddScheme<AuthenticationSchemeOptions, FirebaseAuthenticationHandler>("Firebase", null);

        return services.AddAuthentication();
    }
}

public class FirebaseAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions> {
    public FirebaseAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger,
        UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock) { }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync() {
        if (!Request.Headers.ContainsKey("Authorization"))
            return AuthenticateResult.Fail("No Authorization Header");

        string authorizationHeader = Request.Headers["Authorization"];
        if (string.IsNullOrEmpty(authorizationHeader))
            return AuthenticateResult.NoResult();

        if (!authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            return AuthenticateResult.Fail("Invalid Authorization Header");

        var token = authorizationHeader.Substring("Bearer ".Length).Trim();
        if (string.IsNullOrEmpty(token))
            return AuthenticateResult.Fail("Invalid Token");

        FirebaseToken decodedToken;
        try {
            decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
        }
        catch (FirebaseAuthException) {
            return AuthenticateResult.Fail("Invalid Token");
        }

        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, decodedToken.Uid) };
        var identity = new ClaimsIdentity(claims, nameof(FirebaseAuthenticationHandler));
        var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}