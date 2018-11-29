using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Acklann.Diffa.Reporters
{
    internal class ReporterFactory
    {
        public ReporterFactory()
        {
            var assemblyTypes = (from t in typeof(IReporter).Assembly.ExportedTypes
                                 where
                                    !t.IsAbstract &&
                                    !t.IsInterface &&
                                    typeof(IReporter).IsAssignableFrom(t)
                                 select t);

            TraitsAttribute traits;
            foreach (Type type in assemblyTypes)
            {
                traits = (type.GetCustomAttribute(typeof(TraitsAttribute)) as TraitsAttribute);

                int order = (traits?.Index ?? -1);
                Kind kind = (traits?.Kind ?? Kind.None);

                _reporterTypes.Add((type, order, kind));
            }
        }

        public IEnumerable<IReporter> GetReporters(bool shouldInterrupt = true, Kind filter = Kind.None)
        {
            foreach ((Type type, int rating, Kind kind) in _reporterTypes.OrderByDescending(x => x.Item2))
            {
                if (filter != Kind.None && kind != filter) continue;

                var reporter = (IReporter)Activator.CreateInstance(type, args: shouldInterrupt);
                if (reporter.CanLaunch())
                {
                    yield return reporter;
                }
            }
        }

        public IReporter GetFirstAvailableReporter(bool shouldInterrupt)
        {
            if (_firstReporter == null)
                foreach (IReporter reporter in GetReporters(shouldInterrupt))
                {
                    _firstReporter = reporter;
                    break;
                }

            return _firstReporter ?? new NullReporter();
        }

        #region Private Members

        private readonly ICollection<(Type, int, Kind)> _reporterTypes = new List<(Type, int, Kind)>();
        private IReporter _firstReporter;

        #endregion Private Members
    }
}