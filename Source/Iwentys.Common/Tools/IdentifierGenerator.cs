namespace Iwentys.Common
{
    public class IdentifierGenerator
    {
        private int _currentValue;

        public int Next()
        {
            _currentValue++;
            return _currentValue;
        }
    }
}