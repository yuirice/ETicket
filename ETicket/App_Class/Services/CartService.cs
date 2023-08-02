using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using ETicket.Models;

public static class CartService
{
    #region 公開屬性
    /// <summary>
    /// 訂單編號
    /// </summary>
    //public static string UserNo { get { return SessionService.GetValue("UserNo", ""); } set { SessionService.SetValue("UserNo", value); } }

    public static string ShowNo { get { return SessionService.GetValue("ShowNo", ""); } set { SessionService.SetValue("ShowNo", value); } }

    /// <summary>
    /// 購物批號 LotNo
    /// </summary>
    public static string LotNo
    {
        get { return GetLotNo(); }
        set { HttpContext.Current.Session["CartLotNo"] = value; }
    }

    public static string SeatNo { get { return SessionService.GetValue("SeatNo", ""); } set { SessionService.SetValue("SeatNo", value); } }
    public static string MovieTitle { get { return SessionService.GetValue("MovieTitle", ""); } set { SessionService.SetValue("MovieTitle", value); } }
    public static string ShowTime { get { return SessionService.GetValue("ShowTime", ""); } set { SessionService.SetValue("ShowTime", value); } }
    public static string ShowDate { get { return SessionService.GetValue("ShowDate", ""); } set { SessionService.SetValue("ShowDate", value); } }



    
    /// <summary>
    /// 購物批號建立時間
    /// </summary>
    //public static DateTime LotCreateTime
    //{
    //    get { return GetLotCreateTime(); }
    //    set { HttpContext.Current.Session["CartCreateTime"] = value; }
    //}
    /// <summary>
    /// 購物車筆數
    /// </summary>
    //public static int Counts { get { return GetCartCount(); } }

    /// <summary>
    /// 購物車合計
    /// </summary>
    //public static int Totals { get { return GetCartTotals(); } }
    #endregion
    #region 公用函數
    /// <summary>
    /// 更新購物批號
    /// </summary>
    /// <returns></returns>
    public static string NewLotNo()
    {
        string str_lot_no = "";
        if (!SessionService.IsLogined)
            str_lot_no = Guid.NewGuid().ToString().Substring(0, 15).ToUpper();
        LotNo = str_lot_no;
       
        return str_lot_no;
    }
    #endregion
    #region 公用事件
    /// <summary>
    /// 登入時將現有遊客的購物車加入客戶的購物車
    /// </summary>
    //public static void LoginCart()
    //{
    //    if (!string.IsNullOrEmpty(LotNo))
    //    {
    //        int int_qty = 0;
    //        using (z_repoCarts carts = new z_repoCarts())
    //        {
    //            var datas = carts.repo.ReadAll(m => m.LotNo == LotNo);
    //            if (datas != null)
    //            {
    //                foreach (var item in datas)
    //                {
    //                    int_qty++;
    //                    AddCart(item.ShowNo, item.SeatNo);
    //                    carts.repo.Delete(item);
    //                }
    //                carts.repo.SaveChanges();
    //            }
    //        }
    //        NewLotNo();
    //    }
    //}
    /// <summary>
    /// 加入購物車
    /// </summary>
    /// <param name="productNo">商品編號</param>
    //public static void AddCart(string productNo)
    //{
    //    AddCart(productNo, "", 1);
    //}

    /// <summary>
    /// 加入購物車
    /// </summary>
    /// <param name="productNo">商品編號</param>
    /// <param name="buyQty">數量</param>
    public static void AddCart(Carts cart)
    {

        using (dbEntities db = new dbEntities())
        {
            var datas = db.Carts.Where(m => m.LotNo == cart.LotNo).ToList();
            if (datas == null)
            {
                Carts model = new Carts();
                model.LotNo = cart.LotNo;
                model.UserNo = cart.UserNo;
                model.ShowNo = cart.ShowNo;
                model.SeatNo = cart.SeatNo;
            }
            else
            {
                datas.Add(cart);
            }

        }
        //&&   m.product_spec == prod_Spec);


    }

    private static string GetLotNo()
    {
        return (HttpContext.Current.Session["CartLotNo"] == null) ? NewLotNo() : HttpContext.Current.Session["CartLotNo"].ToString();
    }
    #endregion

    /// <summary>
    /// 取得目前購物車筆數
    /// </summary>
    /// <returns></returns>
    //    private static int GetCartCount()
    //    {
    //        int int_count = 0;
    //        using (z_repoCarts carts = new z_repoCarts())
    //        {
    //            if (SessionService.IsLogined)
    //            {
    //                var data1 = carts.repo.ReadAll(m => m.user_no == SessionService.AccountNo);
    //                if (data1 != null) int_count = data1.Count();
    //            }
    //            else
    //            {
    //                var data2 = carts.repo.ReadAll(m => m.lot_no == LotNo);
    //                if (data2 != null) int_count = data2.Count();
    //            }
    //        }
    //        return int_count;
    //    }

    //    private static int GetCartTotals()
    //    {
    //        int? int_totals = 0;
    //        using (z_repoCarts carts = new z_repoCarts())
    //        {
    //            if (SessionService.IsLogined)
    //            {
    //                var data1 = carts.repo.ReadAll(m => m.user_no == SessionService.AccountNo);
    //                if (data1 != null) int_totals = data1.Sum(m => m.amount);
    //            }
    //            else
    //            {
    //                var data2 = carts.repo.ReadAll(m => m.lot_no == LotNo);
    //                if (data2 != null) int_totals = data2.Sum(m => m.amount);
    //            }
    //        }
    //        if (int_totals == null) int_totals = 0;
    //        return int_totals.GetValueOrDefault();
    //    }

    //    private static string CreateNewOrderNo(vmOrders model)
    //    {
    //        ShopService.OrderID = 0;
    //        ShopService.OrderNo = "0";
    //        string str_order_no = "";
    //        string str_guid = Guid.NewGuid().ToString().Substring(0, 25).ToUpper();
    //        using (tblOrders orders = new tblOrders())
    //        {
    //            Orders newOrders = new Orders();
    //            newOrders.order_closed = 0;
    //            newOrders.order_validate = 0;
    //            newOrders.order_no = "";
    //            newOrders.order_date = DateTime.Now;
    //            newOrders.user_no = SessionService.AccountNo;
    //            newOrders.order_status = "ON";
    //            newOrders.order_guid = str_guid;
    //            newOrders.payment_no = model.payment_no;
    //            newOrders.shipping_no = model.shipping_no;
    //            newOrders.receive_name = model.receive_name;
    //            newOrders.receive_email = model.receive_email;
    //            newOrders.receive_address = model.receive_address;
    //            newOrders.remark = "";

    //            orders.repo.Create(newOrders);
    //            orders.repo.SaveChanges();

    //            var neword = orders.repo.ReadSingle(m => m.order_guid == str_guid);
    //            if (neword != null)
    //            {
    //                str_order_no = neword.rowid.ToString().PadLeft(8, '0');
    //                neword.order_no = str_order_no;
    //                orders.repo.Update(neword);
    //                orders.repo.SaveChanges();

    //                ShopService.OrderID = neword.rowid;
    //                ShopService.OrderNo = str_order_no;
    //            }
    //        }
    //        return str_order_no;
    //    }

}