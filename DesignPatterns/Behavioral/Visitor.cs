using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DesignPatterns.Behavioral
{
    public class Visitor : DesignPattern
    {
        public override void Execute()
        {
            var salad = new Salad();
            var sandwich = new Sandwich();

            salad.Accept<Tomato>();
            salad.Accept<Lettuce>();
            Assert.True(salad.Ingredients.Count == 2);
            Assert.True((salad.Ingredients.FirstOrDefault(i => i is Tomato) as Tomato)?.ShouldBeChopped);
            Assert.True((salad.Ingredients.FirstOrDefault(i => i is Lettuce) as Lettuce)?.ShouldHaveDressing);

            sandwich.Accept<Tomato>();
            sandwich.Accept<Lettuce>();
            Assert.True(sandwich.Ingredients.Count == 2);
            Assert.True((sandwich.Ingredients.FirstOrDefault(i => i is Tomato) as Tomato)?.ShouldBeSliced);
            Assert.False((sandwich.Ingredients.FirstOrDefault(i => i is Lettuce) as Lettuce)?.ShouldHaveDressing);
        }
    }

    #region Visitors
    public interface IIngredientVisitor
    {
        void Visit(Salad salad);
        void Visit(Sandwich sandwich);
    }

    public class Tomato : IIngredientVisitor
    {
        public bool ShouldBeChopped { get; private set; }

        public bool ShouldBeSliced { get; private set; }

        public void Visit(Salad salad)
        {
            ShouldBeChopped = true;
            ShouldBeSliced = false;
            AddToFood(salad);
        }

        public void Visit(Sandwich sandwich)
        {
            ShouldBeChopped = false;
            ShouldBeSliced = true;
            AddToFood(sandwich);
        }

        private void AddToFood(IFoodVisitable foodVisitable)
        {
            foodVisitable.Ingredients.Add(this);
        }
    }

    public class Lettuce : IIngredientVisitor
    {
        public bool ShouldHaveDressing { get; private set; }

        public void Visit(Salad salad)
        {
            ShouldHaveDressing = true;
            AddToFood(salad);
        }

        public void Visit(Sandwich sandwich)
        {
            ShouldHaveDressing = false;
            AddToFood(sandwich);
        }

        private void AddToFood(IFoodVisitable foodVisitable)
        {
            foodVisitable.Ingredients.Add(this);
        }
    }

    #endregion

    #region Visitables
    public class Salad : IFoodVisitable
    {
        public ICollection<IIngredientVisitor> Ingredients { get; } = new List<IIngredientVisitor>();

        public void Accept<TIngredientVisitor>() where TIngredientVisitor : IIngredientVisitor, new()
        {
            var ingredient = new TIngredientVisitor();
            ingredient.Visit(this);
        }
    }

    public class Sandwich : IFoodVisitable
    {
        public ICollection<IIngredientVisitor> Ingredients { get; } = new List<IIngredientVisitor>();

        public void Accept<TIngredientVisitor>() where TIngredientVisitor : IIngredientVisitor, new()
        {
            var ingredient = new TIngredientVisitor();
            ingredient.Visit(this);
        }
    }

    public interface IFoodVisitable
    {
        ICollection<IIngredientVisitor> Ingredients { get; }

        void Accept<TIngredientVisitor>() where TIngredientVisitor : IIngredientVisitor, new();
    }
    #endregion
}