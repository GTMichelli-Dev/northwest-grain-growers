# NorthwestGrainGrowers

## Database Maintenance

### Nightly statistics refresh

SQL Server uses column/index statistics to plan queries. When stats go stale the
optimizer can pick a catastrophic plan and previously-fast queries start timing
out (symptom seen in the wild: `Execution Timeout Expired` on the
`IntakeWeightSheet` PDF endpoint and the `/GrowerDelivery/WeightSheetLots` grid).

The fix is a nightly `sp_updatestats` against the `GrainManagement` database.

Because the production box runs **SQL Server Express**, SQL Server Agent is not
available. We use a **Windows Scheduled Task** instead.

#### One-time setup per environment

1. Ensure `sqlcmd` is on `PATH` (bundled with SQL Server client tools).
2. The account that runs the task must have permission to update statistics on
   `GrainManagement` (sysadmin or db_owner). Windows Authentication is used —
   no SQL password is stored in the task.
3. Open PowerShell and run:

   ```powershell
   $sqlPath = "<repo-root>\GrainManagement\GrainManagement\GrainManagement\SQL\UpdateStatistics.sql"
   $logPath = "<repo-root>\GrainManagement\GrainManagement\GrainManagement\SQL\UpdateStatistics.log"

   $action   = New-ScheduledTaskAction -Execute "sqlcmd.exe" `
                 -Argument "-S . -E -d GrainManagement -C -i `"$sqlPath`" -o `"$logPath`""
   $trigger  = New-ScheduledTaskTrigger -Daily -At 2:00am
   $settings = New-ScheduledTaskSettingsSet -StartWhenAvailable `
                 -AllowStartIfOnBatteries -DontStopIfGoingOnBatteries `
                 -ExecutionTimeLimit (New-TimeSpan -Minutes 30)

   Register-ScheduledTask `
     -TaskName "GrainManagement - Nightly sp_updatestats" `
     -Action   $action `
     -Trigger  $trigger `
     -Settings $settings `
     -Description "Runs sp_updatestats on GrainManagement nightly."
   ```

4. Smoke-test:

   ```powershell
   Start-ScheduledTask -TaskName "GrainManagement - Nightly sp_updatestats"
   Get-ScheduledTaskInfo -TaskName "GrainManagement - Nightly sp_updatestats" |
     Format-List LastRunTime, LastTaskResult, NextRunTime
   ```

   `LastTaskResult = 0` means success. The tail of `UpdateStatistics.log`
   should end with `Statistics for all tables have been updated.`

#### Running manually

```cmd
sqlcmd -S . -E -d GrainManagement -C -i SQL\UpdateStatistics.sql
```

#### Files

| Path | Purpose |
| --- | --- |
| `GrainManagement/SQL/UpdateStatistics.sql` (in [northwest-grain-management](https://github.com/GTMichelli-Dev/northwest-grain-management)) | The maintenance script the task runs. |
| `GrainManagement/SQL/UpdateStatistics.log` (in [northwest-grain-management](https://github.com/GTMichelli-Dev/northwest-grain-management)) | Overwritten on each run. Git-ignored. |

#### Emergency: unstick a currently-slow query

If a page/endpoint is timing out before the next nightly run completes:

```cmd
sqlcmd -S . -E -d GrainManagement -C -Q "EXEC sp_updatestats;"
```
