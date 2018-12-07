using Xunit;

namespace DesignPatterns.Creational
{
    public class AbstractFactory : DesignPattern
    {
        public override void Execute()
        {
            ICarFactory carFactory = new AudiCarFactory();
            Car car = carFactory.CreateCar();
            Assert.Equal("Audi", car.GetMake());

            carFactory = new MazdaCarFactory();
            car = carFactory.CreateCar();
            Assert.Equal("Mazda", car.GetMake());
        }

        #region Definition
        private abstract class Car
        {
            public abstract string GetMake();
        }

        private interface ICarFactory
        {
            Car CreateCar();
        }
        #endregion

        #region Concrete implementation

        private class MazdaCar : Car
        {
            public override string GetMake() => "Mazda";
        }

        private class MazdaCarFactory : ICarFactory
        {
            public Car CreateCar()
            {
                return new MazdaCar();
            }
        }


        private class AudiCar : Car
        {
            public override string GetMake() => "Audi";
        }

        private class AudiCarFactory : ICarFactory
        {
            public Car CreateCar()
            {
                return new AudiCar();
            }
        }
        #endregion
    }
}
