using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.DTOs;
using ProjectManager.Application.UseCases.Register;

namespace ProjectManager.Api.Controllers
{
    [Controller]
    [Route("auth")]
    public class AuthController(IRegisterUseCase registerUseCase) : ControllerBase
    {
        private readonly IRegisterUseCase _registerUseCase = registerUseCase;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    new
                    {
                        correlationId = GetCorrelationId(),
                        errorCode = "INVALID_MODEL",
                        message = "Invalid registration data",
                        deatils = ModelState
                            .Where(kvp => kvp.Value?.Errors?.Count > 0)
                            .ToDictionary(
                                kvp => kvp.Key,
                                kvp => kvp.Value?.Errors?.Select(e => e.ErrorMessage).ToArray() ?? []
                            )
                    });
            }

            try
            {
                var userId = await _registerUseCase.Execute(request);
                return CreatedAtAction(nameof(Register), new { id = userId }, new { id = userId });

            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("already registered"))
            {

                return Conflict(
                    new
                    {
                        correlationId = GetCorrelationId(),
                        errorCode = "EMAIL_ALREADY_EXISTS",
                        message = "Email already registered"
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

        private string GetCorrelationId() => HttpContext.TraceIdentifier;
    }
}
