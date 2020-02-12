using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tt1
{
    public class Neuron
    {
        double N = 0.1; // кое-нт обучения
        public static int CountSynops;
        public Synapse[] Synapses = new Synapse[CountSynops]; // 3 - количество синапсов в нашем нейроне

        public Neuron()
        {
            Synapses[0] = new Synapse() { Input = 0.5 };
            Synapses[1] = new Synapse();
            Synapses[2] = new Synapse();
        }

        public double Error { get; set; }

        public double RealOutput { get; set; }

        public double GetOutput(double x1, double x2)
        {
            Synapses[1].Input = x1;
            Synapses[2].Input = x2;

            double output = 0;
            for (int count = 0; count < Synapses.Length; count++)
                output += Synapses[count].Output;

            RealOutput = output;
            return output;
        }

        public void RecalculateWeights(double IdealOutput)
        {
            Error = Math.Pow(IdealOutput - RealOutput, 2);
            if (Error <= 0.00001) return;
            for (int index = 0; index < Synapses.Length; index++)
                Synapses[index].Weight = Synapses[index].Weight + N * (IdealOutput - Synapses[index].Output) * Synapses[index].Input;

        }
    }
   public class Synapse
    {
        double input = 0;

        public double Input
        {
            get
            {
                return input;
            }
            set
            {
                input = value;
                Output = value * Weight;
            }
        }

        public double Output { get; set; }

        double weight = 0.1;

        public double Weight
        {
            get
            {
                return weight;
            }
            set
            {
                weight = value;
                Output = Input * value;
            }
        }
    }
}
