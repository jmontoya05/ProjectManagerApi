namespace ProjectManager.Application.UseCases.Sprints.Complete
{
    public interface ICompleteSprintUseCase
    {
        Task Execute(Guid projectId, Guid sprintId, CancellationToken ct = default);
    }
}
