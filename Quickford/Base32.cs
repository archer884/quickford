using System;
using System.Text;

namespace Quickford
{
    public class Base32
    {
        const ulong StopBit = 1 << QuadShift;

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

            int fiveBitGroups = 12;
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
                    --fiveBitGroups;
                }
            }

            for (var i = 0; i < fiveBitGroups; ++i)
            {
                output.Append(Encoding.Uppercase[input >> FiveShift]);
                input <<= FiveReset;
            }

            return output.ToString();
        }

        public static ulong? Decode(string input)
        {
            throw new NotImplementedException();
        }
    }
}
