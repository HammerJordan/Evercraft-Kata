using System;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

namespace Evercraft
{
    public struct Stat
    {
        public Stat(int value)
        {
            Value = value;
        }

        public int Value { get; set; }
        public int Modifier => (int) Math.Floor((Value - 10) / 2f);

        public static implicit operator Stat(int value) => new Stat(value);
    }
}