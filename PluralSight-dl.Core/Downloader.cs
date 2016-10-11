using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PluralSight_dl.Core
{
   public class Downloader
    {
        public delegate void DownloadBeginEvenHandler(object sender, DownloadBeginEventArgs e);
        public event DownloadBeginEvenHandler DownloadBegin;

        public delegate void DownloadChangedEventHandler(object sender, DownloadProgressEventArgs e);
        public event DownloadChangedEventHandler DownloadProgressChanged;

        /// <summary>
        /// Downloads the entire course contents to the given path
        /// </summary>
        /// <param name="course">The course to download</param>
        /// <param name="path">The path to download the course to</param>
        public void DownloadCourse(Course course, string path)
        {
            Directory.CreateDirectory(Path.Combine(path, course.Title));
            foreach (var module in course.Modules)
            {
                Directory.CreateDirectory(Path.Combine(path, course.Title, module.Title));
                foreach (var clip in module.Clips)
                {
                    if (DownloadBegin != null)
                    {
                        DownloadBegin(this, new DownloadBeginEventArgs() { clip = clip, module = module });
                    }
                    DownloadClip(clip, Path.Combine(path, course.Title, module.Title, clip.Title + ".mp4"));
                }
            }
        }
        /// <summary>
        /// Downloads the course contents into the specified path
        /// </summary>
        /// <param name="clip">The course object</param>
        /// <param name="Path">The path to store the videos</param>
        private void DownloadClip(Clip clip,string Path)
        {
            WebClient client = new WebClient();
            client.DownloadProgressChanged += Client_DownloadProgressChanged;
            client.DownloadFileAsync(new Uri(clip.Url), Path);
            while (client.IsBusy)
            {

            }
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (DownloadProgressChanged!=null)
            {
                DownloadProgressChanged(this, new DownloadProgressEventArgs() { Percentage = (int)((double)e.BytesReceived / e.TotalBytesToReceive * 100) });
            }
        }

        //Eventargs
        public class DownloadProgressEventArgs : EventArgs
        {
            public int Percentage { get; set; }
        }

        public class DownloadBeginEventArgs : EventArgs
        {
            public Clip clip { get; set; }
            public Module module { get; set; }
        }
    }
}
