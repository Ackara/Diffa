using System;
using System.Collections.Generic;

namespace Acklann.Diffa.Fakes
{
    public class Album
    {
        public DateTime RelaseDate;
        public string Name { get; set; }

        public int Length { get; set; }

        public IEnumerable<string> Artists { get; set; }
    }
}