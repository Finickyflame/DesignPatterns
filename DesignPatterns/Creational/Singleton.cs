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

        private class Government
        {
            private Government()
            {
            }

            private static Government _instance;

            public static Government Instance => _instance ?? (_instance = new Government());
        }
    }
}
