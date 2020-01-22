using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms.Markers;
using System.Threading;
using System.Net.Sockets;
using System.IO;
using Json;
using Newtonsoft.Json;
using rtChart;

namespace OpenStreetMap_CV_Toolkit
{
    public partial class Form1 : Form
    {
        String all_data = "";
        //GMarkerGoogle marker;
        Dictionary<float, RootObject> dictionary;
        Dictionary<float, GMarkerGoogle> dictionary_marker;
        Dictionary<float, int> carId_to_chart_index_mapper;
        Dictionary<float, double> carId_speed_mapper;
        bool isConnectionClose = false;

        public double init_velocity_time = 0.0;
        public double current_time = 0.0;

        private double[] value_array = new double[30];

        long x_axis_max = 0;
        long x_axis_min = 0;
        long y_axis_max = 0;
        long y_axis_min = 0;

        String server_ip = "130.127.198.22";
        int server_port = 8989;

        kayChart kay_chart;

        public long X_RANGE = 20; 
       
        public Form1()
        {
            InitializeComponent();
            init_config();
            //kay_chart = new kayChart(chart2, 20);
            

            chart1.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorX.AutoScroll = true;

            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;


            chart1.ChartAreas[0].AxisX.Maximum = X_RANGE;
            chart1.ChartAreas[0].AxisX.Minimum = 0; 



            dictionary = new Dictionary<float, RootObject>();
            dictionary_marker = new Dictionary<float, GMarkerGoogle>();
            carId_to_chart_index_mapper = new Dictionary<float, int>();
            carId_speed_mapper = new Dictionary<float, double>();
            gmap.MapProvider = GMapProviders.GoogleChinaSatelliteMap;
            
            //listBox1.ValueMember = "Key";
            //listBox1.DisplayMember = "Value";
            carId_speed_mapper.Add(100, 24);
            //listBox1.DataSource = new BindingSource(carId_speed_mapper, null);
            

            // Jervy gym RSU locaiton : 34.678986, -82.847704
            // C1 parking lot locaiton : 34.671500, -82.830270
            // R2 parking lot : 34.675912, -82.823168

            // ADD rsu of Jervy Gym
            GMapOverlay markersOverlay = new GMapOverlay("markers");
            //PointLatLng point = new PointLatLng(34.678986, -82.847704);
            //GMapMarker marker = new GMapPoint(point, 150);
            //GMarkerGoogle marker = new GMarkerGoogle(new GMapPoint(point,10), new Bitmap(Properties.Resources.radio_station_2));

            //GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(34.678986, -82.847704), new Bitmap(Properties.Resources.radio_station_2));
            //GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(root.latitude, root.longitude), GMarkerGoogleType.green);

            //markersOverlay.Markers.Add(marker);
            // gmap.Overlays.Add(markersOverlay);
            //marker.ToolTipMode = MarkerTooltipMode.Always;
            Bitmap rsu_marker = new Bitmap(Properties.Resources.rsu_station_2);

            GMarkerGoogle j1_marker = new GMarkerGoogle(new PointLatLng(34.678986, -82.847704), rsu_marker);
            markersOverlay.Markers.Add(j1_marker);
            gmap.Overlays.Add(markersOverlay);
            //GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(root.latitude, root.longitude), GMarkerGoogleType.green);


            GMarkerGoogle c1_rsu = new GMarkerGoogle(new PointLatLng(34.671500, -82.830270), rsu_marker);
            markersOverlay.Markers.Add(c1_rsu);
            gmap.Overlays.Add(markersOverlay);
            c1_rsu.ToolTipMode = MarkerTooltipMode.Always;


            GMarkerGoogle r1_rsu = new GMarkerGoogle(new PointLatLng(34.675912, -82.823168), rsu_marker);
            markersOverlay.Markers.Add(r1_rsu);
            gmap.Overlays.Add(markersOverlay);
            r1_rsu.ToolTipMode = MarkerTooltipMode.Always;


            PointLatLng p1 = new PointLatLng(34.681877, -82.847935);
            PointLatLng p2 = new PointLatLng(34.670933, -82.835487);
            //MapRoute route = GoogleMapProvider.Instance.GetRoute(start, end, false, false, 15);
            MapRoute route = OpenStreetMapProvider.Instance.GetRoute(p1, p2, false, false, 15);

            GMapRoute r1 = new GMapRoute(route.Points, "My route");
            r1.Stroke.Width = 2;
            r1.Stroke.Color = Color.Orange;
            GMapOverlay routesOverlay1 = new GMapOverlay("routes");
            routesOverlay1.Routes.Add(r1);
            gmap.Overlays.Add(routesOverlay1);

            PointLatLng p3 = new PointLatLng(34.678001, -82.818037);
            //MapRoute route = GoogleMapProvider.Instance.GetRoute(start, end, false, false, 15);
            MapRoute route2 = OpenStreetMapProvider.Instance.GetRoute(p2, p3, false, false, 15);

            GMapRoute r2 = new GMapRoute(route2.Points, "My route2");
            r2.Stroke.Width = 2;
            r2.Stroke.Color = Color.Orange;
            GMapOverlay routesOverlay2 = new GMapOverlay("routes");
            routesOverlay2.Routes.Add(r2);
            gmap.Overlays.Add(routesOverlay2);


            //marker.ToolTipText = string.Format("RSU");

            //data analytics part 
            chart1.Series.Clear();
            //chart1.Series.Add("Car 2");
            //chart1.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
        }

        private void init_config()
        {
            string line;
            String directory = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            try
            {
                StreamReader sr = new StreamReader(directory + "config.txt");
                line = sr.ReadLine();
                if (line != null)
                {
                    char[] delimiterChars = { ' ', ',', ':', '\t' };
                    string[] words = line.Split(delimiterChars);
                    if (words.Length >=2)
                    {
                        server_ip = words[0];
                        server_port = Convert.ToInt32(words[1]);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error in config file");
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            gmap.DragButton = MouseButtons.Left;
            gmap.CanDragMap = true;
            gmap.AutoScroll = true;
            gmap.MapProvider = GMap.NET.MapProviders.BingMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            gmap.SetPositionByKeywords("Clemson, South Carolina");
            gmap.Position = new PointLatLng(34.675455, -82.840624);
            //GMap.NET.Mar
        }

        private void exitToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        static int counter = 20; 
        private void timer1_Tick(object sender, EventArgs e)
        {
            Random rand = new Random();
            double value = (double)rand.Next(0, 100); ///1000.0;
            //marker.Position = new PointLatLng(34.875455+value, -82.840624);
            Console.WriteLine("Ticker running : " + value);
            //chart2.Series[0].Points.AddXY(counter-19, value);

            value = (double)rand.Next(0, 100);
            // chart1.Series[1].Points.AddXY(counter, value);
            //kay_chart.updateChart()
            //kay_chart.TriggeredUpdate(value);
            //kay_chart.updateChart
            // chart2.Series[0].Points.AddXY(counter, value);

            //Series mySeries = chart1.Series[0];
            //Point dp2 = new Point(counter, rand.Next(100));
            //chart1.ChartAreas[0].AxisX.Maximum = counter;
            //chart1.ChartAreas[0].AxisX.Minimum = counter - 20;
            counter++;


            //for(int i = 0; i < chart1.Series[0].LegendText.)

        }

        private void bt_start_Click(object sender, EventArgs e)
        {
            //timer1.Start();
            all_data = String.Empty;
            Thread t = new Thread(new ThreadStart(collect_data));
            t.Start();
            //timer1.Start();
        }
        public void make_connection()
        {
            TcpClient client = null;
            try
            {
                client = new TcpClient(server_ip,server_port);
                isConnectionClose = false;
            }
            catch(Exception er)
            {
                MessageBox.Show("Problem in connection!","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (client == null) return;
            Stream s = null;
            try
            {
                Console.WriteLine("Connected and waiting for message");

                using (StreamReader reader = new StreamReader(client.GetStream(), Encoding.UTF8))
                {
                    string line; 
                    while((line = reader.ReadLine())!= null && isConnectionClose!=true)
                    {
                        // Console.WriteLine(line);
                        all_data += line + Environment.NewLine;
                        if (line.Contains("carid"))
                        {
                            RootObject root = JsonConvert.DeserializeObject<RootObject>(line);
                            Console.WriteLine("Car id : " + root.carid + " : speed : " + root.speed);

                            double time_count = 0.0;
                            if (init_velocity_time == 0.0)
                            {
                                if (Double.TryParse(root.timestamp, out current_time))
                                {
                                    init_velocity_time = current_time;
                                }
                                else
                                {
                                    Func<int> del2 = delegate ()
                                    {
                                        textBox_others.AppendText("Error in converting the timestamp" + Environment.NewLine);
                                        return 0;
                                    };
                                    BeginInvoke(del2);
                                }

                            }

                            if (!dictionary.ContainsKey(root.carid))
                            {
                                dictionary.Add(root.carid, root);
                                GMapOverlay markersOverlay = new GMapOverlay("markers");
                                GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(root.latitude, root.longitude), new Bitmap(Properties.Resources.car1));
                                //GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(root.latitude, root.longitude), GMarkerGoogleType.green);

                                markersOverlay.Markers.Add(marker);
                                gmap.Overlays.Add(markersOverlay);
                                marker.ToolTipMode = MarkerTooltipMode.Always;
                                marker.ToolTipText = ((int)(Math.Round(root.speed, 2) * 2.23694)).ToString() + "mph";
                                dictionary_marker.Add(root.carid, marker);
                                carId_speed_mapper.Add(root.carid, root.speed * 2.23694);
                                //listBox1.Items.Clear();
                                //listBox1.DataSource = new BindingSource(carId_speed_mapper,null);

                                //marker.ToolTip.Marker.Size = ;

                                Func<int> del = delegate ()
                                {
                                    chart1.Series.Add("Car " + (int)root.carid);
                                    carId_to_chart_index_mapper.Add(root.carid,chart1.Series.Count-1); 
                                    chart1.Series[carId_to_chart_index_mapper[root.carid]].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
                                    return 0;
                                };
                                BeginInvoke(del);


                            }
                            else
                            {
                                dictionary[root.carid] = root;
                                dictionary_marker[root.carid].Position = new PointLatLng(root.latitude, root.longitude);
                                dictionary_marker[root.carid].ToolTipText = ((int)(Math.Round(root.speed, 2) * 2.23694)).ToString() + "mph";
                                carId_speed_mapper[root.carid]= (root.speed * 2.23694);

                                if (Double.TryParse(root.timestamp,out current_time))
                                {
                                    time_count = current_time - init_velocity_time;
                                    double time_in_seconds = time_count / 1000.0;
                                    Func<int> del = delegate ()
                                    {
                                        chart1.Series[carId_to_chart_index_mapper[root.carid]].Points.AddXY(time_count/1000.0, root.speed * 2.23694);
                                        value_array[value_array.Length - 1] = root.speed * 2.26;
                                        Array.Copy(value_array, 1, value_array,0, value_array.Length - 1);
                                       
                                        if (root.speed> y_axis_max)
                                        {
                                            y_axis_max = (long)root.speed + 1 ;
                                            chart1.ChartAreas[0].RecalculateAxesScale();
                                            //chart1.ChartAreas[0].AxisX.Minimum = time_count / 1000.0 - 20;
                                            //chart1.ChartAreas[0].RecalculateAxesScale();
                                            //chart1.ChartAreas[0].AxisY.Maximum = (double)y_axis_max + 5;
                                            
                                        }
                                        if (time_in_seconds >= X_RANGE)
                                        {
                                            chart1.ChartAreas[0].AxisX.Minimum = (long)time_in_seconds - X_RANGE;
                                            chart1.ChartAreas[0].AxisX.Maximum = (long)time_in_seconds + 1;
                                            //chart1.Series[0].Label = "Y = #VALY\nX = #VALX"
                                        }
                                        //listBox1.Items.Clear();
                                        //listBox1.DataSource = new BindingSource(carId_speed_mapper, null);

                                        return 0;
                                    };
                                    BeginInvoke(del);
                                }
                            }
                        }
                        else if (line.Contains("eventid"))
                        {
                            Func<int> del = delegate ()
                            {
                                textBox1.AppendText(line+ Environment.NewLine);
                                //chart1.Series[carId_to_chart_index_mapper[root.carid]].Points.AddXY(counter, root.speed);
                                return 0;
                            };
                            BeginInvoke(del);
                            
                        }
                    }
                }                
            }
            finally
            {
                // code in finally block is guranteed 
                // to execute irrespective of 
                // whether any exception occurs or does 
                // not occur in the try block
                if (s != null) s.Close();
                client.Close();
               
            }
        }
        public void collect_data()
        {
            make_connection();
        }
        public void updateUi(int i)
        {
            Func<int> del = delegate ()
            {
                Console.WriteLine(i);
                return 0;
            };
            Invoke(del);
        }

        private void bt_satalite_Click(object sender, EventArgs e)
        {
            gmap.MapProvider = GMapProviders.GoogleChinaSatelliteMap;
        }

        private void bt_normal_Click(object sender, EventArgs e)
        {
            gmap.MapProvider = GMapProviders.GoogleMap;
        }

        private void bt_terrian_Click(object sender, EventArgs e)
        {
            gmap.MapProvider = GMapProviders.GoogleTerrainMap;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            gmap.Zoom = trackBar1.Value;
        }

        private void bt_savedata_Click(object sender, EventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            // set a default file name
            String date = DateTime.Now.ToString("MMMM_dd_yyyy_hh_mm_tt");
            savefile.FileName = "log_"+ date+ ".csv";
            // set filters - this can be done in properties as well
            savefile.Filter = "CSV files (*.csv)|*.txt|All files (*.*)|*.*";

            if (savefile.ShowDialog() == DialogResult.OK)
            {
                using (var fileStream = new FileStream(savefile.FileName, FileMode.Create))
                {
                    Byte[] newinfo = new UTF8Encoding(true).GetBytes(all_data);
                    fileStream.Write(newinfo, 0, newinfo.Length);
                    fileStream.Close();
                    MessageBox.Show("Data saved at log.txt", "Data collection");
                }
            }
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            isConnectionClose = true;
        }

        private void bt_settings_Click(object sender, EventArgs e)
        {
            FromSettings set = new FromSettings();
            set.Show();
        }

        private void bt_clear_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }

        private void bt_save_Click(object sender, EventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            // set a default file name
            String date = DateTime.Now.ToString("MMMM_dd_yyyy_hh_mm_tt");
            savefile.FileName = "EventData_" + date + ".txt";
            // set filters - this can be done in properties as well
            savefile.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (savefile.ShowDialog() == DialogResult.OK)
            {
                using (var fileStream = new FileStream(savefile.FileName, FileMode.Create))
                {
                    Byte[] newinfo = new UTF8Encoding(true).GetBytes(textBox1.Text);
                    fileStream.Write(newinfo, 0, newinfo.Length);
                    fileStream.Close();
                    MessageBox.Show("Data saved at log.txt", "Data collection");
                }
            }
        }
    }
}
