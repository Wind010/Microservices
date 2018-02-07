

namespace Common.BaseRepository
{
    public interface IPageableRepository
    {
        uint PageNumber { get; }

        uint TotalPages { get; }
    }
}
