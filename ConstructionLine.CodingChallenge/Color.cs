using System;
using System.Collections.Generic;

namespace ConstructionLine.CodingChallenge
{
    public class Color : IEquatable<Color>
    {
        public Guid Id { get; }

        public string Name { get; }

        private Color(Guid id, string name)
        {
            Id = id;
            Name = name;
        }


        public static Color Red = new Color(Guid.NewGuid(), "Red");
        public static Color Blue = new Color(Guid.NewGuid(), "Blue");
        public static Color Yellow = new Color(Guid.NewGuid(), "Yellow");
        public static Color White = new Color(Guid.NewGuid(), "White");
        public static Color Black = new Color(Guid.NewGuid(), "Black");


        public static List<Color> All =
            new List<Color>
            {
                Red,
                Blue,
                Yellow,
                White,
                Black
            };

        public bool Equals(Color other)
        {
            if (other == null) return false;
            return Id == other.Id && Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as Color);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 13;
                hashCode = (hashCode * 397) ^ Id.GetHashCode();
                return hashCode;
            }
        }
    }
}