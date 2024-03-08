namespace DesignPatterns.Creational;

public class Prototype
{
    /// <summary>
    /// Client
    /// </summary>
    /// <remarks>
    /// Creates a new object by asking a prototype to clone itself.
    /// </remarks>
    [Fact]
    public void Execute()
    {
        var sheepCollection = new Sheep[]
        {
            new BlackSheep(),
            new WhiteSheep()
        };

        foreach (Sheep sheep in sheepCollection)
        {
            Sheep clone = sheep.Clone();
            clone.Shave();

            Assert.NotEqual(sheep.Shaved, clone.Shaved);
            Assert.Equal(sheep.HairColor, clone.HairColor);
        }
    }

    /// <summary>
    /// Prototype
    /// </summary>
    /// <remarks>
    /// Declares an interface for cloning itself.
    /// </remarks>
    public abstract class Sheep(string color)
    {
        public string HairColor { get; } = color;

        public bool Shaved { get; private set; }

        public abstract Sheep Clone();

        public void Shave()
        {
            Shaved = true;
        }
    }

    /// <summary>
    /// Concrete Prototype
    /// </summary>
    /// <remarks>
    /// Implements an operation for cloning itself
    /// </remarks>
    public class BlackSheep() : Sheep("Black")
    {
        public override Sheep Clone() => new BlackSheep();
    }

    /// <summary>
    /// Concrete Prototype
    /// </summary>
    /// <remarks>
    /// Implements an operation for cloning itself
    /// </remarks>
    public class WhiteSheep() : Sheep("White")
    {
        public override Sheep Clone() => new WhiteSheep();
    }
}