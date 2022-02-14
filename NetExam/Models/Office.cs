using NetExam.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetExam.Models
{
    public class Office : IOffice
    {
        public string LocationName { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public IEnumerable<string> Resourses { get; set; }
    }
}
