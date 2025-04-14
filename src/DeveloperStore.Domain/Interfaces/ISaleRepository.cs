using DeveloperStore.Domain.Entities;

namespace DeveloperStore.Domain.Interfaces
{
    public interface ISaleRepository
    {
        Task<Sale> AddAsync(Sale sale);
        Task<Sale?> GetByIdAsync(Guid id);
        Task<List<Sale>> GetAllAsync();
        Task UpdateAsync(Sale sale);
        Task CancelAsync(Guid id);
    }
}
