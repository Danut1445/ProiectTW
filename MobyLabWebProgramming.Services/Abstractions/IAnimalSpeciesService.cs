using MobyLabWebProgramming.Infrastructure.Requests;
using MobyLabWebProgramming.Infrastructure.Responses;
using MobyLabWebProgramming.Services.DataTransferObjects;

namespace MobyLabWebProgramming.Services.Abstractions;

public interface IAnimalSpeciesService
{
    public Task<ServiceResponse> AddSpecies(AnimalSpeciesRecord animalSpeciesRecord, UserRecord? requestingUser, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> DeleteSpecies(string species, UserRecord? requestingUser, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<AnimalSpeciesRecord>> GetSpecies(string species, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> UpdateSpecies(AnimalSpeciesUpdateRecord animalSpeciesUpdateRecord, UserRecord? requestingUser, CancellationToken cancellationToken = default);
}