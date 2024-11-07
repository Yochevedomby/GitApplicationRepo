using GitApplication.Services;
using GitApplication.Middleware;

var builder = WebApplication.CreateBuilder(args);


// ���� �� ����� CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMyOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // ����� �� ����� ��� ���� ����� �� ����
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
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // ����� ��� ���� ����
    options.Cookie.HttpOnly = true;  // ����� �� ����� �-cookie ��� Javascript
    options.Cookie.IsEssential = true;  // ��� ���� ���� �����
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ����� ������ Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddHttpClient();


// ����� ������� ������� �����
//builder.Services.AddSingleton<JwtService>();
builder.Services.AddScoped<SessionService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();

// ���� �� CORS
app.UseCors("AllowMyOrigin");

app.UseMiddleware<ErrorHandlingMiddleware>(); // ����� Middleware �������
// ����� �������� �� �-JWT
app.UseMiddleware<JwtTokenMiddleware>();
app.UseSession(); // Middleware ���� Session



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
