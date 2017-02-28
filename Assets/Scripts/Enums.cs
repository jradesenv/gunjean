using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Enums
{
    public enum FloatingNumberType
    {
        Damage,
        Heal
    }

    public class Items
    {
        public enum Type
        {
            Money,
            Health
        }

        public enum CollectType
        {
            Instant,
            Collect
        }

        public enum Rarity
        {
            Common = 9,
            SemiRare = 5,
            Rare = 3,
            VeryRare = 1
        }
    }

    public class Damage
    {
        public enum Type
        {
            Normal
        }
    }

    public class Angles
    {
        public enum X
        {
            Right,
            Left
        }

        public enum Y
        {
            Top,
            Bottom
        }
    }
}
