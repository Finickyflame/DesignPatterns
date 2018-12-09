using System;
using Xunit;

namespace DesignPatterns.Creational
{
    public class FactoryMethod : DesignPattern
    {
        public override void Execute()
        {
            Car mazdaCar = CreateCar("Mazda");
            Assert.Equal("Mazda", mazdaCar.GetMake());

            Car audiCar = CreateCar("Audi");
            Assert.Equal("Audi", audiCar.GetMake());

            Assert.Throws<ArgumentOutOfRangeException>(() => CreateCar("Honda"));
        }

        #region Definition
        public abstract class Car
        {
            public abstract string GetMake();
        }

        private static Car CreateCar(string make)
        {
            switch (make)
            {
                case "Mazda":
                    return new MazdaCar();
                case "Audi":
                    return new AudiCar();
                default:
                    throw new ArgumentOutOfRangeException(nameof(make));
            }
        }
        #endregion

        #region Concrete implementation

        public class MazdaCar : Car
        {
            public override string GetMake() => "Mazda";
        }

        public class AudiCar : Car
        {
            public override string GetMake() => "Audi";
        }
        #endregion
    }
}
