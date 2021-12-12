using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using System.Text;
using quiz_game.DbContext;
using quiz_game.Configuration;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContextPool<ApiDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("ApiDbContextConnectionString")));

builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));


builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwt => {

    var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtConfig:Secret"]);
   

    jwt.SaveToken = true;
    jwt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true, // this will validate 
        IssuerSigningKey = new SymmetricSecurityKey(key), // Add the secret key to Jwt encryption
        ValidateIssuer = false,
        ValidateAudience = false,
        RequireExpirationTime = false,
        ValidateLifetime = true
    };
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "QuizGame API", Version = "v1" });
});


builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddEntityFrameworkStores<ApiDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoApi v1"));
}



app.UseHttpsRedirection();

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();