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
public class AnimalSpeciesController(ILogger<UserController> logger, IUserService userService, IAnimalSpeciesService animalSpeciesService) : AuthorizedController(logger, userService)
{
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<AnimalSpeciesRecord>>> GetSpecie([FromQuery] string specie)
    {
        var currentUser = await GetCurrentUser();
        
        return currentUser.Result != null ? 
            FromServiceResponse(await animalSpeciesService.GetSpecie(specie)) : 
            ErrorMessageResult<AnimalSpeciesRecord>(currentUser.Error);
    }
    
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<AnimalSpeciesRecord>>>> GetSpecies([FromQuery] PaginationSearchQueryParams pagination)
    {
        var currentUser = await GetCurrentUser();
        
        return currentUser.Result != null ? 
            FromServiceResponse(await animalSpeciesService.GetSpecies(pagination)) : 
            ErrorMessageResult<PagedResponse<AnimalSpeciesRecord>>(currentUser.Error);
    }
    
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<int>>> GetSpeciesCount()
    {
        var currentUser = await GetCurrentUser();
        
        return currentUser.Result != null ? 
            FromServiceResponse(await animalSpeciesService.GetSpeciesCount()) : 
            ErrorMessageResult<int>(currentUser.Error);
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<RequestResponse>> AddSpecie([FromQuery] AnimalSpeciesRecord animalSpeciesRecord)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null ?
            FromServiceResponse(await animalSpeciesService.AddSpecies(animalSpeciesRecord, currentUser.Result)) :
            ErrorMessageResult(currentUser.Error);
    }
    
    [Authorize]
    [HttpPut]
    public async Task<ActionResult<RequestResponse>> UpdateSpecie([FromQuery] AnimalSpeciesUpdateRecord animalSpeciesUpdateRecord)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null ?
            FromServiceResponse(await animalSpeciesService.UpdateSpecies(animalSpeciesUpdateRecord, currentUser.Result)) :
            ErrorMessageResult(currentUser.Error);
    }
    
    [Authorize]
    [HttpDelete]
    public async Task<ActionResult<RequestResponse>> DeleteSpecie([FromQuery] string specie)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null ?
            FromServiceResponse(await animalSpeciesService.DeleteSpecies(specie, currentUser.Result)) :
            ErrorMessageResult(currentUser.Error);
    }
}