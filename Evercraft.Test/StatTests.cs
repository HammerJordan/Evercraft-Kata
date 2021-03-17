using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;
using Xunit.Extensions;

namespace Evercraft.Test
{
    public class StatTests
    {
        public static List<Tuple<int, int>> values = new List<Tuple<int, int>>();

        public StatTests()
        {
            int mod = -5;
            for (int i = 1; i < 21; i++)
            {
                if (i % 2 == 0)
                    mod++;
                values.Add(new(i,mod));
            }
        }

        [Fact]
        public void StatValue_ReturnsRightModifierValue()
        {
            foreach (var value in values)
            {
                Stat s = new Stat(value.Item1);
                s.Modifier.Should().Be(value.Item2);
            }
        }
    }
}