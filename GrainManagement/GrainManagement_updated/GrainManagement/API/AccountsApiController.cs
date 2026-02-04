using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using GrainManagement.Models;
using GrainManagement.Models.DTO;
using GrainManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Threading.Tasks;





namespace GrainManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsApiController : ControllerBase
    {
        private readonly dbContext _ctx;
       

        public AccountsApiController(dbContext ctx)
        {
            _ctx = ctx;
            
        }

        //// GET: api/AccountsApi
        //[HttpGet]
        //public async Task<object> Get(DataSourceLoadOptions loadOptions)
        //{
        //    var query = _ctx.Accounts
        //        .AsNoTracking()
        //        .Select(a => new AccountDTO
        //        {
        //            Id = a.Id,
        //            EntityName = a.EntityName,
        //            LookupName = a.LookupName,
        //            OwnerFirstName = a.OwnerFirstName,
        //            OwnerLastName = a.OwnerLastName,
        //            IsProducer = a.IsProducer,
        //            Active = a.Active,
        //            Notes = a.Notes,
        //            HedgedAccount = a.HedgedAccount,
        //        });

        //    return await DataSourceLoader.LoadAsync(query, loadOptions ?? new DataSourceLoadOptions());
        //}


        // GET: api/AccountsApi
        [HttpGet]
        public async Task<object> Get()
        {
            var query = await _ctx.Accounts
                       .AsNoTracking()
                       .Select(a => new AccountDTO
                       {

                           Id = a.Id,
                           EntityName = a.EntityName,
                           LookupName = a.LookupName,
                           OwnerFirstName = a.OwnerFirstName,
                           OwnerLastName = a.OwnerLastName,
                           IsProducer = a.IsProducer,
                           Active = a.Active,
                           Notes = a.Notes,
                           HedgedAccount = a.HedgedAccount,
                       }).OrderBy(x => x.Id).ToListAsync();
            return query;
        }





        // PUT: api/AccountsApi/{key}
        [HttpPut("{key}")]
        public IActionResult Put(long key, [FromBody] AccountDTO dto)
        {
            if (dto == null || dto.Id != key)
                return BadRequest("Invalid account payload.");

            var account = _ctx.Accounts.SingleOrDefault(a => a.Id  == key);
            if (account == null)
                return NotFound();

            // If you do NOT want AccountId editable, just ignore dto.AccountId here
            // and show AccountId as read-only in the grid.
            account.EntityName = dto.EntityName ?? account.EntityName;
            account.LookupName = dto.LookupName ?? account.LookupName;
            account.OwnerFirstName = dto.OwnerFirstName ?? account.OwnerFirstName;
            account.OwnerLastName = dto.OwnerLastName ?? account.OwnerLastName;
            account.IsProducer = dto.IsProducer;
            account.Active = dto.Active;
            account.Notes = dto.Notes ?? account.Notes;
            account.HedgedAccount = dto.HedgedAccount;
            

            _ctx.SaveChanges();
            return Ok();
        }
    }
}
