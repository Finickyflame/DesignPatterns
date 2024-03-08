namespace DesignPatterns.Behavioral;

public class Command
{
    /// <summary>
    /// Client
    /// </summary>
    /// <remarks>
    /// Creates the commands and sets its receiver.
    /// </remarks>
    [Fact]
    public void Execute()
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
            foreach (ITask task in tasks)
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
        public CleanState BathroomState { get; set; }

        public CleanState BedroomState { get; set; }

        public CleanState KitchenState { get; set; }
    }

    public enum CleanState
    {
        Dirty, // Default state
        Clean
    }

    /// <summary>
    /// Concrete Command
    /// </summary>
    /// <remarks>
    /// - Defines a binding between a Receiver object and an action.
    /// - Implements Execute by invoking the corresponding operation(s) on Receiver.
    /// </remarks>
    public abstract class HouseTask(House house) : ITask
    {
        protected House House { get; } = house;

        public abstract void Execute();
    }

    /// <inheritdoc />
    public class CleanKitchenTask(House house) : HouseTask(house)
    {
        public override void Execute()
        {
            House.KitchenState = CleanState.Clean;
        }
    }

    /// <inheritdoc />
    public class CleanBathroomTask(House house) : HouseTask(house)
    {
        public override void Execute()
        {
            House.BathroomState = CleanState.Clean;
        }
    }

    /// <inheritdoc />
    public class CleanBedroomTask(House house) : HouseTask(house)
    {
        public override void Execute()
        {
            House.BedroomState = CleanState.Clean;
        }
    }

    /// <inheritdoc />
    public class CleanTheWholeHouseTask(House house) : HouseTask(house)
    {
        public override void Execute()
        {
            House.BathroomState = CleanState.Clean;
            House.BedroomState = CleanState.Clean;
            House.KitchenState = CleanState.Clean;
        }
    }
}