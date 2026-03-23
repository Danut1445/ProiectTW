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

public class FoodStockpileService(IRepository<WebAppDatabaseContext> repository) : IFoodStockpileService
{
    public async Task<ServiceResponse<FoodStockpileRecord>> GetStockpile(Guid userId,
        CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(new FoodStockpileProjectionSpec(userId), cancellationToken);
        
        return result != null ?
            ServiceResponse.ForSuccess(result) :
            ServiceResponse.FromError<FoodStockpileRecord>(CommonErrors.UserNotFound);
    }

    public async Task<ServiceResponse> UpdateStockpile(FoodStockpileUpdateRecord foodStockpile,
        UserRecord? requestingUser = null,
        CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin && requestingUser.Id != foodStockpile.UserId) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin or the own user can update the food stockpile!", ErrorCodes.CannotUpdate));
        }
        
        var entity = await repository.GetAsync(new FoodStockpileSpec(foodStockpile.UserId), cancellationToken);
        
        if (entity != null) // Verify if the user is not found, you cannot update a non-existing entity.
        {
            entity.Meats = entity.Meats + foodStockpile.Meats;
            entity.FishFood = entity.FishFood + foodStockpile.FishFood;
            entity.Fish = entity.Fish + foodStockpile.Fish;
            entity.Grains = entity.Grains + foodStockpile.Grains;
            entity.Plants = entity.Plants + foodStockpile.Plants;

            await repository.UpdateAsync(entity, cancellationToken); // Update the entity and persist the changes.
        }
        else
        {
            return ServiceResponse.FromError(new(HttpStatusCode.NotFound, "User does not exist!", ErrorCodes.EntityNotFound));
        }
        
        return ServiceResponse.ForSuccess();
    }
}