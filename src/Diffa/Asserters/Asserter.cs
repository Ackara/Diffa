using System;
using System.Linq;
using System.Reflection;

namespace Acklann.Diffa.Asserters
{
    internal class Asserter : IAsserter
    {
        public Asserter()
        {
            if (TryCreateMSTestAsserter(out _assert)) return;
            else if (TryCreateXUnitAsserter(out _assert)) return;
            else
            {
                string supportedFrameworks = null;
                new DllNotFoundException($"{nameof(Diffa)} was unable to load a compatible test adapter. Currently only the following frameworks are supported ({supportedFrameworks}). Visit 'https://github.com/Ackara/Diffa/issues' to request support.");
            }
        }

        public Asserter(AssertKind frameworkKind)
        {
            _assert = GetAssertionObject(frameworkKind);
        }

        public static bool TryCreateMSTestAsserter(out Action<object[]> asserter)
        {
            try
            {
                var mstest = Type.GetType("Microsoft.VisualStudio.TestTools.UnitTesting.Assert, Microsoft.VisualStudio.QualityTools.UnitTestFramework");
                asserter = delegate (object[] args) { mstest.InvokeMember("AreEqual", (BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod), null, null, args); };
                return true;
            }
            catch (Exception ex)
            {
                asserter = delegate (object[] args) { };
                System.Diagnostics.Debug.WriteLine($"");
                System.Diagnostics.Debug.WriteLine(ex.GetType().Name);
            }

            return false;
        }

        public static bool TryCreateXUnitAsserter(out Action<object[]> asserter)
        {
            try
            {
                var xunit = Type.GetType("Xunit.Assert, xunit.assert");
                MethodInfo method = xunit.GetMethods().First(m => m.Name == "Equal" && m.IsGenericMethod && m.GetParameters().Length == 2);
                asserter = delegate (object[] args) { method.MakeGenericMethod(typeof(string)).Invoke(null, args); };
                return true;
            }
            catch (Exception ex)
            {
                asserter = delegate (object[] args) { };
                System.Diagnostics.Debug.WriteLine($"");
                System.Diagnostics.Debug.WriteLine(ex.GetType().Name);
            }

            return false;
        }

        public void Assert(object expected, object actual)
        {
            _assert.Invoke(new[] { expected, actual });
        }

        public Action<object[]> GetAssertionObject(AssertKind frameworkIdentifier)
        {
            Action<object[]> action;

            switch (frameworkIdentifier)
            {
                default:
                case AssertKind.MSTest:
                    TryCreateMSTestAsserter(out action);
                    break;
            }

            return action;
        }

        #region Private Members

        private readonly Action<object[]> _assert;

        #endregion Private Members
    }
}