using CoMute.Web.Models;
using CoMute.Web.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CoMute.Web.Controllers.API
{
    public class UserController : ApiController
    {
        [Route("user/add")]
        public HttpResponseMessage Post(RegistrationRequest registrationRequest)
        {
            var context = new CoMuteDBEntities();
            
            //var transaction = context.Database.BeginTransaction();

            // registering a new user

            //checking if the user being entered already exists
            if (UserExist(registrationRequest.Name) == true)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "User already exists");
            }
            else
            {
                var user1 = new User()
                {
                    Name = registrationRequest.Name,
                    Surname = registrationRequest.Surname,
                    Phone = registrationRequest.Phone,
                    Email = registrationRequest.Email,
                    Password = registrationRequest.Password,

                };

                context.Users.Add(user1);
                context.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.Created, user1);
            }
          
        }

        public bool UserExist(string Username)
        {
            var context = new CoMuteDBEntities();
            try
            {
                var result = context.Users.FirstOrDefault(zz => zz.Name == Username);
                return true;

            }
            catch (Exception)
            {

                return false;
            }

        }

        // View your profile according to unique UserID 

        [Route("user/view/{User_ID}")]
        [HttpGet]
        public HttpResponseMessage GET (int User_ID)
        {
            var context = new CoMuteDBEntities();
            var user = context.Users.FirstOrDefault(zz => zz.User_ID == User_ID);
            return Request.CreateResponse(HttpStatusCode.OK, user);
        }

        // updating the user's profile
        [Route("user/update")]
        [HttpPost]
        public HttpResponseMessage UpdateProfile(User userinfo)
        {
            var context = new CoMuteDBEntities();
            var user = context.Users.FirstOrDefault(zz => zz.User_ID == userinfo.User_ID);
           
            user.Surname = userinfo.Surname;
            user.Email = userinfo.Email;
            user.Name = userinfo.Name;
            user.Phone = userinfo.Phone;

            context.Users.Attach(user);
            context.SaveChanges();// error occuring with the saving of the updated User info
            return Request.CreateResponse(HttpStatusCode.OK, user);
        }
    }
}
