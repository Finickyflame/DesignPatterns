using System;
using Xunit;

namespace DesignPatterns.Structural
{
    public class PrivateClassData : DesignPattern
    {
        public override void Execute()
        {
            const double radius = 4;

            var circle = new Circle(radius);
            Assert.Equal(radius * 2, circle.Diameter);
            Assert.Equal(radius * 2 * Math.PI, circle.Circumference);
        }

        public class Circle
        {
            public Circle(double radius)
            {
                this.Data = new CircleData(radius);
            }

            private CircleData Data { get; }

            public double Circumference => this.Diameter * Math.PI;

            public double Diameter => this.Data.Radius * 2;
        }

        /* Private class data */
        public class CircleData
        {
            public CircleData(double radius)
            {
                this.Radius = radius;
            }

            public double Radius { get; }
        }
    }
}
