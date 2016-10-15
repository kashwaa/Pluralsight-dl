using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluralSight_dl.Core
{
    /// <summary>
    /// Gets the clips urls
    /// </summary>
    public class LinksExtractor
    {
        /// <summary>
        /// Get the Course data
        /// </summary>
        /// <param name="userName">The username</param>
        /// <param name="password">The password</param>
        /// <param name="courseName">
        /// The course name, should be extracted from the course url
        /// Example: https://app.pluralsight.com/library/courses/mastering-git/table-of-contents should be "mastering-git"
        /// </param>
        /// <returns>A Course object that contains all modules and clip urls</returns>
        public Course GetLinks(string userName, string password,string courseName)
        {
            //Send login request to get authentication cookies
            Request login = Login(userName, password);

            //Pass the cookie container to a request to get the course details
            Request courseRequest = new GetRequest(login.container);

            string result;
            try
            {
               result = courseRequest.SendRequest($"https://app.pluralsight.com/learner/content/courses/{courseName}");
            }
            catch (Exception)
            {

                throw new Exception("Course not found, check the name and try again");
            }
            
            dynamic courseData = Newtonsoft.Json.JsonConvert.DeserializeObject(result);

            Course c = new Course();
            c.Title = courseData.title;
            for (int mIndex = 0; mIndex < courseData.modules.Count; mIndex++)
            {
                dynamic module = courseData.modules[mIndex];
                Module m = new Module() { Title = mIndex + 1 + "-" + module.title };
                for (int cIndex = 0; cIndex < module.clips.Count; cIndex++)
                {
                    dynamic clip = module.clips[cIndex];
                    m.Clips.Add(new Clip() { Title = cIndex + 1 + "-" + clip.title.Value, Url = GetClipUrl(clip.playerUrl.Value, login) });
                }
                c.Modules.Add(m);

            }

            Request ExerciseFilesRequest = new GetRequest(login.container);
            string ExerciseFilesJson = ExerciseFilesRequest.SendRequest($"https://app.pluralsight.com/learner/courses/{courseName}/exercise-files");
            dynamic ExcerciseFiles = Newtonsoft.Json.JsonConvert.DeserializeObject(ExerciseFilesJson);
            c.ExcerciseFiles = ExcerciseFiles.exerciseFilesUrl ?? null;
            return c;
        }

        /// <summary>
        /// An async wrapper around the GetLinks method
        /// </summary>
        /// <param name="userName">The username</param>
        /// <param name="password">The password</param>
        /// <param name="courseName">
        /// The course name, should be extracted from the course url
        /// Example: https://app.pluralsight.com/library/courses/mastering-git/table-of-contents should be "mastering-git"
        /// </param>
        /// <returns>A Course object that contains all modules and clip urls</returns>
        public async Task<Course> GetLinksAsync(string userName, string password, string courseName)
        {
            return await Task.Run(()=>{
                return GetLinks(userName,password,courseName);
            });
        }

        /// <summary>
        /// Extracts the video url from the player link
        /// </summary>
        /// <param name="playerUrl">The player link</param>
        /// <param name="login">The login request, used to extract authentication cookies</param>
        /// <returns>Video url</returns>
        private string GetClipUrl(string playerUrl,Request login)
        {
            Dictionary<string, string> queryString = new Dictionary<string, string>();
            string[] queryStringItems = playerUrl.Split('?').Last().Split('&');
            foreach (var item in queryStringItems)
            {
                queryString.Add(item.Split('=')[0], item.Split('=')[1]);
            }

            dynamic playerPostData = CreateClipRequestBody(queryString);

            Request request = new PostRequest(login.container);
            string result = request.SendRequest("https://app.pluralsight.com/video/clips/viewclip", Newtonsoft.Json.JsonConvert.SerializeObject(playerPostData), "application/json");
            dynamic dynamicResult = Newtonsoft.Json.JsonConvert.DeserializeObject(result);
            return dynamicResult.urls[0].url;
        }

        /// <summary>
        /// Created json object for the clip link request
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns>json object of the clip</returns>
        private dynamic CreateClipRequestBody(Dictionary<string, string> queryString)
        {
            dynamic playerPostData = new ExpandoObject();
            playerPostData.author = queryString["author"];
            playerPostData.clipIndex = Convert.ToInt32(queryString["clip"]);
            playerPostData.courseName = queryString["course"];
            playerPostData.moduleName = queryString["name"];
            playerPostData.includeCaptions = false;
            playerPostData.locale = "en";
            playerPostData.mediaType = "mp4";
            playerPostData.quality = "1024x768";
            return playerPostData;
        }

        /// <summary>
        /// A pluraldight login request
        /// </summary>
        /// <param name="userName">The username</param>
        /// <param name="Password">The password</param>
        /// <returns>Login response, the request will include the authentication cookies if login succeeds</returns>
        private Request Login(string userName, string Password)
        {
            Request LoginRequest = new PostRequest();
            string result=LoginRequest.SendRequest("https://app.pluralsight.com/id/", $"RedirectUrl=&Username={userName}&Password={Password}&ShowCaptcha=False&ReCaptchaSiteKey= 6LeVIgoTAAAAAIhx_TOwDWIXecbvzcWyjQDbXsaV", "application/x-www-form-urlencoded");
            if (result.Contains("Invalid user name or password"))
            {
                throw new Exception("access denied");
            }
            return LoginRequest;
        }
    }
}
