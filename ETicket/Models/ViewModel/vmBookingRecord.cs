using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


    public class vmBookingRecord
    {
    public int BookingId { get; set; }
    public string ShowNo { get; set; }
    [Display(Name = "場次日期")]
    public Nullable<System.DateTime> ShowDate { get; set; }
    [Display(Name = "場次時間")]
    public string ShowTime { get; set; }
    [Display(Name = "電影名稱")]
    public string Title { get; set; }
    [Display(Name = "票價")]
    public string TicketPrice { get; set; }
    [Display(Name = "影廳")]
    public string HallNo { get; set; }
    [Display(Name = "座位")]
    public string SeatNo { get; set; }
    [Display(Name = "購票者")]

    public string UserName { get; set; }
    [Display(Name = "購票狀態")]
    public bool BookingStatus { get; set; }
    }
