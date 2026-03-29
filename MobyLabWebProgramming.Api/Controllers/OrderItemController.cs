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
public class OrderItemController(ILogger<UserController> logger, IUserService userService, IOrderItemService orderItemService) : AuthorizedController(logger, userService)
{
    [Authorize]
    [HttpPut]
    public async Task<ActionResult<RequestResponse>> AddOrderItem([FromQuery] OrderItemAddRecord orderItemAddRecord)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null ?
            FromServiceResponse(await  orderItemService.AddOrderItem(orderItemAddRecord, currentUser.Result)) :
            ErrorMessageResult(currentUser.Error);
    }
    
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<OrderItemRecord>>> GetOrder([FromQuery] Guid id)
    {
        var currentUser = await GetCurrentUser();
        
        return currentUser.Result != null ? 
            FromServiceResponse(await orderItemService.GetOrderItem(id, currentUser.Result)) : 
            ErrorMessageResult<OrderItemRecord>(currentUser.Error);
    }
    
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<OrderItemRecord>>>> GetPagedOrderItems([FromQuery] Guid orderId, [FromQuery] PaginationSearchQueryParams pagination)
    {
        var currentUser = await GetCurrentUser();
        
        return currentUser.Result != null ? 
            FromServiceResponse(await orderItemService.GetPagedOrderItems(orderId, pagination, currentUser.Result)) : 
            ErrorMessageResult<PagedResponse<OrderItemRecord>>(currentUser.Error);
    }
    
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<OrderItemRecord>>>> GetPagedOrderItemsFromCurrentOrder([FromQuery] PaginationSearchQueryParams pagination)
    {
        var currentUser = await GetCurrentUser();
        
        return currentUser.Result != null ? 
            FromServiceResponse(await orderItemService.GetPagedOrderItemsCurrentNewOrder(pagination, currentUser.Result)) : 
            ErrorMessageResult<PagedResponse<OrderItemRecord>>(currentUser.Error);
    }
    
    [Authorize]
    [HttpPut]
    public async Task<ActionResult<RequestResponse>> UpdateOrder([FromQuery] OrderItemUpdateRecord orderItemUpdateRecord)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null ?
            FromServiceResponse(await orderItemService.UpdateOrderItem(orderItemUpdateRecord, currentUser.Result)) :
            ErrorMessageResult(currentUser.Error);
    }
    
    [Authorize]
    [HttpDelete]
    public async Task<ActionResult<RequestResponse>> DeleteCurrentOrder([FromQuery] Guid id)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null ?
            FromServiceResponse(await orderItemService.DeleteOrderItem(id, currentUser.Result)) :
            ErrorMessageResult(currentUser.Error);
    }
}