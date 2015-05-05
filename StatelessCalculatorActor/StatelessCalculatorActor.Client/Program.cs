using StatelessCalculatorActor.Interfaces;
using Microsoft.ServiceFabric.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatelessCalculatorActor.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var proxy = ActorProxy.Create<IStatelessCalculatorActor>(
                ActorId.NewId(), "fabric:/StatelessCalculatorActorApplication");

            Console.WriteLine(
                "[Actor {0}]: 1 + 5 = {1}",
                proxy.GetActorId(),
                proxy.Add(1, 5).Result);

            Console.WriteLine(
                "[Actor {0}]: 11 - 6 = {1}",
                proxy.GetActorId(),
                proxy.Substract(11, 6).Result);

            Console.WriteLine(
                "[Actor {0}]: 3 * 5 = {1}",
                proxy.GetActorId(),
                proxy.Multiply(3, 5).Result);

            Console.WriteLine(
                "[Actor {0}]: 9 / 2 = {1}",
                proxy.GetActorId(),
                proxy.Divide(9, 2).Result);

            Console.ReadLine();
        }
    }
}
