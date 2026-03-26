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

public class AnimalSpeciesService(IRepository<WebAppDatabaseContext> repository) : IAnimalSpeciesService
{
    public async Task<ServiceResponse> AddSpecies(AnimalSpeciesRecord animalSpeciesRecord, UserRecord? requestingUser = null,
        CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin can add a new specie!", ErrorCodes.CannotAdd));
        }
        
        var result = await repository.GetAsync(new AnimalSpeciesSpec(animalSpeciesRecord.Specie), cancellationToken);
        if (result != null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Conflict, "The specie already exists!", ErrorCodes.UserAlreadyExists));
        }
        
        await repository.AddAsync(new AnimalSpecies
        {
            Specie = animalSpeciesRecord.Specie,
            Description =  animalSpeciesRecord.Description,
            FoodType =  animalSpeciesRecord.FoodType
        }, cancellationToken); // A new entity is created and persisted in the database
        
        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> DeleteSpecies(string species, UserRecord? requestingUser = null, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin can delete a specie!", ErrorCodes.CannotAdd));
        }
        
        var result = await repository.GetAsync(new AnimalSpeciesSpec(species), cancellationToken);
        if (result == null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.NotFound, "The specie dose not exist!", ErrorCodes.EntityNotFound));
        }
        
        await repository.DeleteAsync<AnimalSpecies>(result.Id, cancellationToken);

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse<AnimalSpeciesRecord>> GetSpecie(string species,
        CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(new AnimalSpeciesProjectionSpec(species), cancellationToken);
        
        return result != null ? 
            ServiceResponse.ForSuccess(result) : 
            ServiceResponse.FromError<AnimalSpeciesRecord>(new(HttpStatusCode.NotFound, "The specie dose not exist!", ErrorCodes.EntityNotFound)); // Pack the result or error into a ServiceResponse
    }

    public async Task<ServiceResponse<int>> GetSpeciesCount(CancellationToken cancellationToken = default)
    {
        return ServiceResponse.ForSuccess(await repository.GetCountAsync<AnimalSpecies>(cancellationToken));    
    }

    public async Task<ServiceResponse<PagedResponse<AnimalSpeciesRecord>>> GetSpecies(PaginationSearchQueryParams pagination,
        CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, new AnimalSpeciesProjectionSpec(pagination.Search, false), cancellationToken); // Use the specification and pagination API to get only some entities from the database.

        return ServiceResponse.ForSuccess(result);
    }
    
    public async Task<ServiceResponse> UpdateSpecies(AnimalSpeciesUpdateRecord animalSpeciesUpdateRecord, UserRecord? requestingUser = null,
        CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin can update a specie!", ErrorCodes.CannotAdd));
        }
        
        var entity = await repository.GetAsync(new AnimalSpeciesSpec(animalSpeciesUpdateRecord.Specie), cancellationToken);
        
        if (entity != null) // Verify if the user is not found, you cannot update a non-existing entity.
        {
            entity.Description = animalSpeciesUpdateRecord.Description;
            entity.FoodType = animalSpeciesUpdateRecord.FoodType;

            await repository.UpdateAsync(entity, cancellationToken); // Update the entity and persist the changes.
        }
        else
        {
            return ServiceResponse.FromError(new(HttpStatusCode.NotFound, "Specie does not exist!", ErrorCodes.EntityNotFound));
        }
        
        return ServiceResponse.ForSuccess();
    }
}