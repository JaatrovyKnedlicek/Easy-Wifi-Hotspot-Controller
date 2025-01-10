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

using System;
using System.Diagnostics;

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
        static void ConfigureHotspot(string ssid, string password) {
            string result = ExecuteNetshCommand($"wlan set hostednetwork mode=allow ssid={ssid} key={password}");
            Console.WriteLine(result);
            Console.WriteLine("Press enter to continue...");
            Console.ReadLine();
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
        static void ShowConnectedDevices() {
            string result = ExecuteNetshCommand("wlan show hostednetwork");
            System.Console.WriteLine(result);
            Console.WriteLine("Press enter to continue...");
            Console.ReadLine();
        }


        static void Main(string[] args) {
            MainMenu();
        }
        static void MainMenu() {
            while (true) {
                Console.Clear();
                Console.WriteLine("Easy WiFi hotspot controller 1.0\n(C) Ján Repka 2025\nLicensed under MIT license\nMore info about license in LICENSE file or https://mit-license.org/");
                System.Console.WriteLine("\n1. Configure\n2. Start\n3. Stop\n4. Show connected devices\n5. Exit");
                Console.Write("Choose an option and press enter:");
                string choice = Console.ReadLine();

                if (choice == "1") {
                    Console.Write("Enter SSID: ");
                    string ssid = Console.ReadLine();
                    Console.Write("Enter password (at least 8 characters):");
                    string password = Console.ReadLine();
                    ConfigureHotspot(ssid, password);
                }
                else if (choice == "2") {StartHotspot();}
                else if (choice == "3") {StopHotspot();}
                else if (choice == "4") {ShowConnectedDevices();}
                else if (choice == "5") {Environment.Exit(0);}
                else {continue;}
            }
            

        }
    }
}