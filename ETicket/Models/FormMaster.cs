//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ETicket.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class FormMaster
    {
        public int Id { get; set; }
        public string FormCode { get; set; }
        public string UserNo { get; set; }
        public string StatusCode { get; set; }
        public string FormNo { get; set; }
        public Nullable<System.DateTime> FormDate { get; set; }
        public Nullable<System.DateTime> FormTime { get; set; }
        public string TargetNo { get; set; }
        public string TargetName { get; set; }
        public string DeptNo { get; set; }
        public string DeptName { get; set; }
        public string TitleNo { get; set; }
        public string TitleName { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public string CodeNo { get; set; }
        public string CodeName { get; set; }
        public int Qty1 { get; set; }
        public int Qty2 { get; set; }
        public Nullable<System.DateTime> ApproveTime { get; set; }
        public Nullable<System.DateTime> RejectTime { get; set; }
        public string SourceNo { get; set; }
        public string ApproveNo { get; set; }
        public string RejectNo { get; set; }
        public string NextNo { get; set; }
        public string GuidNo { get; set; }
        public string NotifyKey { get; set; }
        public string FormDescribe { get; set; }
        public string Remark { get; set; }
    }
}
