using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

namespace StatelessCalculatorActor.Interfaces
{
    public interface IStatelessCalculatorActor : IActor
    {
        Task<double> Add(double x, double y);
        Task<double> Substract(double x, double y);
        Task<double> Multiply(double x, double y);
        Task<double> Divide(double x, double y);
    }
}
