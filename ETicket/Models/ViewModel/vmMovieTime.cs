using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;



    /// <summary>
    /// 電影時刻頁面
    /// </summary>
    public class vmMovieTime
    {
        public string ShowNo { get; set; }
        public string Title { get; set; }
        public string Pic { get; set; }
        public Nullable<System.DateTime> ShowDate { get; set; }
        public string ShowTime { get; set; }
        public string HallNo { get; set; }

       

    }
