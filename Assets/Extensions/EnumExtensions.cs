using System;

namespace Extensions
{
    public static class EnumExtensions
    {
        public static bool HasFlag(this Enum variable, Enum value)
        {
            byte num = Convert.ToByte(value);
            return (Convert.ToByte(variable) & num) == num;
        }
    }
}