using CacheServiceApp.Model;

namespace CacheServiceApp.Services
{
    public interface IBirdsService
    {
        Task<List<Bird>> Get();
    }
}
