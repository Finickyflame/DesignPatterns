namespace DesignPatterns.Structural;

public class Proxy
{
    [Fact]
    public void Execute()
    {
        IFriend friend = new FriendOverPhone();
        Assert.False(((FriendOverPhone)friend).IsConnectedToFriend);
        friend.Talk();
        Assert.True(((FriendOverPhone)friend).IsConnectedToFriend);
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
        public bool IsConnectedToFriend => Friend != null;
        private Friend Friend { get; set; }

        public void Talk()
        {
            if (!IsConnectedToFriend)
            {
                CallFriend();
            }
            Friend.Talk();
        }

        private void CallFriend()
        {
            Friend = new Friend();
        }
    }
}