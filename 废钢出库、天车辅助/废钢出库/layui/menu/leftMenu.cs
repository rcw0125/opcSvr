using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace layui.menu
{
    public static class leftMenu
    {
        public static List<TS_MENU> Load()
        {
            List<TS_MENU> listMenu = new List<TS_MENU>();
            TS_MENU menu = new TS_MENU();
            menu.C_ID = "1";
            menu.C_NAME = "设置";
            menu.C_PARENT_ID = "0";
            menu.C_URL = "test";
            menu.C_ICON = "layui-icon layui-icon-set";
            listMenu.Add(menu);
            TS_MENU menu2 = new TS_MENU();
            menu2.C_ID = "2";
            menu2.C_NAME = "在线";
            menu2.C_PARENT_ID = "0";
            menu2.C_URL = "test";
            listMenu.Add(menu2);

            TS_MENU menu3 = new TS_MENU();
            menu3.C_ID = "3";
            menu3.C_NAME = "个人设置";
            menu3.C_PARENT_ID = "1";
            menu3.C_URL = "me";
            listMenu.Add(menu3);

            TS_MENU menu4 = new TS_MENU();
            menu4.C_ID = "4";
            menu4.C_NAME = "个人资料";
            menu4.C_PARENT_ID = "3";
            menu4.C_URL = "me";
            listMenu.Add(menu4);

            TS_MENU menu5 = new TS_MENU();
            menu5.C_ID = "5";
            menu5.C_NAME = "修改密码";
            menu5.C_PARENT_ID = "3";
            menu5.C_URL = "me2";
            listMenu.Add(menu5);

            TS_MENU menu6 = new TS_MENU();
            menu6.C_ID = "6";
            menu6.C_NAME = "在线服务";
            menu6.C_PARENT_ID = "2";
            menu6.C_URL = "/home/layuitest";
            listMenu.Add(menu6);

            TS_MENU menu7 = new TS_MENU();
            menu7.C_ID = "7";
            menu7.C_NAME = "测试";
            menu7.C_PARENT_ID = "0";
            menu7.C_URL = "/home/about";
            listMenu.Add(menu7);

            TS_MENU menu8 = new TS_MENU();
            menu8.C_ID = "8";
            menu8.C_NAME = "table";
            menu8.C_PARENT_ID = "0";
            menu8.C_URL = "/home/layuitable";
            listMenu.Add(menu8);

            TS_MENU menu9 = new TS_MENU();
            menu9.C_ID = "9";
            menu9.C_NAME = "在线服务2";
            menu9.C_PARENT_ID = "2";
            menu9.C_URL = "/home/layuitest2";
            listMenu.Add(menu9);

            

            return listMenu;

        }

        public static string getMenu(List<TS_MENU> listmenu)
        {
            var topmenu = listmenu.Where(o=>o.C_PARENT_ID=="0");
            StringBuilder result = new StringBuilder();
            foreach (var item in topmenu)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" <li id=\""+item.C_ID+"\" class=\"layui-nav-item\">");

                var childlist = listmenu.Where(o => o.C_PARENT_ID == item.C_ID).ToList();
                if (childlist.Count > 0)
                {
                    sb.Append(" <a href=\"javascript:;\" > <span class=\"" + item.C_ICON + "\"></span>" + item.C_NAME + "</a>");
                    sb.Append(" <dl class=\"layui-nav-child\">");
                    sb.Append(getSecondMenu(childlist, listmenu,""));
                    sb.Append(" </dl>");
                }
                else
                {
                    sb.Append(" <a href=\"javascript:jump(\'" + item.C_URL + "\');\" > <span class=\"" + item.C_ICON + "\"></span>"+item.C_NAME+"</a>");
                }
                sb.Append(" </li>");
                result.Append(sb.ToString());
            }
            
            return result.ToString();
        }


        

        public static string getSecondMenu(List<TS_MENU> listmenu, List<TS_MENU> allmenu,string kongge)
        {
            kongge += "&nbsp;&nbsp;";
            StringBuilder result = new StringBuilder();
            result.Append("<dd>");
            foreach (var item in listmenu)
            {
                StringBuilder sb = new StringBuilder();
              

                var childlist = allmenu.Where(o => o.C_PARENT_ID == item.C_ID).ToList();
                if (childlist.Count > 0)
                {
                    sb.Append(" <li id=\"" + item.C_ID + "\" class=\"layui-nav-item\">");
                    sb.Append(" <a href=\"javascript:;\" > <span class=\"" + item.C_ICON + "\"></span>" + kongge + item.C_NAME + "</a>");
                    sb.Append(" <dl class=\"layui-nav-child\">");
                    sb.Append(getSecondMenu(childlist,allmenu,kongge));
                    sb.Append(" </dl>");
                    sb.Append(" </li>");
                }
                else
                {
                    sb.Append(" <a href=\"javascript:jump(\'" + item.C_URL + "\');\" > <span class=\"" + item.C_ICON + "\"></span>" + kongge + item.C_NAME + "</a>");
                }
                result.Append(sb.ToString());
               
            }

            result.Append(" </dd>");
            return result.ToString();
        }

        public static string getMenuNew(List<TS_MENU> listmenu)
        {
            var topmenu = listmenu.Where(o => o.C_PARENT_ID == "0");
            StringBuilder result = new StringBuilder();
            foreach (var item in topmenu)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" <li id=\"" + item.C_ID + "\" class=\"layui-nav-item\">");

                var childlist = listmenu.Where(o => o.C_PARENT_ID == item.C_ID).ToList();
                if (childlist.Count > 0)
                {
                    sb.Append(" <a href=\"javascript:;\" > <span class=\"" + item.C_ICON + "\"></span>" + item.C_NAME + "</a>");                  
                    sb.Append(getSecondMenuNew(childlist, listmenu, ""));
                  
                }
                else
                {
                    sb.Append(" <a href=\"javascript:jump(\'" + item.C_URL + "\');\" > <span class=\"" + item.C_ICON + "\"></span>" + item.C_NAME + "</a>");
                }
                sb.Append(" </li>");
                result.Append(sb.ToString());
            }

            return result.ToString();
        }


        public static string getSecondMenuNew(List<TS_MENU> listmenu, List<TS_MENU> allmenu, string kongge)
        {
            kongge += "&nbsp;&nbsp;";
            StringBuilder result = new StringBuilder();
            result.Append(" <dl class=\"layui-nav-child\">");
            foreach (var item in listmenu)
            {
                StringBuilder sb = new StringBuilder();


                var childlist = allmenu.Where(o => o.C_PARENT_ID == item.C_ID).ToList();
                if (childlist.Count > 0)
                {
                    //sb.Append(" <li id=\"" + item.C_ID + "\" class=\"layui-nav-item\">");
                    sb.Append(" <a href=\"javascript:;\" > <span class=\"" + item.C_ICON + "\"></span>" + kongge + item.C_NAME + "</a>");
                    //sb.Append(" <dl class=\"layui-nav-child\">");
                    sb.Append(getSecondMenuNew(childlist, allmenu, kongge));
                    //sb.Append(" </dl>");
                    //sb.Append(" </li>");
                }
                else
                {

                    sb.Append(" <dd> ");
                    sb.Append(" <a href=\"javascript:jump(\'" + item.C_URL + "\');\" > <span class=\"" + item.C_ICON + "\"></span>" + kongge + item.C_NAME + "</a>");
                    sb.Append(" </dd> ");
                }
                result.Append(sb.ToString());

            }

            result.Append(" </dl>");
            return result.ToString();
        }
    }
}