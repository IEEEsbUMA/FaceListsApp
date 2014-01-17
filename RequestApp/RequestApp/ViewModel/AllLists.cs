using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RequestApp.ViewModel;

namespace RequestApp.ViewModel
{
    public class FacebookListCreadas
    {
        private static ObservableCollection<ListFacebook> list = new ObservableCollection<ListFacebook>();

        public static ObservableCollection<ListFacebook> Lists
        {
            get
            {
                return list;
            }
        }
    }
}
