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
    
    public partial class Modules
    {
        public int Id { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsWorkflow { get; set; }
        public string RoleNo { get; set; }
        public string SortNo { get; set; }
        public string ModuleNo { get; set; }
        public string ModuleName { get; set; }
        public string IconName { get; set; }
        public string Remark { get; set; }
    }
}