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
public class ShopItemController(ILogger<UserController> logger, IUserService userService, IShopItemService shopItemService) : AuthorizedController(logger, userService)
{
    [Authorize]
    [HttpPut]
    public async Task<ActionResult<RequestResponse>> AddShopItem([FromQuery] ShopItemAddRecord shopItemAddRecord)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null ?
            FromServiceResponse(await shopItemService.AddAShopItem(shopItemAddRecord, currentUser.Result)) :
            ErrorMessageResult(currentUser.Error);
    }
    
    [HttpGet]
    public async Task<ActionResult<RequestResponse<ShopItemRecord>>> GetShopItem([FromQuery] Guid id)
    {
        return FromServiceResponse(await shopItemService.GetShopItem(id));
    }
    
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<ShopItemRecord>>>> GetPagedShopItems([FromQuery] PaginationSearchQueryParams pagination)
    {
        return FromServiceResponse(await shopItemService.GetShopItems(pagination));
    }
    
    [Authorize]
    [HttpPut]
    public async Task<ActionResult<RequestResponse>> UpdateAnimal([FromQuery]ShopItemUpdateRecord shopItemUpdateRecord)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null ?
            FromServiceResponse(await shopItemService.UpdateShopItem(shopItemUpdateRecord, currentUser.Result)) :
            ErrorMessageResult(currentUser.Error);
    }
    
    [Authorize]
    [HttpDelete]
    public async Task<ActionResult<RequestResponse>> DeleteShopItem([FromQuery] Guid id)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null ?
            FromServiceResponse(await shopItemService.DeleteShopItem(id, currentUser.Result)) :
            ErrorMessageResult(currentUser.Error);
    }
}