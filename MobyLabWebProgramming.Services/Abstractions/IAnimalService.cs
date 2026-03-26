using MobyLabWebProgramming.Infrastructure.Requests;
using MobyLabWebProgramming.Infrastructure.Responses;
using MobyLabWebProgramming.Services.DataTransferObjects;

namespace MobyLabWebProgramming.Services.Abstractions;

public interface IAnimalService
{
    public Task<ServiceResponse> AddAnimal(AnimalRecord animalRecord, UserRecord? requestingUser, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> DeleteAnimal(Guid id, UserRecord? requestingUser, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<AnimalRecord>> GetAnimal(Guid id, UserRecord? requestingUser, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<PagedResponse<AnimalRecord>>> GetPagedAnimals(Guid userid, PaginationSearchQueryParams pagination, UserRecord? requestingUser, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<PagedResponse<AnimalRecord>>> GetPagedAnimalsCurrentUser(PaginationSearchQueryParams pagination, UserRecord requestingUser, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> UpdateAnimal(AnimalUpdateRecord animalUpdateRecord, UserRecord? requestingUser, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> FeedAnimal(Guid id,  UserRecord? requestingUser, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> PlayWithAnimal(Guid id, UserRecord? requestingUser, CancellationToken cancellationToken = default);
}