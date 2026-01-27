namespace ProjectManager.Application.UseCases.Projects.Get
{
    public interface IGetProjectByIdUseCase
    {
        Task<GetProjectByIdResponse> Execute(Guid projectId, CancellationToken ct = default);
    }
}
