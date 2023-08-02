using Dapper;
using DocumentFormat.OpenXml.Bibliography;
using ETicket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Security;


    public class z_repoVmShows :BaseClass
    {
    #region 建構子及 CRUD
    /// <summary>
    /// Repository 變數
    /// </summary>
    public IEFGenericRepository<vmShows> repo;
    /// <summary>
    /// 建構子
    /// </summary>
    public z_repoVmShows()
    {
        repo = new EFGenericRepository<vmShows>(new dbEntities());
    }
    #endregion
}
