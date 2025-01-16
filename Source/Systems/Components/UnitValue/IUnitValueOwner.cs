namespace EndlessSpace
{
    public interface IUnitValueOwner
    {
        float GetUnitValue(UnitValue value);
        float GetBaseUnitValue(UnitValue value);
        void SetBaseUnitValue(UnitValue value, float base_value);
        void ModifyUnitValue(UnitValue value, float amount);
        void RestoreUnitValue(UnitValue value, float amount);
    }
}
