using System;
using System.Text;

namespace Quickford
{
    public class Base32
    {
        // Encoding constants.
        const int QuadShift = 60;
        const int QuadReset = 4;
        const int FiveShift = 59;
        const int FiveReset = 5;
        const ulong StopBit = 1ul << QuadShift;

        /// <summary>
        /// Encodes an unsigned long as a Crockford Base32 string.
        /// </summary>
        /// <param name="input">Unsigned long to be encoded</param>
        /// <returns>Base32-encoded string</returns>
        public static string Encode(ulong input)
        {
            if (input == 0)
            {
                return "0";
            }

            var buffer = new StringBuilder(13);
            Encode(input, buffer);
            return buffer.ToString();
        }

        public static void Encode(ulong input, StringBuilder buffer)
        {
            if (input == 0)
            {
                buffer.Append('0');
                return;
            }

            var firstFourBits = input >> QuadShift;
            input <<= QuadReset;
            input |= 1;

            if (firstFourBits > 0)
            {
                // In this case, we have no leading zeros to deal with.
                buffer.Append(Encoding.Uppercase[firstFourBits]);
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
                buffer.Append(Encoding.Uppercase[input >> FiveShift]);
                input <<= FiveReset;
            }
        }

        // Decoding constants.
        const byte IntOffset = (byte)'0';
        const byte CaseFlag = 32;
        const ulong Base = 0x20;
        const int PlaceShift = 5;

        /// <summary>
        /// Attempts to decode a string as Crockford Base32.
        /// </summary>
        /// <param name="input">String to be decoded</param>
        /// <param name="decoded">Decoded output</param>
        /// <returns>Boolean indicating success or failure of the decoding operation</returns>
        public static bool TryDecode(string input, out ulong decoded)
        {
            decoded = 0ul;

            if (string.IsNullOrWhiteSpace(input) || input.Length > 13)
            {
                return false;
            }

            var place = (ulong)Math.Pow(Base, input.Length - 1);
            for (var i = 0; i < input.Length; ++i)
            {
                if (ToNormalDigit(input[i], out var digit))
                {
                    decoded += digit * place;
                    place >>= PlaceShift;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Attempts to decode a string as Crockford Base32.
        /// </summary>
        /// <param name="input">String to be decoded</param>
        /// <returns>Unsigned long or null</returns>
        public static ulong? Decode(string input)
        {
            return TryDecode(input, out var decoded) ? decoded as ulong? : null;
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

                    var result = Encoding.ValueMapping[u];
                    if (result > -1)
                    {
                        digit = (byte)result;
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
