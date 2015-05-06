using StatefulCalculatorActor.Interfaces;
using Microsoft.ServiceFabric.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatefulCalculatorActor.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var proxy = ActorProxy.Create<IStatefulCalculatorActor>(ActorId.NewId(), "fabric:/StatefulCalculatorActorApplication");

            var rnd = new Random();
            proxy.Clear().Wait();
            proxy.AddValueAndOperator(rnd.Next(1, 10), CalcOperator.Add).Wait();
            proxy.AddValueAndOperator(rnd.Next(5, 20), CalcOperator.Divide).Wait();
            proxy.AddValueAndOperator(rnd.Next(2, 5), CalcOperator.None).Wait();

            Console.WriteLine("[{0}] Arithmetical Problem: {1}", proxy.GetActorId(), proxy.GetArithmeticalProblem().Result);
            Console.WriteLine("[{0}] Result: {1}", proxy.GetActorId(), proxy.GetResult().Result);

            Console.ReadLine();
        }
    }
}
