using Dapper;
using ETicket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class z_repoShows : BaseClass
{
    #region 建構子及 CRUD
    /// <summary>
    /// Repository 變數
    /// <summary>
    public IEFGenericRepository<Shows> repo;
    /// <summary>
    /// 建構子
    /// <summary>
    public z_repoShows()
    {
        repo = new EFGenericRepository<Shows>(new dbEntities());
    }
    #endregion
}