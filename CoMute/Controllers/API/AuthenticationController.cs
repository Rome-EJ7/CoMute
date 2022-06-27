using CoMute.Web.Models.Dto;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CoMute.Web.Controllers.API
{
    public class AuthenticationController : ApiController
    {
        /// <summary>
        /// Logs a user into the application.
        /// </summary>
        /// <param name="loginRequest">The user's login details</param>
        /// <returns></returns>
        public HttpResponseMessage Post(LoginRequest loginRequest)
        {
            var context = new CoMuteDBEntities();

            var user = context.Users.Where(zz => zz.Email == loginRequest.Email && zz.Password == loginRequest.Password).FirstOrDefault();
            if (user == null)// check to see if user exists
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            else
            {
                //returning the matching user info so the userID can be stored and used to make the other requests
                return Request.CreateResponse(HttpStatusCode.OK, user);
            }
          
        }
    }
}
