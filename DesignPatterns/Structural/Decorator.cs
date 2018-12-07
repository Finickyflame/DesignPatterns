using Xunit;

namespace DesignPatterns.Structural
{
    public class Decorator : DesignPattern
    {
        public override void Execute()
        {
            IMessage defaultMessage = new Message("Hello World!");

            Assert.Equal("Hello World!", defaultMessage.GetMessage());

            IMessage messageWithDecoration = new MessageWithDecoration(defaultMessage);
            Assert.Equal("|!=* Hello World! *=!|", messageWithDecoration.GetMessage());
        }

        #region Definition

        private interface IMessage
        {
            string GetMessage();
        }

        #endregion

        #region Concrete Implementation

        private class Message : IMessage
        {
            private readonly string _message;

            public Message(string message)
            {
                this._message = message;
            }

            public string GetMessage() => this._message;
        }

        /* Decorator */
        private class MessageWithDecoration : IMessage
        {
            private readonly IMessage _message;

            public MessageWithDecoration(IMessage message)
            {
                this._message = message;
            }

            public string GetMessage() => $"|!=* {this._message.GetMessage()} *=!|";
        }

        #endregion
    }
}
