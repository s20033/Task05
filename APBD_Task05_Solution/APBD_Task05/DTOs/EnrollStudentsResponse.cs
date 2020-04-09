using APBD_Task05.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APBD_Task05.DTOs
{
    public class EnrollStudentsResponse
    {
        public Enrollment Semester { get; set; }
        public Study Studies { get; set; }
    }
}
