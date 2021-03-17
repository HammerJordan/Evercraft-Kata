using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;
using Xunit.Extensions;

namespace Evercraft.Test
{
    public class StatTests
    {
        public static IEnumerable<object[]> GetData()
        {
            int mod = -5;
            foreach (var value in Enumerable.Range(1, 20))
            {
                if (value % 2 == 0)
                    mod++;
                yield return new object[] {value, mod};
            }
        }

        [Theory]
        [MemberData(nameof(GetData))]
        public void StatValue_ReturnsRightModifierValue(int value, int mod)
        {
            Stat s = new Stat(value);
            s.Modifier.Should().Be(mod);
        }
    }
}