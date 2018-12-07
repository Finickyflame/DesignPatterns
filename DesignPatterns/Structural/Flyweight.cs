using System;
using System.Collections.Generic;
using Xunit;

namespace DesignPatterns.Structural
{
    public class Flyweight : DesignPattern
    {
        public override void Execute()
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
        private class AliasGenerator
        {
            private readonly Dictionary<string, Alias> _aliases;

            public AliasGenerator()
            {
                this._aliases = new Dictionary<string, Alias>();
            }

            public Alias GetAlias(string name)
            {
                if (!this._aliases.TryGetValue(name, out Alias alias))
                {
                    this._aliases.Add(name, alias = new Alias(Guid.NewGuid()));
                }
                return alias;
            }
        }

        /* FlyWeight */
        private class Alias : IEquatable<Alias>
        {
            public Alias(Guid id)
            {
                this.Id = id;
            }

            private Guid Id { get; }

            public bool Equals(Alias other)
            {
                if (ReferenceEquals(null, other))
                {
                    return false;
                }

                if (ReferenceEquals(this, other))
                {
                    return true;
                }

                return this.Id.Equals(other.Id);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj))
                {
                    return false;
                }

                if (ReferenceEquals(this, obj))
                {
                    return true;
                }

                if (obj.GetType() != this.GetType())
                {
                    return false;
                }

                return this.Equals((Alias) obj);
            }

            public override int GetHashCode()
            {
                return this.Id.GetHashCode();
            }
        }
    }
}
