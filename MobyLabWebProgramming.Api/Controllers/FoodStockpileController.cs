using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobyLabWebProgramming.Infrastructure.Authorization;
using MobyLabWebProgramming.Infrastructure.Requests;
using MobyLabWebProgramming.Infrastructure.Responses;
using MobyLabWebProgramming.Services.Abstractions;
using MobyLabWebProgramming.Services.Authorization;
using MobyLabWebProgramming.Services.DataTransferObjects;

namespace MobyLabWebProgramming.Api.Controllers;

[ApiController] // This attribute specifies for the framework to add functionality to the controller such as binding multipart/form-data.
[Route("api/[controller]/[action]")] 
public class FoodStockpileController(ILogger<UserController> logger, IUserService userService, IFoodStockpileService foodStockpileService) : AuthorizedController(logger, userService)
{
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<FoodStockpileRecord>>> GetFoodstockpile([FromQuery] Guid id)
    {
        var currentUser = await GetCurrentUser();
        
        return currentUser.Result != null ? 
            FromServiceResponse(await foodStockpileService.GetStockpile(id)) : 
            ErrorMessageResult<FoodStockpileRecord>(currentUser.Error);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<FoodStockpileRecord>>> GetCurrentUserFoodStockpile()
    {
        var currentUser = await GetCurrentUser();
        
        return currentUser.Result != null ? 
            FromServiceResponse(await foodStockpileService.GetStockpile(currentUser.Result.Id)) : 
            ErrorMessageResult<FoodStockpileRecord>(currentUser.Error);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> UpdateFoodStockpile([FromQuery] FoodStockpileUpdateRecord foodStockpileUpdateRecord)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ? 
            FromServiceResponse(await foodStockpileService.UpdateStockpile(foodStockpileUpdateRecord, currentUser.Result)) :
            ErrorMessageResult(currentUser.Error);

    }
    
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> UpdateCurrentUserFoodStockpile([FromQuery] FoodStockpileRecord foodStockpileRecord)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ? 
            FromServiceResponse(await foodStockpileService.UpdateStockpile(new FoodStockpileUpdateRecord
            {
                Fish =  foodStockpileRecord.Fish,
                FishFood =  foodStockpileRecord.FishFood,
                Grains =  foodStockpileRecord.Grains,
                Meats =  foodStockpileRecord.Meats,
                Plants =  foodStockpileRecord.Plants,
                UserId = currentUser.Result.Id
            }, currentUser.Result)) :
            ErrorMessageResult(currentUser.Error);

    }
}