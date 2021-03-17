using System.Collections.Generic;
using System.Linq;

namespace Evercraft
{
    public class DiceRoll
    {
        public int Roll { get; }
        public IList<int> Modifiers { get; }
        public int Total => Roll + Modifiers.Sum();
        public bool IsNatural20 => Roll == 20;

        public DiceRoll(int roll, IList<int> modifiers = null)
        {
            Roll = roll;
            Modifiers = modifiers ?? new List<int>();
        }
        
       
    }
}