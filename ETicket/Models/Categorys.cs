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
    
    public partial class Categorys
    {
        public int Id { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsCategory { get; set; }
        public string ParentNo { get; set; }
        public string CategoryNo { get; set; }
        public string SortNo { get; set; }
        public string CategoryName { get; set; }
        public string RouteName { get; set; }
        public string Remark { get; set; }
    }
}
