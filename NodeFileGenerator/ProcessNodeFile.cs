using System;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TextTemplating.VSHost;


namespace NodeFileGenerator
{
    [ComVisible(true)]
    [Guid("e1015179-c603-43d2-97b6-7e3f43fec99b")]
    public class ProcessNodeFile : BaseCodeGenerator
    {

        private static int lineCount = 0;
        private static StringBuilder output = new StringBuilder();

        public override string GetDefaultExtension()
        {
            return ".dist.txt";
        }

        protected override byte[] GenerateCode(string inputFileName, string inputFileContent)
        {
            var proc = new Process();
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.FileName = "cmd.exe";
            proc.StartInfo.Arguments = "-i";
            proc.Start();
            proc.BeginOutputReadLine();

            proc.StandardInput.WriteLine("node C:\\Users\\blake\\Desktop\\hello.js C:\\Users\\blake\\Desktop\\test.txt");
            proc.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
            {
                if (!String.IsNullOrEmpty(e.Data))
                {
                    lineCount++;
                    output.Append("\n[" + lineCount + "]: " + e.Data);
                }
            });
            proc.WaitForExit();

            return new UTF8Encoding(true).GetBytes(output.ToString());
        }
    }
}
