using BookLendingSystem.Exceptions;
using System.Net;
using System.Text;

namespace BookLendingSystem.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Log incoming request details
                await LogRequestAsync(context);

                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred while processing request");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task LogRequestAsync(HttpContext context)
        {
            var request = context.Request;
            var sb = new StringBuilder();

            sb.AppendLine("====== Incoming Request ======");
            sb.AppendLine($"Timestamp     : {DateTime.UtcNow}");
            sb.AppendLine($"Method        : {request.Method}");
            sb.AppendLine($"Path          : {request.Path}");
            sb.AppendLine($"QueryString   : {request.QueryString}");
            sb.AppendLine($"Client IP     : {context.Connection.RemoteIpAddress}");

            if (request.Method == HttpMethods.Post || request.Method == HttpMethods.Put)
            {
                request.EnableBuffering();
                using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
                string body = await reader.ReadToEndAsync();
                request.Body.Position = 0;

                if (!string.IsNullOrWhiteSpace(body))
                    sb.AppendLine($"Body          : {body}");
            }

            sb.AppendLine("================================");

            var logger = context.RequestServices.GetRequiredService<ILogger<ExceptionHandlingMiddleware>>();
            logger.LogInformation(sb.ToString());
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            string message = "An unexpected error occurred.";

            switch (exception)
            {
                case BookNotFoundException:
                case UserNotFoundException:
                case DuplicateUsernameException:
                case LendingRecordNotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    message = exception.Message;
                    break;

                case BookUnavailableException:
                case BookAlreadyBorrowedException:
                case NoAvailableCopiesException:
                case DuplicateBorrowingException:
                    statusCode = HttpStatusCode.BadRequest;
                    message = exception.Message;
                    break;

            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsJsonAsync(new
            {
                error = message
            });
        }
    }
}
