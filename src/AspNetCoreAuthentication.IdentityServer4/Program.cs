var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddAuthentication(o =>
{
    o.DefaultScheme = "cookie";
    o.DefaultChallengeScheme = "openid";
})
    .AddCookie(authenticationScheme: "cookie", o =>
    {
        o.Cookie.Name = "openid-cookie";
        o.ExpireTimeSpan = TimeSpan.FromHours(1);
        o.LoginPath = "/account/login";
        o.AccessDeniedPath = "/account/accessdenied";
    })
    .AddOpenIdConnect("openid", o =>
    {
        o.Authority = $"{builder.Configuration["Okta:Origin"]}/oauth2/default"; //Issuer
        o.ClientId = builder.Configuration["Okta:ClientId"];
        o.ClientSecret = builder.Configuration["Okta:ClientSecret"];

        o.CallbackPath = "/signin/openid";
        o.SignedOutRedirectUri = "/signout/openid";

        o.ResponseType = "code";

        o.Scope.Clear();
        o.Scope.Add("openid");
        o.Scope.Add("profile");
        o.Scope.Add("email");
        o.GetClaimsFromUserInfoEndpoint = true;

        //o.ClaimActions.Clear(); //these are the filter actions, that filter the claims by default by microsoft. Clearing it, will remove those filters.

        o.SaveTokens = false; //OpenIdConnect authorization handler will collect all tokens that will produced during auth process and store them in items->metadata.

        o.MapInboundClaims = false; //it cause that we don't get the claims like http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier
        //but just the raw claim name
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(name: "ManageUsers", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim(claimType: "role", allowedValues: "Admin");
    });
});

var app = builder.Build();

//Configure pipeline with registering Middlewares like Authentication or Autherication.
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages().RequireAuthorization();

app.Run();
