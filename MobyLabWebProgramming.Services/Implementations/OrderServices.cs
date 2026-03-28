using System.Net;
using MobyLabWebProgramming.Database.Repository;
using MobyLabWebProgramming.Database.Repository.Entities;
using MobyLabWebProgramming.Database.Repository.Enums;
using MobyLabWebProgramming.Infrastructure.Errors;
using MobyLabWebProgramming.Infrastructure.Repositories.Interfaces;
using MobyLabWebProgramming.Infrastructure.Requests;
using MobyLabWebProgramming.Infrastructure.Responses;
using MobyLabWebProgramming.Services.Abstractions;
using MobyLabWebProgramming.Services.Constants;
using MobyLabWebProgramming.Services.DataTransferObjects;
using MobyLabWebProgramming.Services.Specifications;

namespace MobyLabWebProgramming.Services.Implementations;

public class OrderServices(IRepository<WebAppDatabaseContext> repository) : IOrderService
{
    public async Task<ServiceResponse> AddNewOrder(UserRecord? requestingUser,
        CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Client)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only a client can make an order!", ErrorCodes.CannotAdd));
        }
        
        var result = await repository.GetAsync(new OrderSpec(requestingUser!.Id, OrderStatusEnum.New), cancellationToken);
        if (result != null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "You already have an unfinished order!", ErrorCodes.CannotAdd));
        }
        
        await repository.AddAsync(new Order
        {
            UserId = requestingUser!.Id,
            OrderStatus = OrderStatusEnum.New,
            TotalPaid = 0
        }, cancellationToken);
        
        return ServiceResponse.ForSuccess();
    }


    public async Task<ServiceResponse> DeleteCurrentOrder(UserRecord? requestingUser,
        CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Client)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only a client can delete his order!", ErrorCodes.CannotDelete));
        }
        
        var result = await repository.GetAsync(new OrderSpec(requestingUser!.Id, OrderStatusEnum.New), cancellationToken);
        if (result == null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.NotFound, "You do not have an unfinished order!", ErrorCodes.CannotDelete));
        }
        
        await repository.DeleteAsync<Order>(result.Id, cancellationToken);
        
        return ServiceResponse.ForSuccess();
    }


    public async Task<ServiceResponse<OrderRecord>> GetOrder(Guid id, UserRecord? requestingUser,
        CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(new OrderProjectionSpec(id), cancellationToken);
        if (result == null)
        {
            return ServiceResponse.FromError<OrderRecord>(new(HttpStatusCode.NotFound, "The order dose not exist!", ErrorCodes.CannotDelete));
        }
        
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin && requestingUser.Id != result.UserId)
        {
            return ServiceResponse.FromError<OrderRecord>(new(HttpStatusCode.Forbidden, "Only a client can delete his order!", ErrorCodes.CannotDelete));
        }
        
        return ServiceResponse.ForSuccess(result);
    }


    public async Task<ServiceResponse<PagedResponse<OrderRecord>>> GetOrders(PaginationSearchQueryParams pagination,
        UserRecord? requestingUser, CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, new OrderProjectionSpec(pagination.Search, requestingUser!.Id), cancellationToken); // Use the specification and pagination API to get only some entities from the database.

        return ServiceResponse.ForSuccess(result);
    }


    public async Task<ServiceResponse> UpdateOrder(OrderUpdateRecord orderUpdateRecord, UserRecord? requestingUser,
        CancellationToken cancellationToken = default)
    {
        var result  = await repository.GetAsync(new OrderSpec(orderUpdateRecord.Id), cancellationToken);

        if (result == null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.NotFound, "The order dose not exist!", ErrorCodes.CannotUpdate));
        }

        if (requestingUser != null && result.UserId != requestingUser.Id && requestingUser.Role != UserRoleEnum.Admin)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "You do not have permissions to update the order!", ErrorCodes.CannotUpdate));
        }

        if (orderUpdateRecord.Status == OrderStatusEnum.New)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "You can not change an order to be considered new!", ErrorCodes.CannotUpdate));
        }
        
        result.OrderStatus = orderUpdateRecord.Status;
        await repository.UpdateAsync(result, cancellationToken);
        
        return ServiceResponse.ForSuccess();
    }


    public async Task<ServiceResponse> PlaceOrder(UserRecord? requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Client)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only a client can place an order!", ErrorCodes.CannotUpdate));
        }
        
        var result = await repository.GetAsync(new OrderSpec(requestingUser!.Id, OrderStatusEnum.New), cancellationToken);
        if (result == null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.NotFound, "You do not have an unfinished order!", ErrorCodes.CannotUpdate));
        }

        result.OrderStatus = OrderStatusEnum.Confirmed;
        await repository.UpdateAsync(result, cancellationToken);
        
        return ServiceResponse.ForSuccess();
    }
}