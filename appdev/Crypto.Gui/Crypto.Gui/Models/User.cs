using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Crypto.Gui
{
    /// <summary>
    /// Class which represents a user
    /// </summary>

    public class User 
    {

        public string Username { set; get; }
        public string Salt { set; get; }
        public string Hash { set; get; }

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

        //Default contstructor
        public User()
        {
            
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

        public void SaveAsXML()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
           $"//WhatsUpp//{Username}";

            //Serializing the user.
            XmlSerializer serializer = new XmlSerializer(typeof(User));


            //Check if WhatsUpp folder exists
            if (!Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);

            FileStream file = File.Create(path + "//User.xml");
            serializer.Serialize(file, this);
            file.Close();

        }

        public User ReadFromXML(string Username)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
           $"//WhatsUpp//{Username}";


            //Serializing the user.
            XmlSerializer serializer = new XmlSerializer(typeof(User));


            //Check if WhatsUpp folder exists
            if (!Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);

            FileStream file = File.Create(path + "//User.xml");
            User newUser = (User) serializer.Deserialize(file);
            file.Close();

            return newUser;
        }

    }
}
