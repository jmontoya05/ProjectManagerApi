using Microsoft.AspNetCore.Mvc;
using ProjectManager.Api.Middlewares;
using ProjectManager.Application.DTOs.Requests;
using ProjectManager.Application.UseCases.Login;
using ProjectManager.Application.UseCases.Register;

namespace ProjectManager.Api.Controllers
{
    [Controller]
    [Route("auth")]
    public class AuthController(IRegisterUseCase registerUseCase, ILoginUseCase loginUseCase) : ControllerBase
    {
        private readonly IRegisterUseCase _registerUseCase = registerUseCase;
        private readonly ILoginUseCase _loginUseCase = loginUseCase;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return GetInvalidModelResponse();

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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return GetInvalidModelResponse();

            try
            {
                var response = await _loginUseCase.Execute(request);
                return Ok(response);
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("The email doesn't exists") ||
                                                        ex.Message.Contains("Invalid password") ||
                                                        ex.Message.Contains("User is blocked"))
            {
                return Unauthorized(
                    new
                    {
                        correlationId = GetCorrelationId(),
                        errorCode = "UNAUTHORIZED",
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

        private string GetCorrelationId() =>
            ExceptionHandlingMiddleware.GetCorrelationId(HttpContext);

        private IActionResult GetInvalidModelResponse()
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
    }
}
