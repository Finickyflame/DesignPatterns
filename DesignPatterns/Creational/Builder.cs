namespace DesignPatterns.Creational;

public class Builder
{
    [Fact]
    public void Execute()
    {
        var chef = new PizzaChef();

        chef.SetPizzaRecipe(new PepperoniPizzaRecipe());
        chef.ConstructPizza();
        Pizza pepperoniPizza = chef.ServePizza();

        Assert.Equal(Sauce.Tomato, pepperoniPizza.Sauce);
        Assert.Equal(Topping.Pepperoni | Topping.Cheese, pepperoniPizza.Toppings);

        chef.SetPizzaRecipe(new SpecialPizzaRecipe());
        chef.ConstructPizza();
        Pizza specialPizza = chef.ServePizza();

        Assert.Equal(Sauce.Spicy, specialPizza.Sauce);
        Assert.Equal(Topping.Bacon | Topping.Cheese | Topping.Onion | Topping.Sausage, specialPizza.Toppings);
    }

    public enum Sauce
    {
        Tomato,
        Spicy
    }

    [Flags]
    public enum Topping
    {
        Sausage = 1,
        Pepperoni = 2,
        Onion = 4,
        Cheese = 8,
        Bacon = 16
    }

    /// <summary>
    /// Product
    /// </summary>
    /// <remarks>
    /// - Represents the complex object under construction.
    /// - Concrete Builder builds the product's internal representation and defines the process by which it's assembled.
    /// - Includes classes that define the constituent parts, including interfaces for assembling the parts into the final
    /// result
    /// </remarks>
    public class Pizza
    {
        public Sauce Sauce { get; private set; }

        public Topping Toppings { get; private set; }

        public void SetSauce(Sauce sauce)
        {
            Sauce = sauce;
        }

        public void SetToppings(Topping toppings)
        {
            Toppings = toppings;
        }
    }

    /// <summary>
    /// Director
    /// </summary>
    /// <remarks>
    /// Constructs an object using the Builder interface.
    /// </remarks>
    public class PizzaChef
    {
        private readonly Queue<Pizza> _createdPizzas;
        private IPizzaRecipe _recipe;

        public PizzaChef()
        {
            _createdPizzas = new Queue<Pizza>();
        }

        public void SetPizzaRecipe(IPizzaRecipe recipe)
        {
            _recipe = recipe;
        }

        public void ConstructPizza()
        {
            _recipe.PrepareNewPizza();
            _recipe.AddSauce();
            _recipe.AddToppings();
            _createdPizzas.Enqueue(_recipe.ServePizza());
        }

        public Pizza ServePizza() => _createdPizzas.Dequeue();
    }

    /// <summary>
    /// Builder
    /// </summary>
    /// <remarks>
    /// Specifies an abstract interface for creating parts of a Product object.
    /// </remarks>
    public interface IPizzaRecipe
    {
        void PrepareNewPizza();

        void AddSauce();

        void AddToppings();

        Pizza ServePizza();
    }

    /// <summary>
    /// Concrete Builder
    /// </summary>
    /// <remarks>
    /// - Constructs and assembles parts of the product by implementing the Builder interface.
    /// - Defines and keeps track of the representation it creates.
    /// - Provides an interface for retrieving the product.
    /// </remarks>
    public abstract class BasePizzaRecipe : IPizzaRecipe
    {
        protected Pizza Pizza { get; private set; }

        public void PrepareNewPizza()
        {
            Pizza = new Pizza();
        }

        public abstract void AddSauce();

        public abstract void AddToppings();

        public Pizza ServePizza() => Pizza;
    }

    /// <summary>
    /// Concrete Builder
    /// </summary>
    /// <remarks>
    /// - Constructs and assembles parts of the product by implementing the Builder interface.
    /// - Defines and keeps track of the representation it creates.
    /// - Provides an interface for retrieving the product.
    /// </remarks>
    public class PepperoniPizzaRecipe : BasePizzaRecipe
    {
        public override void AddSauce()
        {
            Pizza?.SetSauce(Sauce.Tomato);
        }

        public override void AddToppings()
        {
            Pizza?.SetToppings(Topping.Pepperoni | Topping.Cheese);
        }
    }

    /// <summary>
    /// Concrete Builder
    /// </summary>
    /// <remarks>
    /// - Constructs and assembles parts of the product by implementing the Builder interface.
    /// - Defines and keeps track of the representation it creates.
    /// - Provides an interface for retrieving the product.
    /// </remarks>
    public class SpecialPizzaRecipe : BasePizzaRecipe
    {
        public override void AddSauce()
        {
            Pizza?.SetSauce(Sauce.Spicy);
        }

        public override void AddToppings()
        {
            Pizza?.SetToppings(Topping.Bacon | Topping.Cheese | Topping.Onion | Topping.Sausage);
        }
    }
}