using Apache.Ignite.Core;
using Apache.Ignite.Core.Discovery.Tcp.Static;
using Apache.Ignite.Core.Discovery.Tcp;
using ComputeNode.Main;
using System.Runtime;
using System.Diagnostics;

//int length = 1000;
//int[][] arr = new int[length][];
//Random random = new Random();
//for (int i = 0; i < length; i++)
//{
//    arr[i] = new int[length];
//    for (int j = 0; j < length; j++)
//    {
//        arr[i][j] = random.Next(10);
//    }
//}
int[][] arr = new int[][] {
    new int[] { 6, 9,9, 5 },
    new int[] { 6,2,6,9},
    new int[] { 4,8,0,3},
    new int[] { 5, 8, 7, 3 }
};

OutputHelper.Write(arr);

var ignite = Ignition.Start(new IgniteConfiguration
{
    DiscoverySpi = new TcpDiscoverySpi
    {
        LocalPort = 48500,
        LocalPortRange = 20,
        
        IpFinder = new TcpDiscoveryStaticIpFinder
        {
            Endpoints = new[]
               {
                    "127.0.0.1:48500..48520"
                }
        }
    }
});

Stopwatch stopwatch = new Stopwatch();
var compute = ignite.GetCompute();

stopwatch.Start();
var res = compute.Execute(new ArrayTask(), arr);
stopwatch.Stop();
Console.WriteLine("res=");
//OutputHelper.Write(res);

Console.WriteLine("time=");
Console.WriteLine(stopwatch.ElapsedMilliseconds);

