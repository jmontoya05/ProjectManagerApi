namespace ProjectManager.Application.UseCases.Roles.Delete
{
    public interface IDeleteRoleUseCase
    {
        Task Execute(Guid id, CancellationToken ct = default);
    }
}
