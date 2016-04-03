using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
namespace MasterServerGame
{
    class Program
    {
        public static ServerGet serverGet = new ServerGet();
        static void Main(string[] args)
        {
            Console.WriteLine("Master Server Started");
            serverGet.Initalize();
            Console.ReadLine();
        }
    }
}
