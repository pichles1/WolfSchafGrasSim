using SBB.Core;

namespace SBB.BusinessLogic
{
    public class Simulation
    {        
        private readonly Grass _grass;

        private readonly Animal _animal;

        private static Random _random;

        public double CanvasHeight {  get; set; }

        public double CanvasWidth { get; set; }

        public int InputSheep {  get; set; }

        public int InputWolf { get; set; }

        public int InputGrass { get; set; }

        public double Range { get; set; }

        public int GrowRate { get; set; }

        public string Name { get; set; }

        public List<Animal> AnimalList { get; set; } = new List<Animal>();

        public List<Grass> GrassList { get; set; } = new List<Grass>();
        
        public Simulation()
        {
            _random = new Random();
            _grass = new Grass(Name);
            _animal = new Animal(Name);
        }

        //Simulation

        public string GetObjectCounts()
        {
            return $"Sheep: {AnimalList.Count}, Grass: {GrassList.Count}";
        }


        public void StartSim()
        {
            Cleanup();
            InitObjects();
        }

        public void Cleanup()
        {
            AnimalList.Clear();
            GrassList.Clear();
        }

        //Grass

        public void SetGrassXY(Grass grass)
        {
            double randomX = _random.NextDouble() * CanvasWidth;
            double randomY = _random.NextDouble() * CanvasHeight;

            grass.X = randomX;
            grass.Y = randomY;
        }


        public void GrowGrass()
        {
            string grassName = "Grass " + (GrassList.Count + 1);
            Grass newGrass = new Grass(grassName);
            GrassList.Add(newGrass);

            Grass newestGrass = GrassList[GrassList.Count - 1];

            double growX = _random.NextDouble() * CanvasWidth;
            double growY = _random.NextDouble() * CanvasHeight;

            newestGrass.X = growX;
            newestGrass.Y = growY;
        }

        //Animal

        public void SetAnimalXY(Animal animal)
        {
            double randomX = _random.NextDouble() * CanvasWidth;
            double randomY = _random.NextDouble() * CanvasHeight;

            animal.X = randomX;
            animal.Y = randomY;
        }

        public void SetNewAnimalXY(Animal animal)
        {
            double newX = animal.X + _random.Next(-30, 30);
            double newY = animal.Y + _random.Next(-30, 30);

            if (newX < 0) newX = 0;
            if (newY < 0) newY = 0;

            animal.X = newX;
            animal.Y = newY;
        }

        private bool InRangeAnimal(Animal animal, double x, double y, double range)
        {
            double deltaX = animal.X - x;
            double deltaY = animal.Y - y;
            double distanceSquared = deltaX * deltaX + deltaY * deltaY;
            return distanceSquared <= range * range;
        }

        public void EatAnimal()
        {
            double rangeSheep = 20;
            double rangeWolf = 20;

            List<Animal> deleteSheep = new List<Animal>();
            List<Grass> deleteGras = new List<Grass>();
            List<Animal> newAnimal = new List<Animal>();

            foreach (var wolf in AnimalList.OfType<Wolf>())
            {
                foreach (var sheep in AnimalList.OfType<Sheep>())
                {
                    if (InRangeAnimal(wolf, sheep.X, sheep.Y, rangeWolf))
                    {
                        Console.WriteLine("Wolf ate Sheep!");
                        deleteSheep.Add(sheep);

                        wolf.Leben = 100;

                        string wolfName = "Sheep " + (AnimalList.Count + 1);
                        newAnimal.Add(new Wolf(wolfName) { X = wolf.X, Y = wolf.Y });
                        break;
                    }
                }
            }

            foreach (var sheep in AnimalList.OfType<Sheep>())
            {
                foreach (var grass in GrassList)
                {
                    if (InRangeAnimal(sheep, grass.X, grass.Y, rangeSheep))
                    {
                        Console.WriteLine("Sheep ate Grass!");
                        deleteGras.Add(grass);

                        sheep.Leben = 100;

                        string sheepName = "Sheep " + (AnimalList.Count + 1);
                        newAnimal.Add(new Sheep(sheepName) { X = sheep.X, Y = sheep.Y });
                        break;
                    }
                }
            }

            foreach (var sheep in deleteSheep)
            {
                AnimalList.Remove(sheep);
            }

            foreach (var grass in deleteGras)
            {
                GrassList.Remove(grass);
            }

            foreach (var animal in newAnimal)
            {
                AnimalList.Add(animal);
            }
        }

        public void HPHandlerAnimal()
        {
            List<Animal> deadAnimals = new List<Animal>();

            foreach (var animal in AnimalList)
            {
                if (animal is Sheep)
                {
                    animal.Leben -= 5;
                }

                else if (animal is Wolf)
                {
                    animal.Leben -= 10;
                }

                if (!animal.IsAlive)
                {
                    Console.WriteLine($"{animal.GetType().Name} died.");
                    deadAnimals.Add(animal);
                }
            }

            foreach (var deadAnimal in deadAnimals)
            {
                AnimalList.Remove(deadAnimal);
            }
        }
        
        //List Handlers
        public void addGrass()
        {
            string grassName = "Grass " + (GrassList.Count + 1);
            Grass newGrass = new Grass(grassName);
            GrassList.Add(newGrass);
        }

        public void addSheep()
        {
            string sheepName = "Sheep " + (AnimalList.Count + 1);
            Animal newSheep = new Sheep(sheepName);
            AnimalList.Add(newSheep);
        }

        public void addWolf()
        {
            string wolfName = "Wolf " + (AnimalList.Count + 1);
            Animal newWolf = new Wolf(wolfName);
            AnimalList.Add(newWolf);
        }
        
        public void InitObjects()
        {
            for (int i = 0; i < InputSheep; i++)
            {
                addSheep();
            }

            for (int i = 0; i < InputWolf; i++)
            {
                addWolf();
            }
            
            for (int i = 0; i < InputGrass; i++)
            {
                addGrass();
            }
        }

        //Slider Handlers

        public void SetGrowRate(int rate)
        {
            GrowRate = rate;
        }

        public void SetRange(double range)
        {
            Range = range;
        }
    }
}