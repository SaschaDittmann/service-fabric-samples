using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using StatefulCalculatorActor.Interfaces;
using Microsoft.ServiceFabric;
using Microsoft.ServiceFabric.Actors;

namespace StatefulCalculatorActor
{
    [DataContract]
    public class StatefulCalculatorActorState
    {
        public StatefulCalculatorActorState()
        {
            ValueOperatorSets = new List<ValueOperatorSet>();
        }

        [DataMember]
        public List<ValueOperatorSet> ValueOperatorSets { get; set; }
    }
}