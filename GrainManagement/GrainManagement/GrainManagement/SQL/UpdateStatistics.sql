-- Nightly statistics maintenance for GrainManagement.
-- Refreshes stats on every user table so the query optimizer builds good plans
-- (guards against slow/timing-out queries caused by stale statistics).
--
-- Invoked by the Windows Scheduled Task "GrainManagement - Nightly sp_updatestats"
-- (see README.md for setup).

USE GrainManagement;
EXEC sp_updatestats;
