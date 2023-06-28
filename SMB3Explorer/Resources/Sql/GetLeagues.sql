SELECT GUID AS LeagueId, name AS LeagueName
FROM t_leagues
WHERE originalGUID IS NULL;