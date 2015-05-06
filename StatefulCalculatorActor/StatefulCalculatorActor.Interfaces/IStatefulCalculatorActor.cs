using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

namespace StatefulCalculatorActor.Interfaces
{
    public interface IStatefulCalculatorActor : IActor
    {
        Task AddValueAndOperator(double value, CalcOperator calcOperator);
        Task Clear();
        Task<double> GetResult();
        Task<string> GetArithmeticalProblem();
    }
}
