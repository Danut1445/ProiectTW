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
public class AnimalController(ILogger<UserController> logger, IUserService userService, IAnimalService animalService) : AuthorizedController(logger, userService)
{
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<AnimalRecord>>> GetAnimal([FromQuery] Guid id)
    {
        var currentUser = await GetCurrentUser();
        
        return currentUser.Result != null ? 
            FromServiceResponse(await animalService.GetAnimal(id, currentUser.Result)) : 
            ErrorMessageResult<AnimalRecord>(currentUser.Error);
    }
    
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<AnimalRecord>>>> GetPagedAnimal([FromQuery] Guid userId, [FromQuery] PaginationSearchQueryParams pagination)
    {
        var currentUser = await GetCurrentUser();
        
        return currentUser.Result != null ? 
            FromServiceResponse(await animalService.GetPagedAnimals(userId, pagination, currentUser.Result)) : 
            ErrorMessageResult<PagedResponse<AnimalRecord>>(currentUser.Error);
    }
    
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<AnimalRecord>>>> GetPagedAnimalCurrentUser([FromQuery] PaginationSearchQueryParams pagination)
    {
        var currentUser = await GetCurrentUser();
        
        return currentUser.Result != null ? 
            FromServiceResponse(await animalService.GetPagedAnimalsCurrentUser(pagination, currentUser.Result)) : 
            ErrorMessageResult<PagedResponse<AnimalRecord>>(currentUser.Error);
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<RequestResponse>> AddAnimal([FromQuery] AnimalRecord animalRecord)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null ?
            FromServiceResponse(await animalService.AddAnimal(animalRecord, currentUser.Result)) :
            ErrorMessageResult(currentUser.Error);
    }
    
    [Authorize]
    [HttpPut]
    public async Task<ActionResult<RequestResponse>> UpdateAnimal([FromQuery] AnimalUpdateRecord animalUpdateRecord)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null ?
            FromServiceResponse(await animalService.UpdateAnimal(animalUpdateRecord, currentUser.Result)) :
            ErrorMessageResult(currentUser.Error);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> PlayWithAnimal([FromQuery] Guid animalId)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null ?
            FromServiceResponse(await animalService.PlayWithAnimal(animalId, currentUser.Result)) :
            ErrorMessageResult(currentUser.Error);
    }
    
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> FeedAnimal([FromQuery] Guid animalId)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null ?
            FromServiceResponse(await animalService.FeedAnimal(animalId, currentUser.Result)) :
            ErrorMessageResult(currentUser.Error);
    }
    
    [Authorize]
    [HttpDelete]
    public async Task<ActionResult<RequestResponse>> DeleteAnimal([FromQuery] Guid id)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null ?
            FromServiceResponse(await animalService.DeleteAnimal(id, currentUser.Result)) :
            ErrorMessageResult(currentUser.Error);
    }
}