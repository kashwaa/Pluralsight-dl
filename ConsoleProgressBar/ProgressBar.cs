using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProgressBar
{
    /// <summary>
    /// A simple progress bar for console apps
    /// </summary>
    public class ProgressBar
    {
        static readonly object _lock = new object();
        private int _currentProgress;
        private int _length;
        private int _location;
        private string _title;
        public ProgressBar(int length, string title)
        {
            lock (_lock)
            {
                Console.WriteLine();
                this._length = length;
                this._location = Console.CursorTop;
                this._title = title;
            }
        }

        /// <summary>
        /// Sets the current progress of the bar
        /// </summary>
        /// <param name="value">The value of progress, assumes value is >= 0</param>
        public void SetProgress(int value)
        {
            if (value < 0) value = 0;
            lock (_lock)
            {
                this._currentProgress = value;
                Console.SetCursorPosition(0, _location);
                string bar = "|" + (value == 0 ? "" : new string('=', value)) + (_length == _currentProgress ? "" : new string(' ', _length - _currentProgress)) + "|";
                Console.WriteLine(bar);
                Console.WriteLine(_title + " " + (int)((double)_currentProgress / _length * 100) + "%");
            }
        }
    }
}
