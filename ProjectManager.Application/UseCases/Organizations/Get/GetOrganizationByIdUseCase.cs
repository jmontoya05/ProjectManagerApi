using ProjectManager.Application.Ports;

namespace ProjectManager.Application.UseCases.Organizations.Get
{
    public sealed class GetOrganizationByIdUseCase(IOrganizationRepository organizationRepository) : IGetOrganizationByIdUseCase
    {
        private readonly IOrganizationRepository _organizationRepository = organizationRepository;

        public async Task<GetOrganizationByIdResponse> Execute(Guid organizationId, CancellationToken ct = default)
        {
            var organization = await _organizationRepository.GetByIdAsync(organizationId, ct)
                ?? throw new InvalidOperationException("Organization not found");

            return new GetOrganizationByIdResponse
            {
                Id = organization.Id,
                Name = organization.Name,
                Status = organization.Status,
                OwnerId = organization.OwnerId,
                CreatedAt = organization.CreatedAt,
                CreatedBy = organization.CreatedBy,
                UpdatedAt = organization.UpdatedAt,
                UpdatedBy = organization.UpdatedBy
            };
        }
    }
}
