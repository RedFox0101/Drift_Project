namespace Game.Data.DataFields.Base
{
    // [Serializable]
    public class FloatData : IDataField<float>
    {
        public void IncreaseValue(float value)
        {
            SetValue(_value + value);
        }
        
        public void DecreaseValue(float value)
        {
            SetValue(_value - value);
        }
    }
}