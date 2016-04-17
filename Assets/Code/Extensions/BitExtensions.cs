using System;

namespace Extensions
{
    public static class BitExtensions
    {
        public static byte SetBit(this byte b, int field, bool value)
        {
            if (value) //set value
                b = (byte)(b | field);
            else //clear value
                b = (byte)(b & (~field));

            return b;
        }

        public static bool HasBits(this byte b, int field)
        {
            return (b & field) > 0;
        }

        public static bool TryParse<TEnum>(string value, out TEnum result) where TEnum : struct, IConvertible
        {
            var retValue = value == null ? false : Enum.IsDefined(typeof(TEnum), value);
            result = retValue ? (TEnum)Enum.Parse(typeof(TEnum), value) : default(TEnum);
            return retValue;
        }

        public static int PopCount(this byte b)
        {
            int i = b;
            i = i - ((i >> 1) & 0x55555555);
            i = (i & 0x33333333) + ((i >> 2) & 0x33333333);
            return (((i + (i >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24;
        }
    }
}