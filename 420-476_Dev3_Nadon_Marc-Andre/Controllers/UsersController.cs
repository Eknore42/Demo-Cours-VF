using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _420_476_Dev3_Nadon_Marc_Andre.Models;
using BCrypt.Net;

namespace _420_476_Dev3_Nadon_Marc_Andre.Controllers
{
    public class UsersController : Controller
    {
        private NorthwindEntities db = new NorthwindEntities();

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Firstname,Lastname,Login,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                //db.createUser(user.Firstname, user.Lastname, user.Login, user.Password);
                //db.Users.Add(user);
                //db.SaveChanges();
                return RedirectToAction("Login");
            }

            return View(user);
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult logout()
        {
            Session["Name"] = null;
            return RedirectToAction("Login","Users");
        }

        [HttpPost]
        public ActionResult log(string Login, string Password)
        {
            using(NorthwindEntities context = new NorthwindEntities())
            {
                var res = context.compare(Login, Password).FirstOrDefault();
                if(res != null)
                {
                    if (res == 1)
                    {
                        var user = context.Users.Where(u => u.Login.Equals(Login)).FirstOrDefault();
                        Session["Name"] = user.Firstname + " " + user.Lastname;
                        Session["Error"] = null;
                        return RedirectToAction("Index", "Products");
                    }
                    else
                        Session["Error"] = "Mot de passe incorrect";
                }
                else
                    Session["Error"] = "Identifiant non existant";
            }
            return View("Login");
        }
    }
}
