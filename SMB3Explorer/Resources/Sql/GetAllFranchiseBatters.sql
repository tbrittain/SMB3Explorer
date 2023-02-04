-- This is a query that uses the v_stats_batting view to get the batting stats for a league
-- When using this query, you will need to replace the @param1 with the GUID of the league you want to get the stats for
-- When running query directly in a DB client, you will need to transform the GUID to an all caps string without dashes
-- and prefix it with X'', such as with the following example:
-- League ID = 'c3bb2bcd-c40e-4396-a95b-0b2ac10a1a33'
-- Converted to a format like X'C3BB2BCDC40E4396A95B0B2AC10A1A33'

-- In C# Interactive, you can use the following code to convert the GUID to the format needed for the query:
-- new Guid("c3bb2bcd-c40e-4396-a95b-0b2ac10a1a33").ToString("N").ToUpperInvariant()
WITH teams AS
         (SELECT ttli.GUID AS teamGUID, tt.teamName
          FROM t_team_local_ids ttli
                   JOIN t_teams tt ON ttli.GUID = tt.GUID)
SELECT vsb.*, 
       t.teamName, 
       t2.teamName AS mostRecentlyPlayedTeamName, 
       t3.teamName AS previousRecentlyPlayedTeamName
FROM v_stats_batting vsb
         JOIN t_stats_players tsp ON vsb.statsPlayerID = tsp.statsPlayerID
         JOIN t_league_local_ids tlli ON tsp.leagueLocalID = tlli.localID
         JOIN t_leagues tl ON tlli.GUID = tl.GUID
         JOIN teams t ON vsb.teamGUID = t.teamGUID
         JOIN teams t2 ON vsb.mostRecentlyPlayedTeamGUID = t2.teamGUID
         JOIN teams t3 ON vsb.previousRecentlyPlayedTeamGUID = t3.teamGUID
WHERE tl.GUID = CAST(@param1 AS BLOB)
ORDER BY plateAppearances DESC