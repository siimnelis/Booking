using Booking.Exceptions;

namespace Booking.Web.API.Middleware;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    
    public ExceptionHandlerMiddleware(RequestDelegate requestDelegate)
    {
        _next = requestDelegate;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (UnAuthorizedAccessException)
        {
            await WriteErrorResponse(httpContext, "UnAuthorized", StatusCodes.Status403Forbidden);
        }
        catch (CantCancelBooking3DaysBeforePeriodOfStayException)
        {
            await WriteErrorResponse(httpContext, "Can't cancel booking three days before period of stay.");
        }
        catch (NoBookingFoundException)
        {
            await WriteErrorResponse(httpContext, "No booking found.");
        }
        catch (PeriodOfStayCantBeInThePastException)
        {
            await WriteErrorResponse(httpContext, "Period of stat can't be in the past.");
        }
        catch (PeriodOfStayMustBeAtleastForADayException)
        {
            await WriteErrorResponse(httpContext, "Period of stay must be at least for a day.");
        }
        catch (RoomAlreadyBookedForPeriodOfStayException)
        {
            await WriteErrorResponse(httpContext, "Room already booked for the period of stay.");
        }
        catch (RoomNotFoundException)
        {
            await WriteErrorResponse(httpContext, "Room not found.");
        }
        catch (UserNotFoundException)
        {
            await WriteErrorResponse(httpContext, "User not found.");
        }
        catch (BookingException)
        {
            await WriteErrorResponse(httpContext, "");
        }
    }

    private async Task WriteErrorResponse(HttpContext httpContext, string message, int statusCode = StatusCodes.Status400BadRequest)
    {
        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.Headers.ContentType = "text/plain";

        await httpContext.Response.WriteAsync(message);
    }
}