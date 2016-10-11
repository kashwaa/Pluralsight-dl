using System;
using System.Collections.Generic;

namespace PluralSight_dl.Core
{
    public class Module
    {
        private string _title;
        public string Title { get { return _title; } set { _title = CourseHelper.SanitizeFileName(value); } }
        public List<Clip> Clips { get; set; }
        public Module()
        {
            this.Clips = new List<Clip>();
        }
        public override string ToString()
        {
            return this.Title;
        }
    }
}