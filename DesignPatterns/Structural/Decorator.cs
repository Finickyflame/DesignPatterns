namespace DesignPatterns.Structural;

public class Decorator
{
    [Fact]
    public void Execute()
    {
        var normalDoor = new Door();
        Assert.False(normalDoor.Opened);

        normalDoor.Open();
        Assert.True(normalDoor.Opened);

        normalDoor.Close();
        Assert.False(normalDoor.Opened);


        var doorWithLock = new LockProtected(normalDoor);
        Assert.False(doorWithLock.Opened);
        Assert.True(doorWithLock.Locked);

        Assert.Throws<DoorLockedException>(() => doorWithLock.Open());
        Assert.False(doorWithLock.Opened);
        Assert.True(doorWithLock.Locked);

        doorWithLock.UnLock();
        Assert.False(doorWithLock.Opened);
        Assert.False(doorWithLock.Locked);

        doorWithLock.Open();
        Assert.True(doorWithLock.Opened);
        Assert.False(doorWithLock.Locked);

        doorWithLock.Close();
        Assert.False(doorWithLock.Opened);
        Assert.False(doorWithLock.Locked);

        doorWithLock.Lock();
        Assert.False(doorWithLock.Opened);
        Assert.True(doorWithLock.Locked);
    }

    /// <summary>
    /// Component
    /// </summary>
    /// <remarks>
    /// Defines the interface for objects that can have responsibilities added to them dynamically.
    /// </remarks>
    public interface IDoor
    {
        bool Opened { get; }

        void Open();

        void Close();
    }


    /// <summary>
    /// Concrete Component
    /// </summary>
    /// <remarks>
    /// Defines an object to which additional responsibilities can be attached.
    /// </remarks>
    public class Door : IDoor
    {
        public bool Opened { get; private set; }

        public void Open()
        {
            Opened = true;
        }

        public void Close()
        {
            Opened = false;
        }
    }


    /// <summary>
    /// Decorator
    /// </summary>
    /// <remarks>
    /// Maintains a reference to a Component object and defines an interface that conforms to Component's interface.
    /// </remarks>
    public abstract class DoorDecorator(IDoor door) : IDoor
    {
        public virtual bool Opened => door.Opened;

        public virtual void Open() => door.Open();

        public virtual void Close() => door.Close();
    }

    /// <summary>
    /// Concrete Decorator
    /// </summary>
    /// <remarks>
    /// Adds responsibilities to the component
    /// </remarks>
    public class LockProtected(IDoor door) : DoorDecorator(door)
    {
        public bool Locked { get; private set; } = true;

        public override void Open()
        {
            if (Locked)
            {
                throw new DoorLockedException();
            }
            base.Open();
        }

        public override void Close()
        {
            if (Locked)
            {
                throw new DoorLockedException();
            }
            base.Close();
        }

        public void Lock()
        {
            Locked = true;
        }

        public void UnLock()
        {
            Locked = false;
        }
    }

    public class DoorLockedException() : Exception("Door is locked.");
}