-- CREATING NEW VIEW
-- The schema name has to be 'cust'
-- The view name has to start with the prefix 'View'
CREATE VIEW[cust].[ViewBulkPlannings]
 AS SELECT DISTINCT base.Users.Sequence
FROM base.Users
