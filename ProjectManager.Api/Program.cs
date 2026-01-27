using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectManager.Api.Conventions;
using ProjectManager.Api.Middlewares;
using ProjectManager.Application.Ports;
using ProjectManager.Application.Services;
using ProjectManager.Application.UseCases.Auth.Login;
using ProjectManager.Application.UseCases.Auth.Logout;
using ProjectManager.Application.UseCases.Auth.Refresh;
using ProjectManager.Application.UseCases.Auth.Register;
using ProjectManager.Application.UseCases.Auth.SelectOrganization;
using ProjectManager.Application.UseCases.Organizations.Create;
using ProjectManager.Application.UseCases.Organizations.Get;
using ProjectManager.Application.UseCases.Organizations.List;
using ProjectManager.Application.UseCases.Projects.Create;
using ProjectManager.Application.UseCases.Projects.Get;
using ProjectManager.Application.UseCases.Projects.List;
using ProjectManager.Application.UseCases.Teams.AddTeamMember;
using ProjectManager.Application.UseCases.Teams.Create;
using ProjectManager.Application.UseCases.Teams.Get;
using ProjectManager.Application.UseCases.Teams.List;
using ProjectManager.Application.UseCases.Users.GetProfile;
using ProjectManager.Infrastructure.Data;
using ProjectManager.Infrastructure.Repositories;
using ProjectManager.Infrastructure.Services;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ProjectManagerDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddControllers(options =>
{
    options.Conventions.Insert(0, new GlobalRoutePrefixConvention("api/v1"));
});

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured.");
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "ProjectManager";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "ProjectManager";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            RoleClaimType = ClaimTypes.Role
        };
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin", "Owner"));


//Dependency injection
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IRegisterUseCase, RegisterUseCase>();
builder.Services.AddScoped<ILoginUseCase, LoginUseCase>();
builder.Services.AddScoped<ISelectOrganizationUseCase, SelectOrganizationUseCase>();
builder.Services.AddScoped<IRefreshUseCase, RefreshUseCase>();
builder.Services.AddScoped<ILogoutUseCase, LogoutUseCase>();
builder.Services.AddScoped<IGetProfileUseCase, GetProfileUseCase>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<ITeamMemberRepository, TeamMemberRepository>();
builder.Services.AddScoped<ICreateTeamUseCase, CreateTeamUseCase>();
builder.Services.AddScoped<IListTeamsUseCase, ListTeamsUseCase>();
builder.Services.AddScoped<IGetTeamByIdUseCase, GetTeamByIdUseCase>();
builder.Services.AddScoped<IAddTeamMemberUseCase, AddTeamMemberUseCase>();
builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();
builder.Services.AddScoped<IListOrganizationsUseCase, ListOrganizationsUseCase>();
builder.Services.AddScoped<ICreateOrganizationUseCase, CreateOrganizationUseCase>();
builder.Services.AddScoped<IGetOrganizationByIdUseCase, GetOrganizationByIdUseCase>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ICreateProjectUseCase, CreateProjectUseCase>();
builder.Services.AddScoped<IListProjectsUseCase, ListProjectsUseCase>();
builder.Services.AddScoped<IGetProjectByIdUseCase, GetProjectByIdUseCase>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<CorrelationIdMiddleware>();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();

app.UseMiddleware<OrganizationMiddleware>();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
