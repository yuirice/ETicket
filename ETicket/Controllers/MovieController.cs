using Dapper;
using ETicket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ETicket.Controllers
{
    /// <summary>
    /// MovieController
    /// </summary>
    public class MovieController : BaseController
    {
        [HttpGet]
        [LoginAuthorize()]
        public ActionResult Index()
        {
            using (dbEntities db = new dbEntities())
            {
                var model = db.Movies.OrderBy(m => m.MovieNo).ToList();
                return View(model);
            }
           
        }

        public ActionResult Login()
        {
            UserService.Logout();
            vmLogin model = new vmLogin();
            return View(model);
        }

        [HttpPost]
        public ActionResult Login(vmLogin model)
        {
            //1. 沒有通過驗證，返回登入頁繼續輸入
            if (!ModelState.IsValid) return View(model);
            //2. 判斷登入資訊是否正確，不正確時手動引發一個錯誤
            if (!UserService.Login(model))
            {
                ViewBag.ErrorMessage = "帳號或密碼不正確!!";
                return View(model);
            }
            //if(UserService.IsValid==false) return View(model);
            
           return RedirectToAction("Index", "Movie", new { area = "" });
            
        }
        public ActionResult Register()
        {
            vmRegister model = new vmRegister();
            model.GenderCode = "M";
            return View(model);
        }

        /// <summary>
        /// 使用者註冊確認
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Register(vmRegister model)
        {
            //1. 沒有通過驗證，返回登入頁繼續輸入
            if (!ModelState.IsValid) return View(model);
            //2. 判斷登入資訊是否正確，不正確時手動引發一個錯誤
            string str_message = UserService.Register(model);
            if (!string.IsNullOrEmpty(str_message))
            {
                if (str_message == "登入信箱重複註冊!!")
                {
                    ModelState.AddModelError("UserEmail", str_message);
                    return View(model);
                }

                ModelState.AddModelError("UserNo", str_message);
                return View(model);
            }
            //3.新增一筆未審核會員資訊
            string str_code = UserService.RegisterCreate(model);
            //4.寄出一封註冊驗證信件
            using (SendMailService sendEmail = new SendMailService())
            {
                sendEmail.UserRegister(str_code);
            }
            //5.顯示註冊訊息
            TempData["MessageText"] = $"您的註冊資訊已提交，請至您的電子信箱{model.UserEmail}中驗證電子信箱功能,謝謝!!";
            return RedirectToAction("Message", "Movie", new { area = "" });
        }

        /// <summary>
        /// 顯示訊息用
        /// </summary>
        /// <returns></returns>
        public ActionResult Message() { return View(); }

        public ActionResult RegisterValidate( string id) 
        {
            //1.顯示註冊訊息
            TempData["MessageText"] = UserService.RegisterValidateCode(id);
            return RedirectToAction("Message", "Movie", new { area = "" });
        }

        

        public ActionResult Logout()
        {
            UserService.Logout();
            return RedirectToAction("Index", "Movie", new { area = "" });
        }

        public ActionResult Movie() 
        {
            using(dbEntities db = new dbEntities())
            {
                var model = db.Movies.OrderBy(m => m.MovieNo).ToList();
                return View(model);
            }
           
        }

        public ActionResult BookingRecord()
        {
            using (DapperRepository dp = new DapperRepository())
            {
                if (!UserService.IsLogin)
                {
                    return RedirectToAction("Login", "Movie");
                }
                else
                {
                    string str_query = @"
                SELECT DISTINCT Movies.Title, Shows.ShowDate,
                Shows.ShowTime, Shows.HallNo, Shows.ShowNo
                FROM BookingRecord 
                LEFT OUTER JOIN Movies
                RIGHT OUTER JOIN Shows 
                ON Movies.MovieNo = Shows.MovieNo
                ON BookingRecord.ShowNo = Shows.ShowNo
                WHERE BookingRecord.UserNo = @UserNo AND BookingStatus = 'True';
                ";
                    DynamicParameters parm = new DynamicParameters();
                    dp.ParametersClear();
                    parm.Add("UserNo", UserService.UserNo);
                    var model = dp.ReadAll<vmBookingRecord>(str_query, parm);
                    return View(model);
                }
               

                //dp.ParametersClear();
                //var model = dp.ReadAll<vmMovieTime>();
                //parm.Add("MovieNo", MovieNo);
                //dp.Execute(str_query, parm);
                //dp.ParametersClear();
                //var model = dp.ReadAll<vmMovieTime>();
                //return View(model);
            }

        }

        public ActionResult SeatRecord(string ShowNo)
        {
            using (DapperRepository dp = new DapperRepository())
            {
                if (!UserService.IsLogin)
                {
                    return RedirectToAction("Login", "Movie");
                }
                else
                {
                    string str_query = @"
                SELECT SeatNo FROM BookingRecord 
                WHERE BookingRecord.UserNo = @UserNo AND ShowNo = @ShowNo AND BookingStatus = 'True';
                ";
                    DynamicParameters parm = new DynamicParameters();
                    dp.ParametersClear();
                    parm.Add("UserNo", UserService.UserNo); 
                    parm.Add("ShowNo", ShowNo);
                    var model = dp.ReadAll<vmBookingRecord>(str_query, parm);
                    return View(model);
                }


                //dp.ParametersClear();
                //var model = dp.ReadAll<vmMovieTime>();
                //parm.Add("MovieNo", MovieNo);
                //dp.Execute(str_query, parm);
                //dp.ParametersClear();
                //var model = dp.ReadAll<vmMovieTime>();
                //return View(model);
            }

        }

        [HttpGet]
        public ActionResult Delete(int id)
        {

            using (dbEntities db = new dbEntities())
            {
                var model = db.TodoLists.Where(m => m.Id == id).FirstOrDefault();
                if (model != null)
                {
                    db.TodoLists.Remove(model);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Index", "web", new { area = "" });
        }
    }
}