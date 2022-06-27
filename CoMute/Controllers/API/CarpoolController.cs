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
    public class CarpoolController : ApiController
    {
        // GET: Carpool
   

        //create carpool (without condition on overlapping timeframes
        [Route("carpool/add")]
        [HttpPost]
        public HttpResponseMessage Post(CarpoolModel carpoolModel)
        {
            var context = new CoMuteDBEntities();
            var user = context.Users.FirstOrDefault(zz=> zz.User_ID == carpoolModel.User_ID);

                var carpool = new Carpool()
                {
                    Origin = carpoolModel.Origin,
                    DepartureTime = carpoolModel.DepartureTime,
                    ArrivalTime = carpoolModel.ArrivalTime,
                    DaysAvailable = carpoolModel.DaysAvailable,
                    Destination = carpoolModel.Destination,
                    AvailbaleSeats = carpoolModel.AvailbaleSeats,
                    Leader = user.Name + " " + user.Surname,
                    Notes = carpoolModel.Notes
                };

                context.Carpools.Add(carpool);
                context.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.Created, carpool); 
        }

        //join carpool
        [Route("carpool/join")]
        [HttpPost]
        public HttpResponseMessage JoinCarpool(CarpoolModel carpoolModel)
        {
            var context = new CoMuteDBEntities();
            var car = context.Carpools.FirstOrDefault(zz => zz.Carpool_ID == carpoolModel.Carpool_ID);


            car.NumberOfUsers = +1;

            var opp = new User_Carpool()
            {
                Carpool_ID = car.Carpool_ID,
                User_ID = carpoolModel.User_ID
            };
       
            context.Carpools.Add(car);
            context.User_Carpool.Add(opp);
            context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, car);
        }

        [Route("carpool/leave")]
        [HttpPost]
        public HttpResponseMessage LeaveCarpool(CarpoolModel carpoolModel)
        {
            var context = new CoMuteDBEntities();
            var car = context.Carpools.FirstOrDefault(zz => zz.Carpool_ID == carpoolModel.Carpool_ID);
            var carpools = context.User_Carpool.FirstOrDefault(zz=> zz.Carpool_ID == carpoolModel.Carpool_ID && zz.User_ID == carpoolModel.User_ID);

            car.NumberOfUsers = -1;

            context.Carpools.Attach(car);
            context.User_Carpool.Remove(carpools);
            context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, car);
        }

        //viewing all created carpools with a depature time after the current time
        [Route("carpool/viewcreated/")]
        [HttpGet]
        public HttpResponseMessage viewallcurrent()
        {
            var context = new CoMuteDBEntities();
            var carpool = context.Carpools.Where(zz => zz.DepartureTime >= DateTime.Now.TimeOfDay);
            return Request.CreateResponse(HttpStatusCode.OK, carpool);
        }

        //viewing all created carpools
        [Route("carpool/viewcreated/")]
        [HttpGet]
        public HttpResponseMessage viewall()
        {
            var context = new CoMuteDBEntities();
            var carpool = context.Carpools;
            return Request.CreateResponse(HttpStatusCode.OK, carpool);
        }


        //viewing the carpools the User has created
        [Route("carpool/viewcreated/{User_ID}")]
        [HttpGet]
        public HttpResponseMessage viewcreated(int User_ID)
        {
            var context = new CoMuteDBEntities();
            var user = context.Users.FirstOrDefault(zz => zz.User_ID == User_ID);
            var leader = user.Name + " " + user.Surname;
            var carpool = context.Carpools.Where(zz => zz.Leader == leader);
            return Request.CreateResponse(HttpStatusCode.OK, carpool);
        }

        //viewing the carpools the User has joined
        [Route("carpool/viewjoined/{User_ID}")]
        [HttpGet]
        public HttpResponseMessage viewjoined(int User_ID)
        {
            var context = new CoMuteDBEntities();
            var carpool = context.User_Carpool.Join(context.Carpools,
                u => u.Carpool_ID,
                c => c.Carpool_ID,
                (u , c) => new
                {
                    Carpool_ID = u.Carpool_ID,
                    User_ID = u.User_ID,
                    Origin = c.Origin,
                    DepartureTime = c.DepartureTime,
                    ArrivalTime = c.ArrivalTime,
                    DaysAvailable = c.DaysAvailable,
                    Destination = c.Destination,
                    AvailbaleSeats = c.AvailbaleSeats,
                    Leader = c.Leader,
                    Notes = c.Notes
                }).
                Where(zz => zz.User_ID == User_ID);

         
    
            return Request.CreateResponse(HttpStatusCode.OK, carpool);
        }
    }
}