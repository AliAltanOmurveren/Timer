using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.IO;
using Newtonsoft.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyTimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 


    public partial class MainWindow : Window
    {
        TimeSpan time, water_time;
        System.Timers.Timer timer, water_timer;
        double width_interval = 0.0;
        double water_bar_width_interval;

        SoundPlayer normal_alarm_player = new SoundPlayer(@"C:\Users\User\Documents\Projects\C#\MyTimer\normal_alarm.wav");
        SoundPlayer water_alarm_player = new SoundPlayer(@"C:\Users\User\Documents\Projects\C#\MyTimer\water_alarm.wav");

        int num_g = 0;

        public MainWindow()
        {
            InitializeComponent();

            if(Properties.Settings.Default.Durations_file == "")
            {
                Properties.Settings.Default.Durations_file = AppDomain.CurrentDomain.BaseDirectory + "\\json_durations.json";
            }


            Dictionary<string, List<string>> durations;
                
            try
            {

                using (StreamReader s = new StreamReader(Properties.Settings.Default.Durations_file))
                {
                    string json = s.ReadToEnd();
                    durations = JsonConvert.DeserializeObject< Dictionary<string, List<string>>> (json);
                }


                foreach (string s in durations["waterDurations"])
                {
                    cb_water_durations.Items.Add(s);
                }

                foreach (string s in durations["alarmDurations"])
                {
                    cb_durations.Items.Add(s);
                }
            }
            catch (Exception e)
            {

            }
                

                num_g = RetrieveGlassCountFromSettings();
            num_glass.Text = num_g.ToString();


            time_bar.Width = 0;
            water_bar.Width = 0;

            cb_durations.SelectedIndex = 0;

            

            time = TimeSpan.Zero;

            water_time = TimeSpan.Parse(cb_water_durations.SelectedItem.ToString());
            water_time_label.Content = water_time.ToString("c");
            water_bar_width_interval = 400 / water_time.TotalSeconds;

            timer = new System.Timers.Timer(1000);
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
            time_label.Content = cb_durations.SelectedItem.ToString();

            water_timer = new System.Timers.Timer(1000);
            water_timer.Elapsed += Water_timer_Elapsed;
            timer.AutoReset = true;
            water_time_label.Content = cb_water_durations.SelectedItem.ToString();

            stop_button.Content = "Start";
            pause_button.Content = "Pause";
        }

        private void Water_timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() => {
                if (water_time.TotalSeconds < 1)
                {
                    water_time = TimeSpan.Parse(cb_water_durations.SelectedItem.ToString());
                    water_time_label.Content = water_time.ToString("c");
                    water_bar_width_interval = 400 / water_time.TotalSeconds;
                    water_bar.Width = 0;
                    water_alarm_player.Play();
                }
                else
                {
                    water_time = water_time.Add(TimeSpan.FromSeconds(-1));
                    water_time_label.Content = water_time.ToString("c");
                    water_bar.Width += water_bar_width_interval;
                }
            });

        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            

            this.Dispatcher.Invoke(() => {
                if (time.TotalSeconds < 1)
                {
                    stop_timer();
                    normal_alarm_player.Play();
                }
                else
                {
                    time = time.Add(TimeSpan.FromSeconds(-1));
                    time_label.Content = time.ToString("c");

                    time_bar.Width += width_interval;
                }
            });
            
        }

        private void title_bar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            App.Current.MainWindow.DragMove();
        }

        private void minimize_MouseEnter(object sender, MouseEventArgs e)
        {
            SolidColorBrush col = new SolidColorBrush();
            col.Color = Colors.DarkOrange;
            minimize_button.Fill = col;
        }

        private void minimize_MouseLeave(object sender, MouseEventArgs e)
        {
            SolidColorBrush col = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFC857"));
            minimize_button.Fill = col;
        }

        private void minimize_button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            App.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void close_button_MouseEnter(object sender, MouseEventArgs e)
        {
            SolidColorBrush col = new SolidColorBrush();
            col.Color = Colors.DarkRed;
            close_button.Fill = col;
        }

        private void close_button_MouseLeave(object sender, MouseEventArgs e)
        {
            SolidColorBrush col = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFDB3A34"));
            close_button.Fill = col;
        }

        private void close_button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
        
        private void cb_durations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            if((string)stop_button.Content == "Start")
                time_label.Content = cb.SelectedItem.ToString();
        }
        
        private void pause_button_Click(object sender, RoutedEventArgs e)
        {
            if (timer.Enabled)
            {
                timer.Stop();
                pause_button.Content = "Continue";
            }
            else if(!timer.Enabled && (string)stop_button.Content == "Stop")
            {
                timer.Start();
                pause_button.Content = "Pause";
            }

            
        }

        private void stop_button_Click(object sender, RoutedEventArgs e)
        {
            // timer stop
            if ((string) stop_button.Content == "Stop")
            {
                stop_timer();

            }
            else{
                // timer start
                ComboBox cb = cb_durations;
                start_timer(cb.SelectedItem.ToString());
            }
        }

        private void water_timer_event(object sender, RoutedEventArgs e)
        {
            water_timer.Enabled = !water_timer.Enabled;
        }

        private void cb_water_durations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            water_time_label.Content = cb_water_durations.SelectedItem.ToString();
            water_time = TimeSpan.Parse(cb_water_durations.SelectedItem.ToString());
            water_time_label.Content = water_time.ToString("c");
            water_bar.Width = 0;
            water_bar_width_interval = 400 / water_time.TotalSeconds;
        }

        private void Settings_button_MouseEnter(object sender, MouseEventArgs e)
        {
            SolidColorBrush col = new SolidColorBrush();
            col.Color = Colors.DarkGreen;
            settings_button.Fill = col;
        }

        private void Settings_button_MouseLeave(object sender, MouseEventArgs e)
        {
            SolidColorBrush col = (SolidColorBrush)FindResource("app_background_color");
            settings_button.Fill = col;
        }

        private void Settings_button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            json_folder jf = new json_folder();
            jf.ShowInTaskbar = false;
            jf.Owner = Application.Current.MainWindow;
            jf.Show();
        }

        private void plus_button_MouseEnter(object sender, MouseEventArgs e)
        {
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = Color.FromRgb(74,118,82);
            plus_button.Background = brush;
        }

        private void plus_button_MouseLeave(object sender, MouseEventArgs e)
        {
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = Color.FromRgb(128, 191, 140);
            plus_button.Background = brush;
        }

        private void plus_button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            num_g = int.Parse(num_glass.Text);
            num_g++;
            num_glass.Text = num_g.ToString();
            Properties.Settings.Default.Glass_count = num_g;
            Properties.Settings.Default.Save();
        }

        private void minus_button_MouseEnter(object sender, MouseEventArgs e)
        {
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = Color.FromRgb(74, 118, 82);
            minus_button.Background = brush;
        }

        private void minus_button_MouseLeave(object sender, MouseEventArgs e)
        {
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = Color.FromRgb(128, 191, 140);
            minus_button.Background = brush;
        }

        private void minus_button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            num_g = int.Parse(num_glass.Text);
            num_g = num_g <= 0 ? 0 : num_g - 1;
            num_glass.Text = num_g.ToString();
            Properties.Settings.Default.Glass_count = num_g;
            Properties.Settings.Default.Save();
        }


        private void lt_area_MouseEnter(object sender, MouseEventArgs e)
        {
            glass_image.Visibility = Visibility.Hidden;
            lt_text.Visibility = Visibility.Visible;

            float l = (num_g * Properties.Settings.Default.Glass_ml) / 1000.0f;

            num_glass.Text = l.ToString();
        }

        private void lt_area_MouseLeave(object sender, MouseEventArgs e)
        {
            glass_image.Visibility = Visibility.Visible;
            lt_text.Visibility = Visibility.Hidden;

            num_glass.Text = num_g.ToString();
        }

        private void stop_timer()
        {
            timer.Stop();
            ComboBox cb = cb_durations;
            time = TimeSpan.Parse(cb.SelectedItem.ToString());
            time_label.Content = time.ToString("c");
            stop_button.Content = "Start";
            pause_button.Content = "Continue";
            time_bar.Width = 0;
        }

        private void start_timer(string ts)
        {
            time = TimeSpan.Parse(ts);
            time_label.Content = time.ToString("c");
            timer.Start();
            width_interval = 400 / time.TotalSeconds;
            stop_button.Content = "Stop";
            pause_button.Content = "Pause";
        }

        private int RetrieveGlassCountFromSettings()
        {
            if(DateTime.Now.Day == Properties.Settings.Default.Day &&
                DateTime.Now.Month == Properties.Settings.Default.Month &&
                DateTime.Now.Year == Properties.Settings.Default.Year)
            {
                return Properties.Settings.Default.Glass_count;
            }
            else
            {
                Properties.Settings.Default.Glass_count = 0;
                Properties.Settings.Default.Day = DateTime.Now.Day;
                Properties.Settings.Default.Month = DateTime.Now.Month;
                Properties.Settings.Default.Year = DateTime.Now.Year;

                Properties.Settings.Default.Save();

                return 0;
            }
        }

    }
}
