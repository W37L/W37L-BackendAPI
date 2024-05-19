using System.Security.Claims;
using System.Text.Encodings.Web;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

///<summary>
/// Extension methods for adding Firebase authentication to the ASP.NET Core application.
///</summary>
public static class FirebaseAuthenticationExtensions {
    
    ///<summary>
    /// Adds Firebase authentication to the ASP.NET Core application.
    ///</summary>
    ///<param name="services">The collection of services to add the authentication to.</param>
    ///<param name="firebaseProjectId">The Firebase project ID.</param>
    ///<returns>The AuthenticationBuilder to continue configuring authentication.</returns>
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

///<summary>
/// Authentication handler for Firebase authentication.
///</summary>
public class FirebaseAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions> {
    
    ///<summary>
    /// Initializes a new instance of the FirebaseAuthenticationHandler class.
    ///</summary>
    ///<param name="options">The monitor for the options.</param>
    ///<param name="logger">The logger factory.</param>
    ///<param name="encoder">The URL encoder.</param>
    ///<param name="clock">The system clock.</param>
    public FirebaseAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger,
        UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock) { }

    ///<summary>
    /// Handles authentication asynchronously.
    ///</summary>
    ///<returns>An AuthenticateResult indicating the outcome of the authentication process.</returns>
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