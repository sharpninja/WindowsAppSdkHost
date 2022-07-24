namespace CompleteWithInstaller.Activation;

// Extend this class to implement new ActivationHandlers. See DefaultActivationHandler for an example.
// https://github.com/microsoft/TemplateStudio/blob/main/docs/WinUI/activation.md
public abstract class ActivationHandler<T> : IActivationHandler
    where T : class
{
    // Override this method to add the logic for whether to handle the activation.
    // ReSharper disable once UnusedParameter.Global
    protected virtual bool CanHandleInternal(T _) => true;

    // Override this method to add the logic for your activation handler.
    protected abstract Task HandleInternalAsync(T? args);

    public bool CanHandle(object args) => args is T t && CanHandleInternal(t);

    public async Task HandleAsync(object args) => await HandleInternalAsync(args as T);
}
