using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Security.Principal;
namespace cheese
{
    public class Program
    {
        public static string gtdir;
        static async Task Main()
        {
            Console.WriteLine("make sure u run as admin btw");
            
            await StartUp();
            start();
        }

        public static void Banner(bool type)
        {
            string[] art =
{
"                                                                                  ",
"     ▄▄▄▄▄▄  ▄▄                               ▄   ▄▄▄▄   ▄▄▄▄▄▄▄  ",
"    █▀██▀▀▀█▄ ██                              ▀██████▀  █▀▀██▀▀▀▀ ",
"      ██▄▄▄█▀ ██          ▄▄ ▀▀ ▄               ██   ▄     ██     ",
"      ██▀▀▀   ██ ██ ██ ▄████ ██ ████▄ ▄██▀█     ██  ██     ██     ",
"    ▄ ██      ██ ██ ██ ██ ██ ██ ██    ▀███▄     ██  ██     ██     ",
"    ▀██▀     ▄██▄▀██▀█▄▀████▄██▄██ ▀██▄▄██▀     ▀█████     ▀██▄   ",
"                          ██                    ▄   ██            ",
"                        ▀▀▀                     ▀████▀            "
};

            foreach (string line in art)
            {
                int width = line.Length;

                for (int i = 0; i < width; i++)
                {
                    double t = (double)i / (width - 1);

                    int green = (int)(255 * (1 - t)); // fades out
                    int blue = (int)(255 * t);       // fades in
                    if (type)
                    {
                        TypeEffect($"\x1b[38;2;0;{green};{blue}m{line[i]}", 0.2);
                    }
                    else
                    {
                        Console.Write($"\x1b[38;2;0;{green};{blue}m{line[i]}");
                    }
                    
                }

                Console.WriteLine("\x1b[0m"); // reset at end of line
            }

        }
        public static void checkgtagshi()
        {
            if (gtdir == null)
            {
                if (Directory.Exists(@"C:\Program Files (x86)\Steam\steamapps\common\Gorilla Tag"))
                {
                    gtdir = @"C:\Program Files (x86)\Steam\steamapps\common\Gorilla Tag";
                    Console.WriteLine("Found Directory");
                    Thread.Sleep(1000);
                }
                else if (Directory.Exists(@"C:\Program Files\Steam\steamapps\common\Gorilla Tag"))
                {
                    gtdir = @"C:\Program Files\Steam\steamapps\common\Gorilla Tag";
                    Console.WriteLine("Found Directory");
                    Thread.Sleep(1000);
                }
                else
                {
                    Console.Write("Could not find Gorilla Tag folder, please enter the path manually: ");
                    gtdir = Console.ReadLine();
                    
                }
            }
            else
            {
                clearconsole(false);
            }
        }
        public static async Task StartUp()
        {

            EnableAnsi();
            clearconsole(true);
            Console.WriteLine("A bloat free bepiex installer");

            Console.WriteLine("Checking version...");
            await CheckForUpdates(); 
            clearconsole(false);
            Console.WriteLine("Checking dir");
            clearconsole(false);
            if (!IsRunningAsAdmin())
            {
                Console.WriteLine("not running as admin quitting in 5 seconds");
                Thread.Sleep(5000);
                Environment.Exit(0);
            }

        }
        public static void start()
        {
            
            clearconsole(false);

            Console.WriteLine("Type \x1b[41m Help \x1b[0m to see commands");
            Console.Write("\x1b[35m" + Environment.UserName + "\x1b[0m: ");
            string input = Console.ReadLine();
            handercommand(input);
        }

        public static async Task CheckForUpdates()
        {
            string pasteUrl = "https://pastebin.com/raw/6gaiRv2m";
            string currentVersion = "0.0.1";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string content = await client.GetStringAsync(pasteUrl);

                    if (content.Contains(currentVersion))
                    {
                        Console.WriteLine("You are up to date!");
                        
                    }
                    else
                    {
                        clearconsole(false);
                        Console.WriteLine($"Outdated version.\nVersion: {content} is avaiable");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not check for updates: " + ex.Message);
                }
            }
        }
        public static void clearconsole(bool type)
        {
            Console.Clear();
            Banner(type);
        }
        
        public static void handercommand(string command)
        {
            switch (command.ToLower())
            {
                case "help":
                    Console.WriteLine("Available commands:\nhelp - Show this help message\nexit - Exit the application");
                    Console.ReadKey();
                    start();
                    break;
                case "exit":
                    Console.WriteLine("Exiting...");
                    Environment.Exit(0);
                    break;
                case "update":
                    Console.WriteLine("press any button to open link");
                    Console.ReadKey();
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "https://google.com",
                        UseShellExecute = true
                    });
                    start();
                    break;
                case "installmods":
                    checkgtagshi();
                    Console.WriteLine("Installing mods...");
                    
                    string modUrl = "https://github.com/BepInEx/BepInEx/releases/download/v5.4.23.4/BepInEx_win_x64_5.4.23.4.zip"; 
                   
                    DownloadFileAsync(modUrl, $"C:\\Users\\{Environment.UserName}\\Downloads").Wait();
                    Console.WriteLine("Download mods Unzipping...");
                    UnzipFile(Path.Combine(gtdir, "BepInEx_win_x64_5.4.23.4.zip"), gtdir);
                    Console.WriteLine("Install finsihed i think");
                    Console.ReadKey();
                    start();
                    break;
                default:
                    Console.WriteLine("Unknown command. Type 'help' for a list of commands.");
                    Console.ReadKey();
                  start();
                    break;
            }
        }
        public static async Task DownloadFileAsync(string url, string path)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    Console.WriteLine($"Downloading from {url}...");

                    byte[] data = await client.GetByteArrayAsync(url);

                    // Ensure the directory exists
                    Directory.CreateDirectory(Path.GetDirectoryName(path)!);

                    await File.WriteAllBytesAsync(path, data);

                    Console.WriteLine($"Download complete! Saved to {path}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Download failed: " + ex.Message);
                }
            }
        }
        public static void UnzipFile(string zipPath, string extractPath)
        {
            if (!File.Exists(zipPath))
            {
                Console.WriteLine("Zip file does not exist: " + zipPath);
                return;
            }

            try
            {
                // Ensure the destination directory exists
                Directory.CreateDirectory(extractPath);

                // Extract the zip file
                ZipFile.ExtractToDirectory(zipPath, extractPath);
                Console.WriteLine("Extraction complete!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to unzip file: " + ex.Message);
            }
        }
        static void TypeEffect(string text, double delayMs)
        {
            var sw = new Stopwatch();
            foreach (char c in text)
            {
                Console.Write(c);
                sw.Restart();
                while (sw.Elapsed.TotalMilliseconds < delayMs) { } // busy wait
            }
        }
        public static bool IsRunningAsAdmin()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out int lpMode);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, int dwMode);

        private const int STD_OUTPUT_HANDLE = -11;
        private const int ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x4;

        public static void EnableAnsi()
        {
            IntPtr handle = GetStdHandle(STD_OUTPUT_HANDLE);
            if (GetConsoleMode(handle, out int mode))
            {
                mode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING;
                SetConsoleMode(handle, mode);
            }
        }
    }
}