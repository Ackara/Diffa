using Acklann.Diffa.Reporters;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Acklann.Diffa.Resolution
{
    internal class StackTraceParser : IContextBuilder
    {
        public StackTraceParser()
        {
            foreach (string typeName in KnownTestFramework.TestMethodAttributeNames)
            {
                _testMethodAttribute = Type.GetType(typeName, throwOnError: false);
                if (_testMethodAttribute != null) return;
            }

            throw new NotSupportedException(Exceptions.ExceptionMessage.GetTestFrameworkNotSupported());
        }

        public TestContext Context
        {
            get { return _context.IsNotInstaniated ? CreateContext() : _context; }
        }

        public TestContext CreateContext()
        {
            MemberInfo caller;
            TestContext context;
            Type asyncWrapper = null;
            string sourceFile = null, temp;
            var stack = new StackTrace(true);

            foreach (StackFrame frame in stack.GetFrames())
            {
                caller = frame.GetMethod();
                temp = frame.GetFileName();
                sourceFile = (string.IsNullOrEmpty(temp) ? sourceFile : temp);
                if (caller.ReflectedType.IsNested && !string.IsNullOrEmpty(temp)) asyncWrapper = caller.ReflectedType;
#if DEBUG
                System.Diagnostics.Debug.WriteLine($"{caller.ReflectedType.Name} => {caller.Name} | async:{caller.ReflectedType.IsNested}");
                System.Diagnostics.Debug.WriteLine($"at '{frame.GetFileName()}'");
                System.Diagnostics.Debug.WriteLine("");
#endif
                if (TryParse(caller, sourceFile, out context)) return context;
            }

            if (asyncWrapper != null)
            {
                Match match = Regex.Match(asyncWrapper.Name, @"<(?<method>\w+)>");
                if (match.Success)
                {
                    MethodInfo testMethod = Type.GetType($"{asyncWrapper.ReflectedType.FullName}, {asyncWrapper.ReflectedType.Assembly.FullName}").GetMethods()
                        .FirstOrDefault(x => x.Name == match.Groups["method"].Value && x.IsDefined(_testMethodAttribute));
                    if (TryParse(testMethod, sourceFile, out context)) return context;
                }
            }

            throw new TargetException(Exceptions.ExceptionMessage.GetTestNotFoundMessage()) { HelpLink = Exceptions.ExceptionMessage.IssuesLink };
        }

        internal bool TryParse(MemberInfo member, string sourceFile, out TestContext context)
        {
            if (member.IsDefined(_testMethodAttribute))
            {
                Attribute attr = member.GetCustomAttribute(typeof(SaveFilesAtAttribute));
                if (attr == null)
                {
                    attr = member.ReflectedType.GetCustomAttribute(typeof(SaveFilesAtAttribute));
                    if (attr == null)
                    {
                        attr = member.ReflectedType.Assembly.GetCustomAttribute(typeof(SaveFilesAtAttribute));
                    }
                }
                string subDir = ((attr is SaveFilesAtAttribute folder) ? folder.Path : string.Empty);

                attr = member.GetCustomAttribute(typeof(UseAttribute));
                if (attr == null)
                {
                    attr = member.ReflectedType.GetCustomAttribute(typeof(UseAttribute));
                    if (attr == null)
                    {
                        attr = member.ReflectedType.Assembly.GetCustomAttribute(typeof(UseAttribute));
                    }
                }
                var reporter = (attr as UseAttribute);

                if (string.IsNullOrEmpty(sourceFile))
                {
                    Debug.WriteLine($"The source file was not set.");
                    Console.WriteLine($"The source file was not set.");
                }

                _context = context = new TestContext(member.Name, member.ReflectedType.Name, sourceFile, subDir, reporter);
                return true;
            }

            context = TestContext.Empty;
            return false;
        }

        #region Private Members

        private readonly Type _testMethodAttribute;
        private TestContext _context;

        #endregion Private Members
    }
}