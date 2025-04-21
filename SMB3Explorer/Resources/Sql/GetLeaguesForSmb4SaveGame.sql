SELECT tl.name             AS LeagueName,
       tt.teamName,
       COUNT(ts.GUID)      AS NumSeasons,
       MAX(ts.elimination) AS elimination,
       tf.GUID             AS franchiseId
FROM t_leagues tl
         LEFT JOIN t_franchise tf on tl.GUID = tf.leagueGUID
         LEFT JOIN t_teams tt on tf.playerTeamGUID = tt.GUID
         LEFT JOIN t_seasons ts on tl.GUID = ts.historicalLeagueGUID
WHERE tl.originalGUID IS NULL
LIMIT 1;