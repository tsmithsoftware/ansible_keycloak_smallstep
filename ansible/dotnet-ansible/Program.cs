using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
services.AddControllersWithViews();

services.AddAuthentication(options =>
{
    //Sets cookie authentication scheme
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie(cookie =>
{
    //Sets the cookie name and maxage, so the cookie is invalidated.
    cookie.Cookie.Name = "keycloak.cookie";
    cookie.Cookie.MaxAge = TimeSpan.FromMinutes(60);
    cookie.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    cookie.SlidingExpiration = true;
})
.AddOpenIdConnect(options =>
{
    //Use default signin scheme
    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    //Keycloak server
    //options.Authority = Configuration.GetSection("Keycloak")["ServerRealm"];
    options.Authority = "https://keycloak:8443";
    //Keycloak client ID
    //options.ClientId = Configuration.GetSection("Keycloak")["ClientId"];
    options.ClientId = "aspnet_app";
    //Keycloak client secret
    //options.ClientSecret = Configuration.GetSection("Keycloak")["ClientSecret"];
    options.ClientSecret = "gkthF7q6q92PkUC6QsbTzCKGTONgBi2H";

    //Keycloak .wellknown config origin to fetch config
    // options.MetadataAddress = Configuration.GetSection("Keycloak")["Metadata"];
    //Require keycloak to use SSL
    options.RequireHttpsMetadata = false;
    options.GetClaimsFromUserInfoEndpoint = true;
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("email");

    //Save the token
    options.SaveTokens = true;
    //Token response type, will sometimes need to be changed to IdToken, depending on config.
    options.ResponseType = OpenIdConnectResponseType.Code;
    //SameSite is needed for Chrome/Firefox, as they will give http error 500 back, if not set to unspecified.
    options.NonceCookie.SameSite = SameSiteMode.None;
    options.CorrelationCookie.SameSite = SameSiteMode.None;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = "name",
        RoleClaimType = "https://schemas.scopic.com/roles"
    };

    //Configuration.Bind("<Json Config Filter>", options);
    options.Events.OnRedirectToIdentityProvider = async context =>
    {
        context.ProtocolMessage.RedirectUri = "https://localhost:5001/home";
        await Task.FromResult(0);
    };

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
