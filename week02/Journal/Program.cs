using System;
using System.Collections.Generic;
using System.IO;

class Entry
{
    public string Date { get; set; }
    public string Prompt { get; set; }
    public string Response { get; set; }
    
    public Entry(string date, string prompt, string response)
    {
        Date = date;
        Prompt = prompt;
        Response = response;
    }

    public override string ToString()
    {
        return $"Date: {Date} - Prompt: {Prompt}\n{Response}\n";
    }
}

class Journal
{
    private List<Entry> entries = new List<Entry>();
    private List<string> savedFilenames = new List<string>();

    public void AddEntry(string prompt, string response)
    {
        entries.Add(new Entry(DateTime.Now.ToString("MM/dd/yyyy"), prompt, response));
    }

    public void DisplayEntries()
    {
        if (entries.Count == 0)
        {
            Console.WriteLine("No journal entries found.");
            return;
        }
        foreach (var entry in entries)
        {
            Console.WriteLine(entry);
        }
    }

    public void SaveToFile(string filename)
    {
        using (StreamWriter writer = new StreamWriter(filename))
        {
            foreach (var entry in entries)
            {
                writer.WriteLine($"{entry.Date}|{entry.Prompt}|{entry.Response}");
            }
        }
        savedFilenames.Add(filename); 
        Console.WriteLine("Journal saved successfully.");
    }

    public void LoadFromFile(string filename)
    {
        if (!File.Exists(filename))
        {
            Console.WriteLine("File not found. Please check the filename and try again.");
            return;
        }
        entries.Clear();
        foreach (var line in File.ReadAllLines(filename))
        {
            var parts = line.Split('|');
            if (parts.Length == 3)
                entries.Add(new Entry(parts[0], parts[1], parts[2]));
        }
        Console.WriteLine("Journal loaded successfully.");
    }

    public List<string> GetSavedFilenames()
    {
        return savedFilenames;
    }
}

class PromptGenerator
{
    private static List<string> prompts = new List<string>
    {
        "Who was the most interesting person I interacted with today?",
        "What was the best part of my day?",
        "How did I see the hand of the Lord in my life today?",
        "What was the strongest emotion I felt today?",
        "If I had one thing I could do over today, what would it be?"
    };
    
    public static string GetRandomPrompt()
    {
        Random rnd = new Random();
        return prompts[rnd.Next(prompts.Count)];
    }
}

class Program
{
    static void Main()
    {
        Journal journal = new Journal();
        string choice;

        do
        {
            Console.WriteLine("\nWelcome to your Journal! Please select one of the following options:");
            Console.WriteLine("1. Write a new entry");
            Console.WriteLine("2. Display all entries");
            Console.WriteLine("3. Load entries from a file");
            Console.WriteLine("4. Save entries to a file");
            Console.WriteLine("5. Quit the application");
            Console.Write("What would you like to do? ");
            choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    string prompt = PromptGenerator.GetRandomPrompt();
                    Console.WriteLine("\nPrompt: " + prompt);
                    Console.Write("Your response: ");
                    string response = Console.ReadLine();
                    journal.AddEntry(prompt, response);
                    break;
                case "2":
                    journal.DisplayEntries();
                    break;
                case "3":
                    var savedFilenamesForLoad = journal.GetSavedFilenames();
                    if (savedFilenamesForLoad.Count > 0)
                    {
                        Console.WriteLine("Previously used filenames:");
                        for (int i = 0; i < savedFilenamesForLoad.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {savedFilenamesForLoad[i]}");
                        }
                        Console.WriteLine($"{savedFilenamesForLoad.Count + 1}. Enter a new filename");
                    }
                    else
                    {
                        Console.WriteLine("No previously used filenames. Please enter a new filename.");
                    }

                    Console.Write("Please select a filename (1 to {0} or {1} for a new filename): ", 
                    savedFilenamesForLoad.Count + 1, savedFilenamesForLoad.Count + 1);
                    string filenameChoiceLoad = Console.ReadLine();
                    string loadFile;
                    if (int.TryParse(filenameChoiceLoad, out int selectedIndex) && selectedIndex >= 1 && selectedIndex <= savedFilenamesForLoad.Count)
                    {
                        loadFile = savedFilenamesForLoad[selectedIndex - 1];
                    }
                    else
                    {
                        Console.Write("Please enter the new filename: ");
                        loadFile = Console.ReadLine();
                    }
                    journal.LoadFromFile(loadFile);
                    break;
                case "4":
                    var savedFilenames = journal.GetSavedFilenames();
                    if (savedFilenames.Count > 0)
                    {
                        Console.WriteLine("Previously used filenames:");
                        foreach (var filename in savedFilenames)
                        {
                            Console.WriteLine("- " + filename);
                        }
                    }
                    Console.Write("Please enter the filename to save entries to: ");
                    string saveFile = Console.ReadLine();
                    journal.SaveToFile(saveFile);
                    break;
                case "5":
                    Console.WriteLine("Thank you for using the Journal application. Goodbye!");
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please select a valid option.");
                    break;
            }
        } while (choice != "5");
    }
}
