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

public class AnimalServices(IRepository<WebAppDatabaseContext> repository) : IAnimalService
{
    public async Task<ServiceResponse> AddAnimal(AnimalRecord animalRecord, UserRecord? requestingUser,
        CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin can add a new animal!", ErrorCodes.CannotAdd));
        }
        
        var result = await repository.GetAsync(new AnimalSpeciesSpec(animalRecord.BornSpecie), cancellationToken);
        if (result == null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.NotFound, "The specie does not exists!", ErrorCodes.EntityNotFound));
        }
        
        var resultUser = await repository.GetAsync(new UserSpec(animalRecord.UserId), cancellationToken);
        if (resultUser == null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.NotFound, "The user does not exists!", ErrorCodes.EntityNotFound));
        }
        
        await repository.AddAsync(new Animal
        {
            Name = animalRecord.Name,
            Happiness = 100,
            Hunger = 100,
            UserId = animalRecord.UserId,
            BornSpecie =  animalRecord.BornSpecie,
        }, cancellationToken); // A new entity is created and persisted in the database
        
        return ServiceResponse.ForSuccess();
    }


    public async Task<ServiceResponse> DeleteAnimal(Guid id, UserRecord? requestingUser,
        CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(new AnimalSpec(id),  cancellationToken);
        if (result == null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.NotFound, "The animal does not exists!", ErrorCodes.EntityNotFound));
        }
        
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin && requestingUser.Id != result.UserId) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "You do not have permissions!", ErrorCodes.CannotAdd));
        }
        
        await repository.DeleteAsync<Animal>(result.Id, cancellationToken);

        return ServiceResponse.ForSuccess();
    }


    public async Task<ServiceResponse<AnimalRecord>> GetAnimal(Guid id, UserRecord? requestingUser,
        CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(new AnimalProjectionSpec(id),  cancellationToken);
        if (result == null)
        {
            return ServiceResponse.FromError<AnimalRecord>(new(HttpStatusCode.NotFound, "The animal does not exists!", ErrorCodes.EntityNotFound));
        }
        
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin && requestingUser.Id != result.UserId) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError<AnimalRecord>(new(HttpStatusCode.Forbidden, "You do not have permissions!", ErrorCodes.CannotAdd));
        }

        return ServiceResponse.ForSuccess(result);
    }

    public async Task<ServiceResponse<PagedResponse<AnimalRecord>>> GetPagedAnimals(Guid userid, PaginationSearchQueryParams pagination,
        UserRecord? requestingUser, CancellationToken cancellationToken = default)
    {
        var resultUser = await repository.GetAsync(new UserSpec(userid), cancellationToken);
        if (resultUser == null)
        {
            return ServiceResponse.FromError<PagedResponse<AnimalRecord>>(new(HttpStatusCode.NotFound, "The user does not exists!", ErrorCodes.EntityNotFound));
        }
        
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin && requestingUser.Id != userid) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError<PagedResponse<AnimalRecord>>(new(HttpStatusCode.Forbidden, "You do not have permissions!", ErrorCodes.CannotAdd));
        }
        
        var result = await repository.PageAsync(pagination, new AnimalProjectionSpec(pagination.Search, userid), cancellationToken); // Use the specification and pagination API to get only some entities from the database.

        return ServiceResponse.ForSuccess(result);
    }

    public async Task<ServiceResponse<PagedResponse<AnimalRecord>>> GetPagedAnimalsCurrentUser(
        PaginationSearchQueryParams pagination, UserRecord requestingUser,
        CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, new AnimalProjectionSpec(pagination.Search, requestingUser.Id), cancellationToken); // Use the specification and pagination API to get only some entities from the database.

        return ServiceResponse.ForSuccess(result);
    }
    
    public async Task<ServiceResponse> UpdateAnimal(AnimalUpdateRecord animalUpdateRecord, UserRecord? requestingUser,
        CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(new AnimalSpec(animalUpdateRecord.Id),  cancellationToken);
        if (result == null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.NotFound, "The animal does not exists!", ErrorCodes.EntityNotFound));
        }
        
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin && requestingUser.Id != result.UserId) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "You do not have permissions!", ErrorCodes.CannotAdd));
        }
        
        result.Name = animalUpdateRecord.Name;
        await repository.UpdateAsync(result, cancellationToken);
        
        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> FeedAnimal(Guid id, UserRecord? requestingUser,
        CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(new AnimalSpec(id),  cancellationToken);
        if (result == null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.NotFound, "The animal does not exists!", ErrorCodes.EntityNotFound));
        }
        
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin && requestingUser.Id != result.UserId) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "You do not have permissions!", ErrorCodes.CannotAdd));
        }
        
        int hunger = result.Hunger;
        var resultUser =  await repository.GetAsync(new UserSpec(requestingUser!.Id), cancellationToken);
        if (resultUser!.Role != UserRoleEnum.Admin)
        {
            if (result.AnimalSpecie.FoodType == FoodTypesEnum.Fish)
            {
                if (resultUser!.FoodStockpile.Fish > 0)
                {
                    resultUser!.FoodStockpile.Fish--;
                    hunger += 25;
                }
                else
                {
                    return ServiceResponse.FromError(new(HttpStatusCode.Conflict, "You do not have enough food!",
                        ErrorCodes.CannotUpdate));
                }
            }

            if (result.AnimalSpecie.FoodType == FoodTypesEnum.Plants)
            {
                if (resultUser!.FoodStockpile.Plants > 0)
                {
                    resultUser!.FoodStockpile.Plants--;
                    hunger += 25;
                }
                else
                {
                    return ServiceResponse.FromError(new(HttpStatusCode.Conflict, "You do not have enough food!",
                        ErrorCodes.CannotUpdate));
                }
            }

            if (result.AnimalSpecie.FoodType == FoodTypesEnum.FishFood)
            {
                if (resultUser!.FoodStockpile.FishFood > 0)
                {
                    resultUser!.FoodStockpile.FishFood--;
                    hunger += 25;
                }
                else
                {
                    return ServiceResponse.FromError(new(HttpStatusCode.Conflict, "You do not have enough food!",
                        ErrorCodes.CannotUpdate));
                }
            }

            if (result.AnimalSpecie.FoodType == FoodTypesEnum.Meat)
            {
                if (resultUser!.FoodStockpile.Meats > 0)
                {
                    resultUser!.FoodStockpile.Meats--;
                    hunger += 25;
                }
                else
                {
                    return ServiceResponse.FromError(new(HttpStatusCode.Conflict, "You do not have enough food!",
                        ErrorCodes.CannotUpdate));
                }
            }

            if (result.AnimalSpecie.FoodType == FoodTypesEnum.Grains)
            {
                if (resultUser!.FoodStockpile.Grains > 0)
                {
                    resultUser!.FoodStockpile.Grains--;
                    hunger += 25;
                }
                else
                {
                    return ServiceResponse.FromError(new(HttpStatusCode.Conflict, "You do not have enough food!",
                        ErrorCodes.CannotUpdate));
                }
            }
            await repository.UpdateAsync(resultUser.FoodStockpile, cancellationToken);
        }
        else
        {
            hunger += 25;
        }

        if (hunger > 100)
        {
            hunger = 100;
        }
        result.Hunger = hunger;
        await repository.UpdateAsync(result, cancellationToken);
        
        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> PlayWithAnimal(Guid id, UserRecord? requestingUser,
        CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(new AnimalSpec(id),  cancellationToken);
        if (result == null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.NotFound, "The animal does not exists!", ErrorCodes.EntityNotFound));
        }

        result.Happiness += 25;
        if (result.Happiness > 100)
        {
            result.Happiness = 100;
        }
        
        await repository.UpdateAsync(result, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
}