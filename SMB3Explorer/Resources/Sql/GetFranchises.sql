SELECT tl.GUID               AS league_id,
       tl.name               AS league_name,
       tl.allowedTeamType    AS league_team_type_id,
       ttt.typeName          AS league_type_name,
       tf.GUID               AS franchise_id,
       tf.playerTeamGUID     AS player_team_id,
       tt.teamName           AS player_team_name,
       COUNT(tfs.seasonGUID) AS num_seasons
FROM t_leagues tl
         JOIN t_franchise tf ON tl.GUID = tf.leagueGUID
         JOIN t_franchise_seasons tfs ON tf.GUID = tfs.franchiseGUID
         JOIN t_team_types ttt ON ttt.teamType = tl.allowedTeamType
         JOIN t_teams tt ON tt.GUID = tf.playerTeamGUID
GROUP BY tl.GUID, tl.name, tf.GUID
ORDER BY num_seasons DESC