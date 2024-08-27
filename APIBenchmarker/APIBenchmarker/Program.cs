// See https://aka.ms/new-console-template for more information
using APIBenchmarker;
using BenchmarkDotNet.Running;
using System.Net.Http.Headers;

string command = args[0];

if (command.Trim().ToLower() == "imagetest")
{
    if(args.Length > 1)
    {
        int iterations = int.Parse(args[1]);
        Tester tester = new Tester();
        await tester.Setup();
        await tester.TestImages(iterations, false);
    }

}
else if (command.Trim().ToLower() == "benchmark") {
    var summary = BenchmarkRunner.Run<Benchmarker>();

}
else if (command.Trim().ToLower() == "pretest")
{
    var benchmarker = new Benchmarker();
    await benchmarker.Setup();
    await benchmarker.PreTest();
}





