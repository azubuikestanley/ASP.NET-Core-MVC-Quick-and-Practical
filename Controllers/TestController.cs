using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EZCourse.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            var users = new List<User>
            {
                new User {Id = 1, Name = "Mr. Blue"},
                new User {Id = 2, Name = "Ms. Green"},
                new User {Id = 1, Name = "Ma. Blue"},
            };
            var states = new List<State>
            {
                new State {StateOrigin = "Delta"},
                new State {StateOrigin = "Lagos"},
                new State {StateOrigin = "Abuja"},
            };
            //Three option to pass this list to view, 1st - ViewBag, 2nd - ViewData and the third is View(users)
            //ViewBag is a dynamic object, while ViewData is a dictionary 
            //ViewBag.Users = users;
            ViewData["States"] = states;
            return View(users);
        }

        public class User
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
        public class State
        {
            public string StateOrigin { get; set; }
        }


        //different method
        /*public string StringOut(int id, Person person)
            {
               return $"StringOut Action {id} -- {person.FirstName} {person.LastName}";
            }
         */
        /*public IActionResult StringOut(int id, Person person)
        {
            return Content($"StringOut Action {id} -- {person.FirstName} {person.LastName}");
        }
        */

        public IActionResult JsonOut(int id, Person person)
        {
            var obj = new
            {
                Id = id,
                Person = person
            };
            return Json(obj);
        }
        public class Person
        {
            public string FirstName { get; set;  }
            public string LastName { get; set; }
        }
    }
}