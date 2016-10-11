using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluralSight_dl.Core
{
    /// <summary>
    /// A utility class with helper methods for courses
    /// </summary>
    public static class CourseHelper
    {
        /// <summary>
        /// Removes unallowed chars from a file name
        /// </summary>
        /// <param name="input">The unsanitized file name</param>
        /// <returns>Sanitized file name</returns>
        public static string SanitizeFileName(string input)
        {
            string[] unallowedChars = { "/", "\\", "<", ">", "\"", "|", "*", "?" ,":"};
            foreach (var unallowedChar in unallowedChars)
            {
                if (input.Contains(unallowedChar))
                {
                    input = input.Replace(unallowedChar, "");
                }
            }
            return input;
        }
    }
}
