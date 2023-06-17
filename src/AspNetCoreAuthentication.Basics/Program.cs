var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

//registering the Authentication with default scheme. Each type of Authentication has to have its own unique scheme name.
builder.Services.AddAuthentication(defaultScheme: "cookie")
    .AddCookie(authenticationScheme: "cookie", o =>
    {
        o.Cookie.Name = "demo";
        o.ExpireTimeSpan = TimeSpan.FromHours(1);

        o.LoginPath = "/account/login";
        o.LogoutPath = "/account/logout";
        //Challenge is like when you try to do something and you aren't authorized, the application
        //should redirect you to autorize. That is the LoginPath parameter. It's the path to log-in page.
        //After redirection, Identity adds a parameter "ReturnUrl", to the login page, to get back to the prev page,
        //after successfully log-in operation.
        o.AccessDeniedPath = "/account/accessdenied";
    })
    .AddCookie("cookie-google-tmp")
    .AddGoogle(authenticationScheme: "google", o =>
    {
        o.ClientId = builder.Configuration["Google:ClientId"] ?? throw new ArgumentNullException(nameof(o.ClientId));
        o.ClientSecret = builder.Configuration["Google:ClientSecret"] ?? throw new ArgumentNullException(nameof(o.ClientSecret));

        //signin-google is the default callback path on which the google will send the response.
        //o.CallbackPath = "/signin-google;

        //because the defaultScheme is cookie, we don't have to set it now.
        //this is the part where we set the session management handler - "cookie".
        //o.SignInScheme = "cookie";
        o.SignInScheme = "cookie-google-tmp";

        //o.Events = new OAuthEvents
        //{
        //    OnCreatingTicket = e =>
        //    {
        //        e.Principal =
        //    }
        //};
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(name: "ManageUsers", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim(claimType: "role", allowedValues: "admin");
        policy.RequireClaim(claimType: "name", allowedValues: "rolando");
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

app.MapRazorPages()
    //.RequireAuthorization(policyNames: "ManageUsers") //To set that every page requires from Users to be authorized with ManageUsers policy
    .RequireAuthorization(); //It tells us, that every page has to be authorized, but if you want to restrict the scope, i.e. for ManageUsers Policy
                             //you have to set it in parameter of Attribute [Authorize("ManageUsers")]

app.Run();
