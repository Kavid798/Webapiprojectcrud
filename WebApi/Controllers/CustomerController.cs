using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.DTO;
using WebApi.Models;
namespace WebApi.Controllers
{
    public class CustomerController : ApiController
    {
        public List<string> cities = new List<string> { "Pondicherry", "Chennai", "Bangalore" };
        [Route("api/Customers/GetCities")]
        [HttpGet]
        public IEnumerable<string> GetCities()
        {
            return cities;
        }


        [Route("api/Customers/GetAllCustomer")]
        [HttpGet]
        public IEnumerable<Customerdto> GetCustomer()
        {
            Entities entities = new Entities();
            var customers = entities.Customers.Select(c => new Customerdto
            {
                Id = c.Id,
                Name = c.Name,
                Gender = c.Gender,
                BirthDate = c.BirthDate,
                City = c.City,
                MembershipTypeId = c.MembershipTypeId
            }).ToList();

            return customers;
        }

        [Route("api/Customers/Byiddto/{id}")]
        [HttpGet]
        public IEnumerable<Customerdto> GetCustomersId(int id)
        {
            Entities entities = new Entities();
            var customers = entities.Customers.Select(c => new Customerdto
            {
                Id = c.Id,
                Name = c.Name,
                BirthDate = c.BirthDate,
                City = c.City,
                MembershipTypeId = c.MembershipTypeId
            }).Where(c => c.Id == id).ToList();
            return customers;
        }

        [Route("api/Customers/Byidto/{ids}")]
        [HttpGet]
        public IHttpActionResult GetCustomersIds(int ids)
        {
            Entities entities = new Entities();
            var customers = entities.Customers.Select(c => new Customerdto
            {
                Id = c.Id,
                Name = c.Name,
                BirthDate = c.BirthDate,
                City = c.City,
                MembershipTypeId = c.MembershipTypeId
            }).FirstOrDefault(c => c.Id == ids);
            if (customers == null)
            {
                return NotFound();
            }
            return Ok(customers);
        }
        [Route("api/Customers/Byidname/{name}/{city?}")]
        [HttpGet]
        public IHttpActionResult GetCustomersId(string input)
        {
            Entities entities = new Entities();
            var customers = entities.Customers.Select(c => new Customerdto
            {
                Id = c.Id,
                Name = c.Name,
                BirthDate = c.BirthDate,
                City = c.City,
                MembershipTypeId = c.MembershipTypeId
            }).Where(c => c.Name == input || c.City == input).ToList();
            //if (customers.Count() == 0)
            //{
            //    return NotFound();
            //}
            return Ok(customers);
        }

        [HttpPost]
        public IHttpActionResult CreateCustomer([FromBody]Customerdto customerdto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);//Model State

            }
            try
            {
                Entities entities = new Entities();
                Customer customer = new Customer
                {
                    Name = customerdto.Name,
                    Gender = customerdto.Gender,
                    BirthDate = customerdto.BirthDate,
                    City = customerdto.City,
                    MembershipTypeId = customerdto.MembershipTypeId
                };
                entities.Customers.Add(customer);
                entities.SaveChanges();
                customerdto.Id = customer.Id;
                return Created(new Uri(Request.RequestUri + "/" + customerdto.Id), customerdto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return StatusCode(HttpStatusCode.InternalServerError);
            }
        }
        [HttpPut]
        public IHttpActionResult UpdateCustomer([FromUri]int id, [FromBody] Customerdto customerdto)
        {
            Entities entities = new Entities();
            var customerInDb = entities.Customers.FirstOrDefault(c => c.Id == id);
            if (customerInDb == null)
            {
                return NotFound();
            }
            if (id != customerdto.Id)
            {
                return BadRequest();
            }
            customerInDb.Name = customerdto.Name;
            customerInDb.City = customerdto.City;
            customerInDb.Gender = customerdto.Gender;
            customerInDb.BirthDate = customerdto.BirthDate;
            customerInDb.MembershipTypeId = customerdto.MembershipTypeId;

            entities.Entry(customerInDb).State = System.Data.Entity.EntityState.Modified;
            entities.SaveChanges();
            return Ok();
        }
        [HttpDelete]
        
        public IHttpActionResult DeleteCustomer(int id)
        {
            Entities entities = new Entities();
            var customerInDb = entities.Customers.FirstOrDefault(c => c.Id == id);
            if (customerInDb == null)
            {
                return NotFound();
            }
            entities.Entry(customerInDb).State = System.Data.Entity.EntityState.Deleted;
            entities.Customers.Remove(customerInDb);
            entities.SaveChanges();
            return Ok();
        }

    }
}
    
        //[Route("api/Customers/Byid/{id}")]

        //[HttpGet]

        //public WebapiClass GetAllCustomers(int id)
        //{
        //    IEnumerable<WebapiClass> customers = GetCustomers();
        //    var customer = customers.Where(c => c.Id == id).FirstOrDefault();
        //    return customer;
        //}
        //[Route("api/Customers/Byname/{Name}")]

        //[HttpGet]
        //public WebapiClass GetAllCustomersName(string Name)
        //{
        //    IEnumerable<WebapiClass> customers = GetCustomers();
        //    var customer = customers.Where(c => c.Name == Name).FirstOrDefault();
        //    return customer;
        //}

        ////[NonAction]
        ////public IEnumerable<WebapiClass> GetCustomers()
        ////{
        ////    List<WebapiClass> customers = new List<WebapiClass>
        ////    {
        ////        new WebapiClass{Id=1,Name="Kavitha",Age=25},
        ////        new WebapiClass{Id=2,Name="Qwerty",Age=21}


        ////    };
        ////    return customers;
        //}


    

