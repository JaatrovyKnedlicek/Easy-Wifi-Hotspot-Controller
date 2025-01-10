// MIT License
// Copyright (c) 2025 Ján Repka

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System.Diagnostics;
using System.Security.Principal;

namespace Wifi_Hostovanie {
    class Program {
        static string ExecuteNetshCommand(string arguments) {
            var process = new Process {
                StartInfo = new ProcessStartInfo {
                    FileName = "netsh",
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (!string.IsNullOrEmpty(error)) {
                return "ERROR: " + error;
            }

            return output;
        }
        static void Setup() {
            Console.Write("SSID for you WiFi network:");
            string SSID = Console.ReadLine();
            Console.Write("Password for your WiFi network:");
            string Password = Console.ReadLine();
            Console.WriteLine(ExecuteNetshCommand($"wlan set hostednetwork mode=allow ssid={SSID} key={Password}"));
            Console.WriteLine("Press enter to continue...");
            Console.ReadLine();
            MainMenu();
        }

        
        static void StartHotspot() {
            string reuslt = ExecuteNetshCommand("wlan start hostednetwork");
            System.Console.WriteLine(reuslt);
            Console.WriteLine("Press enter to continue...");
            Console.ReadLine();
        }
        static void StopHotspot() {
            string reuslt = ExecuteNetshCommand("wlan stop hostednetwork");
            System.Console.WriteLine(reuslt);
            Console.WriteLine("Press enter to continue...");
            Console.ReadLine();
        }
        static void Info() {
            string result = ExecuteNetshCommand("wlan show hostednetwork");
            System.Console.WriteLine(result);
            Console.WriteLine("Press enter to continue...");
            Console.ReadLine();
        }
        static void Configure() {
            Console.Clear();
            Console.WriteLine("Configuration:\n1. SSID\n2. Password\n3. Allow hosted network\n4. Disallow hosted network\n0. Back");
            string answear = Console.ReadLine();
            if (answear == "1") {
                Console.Write("Write new SSID: ");
                string ssid = Console.ReadLine();
                ExecuteNetshCommand($"wlan set hostednetwork ssid={ssid}");
                Console.WriteLine("Press enter to continue...");
                Console.ReadLine();
            }
            else if (answear == "2") {
                Console.Write("Write new password: ");
                string password = Console.ReadLine();
                ExecuteNetshCommand($"wlan set hostednetwork password={password}");
                Console.WriteLine("Press enter to continue...");
                Console.ReadLine();
            }
            else if (answear == "3") {
                Console.WriteLine(ExecuteNetshCommand($"wlan set hostednetwork mode=allow"));
                Console.WriteLine("Press enter to continue...");
                Console.ReadLine();
            }
            else if (answear == "4") {
                Console.WriteLine(ExecuteNetshCommand($"wlan set hostednetwork mode=disallow"));
                Console.WriteLine("Press enter to continue...");
                Console.ReadLine();
            }
            else {MainMenu();}
        }
        


        static void Main(string[] args) {
            var isAdmin = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
            if (!isAdmin) {
                var processInfo = new ProcessStartInfo {
                    FileName = System.Reflection.Assembly.GetExecutingAssembly().Location,
                    UseShellExecute = true,
                    Verb = "runas"
                };
                try {
                    Process.Start(processInfo);
                }
                catch (Exception) {
                    System.Console.WriteLine("Run this app again as administrator");
                    Console.ReadLine();
                    Environment.Exit(0);
                }
            }

            MainMenu();
        }
        static void MainMenu() {
            while (true) {
                Console.Clear();
                Console.WriteLine("Easy WiFi hotspot controller 1.1\n(C) Ján Repka 2025\nLicensed under MIT license\nMore info about license in LICENSE file or https://mit-license.org/");
                System.Console.WriteLine("\n1. Easy setup\n2. Start\n3. Stop\n4. Info\n5. Configure\n0. Exit");
                Console.Write("Choose an option and press enter:");
                string choice = Console.ReadLine();

                if (choice == "1") {
                    Setup();
                }
                else if (choice == "2") {StartHotspot();}
                else if (choice == "3") {StopHotspot();}
                else if (choice == "4") {Info();}
                else if (choice == "5") {Configure();}
                else if (choice == "0") {Environment.Exit(0);}
                else {continue;}
            }
            

        }
    }
}