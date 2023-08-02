using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

public class vmShows
{
    public int ShowId { get; set; }

    public string MovieNo { get; set; }

    [Display(Name = "場次日期")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
    public Nullable<System.DateTime> ShowDate { get; set; }
    [Display(Name = "場次時間")]
    public string ShowTime { get; set; }
    [Display(Name = "票價")]
    public int TicketPrice { get; set; }
    [Display(Name = "電影名稱")]
    public string Title { get; set; }
    [Display(Name = "電影類型")]
    public string Genre { get; set; }
    [Display(Name = "片長")]
    public string Duration { get; set; }
    [Display(Name = "影廳")]
    public string HallNo { get; set; }
}