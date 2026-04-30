using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.DTOs.Organizations;
using ProjectManager.Application.UseCases.Organizations.Create;
using ProjectManager.Application.UseCases.Organizations.Get;
using ProjectManager.Application.UseCases.Organizations.List;

namespace ProjectManager.Api.Controllers
{
    [ApiController]
    [Route("organizations")]
    [Authorize]
    public sealed class OrganizationController(
        IListOrganizationsUseCase listOrganizationsUseCase, 
        ICreateOrganizationUseCase createOrganizationUseCase, 
        IGetOrganizationByIdUseCase getOrganizationByIdUseCase
    ) : ControllerBase
    {
        private readonly IListOrganizationsUseCase _listOrganizationsUseCase = listOrganizationsUseCase;
        private readonly ICreateOrganizationUseCase _createOrganizationUseCase = createOrganizationUseCase;
        private readonly IGetOrganizationByIdUseCase _getOrganizationByIdUseCase = getOrganizationByIdUseCase;

        [HttpPost]
        [Authorize(Policy = "OrgOwner")]
        public async Task<IActionResult> Create([FromBody] CreateOrganizationRequest request, CancellationToken ct)
        {
            var response = await _createOrganizationUseCase.Execute(request, ct);
            return CreatedAtAction(nameof(Create), new { id = response }, new { id = response });
        }

        [HttpGet]
        [Authorize(Policy = "OrgMember")]
        public async Task<IActionResult> ListByUser(CancellationToken ct)
        {
            var response = await _listOrganizationsUseCase.Execute(ct);
            return Ok(response);
        }

        [HttpGet("{orgId:guid}")]
        [Authorize(Policy = "OrgMember")]
        public async Task<IActionResult> GetById([FromRoute] Guid orgId, CancellationToken ct)
        {
            var response = await _getOrganizationByIdUseCase.Execute(orgId, ct);
            return Ok(response);
        }
    }
}
