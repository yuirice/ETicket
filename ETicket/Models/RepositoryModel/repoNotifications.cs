using Dapper;
using ETicket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Notifications CRUD
/// </summary>
public class z_repoNotifications : BaseClass
{
    #region 建構子及 CRUD
    /// <summary>
    /// Repository 變數
    /// <summary>
    public IEFGenericRepository<Notifications> repo;
    /// <summary>
    /// 建構子
    /// <summary>
    public z_repoNotifications()
    {
        repo = new EFGenericRepository<Notifications>(new dbEntities());
    }
    /// <summary>
    /// 以 Dapper 來讀取資料集合
    /// <summary>
    /// <param name="searchText">查詢條件</param>
    /// <returns></returns>
    public List<Notifications> GetDapperDataList(string searchText)
    {
        using (DapperRepository dp = new DapperRepository())
        {
            string str_query = GetSQLSelect();
            str_query += GetSQLWhere(searchText);
            str_query += GetSQLOrderBy();
            var model = dp.ReadAll<Notifications>(str_query);
            return model;
        }
    }
    /// <summary>
    /// 取得 SQL 欄位及表格名稱
    /// <summary>
    /// <returns></returns>
    private string GetSQLSelect()
    {
        string str_query = @"
SELECT Notifications.Id, Notifications.IsRead, Notifications.CodeNo, vi_CodeNotification.CodeName,
Notifications.SourceNo, Notifications.SenderNo, Users.UserName AS SenderName,
Notifications.ReceiverNo, Users_1.UserName AS ReceiverName, Notifications.SendDate, 
Notifications.SendTime, Notifications.HeaderText, 
Notifications.MessageText, Notifications.Remark
FROM  Notifications 
LEFT OUTER JOIN Users ON Notifications.SenderNo = Users.UserNo 
LEFT OUTER JOIN Users AS Users_1 ON Notifications.ReceiverNo = Users_1.UserNo 
LEFT OUTER JOIN vi_CodeNotification ON Notifications.CodeNo = vi_CodeNotification.CodeNo 
";
        return str_query;
    }
    /// <summary>
    /// 取得 SQL 條件式
    /// <summary>
    /// <param name="searchText">查詢文字</param>
    /// <returns></returns>
    private string GetSQLWhere(string searchText)
    {
        string str_query = "";
        if (!string.IsNullOrEmpty(searchText))
        {
            str_query += " WHERE (";
            str_query += $"Notifications.SourceNo LIKE '%{searchText}%'  OR ";
            str_query += $"Notifications.SenderNo LIKE '%{searchText}%'  OR ";
            str_query += $"Notifications.SenderName LIKE '%{searchText}%'  OR ";
            str_query += $"Notifications.ReceiverNo LIKE '%{searchText}%'  OR ";
            str_query += $"Notifications.ReceiverName LIKE '%{searchText}%'  OR ";
            str_query += $"Notifications.HeaderText LIKE '%{searchText}%'  OR ";
            str_query += $"Notifications.Remark LIKE '%{searchText}%'  ";
            str_query += ") ";
        }
        return str_query;
    }
    /// <summary>
    /// 取得 SQL 排序
    /// <summary>
    /// <returns></returns>
    private string GetSQLOrderBy()
    {
        return " ORDER BY  Notifications.Messages,Notifications.SendDate,Notifications.SendTime";
    }
    /// <summary>
    /// 新增或修改
    /// <summary>
    /// <param name="model"></param>
    public void CreateEdit(Notifications model)
    {
        repo.CreateEdit(model, model.Id);
    }
    /// <summary>
    /// 刪除
    /// <summary>
    /// <param name="id">Id</param>
    public void Delete(int id)
    {
        var model = repo.ReadSingle(m => m.Id == id);
        if (model != null) repo.Delete(model, true);
    }
    /// <summary>
    /// 檢查 Id 是否存在
    /// <summary>
    /// <param name="id">主鍵值</param>
    /// <returns></returns>
    public bool IdExists(int id)
    {
        var model = repo.ReadSingle(m => m.Id == id);
        return (model != null);
    }
    #endregion
    #region 自定義事件及函數
    #endregion
}
