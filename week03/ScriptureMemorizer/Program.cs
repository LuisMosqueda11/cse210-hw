/*
 * Exceeding Requirements:
 * 1. Added a difficulty level feature that allows the user to choose how many words to hide at a time.
 * 2. Enhanced the LoadScripturesFromFile method with additional error handling for file format issues.
 * 3. Modified the HideRandomWords method to only hide words that are not already hidden.
 * 4. Added a welcome message and instructions for the user.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ScriptureMemorizer
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Scripture> scriptures = LoadScripturesFromFile("week03/ScriptureMemorizer/scriptures.txt");
            if (scriptures.Count == 0)
            {
                Console.WriteLine("No scriptures found in the file.");
                return;
            }

            Random random = new Random();
            Scripture scripture = scriptures[random.Next(scriptures.Count)];

            Console.WriteLine("Welcome to Scripture Memorizer!");
            Console.WriteLine("Choose a difficulty level (1: Easy, 2: Medium, 3: Hard):");
            int difficulty = int.Parse(Console.ReadLine());
            int wordsToHide = difficulty * 2; 

            while (true)
            {
                Console.Clear();
                scripture.Display();

                if (scripture.AllWordsHidden())
                {
                    Console.WriteLine("\nAll words are hidden. Good job memorizing!");
                    break;
                }

                Console.Write("\nPress Enter to hide words or type 'quit' to exit: ");
                string userInput = Console.ReadLine().Trim().ToLower();

                if (userInput == "quit")
                {
                    Console.WriteLine("Goodbye!");
                    break;
                }

                scripture.HideRandomWords(wordsToHide);
            }
        }

        static List<Scripture> LoadScripturesFromFile(string filename)
        {
            List<Scripture> scriptures = new List<Scripture>();
            try
            {
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
                Console.WriteLine($"Looking for file at: {filePath}"); 

                foreach (string line in File.ReadAllLines(filePath))
                {
                    Console.WriteLine($"Reading line: {line}");

                    if (string.IsNullOrWhiteSpace(line)) 
                        continue;

                    string[] parts = line.Split('|');
                    if (parts.Length == 2)
                    {
                        string reference = parts[0].Trim();
                        string text = parts[1].Trim();

                        try
                        {
                            scriptures.Add(new Scripture(reference, text));
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine($"Invalid scripture format: {reference}. Error: {ex.Message}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Invalid line format: {line}");
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Scripture file not found.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return scriptures;
        }
    }

    class ScriptureReference
    {
        public string Book { get; }
        public int Chapter { get; }
        public int VerseStart { get; }
        public int? VerseEnd { get; }

        public ScriptureReference(string book, int chapter, int verseStart, int? verseEnd = null)
        {
            Book = book;
            Chapter = chapter;
            VerseStart = verseStart;
            VerseEnd = verseEnd;
        }

        public override string ToString()
        {
            return VerseEnd.HasValue
                ? $"{Book} {Chapter}:{VerseStart}-{VerseEnd}"
                : $"{Book} {Chapter}:{VerseStart}";
        }
    }

    class ScriptureWord
    {
        public string Text { get; }
        public bool IsHidden { get; private set; }

        public ScriptureWord(string text)
        {
            Text = text;
            IsHidden = false;
        }

        public void Hide()
        {
            IsHidden = true;
        }

        public override string ToString()
        {
            return IsHidden ? new string('_', Text.Length) : Text;
        }
    }

    class Scripture
    {
        public ScriptureReference Reference { get; }
        private List<ScriptureWord> Words { get; }

        public Scripture(string reference, string text)
        {
            if (string.IsNullOrWhiteSpace(reference))
            {
                throw new ArgumentException("Reference cannot be null or empty.");
            }
            string[] parts = reference.Split(new[] { ' ' }, 3, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 3)
            {
                throw new FormatException($"Invalid scripture reference format: '{reference}'. Expected format: 'Book Chapter:Verse'.");
            }


            string book = parts[0] + " " + parts[1];
            string[] chapterVerse = parts[2].Split(':');
            if (chapterVerse.Length != 2)
            {
                throw new FormatException("Invalid chapter:verse format. Expected format: 'Chapter:Verse'.");
            }

            int chapter;
            if (!int.TryParse(chapterVerse[0], out chapter))
            {
                throw new FormatException("Invalid chapter format. Chapter must be a number.");
            }

            string[] verses = chapterVerse[1].Split('-');
            int verseStart;
            if (!int.TryParse(verses[0], out verseStart))
            {
                throw new FormatException("Invalid verse format. Verse must be a number.");
            }

            int? verseEnd = null;
            if (verses.Length > 1)
            {
                if (int.TryParse(verses[1], out int tempVerseEnd))
                {
                    verseEnd = tempVerseEnd;
                    }
                    else
                    {
                        throw new FormatException("Invalid verse format. Verse must be a number.");
                        }
                        }


            Reference = new ScriptureReference(book, chapter, verseStart, verseEnd);
            Words = text.Split(' ').Select(word => new ScriptureWord(word)).ToList();
        }

        public void Display()
        {
            Console.WriteLine(Reference);
            Console.WriteLine(string.Join(" ", Words));
        }

        public void HideRandomWords(int numWords = 3)
        {
            var visibleWords = Words.Where(word => !word.IsHidden).ToList();
            if (visibleWords.Count == 0) return;

            Random random = new Random();
            for (int i = 0; i < Math.Min(numWords, visibleWords.Count); i++)
            {
                int index = random.Next(visibleWords.Count);
                visibleWords[index].Hide();
            }
        }

        public bool AllWordsHidden()
        {
            return Words.All(word => word.IsHidden);
        }
    }
}
