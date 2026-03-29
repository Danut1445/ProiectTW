using MobyLabWebProgramming.Infrastructure.Requests;
using MobyLabWebProgramming.Infrastructure.Responses;
using MobyLabWebProgramming.Services.DataTransferObjects;

namespace MobyLabWebProgramming.Services.Abstractions;

public interface IOrderItemService
{
    public Task<ServiceResponse> AddOrderItem(OrderItemAddRecord orderItemAddRecord, UserRecord? requestingUser, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> DeleteOrderItem(Guid id, UserRecord? requestingUser, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<OrderItemRecord>> GetOrderItem(Guid id, UserRecord? requestingUser, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<PagedResponse<OrderItemRecord>>> GetPagedOrderItems(Guid orderid, PaginationSearchQueryParams pagination, UserRecord? requestingUser, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<PagedResponse<OrderItemRecord>>> GetPagedOrderItemsCurrentNewOrder(PaginationSearchQueryParams pagination, UserRecord? requestingUser, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> UpdateOrderItem(OrderItemUpdateRecord orderItemUpdateRecord, UserRecord? requestingUser, CancellationToken cancellationToken = default);
}