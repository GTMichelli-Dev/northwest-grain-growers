// Controllers/UsersApiController.cs
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using GrainManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;

namespace GrainManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersApiController : ControllerBase
    {
        private readonly dbContext _ctx;

        public UsersApiController(dbContext ctx)
        {
            _ctx = ctx;
        }






        // Controllers/UsersApiController.cs

        [HttpGet]
        public object Get(DataSourceLoadOptions loadOptions)
        {
            // 1) Get full users + privileges from DB
            var users = _ctx.Users
                .Include(u => u.UserPrivileges)
                    .ThenInclude(up => up.Privilege)
                .AsNoTracking()
                .ToList();   // everything after this is in-memory

            // 2) Project to DTOs, including UserPin
            var dtoQuery = users
                .Select(u => new UserEditDto
                {
                    Uid = u.Uid,
                    UserPin = u.UserPin,   // <-- IMPORTANT: now DevExtreme can sort on this
                    Name = u.Name,
                    PrivilegeIds = u.UserPrivileges
                        .OrderBy(up => up.PrivilegeId)
                        .Select(up => up.PrivilegeId)
                        .ToList(),
                    PrivilegeNames = string.Join(", ",
                        u.UserPrivileges
                            .OrderBy(up => up.PrivilegeId) // Order by PrivilegeId, same as PrivilegeIds
                            .Select(up => up.Privilege.Description))
                })
                .AsQueryable();

            // 3) Let DataSourceLoader apply paging/sorting on the DTOs in memory
            return DataSourceLoader.Load(dtoQuery, loadOptions);
        }


        // privilege lookup for TagBox
        [HttpGet("privileges")]
        public IActionResult GetPrivileges()
        {
            var data = _ctx.Privileges
                .Select(p => new { Id = p.Id, Name = p.Description })
                .OrderBy(p => p.Id)
                .ToList();

            return Ok(data);
        }


        [HttpPost]
        public IActionResult Post([FromBody] UserEditDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid user payload.");

            // Basic validation
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Name is required.");

            // Uniqueness checks
            bool pinExists = _ctx.Users.Any(u => u.UserPin == dto.UserPin);
            if (pinExists)
                return Conflict(new { message = "User Pin must be unique." });

            bool nameExists = _ctx.Users.Any(u => u.Name == dto.Name);
            if (nameExists)
                return Conflict(new { message = "User name must be unique." });

            // Create user
            var user = new User
            {
                Uid = Guid.NewGuid(),
                UserPin = dto.UserPin,
                Name = dto.Name
            };

            _ctx.Users.Add(user);

            // Attach privileges (defend against nulls/dupes)
            var newIds = (dto.PrivilegeIds ?? new List<int>()).Distinct().ToList();
            foreach (var pid in newIds)
            {
                _ctx.UserPrivileges.Add(new UserPrivilege
                {
                    Uid = Guid.NewGuid(),
                    UserUid = user.Uid,
                    PrivilegeId = pid
                });
            }

            _ctx.SaveChanges();

            // Return the created row as the grid expects
            var result = new UserEditDto
            {
                Uid = user.Uid,
                UserPin = user.UserPin,
                Name = user.Name,
                PrivilegeIds = newIds,
                PrivilegeNames = string.Join(", ",
                    _ctx.Privileges.Where(p => newIds.Contains(p.Id)).Select(p => p.Description))
            };

            return Ok(result);
        }

        [HttpPut("{key}")]
        public IActionResult Put(Guid key, [FromBody] UserEditDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid user payload.");

            var user = _ctx.Users
                .Include(u => u.UserPrivileges)
                .SingleOrDefault(u => u.Uid == key);

            if (user == null)
                return NotFound();

            // Use incoming values if present, otherwise keep existing
            var newPin = dto.UserPin == 0 ? user.UserPin : dto.UserPin;
            var newName = dto.Name ?? user.Name;

            if (string.IsNullOrWhiteSpace(newName))
                return BadRequest(new { message = "User name is required." });

            // Uniqueness checks use the final values
            bool pinExists = _ctx.Users
                .Any(u => u.UserPin == newPin && u.Uid != key);
            if (pinExists)
                return Conflict(new { message = "User Pin must be unique." });

            if (newPin > 9999)
            {
                return BadRequest(new { message = "User Pin must be less than 10000." });
            }
            if (newPin <1)
            {
                return BadRequest(new { message = "User Pin must be greater than 0." });
            }
            bool nameExists = _ctx.Users
                .Any(u => u.Name == newName && u.Uid != key);
            if (nameExists)
                return Conflict(new { message = "User name must be unique." });

            // Apply scalar updates
            user.UserPin = newPin;
            user.Name = newName;

            // === Update privileges ===
            var currentIds = user.UserPrivileges
                .Select(up => up.PrivilegeId)
                .ToList();

            var newIds = dto.PrivilegeIds ?? currentIds;

            var toRemove = user.UserPrivileges
                .Where(up => !newIds.Contains(up.PrivilegeId))
                .ToList();
            _ctx.UserPrivileges.RemoveRange(toRemove);

            var toAddIds = newIds.Except(currentIds);
            foreach (var pid in toAddIds)
            {
                _ctx.UserPrivileges.Add(new UserPrivilege
                {
                    Uid = Guid.NewGuid(),
                    UserUid = user.Uid,
                    PrivilegeId = pid
                });
            }

            _ctx.SaveChanges();
            return Ok();
        }


        [HttpDelete("{key}")]
        public IActionResult Delete(Guid key)
        {
            var user = _ctx.Users
                .Include(u => u.UserPrivileges)
                .SingleOrDefault(u => u.Uid == key);

            if (user == null)
                return NotFound();

            // remove privileges first
            _ctx.UserPrivileges.RemoveRange(user.UserPrivileges);

            // remove user
            _ctx.Users.Remove(user);

            _ctx.SaveChanges();
            return Ok();
        }

    }
}
