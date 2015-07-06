using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rand = new Random();
            var results = new Dictionary<double, double>();

            for (var i = 0; i < 100000; ++i)
            {
                var r = GaussianRand(rand);

                r *= 10;
                r = Math.Floor(r);
                r /= 10;

                if (!results.ContainsKey(r))
                    results.Add(r, 1);
                else
                    results[r] += 1;
            }
            
            File.WriteAllLines("results.csv", results.OrderBy(pair => pair.Key)
                                                    .Select(pair => "\"val" + pair.Key + "\"," + pair.Value));
        }

        static double GaussianRand(Random rand, double mean = 0, double stdDev = 0.1)
        {
            double u1 = rand.NextDouble(); //these are uniform(0,1) random doubles
            double u2 = rand.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
            double randNormal = mean + stdDev * randStdNormal; //random normal(mean,stdDev^2)
            return randNormal;
        }
    }
}
