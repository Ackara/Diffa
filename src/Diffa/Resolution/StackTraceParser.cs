using Acklann.Diffa.Reporters;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Acklann.Diffa.Resolution
{
    internal class StackTraceParser : IContextBuilder
    {
        static StackTraceParser()
        {
        }

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
                System.Diagnostics.Debug.WriteLine($"at '{temp}'");
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

        public bool TryParse(MemberInfo member, string sourceFile, out TestContext context)
        {
            if (member.IsDefined(_testMethodAttribute))
            {
                Attribute attr = member.GetCustomAttribute(typeof(ApprovedFolderAttribute));
                if (attr == null)
                {
                    attr = member.ReflectedType.GetCustomAttribute(typeof(ApprovedFolderAttribute));
                    if (attr == null)
                    {
                        attr = member.ReflectedType.Assembly.GetCustomAttribute(typeof(ApprovedFolderAttribute));
                    }
                }
                string subDir = ((attr is ApprovedFolderAttribute folder) ? folder.Path : string.Empty);

                attr = member.GetCustomAttribute(typeof(ReporterAttribute));
                if (attr == null)
                {
                    attr = member.ReflectedType.GetCustomAttribute(typeof(ReporterAttribute));
                    if (attr == null)
                    {
                        attr = member.ReflectedType.Assembly.GetCustomAttribute(typeof(ReporterAttribute));
                    }
                }
                var reporter = (attr as ReporterAttribute);

                if (string.IsNullOrEmpty(sourceFile))
                {
                    if (TryResolveSourceFilePath(member.ReflectedType.Name, out sourceFile) == false)
                    {
                        Debug.WriteLine($"DiffA | Could not resolove {member.ReflectedType.Name} source file.");
                        Console.WriteLine($"The source file was not set.");
                        Console.WriteLine($"proj: '{TestContext.ProjectDirectory}'");
                    }
                }

                var guid = (member.GetCustomAttribute(typeof(ApprovedNameAttribute)) as ApprovedNameAttribute);
                string className = (guid == null ? member.ReflectedType.Name : string.Empty);
                string testName = (guid == null ? member.Name : guid.Guid);

                _context = context = new TestContext(testName, className, sourceFile, subDir, reporter);
                return true;
            }

            context = TestContext.Empty;
            return false;
        }

        public bool TryResolveSourceFilePath(string className, out string testClassFileName)
        {
            testClassFileName = null;
            if (Directory.Exists(TestContext.ProjectDirectory))
            {
                testClassFileName = (from file in Directory.EnumerateFiles(TestContext.ProjectDirectory, $"{className}*", SearchOption.AllDirectories)
                                     where
                                        file.StartsWith(Path.Combine(TestContext.ProjectDirectory, "bin"), StringComparison.OrdinalIgnoreCase) == false
                                        &&
                                        file.StartsWith(Path.Combine(TestContext.ProjectDirectory, "obj"), StringComparison.OrdinalIgnoreCase) == false
                                     select file).FirstOrDefault();
            }
            else System.Diagnostics.Debug.WriteLine($"DiffA | Could not file the test-project at '{TestContext.ProjectDirectory}'.");

            return string.IsNullOrEmpty(testClassFileName) == false;
        }

        #region Private Members

        private readonly Type _testMethodAttribute;
        private TestContext _context;

        #endregion Private Members
    }
}