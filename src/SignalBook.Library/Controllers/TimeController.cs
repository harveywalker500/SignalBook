using System.Timers;

namespace SignalBook.Controllers;

public class TimeController
{
    private static System.Timers.Timer clockTimer;
    public DateTime Time;

    public TimeController()
    {
        InitializeTime();
        StartTimer();
    }

    private void InitializeTime()
    {
        Console.WriteLine("Enter the time in a HH:MM:SS format.");
        string? userInput = Console.ReadLine();

        if (string.IsNullOrEmpty(userInput)) { Time = DateTime.Now; }

        try
        {
            Time = DateTime.Parse(userInput);
        }
        catch (FormatException)
        {
            Console.Error.Write("Invalid time format. Reverting to current system time.\n");
            Time = DateTime.Now;
        }
        catch (Exception ex)
        {
            Console.Error.Write($"An error occurred: {ex.Message}. Reverting to current system time.\n");
            Time = DateTime.Now;
        }
    }

    public void StartTimer()
    {
        clockTimer = new System.Timers.Timer(1000);
        clockTimer.Elapsed += ClockTimerCallback;
        clockTimer.AutoReset = true;
        clockTimer.Enabled = true;
    }

    public void StopTimer(bool pause = false)
    {
        clockTimer.Stop();
        if (pause)
        {
            clockTimer.Dispose();
        }
    }

    private void ClockTimerCallback(object o, ElapsedEventArgs e)
    {
        Time += TimeSpan.FromSeconds(1);

        // For debugging purposes
        // Console.WriteLine($"Time is {Time.TimeOfDay}");
    }
}