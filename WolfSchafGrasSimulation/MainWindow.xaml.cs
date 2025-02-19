using System;
using System.Linq;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WolfSchafGrasSimulation
{
    public partial class MainWindow : Window
    {
        private readonly Simulation _simulation;

        private DispatcherTimer _timer;

        private bool _running = false;

        private string previousTextSheep;

        private string previousTextWolf;

        private string previousTextGrass;

        private LiveChartPopulation _liveChartWindow;

        private DispatcherTimer _checkEmptyTimer;

        private DispatcherTimer _delayTimer;

        private bool _delayedStopRequested = false;

        public double SimSpeed { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            _liveChartWindow = new LiveChartPopulation();
            _liveChartWindow.Show();

            this.Closing += MainWindow_Closing;

            txtSheep.Text = "10";
            txtWolf.Text = "10";
            txtGrass.Text = "20";

            _simulation = new Simulation();
            _simulation.OnDrawAnimal = DrawAnimalOnCanvas;
            _simulation.OnDrawGras = DrawGrasOnCanvas;
            _simulation.ClearCanvas = ClearCanvas;

            previousTextSheep = txtSheep.Text;
            previousTextWolf = txtWolf.Text;
            previousTextGrass = txtGrass.Text;

            speedSlider.ValueChanged += SpeedSlider_ValueChanged;
            grassGrowRateSlider.ValueChanged += GrassGrowRateSlider_ValueChanged;
            rangeSlider.ValueChanged += RangeSlider_ValueChanged;

            _simulation.SetSimSpeed(speedSlider.Value);
            _simulation.SetGrowRate(Convert.ToInt32(grassGrowRateSlider.Value));
            _simulation.SetRange(rangeSlider.Value);

            _checkEmptyTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _checkEmptyTimer.Tick += CheckEmptyTimer_Tick;

            _delayTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _delayTimer.Tick += DelayTimer_Tick;

            _timer.Tick += Timer_Tick;
            _timer.Interval = TimeSpan.FromMilliseconds(500);
            _timer = new DispatcherTimer();
            UpdateTimerInterval();

        }
        public void SetSimSpeed(double speed)
        {
            SimSpeed = speed;
            UpdateTimerInterval();
        }

        private void UpdateTimerInterval()
        {
            _timer.Interval = TimeSpan.FromMilliseconds(SimSpeed);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _simulation.DuringSim;
        }

        private void SimCanvasSize()
        {
            _simulation.CanvasHeight = SimCanvas.ActualHeight;
            _simulation.CanvasWidth = SimCanvas.ActualWidth;
        }

        //Button Handlers
        private void btnStartStopSim_Click(object sender, RoutedEventArgs e)
        {
            btnStartStopSim.Content = _running ? "Start Simulation" : "Stop Simulation";

            SimCanvasSize();

            GetInput();

            if (_running == false)
            {
                if (txtSheep.Text == "" || txtWolf.Text == "" || txtGrass.Text == "")
                {
                    MessageBox.Show("Not all Values are given!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    _running = !_running;
                    btnStartStopSim.Content = _running ? "Start Simulation" : "Stop Simulation";
                }
                else
                {
                    StartSimulation();
                }
            }
            else
            {
                _simulation.StopSim();
                _timer.Stop();
                _liveChartWindow.StopSimChart();
                _checkEmptyTimer.Stop();
            }

            _running = !_running;
        }

        private void StartSimulation()
        {
            if (SimCanvas.Children.Count == 0)
            {
                SimCanvas.Children.Clear();
                _simulation.StartSim();
                DrawWorld();
                _timer.Start();
                _liveChartWindow.StartSimChart();
            }
            else
            {
                ResumeSimulation();
            }
            previousTextSheep = txtSheep.Text;
            previousTextWolf = txtWolf.Text;
            previousTextGrass = txtGrass.Text;

            _checkEmptyTimer.Start();
        }

        private void ResumeSimulation()
        {
            if (txtSheep.Text != previousTextSheep || txtWolf.Text != previousTextWolf || txtGrass.Text != previousTextGrass)
            {
                SimCanvas.Children.Clear();
                _simulation.StartSim();
                DrawWorld();
                _timer.Start();
                _liveChartWindow.StartSimChart();
            }
            else
            {
                _simulation.ContSim();
                _timer.Start();
                _liveChartWindow.ContSimChart();
            }
        }

        private void btnRestartSim_Click(object sender, RoutedEventArgs e)
        {
            if (txtSheep.Text == "" || txtWolf.Text == "" || txtGrass.Text == "")
            {
                MessageBox.Show("Not all Values are given!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else
            {
                SimCanvasSize();
                GetInput();
                _simulation.StopSim();
                _liveChartWindow.StopSimChart();
                SimCanvas.Children.Clear();
                _simulation.StartSim();
                DrawWorld();
                _timer.Start();
                _liveChartWindow.StartSimChart();
                _running = true;
                btnStartStopSim.Content = "Stop Simulation";

                _checkEmptyTimer.Start();
            }
        }

        private void DrawAnimalOnCanvas(Ellipse ellipse)
        {
            SimCanvas.Children.Add(ellipse);
        }

        private void DrawGrasOnCanvas(Rectangle rect)
        {
            SimCanvas.Children.Add(rect);
        }

        private void ClearCanvas()
        {
            SimCanvas.Children.Clear();
        }

        private void GetInput()
        {
            if (int.TryParse(txtSheep.Text, out int input))
            {
                _simulation.InputSheep = input;
            }
            if (int.TryParse(txtWolf.Text, out input))
            {
                _simulation.InputWolf = input;
            }
            if (int.TryParse(txtGrass.Text, out input))
            {
                _simulation.InputGrass = input;
            }
        }

        private void DrawWorld()
        {
            StartHandlerGrass();
            StartHandlerAnimal();
        }

        public void DurSim()
        {
            ClearCanvas();
            ContHandlerGrass();
            ContHandlerAnimal();
        }

        private void StartHandlerGrass()
        {
            foreach (var grass in GrassList)
            {
                SetGrassXY(grass);
            }

            foreach (var grass in GrassList)
            {
                DrawGrass(grass);
            }
        }

        private void ContHandlerGrass()
        {
            for (int i = 0; i <= GrowRate; i++)
            {
                GrowGrass();
                foreach (var grass in GrassList)
                {
                    DrawGrass(grass);
                }
            }
        }

        private void DrawGrass(Grass grass)
        {
                Rectangle rect = new Rectangle()
                {
                    Width = 5,
                    Height = 5,
                    Fill = Brushes.Green
                };

                SimCanvas.SetLeft(rect, grass.X);
                SimCanvas.SetTop(rect, grass.Y);
                SimCanvas.Children.Add(rect);
        }

        private void StartHandlerAnimal()
        {
            foreach (var animal in AnimalList)
            {
                if (animal.GetType() == typeof(Sheep))
                {
                    DrawAnimal(animal, Brushes.White, _random);
                }

                if (animal.GetType() == typeof(Wolf))
                {
                    DrawAnimal(animal, Brushes.Gray, _random);
                }
            }
        }

        private void ContHandlerAnimal()
        {
            AnimalAct();
            HPHandlerAnimal();
        }

        //Animal
        private void AnimalAct()
        {
            foreach (var animal in AnimalList)
            {
                MoveAnimal(animal, _random);
            }
            EatAnimal();
        }

        private void DrawAnimal(Animal animal, Brush color, Random random)
        {
            Ellipse rangeIndi = new Ellipse()
            {
                Width = Range,
                Height = Range,
                Stroke = Brushes.Black
            };

            Ellipse ellipse = new Ellipse()
            {
                Width = 10,
                Height = 10,
                Fill = color
            };

            animal.X = random.NextDouble() * CanvasWidth;
            animal.Y = random.NextDouble() * CanvasHeight;

            SimCanvas.SetLeft(ellipse, animal.X);
            SimCanvas.SetTop(ellipse, animal.Y);
            SimCanvas.SetLeft(rangeIndi, animal.X - (0.5 * Range - 5));
            SimCanvas.SetTop(rangeIndi, animal.Y - (0.5 * Range - 5));
            SimCanvas.Children.Add(ellipse);
            SimCanvas.Children.Add(rangeIndi);
        }

        private void MoveAnimal(Animal animal, Random random)
        {
            Brush color = animal.GetType() == typeof(Wolf) ? Brushes.Gray : Brushes.White;

            Ellipse rangeIndi = new Ellipse()
            {
                Width = Range,
                Height = Range,
                Stroke = Brushes.Black
            };

            Ellipse ellipse = new Ellipse()
            {
                Width = 10,
                Height = 10,
                Fill = color
            };

            double newX = animal.X + random.Next(-30, 30);
            double newY = animal.Y + random.Next(-30, 30);

            if (newX < 0) newX = 0;
            if (newY < 0) newY = 0;

            animal.X = newX;
            animal.Y = newY;

            SimCanvas.SetLeft(ellipse, animal.X);
            SimCanvas.SetTop(ellipse, animal.Y);
            SimCanvas.SetLeft(rangeIndi, animal.X - (0.5 * Range - 5));
            SimCanvas.SetTop(rangeIndi, animal.Y - (0.5 * Range - 5));
            SimCanvas.Children.Add(ellipse);
            SimCanvas.Children.Add(rangeIndi);
        }

        private void DelayTimer_Tick(object sender, EventArgs e)
        {
            _delayTimer.Stop();

            if (_delayedStopRequested && _running)
            {
                _simulation.StopSim();
                _liveChartWindow.StopSimChart();
                SimCanvas.Children.Clear();
                _running = false;
                btnStartStopSim.Content = "Start Simulation";
            }
            _delayedStopRequested = false;
        }

        private void CheckEmptyTimer_Tick(object sender, EventArgs e)
        {
            if (Simulation.AnimalList.Count == 0 && !_delayTimer.IsEnabled)
            {
                if (!_delayedStopRequested)
                {
                    _delayedStopRequested = true;
                    _delayTimer.Start();
                }
            }
            else if(Simulation.AnimalList.Count > 0 && _delayTimer.IsEnabled)
            {
                _delayTimer.Stop();
                _delayedStopRequested = false;
            }
        }

        //Slider Handlers
        private void SpeedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _simulation.SetSimSpeed(speedSlider.Value);
        }

        private void GrassGrowRateSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _simulation.SetGrowRate(Convert.ToInt32(grassGrowRateSlider.Value));
        }

        private void RangeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _simulation.SetRange(rangeSlider.Value);
        }

        //Programm End
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(_liveChartWindow != null)
            {
                _liveChartWindow.Close();
            }

            Application.Current.Shutdown();
        }
    }
}