namespace DalleImageGeneratorWithSemanticKernel;



public static class ConsoleUtil
{


    /// <summary>
    /// Displays a rotating dash animation in the console until the provided cancellation token is triggered.
    /// </summary>
    /// <param name="token">A <see cref="CancellationToken"/> used to signal the cancellation of the animation.</param>
    /// <remarks>
    /// The method cycles through a set of characters ('-', '\', '|', '/') to create a rotating effect.
    /// The animation updates every 200 milliseconds and restores the console's foreground color after each update.
    /// </remarks>
    public static void RotateDash(CancellationToken token)
    {
        int index = 0;

        while (true)
        {
            if (token.IsCancellationRequested)
            {
                break;
            }
            char[] dash = { '-', '\\', '|', '/' };
            var previousColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(dash[index]);
            index = (index + 1) % dash.Length;
            Thread.Sleep(200); // Pause for 1000 milliseconds

            Console.ForegroundColor = previousColor;
        }
    }

}








