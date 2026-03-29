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

public class ShopItemServices(IRepository<WebAppDatabaseContext> repository) : IShopItemService
{
    public async Task<ServiceResponse> AddAShopItem(ShopItemAddRecord shopItemAddRecord, UserRecord? requestingUser,
        CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin && requestingUser.Role != UserRoleEnum.Personnel) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only staff can add a new shop item!", ErrorCodes.CannotAdd));
        }
        
        await repository.AddAsync(new ShopItem
        {
            Name =  shopItemAddRecord.Name,
            Description = shopItemAddRecord.Description,
            Price = shopItemAddRecord.Price,
        }, cancellationToken); // A new entity is created and persisted in the database
        
        return ServiceResponse.ForSuccess();
    }


    public async Task<ServiceResponse> DeleteShopItem(Guid id, UserRecord? requestingUser,
        CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin && requestingUser.Role != UserRoleEnum.Personnel) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only staff can delete shop item!", ErrorCodes.CannotDelete));
        }
        
        var result = await repository.GetAsync(new ShopItemSpec(id), cancellationToken);
        if (result == null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.NotFound, "Shop item does not exist", ErrorCodes.CannotDelete));
        }

        if (result.OrderItems != null! && result.OrderItems.Count > 0)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Can not delete an item that has been used", ErrorCodes.CannotDelete));
        }
        
        await repository.DeleteAsync<ShopItem>(result.Id, cancellationToken);

        return ServiceResponse.ForSuccess();
    }


    public async Task<ServiceResponse<ShopItemRecord>> GetShopItem(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(new ShopItemProjectionSpec(id), cancellationToken);
        if (result == null)
        {
            return ServiceResponse.FromError<ShopItemRecord>(new(HttpStatusCode.NotFound, "Shop item does not exist", ErrorCodes.EntityNotFound));
        }
       
        return ServiceResponse.ForSuccess(result);
    }


    public async Task<ServiceResponse<PagedResponse<ShopItemRecord>>> GetShopItems(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, new ShopItemProjectionSpec(), cancellationToken); // Use the specification and pagination API to get only some entities from the database.

        return ServiceResponse.ForSuccess(result);
    }


    public async Task<ServiceResponse> UpdateShopItem(ShopItemUpdateRecord shopItemUpdateRecord, UserRecord? requestingUser,
        CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin && requestingUser.Role != UserRoleEnum.Personnel) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only staff can update shop item!", ErrorCodes.CannotUpdate));
        }
        
        var result = await repository.GetAsync(new ShopItemSpec(shopItemUpdateRecord.Id), cancellationToken);
        if (result == null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.NotFound, "Shop item does not exist", ErrorCodes.CannotUpdate));
        }

        if (result.OrderItems != null! && result.OrderItems.Count > 0)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Can not update an item that has been used", ErrorCodes.CannotUpdate));
        }

        if (shopItemUpdateRecord.Description != null)
        {
            result.Description = shopItemUpdateRecord.Description;
        }

        if (shopItemUpdateRecord.Price != null)
        {
            result.Price = shopItemUpdateRecord.Price.Value;
        }

        if (shopItemUpdateRecord.Name != null)
        {
            result.Name = shopItemUpdateRecord.Name;
        }
        
        await repository.UpdateAsync(result, cancellationToken);

        return ServiceResponse.ForSuccess();
    }
}