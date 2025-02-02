using System;

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

    public class UnitValueInfo
    {
        UnitValue value;
        public float CurrentValue { get; private set; }
        public float BaseValue { get; private set; }
        public float MaxValue { get; private set; }

        public UnitValueInfo(UnitValue value, float base_value, float max_value = float.MaxValue)
        {
            this.value = value;
            BaseValue = base_value;
            CurrentValue = base_value;
            MaxValue = max_value;
        }

        public void ModifyValue(float amount)
        {
            CurrentValue = Math.Clamp(CurrentValue + amount, 0, MaxValue);
        }

        public void SetValue(float value)
        {
            CurrentValue = Math.Clamp(value, 0, MaxValue);
        }

        public void SetMaxValue(float max_value)
        {
            MaxValue = max_value;
            CurrentValue = Math.Clamp(CurrentValue, 0, MaxValue);
        }
    }
}
