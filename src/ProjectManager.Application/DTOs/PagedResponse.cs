namespace ProjectManager.Application.DTOs
{
    public sealed class PagedResponse<T>
    {
        public IEnumerable<T> Items { get; set; } = [];
        public string? NextCursor { get; set; }
        public bool HasNextPage => !string.IsNullOrEmpty(NextCursor);
    }
}
