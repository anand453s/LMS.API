using LMS.API.Security;
using LMS.Repository.EF.Context;
using LMS.Repository.Interfaces;
using LMS.Repository.Repositories;
using LMS.Service.Interfaces;
using LMS.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
    };
    options.SaveToken = true;
});
builder.Services.AddControllers();

builder.Services.AddTransient<IAdminService, AdminService>();

builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

builder.Services.AddTransient<ILoginService, LoginService>();
builder.Services.AddTransient<IUserLoginRepository, UserLoginRepository>();

builder.Services.AddTransient<IStudentService, StudentService>();
builder.Services.AddTransient<IStudentRepository, StudentRepository>();

builder.Services.AddTransient<IInstructorServices, InstructorServices>();
builder.Services.AddTransient<IInstructorRepository, InstructorRepository>();

builder.Services.AddTransient<ICourseServices, CourseServices>();
builder.Services.AddTransient<ICourseRepository, CourseRepository>();

builder.Services.AddTransient<ICourseMaterialService, CourseMaterialService>();
builder.Services.AddTransient<ICourseMaterialRepository, CourseMaterialRepository>();

builder.Services.AddTransient<IStudentCourseServices, StudentCourseServices>();
builder.Services.AddTransient<IStudentCourseRepository, StudentCourseRepository>();

builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddTransient<AuthenticationService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Authorization header using bearer (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

//services cors
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app cors
app.UseCors("corsapp");

app.UseAuthentication();

app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
