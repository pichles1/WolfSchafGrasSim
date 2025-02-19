using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using SBB.BusinessLogic;
using SBB.Core;

namespace SBB.Simulator
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

            previousTextSheep = txtSheep.Text;
            previousTextWolf = txtWolf.Text;
            previousTextGrass = txtGrass.Text;

            speedSlider.ValueChanged += SpeedSlider_ValueChanged;
            grassGrowRateSlider.ValueChanged += GrassGrowRateSlider_ValueChanged;
            rangeSlider.ValueChanged += RangeSlider_ValueChanged;

            _simulation.SetGrowRate(Convert.ToInt32(grassGrowRateSlider.Value));
            _simulation.SetRange(rangeSlider.Value);

            _checkEmptyTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(0.5)
            };
            _checkEmptyTimer.Tick += CheckEmptyTimer_Tick;

            _delayTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(0.5)
            };
            _delayTimer.Tick += DelayTimer_Tick;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(speedSlider.Value);
            _timer.Tick += Timer_Tick;
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

        private void Timer_Tick(object? sender, EventArgs e)
        {
            DurSimulation();
            ChartPopCounter();
        }

        private void SimCanvasSize()
        {
            _simulation.CanvasHeight = SimCanvas.ActualHeight;
            _simulation.CanvasWidth = SimCanvas.ActualWidth;
        }

        private void ChartPopCounter()
        {
            _liveChartWindow.SheepCount = _simulation.AnimalList.OfType<Sheep>().Count();
            _liveChartWindow.WolfCount = _simulation.AnimalList.OfType<Wolf>().Count();
            _liveChartWindow.GrassCount = _simulation.GrassList.Count();
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
                    MessageBox.Show("Not all Values are given!", "VALUE IS NULL!", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                StopSimulation();
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

        private void StopSimulation()
        {
            _timer.Stop();
            _checkEmptyTimer.Stop();
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
                _timer.Start();
                _liveChartWindow.ContSimChart();
            }
        }

        private void btnRestartSim_Click(object sender, RoutedEventArgs e)
        {
            if (txtSheep.Text == "" || txtWolf.Text == "" || txtGrass.Text == "")
            {
                MessageBox.Show("Not all Values are given!", "VALUE IS NULL!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            else
            {
                SimCanvasSize();
                GetInput();
                StopSimulation();
                _liveChartWindow.StopSimChart();
                SimCanvas.Children.Clear();
                _simulation.Cleanup();
                _simulation.StartSim();
                DrawWorld();
                _timer.Start();
                _liveChartWindow.StartSimChart();
                _running = true;
                btnStartStopSim.Content = "Stop Simulation";

                _checkEmptyTimer.Start();
            }
        }

        /*private void DrawAnimalOnCanvas(Ellipse ellipse)
        {
            SimCanvas.Children.Add(ellipse);
        }

        private void DrawGrasOnCanvas(Rectangle rect)
        {
            SimCanvas.Children.Add(rect);
        }
        */

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

        private void DurSimulation()
        {
            ClearCanvas();
            ContHandlerGrass();
            ContHandlerAnimal();
        }

        private void StartHandlerGrass()
        {
            foreach (var grass in _simulation.GrassList)
            {
                _simulation.SetGrassXY(grass);
            }

            foreach (var grass in _simulation.GrassList)
            {
                DrawGrass(grass);
            }
        }

        private void ContHandlerGrass()
        {
            for (int i = 0; i <= _simulation.GrowRate; i++)
            {
                _simulation.GrowGrass();
                foreach (var grass in _simulation.GrassList)
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

                Canvas.SetLeft(rect, grass.X);
                Canvas.SetTop(rect, grass.Y);
                SimCanvas.Children.Add(rect);
        }

        private void StartHandlerAnimal()
        {
            foreach (var animal in _simulation.AnimalList)
            {
                _simulation.SetAnimalXY(animal);
                if (animal.GetType() == typeof(Sheep))
                {
                    DrawAnimal(animal, Brushes.White);
                }

                if (animal.GetType() == typeof(Wolf))
                {
                    DrawAnimal(animal, Brushes.Gray);
                }
            }
        }

        private void ContHandlerAnimal()
        {
            AnimalAct();
        }

        //Animal
        private void AnimalAct()
        {
            foreach (var animal in _simulation.AnimalList)
            {
                _simulation.SetNewAnimalXY(animal);
                MoveAnimal(animal);
            }
            _simulation.EatAnimal();
            _simulation.HPHandlerAnimal();
        }

        private void DrawAnimal(Animal animal, Brush color)
        {
            Ellipse rangeIndi = new Ellipse()
            {
                Width = _simulation.Range,
                Height = _simulation.Range,
                Stroke = Brushes.Black
            };

            Ellipse ellipse = new Ellipse()
            {
                Width = 10,
                Height = 10,
                Fill = color
            };

            Canvas.SetLeft(ellipse, animal.X);
            Canvas.SetTop(ellipse, animal.Y);
            Canvas.SetLeft(rangeIndi, animal.X - (0.5 * _simulation.Range - 5));
            Canvas.SetTop(rangeIndi, animal.Y - (0.5 * _simulation.Range - 5));
            SimCanvas.Children.Add(ellipse);
            SimCanvas.Children.Add(rangeIndi);
        }

        private void MoveAnimal(Animal animal)
        {
            Brush color = animal.GetType() == typeof(Wolf) ? Brushes.Gray : Brushes.White;

            Ellipse rangeIndi = new Ellipse()
            {
                Width = _simulation.Range,
                Height = _simulation.Range,
                Stroke = Brushes.Black
            };

            Ellipse ellipse = new Ellipse()
            {
                Width = 10,
                Height = 10,
                Fill = color
            };

            Canvas.SetLeft(ellipse, animal.X);
            Canvas.SetTop(ellipse, animal.Y);
            Canvas.SetLeft(rangeIndi, animal.X - (0.5 * _simulation.Range - 5));
            Canvas.SetTop(rangeIndi, animal.Y - (0.5 * _simulation.Range - 5));
            SimCanvas.Children.Add(ellipse);
            SimCanvas.Children.Add(rangeIndi);
        }

        private void DelayTimer_Tick(object? sender, EventArgs e)
        {
            _delayTimer.Stop();

            if (_delayedStopRequested && _running)
            {
                StopSimulation();
                _liveChartWindow.StopSimChart();
                if (_simulation.AnimalList.OfType<Wolf>().Count() == 0 && !_delayTimer.IsEnabled)
                {
                    MessageBox.Show("All Wolfes are Dead!" + Environment.NewLine + "Restart Simulation.", "INFO!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (_simulation.AnimalList.OfType<Sheep>().Count() == 0 && !_delayTimer.IsEnabled)
                {
                    MessageBox.Show("All Sheeps are Dead!" + Environment.NewLine + "Restart Simulation.", "INFO!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                SimCanvas.Children.Clear();
                _simulation.Cleanup();
                _running = false;
                btnStartStopSim.Content = "Start Simulation";
            }
            _delayedStopRequested = false;
        }

        private void CheckEmptyTimer_Tick(object? sender, EventArgs e)
        {
            if (_simulation.AnimalList.OfType<Wolf>().Count() == 0 && !_delayTimer.IsEnabled)
            {
                if (_delayedStopRequested == false)
                {
                    _delayedStopRequested = true;
                    _delayTimer.Start();
                }
            }
            else if (_simulation.AnimalList.OfType<Sheep>().Count() == 0 && !_delayTimer.IsEnabled)
            {
                if (_delayedStopRequested == false)
                {
                    _delayedStopRequested = true;
                    _delayTimer.Start();
                }
            }
            else if(_simulation.AnimalList.Count > 0 && _delayTimer.IsEnabled)
            {
                _delayTimer.Stop();
                _delayedStopRequested = false;
            }
        }

        //Slider Handlers
        private void SpeedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SetSimSpeed(speedSlider.Value);
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
        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if(_liveChartWindow != null)
            {
                _liveChartWindow.Close();
            }

            Application.Current.Shutdown();
        }
    }
}