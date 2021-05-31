using System.Diagnostics;
using System.Threading.Tasks;
using System;

namespace Aimrank.Agones.Infrastructure.CSGO
{
    internal class ServerProcess : IDisposable
    {
        private readonly Process _process;
        
        public string SteamToken { get; }
        public bool IsRunning { get; private set; }
        
        public ServerProcess(string steamToken)
        {
            SteamToken = steamToken;
            
            var shellCommand = $"cd /home/steam/csgo && /home/steam/start-csgo.sh {steamToken}";
            
            _process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{shellCommand}\"",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                }
            };
        }

        public void Start()
        {
            _process.Start();

            IsRunning = true;
        }

        public async Task StopAsync()
        {
            if (IsRunning)
            {
                await ExecuteScreenCommandAsync("quit");
            }

            IsRunning = false;
        }

        public async Task StartMatchAsync(MatchSettings matchSettings)
        {
            await ExecuteScreenCommandAsync($"stuff 'aimrank_whitelist {matchSettings.Whitelist}\n'");
            await ExecuteScreenCommandAsync($"stuff 'map {matchSettings.Map}\n'");
        }

        public void Dispose()
        {
            StopAsync().Wait();

            try
            {
                _process.Kill(true);
            }
            finally
            {
                _process.Dispose();
            }
        }
        
        private Task ExecuteScreenCommandAsync(string command)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "sh",
                    Arguments = $"-c \"screen -p 0 -S csgo -X {command}\"",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                }
            };

            process.Start();
            
            return process.WaitForExitAsync();
        }
    }
}
