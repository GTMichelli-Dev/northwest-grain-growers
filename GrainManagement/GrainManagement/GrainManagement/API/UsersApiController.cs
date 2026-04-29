// Controllers/UsersApiController.cs
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using GrainManagement.Models;
using GrainManagement.Models.DTO;
using GrainManagement.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace GrainManagement.Controllers
{
    [Route("api/[controller]")]
    [UseAdminConnection]
    [ApiController]
    public class UsersApiController : ControllerBase
    {
        private readonly dbContext _ctx;
        private readonly ILogger<UsersApiController> _logger;

        public UsersApiController(dbContext ctx, ILogger<UsersApiController> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }






        // Controllers/UsersApiController.cs

        [HttpGet]
        public object Get(DataSourceLoadOptions loadOptions)
        {

            DevExtremeUtils.NormalizeLoadOptions(loadOptions);
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
                    UserId = u.UserId,
                    Pin = u.Pin,   // <-- IMPORTANT: now DevExtreme can sort on this
                    UserName = u.UserName,
                    IsActive = u.IsActive,
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
           

            _logger.LogInformation("QueryString: {qs}", Request.QueryString.Value);

            _logger.LogInformation("DX Filter (raw): {filter}",
                JsonSerializer.Serialize(loadOptions.Filter));

            return DataSourceLoader.Load(dtoQuery, loadOptions);
        }


        // privilege lookup for TagBox
        [HttpGet("privileges")]
        public IActionResult GetPrivileges()
        {
            var data = _ctx.Privileges
                .Select(p => new { Id = p.PrivilegeId, Name = p.Description })
                .OrderBy(p => p.Id)
                .ToList();

            return Ok(data);
        }


        [HttpPost]
        public IActionResult Post([FromBody] UserEditDto dto)
        {
            if (dto == null)
                return BadRequest(new { message = "Invalid user payload." });

            if (string.IsNullOrWhiteSpace(dto.UserName))
                return BadRequest(new { message = "Name is required." });

            bool pinExists = _ctx.Users.Any(u => u.Pin == dto.Pin);
            if (pinExists)
                return Conflict(new { message = "User Pin must be unique." });

            bool nameExists = _ctx.Users.Any(u => u.UserName == dto.UserName);
            if (nameExists)
                return Conflict(new { message = "User name must be unique." });

            var user = new User
            {
                Pin = dto.Pin,
                UserName = dto.UserName,
                // Default new users to active when not specified.
                IsActive = dto.IsActive ?? true,
            };

            _ctx.Users.Add(user);
            _ctx.SaveChanges(); // Save user first to get UserId

            // Default new users to PrivilegeIds 1–6 (everything except Remote Admin = 7)
            // when the caller didn't specify any privileges.
            var newIds = (dto.PrivilegeIds ?? new List<int>()).Distinct().ToList();
            if (newIds.Count == 0)
                newIds = new List<int> { 1, 2, 3, 4, 5, 6 };

            foreach (var pid in newIds)
            {
                _ctx.UserPrivileges.Add(new UserPrivilege
                {
                    UserId = user.UserId, // Now populated
                    PrivilegeId = pid
                });
            }

            _ctx.SaveChanges(); // Save privileges

            var result = new UserEditDto
            {
                UserId = user.UserId,
                Pin = user.Pin,
                UserName = user.UserName,
                PrivilegeIds = newIds,
                PrivilegeNames = string.Join(", ",
                    _ctx.Privileges.Where(p => newIds.Contains(p.PrivilegeId)).Select(p => p.Description))
            };

            return Ok(result);
        }

        [HttpPut("{key}")]
        public IActionResult Put(long key, [FromBody] UserEditDto dto)
        {
            if (dto == null)
                return BadRequest(new { message = "Invalid user payload." });


            var user = _ctx.Users
                .Include(u => u.UserPrivileges)
                .SingleOrDefault(u => u.UserId == key);

            if (user == null)
                return NotFound();

            // Use incoming values if present, otherwise keep existing
            var newPin = dto.Pin == 0 ? user.Pin : dto.Pin;
            var newName = dto.UserName ?? user.UserName;

            if (string.IsNullOrWhiteSpace(newName))
                return BadRequest(new { message = "User name is required." });

            // Uniqueness checks use the final values
            bool pinExists = _ctx.Users
                .Any(u => u.Pin == newPin && u.UserId != key);

            if (pinExists)
                return Conflict(new { message = "User Pin must be unique." });

            if (newPin > 999999)  
            {
                return BadRequest(new { message = "User Pin must be less than 100000." });
            }
            if (newPin <1)
            {
                return BadRequest(new { message = "User Pin must be greater than 0." });
            }
            bool nameExists = _ctx.Users
                .Any(u => u.UserName == newName && u.UserId != key);
            if (nameExists)
                return Conflict(new { message = "User name must be unique." });

            // Apply scalar updates
            user.Pin = newPin;
            user.UserName = newName;
            // Only update IsActive when the caller explicitly sent it. The DevExtreme
            // grid sends partial PUTs that contain only changed fields, so an absent
            // IsActive must NOT clobber the stored value.
            if (dto.IsActive.HasValue)
                user.IsActive = dto.IsActive.Value;

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
               
                    UserId = user.UserId,
                    PrivilegeId = pid
                });
            }

            _ctx.SaveChanges();
            return Ok();
        }


        [HttpDelete("{key}")]
        public IActionResult Delete(long key)
        {
            var user = _ctx.Users
                .Include(u => u.UserPrivileges)
                .SingleOrDefault(u => u.UserId == key);

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
