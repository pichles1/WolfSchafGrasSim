namespace SBB.Core
{
    public class Grass
    {
        public string Name { get; private set; }

        public Grass(string name)
        {
            this.Name = name;
        }

        public double X { get; set; }

        public double Y { get; set; }

        public bool IsEaten { get; set; }
    }
}
