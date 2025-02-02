namespace EndlessSpace
{
    public class IronCorpseOverloadEffect : UnitEffect
    {
        float total_magnitude;

        public IronCorpseOverloadEffect(Unit source) : base("Iron Corpse Overload Effect", source, null, 1f, 5f, 0f)
        {
            total_magnitude = 0f;
            float base_magnitude = source.GetBaseUnitValue(UnitValue.Magnitude);
            source.SetBaseUnitValue(UnitValue.Magnitude, base_magnitude + magnitude);
            total_magnitude += magnitude;
        }

        public override void OnEffectEnd()
        {
            float base_magnitude = source.GetBaseUnitValue(UnitValue.Magnitude);
            source.SetBaseUnitValue(UnitValue.Magnitude, base_magnitude - total_magnitude);
            total_magnitude = 0f;
            base.OnEffectEnd();
        }
    }
}
