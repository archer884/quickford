# quickford

> Base32 encoding for 64-bit values.

[Crockford Base32 Encoding](https://www.crockford.com/wrmg/base32.html) is most commonly used to make numeric identifiers slightly more user-resistant. Similar to [Hashids](http://hashids.org/), the purpose here is to make the identifiers shorter and less confusing. Unlike Hashids, Crockford Base32 does nothing to conceal the real value of the number (beyond the actual encoding, anyway) and the fact that they are sequential is still pretty obvious when you see consecutive identifiers side by side.

This library does not support encoding and decoding of arbitrary data. Additionally, the spec supports the idea of check digits, but this library currently does not.

**The primary purpose of this library is to provide high performance, user-resistant encoding of numeric identifiers.** To that end, both encoding and decoding are, in fact, pretty darn fast--an average of eight times faster than the most popular alternative on nuget.org. Additionally, no initialization is required; all methods are static.

This library is a port of [crockford](https://github.com/archer884/crockford) for Rust. The present API is stable, but it may be expanded in the future to encompass more of what crockford offers in terms of efficiency for encoding.

## Usage

### Encoding

Encoding is a one-step process.

```csharp
var x = Base32.Encode(5111);
Assert.Equal("4ZQ", x);
```

If you want lowercase, then... Well, tough. However, we do now support encoding to a buffer of your choice rather than a new one created in the function. Read on to learn about plan B...

### Decoding

Decoding is a two-step process. This is because you can feed any string to the decoder, and the decoder will return an error if you try to convince it that `"Hello, world!"` is a number. (Hint: it isn't.)

```csharp
var x = Base32.Decode("4zq");
var y = Base32.Decode("4ZQ");

Assert.Equal(5111, x.Value);
Assert.Equal(5111, y.Value);
```

So, step one is to call the decode function. Step two is to match/verify/unwrap/throw away the output.

## License

Licensed under either of

* Apache License, Version 2.0 ([LICENSE-APACHE][apc] or http://www.apache.org/licenses/LICENSE-2.0)
* MIT License ([LICENSE-MIT][mit] or http://opensource.org/licenses/MIT)

at your option.

### Contribution

Unless you explicitly state otherwise, any contribution intentionally submitted for inclusion in the work by you, as defined in the Apache-2.0 license, shall be dual licensed as above, without any additional terms or conditions.

[apc]: https://github.com/archer884/quickford/blob/master/LICENSE-APACHE
[mit]: https://github.com/archer884/quickford/blob/master/LICENSE-MIT
