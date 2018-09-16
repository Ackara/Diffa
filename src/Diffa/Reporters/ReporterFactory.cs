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

            foreach (Type type in assemblyTypes)
            {
                int? order = (type.GetCustomAttribute(typeof(RankAttribute)) as RankAttribute)?.Index;
                _reporterTypes.Add(((order ?? 10), type));
            }
        }

        public IReporter GetFirstAvailableReporter(bool shouldPause)
        {
            if (_firstReporter == null)
                foreach ((int, Type Type) info in _reporterTypes.OrderByDescending(x => x.Item1))
                {
                    var reporter = (IReporter)Activator.CreateInstance(info.Type, args: shouldPause);
                    if (reporter.CanLaunch())
                    {
                        _firstReporter = reporter;
                        break;
                    }
                }

            return _firstReporter;
        }

        #region Private Members

        private readonly ICollection<(int, Type)> _reporterTypes = new List<(int, Type)>();
        private IReporter _firstReporter;

        #endregion Private Members
    }
}