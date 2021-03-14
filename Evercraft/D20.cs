using System;

namespace Evercraft
{
    public class D20 : IDice
    {
        private readonly Random random = new Random();
        
        public int Roll()
        {
            return random.Next(1, 21);
        }
    }
}