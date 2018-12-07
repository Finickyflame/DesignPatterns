using System;
using System.Collections.Generic;
using Xunit;

namespace DesignPatterns.Creational
{
    public class Builder : DesignPattern
    {
        public override void Execute()
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

        #region Definition

        private enum Sauce
        {
            Tomato,
            Spicy
        }

        [Flags]
        private enum Topping
        {
            Sausage = 1,
            Pepperoni = 2,
            Onion = 4,
            Cheese = 8,
            Bacon = 16
        }

        private class Pizza
        {
            public Sauce Sauce { get; private set; }

            public Topping Toppings { get; private set; }

            public void SetSauce(Sauce sauce)
            {
                this.Sauce = sauce;
            }

            public void SetToppings(Topping toppings)
            {
                this.Toppings = toppings;
            }
        }

        private class PizzaChef
        {
            private IPizzaRecipe _recipe;
            private readonly Queue<Pizza> _createdPizzas;

            public PizzaChef()
            {
                this._createdPizzas = new Queue<Pizza>();
            }

            public void SetPizzaRecipe(IPizzaRecipe recipe)
            {
                this._recipe = recipe;
            }

            public void ConstructPizza()
            {
                this._recipe.PrepareNewPizza();
                this._recipe.AddSauce();
                this._recipe.AddToppings();
                this._createdPizzas.Enqueue(this._recipe.ServePizza());
            }

            public Pizza ServePizza()
            {
                return this._createdPizzas.Dequeue();
            }
        }

        private interface IPizzaRecipe
        {
            void PrepareNewPizza();

            void AddSauce();

            void AddToppings();

            Pizza ServePizza();
        }

        #endregion

        #region Concrete Implementation

        private abstract class BasePizzaRecipe : IPizzaRecipe
        {
            protected Pizza Pizza { get; private set; }

            public void PrepareNewPizza()
            {
                this.Pizza = new Pizza();
            }

            public abstract void AddSauce();
            public abstract void AddToppings();

            public Pizza ServePizza()
            {
                return this.Pizza;
            }
        }

        private class PepperoniPizzaRecipe : BasePizzaRecipe
        {
            public override void AddSauce()
            {
                this.Pizza?.SetSauce(Sauce.Tomato);
            }

            public override void AddToppings()
            {
                this.Pizza?.SetToppings(Topping.Pepperoni | Topping.Cheese);
            }
        }

        private class SpecialPizzaRecipe : BasePizzaRecipe
        {
            public override void AddSauce()
            {
                this.Pizza?.SetSauce(Sauce.Spicy);
            }

            public override void AddToppings()
            {
                this.Pizza?.SetToppings(Topping.Bacon | Topping.Cheese | Topping.Onion | Topping.Sausage);
            }
        }

        #endregion
    }
}
