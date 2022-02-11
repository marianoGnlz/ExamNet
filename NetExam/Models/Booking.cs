using NetExam.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetExam.Models
{
    public class Booking : IBooking
    {
        public DateTime DateTime { get; set; }
        public IOffice Office { get; set; }
    }
}
