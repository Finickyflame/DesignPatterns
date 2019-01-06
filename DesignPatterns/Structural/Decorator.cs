using System;
using Xunit;

namespace DesignPatterns.Structural
{
    public class Decorator : DesignPattern
    {
        public override void Execute()
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
                this.Opened = true;
            }

            public void Close()
            {
                this.Opened = false;
            }
        }


        /// <summary>
        /// Decorator
        /// </summary>
        /// <remarks>
        /// Maintains a reference to a Component object and defines an interface that conforms to Component's interface.
        /// </remarks>
        public abstract class DoorDecorator : IDoor
        {
            private readonly IDoor _door;

            protected DoorDecorator(IDoor door)
            {
                this._door = door;
            }

            public virtual bool Opened => this._door.Opened;

            public virtual void Open() => this._door.Open();

            public virtual void Close() => this._door.Close();
        }

        /// <summary>
        /// Concrete Decorator
        /// </summary>
        /// <remarks>
        /// Adds responsibilities to the component
        /// </remarks>
        public class LockProtected : DoorDecorator
        {
            public LockProtected(IDoor door)
            : base(door)
            {
                this.Locked = true;
            }

            public bool Locked { get; private set; }


            public override void Open()
            {
                if (this.Locked)
                {
                    throw new DoorLockedException();
                }
                base.Open();
            }

            public override void Close()
            {
                if (this.Locked)
                {
                    throw new DoorLockedException();
                }
                base.Close();
            }

            public void Lock()
            {
                this.Locked = true;
            }

            public void UnLock()
            {
                this.Locked = false;
            }
        }

        public class DoorLockedException : Exception
        {
            public DoorLockedException()
            : base("Door is locked.")
            {

            }
        }
    }
}