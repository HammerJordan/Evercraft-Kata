using System;
using Xunit;
using FluentAssertions;

namespace Evercraft.Test
{
    public class CharacterTests
    {
        [Fact]
        public void Name_ShouldBeAssigned()
        {
            const string name = "Wizard";
            var character = new Character() {Name = name};

            character.Name.Should().BeEquivalentTo(name);
        }
    }
}