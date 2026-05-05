namespace ProjectManager.Application.UseCases.Permissions.Delete
{
    public interface IDeletePermissionUseCase
    {
        Task Execute(Guid id, CancellationToken ct = default);
    }
}
