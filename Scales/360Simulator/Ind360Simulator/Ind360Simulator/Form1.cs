using Microsoft.Extensions.Configuration;

namespace Ind360Simulator
{
    public partial class Form1 : Form
    {
        private readonly List<string> _scaleAddresses = new();
        private readonly List<TrackBar> _scaleTrackBars = new();

        public Form1()
        {
            InitializeComponent();

            // Load configuration
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
                .Build();

            _scaleAddresses = config.GetSection("Scales:Addresses").Get<List<string>>() ?? new List<string>();

            // Dynamically add controls for each scale
            int y = 20;
            for (int i = 0; i < _scaleAddresses.Count; i++)
            {
                var address = _scaleAddresses[i];

                // IP Address Label
                var ipLabel = new Label
                {
                    Text = $"IP: {address}",
                    Location = new Point(20, y),
                    AutoSize = true
                };
                Controls.Add(ipLabel);

                // Weight Label
                var weightLabel = new Label
                {
                    Text = "Weight: 0",
                    Location = new Point(200, y),
                    AutoSize = true
                };
                Controls.Add(weightLabel);

                // TrackBar
                var trackBar = new TrackBar
                {
                    Minimum = -100,
                    Maximum = 12000,
                    TickFrequency = 500,
                    LargeChange = 500,
                    SmallChange = 100,
                    Value = 0,
                    Width = 300,
                    Location = new Point(20, y + 25),
                    Tag = (address, weightLabel) // Store the address and weight label for event handler
                };
                trackBar.ValueChanged += TrackBar_ValueChanged;
                Controls.Add(trackBar);
                _scaleTrackBars.Add(trackBar);

                y += 70; // Space between controls
            }

            this.Load += Form1_Load;
            this.FormClosing += Form1_FormClosing;
        }


        private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
        {
            Console.WriteLine("Form closing started.");
            var tasks = _scaleAddresses
                .Select(addr => StopSimulatingAsync(addr, reinit: true))
                .ToArray();

            try
            {
                // Wait for all tasks, but timeout after 3 seconds
                if (!Task.WhenAll(tasks).Wait(TimeSpan.FromSeconds(3)))
                {
                    Console.WriteLine("Timeout waiting for scales to stop.");
                }
                else
                {
                    Console.WriteLine("All scales stopped simulating.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error stopping simulation: {ex.Message}");
            }
            Console.WriteLine("Form closing finished.");
        }


        private async void Form1_Load(object? sender, EventArgs e)
        {
            using var cts = new CancellationTokenSource();
            var token = cts.Token;

            var tasks = _scaleAddresses
                .Select(addr => StartSimulatingAsync(addr, reinit: true, cancellationToken: token))
                .ToArray();

            try
            {
                await Task.WhenAll(tasks);
                Console.WriteLine("All scales started simulating.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting simulation: {ex.Message}");
            }
        }


        private async void TrackBar_ValueChanged(object? sender, EventArgs e)
        {
            if (sender is TrackBar trackBar && trackBar.Tag is ValueTuple<string, Label> tag)
            {
                string address = tag.Item1;
                Label weightLabel = tag.Item2;
                int weight = trackBar.Value;

                // Update the weight label
                weightLabel.Text = $"Weight: {weight}";

                try
                {
                    var response = await SendWeightAsync(address, weight, reinit: true, cancellationToken: CancellationToken.None);
                    Console.WriteLine($"{DateTime.Now:HH:mm:ss} -> Sent {weight} to {address}, response: {response}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending weight to {address}: {ex.Message}");
                }
            }
        }

        private static readonly HttpClient _http = new HttpClient();

        public static async Task<string> SendWeightAsync(string IpAddress, int weight, bool reinit = true, CancellationToken cancellationToken = default)
        {
            var url = $"http://{IpAddress}/DAW?WeightSim_WgtInput={weight}" +
                      (reinit ? "&Reinit" : "");

            var response = await _http.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync(cancellationToken);
        }


        public static async Task<string> StartSimulatingAsync(string IpAddress, bool reinit = true, CancellationToken cancellationToken = default)
        {
            var url = $"http://{IpAddress}/DAW?WeightSimSwitch=1&Reinit";

            var response = await _http.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync(cancellationToken);
        }
        public static async Task<string> StopSimulatingAsync(string IpAddress, bool reinit = true)
        {
            var url = $"http://{IpAddress}/DAW?WeightSimSwitch=0&Reinit";

            var response = await _http.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
