using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluralSight_dl.Core
{
    public class Course
    {
        private string _title;
        public string Title { get { return _title; } set { _title = CourseHelper.SanitizeFileName(value); } }
        public List<Module> Modules { get; set; }
        public string ExcerciseFiles { get; set; }
        public Course()
        {
            this.Modules = new List<Module>();
        }
        public override string ToString()
        {
            return this.Title;
        }
    }
}
