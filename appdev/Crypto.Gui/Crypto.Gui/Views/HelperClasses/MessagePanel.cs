using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Crypto.Gui
{
    /// <summary>
    /// This is a class to make the VIEW of a message.
    /// Class which represents a Message from a person to another person.
    /// These messagePanels are added to the contentPanel and will later be saved to a file
    /// </summary>
    public class MessagePanel : Canvas
    {
        private string _sender;
        private string _message;
        private DateTime _messageTime;
        private TextBlock _messageTextBlock = new TextBlock();
        private Image _avatarOfSender = new Image(); //Extra?
        private Label _timeLabel = new Label();
        


        //TODO: Needs to switch sender from string to User object
        //TODO: User object will have Username (And maybe later avatar) data.
        public MessagePanel(string sender, string message, DateTime messageTime)
        {
            this._message = message;
            this._messageTime = messageTime;
            this._sender = sender;

            Init();
        }
        /// <summary>
        /// Setting up the messagePanel
        /// </summary>
        private void Init()
        {
            
            //Setup Message
            _messageTextBlock.Text = _message;
            _messageTextBlock.Width = 600;
            _messageTextBlock.TextWrapping = TextWrapping.Wrap;
            Canvas.SetLeft(_messageTextBlock, 10);
            Canvas.SetTop(_messageTextBlock, 50);


            //Setup Time of Message
            _timeLabel.Content = _messageTime.ToLongDateString() + " " +_messageTime.ToLongTimeString();
            Canvas.SetTop(_timeLabel, 10);
            
            //Setup messagePanel
            _messageTextBlock.Measure(new Size());
            _messageTextBlock.Arrange(new Rect());
            
            this.Width = 672-10-20-10;
            this.Height = _messageTextBlock.ActualHeight + 70;
            
            this.Background = new SolidColorBrush(Colors.CornflowerBlue); //Put this in the userFile or let it pick randomly from a hardcoded list?
            this.Children.Add(_messageTextBlock);
            this.Children.Add(_timeLabel);
            
        }
    }
}