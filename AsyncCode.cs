using System;
using System.IO;
using System.Threading.Tasks;

namespace Test
{
        public abstract class Copier
        {
            public abstract void CopyBlockFromStream(Stream source, Stream destination);
        }

        public class NullCopier : Copier
        {
            public override void CopyBlockFromStream(Stream source, Stream destination)
            {
            }
        }

        public class MonitoredCopier : Copier
        {
            private object monitor = new object();
            private const int BlockSize = 8192;

            public override void CopyBlockFromStream(Stream source, Stream destination)
            {
                lock (monitor)
                {
                    var buf = new byte[BlockSize];
                    var nRead = source.Read(buf, 0, buf.Length);
                    destination.Write(buf, 0, nRead);
                }
            }
        }

        public class UserCode
        {
            /// <summary>
            /// The old code that we cannot really change.
            /// </summary>
            public void LegacyCopyBlock(Copier copier)
            {
                var buffer = GenerateRandomData();
                var src = new MemoryStream(buffer);
                var dst = new MemoryStream();
                copier.CopyBlockFromStream(src, dst);
            }

            /// <summary>
            /// New and exciting async code.
            /// </summary>
            public Task CopyBlockAsync(Copier copier)
            {
                //$TODO!
                throw new NotImplementedException();
            }

            private static byte[] GenerateRandomData()
            {
                var random = new Random(4711);
                var buf = new byte[random.Next(10000)];
                random.NextBytes(buf);
                return buf;
            }
        }
}