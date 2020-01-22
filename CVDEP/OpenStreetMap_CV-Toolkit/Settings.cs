using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace OpenStreetMap_CV_Toolkit
{
    public partial class FromSettings : Form
    {
        public FromSettings()
        {
            InitializeComponent();
            read_config();
        }

        private void bt_cancle_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void read_config()
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
                    if (words.Length >= 2)
                    {
                        textBox_server.Text = words[0];
                        textBox_port.Text = words[1];
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in config file");
            }
        }

        private void bt_set_Click(object sender, EventArgs e)
        {
            String server_ip = textBox_server.Text;
            IPAddress address;
            if(IPAddress.TryParse(server_ip,out address))
            {
                // valid ip address ;
                int port;  
                if(Int32.TryParse(textBox_port.Text,out port))
                {
                    // valid server ip and port. then save to file.
                    String directory = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
                    try
                    {
                        StreamWriter sw = new StreamWriter(directory + "config.txt");
                        sw.WriteLine(server_ip + "," + port);
                        sw.Close();
                        MessageBox.Show("Configuration saved!");
                        this.Dispose();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error in writing to file!");
                    }
                }
                else
                {
                    MessageBox.Show("Error port input");
                }
            }
            else
            {
                MessageBox.Show("Invalid ip address!");
            } 
        }
    }
}
