using MobyLabWebProgramming.Infrastructure.Requests;
using MobyLabWebProgramming.Infrastructure.Responses;
using MobyLabWebProgramming.Services.DataTransferObjects;

namespace MobyLabWebProgramming.Services.Abstractions;

public interface IOrderService
{
    public Task<ServiceResponse> AddNewOrder(UserRecord? requestingUser, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> DeleteCurrentOrder(UserRecord? requestingUser, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<OrderRecord>> GetOrder(Guid id, UserRecord? requestingUser, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<PagedResponse<OrderRecord>>> GetOrders(PaginationSearchQueryParams pagination, UserRecord? requestingUser, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> UpdateOrder(OrderUpdateRecord orderUpdateRecord, UserRecord? requestingUser, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> PlaceOrder(UserRecord? requestingUser, CancellationToken cancellationToken = default);
}