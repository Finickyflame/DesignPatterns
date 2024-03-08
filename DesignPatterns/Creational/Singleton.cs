namespace DesignPatterns.Creational;

public class Singleton
{
    [Fact]
    public void Execute()
    {
        /*
         * You can't create a new Government:
         * var instance = new Government();
         * Because the constructor is inaccessible due to its protection level.
         */

        // So, the only way to get it, is by asking for its instance:
        var instance = Government.Instance;

        Assert.Equal(Government.Instance, instance);
        Assert.Equal(Government.Instance.FormedOn, instance.FormedOn);
    }

    /// <summary>
    /// Singleton
    /// </summary>
    /// <remarks>
    /// - Defines an Instance operation that lets clients access its unique instance. Instance is a class operation.
    /// - Responsible for creating and maintaining its own unique instance.
    /// </remarks>
    public class Government
    {
        /* Static constructor */
        static Government()
        {
            var random = new Random();
            int year = random.Next(1000, 2000);
            int month = random.Next(1, 12);
            int day = random.Next(1, DateTime.DaysInMonth(year, month));
            var randomDate = new DateTime(year, month, day);

            Instance = new Government(randomDate);
        }

        /* Instance constructor */
        private Government(DateTime formedOn)
        {
            // We keep the constructor private, so it can only be called by the static constructor.
            FormedOn = formedOn;
        }

        public DateTime FormedOn { get; }

        public static Government Instance { get; }
    }
}