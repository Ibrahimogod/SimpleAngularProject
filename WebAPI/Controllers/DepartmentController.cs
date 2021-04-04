using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class DepartmentController : ApiController
    {
        public IHttpActionResult Get()
        {
            string query = @"Exec SelectDepartments;";

            DataTable table = new DataTable();
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EmployeeAppDB"].ConnectionString))
            {
                using(var cmd = new SqlCommand(query,conn))
                {
                    using(var adabter = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.Text;
                        adabter.Fill(table);
                    }
                }
            }

            return Ok(table);
        }

        [HttpPost]
        public IHttpActionResult Post(Department department)
        {
            try
            {
                string query = $"Exec AddDepartment @DepName = '{department.DepartmentName}';";

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
            catch (Exception)
            {
                return BadRequest();
            }

            return Created<Department>("",department);//StatusCode(HttpStatusCode.Created);
        }

        public IHttpActionResult Put(Department department)
        {
            string query = $"Exec UpdateDepartment @DepName = '{department.DepartmentName}' , @DepId = {department.DepartmentId};";
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
            catch (Exception)
            {
                return NotFound();
            }

            return Ok(table);
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            string query = $"Exec DeleteDepartment  @DepId = {id};";
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
            catch (Exception)
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.Accepted);
        }


    }
}
