using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestApp.ViewModel
{
    public class FacebookList
    {
        private static ObservableCollection<Friend> friends = new ObservableCollection<Friend>();

        public static ObservableCollection<Friend> Friends
        {
            get
            {
                return friends;
            }
        }
    }
    public class UserOfAList
    {
        // Esta instancia se usa para mostrar los usuarios de las listas
        private static ObservableCollection<Friend> userlist = new ObservableCollection<Friend>();

        public static ObservableCollection<Friend> UserLists
        {
            get
            {
                return userlist;
            }
        }
    }
}
