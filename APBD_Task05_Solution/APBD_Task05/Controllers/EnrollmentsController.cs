using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using APBD_Task05.DAL;
using APBD_Task05.DTOs;
using APBD_Task05.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APBD_Task05.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private IStudentDb _dbservice;
        private string connString = "Data Source=db-mssql;Initial Catalog = s20033; Integrated Security = True";
        public EnrollmentsController(IStudentDb service)
        {
            this._dbservice = service;

        }
        [HttpPost]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            using (SqlConnection con = new SqlConnection(connString))
            using (SqlCommand command = new SqlCommand())
            {
                command.CommandText = "select * from studies where Name=@Name;";
                command.Parameters.AddWithValue("Name", request.Studies);
                command.Connection = con;
                con.Open();
                SqlTransaction tran = con.BeginTransaction(0);
                command.Transaction = tran;

                try
                {
                    int IdStudies, IdEnrollment;
                    var dr = command.ExecuteReader();
                    if (!dr.Read())
                    {
                        tran.Rollback();
                        return BadRequest("Studies not Found in the database!");
                    }
                    else { IdStudies = (int)dr["IdStudy"]; }
                    dr.Close();

                    command.CommandText = "Select Max(StartDate) from enrollment where semester=1 and idStudy=@IdStudies;";
                    command.Parameters.AddWithValue("IdStudies", IdStudies);
                    dr = command.ExecuteReader();

                    if (dr.Read())
                    {
                        dr.Close();
                        command.CommandText = "select IdEnrollment from enrollment where semester=1 and idStudy=@IdStudies;";
                        dr = command.ExecuteReader();
                        dr.Read();
                        IdEnrollment = (int)dr["IdEnrollment"];
                    }
                    else
                    {
                        dr.Close();
                        command.CommandText = "select convert(varchar(20),getdate(),123) 'Date';";
                        dr = command.ExecuteReader();
                        dr.Read();
                        DateTime date = DateTime.Parse(dr["Date"].ToString());
                        dr.Close();
                        command.CommandText = "Select Max(IdEnrollment) 'max_id' from Enrollment;";
                        dr = command.ExecuteReader();
                        dr.Read();
                        IdEnrollment = (int)dr["max_id"] + 1;

                        dr.Close();
                        command.CommandText = "Insert into Enrollment values(@IdEnrollment, 1," + IdStudies + ",'" + date + "');";
                        command.Parameters.AddWithValue("IdEnrollment", IdEnrollment);
                        command.ExecuteNonQuery();
                        dr.Close();

                        //  command.CommandText="select Max(IdEnrollment"

                    }
                    dr.Close();
                    command.CommandText = "select * from Student where IndexNumber = @indexnumber;";
                    command.Parameters.AddWithValue("indexnumber", request.IndexNumber);
                    dr = command.ExecuteReader();

                    if (!dr.Read())
                    {
                        dr.Close();
                        command.CommandText = "insert into student(IndexNumber, FirstName, LastName,BirthDate, IdEnrollment) values (@par1, @par2, @par3,@par4,@IdEnrollment)";
                        command.Parameters.AddWithValue("para1", request.IndexNumber);
                        command.Parameters.AddWithValue("para2", request.FirstName);
                        command.Parameters.AddWithValue("para3", request.LastName);
                        command.Parameters.AddWithValue("para4", request.BirthDate);
                        command.Parameters.AddWithValue("IdEnrollment", IdEnrollment);
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        tran.Commit();
                        return BadRequest("Student already exists");
                    }
                    tran.Commit();

                }
                catch (Exception)
                {
                    tran.Rollback();
                }


                return Ok();
            }
        }
        [HttpPost("Promote")]
        public IActionResult PromoteStudent(Enrollment enrollment)
        {
            using (SqlConnection con = new SqlConnection(connString))
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = con;


                con.Open();

                SqlTransaction tran = con.BeginTransaction();
                try
                {

                    command.CommandText = "Select * From Enrollment e Join Studies s on e.idStudy= s.idStudy Where s.Name=@Name and e.semester =@semester;";
                    command.Parameters.AddWithValue("Name", enrollment.Studies);
                    command.Parameters.AddWithValue("semester", enrollment.Semester);
                    con.Open();


                    var dr = command.ExecuteReader();
                    if (dr.Read())
                    {
                        dr.Close();
                        command.CommandText = "Promote";
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        var dri = command.ExecuteReader();

                    }
                    else
                    {
                        return NotFound("There is no record ");
                    }
                }
                catch (Exception)
                {
                    tran.Rollback();
                }

            }
          
            return Ok("Student Promoted");
        }

    }
}
