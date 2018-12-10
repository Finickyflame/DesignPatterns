using Xunit;

namespace DesignPatterns.Behavioral
{
    public class Command : DesignPattern
    {
        /// <summary>
        /// Client
        /// </summary>
        /// <remarks>
        /// Creates the commands and sets its receiver.
        /// </remarks>
        public override void Execute()
        {
            var maid = new Maid();

            var firstHouse = new House();
            Assert.Equal(CleanState.Dirty, firstHouse.BathroomState);
            Assert.Equal(CleanState.Dirty, firstHouse.BedroomState);
            Assert.Equal(CleanState.Dirty, firstHouse.KitchenState);

            // The client asks the maid to clean 2 rooms.
            maid.ExecuteTasks(new CleanBathroomTask(firstHouse), new CleanKitchenTask(firstHouse));

            Assert.Equal(CleanState.Clean, firstHouse.BathroomState);
            Assert.Equal(CleanState.Dirty, firstHouse.BedroomState);
            Assert.Equal(CleanState.Clean, firstHouse.KitchenState);


            var secondHouse = new House();

            // The client only wants the bedroom to be cleaned.
            maid.ExecuteTasks(new CleanBedroomTask(secondHouse));

            Assert.Equal(CleanState.Dirty, secondHouse.BathroomState);
            Assert.Equal(CleanState.Clean, secondHouse.BedroomState);
            Assert.Equal(CleanState.Dirty, secondHouse.KitchenState);


            var thirdHouse = new House();

            // The client wants to have the whole house cleaned.
            maid.ExecuteTasks(new CleanTheWholeHouseTask(thirdHouse));

            Assert.Equal(CleanState.Clean, thirdHouse.BathroomState);
            Assert.Equal(CleanState.Clean, thirdHouse.BedroomState);
            Assert.Equal(CleanState.Clean, thirdHouse.KitchenState);
        }

        #region Definition

        /// <summary>
        /// Command
        /// </summary>
        /// <remarks>
        /// Defines the interface for executing an operation.
        /// </remarks>
        public interface ITask
        {
            void Execute();
        }

        /// <summary>
        /// Invoker
        /// </summary>
        /// <remarks>
        /// Handles the commands and determines when they are executed.
        /// </remarks>
        public class Maid
        {
            public void ExecuteTasks(params ITask[] tasks)
            {
                foreach(ITask task in tasks)
                {
                    task.Execute();
                }
            }
        }

        /// <summary>
        /// Receiver
        /// </summary>
        /// <remarks>
        /// Receives actions via commands.
        /// </remarks>
        public class House
        {
            public CleanState KitchenState { get; set; }

            public CleanState BathroomState { get; set; }

            public CleanState BedroomState { get; set; }
        }

        public enum CleanState
        {
            Dirty, // Default state
            Clean
        }

        #endregion

        #region Concrete Implementations

        public abstract class HouseTask : ITask
        {
            protected HouseTask(House house)
            {
                this.House = house;
            }

            protected House House { get; }

            public abstract void Execute();
        }

        public class CleanKitchenTask : HouseTask
        {
            public CleanKitchenTask(House house)
                : base(house)
            {
            }

            public override void Execute()
            {
                this.House.KitchenState = CleanState.Clean;
            }
        }

        public class CleanBathroomTask : HouseTask
        {
            public CleanBathroomTask(House house)
                : base(house)
            {
            }

            public override void Execute()
            {
                this.House.BathroomState = CleanState.Clean;
            }
        }

        public class CleanBedroomTask : HouseTask
        {
            public CleanBedroomTask(House house)
                : base(house)
            {
            }

            public override void Execute()
            {
                this.House.BedroomState = CleanState.Clean;
            }
        }

        public class CleanTheWholeHouseTask : HouseTask
        {
            public CleanTheWholeHouseTask(House house)
                : base(house)
            {
            }

            public override void Execute()
            {
                this.House.BathroomState = CleanState.Clean;
                this.House.BedroomState = CleanState.Clean;
                this.House.KitchenState = CleanState.Clean;
            }
        }

        #endregion
    }
}
