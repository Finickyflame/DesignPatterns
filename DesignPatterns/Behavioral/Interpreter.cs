using System;
using System.Collections.Generic;
using Xunit;

namespace DesignPatterns.Behavioral
{
    public class Interpreter : DesignPattern
    {
        /// <summary>
        /// Client
        /// </summary>
        /// <remarks>
        /// Build a syntax tree representing a particular sentence in the language that the grammar defines.
        /// </remarks>
        public override void Execute()
        {
            var context = new Context();

            // expression1 = 1 + 2
            var expression1 = new AddExpression(new ConstantExpression(1), new ConstantExpression(2));
            Assert.Equal(1 + 2, expression1.Solve(context));

            // expression2 = 4 * 6
            var expression2 = new MultiplyExpression(new ConstantExpression(4), new ConstantExpression(6));
            Assert.Equal(4.0 * 6.0, expression2.Solve(context));

            // expression3 = expression2 / 2
            var expression3 = new DivideExpression(expression2, new ConstantExpression(2));
            Assert.Equal(4.0 * 6.0 / 2.0, expression3.Solve(context));

            // expression4 = expression1 - expression3
            var expression4 = new SubtractExpression(new VariableExpression(nameof(expression1)), new VariableExpression(nameof(expression3)));
            context.SetVariable(nameof(expression1), expression1);
            context.SetVariable(nameof(expression3), expression3);

            Assert.Equal((/* expression1 */ 1 + 2) - (/* expression3 */ 4.0 * 6.0 / 2.0), expression4.Solve(context));


            // f(x) = ((5 * x) / 2) + ((2 ^ x) - 6)
            // x = 6
            var expression5 = new AddExpression(
                new DivideExpression(
                    new MultiplyExpression(
                        new ConstantExpression(5),
                        new VariableExpression("x")),
                    new ConstantExpression(2)),
                new SubtractExpression(
                    new PowerExpression(
                        new ConstantExpression(2),
                        new VariableExpression("x")),
                    new ConstantExpression(6)));

            double x = 6;
            context.SetVariable("x", new ConstantExpression(x));

            Assert.Equal(((5 * x) / 2) + (Math.Pow(2, x) - 6), expression5.Solve(context));
        }

        #region Definition

        /// <summary>
        /// Context
        /// </summary>
        /// <remarks>
        /// Contains information that is global to the interpreter.
        /// </remarks>
        public class Context
        {
            private readonly Dictionary<string, Expression> _variables;

            public Context(params KeyValuePair<string, Expression>[] variables)
            {
                this._variables = new Dictionary<string, Expression>(variables);
            }

            public void SetVariable(string variableName, Expression value)
            {
                this._variables[variableName] = value;
            }

            public Expression GetVariable(string variableName)
            {
                return this._variables[variableName];
            }
        }

        /// <summary>
        /// AbstractExpression.
        /// </summary>
        /// <remarks>
        /// Declares an interface for executing an operation.
        /// </remarks>
        public abstract class Expression
        {
            public abstract double Solve(Context context);
        }

        #endregion

        #region Concrete Implementation

        /// <summary>
        /// Non terminal expression
        /// </summary>
        public class AddExpression : Expression
        {
            private readonly Expression _leftExpression;
            private readonly Expression _rightExpression;

            public AddExpression(Expression leftExpression, Expression rightExpression)
            {
                this._leftExpression = leftExpression;
                this._rightExpression = rightExpression;
            }

            public override double Solve(Context context)
            {
                return this._leftExpression.Solve(context) + this._rightExpression.Solve(context);
            }
        }


        /// <summary>
        /// Terminal expression
        /// </summary>
        public class ConstantExpression : Expression
        {
            private readonly double _number;

            public ConstantExpression(double number)
            {
                this._number = number;
            }

            public override double Solve(Context context)
            {
                return this._number;
            }
        }


        /// <summary>
        /// Non terminal expression
        /// </summary>
        public class DivideExpression : Expression
        {
            private readonly Expression _leftExpression;
            private readonly Expression _rightExpression;

            public DivideExpression(Expression leftExpression, Expression rightExpression)
            {
                this._leftExpression = leftExpression;
                this._rightExpression = rightExpression;
            }

            public override double Solve(Context context)
            {
                return this._leftExpression.Solve(context) / this._rightExpression.Solve(context);
            }
        }

        /// <summary>
        /// Non terminal expression
        /// </summary>
        public class MultiplyExpression : Expression
        {
            private readonly Expression _leftExpression;
            private readonly Expression _rightExpression;

            public MultiplyExpression(Expression leftExpression, Expression rightExpression)
            {
                this._leftExpression = leftExpression;
                this._rightExpression = rightExpression;
            }

            public override double Solve(Context context)
            {
                return this._leftExpression.Solve(context) * this._rightExpression.Solve(context);
            }
        }

        /// <summary>
        /// Non terminal expression
        /// </summary>
        public class PowerExpression : Expression
        {
            private readonly Expression _leftExpression;
            private readonly Expression _rightExpression;

            public PowerExpression(Expression leftExpression, Expression rightExpression)
            {
                this._leftExpression = leftExpression;
                this._rightExpression = rightExpression;
            }

            public override double Solve(Context context)
            {
                return Math.Pow(this._leftExpression.Solve(context), this._rightExpression.Solve(context));
            }
        }

        /// <summary>
        /// Non terminal expression
        /// </summary>
        public class SubtractExpression : Expression
        {
            private readonly Expression _leftExpression;
            private readonly Expression _rightExpression;

            public SubtractExpression(Expression leftExpression, Expression rightExpression)
            {
                this._leftExpression = leftExpression;
                this._rightExpression = rightExpression;
            }

            public override double Solve(Context context)
            {
                return this._leftExpression.Solve(context) - this._rightExpression.Solve(context);
            }
        }

        /// <summary>
        /// Non terminal expression
        /// </summary>
        public class VariableExpression : Expression
        {
            private readonly string _variableName;

            public VariableExpression(string variableName)
            {
                this._variableName = variableName;
            }

            public override double Solve(Context context)
            {
                return context.GetVariable(this._variableName).Solve(context);
            }
        }

        #endregion
    }
}
