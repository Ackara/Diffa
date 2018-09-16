using Acklann.Diffa.Reporters;
using System;
using System.Diagnostics;
using System.Reflection;

namespace Acklann.Diffa.Resolution
{
    internal class StackTraceContextBuilder : IContextBuilder
    {
        public StackTraceContextBuilder()
        {
            foreach (string typeName in KnownTestFramework.TestMethodAttributeNames)
            {
                _testMethodAttribute = Type.GetType(typeName, throwOnError: false);
                if (_testMethodAttribute != null) return;
            }

            throw new NotSupportedException(Exceptions.ExceptionMessage.TestFrameworkNotSupported());
        }

        public TestContext Context
        {
            get { return _context.IsNotInstaniated ? CreateContext() : _context; }
        }

        public TestContext CreateContext()
        {
            MethodBase caller;
            var stack = new StackTrace(true);

            foreach (StackFrame frame in stack.GetFrames())
            {
                caller = frame.GetMethod();

                if (caller.GetCustomAttribute(_testMethodAttribute) != null)
                {
                    Attribute attr = caller.GetCustomAttribute(typeof(SaveFilesAtAttribute));
                    if (attr == null)
                    {
                        attr = caller.ReflectedType.GetCustomAttribute(typeof(SaveFilesAtAttribute));
                        if (attr == null)
                        {
                            attr = caller.ReflectedType.Assembly.GetCustomAttribute(typeof(SaveFilesAtAttribute));
                        }
                    }
                    string subDir = ((attr is SaveFilesAtAttribute folder) ? folder.Path : string.Empty);

                    attr = caller.GetCustomAttribute(typeof(UseAttribute));
                    if (attr == null)
                    {
                        attr = caller.ReflectedType.GetCustomAttribute(typeof(UseAttribute));
                        if (attr == null)
                        {
                            attr = caller.ReflectedType.Assembly.GetCustomAttribute(typeof(UseAttribute));
                        }
                    }
                    var reporter = (attr as UseAttribute);

                    return _context = new TestContext(caller.Name, caller.ReflectedType.Name, frame.GetFileName(), subDir, reporter);
                }
            }

            // TODO: Create a proper exception message.
            throw new System.NotImplementedException();
        }

        #region Private Members

        private readonly Type _testMethodAttribute;
        private TestContext _context;

        #endregion Private Members
    }
}