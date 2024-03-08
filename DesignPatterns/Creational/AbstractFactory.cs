namespace DesignPatterns.Creational;

public class AbstractFactory
{
    [Fact]
    public void Execute()
    {
        var carDealer = new CarDealer(new VolvoVehicleFactory());
        Car car = carDealer.Car;
        Assert.Equal("Volvo", car.GetMake());

        Truck truck = carDealer.Truck;
        Assert.Equal("Volvo", truck.GetMake());


        carDealer = new CarDealer(new FordVehicleFactory());
        car = carDealer.Car;
        Assert.Equal("Ford", car.GetMake());

        truck = carDealer.Truck;
        Assert.Equal("Ford", truck.GetMake());
    }

    /// <summary>
    /// Abstract Factory
    /// </summary>
    /// <remarks>
    /// Declares an interface for operations that create abstract products.
    /// </remarks>
    public interface IVehicleFactory
    {
        Car CreateCar();

        Truck CreateTruck();
    }

    /// <summary>
    /// Abstract Product
    /// </summary>
    /// <remarks>
    /// Declares an interface for a type of product object.
    /// </remarks>
    public abstract class Car
    {
        public abstract string GetMake();
    }

    /// <summary>
    /// Abstract Product
    /// </summary>
    /// <remarks>
    /// Declares an interface for a type of product object.
    /// </remarks>
    public abstract class Truck
    {
        public abstract string GetMake();
    }

    /// <summary>
    /// Client
    /// </summary>
    /// <remarks>
    /// Uses interfaces declared by Abstract Factory and Abstract Product classes.
    /// </remarks>
    public class CarDealer(IVehicleFactory vehicleFactory)
    {
        public Car Car { get; } = vehicleFactory.CreateCar();

        public Truck Truck { get; } = vehicleFactory.CreateTruck();
    }

    /// <summary>
    /// Product
    /// </summary>
    /// <remarks>
    /// - Defines a product object to be created by the corresponding concrete factory.
    /// - Implements the Abstract Product interface.
    /// </remarks>
    public class FordCar : Car
    {
        public override string GetMake() => "Ford";
    }

    /// <summary>
    /// Product
    /// </summary>
    /// <remarks>
    /// - Defines a product object to be created by the corresponding concrete factory.
    /// - Implements the Abstract Product interface.
    /// </remarks>
    public class FordTruck : Truck
    {
        public override string GetMake() => "Ford";
    }

    /// <summary>
    /// Concrete Factory
    /// </summary>
    /// <remarks>
    /// Implements the operations to create concrete product objects.
    /// </remarks>
    public class FordVehicleFactory : IVehicleFactory
    {
        public Car CreateCar() => new FordCar();

        public Truck CreateTruck() => new FordTruck();
    }

    /// <summary>
    /// Product
    /// </summary>
    /// <remarks>
    /// - Defines a product object to be created by the corresponding concrete factory.
    /// - Implements the Abstract Product interface.
    /// </remarks>
    public class VolvoCar : Car
    {
        public override string GetMake() => "Volvo";
    }

    /// <summary>
    /// Product
    /// </summary>
    /// <remarks>
    /// - Defines a product object to be created by the corresponding concrete factory.
    /// - Implements the Abstract Product interface.
    /// </remarks>
    public class VolvoTruck : Truck
    {
        public override string GetMake() => "Volvo";
    }

    /// <summary>
    /// Concrete Factory
    /// </summary>
    /// <remarks>
    /// Implements the operations to create concrete product objects.
    /// </remarks>
    public class VolvoVehicleFactory : IVehicleFactory
    {
        public Car CreateCar() => new VolvoCar();

        public Truck CreateTruck() => new VolvoTruck();
    }
}