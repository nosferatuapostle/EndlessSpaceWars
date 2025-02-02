using System;

namespace EndlessSpace
{
    public class BiomantesInvisabilityEffect : UnitEffect
    {
        public BiomantesInvisabilityEffect(Unit source) : base("Biomantes Invisability Effect", source, null, 0f, 10f, 0f)
        {
            source.AddKeyword("invisible");
            CheckInvisable();
        }

        public override void OnEffectEnd()
        {
            source.RemoveKeyword("invisible");
            CheckInvisable();
            base.OnEffectEnd();
        }

        void CheckInvisable() => (source as Character).CheckInvisable();
    }
}
