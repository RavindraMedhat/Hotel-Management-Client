using Hotel_Management_Client.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Hotel_Management_Client.Controllers
{
    public class HotelBranchController : Controller
    {
        // GET: HotelBranch
        private string API_HOTEL_BRANCH;

        public HotelBranchController()
        {
            API_HOTEL_BRANCH = @"http://localhost:17312/api/HotelBranchTBs";
        }

        public async Task<ActionResult> Index(int id, string HotelName)
        {
            var Email = HttpContext.Session.GetString("Email");
            var Role = HttpContext.Session.GetString("Role");
            var Redirect = HttpContext.Session.GetString("Redirect");
            var RedirctID = HttpContext.Session.GetInt32("RedirctID");
            if (Email == null || Role == null || Redirect == null || RedirctID == null)
            {
                return RedirectToAction("login", "UserRegistration");

            }
            
            List<HotelBranchViewModelForIndex> HotelBranchs = new List<HotelBranchViewModelForIndex>();

            using (var httpClient = new HttpClient())
            {
                using (var resonse = await httpClient.GetAsync(API_HOTEL_BRANCH + "/ByHotelId/" + id))
                {
                    var apiresponse = await resonse.Content.ReadAsStringAsync();
                    HotelBranchs = JsonConvert.DeserializeObject<List<HotelBranchViewModelForIndex>>(apiresponse);
                }
            }
            ViewBag.HotelName = HotelName;
            if (HotelName == null)
            {
                ViewBag.HotelName = HotelBranchs[0].Hotel_Name;
            }
            ViewBag.Hotel_ID = id;

            return View(HotelBranchs);
        }

        // GET: HotelBranch/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HotelBranch/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HotelBranch/Create
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

        // GET: HotelBranch/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HotelBranch/Edit/5
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

        // GET: HotelBranch/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HotelBranch/Delete/5
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
