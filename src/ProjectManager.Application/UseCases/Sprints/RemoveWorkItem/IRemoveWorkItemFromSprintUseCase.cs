namespace ProjectManager.Application.UseCases.Sprints.RemoveWorkItem
{
    public interface IRemoveWorkItemFromSprintUseCase
    {
        Task Execute(Guid projectId, Guid sprintId, Guid workItemId, CancellationToken ct = default);
    }
}
