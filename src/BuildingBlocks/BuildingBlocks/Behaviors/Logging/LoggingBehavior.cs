using MediatR;
using System.Diagnostics;

namespace BuildingBlocks.Behaviors.Logging
{
    public class LoggingBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, IRequest<TResponse>
        where TResponse : notnull
    {
        private readonly IAppLogger _logger;

        public LoggingBehavior(IAppLogger logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[START] Handle request={Request} - Response={Response} - RequestData={RequestData}",
                typeof(TRequest).Name,
                typeof(TResponse).Name,
                request);

            var timer = new Stopwatch();
            timer.Start();

            var response = await next(cancellationToken);

            timer.Stop();
            var timeTaken = timer.Elapsed;

            if (timeTaken.Seconds > 3)
            {
                _logger.LogWarning("[PERFORMANCE] The request {Request} took {TimeTaken}",
                    typeof(TRequest).Name,
                    timeTaken.Seconds);
            }

            _logger.LogInformation("[END] Handled {Request} with {Response}",
                 typeof(TRequest).Name,
                 typeof(TResponse).Name);

            return response;
        }
    }
}
