SELECT tl.GUID               AS leagueId,
       tl.name               AS leagueName,
       tl.allowedTeamType    AS leagueTeamTypeId,
       ttt.typeName          AS leagueTypeName,
       tf.GUID               AS franchiseId,
       tf.playerTeamGUID     AS playerTeamId,
       tt.teamName           AS playerTeamName,
       COUNT(tfs.seasonGUID) AS numSeasons
FROM t_leagues tl
         JOIN t_franchise tf ON tl.GUID = tf.leagueGUID
         JOIN t_franchise_seasons tfs ON tf.GUID = tfs.franchiseGUID
         JOIN t_team_types ttt ON ttt.teamType = tl.allowedTeamType
         JOIN t_teams tt ON tt.GUID = tf.playerTeamGUID
GROUP BY tl.GUID, tl.name, tf.GUID
ORDER BY numSeasons DESC