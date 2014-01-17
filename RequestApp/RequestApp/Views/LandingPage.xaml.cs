using Facebook;
using RequestApp.Common;
using RequestApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Dynamic;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// La plantilla de elemento Página básica está documentada en http://go.microsoft.com/fwlink/?LinkId=234237

namespace RequestApp.Views
{
    /// <summary>
    /// Página básica que proporciona características comunes a la mayoría de las aplicaciones.
    /// </summary>
    public sealed partial class LandingPage : RequestApp.Common.LayoutAwarePage
    {
        FacebookClient fb = new FacebookClient(App.AccessToken);
        string seleccion = null;
        //private SelectionMode _listModeSelection = SelectionMode.Single;

        //public SelectionMode ListModeSelection 
        //{
        //    get
        //    {
        //        return _listModeSelection;
        //    }
 
        //    set
        //    {
        //        if (value != _listModeSelection)
        //        {
        //            _listModeSelection = value;
        //            OnPropertyChanged("ListModeSelection");
        //        }
        //    }
             
        //}
        public LandingPage()
        {
            this.InitializeComponent();
            LoadUserInfo();
            showList();
            ItemListView.ItemsSource = FacebookListCreadas.Lists;
            selectList.ItemsSource = UserOfAList.UserLists;

        }

        /// <summary>
        /// Rellena la página con el contenido pasado durante la navegación. Cualquier estado guardado se
        /// proporciona también al crear de nuevo una página a partir de una sesión anterior.
        /// </summary>
        /// <param name="navigationParameter">Valor de parámetro pasado a
        /// <see cref="Frame.Navigate(Type, Object)"/> cuando se solicitó inicialmente esta página.
        /// </param>
        /// <param name="pageState">Diccionario del estado mantenido por esta página durante una sesión
        /// anterior. Será null la primera vez que se visite una página.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        /// <summary>
        /// Mantiene el estado asociado con esta página en caso de que se suspenda la aplicación o
        /// se descarte la página de la memoria caché de navegación. Los valores deben cumplir los requisitos
        /// de serialización de <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">Diccionario vacío para rellenar con un estado serializable.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }


        private async void LoadUserInfo()
        {

            dynamic parameters = new ExpandoObject();
            parameters.access_token = App.AccessToken;
            parameters.fields = "name";

            dynamic result = await fb.GetTaskAsync("me", parameters);

            string profilePictureUrl = string.Format("http://graph.facebook.com/{0}/picture?type={1}&access_token={2}", App.FacebookId, "large", fb.AccessToken);

            this.MyImage.Source = new BitmapImage(new Uri(profilePictureUrl));
            this.MyName.Text = result.name;
        }

        /// <summary>
        /// Este procedimiento me guarda todos mis usuarios en el grid
        /// </summary>
        async private void showList()
        {
            dynamic friendsTaskResult = await fb.GetTaskAsync("/me/friendlists");
            var resultJSON = (IDictionary<string, object>)friendsTaskResult; ///Este es el JSON que devuelve
            var data = (IEnumerable<object>)resultJSON["data"]; //Aqui hago el filtro, Juanma, Rosana
            foreach (var item in data)
            {
                var lista = (IDictionary<string, object>)item;
                FacebookListCreadas.Lists.Add(new ListFacebook { //AllLists
                    idList = (string)lista["id"],
                    Name = (string)lista["name"],
                    List_Type = (string)lista["list_type"] });
                //System.Diagnostics.Debug.WriteLine((string)lista["name"]);
            }
        }

        async private void btnShowAllFriends_Click(object sender, RoutedEventArgs e)
        {

            dynamic friendsTaskResult = await fb.GetTaskAsync("/me/friends");
            var resultJSON = (IDictionary<string, object>)friendsTaskResult; ///Este es el JSON que devuelve
            var data = (IEnumerable<object>)resultJSON["data"]; //Aqui hago el filtro, Juanma, Rosana
            foreach (var item in data)
            {
                var friend = (IDictionary<string, object>)item;
                FacebookList.Friends.Add(new Friend
                {

                    Name = (string)friend["name"],
                    id = (string)friend["id"],
                    PictureUri = new Uri(string.Format("https://graph.facebook.com/{0}/picture?type={1}&access_token={2}",
                    (string)friend["id"],
                    "large",
                    App.AccessToken))
                    
                });
                System.Diagnostics.Debug.WriteLine((string)friend["id"]);
            }
            allUsersGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
            selectList.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        //This procedure shows the users of a friendlist 

        async private void ItemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UserOfAList.UserLists.Clear();
            ListView lv = sender as ListView;
            ListFacebook itemmm = (ListFacebook)lv.SelectedItem;
            string id = itemmm.idList;
            seleccion = id;
            dynamic members = await fb.GetTaskAsync("/"+id+"/members");

            
            var resultJSON = (IDictionary<string, object>)members;
            var data = (IEnumerable<object>)resultJSON["data"];
            foreach (var item in data)
            {
                var user = (IDictionary<string, object>)item;
                UserOfAList.UserLists.Add(new Friend
                {
                    Name = (string)user["name"],
                    id = (string)user["id"],
                    PictureUri = new Uri(string.Format("https://graph.facebook.com/{0}/picture?type={1}&access_token={2}",
                    (string)user["id"],
                    "large",
                    App.AccessToken))
                });
            }

            allUsersGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            selectList.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        private void allUsersGrid_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {

        }

        private void allUsersGrid_Drop(object sender, DragEventArgs e)
        {

        }

        private void List_DragItems(object sender, DragItemsStartingEventArgs e)
        {

        }

        private void List_DropItems(object sender, DragEventArgs e)
        {

        }

        async private void Button_Click_Add_List(object sender, RoutedEventArgs e)
        {
             // ItemListView.get
            //if (seleccion != null)
           // {

          /*  dynamic fbPostTaskResult = await fb.PostTaskAsync("2375966128254/members/1426969287");
            var result = (IDictionary<string, object>)fbPostTaskResult;*/
            try
            {
                dynamic fbPostAdd = await fb.PostTaskAsync("2375966128254/members/100001006332355");
            }
            catch (Exception ex)
            {
                MessageDialog exceptionMessageDialog = new MessageDialog("Exception during post: " + ex.Message);
                exceptionMessageDialog.ShowAsync();
            }

            //BORRA AMIGOS
            //dynamic fbDeleteResult = await fb.DeleteTaskAsync("2375966128254/members/100001006332355");

                /*if (result.g == true)
                {
                    string message = String.Empty;
                    message = "Usuario creado correctamente";
                    MessageDialog dialog = new MessageDialog(message);
                    dialog.ShowAsync();
                }*/

           // }

           
            System.Diagnostics.Debug.WriteLine(seleccion);

        }
    }
}
