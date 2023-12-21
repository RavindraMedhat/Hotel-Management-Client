using Hotel_Management_Client.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_Management_Client.Controllers
{
    public class RoomController : Controller
    {
        // GET: RoomController
        private string API_ROOM;
        private string API_URL_ROOMCAT;
        private string API_HOTEL_BRANCH;
        private string API_Details_ROOM_ID;

        public RoomController()
        {
            API_ROOM = @"http://localhost:17312/api/roomtbs";
            API_URL_ROOMCAT = @"http://localhost:17312/api/RoomCategoryTBs";
            API_HOTEL_BRANCH = @"http://localhost:17312/api/HotelBranchTBs";
            API_Details_ROOM_ID = @"http://localhost:17312/api/bookings/ByRoomIDAndDate";
        }
        public async Task<ActionResult> Index(int id)
        {
            var Email = HttpContext.Session.GetString("Email");
            var Role = HttpContext.Session.GetString("Role");
            var Redirect = HttpContext.Session.GetString("Redirect");
            var RedirctID = HttpContext.Session.GetInt32("RedirctID");
            if (Email == null || Role == null || Redirect == null || RedirctID == null)
            {
                return RedirectToAction("login", "UserRegistration");
            }

            

            List<RoomCategoryTBforcheckbox> roomCategoryTBforcheckbox = new List<RoomCategoryTBforcheckbox>();

            using (var httpclient = new HttpClient())
            {
                using (var response = await httpclient.GetAsync(API_URL_ROOMCAT + "/ByBranchID/" + id))
                {
                    var apiresponse = await response.Content.ReadAsStringAsync();
                    roomCategoryTBforcheckbox = JsonConvert.DeserializeObject<List<RoomCategoryTBforcheckbox>>(apiresponse);
                }
            }
            ViewBag.roomcategory = roomCategoryTBforcheckbox;

            List<RoomTBForIndex> room = new List<RoomTBForIndex>();

            using (var httpclient = new HttpClient())
            {
                using (var response = await httpclient.GetAsync(API_ROOM + "/ByBranchID/" + id))
                {
                    var apiresponse = await response.Content.ReadAsStringAsync();
                    room = JsonConvert.DeserializeObject<List<RoomTBForIndex>>(apiresponse);
                }
            }
            ViewBag.Branch_ID = id;
            HotelBranchViewModelForDetails HotelBranchsdetail;

            using (var httpClient = new HttpClient())
            {
                using (var resonse = await httpClient.GetAsync(API_HOTEL_BRANCH + "/" + id))
                {
                    var apiresponse = await resonse.Content.ReadAsStringAsync();
                    HotelBranchsdetail = JsonConvert.DeserializeObject<HotelBranchViewModelForDetails>(apiresponse);
                }
            }
            ViewBag.Branch_Name = HotelBranchsdetail.Branch_Name;

            return View(room);
        }

        // GET: RoomController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var Email = HttpContext.Session.GetString("Email");
            var Role = HttpContext.Session.GetString("Role");
            var Redirect = HttpContext.Session.GetString("Redirect");
            var RedirctID = HttpContext.Session.GetInt32("RedirctID");
            
            if (Email == null || Role == null || Redirect == null || RedirctID == null)
            {
                return RedirectToAction("login", "UserRegistration");
            }


            RoomViewModelForDetails roomViewModelForDetails;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(API_ROOM + "/" + id))
                {
                    var apiresponse = await response.Content.ReadAsStringAsync();
                    roomViewModelForDetails = JsonConvert.DeserializeObject<RoomViewModelForDetails>(apiresponse);
                }
            }
            //ViewBag.Room_ID = id;
            List<ViewModelForAvailability> ViewModelForAvailability;
            using (var httpClient = new HttpClient())
            {
                reqDataForAvailability reqData = new reqDataForAvailability() { date = DateTime.Now, Room_id = id };

                var jsondata = JsonConvert.SerializeObject(reqData);
                var contentdata = new StringContent(jsondata, Encoding.UTF8, @"Application/json");
                using (var response = await httpClient.PostAsync(API_Details_ROOM_ID, contentdata))
                {
                    var apiresponse = await response.Content.ReadAsStringAsync();
                    ViewModelForAvailability = JsonConvert.DeserializeObject<List<ViewModelForAvailability>>(apiresponse);
                }
            }
            ViewBag.Availability = ViewModelForAvailability;

            ViewBag.UID = (int)(HttpContext.Session.GetInt32("UID"));
            //roomViewModelForDetails.Room_ID = id;
            return View(roomViewModelForDetails);
        }

        // GET: RoomController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RoomController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RoomController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RoomController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RoomController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RoomController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
