using Xunit;

namespace Quickford.Test
{
    public class Encoding
    {
        [Fact]
        public void Zero_returns_zero()
        {
            Assert.Equal("0", Base32.Encode(0));
        }

        [Fact]
        public void Large_value_returns_correct_large_value()
        {
            Assert.Equal("1ZZZ", Base32.Encode(65535));
        }

        [Fact]
        public void Input_5111_is_4zq()
        {
            Assert.Equal("4ZQ", Base32.Encode(5111));
        }

        [Fact]
        public void Input_18446744073709551615_is_FZZZZZZZZZZZZ()
        {
            Assert.Equal("FZZZZZZZZZZZZ", Base32.Encode(18446744073709551615));
        }

        [Fact]
        public void Large_odd_number()
        {
            var input = 0b10000000_00000000_00000000_00000000_00000000_00000000_00000000_00000000;
            var output = Base32.Decode(Base32.Encode(input));

            Assert.Equal(input, output.Value);
        }

        [Fact]
        public void Tiny_number()
        {
            var input = 1ul;
            var output = Base32.Decode(Base32.Encode(input));

            Assert.Equal(input, output.Value);
        }
    }
}
