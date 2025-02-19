using LiveCharts;
using System.Collections.ObjectModel;
using System.Timers;
using System.Windows;

namespace SBB.Simulator
{
    public partial class LiveChartPopulation : Window
    {
        private System.Timers.Timer _updateTimer;

        private ChartValues<int> SheepPopulation { get; set; }

        private ChartValues<int> WolfPopulation { get; set; }

        private ChartValues<int> GrassPopulation { get; set; }

        public int SheepCount { get; set; }

        public int WolfCount { get; set; }

        public int GrassCount { get; set; }

        private const int MaxTicks = 20;

        private List<string> TimeLabels { get; set; }

        private int _timeCounter = 0;

        public LiveChartPopulation()
        {
            InitializeComponent();

            SheepPopulation = new ChartValues<int>();
            WolfPopulation = new ChartValues<int>();
            GrassPopulation = new ChartValues<int>();
            TimeLabels = new List<string>();

            DataContext = this;

            _updateTimer = new System.Timers.Timer(500);
            _updateTimer.Elapsed += UpdatePopulationData;
        }

        private void UpdatePopulationData(object? sender, ElapsedEventArgs e)
        {
            int currentSheepPopulation = SheepCount;
            int currentWolfPopulation = WolfCount;
            int currentGrassPopulation = GrassCount;

            Application.Current.Dispatcher.Invoke(() =>
            {
                SheepPopulation.Add(currentSheepPopulation);
                WolfPopulation.Add(currentWolfPopulation);
                GrassPopulation.Add(currentGrassPopulation);

                _timeCounter++;
                TimeLabels.Add(_timeCounter.ToString());

                if (SheepPopulation.Count > MaxTicks)
                {
                    SheepPopulation.RemoveAt(0);
                    WolfPopulation.RemoveAt(0);
                    GrassPopulation.RemoveAt(0);
                    TimeLabels.RemoveAt(0);
                }

                populationChart.AxisX[0].MinValue = 0;
                populationChart.AxisX[0].MaxValue = MaxTicks - 1;

                populationChart.AxisY[0].MaxValue = Math.Max(50, new[] { currentGrassPopulation, currentSheepPopulation, currentWolfPopulation }.Max());

                populationChart.Series[0].Values = SheepPopulation;
                populationChart.Series[1].Values = WolfPopulation;
                populationChart.Series[2].Values = GrassPopulation;

                populationChart.AxisX[0].Labels = TimeLabels;
            });
        }
        
        public void ChartCleanup()
        {
            SheepPopulation.Clear();
            WolfPopulation.Clear();
            GrassPopulation.Clear();
            TimeLabels.Clear();
            _timeCounter = 0;
        }

        public void StartSimChart()
        {
            ChartCleanup();

            if (!_updateTimer.Enabled)
            {
                _updateTimer.Start();
            }
        }

        public void StopSimChart()
        {
            if (_updateTimer.Enabled)
            {
                _updateTimer.Stop();
            }
        }

        public void ContSimChart()
        {
            if (!_updateTimer.Enabled)
            {
                _updateTimer.Start();
            }
        }
    }
}
