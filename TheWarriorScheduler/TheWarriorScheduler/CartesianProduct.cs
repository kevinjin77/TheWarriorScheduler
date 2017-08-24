using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWarriorScheduler
{
    /* From http://www.felicepollano.com/2011/08/12/HelperClassForTheCartesianProductOfMoreThanTwoArrays.aspx */
    public class CartesianProduct<T>
    {
        int[] lengths;
        T[][] arrays;
        public CartesianProduct(params T[][] arrays)
        {
            lengths = arrays.Select(k => k.Length).ToArray();
            if (lengths.Any(l => l == 0))
                throw new ArgumentException("Zero length array unhandled.");
            this.arrays = arrays;
        }
        public IEnumerable<T[]> Get()
        {
            int[] walk = new int[arrays.Length];
            int x = 0;
            yield return walk.Select(k => arrays[x++][k]).ToArray();
            while (Next(walk))
            {
                x = 0;
                yield return walk.Select(k => arrays[x++][k]).ToArray();
            }

        }
        private bool Next(int[] walk)
        {
            int whoIncrement = 0;
            while (whoIncrement < walk.Length)
            {
                if (walk[whoIncrement] < lengths[whoIncrement] - 1)
                {
                    walk[whoIncrement]++;
                    return true;
                }
                else
                {
                    walk[whoIncrement] = 0;
                    whoIncrement++;
                }
            }
            return false;
        }
    }
}
