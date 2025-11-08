using Agvantage_TransferV2.GmModels;
using Agvantage_TransferV2.Logging;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Agvantage_TransferV2.Sync;

public sealed class AccountSyncService : IAccountSyncService
{
    private readonly GMDbContext _ctx;
    private readonly ITransferLogger _log;
    private readonly ILogger<AccountSyncService> _logger;

    public AccountSyncService(GMDbContext nw, ITransferLogger log, ILogger<AccountSyncService> logger)
    {
        
        _ctx = nw;
        _log = log;
        _logger = logger;
    }


    public async Task UpsertAsync(IEnumerable<Account> accountList)
    {
        try
        {
            // Load all current accounts from DB
            var dbAccounts = await _ctx.Accounts.ToListAsync();
            var dbAccountsById = dbAccounts.ToDictionary(a => a.AccountId);

            // Track incoming AccountIds
            var incomingIds = new HashSet<long>(accountList.Select(a => a.AccountId));
            var updated=0;
            var added=0;
            var deactivated=0;
            // Update or add incoming accounts
            foreach (var incoming in accountList)
            {
                if (dbAccountsById.TryGetValue(incoming.AccountId, out var dbAcc))
                {
                    updated++;
                    // Update fields
                    dbAcc.EntityName = incoming.EntityName;
                    dbAcc.LookupName = incoming.LookupName;
                    dbAcc.OwnerFirstName = incoming.OwnerFirstName;
                    dbAcc.OwnerLastName = incoming.OwnerLastName;
                    dbAcc.Address1 = incoming.Address1;
                    dbAcc.Address2 = incoming.Address2;
                    dbAcc.City = incoming.City;
                    dbAcc.State = incoming.State;
                    dbAcc.Zip = incoming.Zip;
                    dbAcc.Phone1 = incoming.Phone1;
                    dbAcc.Phone2 = incoming.Phone2;
                    dbAcc.Phone3 = incoming.Phone3;
                    dbAcc.Email = incoming.Email;
                    dbAcc.TaxExempt = incoming.TaxExempt;
                    dbAcc.TaxExemptDate = incoming.TaxExemptDate;
                    dbAcc.IsProducer = incoming.IsProducer;
                    dbAcc.Active = incoming.Active;
                    dbAcc.EmailStatement = incoming.EmailStatement;
                    dbAcc.PrintStatement = incoming.PrintStatement;
                   // dbAcc.Wholesale = incoming.Wholesale;
                   // dbAcc.CustomerPaysRoyalties = incoming.CustomerPaysRoyalties;
                    //dbAcc.AutoPrice = incoming.AutoPrice;
                    //dbAcc.Contact = incoming.Contact;
                    dbAcc.Notes = incoming.Notes;
                    //dbAcc.HedgedAccount = incoming.HedgedAccount;
                    //dbAcc.IsHauler = incoming.IsHauler;
                }
                else
                {
                    added++;
                    // Add new account
                    _ctx.Accounts.Add(incoming);
                }
            }

            // Mark accounts as inactive if not in incoming list
            foreach (var dbAcc in dbAccounts)
            {
                if (!incomingIds.Contains(dbAcc.AccountId) && dbAcc.Active)
                {
                    deactivated++;  
                    dbAcc.Active = false;
                }
            }

            await _ctx.SaveChangesAsync();
           _logger.LogInformation("Accounts UpsertAsync completed: {Added} added, {Updated} updated, {Deactivated} deactivated.", added, updated, deactivated);
            await _log.InfoAsync($"Accounts UpsertAsync completed: {added} added, {updated} updated, {deactivated} deactivated.", "Accounts");
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"An error occurred in Accounts UpsertAsync: {TransferLogger.UsefulMessage(ex)}");

            await _log.WarnAsync($"An error occurred in Accounts UpsertAsync: {TransferLogger.UsefulMessage(ex)}", "Accounts");
        }
    }

}
