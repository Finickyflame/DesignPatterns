namespace DesignPatterns.Behavioral;

public class Interpreter
{
    /// <summary>
    /// Client
    /// </summary>
    /// <remarks>
    /// Build a syntax tree representing a particular sentence in the language that the grammar defines.
    /// </remarks>
    [Fact]
    public void Execute()
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

        Assert.Equal( /* expression1 */1 + 2 - /* expression3 */4.0 * 6.0 / 2.0, expression4.Solve(context));


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

        Assert.Equal(5 * x / 2 + (Math.Pow(2, x) - 6), expression5.Solve(context));
    }

    /// <summary>
    /// Context
    /// </summary>
    /// <remarks>
    /// Contains information that is global to the interpreter.
    /// </remarks>
    public class Context(params KeyValuePair<string, Expression>[] variables)
    {
        private readonly Dictionary<string, Expression> _variables = new(variables);

        public void SetVariable(string variableName, Expression value)
        {
            _variables[variableName] = value;
        }

        public Expression GetVariable(string variableName)
        {
            return _variables[variableName];
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

    /// <summary>
    /// Non terminal expression
    /// </summary>
    public class AddExpression(Expression leftExpression, Expression rightExpression) : Expression
    {
        public override double Solve(Context context) => leftExpression.Solve(context) + rightExpression.Solve(context);
    }


    /// <summary>
    /// Terminal expression
    /// </summary>
    public class ConstantExpression(double number) : Expression
    {
        public override double Solve(Context context) => number;
    }


    /// <summary>
    /// Non terminal expression
    /// </summary>
    public class DivideExpression(Expression leftExpression, Expression rightExpression) : Expression
    {
        public override double Solve(Context context) => leftExpression.Solve(context) / rightExpression.Solve(context);
    }

    /// <summary>
    /// Non terminal expression
    /// </summary>
    public class MultiplyExpression(Expression leftExpression, Expression rightExpression) : Expression
    {
        public override double Solve(Context context) => leftExpression.Solve(context) * rightExpression.Solve(context);
    }

    /// <summary>
    /// Non terminal expression
    /// </summary>
    public class PowerExpression(Expression leftExpression, Expression rightExpression) : Expression
    {
        public override double Solve(Context context) => Math.Pow(leftExpression.Solve(context), rightExpression.Solve(context));
    }

    /// <summary>
    /// Non terminal expression
    /// </summary>
    public class SubtractExpression(Expression leftExpression, Expression rightExpression) : Expression
    {
        public override double Solve(Context context) => leftExpression.Solve(context) - rightExpression.Solve(context);
    }

    /// <summary>
    /// Non terminal expression
    /// </summary>
    public class VariableExpression(string variableName) : Expression
    {
        public override double Solve(Context context) => context.GetVariable(variableName).Solve(context);
    }
}