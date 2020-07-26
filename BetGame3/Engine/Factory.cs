using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetGame3.Engine
{
   public static class Factory
    {
        public static Punter GetPunterInstance(string classname)
        {
            if (classname.Equals("Ava"))
            {
                return new Ava() { Cash = 50 };
            }
            else if (classname.Equals("Harper"))
            {
                return new Harper() { Name = "Harper", Cash = 50 };
            }
            else if (classname.Equals("Willow"))
            {
                return new Willow() { Name = "Willow", Cash = 50 };
            }
            else
            {
                return null;
            }
        }
    }
}
