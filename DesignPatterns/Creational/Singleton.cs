using Xunit;

namespace DesignPatterns.Creational
{
    public class Singleton : DesignPattern
    {
        public override void Execute()
        {
            Government instance = Government.Instance;
            Assert.Equal(Government.Instance, instance);
        }

        /* Singleton */
        private sealed class Government
        {
            private Government()
            {
                // Singleton constructor is private, it can only have one instance.
            }

            public static Government Instance { get; } = new Government();
        }
    }
}
