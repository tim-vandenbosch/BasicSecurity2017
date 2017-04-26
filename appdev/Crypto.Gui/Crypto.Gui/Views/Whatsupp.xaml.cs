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
    /// Interaction logic for Whatsupp.xaml
    /// </summary>
    public partial class Whatsupp : Window
    {
        public Whatsupp()
        {
            InitializeComponent();

            //this._authUser = authorizedUser;

            setupContactList();
            setupContentPanel();
            setupImageList();

            this.Show();
        }
        /// <summary>
        /// Function to load all the previous messages from the AuthorizedUser to the other user and the other way around.
        /// If no contact is selected it will show a appropriate message
        /// </summary>
        public void setupContentPanel()
        {

            //Load messages from autUser to selectedContact
            //Load messages from selectedContact to autUser
            //Put them in the same list and make a datetime comparator to sort the list on date-time.
            //Create the messages following to this list and add them to the contentPanel 

            //Test content

            this.contentPanel.Children.Add(new MessagePanel("Joske", "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus laoreet, diam iaculis mattis dignissim, sapien felis aliquam erat, sit amet lobortis felis erat nec quam. Nullam consequat sem sed tincidunt rutrum. Fusce lobortis urna in volutpat congue. Praesent at tellus sit amet nisl volutpat rhoncus eu sed lacus. Nam maximus risus dapibus dapibus tincidunt. Fusce a est turpis. Nulla facilisis, tellus at vulputate tempor, tortor ante scelerisque massa, sit amet porta urna nulla vel diam. Aliquam tristique condimentum ipsum, eget bibendum quam egestas nec. ",
new DateTime(2017, 03, 29, 20, 25, 00)));
            Label seperator = new Label();
            seperator.Content = "";
            this.contentPanel.Children.Add(seperator);

            this.contentPanel.Children.Add(new MessagePanel("Joske", "This is the second message",
            new DateTime(2017, 03, 29, 20, 25, 00)));
        }

        /// <summary>
        /// Function to load all the contacts of a person in the file.
        /// </summary>
        public void setupContactList()
        {
            //Load contactlist from the authorizedUser object.

            //Test content
            this.contactsListView.Items.Add("TestUser1");
        }

        /// <summary>
        /// Function to load all the images of a conversation.
        /// </summary>
        public void setupImageList()
        {
            //Load imagelist from the authorizedUser object.

            //Test content
            this.imageListView.Items.Add("TestImage1");
        }

        /// <summary>
        /// Function to open te account settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void accountButton_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Function to logout
        /// Function needs show a confirmation screen and save all unsaved settings.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult confirmationResult = MessageBox.Show(this, "Are you sure you want to logout?", "Logout",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirmationResult == MessageBoxResult.Yes)
            {
                //Save changes and logout   
            }
        }
    }
}
