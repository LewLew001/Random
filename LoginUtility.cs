//-----------------------------------------------------------------------------
        void Session_Start(object sender, EventArgs e)
        {
            LoginData.Initialize(HttpContext.Current.User.Identity.Name);
        }
//-----------------------------------------------------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            TextBox1.Text = $"{LoginData.GroupsCsvValue} ---  Admin: {LoginData.HasGroup(GroupType.ADMIN)} --- Group1: {LoginData.HasGroup(GroupType.GROUP1)} --- OldValue: {LoginData.OldGroupValue}";
        }

//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1
{
    //------------------------------------
    [Flags]
    public enum GroupType
    {
        NONE = 0,
        ADMIN = 1,
        USER = 2,
        GROUP1 = 4,
        GROUP2 = 8
    }
    //------------------------------------
    public static class LoginData
    {
        //------------------------------------
        private const string UserGroupsSessionKey = "UserGroups";
        private const string OldInfoSessionKey = "Group";
        //------------------------------------
        public static List<GroupType> Groups
        {
            get => HttpContext.Current.Session[UserGroupsSessionKey] as List<GroupType>;
            set
            {
                HttpContext.Current.Session[UserGroupsSessionKey] = value;
            }
        }
        public static string GroupsCsvValue => Groups != null ? string.Join(",", Groups) : string.Empty;
        public static bool HasGroup(GroupType group) => Groups != null && Groups.Count > 0 && Groups.Contains(group);
        //------------------------------------
        /// <summary>
        /// Paleidimui is global.asax informacijos uzpildymui
        /// </summary>
        /// <param name="login">Vartotojo login</param>
        public static void Initialize(string login)
        {
            List<GroupType> loginGroups = GetUserGroups(login);
            HttpContext.Current.Session[UserGroupsSessionKey] = loginGroups;
            SetOldValue();
        }
        //------------------------------------
        private static List<GroupType> GetUserGroups(string login)
        {
            // Selectas is duomenu bazes, kad gauti grupes pagal login
            return new List<GroupType> { GroupType.ADMIN, GroupType.USER, GroupType.GROUP2 };
        }

        //------------------------------------
        [ObsoleteAttribute("Sena versija, jei prireiktu ar liktu kazkur skyle, kad butu reiksme ir neluztu programa", false)]
        public static string OldGroupValue
        {
            get => HttpContext.Current.Session[OldInfoSessionKey] as string;
            private set
            {
                HttpContext.Current.Session[OldInfoSessionKey] = value;
            }
        }
        //------------------------------------
        private static void SetOldValue()
        {
            HttpContext.Current.Session[OldInfoSessionKey] = Groups.FirstOrDefault().ToString() ?? string.Empty;
        }
        //------------------------------------
        //------------------------------------
        //------------------------------------
    }
}







