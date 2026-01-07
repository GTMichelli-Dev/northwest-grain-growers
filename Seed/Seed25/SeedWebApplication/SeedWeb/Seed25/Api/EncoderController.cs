using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Seed25.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class EncoderController : ControllerBase
    {
        public EncoderController()
        {
        }

        [HttpGet("encode/{value:int}")]
            public IActionResult Encode(int value)
            {
                try
                {
                    string code = Base32Crockford.Encode(value);
                    return Ok(new { code });
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    return BadRequest(new { error = ex.Message });
                }
        }

        [HttpGet("decode/{code}")]
        public IActionResult Decode(string code)
        {
            try
            {
                int value = Base32Crockford.Decode(code);
                return Ok(new { value });
            }
            catch (FormatException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }


    public static class Base32Crockford
    {
        // Crockford alphabet (no I, L, O, U)
        private const string Alphabet = "0123456789ABCDEFGHJKMNPQRSTVWXYZ";

        // Maps ambiguous inputs to canonical digits (O->0, I/L->1)
        private static int DecodeChar(char ch)
        {
            char c = char.ToUpperInvariant(ch);
            if (c == 'O') c = '0';
            if (c == 'I' || c == 'L') c = '1';

            int ix = Alphabet.IndexOf(c);
            if (ix >= 0) return ix;

            throw new FormatException($"Invalid Base-32 Crockford character: '{ch}'.");
        }

        public static string Encode(int value)
        {
            if (value < 0 || value > 999_999)
                throw new ArgumentOutOfRangeException(nameof(value), "Must be 0..999,999");

            if (value == 0) return "0000";

            Span<char> buf = stackalloc char[8]; // plenty for safety
            int i = buf.Length;
            int v = value;
            while (v > 0)
            {
                v = Math.DivRem(v, 32, out int rem);
                buf[--i] = Alphabet[rem];
            }
            var s = new string(buf[i..]);
            return s.PadLeft(4, '0'); // fixed width (4)
        }

        public static int Decode(string code)
        {
            if (string.IsNullOrWhiteSpace(code) || code.Length > 32)
                throw new ArgumentException("Code must be non-empty.");

            int value = 0;
            foreach (char ch in code)
            {
                if (ch == '-' || ch == ' ') continue; // allow separators
                int digit = DecodeChar(ch);
                checked { value = value * 32 + digit; }
            }
            if (value < 0 || value > 999_999)
                throw new FormatException("Decoded value out of expected range (0..999,999).");

            return value;
        }

        // Optional: simple mod-32 checksum char using the same alphabet
        public static char ChecksumChar(int value) => Alphabet[value % 32];
    }

}
