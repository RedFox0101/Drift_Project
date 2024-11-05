namespace Game.Data.DataFields.Base
{
    // [Serializable]
    public abstract class IntData : IDataField<int>
    {
        public void IncreaseValue(int value)
        {
            SetValue(_value + value);
        }
        
        public void DecreaseValue(int value)
        {
            SetValue(_value - value);
        }
    }
}