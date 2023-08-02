using Dapper;
using DocumentFormat.OpenXml.EMMA;
using ETicket.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Security;

/// <summary>
/// 使用者相關服務
/// </summary>
public static class UserService
{
    public static string UserNo { get { return SessionService.GetValue("UserNo", ""); } set { SessionService.SetValue("UserNo", value);}} 
    public static string UserName { get { return SessionService.GetValue("UserName", ""); } set { SessionService.SetValue("UserName", value); } }
    public static string Email { get { return SessionService.GetValue("Email", ""); } set { SessionService.SetValue("Email", value); } }
    public static string RoleNo { get { return SessionService.GetValue("RoleNo", ""); } set { SessionService.SetValue("RoleNo", value); } }
    public static string RoleName { get { using (z_repoRoles roles = new z_repoRoles()) { return roles.GetDataName(RoleNo); } } }
    public static string DeptName { get { return SessionService.GetValue("DeptName", ""); } set { SessionService.SetValue("DeptName", value); } }
    public static string TitleName { get { return SessionService.GetValue("TitleName", ""); } set { SessionService.SetValue("TitleName", value); } }
    public static string UserImage { get { return GetUserImage(UserNo); } }
    public static bool IsLogin { get { return SessionService.GetBoolValue("IsLogin", false); } set { SessionService.SetValue("IsLogin", value); } }
    //public static bool IsValid { get { return SessionService.GetBoolValue("IsValid", false); } set { SessionService.SetValue("IsValid", value); } }
    public static Securitys UserSecurity { get; set; } = new Securitys
    {
        TargetNo = "",
        RoleNo = "",
        IsAdd = false,
        IsConfirm = false,
        IsDelete = false,
        IsDownload = false,
        IsEdit = false,
        IsPrint = false,
        IsUndo = false,
        IsUpload = false
    };
    public static string GetUserImage(string userNo)
    {
        string str_image = string.Format("~/Images/User/{0}.jpg", userNo);
        string str_stamp = DateTime.Now.ToString("yyyyMMddHHmmssff");
        if (!File.Exists(HttpContext.Current.Server.MapPath(str_image)))
            str_image = "~/Images/User/none.jpg";
        return string.Format("{0}?t={1}", str_image, str_stamp);
    }
    /// <summary>
    /// 登入
    /// </summary>
    /// <param name="userNo">使用者代號</param>
    /// <param name="userName">使用者姓名</param>
    /// <param name="roleNo">角色代號</param>

    public static bool Login(vmLogin model)
    {
        using (z_repoUsers users = new z_repoUsers())
        {
            using(z_repoDepartments dept = new z_repoDepartments())
            {
                using(z_repoTitles title = new z_repoTitles())
                {
                    var data = users.repo.ReadSingle(m =>
                    m.UserNo == model.UserNo &&
                    m.Password == model.Password &&
                    m.IsValid ==true);
                    if(data == null) return false;
                    
                    UserName = data.UserName;
                    RoleNo = data.RoleNo;
                    UserNo = data.UserNo;
                    Email = data.ContactEmail;

                    IsLogin = true;
                    return true;
                }
            }
            
           
        }
    }
    /// <summary>
    /// 使用者註冊
    /// </summary>
    /// <param name="model">註冊資訊</param>
    /// <returns></returns>
    public static string Register(vmRegister model)
    {
        using(z_repoUsers user = new z_repoUsers())
        {
            var data = user.repo.ReadSingle(m => m.UserNo == model.UserNo);
            if (data != null) return "登入帳號重複註冊!!";
             data = user.repo.ReadSingle(m => m.ContactEmail == model.UserEmail);
            if (data != null) return "登入信箱重複註冊!!";
            return "";
        }
    }
    public static void Login(string userNo, string userName, string roleNo)
    {
        UserNo = userNo;
        UserName = userName;
        RoleNo = roleNo;
        DeptName = "";
        TitleName = "";
        IsLogin = true;
        if (roleNo == "User")
        { using (z_repoUsers user = new z_repoUsers()) { user.SetUserInfo(); } }
        { using (z_repoCompanys user = new z_repoCompanys()) { user.SetDefaultCompany(); } }
    }

    public static string RegisterCreate(vmRegister model)
    {
        using(z_repoUsers user = new z_repoUsers())
        {
            using(CryptographyService cryp = new CryptographyService())
            {
                string str_code = Guid.NewGuid().ToString().ToUpper().Replace("-", "");
                string str_password = cryp.SHA256Encode(model.Password);
                Users newUser = new Users();
                newUser.IsValid = false;
                newUser.RoleNo = "Member";
                newUser.ValidateCode = str_code;
                newUser.UserNo = model.UserNo;
                newUser.UserName = model.UserName;
                //newUser.Password = str_password;
                newUser.Password = model.Password;
                newUser.ContactEmail = model.UserEmail;
                newUser.ContactTel = model.UserTel;
                newUser.ContactAddress = model.UserAddress;
                newUser.DeptNo = "";
                newUser.TitleNo = "";
                newUser.Remark = "";

                user.repo.Create(newUser);
                user.repo.SaveChanges();
                return str_code;
            }
        }
    }

    /// <summary>
    /// 致行註冊電子信箱驗證
    /// </summary>
    /// <param name="validateCode">驗證碼</param>
    /// <returns></returns>
    public static string RegisterValidateCode(string validateCode)
    {
        using(z_repoUsers user = new z_repoUsers())
        {
            var data = user.repo.ReadSingle(m =>m.ValidateCode == validateCode);
            if (data == null) return "驗證碼不存在!!";
            if (data.IsValid) return "此帳號電子信箱已驗證過，不可重複驗證!!";

            using (DapperRepository dp = new DapperRepository())
            {
                string str_query = "UPDATE Users SET IsValid = @IsValid WHERE ValidateCode = @ValidateCode";
                DynamicParameters parm = new DynamicParameters();
                parm.Add("IsValid", true);
                parm.Add("ValidateCode", validateCode);
                dp.Execute(str_query, parm);
            }
            return "您的電子信箱已通過驗證，請至系統登入頁進行登入，謝謝!!";
        }
    }
    /// <summary>
    /// 登出
    /// </summary>
    public static void Logout()
    {
        UserNo = "";
        UserName = "";
        RoleNo = "";
        DeptName = "";
        TitleName = "";
        IsLogin = false;
    }
    /// <summary>
    /// 除錯模式預設使用者
    /// </summary>
    public static void DemoUser()
    {
        UserNo = "demo";
        UserName = "測試帳號";
        DeptName = "資訊部";
        TitleName = "程式設計師";
        IsLogin = true;
        DemoSecurity();
    }
    /// <summary>
    /// 權限初始化
    /// </summary>
    public static void InitSecurity()
    {
        UserSecurity.Id = -1;
        UserSecurity.TargetNo = "";
        UserSecurity.RoleNo = "";
        UserSecurity.IsAdd = false;
        UserSecurity.IsConfirm = false;
        UserSecurity.IsDelete = false;
        UserSecurity.IsDownload = false;
        UserSecurity.IsEdit = false;
        UserSecurity.IsInvalid = false;
        UserSecurity.IsPrint = false;
        UserSecurity.IsUndo = false;
        UserSecurity.IsUpload = false;
    }
    /// <summary>
    /// 除錯模式預設使用者權限
    /// </summary>
    public static void DemoSecurity()
    {
        UserSecurity.Id = -1;
        UserSecurity.TargetNo = UserNo;
        UserSecurity.RoleNo = RoleNo;
        UserSecurity.IsAdd = true;
        UserSecurity.IsConfirm = true;
        UserSecurity.IsDelete = true;
        UserSecurity.IsDownload = true;
        UserSecurity.IsEdit = true;
        UserSecurity.IsInvalid = true;
        UserSecurity.IsPrint = true;
        UserSecurity.IsUndo = true;
        UserSecurity.IsUpload = true;
    }
}