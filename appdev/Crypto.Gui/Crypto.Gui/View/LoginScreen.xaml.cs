using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Crypto.Gui
{
    /// <summary>
    /// Interaction logic for LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
        public LoginScreen()
        {
            InitializeComponent();
        }

        private void AfterLogin()
        {
            //Loading the contactList
            //Loading the avatar
            //Loading the ImageList
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            //Check if a folder exists of the given username (the new username of the registering user)
            //if it exists --> The username is already taken, and we don't allow duplicates, this means the new user needs to change his username.    
            //If it does not exist --> New user, create new folder and new info.xml file with given credentials (Password is hashed and salted).

            //For new user.xml creation
            //Generate random salt
            //Add to user password and create hash
            //Make new User object and serialize it with XMLSerializer to make it easier to write to the user.xml file.
            //Store username (redundant?), salt and hash in the user.xml file 
            //Store public keys and private keys in the same folder.
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            //Check the user credentials with the "user.xml" file in the /"username" folder.
            //Load the user info from that xml file.
            //If correct --> Welcome "user" and proceed to the Homepage with that user info.
            //If false --> display "Incorrect password or username"        

            //Login will never tell if a user exists or not, even if the user inputs the correct username.

            //Calls afterLogin if succesfull
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            //Exit and closes the application.

        }

        private void aboutButton_Click(object sender, RoutedEventArgs e)
        {
            //Display new window with information about the team/platform/license.
        }
    }
}
