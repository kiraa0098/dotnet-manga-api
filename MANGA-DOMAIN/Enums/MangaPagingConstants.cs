namespace MANGA_DOMAIN.Enums;

public static class MangaPagingConstants
{
    public const int DefaultPageSize = 1; // TODO: Default is 10 but changes to 1 for better debugging

    public static int GetOffset(int pageNumber, int pageSize = DefaultPageSize)
    {
        return (pageNumber - 1) * pageSize;
    }
}