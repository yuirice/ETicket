﻿using ETicket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public partial class ListItemData : BaseClass
{
    /// <summary>
    /// 主版頁面列表
    /// </summary>
    /// <returns></returns>
    public List<SelectListItem> LayoutList()
    {
        using (CodeBase code = new CodeBase())
        {
            List<string> lists = code.GetLayoutList();
            var data = lists.Select(u => new SelectListItem
            {
                Text = u,
                Value = u
            }).ToList();
            return data;
        }
    }
}
