namespace ProjectManager.Application.UseCases.Sprints.Start
{
    public interface IStartSprintUseCase
    {
        Task Execute(Guid projectId, Guid sprintId, CancellationToken ct = default);
    }
}
