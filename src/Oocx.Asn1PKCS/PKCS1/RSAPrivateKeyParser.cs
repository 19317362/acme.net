using System;
using System.IO;
using System.Linq;
using System.Text;

using Oocx.Asn1PKCS.Asn1BaseTypes;
using Oocx.Asn1PKCS.Parser;

namespace Oocx.Asn1PKCS.PKCS1
{
    public class RSAPrivateKeyParser
    {
        private readonly Asn1Parser parser;

        public RSAPrivateKeyParser(Asn1Parser parser)
        {
            this.parser = parser ?? throw new ArgumentNullException(nameof(parser));
        }

        public RSAPrivateKey ParsePem(string pem)
        {
            using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(pem)))
            {
                return ParsePem(stream);
            }
        }

        public RSAPrivateKey ParsePem(Stream input)
        {
            var der = DecodePem(input);

            using (var derStream = new MemoryStream(der))
            {
                return ParseDer(derStream);
            }
        }

        public RSAPrivateKey ParseDer(MemoryStream derStream)
        {
            var asn1 = (Sequence) parser.Parse(derStream).First();

            var ints = asn1.Children.Cast<Integer>().ToArray();

            return new RSAPrivateKey(ints[0], ints[1], ints[2], ints[3], ints[4], ints[5], ints[6], ints[7], ints[8]);
        }

        private static byte[] DecodePem(Stream input)
        {
            return PEM.Decode(input, PEM.RSAPrivateKey);
        }
    }
}