SELECT tl.GUID               AS leagueId,
       tl.name               AS leagueName,
       tl.allowedTeamType    AS leagueTeamTypeId,
       ttt.typeName          AS leagueTypeName,
       tf.GUID               AS franchiseId,
       tf.playerTeamGUID     AS playerTeamId,
       tt.teamName           AS playerTeamName,
       COUNT(tfs.seasonGUID) AS numSeasons,
       MAX(ts.elimination)   AS elimination
FROM t_leagues tl
         LEFT JOIN t_franchise tf ON tl.GUID = tf.leagueGUID
         LEFT JOIN t_franchise_seasons tfs ON tf.GUID = tfs.franchiseGUID
         JOIN t_team_types ttt ON ttt.teamType = tl.allowedTeamType
         LEFT JOIN t_teams tt ON tt.GUID = tf.playerTeamGUID
         LEFT JOIN t_seasons ts ON tl.GUID = ts.historicalLeagueGUID
GROUP BY tl.GUID, tl.name, tf.GUID
ORDER BY numSeasons DESC