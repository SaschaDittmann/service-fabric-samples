using Microsoft.JScript;
using Microsoft.JScript.Vsa;
using Microsoft.ServiceFabric.Actors;
using StatefulCalculatorActor.Interfaces;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace StatefulCalculatorActor
{
    public class StatefulCalculatorActor : Actor<StatefulCalculatorActorState>, IStatefulCalculatorActor
    {
        private readonly CultureInfo _culture;
        public StatefulCalculatorActor()
        {
            _culture = CultureInfo.CreateSpecificCulture("en-US");
        }

        public Task AddValueAndOperator(double value, CalcOperator calcOperator)
        {
            ActorEventSource.Current.ActorMessage(this, "Adding value {0} and operator {1} to set.", value, calcOperator);
            State.ValueOperatorSets.Add(new ValueOperatorSet
            {
                Value = value,
                Operator = calcOperator,
            });

            return Task.FromResult(true);
        }

        public Task Clear()
        {
            ActorEventSource.Current.ActorMessage(this, "Removing all sets.");
            State.ValueOperatorSets.Clear();

            return Task.FromResult(true);
        }

        public async Task<double> GetResult()
        {
            var arithmeticalProblem = await GetArithmeticalProblem();
            return JScriptEval(arithmeticalProblem);
        }

        public Task<string> GetArithmeticalProblem()
        {
            ActorEventSource.Current.ActorMessage(this, "Building arithmetical problem");
            var sb = new StringBuilder();

            foreach(var set in State.ValueOperatorSets)
            {
                sb.Append(set.Value.ToString(_culture));
                switch (set.Operator)
                {
                    case CalcOperator.Add:
                        sb.Append(" + ");
                        break;
                    case CalcOperator.Substract:
                        sb.Append(" - ");
                        break;
                    case CalcOperator.Multiply:
                        sb.Append(" * ");
                        break;
                    case CalcOperator.Divide:
                        sb.Append(" / ");
                        break;
                }
            };

            return Task.FromResult(sb.ToString());
        }

        private static readonly VsaEngine _engine = VsaEngine.CreateEngine();
        private static double JScriptEval(string expr)
        {
            // error checking etc removed for brevity
            return double.Parse(Eval.JScriptEvaluate(expr, _engine).ToString());
        }
    }
}
