SELECT name AS LeagueName
FROM t_leagues
WHERE originalGUID IS NULL
LIMIT 1;