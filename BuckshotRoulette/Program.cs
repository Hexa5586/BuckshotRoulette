// Code Stat Regex: ^b*[^:b#/]+.*$
// Publish: dotnet publish -c Release -r <platform> -p:PublishSingleFile=true --self-contained true -p:DebugType=none

/// <summary>
/// The entry point of the Buckshot Roulette application.
/// Initializes the game context and runs the execution loop until an exit signal (1) is received.
/// </summary>

Console.OutputEncoding = System.Text.Encoding.UTF8;

var context = new BuckshotRoulette.Simplified.Contexts.GlobalContext();

context.Configs.Initialize();
context.Locale.LoadLocale(context.Configs.Language);

context.CurrentState = new BuckshotRoulette.Simplified.States.SplashStates.OperatingState(); // Start with the Splash State

// The game loop: continues executing states until a state returns 1 (Ending State)
while (true)
{
    int state_return = 0;

    try
    {
        state_return = context.Execute();
        // The message will be reset to null after Operating State, so there's no need to manually clear it here
    }
    catch (Exception ex)
    {
        // Capture any exceptions during state execution and store the error message
        context.ErrorMessage = ex.Message;
    }

    if (state_return == 0)
    {
        // Normal execution, continue to next iteration
    }
    else
    {
        // Ending State reached, break the loop to end the game
        break;
    }
    
}