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
    
    public partial class TodoLists
    {
        public int Id { get; set; }
        public bool IsFinished { get; set; }
        public string UserNo { get; set; }
        public string CodeNo { get; set; }
        public string TitleName { get; set; }
        public System.DateTime DeadlineDate { get; set; }
        public string Remark { get; set; }
    }
}
