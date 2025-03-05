namespace Shared.Dtos
{
    public record PagedResult<T>(
        int PageNumber,
        int PageSize,
        int TotalPages,
        int TotalRecords,
        List<T> Data
    );
}
