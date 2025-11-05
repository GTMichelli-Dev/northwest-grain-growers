using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using GrainManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

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

        // GET: api/AccountsApi
        [HttpGet]
        public object Get(DataSourceLoadOptions loadOptions)
        {
            // Project EF entity -> AccountDTO
            var query = _ctx.Accounts
                .AsNoTracking()
                .Select(a => new AccountDTO
                {
                    Uid = a.Uid,
                    AccountId = a.AccountId,
                    EntityName = a.EntityName,
                    LookupName = a.LookupName,
                    OwnerFirstName = a.OwnerFirstName,
                    OwnerLastName = a.OwnerLastName,
                    IsProducer = a.IsProducer,
                    Active = a.Active,
                    Notes = a.Notes,
                    HedgedAccount = a.HedgedAccount,
                    IsHauler = a.IsHauler
                });

            return DataSourceLoader.Load(query, loadOptions);
        }

        // PUT: api/AccountsApi/{key}
        [HttpPut("{key}")]
        public IActionResult Put(Guid key, [FromBody] AccountDTO dto)
        {
            if (dto == null || dto.Uid != key)
                return BadRequest("Invalid account payload.");

            var account = _ctx.Accounts.SingleOrDefault(a => a.Uid == key);
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
            account.IsHauler = dto.IsHauler;

            _ctx.SaveChanges();
            return Ok();
        }
    }
}
