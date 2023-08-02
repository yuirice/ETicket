using Dapper;
using ETicket.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace ETicket.Controllers
{
    /// <summary>
    /// TicketOrderController
    /// </summary>
    public class TicketOrderController : BaseController
    {
        [HttpGet]
        [LoginAuthorize()]
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Order(string MovieNo)
        {
            using (DapperRepository dp = new DapperRepository())
            {

                string str_query = @"
                SELECT Shows.ShowNo, Movies.Title, Movies.pic, Shows.ShowDate, Shows.ShowTime, Shows.HallNo
                FROM Shows 
                LEFT OUTER JOIN Movies 
                ON Shows.MovieNo = Movies.MovieNo
                WHERE (Shows.MovieNo = @MovieNo)
                ORDER BY Shows.ShowDate, Shows.ShowTime, Shows.HallNo
                ";
                DynamicParameters parm = new DynamicParameters();
                dp.ParametersClear();
                parm.Add("MovieNo", MovieNo);
                var model = dp.ReadAll<vmMovieTime>(str_query, parm);
                return View(model);

                //dp.ParametersClear();
                //var model = dp.ReadAll<vmMovieTime>();
                //parm.Add("MovieNo", MovieNo);
                //dp.Execute(str_query, parm);
                //dp.ParametersClear();
                //var model = dp.ReadAll<vmMovieTime>();
                //return View(model);
            }

        }

        public ActionResult TicketOrder(string ShowNo)
        {
            if (!UserService.IsLogin) return RedirectToAction("Login", "Movie");
            CartService.ShowNo = ShowNo;
            using (DapperRepository dp = new DapperRepository())
            {
                string str_query = @"
                 SELECT  BookingRecord.SeatNo, Shows.ShowDate, 
                 Shows.ShowTime, Shows.HallNo, Movies.Title
                 FROM Movies 
                 LEFT OUTER JOIN Shows 
                 ON Movies.MovieNo = Shows.MovieNo 
                 LEFT OUTER JOIN BookingRecord
                 ON Shows.ShowNo = BookingRecord.ShowNo
                 WHERE (Shows.ShowNo = @ShowNo) AND (BookingStatus = 'True');
                 ";
                DynamicParameters parm = new DynamicParameters();
                dp.ParametersClear();
                parm.Add("ShowNo", ShowNo);
                var model = dp.ReadAll<vmBookingRecord>(str_query, parm);
                return View(model);
            }

        }
      


        public ActionResult SaveSelectedSeats(string divIds)
        {
            using (DapperRepository dp = new DapperRepository())
            {

                if (UserService.IsLogin)
                {
                    CartService.SeatNo = divIds;
                    string[] div = divIds.Split(',');
                    Session["div"] = div;
                    string str_query = @"
                 SELECT  Shows.ShowDate, Shows.ShowTime, Movies.Title, Shows.HallNo
                 FROM Movies 
                 LEFT OUTER JOIN Shows 
                 ON Movies.MovieNo = Shows.MovieNo 
                 WHERE (Shows.ShowNo = @ShowNo)
                 ";
                    DynamicParameters parm = new DynamicParameters();
                    dp.ParametersClear();

                    parm.Add("ShowNo", CartService.ShowNo);
                    var model = dp.ReadAll<vmBookingRecord>(str_query, parm);

                    
                    return View(model);

                }
                else
                {
                    return RedirectToAction("Login", "Movie");
                }

            }
        }

        


        public ActionResult Confirm()
        {
            string[] Div = Session["div"] as string[];



            using (DapperRepository dp = new DapperRepository())
            {

                if (UserService.IsLogin)
                {

                    foreach (string s in Div)
                    {
                        dp.CommandType = CommandType.Text;
                        string str_query = @"
                           INSERT INTO BookingRecord (UserNo, ShowNo, SeatNo, BookingStatus)
                           VALUES (@UserNo, @ShowNo, @SeatNo, 'true')
                           ";
                        DynamicParameters parm = new DynamicParameters();
                        dp.ParametersClear();

                        parm.Add("UserNo", UserService.UserNo);
                        parm.Add("ShowNo", CartService.ShowNo);
                        parm.Add("SeatNo", s);
                        string str_conn = WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
                        using (var conn = new SqlConnection(str_conn))
                        {
                            conn.Execute(str_query, parm);
                        }
                    }

                    using (SendMailService sendEmail = new SendMailService())
                    {
                        sendEmail.UserOrder();
                    }

                    return RedirectToAction("index", "Movie", new { area = "" });
                }
                else
                {
                    return RedirectToAction("Login", "Movie");
                }

            }






        }
    }
}

