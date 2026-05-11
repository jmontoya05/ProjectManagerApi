namespace ProjectManager.Application.UseCases.Sprints.Delete
{
    public interface IDeleteSprintUseCase
    {
        Task Execute(Guid projectId, Guid sprintId, CancellationToken ct = default);
    }
}
