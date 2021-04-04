using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using WebAPI.Models;
using System.Web;

namespace WebAPI.Controllers
{
    public class EmployeeController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            string query = @"Exec SelectEmployees;";

            DataTable table = new DataTable();
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EmployeeAppDB"].ConnectionString))
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    using (var adabter = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.Text;
                        adabter.Fill(table);
                    }
                }
            }

            return Ok(table);
        }

        [HttpPost]
        public IHttpActionResult Post(Employee employee)
        {
            try
            {
                string query = $"Exec AddEmployees @EmployeeName = '{employee.EmployeeName}', @Department = {GetDepartmentId(employee.Department)}, @DateOfJoining = '{employee.DateOfJoining}',@PhotoName = '{employee.PhotoName}';";

                DataTable table = new DataTable();
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EmployeeAppDB"].ConnectionString))
                {
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        using (var adabter = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.Text;
                            adabter.Fill(table);
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                return BadRequest();
            }

            return Created<Employee>("",employee);
        }

        int GetDepartmentId(string departmentName)
        {
            var query = $"Exec GetDepartmentId @DepartmentName = '{departmentName}';";
            int depId = 0;
            try
            {
                using(var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EmployeeAppDB"].ConnectionString))
                {
                    using(var cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            depId = Convert.ToInt32(reader.GetValue(0));
                        }
                        conn.Close();
                    }
                }
            }
            catch(Exception ex)
            {
            }

            return depId;
        }

        [HttpPut]
        public IHttpActionResult Put(Employee employee)
        {
            string query = $"Exec UpdateEmployee @EmployeeId = {employee.EmployeeId} , @EmployeeName = '{employee.EmployeeName}',@Department = {GetDepartmentId(employee.Department)}, @DateOfJoining = '{employee.DateOfJoining}', @PhotoName='{employee.PhotoName}';";

            DataTable table = new DataTable();
            try
            {

                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EmployeeAppDB"].ConnectionString))
                {
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        using (var adabter = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.Text;
                            adabter.Fill(table);
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                return NotFound();
            }

            return Ok(table);
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            string query = $"Exec DeleteEmployee @EmployeeId = {id};";
            DataTable table = new DataTable();
            try
            {

                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EmployeeAppDB"].ConnectionString))
                {
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        using (var adabter = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.Text;
                            adabter.Fill(table);
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.Accepted);
        }

        [Route("api/Employee/GetAllDepartmentNames")]
        [HttpGet]
        public IHttpActionResult GetAllDepartments()
        {
            string query = @"Exec SelectAllDepartment;";

            DataTable table = new DataTable();
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EmployeeAppDB"].ConnectionString))
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    using (var adabter = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.Text;
                        adabter.Fill(table);
                    }
                }
            }

            return Ok(table);
        }

        [Route("api/Employee/SaveFile")]
        public string SavePhoto()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                var postedFile = httpRequest.Files[0];
                string fileName = postedFile.FileName;
                var physicalPath = HttpContext.Current.Server.MapPath("~/Photos/" + fileName);
                postedFile.SaveAs(physicalPath);

                return fileName;
            }
            catch (Exception ex)
            {
                return "anonymous.jpg";
            }
        }
    }
}
