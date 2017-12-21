using System;
using System.Text;

namespace Quickford
{
    public class Base32
    {
        // Encoding constants.
        const ulong StopBit = 1ul << QuadShift;
        const int QuadShift = 60;
        const int QuadReset = 4;
        const int FiveShift = 59;
        const int FiveReset = 5;

        public static string Encode(ulong input)
        {
            if (input == 0) { return "0"; }

            var output = new StringBuilder();

            var firstFourBits = input >> QuadShift;
            input <<= QuadReset;
            input |= 1;

            if (firstFourBits > 0)
            {
                // In this case, we have no leading zeros to deal with.
                output.Append(Encoding.Uppercase[firstFourBits]);
            }
            else
            {
                // In this case, eat any leading zeros so that we don't end up encoding them.
                // Zeros must be consumed in groups of five.
                while (input >> FiveShift == 0)
                {
                    input <<= FiveReset;
                }
            }

            while (input != StopBit)
            {
                output.Append(Encoding.Uppercase[input >> FiveShift]);
                input <<= FiveReset;
            }

            return output.ToString();
        }

        // Decoding constants.
        const byte IntOffset = (byte)'0';
        const byte CaseFlag = 32;
        const ulong Base = 0x20;
        const int PlaceShift = 5;

        public static ulong? Decode(string input)
        {
            // This is obviously not a number.
            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            // Encoded ulong values are never more than 13 digits.
            if (input.Length > 13)
            {
                return null;
            }

            var place = (ulong)Math.Pow(Base, input.Length - 1);
            var n = 0ul;
            for (var i = 0; i < input.Length; ++i)
            {
                if (ToNormalDigit(input[i], out var digit))
                {
                    n += digit * place;
                    place >>= PlaceShift;
                }
                else
                {
                    return null;
                }
            }
            return n;
        }

        private static bool ToNormalDigit(char u, out byte digit)
        {
            switch (u)
            {
                // First we normalize O and o to 0
                case 'O':
                case 'o':
                    digit = 0;
                    return true;

                // Then I, i, L, and l to 1
                case 'I':
                case 'i':
                case 'L':
                case 'l':
                    digit = 1;
                    return true;

                // U and u are useful only for check digits, which are not implemented. However,
                // it is necessary to filter these out separately from other check digits because
                // these fall within the alphabetic range we otherwise convert.
                case 'U':
                case 'u':
                    digit = 0;
                    return false;

                default:
                    
                    if (u >= '0' && u <= '9')
                    {
                        digit = (byte)(u - IntOffset);
                        return true;
                    }

                    var searchIndex = Array.BinarySearch(Encoding.Uppercase, (char)(u & ~CaseFlag));
                    if (searchIndex >= 0)
                    {
                        digit = (byte)searchIndex;
                        return true;
                    }
                    else
                    {
                        digit = 0;
                        return false;
                    }
            }
        }
    }
}
