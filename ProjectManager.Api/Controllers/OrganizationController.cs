using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Api.Middlewares;
using ProjectManager.Application.UseCases.Organizations.Create;
using ProjectManager.Application.UseCases.Organizations.List;
using System.Security.Claims;

namespace ProjectManager.Api.Controllers
{
    [ApiController]
    [Route("organizations")]
    [Authorize]
    public sealed class OrganizationController(IListOrganizationsUseCase listOrganizationsUseCase, ICreateOrganizationUseCase createOrganizationUseCase) : ControllerBase
    {
        private readonly IListOrganizationsUseCase _listOrganizationsUseCase = listOrganizationsUseCase;
        private readonly ICreateOrganizationUseCase _createOrganizationUseCase = createOrganizationUseCase;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrganizationRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(
                    new
                    {
                        correlationId = GetCorrelationId(),
                        errorCode = "INVALID_TOKEN",
                        message = "Invalid token."
                    });
            }

            try
            {
                var response = await _createOrganizationUseCase.Execute(request, userId);

                return CreatedAtAction(nameof(Create), new { id = response }, new { id = response });
            }
            catch (InvalidOperationException ex)
            {

                return BadRequest(
                    new
                    {
                        correlationId = GetCorrelationId(),
                        errorCode = "BAD_REQUEST",
                        message = ex.Message
                    });
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    new
                    {
                        correlationId = GetCorrelationId(),
                        errorCode = "INTERNAL_ERROR",
                        message = "An unexpected error occurred"
                    });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ListByUser()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(
                    new
                    {
                        correlationId = GetCorrelationId(),
                        errorCode = "INVALID_TOKEN",
                        message = "Invalid token."
                    });
            }

            try
            {
                var response = await _listOrganizationsUseCase.Execute(userId);
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    new
                    {
                        correlationId = GetCorrelationId(),
                        errorCode = "INTERNAL_ERROR",
                        message = "An unexpected error occurred"
                    });
            }
        }

        private string GetCorrelationId() =>
            ExceptionHandlingMiddleware.GetCorrelationId(HttpContext);
    }
}
