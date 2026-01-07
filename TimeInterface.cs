using System;
using System.Timers;

namespace SignalBook;

public class TimeInterface
{
    private static System.Timers.Timer clockTimer;
    public DateTime Time;

    public TimeInterface()
    {
        InitializeTime();
        SetTimer();
        Console.ReadLine();
        clockTimer.Stop();
        clockTimer.Dispose();
    }

    public void InitializeTime()
    {
        Console.WriteLine("Enter the time in a HH:MM:SS format\n>>>");
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
        } catch (Exception ex)
        {
            Console.Error.Write($"An error occurred: {ex.Message}. Reverting to current system time.\n");
            Time = DateTime.Now;
        }
    }

    private void SetTimer()
    {
        clockTimer = new System.Timers.Timer(1000);
        clockTimer.Elapsed += ClockTimerCallback;
        clockTimer.AutoReset = true;
        clockTimer.Enabled = true;
    }

    private void ClockTimerCallback(object o, ElapsedEventArgs e)
    {
        Time += TimeSpan.FromSeconds(1);
        Console.WriteLine($"Time is {Time}");
    }
}