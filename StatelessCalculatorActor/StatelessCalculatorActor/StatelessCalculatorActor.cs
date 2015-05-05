using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StatelessCalculatorActor.Interfaces;
using Microsoft.ServiceFabric;
using Microsoft.ServiceFabric.Actors;

namespace StatelessCalculatorActor
{
    public class StatelessCalculatorActor : Actor, IStatelessCalculatorActor
    {
        public async Task<double> Add(double x, double y)
        {
            ActorEventSource.Current.ActorMessage(this, "Add {0} and {1}", x, y);

            return await Task.FromResult(x + y);
        }

        public async Task<double> Divide(double x, double y)
        {
            ActorEventSource.Current.ActorMessage(this, "Divide {0} by {1}", x, y);

            return await Task.FromResult(x / y);
        }

        public async Task<double> Multiply(double x, double y)
        {
            ActorEventSource.Current.ActorMessage(this, "Multiply {0} and {1}", x, y);

            return await Task.FromResult(x * y);
        }

        public async Task<double> Substract(double x, double y)
        {
            ActorEventSource.Current.ActorMessage(this, "Substract {1} from {0}", x, y);

            return await Task.FromResult(x - y);
        }
    }
}
