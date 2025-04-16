using DeveloperStore.Domain.Entities;

namespace DeveloperStore.Application.Interfaces
{
    public interface ISaleService
    {
        Task<Sale> CreateSaleAsync(Sale sale);
        Task CancelSaleAsync(int saleId, string reason);
        Task ModifySaleAsync(Sale sale, string modifiedBy);
    }
}
