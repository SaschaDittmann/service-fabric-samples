using Microsoft.JScript;
using Microsoft.JScript.Vsa;

namespace Calculator.Common
{
    public class CalcEngine
    {
        private static readonly VsaEngine _engine = VsaEngine.CreateEngine();
        public static double Evaluate(string expr)
        {
            // error checking etc removed for brevity
            return double.Parse(Eval.JScriptEvaluate(expr, _engine).ToString());
        }
    }
}
