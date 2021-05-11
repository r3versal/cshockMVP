using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SYW.Web.Controllers
{
    using Business.Handlers;
    using SYW.API.Controllers;
    using Common.Models;
    using Newtonsoft.Json;
    using System.Net.Http;

    public class HomeController : Controller
    {

        //public IActionResult Index()
        //{
            //IEnumerable<User> users = null;

            //using (var client = new HttpClient())
            //{
            //    client.BaseAddress = new Uri("http://localhost:5000/v1/api/testGet");
            //    //HTTP GET
            //    var responseTask = client.GetAsync("user");
            //    responseTask.Wait();

            //    var result = responseTask.Result;
            //    if (result.IsSuccessStatusCode)
            //    {
            //        var readTask = result.Content.ReadAsAsync<IList<User>>();
            //        readTask.Wait();

            //        users = readTask.Result;
            //    }
            //    else //web api sent error response 
            //    {
            //        //log response status here..

            //        users = Enumerable.Empty<User>();

            //        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            //    }
            //}
          //  return View(users);
       // }
 
        //[HttpPost("schedule/edit")]
        //[Produces("application/json")]
        //public async Task<IActionResult> Submit(string jsonData)
        //{
        //    try
        //    {
        //        if (jsonData != null)
        //        {
        //            var userInfo = JsonConvert.DeserializeObject<SignupRequest>(jsonData);
                    //var user = await UserController.SignupUser(userInfo);
                    //if (user != null)
                    //{
                    //    return Ok("{}");
                    //}
                    //else
                    //{
                    //    return NotFound("User not saved.");
                    //}
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return NotFound("Error occured while saving schedule: " + ex.ToString());
        //    }
        //    return RedirectToAction("Index");
        //}
    }
}
