using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Api.Middlewares;
using ProjectManager.Application.DTOs.Organizations;
using ProjectManager.Application.UseCases.Organizations.Create;
using ProjectManager.Application.UseCases.Organizations.Get;
using ProjectManager.Application.UseCases.Organizations.List;
using System.Security.Claims;

namespace ProjectManager.Api.Controllers
{
    [ApiController]
    [Route("organizations")]
    [Authorize]
    public sealed class OrganizationController(IListOrganizationsUseCase listOrganizationsUseCase, ICreateOrganizationUseCase createOrganizationUseCase, IGetOrganizationByIdUseCase getOrganizationByIdUseCase) : ControllerBase
    {
        private readonly IListOrganizationsUseCase _listOrganizationsUseCase = listOrganizationsUseCase;
        private readonly ICreateOrganizationUseCase _createOrganizationUseCase = createOrganizationUseCase;
        private readonly IGetOrganizationByIdUseCase _getOrganizationByIdUseCase = getOrganizationByIdUseCase;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrganizationRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(
                    new
                    {
                        correlationId = ExceptionHandlingMiddleware.GetCorrelationId(HttpContext),
                        errorCode = "INVALID_TOKEN",
                        message = "Invalid token."
                    });
            }

            var response = await _createOrganizationUseCase.Execute(request, userId);
            return CreatedAtAction(nameof(Create), new { id = response }, new { id = response });
        }

        [HttpGet]
        public async Task<IActionResult> ListByUser()
        {
            var response = await _listOrganizationsUseCase.Execute();
            return Ok(response);
        }

        [HttpGet("{organizationId}")]
        public async Task<IActionResult> GetById([FromRoute] Guid organizationId)
        {
            var response = await _getOrganizationByIdUseCase.Execute(organizationId);
            return Ok(response);
        }
    }
}
