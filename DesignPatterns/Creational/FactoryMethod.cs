﻿namespace DesignPatterns.Creational;

public class FactoryMethod
{
    [Fact]
    public void Execute()
    {
        ICarFactory carFactory = new AudiCarFactory();
        Car car = carFactory.CreateCar();
        Assert.Equal("Audi", car.Make);

        carFactory = new MazdaCarFactory();
        car = carFactory.CreateCar();
        Assert.Equal("Mazda", car.Make);
    }

    /// <summary>
    /// Product
    /// </summary>
    /// <remarks>
    /// Defines the interface of objects the factory method creates.
    /// </remarks>
    public abstract class Car
    {
        public abstract string Make { get; }
    }

    /// <summary>
    /// Creator
    /// </summary>
    /// <remarks>
    /// - Declares the factory method, which returns an object of type Product.
    /// - Creator may also define a default implementation of the factory method that returns a default Concrete Product
    /// object.
    /// </remarks>
    public interface ICarFactory
    {
        // Factory Method
        Car CreateCar();
    }

    /// <summary>
    /// Concrete Product
    /// </summary>
    /// <remarks>
    /// Implements the Product interface.
    /// </remarks>
    public class MazdaCar : Car
    {
        public override string Make => "Mazda";
    }

    /// <summary>
    /// Concrete Creator
    /// </summary>
    /// <remarks>
    /// Overrides the factory method to return an instance of a ConcreteProduct.
    /// </remarks>
    public class MazdaCarFactory : ICarFactory
    {
        public Car CreateCar() => new MazdaCar();
    }


    /// <summary>
    /// Concrete Product
    /// </summary>
    /// <remarks>
    /// Implements the Product interface.
    /// </remarks>
    public class AudiCar : Car
    {
        public override string Make => "Audi";
    }

    /// <summary>
    /// Concrete Creator
    /// </summary>
    /// <remarks>
    /// Overrides the factory method to return an instance of a ConcreteProduct.
    /// </remarks>
    public class AudiCarFactory : ICarFactory
    {
        public Car CreateCar() => new AudiCar();
    }
}