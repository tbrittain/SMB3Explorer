SELECT id                        AS seasonID,
       historicalLeagueGUID      AS leagueGUID,
       RANK() OVER (ORDER BY id) AS seasonNum
FROM t_seasons
         JOIN t_leagues ON t_seasons.historicalLeagueGUID = t_leagues.GUID
         JOIN t_franchise tf ON t_leagues.GUID = tf.leagueGUID
WHERE t_leagues.GUID = CAST(@leagueId AS BLOB)
ORDER BY ID DESC