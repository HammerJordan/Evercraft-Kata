using System.Collections.Generic;
using System.Linq;

namespace Evercraft
{
    public class DiceRoll
    {
        public int Roll { get; }
        public IEnumerable<int> Modifiers { get; }
        public int Total => Roll + Modifiers.Sum();
        public bool Natural20 => Roll == 20;

        public DiceRoll(int roll, IEnumerable<int> modifiers)
        {
            Roll = roll;
            Modifiers = modifiers ?? new List<int>();
        }
    }
}