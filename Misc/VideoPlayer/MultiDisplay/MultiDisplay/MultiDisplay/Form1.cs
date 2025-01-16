using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using LibVLCSharp.Shared;
using LibVLCSharp.WinForms;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace MultiDisplay
{
    public partial class Form1 : Form
    {

        private LibVLC _libVLC;

        public Form1()
        {
            InitializeComponent();
            Core.Initialize();

            _libVLC = new LibVLC();

       
            // Add more streams as needed

            GetCameras();
        }

        public class Cameras
        {
            public string cameraName { get; set; }
            public string URL { get; set; }
        }


        public void GetCameras()
        {

            string executableDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string jsonFilePath = Path.Combine(executableDirectory, "Cameras.json");



            if (!File.Exists(jsonFilePath))
            {

                var list = new List<Cameras>();
                list.Add(new Cameras { cameraName = "Camera 1", URL = "rtsp://admin:Scales123@10.0.1.241/Streaming/channels/1/picture" });
                list.Add(new Cameras { cameraName = "Camera 2", URL = "rtsp://admin:Scales123@10.0.1.240/Streaming/channels/1/picture" });
                //list.Add(new Cameras { cameraName = "Camera 3", URL = "rtsp://admin:Scales123@10.0.1.241/Streaming/channels/1/picture" });
                //list.Add(new Cameras { cameraName = "Camera 4", URL = "rtsp://admin:Scales123@10.0.1.240/Streaming/channels/1/picture" });
                //list.Add(new Cameras { cameraName = "Camera 5", URL = "rtsp://admin:Scales123@10.0.1.241/Streaming/channels/1/picture" });
                //list.Add(new Cameras { cameraName = "Camera 6", URL = "rtsp://admin:Scales123@10.0.1.240/Streaming/channels/1/picture" });


                // Serialize the dictionary to JSON format
                string json = JsonConvert.SerializeObject(list, Formatting.Indented);

                // Write the JSON to a file
                System.IO.File.WriteAllText(jsonFilePath, json);





            }
          

                string jsonContent = File.ReadAllText(jsonFilePath);

                // Parse JSON content if needed
                var lst = JsonConvert.DeserializeObject<List<Cameras>>(jsonContent);
            foreach (var item in lst)
            {
                AddVlcStream(item.URL);
            }

            //AddVlcStream("rtsp://admin:Scales123@10.0.1.241/Streaming/channels/1/picture");
            //AddVlcStream("rtsp://admin:Scales123@10.0.1.240/Streaming/channels/1/picture");
            //AddVlcStream("rtsp://admin:Scales123@10.0.1.241/Streaming/channels/1/picture");
            //AddVlcStream("rtsp://admin:Scales123@10.0.1.240/Streaming/channels/1/picture");
            //AddVlcStream("rtsp://admin:Scales123@10.0.1.241/Streaming/channels/1/picture");
            //AddVlcStream("rtsp://admin:Scales123@10.0.1.240/Streaming/channels/1/picture");
            // Add more streams as needed



        }


        private void AddVlcStream(string streamUrl)
        {
            

            var videoView = new VideoView();
            videoView.MediaPlayer = new MediaPlayer(_libVLC);
            videoView.MediaPlayer.EnableKeyInput = true;
            videoView.MediaPlayer.EnableMouseInput = true;
            
            var media = new Media(_libVLC, new Uri(streamUrl));
            videoView.MediaPlayer.Play(media);
            videoView.Dock = DockStyle.Fill;
            videoView.Click += VideoView_Click;
            
          
            
            tableLayoutPanel1.Controls.Add(videoView);

        }

      
        private void VideoView_Click(object sender, EventArgs e)
        {
            var videoView = (VideoView)sender;
            MessageBox.Show("Clicked");
            // Remove the clicked video from the layout
            tableLayoutPanel1.Controls.Remove(videoView);
            tableLayoutPanel1.Hide();
            // Maximize the form to full screen
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;

            // Add the video to the form directly
            Controls.Add(videoView);
            videoView.Dock = DockStyle.Fill;

            // Subscribe to a custom event to handle exiting full screen
            videoView.DoubleClick += VideoView_DoubleClick;
        }

        private void VideoView_DoubleClick(object sender, EventArgs e)
        {
            var videoView = (VideoView)sender;

            // Remove the video from the form and restore original layout
            Controls.Remove(videoView);
            tableLayoutPanel1.Controls.Add(videoView);
            tableLayoutPanel1.Show();
            videoView.Dock = DockStyle.Fill;

            // Restore the form's properties
            FormBorderStyle = FormBorderStyle.Sizable;
            WindowState = FormWindowState.Normal;

            // Unsubscribe from the custom event
            videoView.DoubleClick -= VideoView_DoubleClick;
        }

        private void tableLayoutPanel1_Click(object sender, EventArgs e)
        {

        }
    }


    }
