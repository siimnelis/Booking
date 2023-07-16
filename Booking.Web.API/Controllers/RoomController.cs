using Booking.Web.API.Extensions;
using Booking.Web.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Web.API.Controllers;

[ApiController]
[Route("rooms")]
[Authorize]
public class RoomsController
{
    private IRoomsService RoomsService { get; }
    private IDateTime DateTime { get; }

    public RoomsController(IRoomsService roomsService, IDateTime dateTime)
    {
        RoomsService = roomsService;
        DateTime = dateTime;
    }

    /// <remarks>
    /// Sample request:
    ///
    ///     Start: 2023-08-01
    ///     End: 2023-08-02
    ///
    /// </remarks>
    /// <summary>
    /// Return are room available for a booking for a period of stay..
    /// </summary>
    /// <response code="200">Rooms that are available are returned.</response>
    /// <response code="400">A exception was thrown with a message describing the problem.</response>
    [HttpGet("availableForPeriodOfStay")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string),StatusCodes.Status400BadRequest, "text/plain")]
    [Produces("application/json", "text/plain")]
    public IEnumerable<Room> GetRoomsAvailableForPeriodOfStay([FromQuery]PeriodOfStay periodOfStay)
    {
        return RoomsService.GetRoomsAvailableForPeriodOfStay(periodOfStay.Map(DateTime)).Map();
    }
}