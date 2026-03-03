using ProjectManager.Application.DTOs;
using ProjectManager.Application.DTOs.WorkItems;
using ProjectManager.Application.DTOs.WorkItems.Response;

namespace ProjectManager.Application.UseCases.WorkItems.List
{
    public interface IListWorkItemsUseCase
    {
        Task<PagedResponse<WorkItemResponse>> Execute(Guid projectId, WorkItemFilter filter, CancellationToken ct = default);
    }
}
