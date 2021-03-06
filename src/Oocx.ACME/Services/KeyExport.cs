﻿using System;
using System.IO;
using System.Security.Cryptography;
using Oocx.Asn1PKCS;
using Oocx.Asn1PKCS.PKCS1;

namespace Oocx.ACME.Services
{
    public class KeyExport
    {
        private readonly string basePath;

        public KeyExport(string basePath)
        {
            this.basePath = basePath ?? throw new ArgumentNullException(nameof(basePath));
        }

        public void Save(RSAParameters key, string keyName, KeyFormat format)
        {
            var keyFileName = Path.Combine(basePath, $"{keyName}." + GetExtension(format));

            var privateKey = new RSAPrivateKey(key);

            using (var stream = File.OpenWrite(keyFileName))
            {
                privateKey.WriteTo(stream, format);
            }
        }

        private static string GetExtension(KeyFormat format)
        {
            switch (format)
            {
                case KeyFormat.DER       : return "der";
                case KeyFormat.DotNetXml : return "xml";
                case KeyFormat.PEM       : return "pem";
                default                  : throw new Exception("Unsupported format:" + format);
            }
        }
    }
}