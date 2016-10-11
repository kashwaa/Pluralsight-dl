using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleProgressBar
{
    class Program
    {
        static void Main(string[] args)
        {
            ProgressBar pb = new ProgressBar(3,"Downloading");
            for (int i = 1; i <= 3; i++)
            {
                pb.SetProgress(i);
                Thread.Sleep(500);
            }

            pb = new ProgressBar(3, "Downloading");
            for (int i = 1; i <= 3; i++)
            {
                pb.SetProgress(i);
                Thread.Sleep(500);
            }


            pb = new ProgressBar(3, "Downloading");
            for (int i = 1; i <= 3; i++)
            {
                pb.SetProgress(i);
                Thread.Sleep(500);
            }
        }
    }
  
}
