namespace Shared.Dtos;

public record PaginationResponseDto<T>(
    int PageNumber,
    int PageSize,
    int TotalPages,
    int TotalRecords,
    List<T> Data
);
