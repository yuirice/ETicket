using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

/// <summary>
/// 註冊頁面用 Model
/// </summary>
public class vmRegister
{
    [Display(Name = "登入帳號")]
    [Required(ErrorMessage = "登入帳號不可空白!!")]
    public string UserNo { get; set; }
    [Display(Name = "登入名稱")]
    [Required(ErrorMessage = "登入名稱不可空白!!")]
    public string UserName { get; set; }
    [Display(Name = "登人密碼")]
    [Required(ErrorMessage = "登人密碼不可空白!!")]
    [MinLength(4, ErrorMessage ="登入密碼不可小於 4 個字元!!")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Display(Name = "確認密碼")]
    [Required(ErrorMessage = "確認密碼不可空白!!")]
    [DataType(DataType.Password)]
    [Compare("Password" ,ErrorMessage ="確認密碼與登入密碼不符!!")]
    public string ConfirmPassword { get; set; }
    [Display(Name ="電子信箱")]
    [Required(ErrorMessage = "電子信箱不可空白!!")]
    [EmailAddress(ErrorMessage = "電子信箱格式錯誤!!")]
    public string UserEmail { get; set; }
    [Display(Name = "性別")]
    public string GenderCode { get; set; }
    [Display(Name = "連絡電話")]
    public string UserTel { get; set; }
    [Display(Name = "聯絡地址")]
    public string UserAddress { get; set; }
}