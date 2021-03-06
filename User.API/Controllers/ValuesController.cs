﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using User.API.Data;
using User.API.Models;

namespace User.API.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private UserContext _userContext;
        public ValuesController(UserContext userContext)
        {
            _userContext = userContext;
        }
        // GET api/values
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return Json(await _userContext.Users.SingleOrDefaultAsync(u => u.Name == "jesse"));
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
