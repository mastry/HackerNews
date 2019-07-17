using System.Threading.Tasks;
using HackerNews.Models;

namespace HackerNews
{
    public interface IHackerNewsClient
    {
        Task<int[]> GetIncomingItems();
        Task<Item> GetItem(int id);
        Task<int[]> GetPopularItems();
        Task<int[]> GetTopItems();
    }
}