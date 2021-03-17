using System;
using System.Reflection.Metadata.Ecma335;

namespace Evercraft
{
    public class CharacterBuilder
    {
        public int ArmorClass { get; set; } = 10;
        public int MaxHitPoints { get; set; } = 5;
        public string Name { get; set; } = "Hero";
        public AlignmentType Alignment { get; set; } = AlignmentType.Neutral;
        public int Xp { get; set; } = 0;
        public int Strength { get; set; } = 10;
        public int Dexterity { get; set; } = 10;
        public int Constitution { get; set; } = 10;
        public int Wisdom { get; set; } = 10;
        public int Intelligence { get; set; } = 10;
        public int Charisma { get; set; } = 10;

        public CharacterBuilder SetArmorClass(int armorClass)
        {
            ArmorClass = armorClass;
            return this;
        }

        public CharacterBuilder SetHp(int hp)
        {
            MaxHitPoints = hp;
            return this;
        }

        public CharacterBuilder SetName(string name)
        {
            Name = name;
            return this;
        }

        public CharacterBuilder SetAlignment(AlignmentType alignment)
        {
            Alignment = alignment;
            return this;
        }

        public CharacterBuilder SetXp(int xp)
        {
            Xp = xp;
            return this;
        }

        public CharacterBuilder SetStat(StatTypes type, int value)
        {
            switch (type)
            {
                case StatTypes.Strength:
                    Strength = value;
                    break;
                case StatTypes.Dexterity:
                    Dexterity = value;
                    break;
                case StatTypes.Constitution:
                    Constitution = value;
                    break;
                case StatTypes.Wisdom:
                    Wisdom = value;
                    break;
                case StatTypes.Intelligence:
                    Intelligence = value;
                    break;
                case StatTypes.Charisma:
                    Charisma = value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            return this;
        }

        public Character Build()
        {
            return new Character(this);
        }
        
        public static implicit operator Character(CharacterBuilder builder)
        {
            return builder.Build();
        }
    }
}