using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingTechniques
{
    public class User
    {
        public string FullName { get; set; } = default!;

        public int Age { get; set; }

        public DateOnly DateOfBirth { get; set; }
    }
}