using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectManager.Api.Authorization;
using ProjectManager.Api.Conventions;
using ProjectManager.Api.Middlewares;
using ProjectManager.Application.Ports;
using ProjectManager.Application.Services;
using ProjectManager.Application.UseCases.Auth.Invite;
using ProjectManager.Application.UseCases.Auth.Login;
using ProjectManager.Application.UseCases.Auth.Logout;
using ProjectManager.Application.UseCases.Auth.Refresh;
using ProjectManager.Application.UseCases.Auth.SelectOrganization;
using ProjectManager.Application.UseCases.Organizations.Create;
using ProjectManager.Application.UseCases.Organizations.Get;
using ProjectManager.Application.UseCases.Organizations.List;
using ProjectManager.Application.UseCases.Projects.Create;
using ProjectManager.Application.UseCases.Projects.Get;
using ProjectManager.Application.UseCases.Projects.List;
using ProjectManager.Application.UseCases.Projects.Update;
using ProjectManager.Application.UseCases.Projects;
using ProjectManager.Application.UseCases.Roles;
using ProjectManager.Application.UseCases.Teams.AddTeamMember;
using ProjectManager.Application.UseCases.Teams.Create;
using ProjectManager.Application.UseCases.Teams.Get;
using ProjectManager.Application.UseCases.Teams.List;
using ProjectManager.Application.UseCases.Users.GetProfile;
using ProjectManager.Application.UseCases.WorkItems.Create;
using ProjectManager.Application.UseCases.WorkItems.List;
using ProjectManager.Application.UseCases.WorkItems.Update;
using ProjectManager.Infrastructure.Persistence.Context;
using ProjectManager.Infrastructure.Persistence.Repositories;
using ProjectManager.Infrastructure.Services;
using System.Security.Claims;
using System.Text;
using ProjectManager.Application.UseCases.Organizations.RoleAssigment;
using ProjectManager.Application.UseCases.Permissions.Create;
using ProjectManager.Application.UseCases.Permissions.Delete;
using ProjectManager.Application.UseCases.Permissions.GetAll;
using ProjectManager.Application.UseCases.Permissions.GetById;
using ProjectManager.Application.UseCases.Permissions.Update;
using ProjectManager.Application.UseCases.Roles.Create;
using ProjectManager.Application.UseCases.Roles.Delete;
using ProjectManager.Application.UseCases.Roles.GetAll;
using ProjectManager.Application.UseCases.Roles.GetAllByOrganization;
using ProjectManager.Application.UseCases.Roles.GetById;
using ProjectManager.Application.UseCases.Roles.Update;
using ProjectManager.Application.UseCases.Sprints.Create;
using ProjectManager.Application.UseCases.Sprints.Update;
using ProjectManager.Application.UseCases.Sprints.GetById;
using ProjectManager.Application.UseCases.Sprints.ListByProject;
using ProjectManager.Application.UseCases.Sprints.Delete;
using ProjectManager.Application.UseCases.Sprints.Start;
using ProjectManager.Application.UseCases.Sprints.Complete;
using ProjectManager.Application.UseCases.Sprints.AddWorkItem;
using ProjectManager.Application.UseCases.Sprints.RemoveWorkItem;
using ProjectManager.Application.UseCases.Sprints.ListWorkItems;
using ProjectManager.Application.UseCases.Sprints.ReorderWorkItems;
using ProjectManager.Application.UseCases.Sprints.GetVelocity;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ProjectManagerDbContext>((_, options) =>
{
    options.UseSqlServer(connectionString);
}, contextLifetime: ServiceLifetime.Scoped, optionsLifetime: ServiceLifetime.Scoped);

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
    .AddPolicy("AdminOnly", policy => policy.RequireRole("Admin", "Owner"))
    .AddPolicy("OrgOwner", policy => policy.RequireRole("OrgOwner"))
    .AddPolicy("OrgAdmin", policy => policy.RequireRole("OrgOwner", "OrgAdmin"))
    .AddPolicy("OrgMember", policy => policy.RequireRole("OrgOwner", "OrgAdmin", "OrgMember"))
    .AddPolicy("ProjectManager", policy => policy.Requirements.Add(new ProjectRoleRequirement("ProjectManager")))
    .AddPolicy("ProjectMember", policy => policy.Requirements.Add(new ProjectRoleRequirement("ProjectMember", true)))
    .AddPolicy("ProjectViewer", policy => policy.Requirements.Add(new ProjectRoleRequirement("ProjectViewer", true)))
    // Granular work item policies
    .AddPolicy("WorkItem.Create", policy => policy.RequireClaim("Permission", "WorkItem.Create"))
    .AddPolicy("WorkItem.Edit", policy => policy.RequireClaim("Permission", "WorkItem.Edit"))
    .AddPolicy("WorkItem.Delete", policy => policy.RequireClaim("Permission", "WorkItem.Delete"))
    .AddPolicy("WorkItem.Assign", policy => policy.RequireClaim("Permission", "WorkItem.Assign"))
    .AddPolicy("WorkItem.Comment", policy => policy.RequireClaim("Permission", "WorkItem.Comment"))
    .AddPolicy("WorkItem.ChangeStatus", policy => policy.RequireClaim("Permission", "WorkItem.ChangeStatus"))
    // Team management policies
    .AddPolicy("Team.AddMember", policy => policy.RequireClaim("Permission", "Team.AddMember"))
    .AddPolicy("Team.RemoveMember", policy => policy.RequireClaim("Permission", "Team.RemoveMember"));

//Dependency injection
builder.Services.AddScoped<IAuthorizationHandler, ProjectRoleHandler>();
builder.Services.AddScoped<ITokenService, TokenService>();
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
builder.Services.AddScoped<IUpdateProjectUseCase, UpdateProjectUseCase>();
builder.Services.AddScoped<IWorkItemRepository, WorkItemRepository>();
builder.Services.AddScoped<IListWorkItemsUseCase, ListWorkItemsUseCase>();
builder.Services.AddScoped<ICreateWorkItemUseCase, CreateWorkItemUseCase>();
builder.Services.AddScoped<IUpdateWorkItemStatusUseCase, UpdateWorkItemStatusUseCase>();
builder.Services.AddScoped<IInvitationRepository, InvitationRepository>();
builder.Services.AddScoped<IInviteUserUseCase, InviteUserUseCase>();
builder.Services.AddScoped<ICompleteInvitationUseCase, CompleteInvitationUseCase>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ITenantContext, TenantContext>();
builder.Services.AddScoped<IEmailService, MockEmailService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IGetAllPermissionsUseCase, GetAllPermissionsUseCase>();
builder.Services.AddScoped<IGetPermissionByIdUseCase, GetPermissionByIdUseCase>();
builder.Services.AddScoped<ICreatePermissionUseCase, CreatePermissionUseCase>();
builder.Services.AddScoped<IUpdatePermissionUseCase, UpdatePermissionUseCase>();
builder.Services.AddScoped<IDeletePermissionUseCase, DeletePermissionUseCase>();
builder.Services.AddScoped<IGetAllRolesUseCase, GetAllRolesUseCase>();
builder.Services.AddScoped<IGetAllRolesByOrganizationUseCase, GetAllRolesByOrganizationUseCase>();
builder.Services.AddScoped<IGetRoleByIdUseCase, GetRoleByIdUseCase>();
builder.Services.AddScoped<ICreateRoleUseCase, CreateRoleUseCase>();
builder.Services.AddScoped<IUpdateRoleUseCase, UpdateRoleUseCase>();
builder.Services.AddScoped<IDeleteRoleUseCase, DeleteRoleUseCase>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
builder.Services.AddScoped<IRolePermissionService, RolePermissionService>();
builder.Services.AddScoped<IOrganizationRoleAssignmentUseCase, OrganizationRoleAssignmentUseCase>();
builder.Services.AddScoped<IProjectRoleAssignmentService, ProjectRoleAssignmentService>();
builder.Services.AddScoped<ISprintRepository, SprintRepository>();
builder.Services.AddScoped<ISprintWorkItemRepository, SprintWorkItemRepository>();
builder.Services.AddScoped<ICreateSprintUseCase, CreateSprintUseCase>();
builder.Services.AddScoped<IUpdateSprintUseCase, UpdateSprintUseCase>();
builder.Services.AddScoped<IGetSprintByIdUseCase, GetSprintByIdUseCase>();
builder.Services.AddScoped<IListSprintsByProjectUseCase, ListSprintsByProjectUseCase>();
builder.Services.AddScoped<IDeleteSprintUseCase, DeleteSprintUseCase>();
builder.Services.AddScoped<IStartSprintUseCase, StartSprintUseCase>();
builder.Services.AddScoped<ICompleteSprintUseCase, CompleteSprintUseCase>();
builder.Services.AddScoped<IAddWorkItemToSprintUseCase, AddWorkItemToSprintUseCase>();
builder.Services.AddScoped<IRemoveWorkItemFromSprintUseCase, RemoveWorkItemFromSprintUseCase>();
builder.Services.AddScoped<IListSprintWorkItemsUseCase, ListSprintWorkItemsUseCase>();
builder.Services.AddScoped<IReorderSprintWorkItemsUseCase, ReorderSprintWorkItemsUseCase>();
builder.Services.AddScoped<IGetSprintVelocityUseCase, GetSprintVelocityUseCase>();

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
app.UseMiddleware<TenantContextMiddleware>();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();
