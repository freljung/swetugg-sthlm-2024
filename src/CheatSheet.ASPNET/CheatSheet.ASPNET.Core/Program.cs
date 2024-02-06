using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);


// A01 Broken Access Control - Cookie settings
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = false;
    });


// A01 Broken Access Control Enforce Authorization - Authorization policy
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    options.AddPolicy("AwesomeUser", new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .RequireRole("User")
        .RequireClaim("AwesomeClaim")
        .Build());
});



// Does not add AntiForgery support
builder.Services.AddControllers();

// Calls services.AddAntiforgery(); by AddControllersWithViewsCore() -> AddViews() -> AddViewServices()
builder.Services.AddControllersWithViews();

// Identification and Authentication Failures
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.Expiration = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true;
});

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredUniqueChars = 6;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    options.Lockout.MaxFailedAccessAttempts = 3;

    options.SignIn.RequireConfirmedEmail = true;

    options.User.RequireUniqueEmail = true;
});


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHsts();
app.UseHttpsRedirection();

// Listed in the Cheat sheet, but comes from a separate NuGet package
// that hasn't been updated since 2020-02-12
// Repo available at https://github.com/NWebsec/NWebsec/tree/dev
//app.UseXContentTypeOptions();
//app.UseReferrerPolicy(opts => opts.NoReferrer());
//app.UseXXssProtection(options => options.FilterDisabled());
//app.UseXfo(options => options.Deny());

//app.UseCsp(opts => opts
// .BlockAllMixedContent()
// .StyleSources(s => s.Self())
// .StyleSources(s => s.UnsafeInline())
// .FontSources(s => s.Self())
// .FormActions(s => s.Self())
// .FrameAncestors(s => s.Self())
// .ImageSources(s => s.Self())
// .ScriptSources(s => s.Self())
// );


// A01 Broken Access Control - Cookie settings
app.UseAuthorization()
    .UseCookiePolicy(new CookiePolicyOptions
    {
        HttpOnly = HttpOnlyPolicy.Always,
        Secure = builder.Environment.IsDevelopment() ? CookieSecurePolicy.SameAsRequest
                                                     : CookieSecurePolicy.Always,
    });


app.MapControllers();

app.Run();