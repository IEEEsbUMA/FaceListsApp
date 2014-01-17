using Facebook.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en http://go.microsoft.com/fwlink/?LinkId=234238

namespace RequestApp.Views
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    /// 



    public sealed partial class HomePage : Page
    {

        private FacebookSession session;


        public HomePage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Se invoca cuando esta página se va a mostrar en un objeto Frame.
        /// </summary>
        /// <param name="e">Datos de evento que describen cómo se llegó a esta página. La propiedad Parameter
        /// se usa normalmente para configurar la página.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        async private void btnFacebookLogin_Click(object sender, RoutedEventArgs e)
        {
            if (!App.isAuthenticated)
            {
                App.isAuthenticated = true;
                await Authenticate();
            }
        }

        async private Task Authenticate()
        {
           
            try
            {
                session = await App.FacebookSessionClient.LoginAsync("user_about_me,read_stream, read_friendlists, manage_friendlists,user_relationships");
                if (session != null)
                {
                    App.AccessToken = session.AccessToken;
                    App.FacebookId = session.FacebookId;

                    Frame.Navigate(typeof(LandingPage));
                }
                else
                {
                    RequestApp.App.WriteLine(session.ToString);
                }

               
            }
            catch (InvalidOperationException e)
            {
                string message = "Login failed! Exception details: " + e.Message;
                MessageDialog dialog = new MessageDialog(message);
                dialog.ShowAsync();
            }
        }

    }
}
