namespace MANGA_DOMAIN.Enums;

public static class MangaPagingConstants
{
    public const int DefaultPageSize = 10;

    public static int GetOffset(int pageNumber, int pageSize = DefaultPageSize)
    {
        return (pageNumber - 1) * pageSize;
    }
}