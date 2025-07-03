using System.Text;
using chatbot.Data;
using chatbot.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Chatbot API", Version = "v1" });


    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});


builder.Services.AddCors((options) =>
{
    options.AddPolicy("DevCors", (corsBuilder) =>
    {
        corsBuilder.WithOrigins("http://localhost:4200", "http://localhost:3000", "http://localhost:8000", "http://localhost:5000")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
    });
    options.AddPolicy("ProdCors", (corsBuilder) =>
    {
        corsBuilder.WithOrigins("https://myProductionSite.com")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
    });
});

builder.Services.AddScoped<AuthHelper>();
builder.Services.AddScoped<DataContextEF>();


string? tokenKeyString = builder.Configuration.GetSection("AppSettings:TokenKey").Value;
if (string.IsNullOrEmpty(tokenKeyString))
{
    throw new InvalidOperationException("TokenKey is not configured in AppSettings.");
}



SymmetricSecurityKey tokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKeyString));


TokenValidationParameters tokenValidationParameters = new TokenValidationParameters()
{
    IssuerSigningKey = tokenKey,
    ValidateIssuer = false,
    ValidateAudience = false,
    ValidateIssuerSigningKey = true,
    ClockSkew = TimeSpan.FromSeconds(30)
};

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = tokenValidationParameters;
    options.IncludeErrorDetails = true; // This will include error details in the response when authentication fails.
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // This middleware will display detailed error information in the browser when an exception occurs.
    app.UseCors("DevCors");
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseCors("ProdCors");
    app.UseHttpsRedirection();
}


// This will make sure to add the authentication and authorization middleware after CORS and before the endpoint mapping.
app.UseAuthentication(); // Adds authentication middleware to the request pipeline.
app.UseAuthorization(); // Adds authorization middleware to the request pipeline, which checks if the user is authenticated and authorized to access the requested resource.


app.MapControllers(); // Maps [ApiController] classes to HTTP endpoints.


app.Run();

