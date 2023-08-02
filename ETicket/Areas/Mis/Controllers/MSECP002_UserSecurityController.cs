using Dapper;
using ETicket.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ETicket.Areas.Mis.Controllers
{
    /// <summary>
    /// MSECP002_UserSecurity 使用者程式權限設定
    /// </summary>
    public class MSECP002_UserSecurityController : BaseController
    {
        /// <summary>
        /// 資料列表
        /// </summary>
        /// <param name="page">目前頁數</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <param name="searchText">搜尋文字</param>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize()]
        public ActionResult Index(int page = 1, int pageSize = 10, string searchText = "")
        {
            //檢查瀏覽權限
            if (UserService.RoleNo != "Mis")
                return RedirectToAction("Login", "Movie");

            using (DapperRepository dp = new DapperRepository())
            {
                string str_query = @"
              SELECT  Shows.ShowDate, Shows.ShowTime, Shows.TicketPrice,
                      Movies.Title, Shows.HallNo, BookingRecord.SeatNo,
                      BookingRecord.BookingStatus, Users.UserName, BookingRecord.BookingId
                      FROM Users INNER JOIN
                      BookingRecord ON Users.UserNo = BookingRecord.UserNo
                      JOIN Movies
                      JOIN Shows
                      ON Movies.MovieNo = Shows.MovieNo ON BookingRecord.ShowNo = Shows.ShowNo
                      ORDER BY Shows.ShowDate DESC, Shows.ShowTime DESC, Shows.HallNo, BookingRecord.SeatNo;
                ";
                

                int startIndex = (page - 1) * pageSize;
                var model = dp.ReadAll<vmBookingRecord>(str_query).Skip(startIndex).Take(pageSize).ToList();
                SessionService.Page = page;
                SessionService.PageSize = pageSize;
                var model2 = dp.ReadAll<vmBookingRecord>(str_query).ToList();
                SessionService.TotalPage = (model2.Count) / pageSize + 1;
                return View(model);
            }
        }

        /// <summary>
        /// 明細
        /// </summary>
        /// <param name="id">記錄 ID</param>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize()]
        public ActionResult Detail(int id = 0)
        {
            using (z_repoUsers repos = new z_repoUsers())
            {
                var model = repos.repo.ReadSingle(m => m.Id == id);
                SessionService.TagNo1 = model.RoleNo;
                SessionService.TagNo2 = model.UserNo;
                repos.SetSubHeader(model.UserNo, "使用者");
                return RedirectToAction(ActionService.Index, "MSECP002_Security", new { area = ActionService.Area });
            }
        }

        /// <summary>
        /// 新增/修改
        /// </summary>
        /// <param name="id">記錄 ID</param>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize()]
        public ActionResult CreateEdit(int id = 0)
        {
            //檢查新增/修改權限
            if (!PrgService.IsProgramSecurity(enSecurtyMode.CreateEdit, id))
                return RedirectToAction(ActionService.Index, ActionService.Controller, new { area = ActionService.Area });
         
                using (DapperRepository dp = new DapperRepository())
                {
                    string str_query = @"
                      SELECT  Shows.ShowDate, Shows.ShowTime, Shows.TicketPrice,
                      Movies.Title, Shows.HallNo, BookingRecord.SeatNo,
                      BookingRecord.BookingStatus, Users.UserName, BookingRecord.BookingId
                      FROM Users INNER JOIN
                      BookingRecord ON Users.UserNo = BookingRecord.UserNo
                      LEFT OUTER JOIN Movies
                      LEFT OUTER JOIN Shows
                      ON Movies.MovieNo = Shows.MovieNo ON BookingRecord.ShowNo = Shows.ShowNo
                      WHERE BookingId = @BookingId
                      ORDER BY Shows.ShowDate DESC, Shows.ShowTime DESC, Shows.HallNo, BookingRecord.SeatNo;
                    ";
                DynamicParameters parm = new DynamicParameters();
                dp.ParametersClear();
                parm.Add("BookingId", id);
                var model = dp.ReadSingle<vmBookingRecord>(str_query, parm);


                return View(model);
                }
            
        }

        /// <summary>
        /// 新增/修改
        /// </summary>
        /// <param name="model">資料</param>
        /// <returns></returns>
        [HttpPost]
        [LoginAuthorize()]
        public ActionResult CreateEdit(vmBookingRecord model)
        {
            if (!ModelState.IsValid) return View(model);

            using (dbEntities db = new dbEntities())
            {
                var p = db.BookingRecord.Find(model.BookingId); // Find(id) 用法: id 參數指 primary key
                p.BookingStatus = model.BookingStatus;
                db.SaveChanges();


            }


            return RedirectToAction(ActionService.Index, ActionService.Controller, new { area = ActionService.Area });

        }

        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="id">記錄 ID</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Delete(int id = 0)
        {
            //檢查刪除權限
            if (!PrgService.IsProgramSecurity(enSecurtyMode.Delete))
                return RedirectToAction(ActionService.Index, ActionService.Controller, new { area = ActionService.Area });

            using (dbEntities db = new dbEntities())
            {
               

                    var model = db.BookingRecord.Where(m => m.BookingId == id).FirstOrDefault();
                    if (model != null)
                    {
                        db.BookingRecord.Remove(model);
                        db.SaveChanges();

                    }
                }
            return RedirectToAction("Index", "MSECP002_UserSecurity", new { area = "Mis" });
        }

        

        /// <summary>
        /// 選取
        /// </summary>
        /// <param name="id">記錄 ID</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Select(int id = 0)
        {
            PrgService.SelectedId = id;
            return RedirectToAction(ActionService.Index, ActionService.Controller, new { area = ActionService.Area, page = PrgService.PageNumber, searchText = PrgService.SearchText });
        }

        /// <summary>
        /// 查詢
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [LoginAuthorize()]
        public ActionResult Search()
        {
            object obj_text = Request.Form[ActionService.SearchText];
            string str_text = (obj_text == null) ? string.Empty : obj_text.ToString();
            return RedirectToAction(ActionService.Index, ActionService.Controller, new { area = ActionService.Area, searchText = str_text });
        }

        [HttpGet]
        [LoginAuthorize()]
        public PartialViewResult AddPrgList(string id)
        {
            using (z_repoSecuritys sec = new z_repoSecuritys())
            {
                string str_role_no = id.Split('_')[0];
                string str_user_no = id.Split('_')[1];
                var model = sec.GetDapperUserAddPrgList(str_role_no, str_user_no);
                SessionService.TagNo1 = str_role_no;
                SessionService.TagNo2 = str_user_no;
                return PartialView("AddPrgList", model);
            }
        }

        [HttpPost]
        [LoginAuthorize()]
        public ActionResult AddPrgList(FormCollection collection)
        {
            using (z_repoSecuritys sec = new z_repoSecuritys())
            {
                if (collection.AllKeys.Count() > 0)
                {
                    string str_role_no = SessionService.TagNo1;
                    string str_user_no = SessionService.TagNo2;
                    foreach (string key in collection.AllKeys)
                    {
                        if (key.EndsWith("_1"))
                        {
                            string str_prg_no = key.Substring(0, key.Length - 2);
                            sec.AddSecurityPrg(str_role_no, str_user_no, str_prg_no);
                        }
                    }
                }
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        [LoginAuthorize()]
        public PartialViewResult DelPrgList(string id)
        {
            using (z_repoSecuritys sec = new z_repoSecuritys())
            {
                string str_role_no = id.Split('_')[0];
                string str_user_no = id.Split('_')[1];
                var model = sec.GetDapperUserDelPrgList(str_role_no, str_user_no);
                ActionService.TargetNo = id;
                return PartialView("DelPrgList", model);
            }
        }

        [HttpPost]
        [LoginAuthorize()]
        public ActionResult DelPrgList(FormCollection collection)
        {
            using (z_repoSecuritys sec = new z_repoSecuritys())
            {
                if (collection.AllKeys.Count() > 0)
                {
                    string str_role_no = SessionService.TagNo1;
                    string str_user_no = SessionService.TagNo2;
                    foreach (string key in collection.AllKeys)
                    {
                        if (key.EndsWith("_1"))
                        {
                            string str_prg_no = key.Substring(0, key.Length - 2);
                            sec.DelSecurityPrg(str_role_no, str_user_no, str_prg_no);
                        }
                    }
                }
                return RedirectToAction("Index");
            }
        }
    }
}
