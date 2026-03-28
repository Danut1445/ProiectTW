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
public class OrderController(ILogger<UserController> logger, IUserService userService, IOrderService orderService) : AuthorizedController(logger, userService)
{
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<OrderRecord>>> GetOrder([FromQuery] Guid id)
    {
        var currentUser = await GetCurrentUser();
        
        return currentUser.Result != null ? 
            FromServiceResponse(await orderService.GetOrder(id, currentUser.Result)) : 
            ErrorMessageResult<OrderRecord>(currentUser.Error);
    }
    
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<OrderRecord>>>> GetPagedOrders([FromQuery] PaginationSearchQueryParams pagination)
    {
        var currentUser = await GetCurrentUser();
        
        return currentUser.Result != null ? 
            FromServiceResponse(await orderService.GetOrders(pagination, currentUser.Result)) : 
            ErrorMessageResult<PagedResponse<OrderRecord>>(currentUser.Error);
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<RequestResponse>> AddOrder()
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null ?
            FromServiceResponse(await  orderService.AddNewOrder(currentUser.Result)) :
            ErrorMessageResult(currentUser.Error);
    }
    
    [Authorize]
    [HttpPut]
    public async Task<ActionResult<RequestResponse>> UpdateOrder([FromQuery] OrderUpdateRecord orderUpdateRecord)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null ?
            FromServiceResponse(await orderService.UpdateOrder(orderUpdateRecord, currentUser.Result)) :
            ErrorMessageResult(currentUser.Error);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> PlaceOrder()
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null ?
            FromServiceResponse(await orderService.PlaceOrder(currentUser.Result)) :
            ErrorMessageResult(currentUser.Error);
    }

    
    [Authorize]
    [HttpDelete]
    public async Task<ActionResult<RequestResponse>> DeleteCurrentOrder()
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null ?
            FromServiceResponse(await orderService.DeleteCurrentOrder(currentUser.Result)) :
            ErrorMessageResult(currentUser.Error);
    }
}