using Xunit;

namespace Quickford.Test
{
    public class Decoding
    {
        [Fact]
        public void Zero_length_strings_fail()
        {
            Assert.False(Base32.Decode("").HasValue);
        }

        [Fact]
        public void Long_strings_fail()
        {
            var input = "12345678910121";
            Assert.False(Base32.Decode(input).HasValue);
        }

        [Fact]
        public void Invalid_bytes_fail()
        {
            var input = "fZZ!2";
            Assert.False(Base32.Decode(input).HasValue);
        }

        [Fact]
        public void Zero_becomes_zero()
        {
            Assert.True(0ul == Base32.Decode("0").Value);
        }

        [Fact]
        public void Large_values_become_large_values()
        {
            Assert.True(65535ul == Base32.Decode("1ZZZ"));
            Assert.True(65535ul == Base32.Decode("1zzz"));
        }

        [Fact]
        public void Oo_becomes_zero()
        {
            Assert.True(0 == Base32.Decode("O").Value);
            Assert.True(0 == Base32.Decode("o").Value);
        }

        [Fact]
        public void IiLl_becomes_one()
        {
            Assert.True(1 == Base32.Decode("I").Value);
            Assert.True(1 == Base32.Decode("i").Value);
            Assert.True(1 == Base32.Decode("L").Value);
            Assert.True(1 == Base32.Decode("l").Value);
        }

        [Fact]
        public void Z_equals_31()
        {
            Assert.True(31 == Base32.Decode("Z").Value);
            Assert.True(31 == Base32.Decode("z").Value);
        }

        [Fact]
        public void Four_z_q_works()
        {
            Assert.True(5111 == Base32.Decode("4zq").Value);
            Assert.True(5111 == Base32.Decode("4ZQ").Value);
        }

        [Fact]
        public void Max_value_works()
        {
            Assert.True(18446744073709551615ul == Base32.Decode("fzzzzzzzzzzzz").Value);
        }

        [Fact]
        public void U_produces_a_null_instead_of_a_crash()
        {
            Assert.False(Base32.Decode("iVuv").HasValue);
            Assert.False(Base32.Decode("iVUv").HasValue);
        }
    }
}
