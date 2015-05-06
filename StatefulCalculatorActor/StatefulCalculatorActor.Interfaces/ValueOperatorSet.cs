using System.Runtime.Serialization;

namespace StatefulCalculatorActor.Interfaces
{
    [DataContract]
    public class ValueOperatorSet
    {
        [DataMember]
        public double Value { get; set; }

        [DataMember]
        public CalcOperator Operator { get; set; }
    }
}
