namespace CompleteWithInstaller.Activation;

public sealed class DefaultActivationHandler : ActivationHandler<LaunchActivatedEventArgs>
{
    private readonly INavigationService _navigationService;

    public DefaultActivationHandler(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
        // None of the ActivationHandlers has handled the activation.
        => _navigationService.Frame.Content == null;

    protected async override Task HandleInternalAsync(LaunchActivatedEventArgs? args)
    {
        _navigationService.NavigateTo(typeof(MainViewModel).FullName ?? throw new
            InvalidOperationException(), args?.Arguments ?? ""
        );

        await Task.CompletedTask;
    }
}
