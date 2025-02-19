using LiveCharts;
using LiveCharts.Definitions.Charts;
using LiveCharts.Wpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WolfSchafGrasSimulation.Class;

namespace WolfSchafGrasSimulation
{
    public partial class LiveChartPopulation : Window
    {
        private System.Timers.Timer _updateTimer;

        private ChartValues<int> SheepPopulation { get; set; }

        private ChartValues<int> WolfPopulation { get; set; }

        private ChartValues<int> GrasPopulation { get; set; }

        private List<string> TimeLabels { get; set; }

        private int _timeCounter = 0;

        public LiveChartPopulation()
        {
            InitializeComponent();

            SheepPopulation = new ChartValues<int>();
            WolfPopulation = new ChartValues<int>();
            GrasPopulation = new ChartValues<int>();
            TimeLabels = new List<string>();

            DataContext = this;

            _updateTimer = new System.Timers.Timer(500);
            _updateTimer.Elapsed += UpdatePopulationData;
        }

        private void UpdatePopulationData(object sender, ElapsedEventArgs e)
        {
            int currentSheepPopulation = Simulation.AnimalList.OfType<Sheep>().Count();
            int currentWolfPopulation = Simulation.AnimalList.OfType<Wolf>().Count();
            int currentGrasPopulation = Simulation.GrassList.Count();

            Application.Current.Dispatcher.Invoke(() =>
            {
                SheepPopulation.Add(currentSheepPopulation);
                WolfPopulation.Add(currentWolfPopulation);
                GrasPopulation.Add(currentGrasPopulation);

                _timeCounter++;
                TimeLabels.Add(_timeCounter.ToString());

                populationChart.AxisX[0].MaxValue = _timeCounter;
                populationChart.AxisY[0].MaxValue = Math.Max(50, new[] { currentGrasPopulation, currentSheepPopulation, currentWolfPopulation }.Max());

                populationChart.Series[0].Values = SheepPopulation;
                populationChart.Series[1].Values = WolfPopulation;
                populationChart.Series[2].Values = GrasPopulation;

                populationChart.AxisX[0].Labels = TimeLabels;
            });
        }
        
        public void ChartCleanup()
        {
            SheepPopulation.Clear();
            WolfPopulation.Clear();
            GrasPopulation.Clear();
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
