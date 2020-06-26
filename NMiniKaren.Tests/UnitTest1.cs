using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMiniKanren;
using System;

namespace NMiniKanren.Tests
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void TestEq()
        {
            // k�ṩNMiniKanren�ķ�����q�Ǵ�����δ֪������
            var res = KRunner.Run(null /* ������п��ܵĽ�� */, (k, q) =>
            {
                // q == 5 ���� q == 6
                return k.Any(
                    k.Eq(q, 5),
                    k.Eq(q, 6));
            });
            KRunner.PrintResult(res);  // ��������[5, 6]
        }


        [TestMethod]
        public void TestEq1()
        {
            // k�ṩNMiniKanren�ķ�����q�Ǵ�����δ֪������
            var res = KRunner.Run(1 /* ���1����� */, (k, q) =>
            {
                // q == 5 ���� q == 6
                return k.Any(
                    k.Eq(q, 5),
                    k.Eq(q, 6));
            });
            KRunner.PrintResult(res);  // ��������[5]
        }


        [TestMethod]
        public void TestEq0()
        {
            Console.WriteLine("======");
            KRunner.PrintResult(KRunner.Run(null, (k, q) =>
            {
                return k.Eq(q, 5);
            }));  // ���[5]
            Console.WriteLine("======");
            KRunner.PrintResult(KRunner.Run(null, (k, q) =>
            {
                return k.Eq(q, k.List(1, 2));
            }));  // ���[(1 2)]
            Console.WriteLine("======");
            KRunner.PrintResult(KRunner.Run(null, (k, q) =>
            {
                return k.Eq(k.List(1, q), k.List(1, 2));
            }));  // ���[2]
            Console.WriteLine("======");
            KRunner.PrintResult(KRunner.Run(null, (k, q) =>
            {
                return k.Eq(k.List(2, q), k.List(1, 2));
            }));  // ���[]
            Console.WriteLine("======");
            KRunner.PrintResult(KRunner.Run(null, (k, q) =>
            {
                return k.Fail;
            }));  // ���[]
            Console.WriteLine("======");
            KRunner.PrintResult(KRunner.Run(null, (k, q) =>
            {
                return k.Succeed;
            }));  // ���[_0]
            Console.WriteLine("======");
        }

        [TestMethod]
        public void TestAnd()
        {
            Console.WriteLine("======");
            KRunner.PrintResult(KRunner.Run(null, (k, q) =>
            {
                return k.All(
                    k.Eq(q, 1),
                    k.Eq(q, 2));
            }));  // ���[]
            Console.WriteLine("======");
            KRunner.PrintResult(KRunner.Run(null, (k, q) =>
            {
                var x = k.Fresh();
                var y = k.Fresh();
                return k.All(
                    k.Eq(x, 1),
                    k.Eq(y, x),
                    k.Eq(q, k.List(x, y)));
            }));  // ���[(1 1)]
            Console.WriteLine("======");
        }

        [TestMethod]
        public void TestOr()
        {
            Console.WriteLine("======");
            KRunner.PrintResult(KRunner.Run(null, (k, q) =>
            {
                return k.Any(
                    k.Eq(q, 5),
                    k.Eq(q, 6));
            }));  // ���[5, 6]
            Console.WriteLine("======");
            KRunner.PrintResult(KRunner.Run(null, (k, q) =>
            {
                var x = k.Fresh();
                var y = k.Fresh();
                return k.All(
                    k.Any(k.Eq(x, 5), k.Eq(y, 6)),
                    k.Eq(q, k.List(x, y)));
            }));  // ���[(5 _0), (_0 6)]
            Console.WriteLine("======");
        }

        [TestMethod]
        public void TestIf()
        {
            Console.WriteLine("======");
            KRunner.PrintResult(KRunner.Run(null, (k, q) =>
            {
                var x = k.Fresh();
                return k.All(
                    k.Eq(x, 1),
                    k.If(
                        k.Eq(x, 1),
                        k.Eq(q, 2),
                        k.Eq(q, 3)));
            }));  // ���[2]
            Console.WriteLine("======");
            KRunner.PrintResult(KRunner.Run(null, (k, q) =>
            {
                var x = k.Fresh();
                var y = k.Fresh();
                return k.All(
                    k.Eq(x, 1),
                    k.Eq(y, 2),
                    k.If(
                        k.Eq(x, 1),
                        k.Eq(x, y),
                        k.Succeed),
                    k.Eq(q, k.List(x, y)));
            }));  // ���[]
            Console.WriteLine("======");
            KRunner.PrintResult(KRunner.Run(null, (k, q) =>
            {
                var x = k.Fresh();
                var y = k.Fresh();
                return k.All(
                    k.Eq(x, 1),
                    k.Eq(y, 2),
                    k.If(
                        k.Eq(x, y),
                        k.Eq(x, 1),
                        k.Succeed),
                    k.Eq(q, k.List(x, y)));
            }));;  // ���[(1 2)]
            Console.WriteLine("======");
            KRunner.PrintResult(KRunner.Run(null, (k, q) =>
            {
                var x = k.Fresh();
                var y = k.Fresh();
                return k.All(
                    k.If(
                        k.Eq(x, y),
                        k.Eq(x, 1),
                        k.Succeed),
                    k.Eq(x, 1),
                    k.Eq(y, 2),
                    k.Eq(q, k.List(x, y)));
            }));;  // ���[]
            Console.WriteLine("======");
        }

        [TestMethod]
        public void TestSeq()
        {
            Console.WriteLine("======");
            KRunner.PrintResult(KRunner.Run(null, (k, q) =>
            {
                var x = k.Fresh();
                var y = k.Fresh();
                return k.All(
                    k.Any(k.Eq(x, 1), k.Eq(x, 2)),
                    k.Any(k.Eq(y, "a"), k.Eq(y, "b")),
                    k.Eq(q, k.List(x, y)));
            }));  // ���[(1 a), (1 b), (2 a), (2 b)]
            Console.WriteLine("======");
            KRunner.PrintResult(KRunner.Run(null, (k, q) =>
            {
                var x = k.Fresh();
                var y = k.Fresh();
                return k.Alli(
                    k.Any(k.Eq(x, 1), k.Eq(x, 2)),
                    k.Any(k.Eq(y, "a"), k.Eq(y, "b")),
                    k.Eq(q, k.List(x, y)));
            }));  // ���[(1 a), (2 a), (1 b), (2 b)]
            Console.WriteLine("======");
            KRunner.PrintResult(KRunner.Run(null, (k, q) =>
            {
                var x = k.Fresh();
                var y = k.Fresh();
                return k.All(
                    k.Any(k.Eq(x, 1), k.Eq(x, 2), k.Eq(x, 3)),
                    k.Any(k.Eq(y, "a"), k.Eq(y, "b")),
                    k.Eq(q, k.List(x, y)));
            }));  // ���[(1 a), (1 b), (2 a), (2 b), (3 a), (3 b)]
            Console.WriteLine("======");
            KRunner.PrintResult(KRunner.Run(null, (k, q) =>
            {
                var x = k.Fresh();
                var y = k.Fresh();
                return k.Alli(
                    k.Any(k.Eq(x, 1), k.Eq(x, 2), k.Eq(x, 3)),
                    k.Any(k.Eq(y, "a"), k.Eq(y, "b")),
                    k.Eq(q, k.List(x, y)));
            }));  // ���[(1 a), (2 a), (1 b), (3 a), (2 b), (3 b)]
            Console.WriteLine("======");
            KRunner.PrintResult(KRunner.Run(null, (k, q) =>
            {
                return k.Any(
                    k.Any(k.Eq(q, 1), k.Eq(q, 2)),
                    k.Any(k.Eq(q, 3), k.Eq(q, 4)));
            }));  // ���[1, 2, 3, 4]
            Console.WriteLine("======");
            KRunner.PrintResult(KRunner.Run(null, (k, q) =>
            {
                return k.Anyi(
                    k.Any(k.Eq(q, 1), k.Eq(q, 2)),
                    k.Any(k.Eq(q, 3), k.Eq(q, 4)));
            }));  // ���[1, 3, 2, 4]
            Console.WriteLine("======");
        }

        [TestMethod]
        public void TestSample()
        {
            KRunner.PrintResult(KRunner.Run(null, (k, q) =>
            {
                var x = k.Fresh();
                var y = k.Fresh();
                return k.All(
                    k.Any(k.Eq(x, 1), k.Eq(x, 2)),
                    k.Any(k.Eq(y, x), k.Eq(y, "b")),
                    k.Eq(q, k.List(x, y)));
            }));  // ���[(1 1), (1 b), (2 2), (2 b)]
        }

        [TestMethod]
        public void TestUnif()
        {
            KRunner.PrintResult(KRunner.Run(null, (k, q) =>
            {
                var x = k.Fresh();
                var y = k.Fresh();
                return k.All(
                    k.Eq(y, x),
                    k.Eq(q, k.List(x, y)),
                    k.Eq(x, 1));
            })); // ���[(1 1)]
        }

        [TestMethod]
        public void TestRenumber()
        {
            Console.WriteLine("======");
            KRunner.PrintResult(KRunner.Run(null, (k, q) =>
            {
                var x = k.Fresh();
                var y = k.Fresh();
                return k.Eq(q, k.List(x, y));
            }));
            Console.WriteLine("======");
            KRunner.PrintResult(KRunner.Run(null, (k, q) =>
            {
                var x = k.Fresh();
                var y = k.Fresh();
                return k.All(
                    k.Eq(x, y),
                    k.Eq(q, k.List(x, y)));
            }));
            Console.WriteLine("======");
        }

        [TestMethod]
        public void TestTree()
        {
            Console.WriteLine("======");
            KRunner.PrintResult(KRunner.Run(null, (k, q) =>
            {
                var x = k.Fresh();
                var y = k.Fresh();
                return k.All(
                    k.Eq(
                        k.List(1, k.List(2, 3), 4),
                        k.List(1, k.List(x, 3), y)),
                    k.Eq(q, k.List(x, y)));
            }));
            Console.WriteLine("======");
        }

    }
}
