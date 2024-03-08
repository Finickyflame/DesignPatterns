namespace DesignPatterns.Structural;

public class PrivateClassData
{
    [Fact]
    public void Execute()
    {
        const double radius = 4;

        var circle = new Circle(radius);
        Assert.Equal(radius * 2, circle.Diameter);
        Assert.Equal(radius * 2 * Math.PI, circle.Circumference);
    }

    public class Circle(double radius)
    {
        public double Circumference => Diameter * Math.PI;

        public double Diameter => Data.Radius * 2;

        private CircleData Data { get; } = new(radius);
    }

    /* Private class data */
    public class CircleData(double radius)
    {
        public double Radius { get; } = radius;
    }
}