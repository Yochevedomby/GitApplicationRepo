using GitApplication.Services;
using GitApplication.Middleware;

var builder = WebApplication.CreateBuilder(args);


// הוסף את שירות CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMyOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // מגדיר את הלקוח שלך ככזה שמותר לו לגשת
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient<GitHubService>();


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // הגדרת זמן תוקף לסשן
    options.Cookie.HttpOnly = true;  // מגביל את הגישה ל-cookie דרך Javascript
    options.Cookie.IsEssential = true;  // ככה הסשן יהיה חיוני
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// הוספת שירותי Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddHttpClient();


// הוספת שירותים מותאמים אישית
//builder.Services.AddSingleton<JwtService>();
builder.Services.AddScoped<SessionService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();

// הפעל את CORS
app.UseCors("AllowMyOrigin");

app.UseMiddleware<ErrorHandlingMiddleware>(); // הוספת Middleware לשגיאות
// הוספת המידלוור של ה-JWT
app.UseMiddleware<JwtTokenMiddleware>();
app.UseSession(); // Middleware עבור Session



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
