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
        public abstract class Car
        {
            public abstract string GetMake();
        }

        public interface ICarFactory
        {
            Car CreateCar();
        }
        #endregion

        #region Concrete implementation

        public class MazdaCar : Car
        {
            public override string GetMake() => "Mazda";
        }

        public class MazdaCarFactory : ICarFactory
        {
            public Car CreateCar()
            {
                return new MazdaCar();
            }
        }


        public class AudiCar : Car
        {
            public override string GetMake() => "Audi";
        }

        public class AudiCarFactory : ICarFactory
        {
            public Car CreateCar()
            {
                return new AudiCar();
            }
        }
        #endregion
    }
}
