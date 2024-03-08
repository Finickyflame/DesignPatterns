namespace DesignPatterns.Structural;

public class Flyweight
{
    [Fact]
    public void Execute()
    {
        var aliasGenerator = new AliasGenerator();

        Alias firstAlias = aliasGenerator.GetAlias("John Doe");
        Assert.NotNull(firstAlias);

        Alias sameAlias = aliasGenerator.GetAlias("John Doe");
        Assert.Equal(firstAlias, sameAlias);

        Alias anotherAlias = aliasGenerator.GetAlias("Jane Doe");
        Assert.NotEqual(firstAlias, anotherAlias);
    }

    /* Factory */
    public class AliasGenerator
    {
        private readonly Dictionary<string, Alias> _aliases = [];

        public Alias GetAlias(string name)
        {
            if (!_aliases.TryGetValue(name, out Alias alias))
            {
                _aliases.Add(name, alias = new Alias(Guid.NewGuid()));
            }
            return alias;
        }
    }

    /* FlyWeight */
    public record Alias(Guid Id);
}