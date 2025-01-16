using Microsoft.Xna.Framework;

namespace EndlessSpace
{
    public class Summoned : Unit
    {
        public Summoned(string name, string path, Vector2 position, Vector2 size) : base(new string[] { path }, position, size, UnitFaction.Summoned, null)
        {
            Name = name;
            AddAnimation(0, "idle", 1);
            AddAnimation(0, "death", 1, false);
        }
    }
}
