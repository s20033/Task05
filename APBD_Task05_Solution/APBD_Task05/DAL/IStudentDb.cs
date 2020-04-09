using APBD_Task05.DTOs;
using APBD_Task05.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APBD_Task05.DAL
{
   public interface IStudentDb
    {
        IEnumerable<Student> GetStudents();
        
        void EnrollStudent(EnrollStudentRequest request);
        void PromoteStudent(int Semester, string Name);
    }
}
