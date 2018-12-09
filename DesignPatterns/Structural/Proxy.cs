using Xunit;

namespace DesignPatterns.Structural
{
    public class Proxy : DesignPattern
    {
        public override void Execute()
        {
            IFriend friend = new FriendOverPhone();
            Assert.False(((FriendOverPhone) friend).IsConnectedToFriend);
            friend.Talk();
            Assert.True(((FriendOverPhone) friend).IsConnectedToFriend);
        }

        /* Subject */
        public interface IFriend
        {
            void Talk();
        }

        /* Real subject */
        public class Friend : IFriend
        {
            public void Talk()
            {
            }
        }

        /* Proxy */
        public class FriendOverPhone : IFriend
        {
            private Friend Friend { get; set; }

            public bool IsConnectedToFriend => this.Friend != null;

            public void Talk()
            {
                if (!this.IsConnectedToFriend)
                {
                    this.CallFriend();
                }
                this.Friend.Talk();
            }

            private void CallFriend()
            {
                this.Friend = new Friend();
            }
        }
    }
}
