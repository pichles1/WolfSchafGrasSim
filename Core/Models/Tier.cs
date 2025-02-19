namespace SBB.Core
{
    public class Animal
    {
        public string Name { get; private set; }

        public Animal(string name)
        {
            this.Name = name;
        }

        public double X { get; set; }

        public double Y { get; set; }

        public int Leben { get; set; } = 100;

        public bool IsAlive => Leben > 0;
    }
}
