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

public class OrderItemServices(IRepository<WebAppDatabaseContext> repository, IOrderService orderService) : IOrderItemService
{
    public async Task<ServiceResponse> AddOrderItem(OrderItemAddRecord orderItemAddRecord, UserRecord? requestingUser,
        CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Client) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only clients can make an order!", ErrorCodes.CannotAdd));
        }
        
        var result = await repository.GetAsync(new OrderSpec(requestingUser!.Id, OrderStatusEnum.New), cancellationToken);
        if (result == null)
        {
            await orderService.AddNewOrder(requestingUser);
            result = await repository.GetAsync(new OrderSpec(requestingUser!.Id, OrderStatusEnum.New), cancellationToken);
        }
        
        var resultShopItem = await repository.GetAsync(new ShopItemSpec(orderItemAddRecord.ItemId), cancellationToken);
        if (resultShopItem == null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.NotFound, "Shop Item not found!", ErrorCodes.EntityNotFound));
        }
        
        await repository.AddAsync(new OrderItem
        {
            Id = new Guid(),
            OrderId = result!.Id,
            ItemId = orderItemAddRecord.ItemId,
            Quantity = orderItemAddRecord.Quantity,
        }, cancellationToken);
        result.TotalPaid += orderItemAddRecord.Quantity * resultShopItem.Price;
        await repository.UpdateAsync(result, cancellationToken);
        
        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> DeleteOrderItem(Guid id, UserRecord? requestingUser,
        CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Client) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only clients can delete items from an order!", ErrorCodes.CannotDelete));
        }
        
        var result = await repository.GetAsync(new OrderItemSpec(id), cancellationToken);
        if (result == null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.NotFound, "Order item not found!", ErrorCodes.CannotDelete));
        }
        
        if (result.Order.OrderStatus != OrderStatusEnum.New)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Can not update the items of an already placed order!", ErrorCodes.CannotUpdate));
        }
        result.Order.TotalPaid -= result.Quantity * result.ShopItem.Price;
        await repository.UpdateAsync(result.Order, cancellationToken);
        
        await repository.DeleteAsync<OrderItem>(result.Id, cancellationToken);
        
        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse<OrderItemRecord>> GetOrderItem(Guid id, UserRecord? requestingUser,
        CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(new OrderItemProjectionSpec(id), cancellationToken);
        if (result == null)
        {
            return ServiceResponse.FromError<OrderItemRecord>(new(HttpStatusCode.NotFound, "Order item not found!", ErrorCodes.EntityNotFound));
        }
        
        var resultNonProjection = await repository.GetAsync(new OrderItemSpec(id), cancellationToken);
        if (requestingUser != null && requestingUser.Id != resultNonProjection!.Order.UserId && requestingUser.Role == UserRoleEnum.Client) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError<OrderItemRecord>(new(HttpStatusCode.Forbidden, "Only the own user or staff can see order items!", ErrorCodes.WrongPassword));
        }
        
        return ServiceResponse.ForSuccess(result);
    }

    public async Task<ServiceResponse<PagedResponse<OrderItemRecord>>> GetPagedOrderItems(Guid orderid,
        PaginationSearchQueryParams pagination, UserRecord? requestingUser,
        CancellationToken cancellationToken = default)
    {
        var resultOrder = await repository.GetAsync(new OrderSpec(orderid), cancellationToken);
        if (resultOrder == null)
        {
            return ServiceResponse.FromError<PagedResponse<OrderItemRecord>>(new(HttpStatusCode.NotFound, "Order Not Found", ErrorCodes.EntityNotFound));
        }
        
        if (requestingUser != null && requestingUser.Id != resultOrder.UserId && requestingUser.Role == UserRoleEnum.Client) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError<PagedResponse<OrderItemRecord>>(new(HttpStatusCode.Forbidden, "Only the own user or staff can see order items!", ErrorCodes.WrongPassword));
        }
        
        var result = await repository.PageAsync(pagination, new OrderItemProjectionSpec(pagination.Search, orderid), cancellationToken); // Use the specification and pagination API to get only some entities from the database.

        return ServiceResponse.ForSuccess(result);
    }

    public async Task<ServiceResponse<PagedResponse<OrderItemRecord>>> GetPagedOrderItemsCurrentNewOrder(
        PaginationSearchQueryParams pagination, UserRecord? requestingUser,
        CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Client) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError<PagedResponse<OrderItemRecord>>(new(HttpStatusCode.Forbidden, "Only the own user can see current order items!", ErrorCodes.WrongPassword));
        }
        
        var resultOrder = await repository.GetAsync(new OrderSpec(requestingUser!.Id, OrderStatusEnum.New), cancellationToken);
        if (resultOrder == null)
        {
            return ServiceResponse.FromError<PagedResponse<OrderItemRecord>>(new(HttpStatusCode.NotFound, "Order Not Found", ErrorCodes.EntityNotFound));
        }
        
        var result = await repository.PageAsync(pagination, new OrderItemProjectionSpec(pagination.Search, resultOrder.Id), cancellationToken); // Use the specification and pagination API to get only some entities from the database.

        return ServiceResponse.ForSuccess(result);
    }
    
    public async Task<ServiceResponse> UpdateOrderItem(OrderItemUpdateRecord orderItemUpdateRecord,
        UserRecord? requestingUser, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(new OrderItemSpec(orderItemUpdateRecord.Id), cancellationToken);
        if (result == null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.NotFound, "Order Item Not Found", ErrorCodes.EntityNotFound));
        }

        if (requestingUser != null && requestingUser.Id != result.Order.UserId)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the own user can update an order!", ErrorCodes.CannotUpdate));
        }

        if (result.Order.OrderStatus != OrderStatusEnum.New)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Can not update the items of an already placed order!", ErrorCodes.CannotUpdate));
        }

        double currentPrice = result.Quantity * result.ShopItem.Price;
        double newPrice = orderItemUpdateRecord.Quantity * result.ShopItem.Price;
        result.Quantity = orderItemUpdateRecord.Quantity;
        result.Order.TotalPaid += (newPrice - currentPrice);
        await repository.UpdateAsync(result.Order, cancellationToken);
        await repository.UpdateAsync(result, cancellationToken);

        return ServiceResponse.ForSuccess();
    }
}