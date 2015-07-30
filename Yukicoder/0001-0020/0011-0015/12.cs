using System;
using System.Linq;
using System.Collections.Generic;
using Debug = System.Diagnostics.Debug;
using StringBuilder = System.Text.StringBuilder;
//using System.Numerics;
namespace Program
{
    public class Solver
    {
        public void Solve()
        {
            var n = sc.Integer();
            var a = sc.Integer(n);
            var goal = 0;
            foreach (var x in a) goal |= 1 << x;
            var isprime = Sieve(5000000);
            var primes = new List<int>();
            for (int i = 0; i < isprime.Length; i++)
                if (isprime[i]) primes.Add(i);
            var flag = new int[primes.Count];
            for (int i = 0; i < primes.Count; i++)
            {
                var v = primes[i];
                while (v > 0)
                {
                    flag[i] |= 1 << (v % 10);
                    v /= 10;
                }
            }
            var prev = new int[primes.Count];
            prev[0] = 1;
            for (int i = 1; i < primes.Count; i++)
                prev[i] = primes[i - 1] + 1;
            var next = new int[primes.Count];
            next[primes.Count - 1] = 5000000;
            for (int i = 0; i < primes.Count - 1; i++)
                next[i] = primes[i + 1] - 1;
            var ptr = 0;
            var max = -1;
            while (ptr < primes.Count)
            {
                var i = ptr;
                var now = 0;
                while (i < primes.Count)
                {
                    now |= flag[i];
                    if (now == goal)
                        max = Math.Max(max, next[i] - prev[ptr]);
                    if ((now | goal) > goal) break;
                    i++;
                }
                ptr = i + 1;
            }
            IO.Printer.Out.WriteLine(max);
        }
        static public bool[] Sieve(int p)
        {
            var isPrime = new bool[p + 1];
            for (int i = 2; i <= p; i++) isPrime[i] = true;
            for (int i = 2; i * i <= p; i++)
                if (!isPrime[i]) continue;
                else for (int j = i * i; j <= p; j += i)
                        isPrime[j] = false;
            return isPrime;
        }

        internal IO.StreamScanner sc = new IO.StreamScanner(Console.OpenStandardInput());
        static T[] Enumerate<T>(int n, Func<T> f) { var a = new T[n]; for (int i = 0; i < n; ++i) a[i] = f(); return a; }
        static T[] Enumerate<T>(int n, Func<int, T> f) { var a = new T[n]; for (int i = 0; i < n; ++i) a[i] = f(i); return a; }
    }
}

#region Ex
namespace Program.IO
{
    using System.IO;
    using System.Linq;
    public class Printer : StreamWriter
    {
        static Printer()
        {
            Out = new Printer(Console.OpenStandardOutput()) { AutoFlush = false };
        }
        public static Printer Out { get; set; }
        public override IFormatProvider FormatProvider { get { return System.Globalization.CultureInfo.InvariantCulture; } }
        public Printer(System.IO.Stream stream) : base(stream, new System.Text.UTF8Encoding(false, true)) { }
        public void Write<T>(string format, IEnumerable<T> source) { base.Write(format, source.OfType<object>().ToArray()); }
        public void WriteLine<T>(string format, IEnumerable<T> source) { base.WriteLine(format, source.OfType<object>().ToArray()); }
    }
    public class StreamScanner
    {
        public StreamScanner(Stream stream) { str = stream; }
        private readonly Stream str;
        private readonly byte[] buf = new byte[1024];
        private int len, ptr;
        public bool isEof = false;
        public bool IsEndOfStream { get { return isEof; } }
        private byte read()
        {
            if (isEof) return 0;
            if (ptr >= len) { ptr = 0; if ((len = str.Read(buf, 0, 1024)) <= 0) { isEof = true; return 0; } }
            return buf[ptr++];
        }
        public char Char() { byte b = 0; do b = read(); while (b < 33 || 126 < b); return (char)b; }
        public char[] Char(int n) { var a = new char[n]; for (int i = 0; i < n; i++) a[i] = Char(); return a; }
        public string Scan()
        {
            var sb = new StringBuilder();
            for (var b = Char(); b >= 33 && b <= 126; b = (char)read())
                sb.Append(b);
            return sb.ToString();
        }
        public long Long()
        {
            if (isEof) return long.MinValue;
            long ret = 0; byte b = 0; var ng = false;
            do b = read();
            while (b != '-' && (b < '0' || '9' < b));
            if (b == '-') { ng = true; b = read(); }
            for (; true; b = read())
            {
                if (b < '0' || '9' < b)
                    return ng ? -ret : ret;
                else ret = ret * 10 + b - '0';
            }
        }
        public int Integer() { return (isEof) ? int.MinValue : (int)Long(); }
        public double Double() { return double.Parse(Scan(), System.Globalization.CultureInfo.InvariantCulture); }
        private T[] enumerate<T>(int n, Func<T> f)
        {
            var a = new T[n];
            for (int i = 0; i < n; ++i) a[i] = f();
            return a;
        }

        public string[] Scan(int n) { return enumerate(n, Scan); }
        public double[] Double(int n) { return enumerate(n, Double); }
        public int[] Integer(int n) { return enumerate(n, Integer); }
        public long[] Long(int n) { return enumerate(n, Long); }
        public void Flush() { str.Flush(); }

    }
}
static class Ex
{
    static public string AsString(this IEnumerable<char> ie) { return new string(System.Linq.Enumerable.ToArray(ie)); }
    static public string AsJoinedString<T>(this IEnumerable<T> ie, string st = " ") { return string.Join(st, ie); }
    static public void Main()
    {
        var solver = new Program.Solver();
        solver.Solve();
        Program.IO.Printer.Out.Flush();
    }
}
#endregion