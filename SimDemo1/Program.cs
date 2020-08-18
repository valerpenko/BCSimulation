using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimLib;

namespace SimDemo1
{
    class Program
    {
        static void Main(string[] args)
        {
            const int nodeCount = 7;
            Network net = new Network(nodeCount);
            int[,] links =
            //new int[nodeCount, 2] { { 0, 1 }, { 0, 2 }, { 1, 3 }, { 3, 0 }};
            new int[nodeCount, 2] { { 0, 1 }, { 0, 2 }, { 1, 3 }, { 2, 3 }, { 3, 4 }, { 3, 5 }, { 3, 6 } };
            net.SetConnections(links);
            ConsoleObserver co = new ConsoleObserver(net);
            net.Model.AddObserver(co);
            net.SendMessage(1, "OK");
            net.SendMessage(0, "Hello");
            net.Run();
            for (int i = 0; i < net.NetworkStatus().Length; i++)
                Console.WriteLine(net.NetworkStatus()[i]);
        }

    }
    class ConsoleObserver : Observer
    {
        private Network net;
        public ConsoleObserver(Network n) { net = n; }
        public void HandleEvent()
        {
            Console.WriteLine(net.Model.CurNodeCommand.NodeCommandResult);
        }
    }
}
