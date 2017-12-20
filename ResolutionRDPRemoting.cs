using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Resolution.RDP.Remoting
{
    class Program
    {
        static int Main(string[] args)
        {

            if (args.Length < 3)
            {
                Console.WriteLine("Usage: Resolution.RDP.Remoting \"<name of computer>\" screenwidth screenheight");
                return 1;
            }

            //string targetMachine = args[0];
            string rdpFile = args[0];
            int targetWidth = int.Parse(args[1]);
            int targetHeight = int.Parse(args[2]);

            //  Open the Rdp Session to the target.
            var rdpProcess = new Process
            {
                StartInfo = {
                   FileName = Environment.ExpandEnvironmentVariables(@"%SystemRoot%\system32\mstsc.exe"),
                    Arguments = string.Format("\"{0}\" /admin /w:{1} /h:{2} /control", rdpFile, targetWidth, targetHeight)
                }
            };

            rdpProcess.Start();
            while (rdpProcess.HasExited == false)
                Thread.Sleep(500);  //  Give the process a moment to startup
 
            if (rdpProcess.ExitCode != 0)
                throw new RemotingException(string.Format("Unable to open an RDP window to the following machine: {0}", rdpFile));
 
            Thread.Sleep(5000);   //  Connection Wait
 
            var rdpProcesses = Process.GetProcessesByName("mstsc");
            if (rdpProcesses.Any() == false)
                throw new Exception("Remote Desktop Process not found.");

            return 0;
        }
    }
}
