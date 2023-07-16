using Booking.Web.API.Extensions;
using Booking.Web.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Web.API.Controllers;

[ApiController]
[Route("bookings")]
[Authorize]
public class BookingsController : ControllerBase
{
    private IBookingService BookingService { get; }
    private IDateTime DateTime { get; }
    
    public BookingsController(IBookingService bookingService, IDateTime dateTime)
    {
        BookingService = bookingService;
        DateTime = dateTime;
    }

    /// <summary>
    /// Returns authenticated users bookings.
    /// </summary>
    /// <response code="200">Returns all bookings for a room</response>
    /// <response code="400">A exception was thrown with a message describing the problem.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string),StatusCodes.Status400BadRequest, "text/plain")]
    public IEnumerable<Models.Booking> GetUserBookings()
    {
        return BookingService.GetUserBookings().Map();
    }
    
    /// <summary>
    /// Returns all bookings for a room
    /// </summary>
    /// <response code="200">Returns all bookings for a room</response>
    /// <response code="400">A exception was thrown with a message describing the problem.</response>
    [HttpGet("/rooms/{roomId}/bookings")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string),StatusCodes.Status400BadRequest, "text/plain")]
    public IEnumerable<Models.Booking> GetBookingsForARoom(int roomId)
    {
        return BookingService.GetBookingsForARoom(roomId).Map();
    }
    
    /// <summary>
    /// Books a room
    /// </summary>
    /// <response code="201">Returns the newly created booking</response>
    /// <response code="400">A exception was thrown with a message describing the problem.</response>
    /// <remarks>
    /// Sample body:
    ///
    /// {
    ///  "start": "2023-08-01",
    ///  "end": "2023-08-02"
    /// }
    ///
    /// </remarks>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string),StatusCodes.Status400BadRequest, "text/plain")]
    public Models.Booking BookRoom(int roomId, PeriodOfStay periodOfStay)
    {
        Response.StatusCode = StatusCodes.Status201Created;
        return BookingService.BookRoom(roomId, periodOfStay.Map(DateTime)).Map();
    }
    
    /// <summary>
    /// Books the cheapest room available.
    /// </summary>
    /// <response code="201">Returns the newly created booking</response>
    /// <response code="400">A exception was thrown with a message describing the problem.</response>
    /// <remarks>
    /// Sample body:
    ///
    /// {
    ///  "start": "2023-08-01",
    ///  "end": "2023-08-02"
    /// }
    ///
    /// </remarks>
    [HttpPost(":bookCheapestRoom")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string),StatusCodes.Status400BadRequest, "text/plain")]
    public Models.Booking BookCheapestRoom(PeriodOfStay periodOfStay)
    {
        Response.StatusCode = StatusCodes.Status201Created;
        return BookingService.BookCheapestRoom(periodOfStay.Map(DateTime)).Map();
    }
    
    /// <summary>
    /// Cancels the booking, if booking is already canceled, no errors are thrown.
    /// </summary>
    /// <response code="200">Cancels the booking.</response>
    /// <response code="400">A exception was thrown with a message describing the problem.</response>
    [HttpPost("{bookingId}/:cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string),StatusCodes.Status400BadRequest, "text/plain")]
    public void CancelBooking(int bookingId)
    {
        Response.StatusCode = StatusCodes.Status204NoContent;
        BookingService.CancelBooking(bookingId);
    }
    
}