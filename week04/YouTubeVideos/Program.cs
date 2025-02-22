using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        Video video1 = new Video("Mastering JavaScript", "Emily Carter", 750);
        Video video2 = new Video("Intro to Data Science", "Michael Reynolds", 1350);
        Video video3 = new Video("Game Development with Unity", "Sophia Martinez", 1100);

        video1.AddComment(new Comment("TechGeek99", "Awesome breakdown of JavaScript concepts!"));
        video1.AddComment(new Comment("CodeMaster21", "This really helped me understand closures."));
        video1.AddComment(new Comment("DevDude", "Looking forward to more advanced topics!"));

        video2.AddComment(new Comment("DataNerd", "Great introduction to data science!"));
        video2.AddComment(new Comment("StatsPro", "Well explained and easy to follow."));
        video2.AddComment(new Comment("MLBeginner", "Loved the practical examples."));

        video3.AddComment(new Comment("GameDevX", "Unity is such a powerful tool!"));
        video3.AddComment(new Comment("PixelCrafter", "This tutorial is super detailed, thanks!"));
        video3.AddComment(new Comment("IndieCoder", "Excited to build my own game!"));

        List<Video> videos = new List<Video> { video1, video2, video3 };

        foreach (var video in videos)
        {
            Console.WriteLine($"Title: {video.Title}");
            Console.WriteLine($"Author: {video.Author}");
            Console.WriteLine($"Length: {video.LengthInSeconds} seconds");
            Console.WriteLine($"Number of Comments: {video.GetNumberOfComments()}");

            Console.WriteLine("Comments:");
            foreach (var comment in video.GetComments())
            {
                Console.WriteLine($"- {comment.CommenterName}: {comment.CommentText}");
            }
            Console.WriteLine();
        }
    }
}
