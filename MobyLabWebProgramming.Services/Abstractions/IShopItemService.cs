using MobyLabWebProgramming.Infrastructure.Requests;
using MobyLabWebProgramming.Infrastructure.Responses;
using MobyLabWebProgramming.Services.DataTransferObjects;

namespace MobyLabWebProgramming.Services.Abstractions;

public interface IShopItemService
{
    public Task<ServiceResponse> AddAShopItem(ShopItemAddRecord shopItemAddRecord, UserRecord? requestingUser, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> DeleteShopItem(Guid id, UserRecord? requestingUser, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<ShopItemRecord>> GetShopItem(Guid id, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<PagedResponse<ShopItemRecord>>> GetShopItems(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> UpdateShopItem(ShopItemUpdateRecord shopItemUpdateRecord, UserRecord? requestingUser, CancellationToken cancellationToken = default);
}