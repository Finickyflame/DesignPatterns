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

        public interface IMessage
        {
            string GetMessage();
        }

        #endregion

        #region Concrete Implementation

        public class Message : IMessage
        {
            private readonly string _message;

            public Message(string message)
            {
                this._message = message;
            }

            public string GetMessage() => this._message;
        }

        /* Decorator */
        public class MessageWithDecoration : IMessage
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
