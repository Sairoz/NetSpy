using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfectedApplication.Helper
{
    public static class NumbersHelper
    {
        public static int GenerateNumber(int min, int max)
        {
            Random rnd = new Random();
            return rnd.Next(min, max);
        }

        public static bool IsFloat(float number)
        {
            if (number != (int)number)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
