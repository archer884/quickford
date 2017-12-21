using System;
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
    }
}
