using MobyLabWebProgramming.Infrastructure.Requests;
using MobyLabWebProgramming.Infrastructure.Responses;
using MobyLabWebProgramming.Services.DataTransferObjects;

namespace MobyLabWebProgramming.Services.Abstractions;

public interface IFoodStockpileService
{
    public Task<ServiceResponse<FoodStockpileRecord>> GetStockpile(Guid userId, CancellationToken cancellationToken = default);
    
    public Task<ServiceResponse> UpdateStockpile(FoodStockpileUpdateRecord foodStockpile, UserRecord? requestingUser = null,
        CancellationToken cancellationToken = default);
}