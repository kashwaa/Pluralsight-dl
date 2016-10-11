using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluralSight_dl.Core;
using System.IO;
using ConsoleProgressBar;

namespace PluralSight_dl
{
    class Program
    {
        private const string DISCLAIMER = "This software is for educational purposes only\nThe author will not be held responsible for misuse";
        static void Main(string[] args)
        {
            Console.WriteLine(DISCLAIMER);

            string username = ReadInput("PluralSight Username");
            string password = ReadInput("Pluralsight Password", hideInput:true);
            string courseName = ReadInput("Course name");

            Course course = null;
            try
            {
                course = new LinksExtractor().GetLinks(username, password, courseName);
            }
            catch(Exception e) 
            {
                Console.WriteLine(e.Message);
                return;
            }

            Downloader downloader = new Downloader();
            ProgressBar bar=null;

            //Display a progress bar when download starts
            downloader.DownloadBegin += (sender, e) => {
                bar = new ProgressBar(100, e.module.Title + "/" + e.clip.Title);
            };

            //Update the bar as download progresses
            downloader.DownloadProgressChanged += (sender, e) => {
                bar.SetProgress(e.Percentage);
            };

            downloader.DownloadCourse(course, Environment.CurrentDirectory);
        }

        /// <summary>
        /// Reads a string from the console
        /// </summary>
        /// <param name="caption">The title to display before the input</param>
        /// <param name="hideInput">If true will shadow the entered text</param>
        /// <returns>The entered text</returns>
        private static string ReadInput(string caption, bool hideInput = false)
        {
            Console.Write(caption+": ");
            StringBuilder data = new StringBuilder();
            var key=Console.ReadKey();
            while (key.Key!=ConsoleKey.Enter)
            {
                if (key.Key==ConsoleKey.Backspace&& data.Length > 0)
                {
                    Console.Write(" \b");//erase the previous character
                    data.Remove(data.Length - 1, 1);
                    key = Console.ReadKey();
                    continue;
                }
                data.Append(key.KeyChar);
                if (hideInput)
                {
                    Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                    Console.Write("*");
                }
                key = Console.ReadKey();
            }
            Console.SetCursorPosition(0, Console.CursorTop + 1);
            return data.ToString();
        }


    }


}

