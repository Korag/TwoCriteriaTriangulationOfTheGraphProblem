using euler_graph_generator.AdditionalMethods;
using euler_graph_generator.ViewModels;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace euler_graph_generator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    /* 
     W sumie to tak:
     1. naprawa czasem nie działa(graf po naprawie jest albo niespójny(to zdarza się najczęściej) albo jest spójny ale nie eulerowski)
     2. Przycisk do powtórzenia kolorowania nie blokuje się, więc można parę razy kliknąć
         */
    public partial class MainWindow : Window
    {
        private bool isConnected = false;
        private bool isEuler = false;
        private string message = "przed naprawą";
        private MainWindowViewModel vm;
        private int repairCounter = 0;
        public MainWindow()
        {
            vm = new MainWindowViewModel();
            DataContext = vm;
            WindowState = WindowState.Maximized;
            vm.worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            InitializeComponent();
        }
        private void EditEulerPathOnUI()
        {
            string eulerPathString = "";
            if (vm.EulerPath.Count >= 1)
            {
                for (int i = 0; i < vm.EulerPath.Count; i++)
                {
                    if (i != vm.EulerPath.Count - 1)
                    {
                        eulerPathString += vm.EulerPath[i] + " => ";
                    }
                    else
                    {
                        eulerPathString += vm.EulerPath[i];
                    }


                }
                EulerPath.Foreground = Brushes.Green;
                EulerPath.Visibility = Visibility.Visible;
                EulerPath.Text = eulerPathString;
            }
        }
        //to się dzieje po skończeniu naprawy
        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            EulerPath.Foreground = Brushes.Red;
            EulerPath.Visibility = Visibility.Hidden;
            EulerPath.Text = "";
            if (!vm.worker.IsBusy)
            {
                repairCounter++;
                isConnected = vm.DepthFirstSearch();
                if (isConnected)
                {
                    isEuler = vm.CheckIfEuler();
                    if (isEuler)
                    {
                        var list = vm.EulerPath;
                        bool result = true;
                        if (list.Count > 1)
                        {
                            result = list[0] != list[list.Count - 1];
                        }

                        if (result)
                        {
                            IsEuler.Content = "Półeulerowski";
                        }
                        else
                        {
                            IsEuler.Content = "Eulerowski";
                        }
                        EditEulerPathOnUI();
                        IsEuler.Foreground = Brushes.Green;
                        Napraw_graf.IsEnabled = false;
                        Euler.IsEnabled = true;
                    }
                    else
                    {
                        IsEuler.Content = "NIE";
                        IsEuler.Foreground = Brushes.Red;
                        isEuler = vm.CheckIfEuler();
                        Generuj_Click(this, null);
                        vm.worker.RunWorkerAsync();
                    }
                    IsConnected.Content = "TAK";
                    IsConnected.Foreground = Brushes.Green;
                }
                else
                {
                    IsEuler.Content = "NIE";
                    IsEuler.Foreground = Brushes.Red;
                    IsConnected.Content = "NIE";
                    IsConnected.Foreground = Brushes.Red;
                    Generuj_Click(this, null);
                    vm.worker.RunWorkerAsync();
                    //isConnected = vm.DepthFirstSearch();
                }
                Napraw_graf.IsEnabled = true;
                Generuj.IsEnabled = true;
                message = "po naprawie";
                Zapisz.IsEnabled = true;
            }
        }



        private void Generuj_Click(object sender, RoutedEventArgs e)
        {
            EulerPath.Foreground = Brushes.Red;
            EulerPath.Visibility = Visibility.Hidden;
            EulerPath.Text = "";

            IsEuler.Visibility = Visibility.Hidden;
            IsConnected.Visibility = Visibility.Hidden;
            vm.ResetData();
            vm.ReLayoutGraph();
            Reset.IsEnabled = true;
            if (vm.Graph.Vertices.Count() < 1)
            {
                Napraw_graf.IsEnabled = false;
                Euler.IsEnabled = false;
            }

            if (vm.Graph.Vertices.Count() > 1)
            {
                IsEuler.Visibility = Visibility.Visible;
                IsConnected.Visibility = Visibility.Visible;
                isConnected = vm.DepthFirstSearch();
                if (isConnected)
                {
                    IsConnected.Content = "TAK";
                    IsConnected.Foreground = Brushes.Green;
                    isEuler = vm.CheckIfEuler();
                    if (isEuler)
                    {
                        var list = vm.EulerPath;
                        bool result = true;
                        if (list.Count > 1)
                        {
                            result = list[0] != list[list.Count - 1];
                        }

                        if (result)
                        {
                            IsEuler.Content = "Półeulerowski";
                        }
                        else
                        {
                            IsEuler.Content = "Eulerowski";
                        }
                        EditEulerPathOnUI();
                        IsEuler.Foreground = Brushes.Green;
                        Napraw_graf.IsEnabled = false;
                        Euler.IsEnabled = true;
                    }
                    else
                    {
                        IsEuler.Content = "NIE";
                        IsEuler.Foreground = Brushes.Red;
                        Napraw_graf.IsEnabled = true;

                    }
                }
                else
                {
                    IsConnected.Content = "NIE";
                    IsConnected.Foreground = Brushes.Red;
                    IsEuler.Content = "NIE";
                    IsEuler.Foreground = Brushes.Red;
                    Napraw_graf.IsEnabled = true;
                }
            }
            message = "przed naprawą";
            FileSaver.firstTime = true;
            vm.SaveToFile(isConnected, IsEuler.Content.ToString(), message, true);
            FileSaver.firstTime = false;
            Zapisz.IsEnabled = true;
        }

        private void Napraw_Click(object sender, RoutedEventArgs e)
        {
            repairCounter = 0;
            Zapisz.IsEnabled = false;
            Generuj.IsEnabled = false;
            Napraw_graf.IsEnabled = false;
            vm.worker.RunWorkerAsync();

        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            EulerPath.Foreground = Brushes.Red;
            EulerPath.Visibility = Visibility.Hidden;
            EulerPath.Text = "";
            Generuj.IsEnabled = true;
            Reset.IsEnabled = false;
            Euler.IsEnabled = false;
            Zapisz.IsEnabled = false;
            Napraw_graf.IsEnabled = false;
            liczbaWierzcholkow.Value = 0;
            prawdopodobienstwo.Value = 0;

            IsEuler.Visibility = Visibility.Hidden;
            IsConnected.Visibility = Visibility.Hidden;
            vm.ResetData();
        }

        private void Zapisz_Click(object sender, RoutedEventArgs e)
        {
            Zapisz.IsEnabled = false;
            vm.SaveToFile(isConnected, IsEuler.Content.ToString(), message, false);
        }

        private void Euler_Click(object sender, RoutedEventArgs e)
        {
            if (vm.Graph.Vertices.Count() > 1)
            {
                IsEuler.Visibility = Visibility.Visible;
                IsConnected.Visibility = Visibility.Visible;
                if (vm.DepthFirstSearch())
                {
                    IsConnected.Content = "TAK";
                    IsConnected.Foreground = Brushes.Green;
                    if (vm.CheckIfEuler())
                    {
                        var list = vm.EulerPath;
                        bool result = true;
                        if (list.Count > 1)
                        {
                            result = list[0] != list[list.Count - 1];
                        }

                        if (result)
                        {
                            IsEuler.Content = "Półeulerowski";
                        }
                        else
                        {
                            IsEuler.Content = "Eulerowski";
                        }
                        EditEulerPathOnUI();
                        IsEuler.Foreground = Brushes.Green;

                    }
                    else
                    {
                        IsEuler.Content = "NIE";
                        IsEuler.Foreground = Brushes.Red;

                    }
                }
                else
                {
                    IsConnected.Content = "NIE";
                    IsConnected.Foreground = Brushes.Red;
                    IsEuler.Content = "NIE";
                    IsEuler.Foreground = Brushes.Red;
                }
            }
        }


    }
}
