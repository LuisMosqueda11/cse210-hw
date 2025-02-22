/*
# Showing Creativity and Exceeding Requirements
To exceed the core requirements, I added a **leveling system** to enhance the gamification aspect of the program.
- The user levels up every 1000 points.
- The current level is displayed alongside the total score when the user views their score.
This feature encourages users to stay motivated by providing a sense of progression and achievement beyond just points.
*/

using System;
using System.Collections.Generic;
using System.IO;

namespace EternalQuest
{
    abstract class Goal
    {
        protected string _name;
        protected string _description;
        protected int _points;
        protected bool _isComplete;

        public Goal(string name, string description, int points)
        {
            _name = name;
            _description = description;
            _points = points;
            _isComplete = false;
        }

        public abstract void RecordEvent();
        public abstract string GetProgress();
        public virtual string GetGoalStatus()
        {
            return _isComplete ? "[X]" : "[ ]";
        }

        public int GetPoints()
        {
            return _points;
        }

        public string GetName()
        {
            return _name;
        }

        public bool IsComplete()
        {
            return _isComplete;
        }
    }

    class SimpleGoal : Goal
    {
        public SimpleGoal(string name, string description, int points) : base(name, description, points) { }

        public override void RecordEvent()
        {
            _isComplete = true;
        }

        public override string GetProgress()
        {
            return GetGoalStatus();
        }
    }

    class EternalGoal : Goal
    {
        public EternalGoal(string name, string description, int points) : base(name, description, points) { }

        public override void RecordEvent()
        {
        }

        public override string GetProgress()
        {
            return GetGoalStatus();
        }
    }

    class ChecklistGoal : Goal
    {
        private int _targetCount;
        private int _currentCount;
        private int _bonusPoints;

        public ChecklistGoal(string name, string description, int points, int targetCount, int bonusPoints)
            : base(name, description, points)
        {
            _targetCount = targetCount;
            _currentCount = 0;
            _bonusPoints = bonusPoints;
        }

        public override void RecordEvent()
        {
            _currentCount++;
            if (_currentCount >= _targetCount)
            {
                _isComplete = true;
                _points += _bonusPoints; 
            }
        }

        public override string GetProgress()
        {
            return $"{GetGoalStatus()} Completed {_currentCount}/{_targetCount} times";
        }
    }

    class Program
    {
        private static List<Goal> _goals = new List<Goal>();
        private static int _totalScore = 0;

        static void Main(string[] args)
        {
            LoadGoals(); 
            bool running = true;

            while (running)
            {
                Console.WriteLine("\nEternal Quest Program");
                Console.WriteLine("1. View Goals");
                Console.WriteLine("2. Create New Goal");
                Console.WriteLine("3. Record Event");
                Console.WriteLine("4. View Score");
                Console.WriteLine("5. Save and Exit");
                Console.Write("Choose an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewGoals();
                        break;
                    case "2":
                        CreateGoal();
                        break;
                    case "3":
                        RecordEvent();
                        break;
                    case "4":
                        ViewScore();
                        break;
                    case "5":
                        SaveGoals();
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        static void ViewGoals()
        {
            Console.WriteLine("\nYour Goals:");
            for (int i = 0; i < _goals.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_goals[i].GetProgress()} - {_goals[i].GetName()}");
            }
        }

        static void CreateGoal()
        {
            Console.Write("Enter goal type (1. Simple, 2. Eternal, 3. Checklist): ");
            string type = Console.ReadLine();
            Console.Write("Enter goal name: ");
            string name = Console.ReadLine();
            Console.Write("Enter goal description: ");
            string description = Console.ReadLine();
            Console.Write("Enter points for this goal: ");
            int points = int.Parse(Console.ReadLine());

            switch (type)
            {
                case "1":
                    _goals.Add(new SimpleGoal(name, description, points));
                    break;
                case "2":
                    _goals.Add(new EternalGoal(name, description, points));
                    break;
                case "3":
                    Console.Write("Enter target count: ");
                    int targetCount = int.Parse(Console.ReadLine());
                    Console.Write("Enter bonus points: ");
                    int bonusPoints = int.Parse(Console.ReadLine());
                    _goals.Add(new ChecklistGoal(name, description, points, targetCount, bonusPoints));
                    break;
                default:
                    Console.WriteLine("Invalid goal type.");
                    break;
            }
        }

        static void RecordEvent()
        {
            ViewGoals();
            Console.Write("Enter the number of the goal you accomplished: ");
            int index = int.Parse(Console.ReadLine()) - 1;

            if (index >= 0 && index < _goals.Count)
            {
                _goals[index].RecordEvent();
                _totalScore += _goals[index].GetPoints();
                Console.WriteLine("Event recorded!");
            }
            else
            {
                Console.WriteLine("Invalid goal number.");
            }
        }

        static void ViewScore()
        {
            int level = _totalScore / 1000; 
            Console.WriteLine($"\nYour total score: {_totalScore} points");
            Console.WriteLine($"Your level: {level}");
        }

        static void SaveGoals()
        {
            using (StreamWriter writer = new StreamWriter("goals.txt"))
            {
                writer.WriteLine(_totalScore);
                foreach (var goal in _goals)
                {
                    writer.WriteLine($"{goal.GetType().Name}|{goal.GetName()}|{goal.GetPoints()}");
                }
            }
        }

        static void LoadGoals()
        {
            if (File.Exists("goals.txt"))
            {
                using (StreamReader reader = new StreamReader("goals.txt"))
                {
                    _totalScore = int.Parse(reader.ReadLine());
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split('|');
                        string type = parts[0];
                        string name = parts[1];
                        int points = int.Parse(parts[2]);

                        switch (type)
                        {
                            case "SimpleGoal":
                                _goals.Add(new SimpleGoal(name, "", points));
                                break;
                            case "EternalGoal":
                                _goals.Add(new EternalGoal(name, "", points));
                                break;
                            case "ChecklistGoal":
                                _goals.Add(new ChecklistGoal(name, "", points, 0, 0));
                                break;
                        }
                    }
                }
            }
        }
    }
}
