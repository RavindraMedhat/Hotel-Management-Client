using Hotel_Management_Client.Models;
using Hotel_Management_Client.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_Management_Client.Controllers
{
    public class UserRegistrationController : Controller
    {
        private string API_UserRegistration;
        private string API_Login;
        public UserRegistrationController()
        {
            API_UserRegistration = @"http://localhost:17312/api/userregistrations";
            API_Login = @"http://localhost:17312/api/users/login";


        }
        // GET: UserRegisrationController
        public ActionResult Index()
        {
            return View();
        }

        // GET: UserRegisrationController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserRegisrationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserRegisrationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserRegistrationCreateModelForCustomer collection)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var form = new MultipartFormDataContent())
                    {
                        // Add your model properties as content fields
                        form.Add(new StringContent(collection.First_Name), "First_Name");
                        form.Add(new StringContent(collection.Last_Name), "Last_Name");
                        form.Add(new StringContent(collection.Email), "Email");
                        form.Add(new StringContent(collection.Password), "Password");
                        form.Add(new StringContent(collection.ConatactNo), "ConatactNo");
                        form.Add(new StringContent(collection.DOB.ToString()), "DOB");
                        form.Add(new StringContent(collection.Gender), "Gender");
                        form.Add(new StringContent(collection.State), "State");
                        form.Add(new StringContent(collection.Active_Flag.ToString()), "Active_Flag");
                        form.Add(new StringContent(collection.Delete_Flag.ToString()), "Delete_Flag");
                        form.Add(new StringContent(collection.sortedfield.ToString()), "sortedfield");

                        // Add image files
                        if (collection.Profile_Image != null)
                        {


                            var streamContent = new StreamContent(collection.Profile_Image.OpenReadStream());
                            streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                            {
                                Name = "Profile_Image",
                                FileName = collection.Profile_Image.FileName
                            };
                            form.Add(streamContent, "Profile_Image", collection.Profile_Image.FileName);
                        }


                        // Send the request
                        using (var response = await httpClient.PostAsync(API_UserRegistration, form))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                // Request was successful, handle the response here
                                var responseBody = await response.Content.ReadAsStringAsync();
                                Console.WriteLine("Response: " + responseBody);
                            }
                            else
                            {
                                // Handle an error response
                                Console.WriteLine("Error: " + response.StatusCode);
                            }
                        }
                    }
                }

                return RedirectToAction("login", "userregistration");
            }
            catch
            {
                return View();
            }
        }


        // GET: UserRegisrationController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserRegisrationController/Edit/5
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

        // GET: UserRegisrationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserRegisrationController/Delete/5
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
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginRequest collection)
        {
            try
            {
                LoginResponse loginResponse;
                using (var httpClient = new HttpClient())
                {
                    var jsondata = JsonConvert.SerializeObject(collection);
                    var contentdata = new StringContent(jsondata, Encoding.UTF8, @"Application/json");
                    using (var response = await httpClient.PostAsync(API_Login, contentdata))
                    {
                        var apiresponse = await response.Content.ReadAsStringAsync();
                        loginResponse = JsonConvert.DeserializeObject<LoginResponse>(apiresponse);
                    }
                }
                if (loginResponse.Success)
                {
                    HttpContext.Session.SetString("Email", collection.EmailID);
                    HttpContext.Session.SetString("Role", loginResponse.Role);
                    HttpContext.Session.SetString("Redirect", loginResponse.Redirect);
                    HttpContext.Session.SetInt32("RedirctID", loginResponse.RedirctID);
                    HttpContext.Session.SetInt32("UID", loginResponse.UId);


                    return RedirectToAction("Index", "Hotel");
                }
                else
                {
                    return View();
                }


            }
            catch
            {
                return View();
            }
        }
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();

            HttpContext.Session.Remove("Email");
            HttpContext.Session.Remove("Role");
            HttpContext.Session.Remove("Redirect");
            HttpContext.Session.Remove("RedirctID");
            return RedirectToAction("login", "UserRegistration");
        }
    }
}

