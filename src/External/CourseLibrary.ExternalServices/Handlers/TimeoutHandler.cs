namespace CourseLibrary.ExternalServices.Handlers;

public sealed class TimeoutHandler : DelegatingHandler
{
    private readonly TimeSpan _timeout;

    public TimeoutHandler(TimeSpan timeout) => _timeout = timeout;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(_timeout);
        return await base.SendAsync(request, cts.Token);
    }
}

