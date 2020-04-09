using APBD_Task05.DTOs;
using APBD_Task05.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APBD_Task05.DAL
{
    public class MockDbService : IStudentDb
    {
        public void EnrollStudent(EnrollStudentRequest request)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Student> GetStudents()
        {
            throw new NotImplementedException();
        }

        public void PromoteStudent(int Semester, string Name)
        {
            throw new NotImplementedException();
        }
    }
}
