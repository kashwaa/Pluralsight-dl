using System.Collections.Generic;

namespace PluralSight_dl.Core
{
    public class Clip
    {
        private string _title;
        public string Title { get { return _title; } set { _title = CourseHelper.SanitizeFileName(value); } }
        public string Url { get; set; }
        public override string ToString()
        {
            return this.Title;
        }
    }
}