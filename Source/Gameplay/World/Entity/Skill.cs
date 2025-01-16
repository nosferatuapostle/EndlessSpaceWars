using Microsoft.Xna.Framework;

namespace EndlessSpace
{
    public abstract class Skill
    {
        protected Unit owner;

        public Skill(string name, Unit owner)
        {
            Name = name;
            this.owner = owner;
        }

        public string Name { get; private set; }
        public bool Active { get; set; } = false;

        public virtual void Update()
        {
            if (Active)
            {
                Use();
                Reset();
            }
        }

        protected abstract void Use();

        protected virtual void Reset()
        {
            Active = false;
        }
    }
}
