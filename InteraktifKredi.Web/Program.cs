using InteraktifKredi.Web.Services;
using InteraktifKredi.Web.Models.Api;

var builder = WebApplication.CreateBuilder(args);

// Configure API Settings from appsettings.json
// This will read ApiSettings section and make it available via IOptions<ApiSettings>
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

// Get API settings for HttpClient configuration
var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>() ?? new ApiSettings();

// Register HttpClient with IApiService (Dependency Injection)
// Keep it simple - no custom headers that might cause issues
builder.Services.AddHttpClient<IApiService, ApiService>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(apiSettings.Timeout);
    // Don't set custom headers - let the API service handle it
});

// Add services to the container.
builder.Services.AddRazorPages();

// Configure Session (for storing user data)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
