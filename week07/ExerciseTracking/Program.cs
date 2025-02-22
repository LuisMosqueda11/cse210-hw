/*
# Showing Creativity and Exceeding Requirements
This program includes a creative addition: a menu system that allows the user to choose between kilometers and miles
for distance, speed, and pace calculations. The user's choice is applied to all activities, and the program
displays the results in the selected unit system. This enhances the program's flexibility and user interaction.
*/

using System;
using System.Collections.Generic;

public enum UnitSystem { Miles, Kilometers }

public class Activity
{
    private DateTime date;
    private int durationInMinutes;
    private UnitSystem unitSystem;

    public Activity(DateTime date, int durationInMinutes, UnitSystem unitSystem)
    {
        this.date = date;
        this.durationInMinutes = durationInMinutes;
        this.unitSystem = unitSystem;
    }

    public DateTime Date => date;
    public int DurationInMinutes => durationInMinutes;
    public UnitSystem UnitSystem => unitSystem;

    public virtual double GetDistance()
    {
        return 0; 
    }

    public virtual double GetSpeed()
    {
        return 0; 
    }

    public virtual double GetPace()
    {
        return 0; 
    }

    public string GetUnit()
    {
        return unitSystem == UnitSystem.Miles ? "miles" : "kilometers";
    }

    public virtual string GetSummary()
    {
        string unit = GetUnit();
        return $"{Date.ToShortDateString()} {GetType().Name} ({DurationInMinutes} min) - Distance {GetDistance():F1} {unit}, Speed {GetSpeed():F1} {(unitSystem == UnitSystem.Miles ? "mph" : "kph")}, Pace: {GetPace():F1} min per {unit}";
    }
}

public class Running : Activity
{
    private double distance;

    public Running(DateTime date, int durationInMinutes, double distance, UnitSystem unitSystem)
        : base(date, durationInMinutes, unitSystem)
    {
        this.distance = distance;
    }

    public override double GetDistance()
    {
        return distance;
    }

    public override double GetSpeed()
    {
        return (distance / DurationInMinutes) * 60;
    }

    public override double GetPace()
    {
        return DurationInMinutes / distance;
    }
}

public class Cycling : Activity
{
    private double speed;

    public Cycling(DateTime date, int durationInMinutes, double speed, UnitSystem unitSystem)
        : base(date, durationInMinutes, unitSystem)
    {
        this.speed = speed;
    }

    public override double GetDistance()
    {
        return (speed * DurationInMinutes) / 60;
    }

    public override double GetSpeed()
    {
        return speed;
    }

    public override double GetPace()
    {
        return 60 / speed;
    }
}

public class Swimming : Activity
{
    private int laps;

    public Swimming(DateTime date, int durationInMinutes, int laps, UnitSystem unitSystem)
        : base(date, durationInMinutes, unitSystem)
    {
        this.laps = laps;
    }

    public override double GetDistance()
    {
        double distanceInKm = laps * 50 / 1000.0;
        return UnitSystem == UnitSystem.Miles ? distanceInKm * 0.62 : distanceInKm;
    }

    public override double GetSpeed()
    {
        return (GetDistance() / DurationInMinutes) * 60;
    }

    public override double GetPace()
    {
        return DurationInMinutes / GetDistance();
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Choose a unit system:");
        Console.WriteLine("1. Miles");
        Console.WriteLine("2. Kilometers");
        Console.Write("Enter your choice (1 or 2): ");
        int choice = int.Parse(Console.ReadLine());

        UnitSystem unitSystem = choice == 1 ? UnitSystem.Miles : UnitSystem.Kilometers;

        List<Activity> activities = new List<Activity>
        {
            new Running(new DateTime(2022, 11, 3), 30, 3.0, unitSystem),
            new Cycling(new DateTime(2022, 11, 3), 30, 9.7, unitSystem),
            new Swimming(new DateTime(2022, 11, 3), 30, 20, unitSystem)
        };

        Console.WriteLine("\nActivity Summaries:");
        foreach (var activity in activities)
        {
            Console.WriteLine(activity.GetSummary());
        }
    }
}
