using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using AutoMapper;
using DAL;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;
using Logic.Models;
using Logic.Interfaces;
using Logic.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false,
            RequireExpirationTime = true,
        };
    });

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(cfg =>
{
    cfg.CreateMap<LessonTemplate, LessonTemplateDTO>();
    cfg.CreateMap<LessonTemplateDTO, LessonTemplate>();
    cfg.CreateMap<Student, ProfileDTO>();
    cfg.CreateMap<Tutor, ProfileDTO>();
    cfg.CreateMap<ProfileDTO, Student>();
    cfg.CreateMap<ProfileDTO, Tutor>();
    cfg.CreateMap<Comment, CommentDTO>();
    cfg.CreateMap<CommentDTO, Comment>();
    cfg.CreateMap<LessonDTO, Lesson>();
    cfg.CreateMap<Lesson, LessonDTO>()
        .ForMember(dest => dest.AllowedTemplatesId, opt => opt.MapFrom(src => src.AllowedTemplates.Select(e => e.Id).ToList()))
        .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.Student.Id));
});

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IJWTService, JWTService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<ILessonTemplatesService, LessonTemplateService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ILessonService, LessonService>();
builder.Services.AddScoped<ISearchService, TreeSearchService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();