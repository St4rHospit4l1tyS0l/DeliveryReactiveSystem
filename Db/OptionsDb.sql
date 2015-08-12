ALTER DATABASE CallCenter
SET ALLOW_SNAPSHOT_ISOLATION ON

ALTER DATABASE CallCenter
SET READ_COMMITTED_SNAPSHOT ON
GO

sp_who2
GO

--KILL 55

SELECT is_read_committed_snapshot_on FROM
sys.databases WHERE name= 'CallCenter'