using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shantae
{
    public class Program
    {
        const string OFFSETS_API = "http://192.168.172.2:5000";
        const string SHANTAE_103 = "ShantaeCurse";
        const string SHANTAE_104 = "Shantae and the Pirate's Curse";

        private static readonly Dictionary<string, string> WantedLUT = new() {
            {"steam", SHANTAE_103},
            {"gog", SHANTAE_104},
        };

        const int PROCESS_WM_READ = 0x0010;
        const int PROCESS_VM_WRITE = 0x0020;
        const int PROCESS_VM_OPERATION = 0x0008;

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess,
          int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress,
          byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);

        public static void Main(string[] args)
        {
            string json;

            using (WebClient client = new())
            {
                json = client.DownloadString(OFFSETS_API);
            }

            var analysis = JsonSerializer.Deserialize<Analysis>(json);

            if (analysis == null)
            {
                return;
            }

            string wantedSource;
            string wantedTarget;

            if (args.Length >= 2)
            {
                wantedSource = WantedLUT[args[0]];
                wantedTarget = WantedLUT[args[1]];
            }
            else
            {
                wantedSource = SHANTAE_104;
                wantedTarget = SHANTAE_103;
            }

            var sourceProcess = Process.GetProcessesByName(wantedSource)[0];
            var sourceHandle = OpenProcess(PROCESS_WM_READ, false, sourceProcess.Id);
            var sourceAddress = BaseAddress(sourceProcess);

            var sourceStart = IntPtr.Add(sourceAddress, analysis.Block.GOG.Start);
            var sourceBuffer = new byte[analysis.Block.GOG.Length * 5];
            var sourceRead = 0;

            if (ReadProcessMemory((int)sourceHandle, (int)sourceStart, sourceBuffer, sourceBuffer.Length, ref sourceRead) == false)
            {
                Console.WriteLine("Could not read from source process memory.");
                return;
            }

            Console.WriteLine($"Read {sourceRead} bytes from source process memory.");

            var targetProcess = Process.GetProcessesByName(wantedTarget)[0];
            var targetHandle = OpenProcess(PROCESS_WM_READ | PROCESS_VM_WRITE | PROCESS_VM_OPERATION, false, targetProcess.Id);
            var targetAddress = BaseAddress(targetProcess);

            var targetStart = IntPtr.Add(targetAddress, analysis.Block.Steam.Start);
            var targetBuffer = sourceBuffer;
            var targetWritten = 0;

            if (WriteProcessMemory((int)targetHandle, (int)targetStart, targetBuffer, targetBuffer.Length, ref targetWritten) == false)
            {
                Console.WriteLine("Could not write to target process memory.");
                return;
            }

            Console.WriteLine($"Written {targetWritten} bytes to target process memory.");
        }

        private static nint BaseAddress(Process process)
        {
            ProcessModule module;

            if (process.MainModule == null)
            {
                return 0;
            }

            module = process.MainModule;
            return module.BaseAddress;
        }
    }

    public class Analysis
    {
        [JsonPropertyName("items")]
        public List<AnalysisItem> Items { get; set; } = [];
        [JsonPropertyName("block")]
        public AnalysisBlock Block { get; set; } = new();

        public class AnalysisBlock
        {
            [JsonPropertyName("xml")]
            public BlockEntry XML { get; set; } = new BlockEntry();
            [JsonPropertyName("104")]
            public BlockEntry GOG { get; set; } = new BlockEntry();
            [JsonPropertyName("103")]
            public BlockEntry Steam { get; set; } = new BlockEntry();

            public class BlockEntry
            {
                [JsonPropertyName("start")]
                public int Start { get; set; } = 0;
                [JsonPropertyName("end")]
                public int End { get; set; } = 0;
                [JsonPropertyName("length")]
                public int Length { get; set; } = 0;
            }
        }

        public class AnalysisItem
        {
            [JsonPropertyName("item")]
            public string Name { get; set; } = string.Empty;

            [JsonPropertyName("offsets")]
            public ItemOffset Offset { get; set; } = new ItemOffset();

            [JsonPropertyName("type")]
            public string Type { get; set; } = string.Empty;

            [JsonPropertyName("start")]
            public int Start { get; set; } = 0;

            [JsonPropertyName("length")]
            public int Length { get; set; } = 0;

            public class ItemOffset
            {
                [JsonPropertyName("xml")]
                public int XML { get; set; }
                [JsonPropertyName("104")]
                public int GOG { get; set; }
                [JsonPropertyName("103")]
                public int Steam { get; set; }
            }
        }
    }
}