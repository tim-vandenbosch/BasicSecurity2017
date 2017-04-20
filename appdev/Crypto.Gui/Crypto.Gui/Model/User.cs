using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Gui
{
    /// <summary>
    /// Class which represents a user
    /// </summary>
    public class User
    {
        public string Username { get; }
        private string Salt;
        private string Hash;
        
        //Suggestions:
        //ArrayList of Contacts?
        //Avater?
        //BackgroundColor?
        //MessageColor?


        /// <summary>
        /// Construstor for a user.
        /// This constructor can be used by the AuthorizeLogin(Read from file) and the RegisterUser(make new user) functions from the LoginScreen.
        /// </summary>
        /// <param name="username">The username of the user</param>
        /// <param name="salt">The salt added to the user's password</param>
        /// <param name="hash">The hash of the salted password</param>
        public User(string username, string salt, string hash)
        {
            this.Username = username;
            this.Salt = salt;
            this.Hash = hash;
        }


        public bool changePassword()
        {
            //This function changes the password which can be done from the account settings.
            //This function checks with the current Hash, otherwise it throws an error.
            //It returns a bool to check if it succeeded in changing the password or not.
            //This function will automatically change the salt/hash of the logged in user as well as the "user.xml" file of the user in his own folder.

            return true;
        }

        public bool CompareCredentials(string userName, string userSalt, string userHash)
        {
            //This function will be used to check the credentials when logging in.

            //Check if the username, salt and the hash are the same
            if (this.Username.Equals(userName) && this.Salt.Equals(userSalt) && this.Hash.Equals(userHash))
                return true;
            return false;
        }

    }
}
