using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ETicket.Controllers
{
    /// <summary>
    /// UserController
    /// </summary>
    public class UserController : BaseController
    {
        [HttpGet]
        [LoginAuthorize()]
        public ActionResult Index()
        {
            return View();
        }

       
    }
}