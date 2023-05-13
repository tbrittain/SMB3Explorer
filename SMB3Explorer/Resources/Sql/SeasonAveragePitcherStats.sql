WITH mostRecentSeason AS (SELECT id                        AS seasonID,
                                 RANK() OVER (ORDER BY id) AS seasonNum
                          FROM t_seasons
                                   JOIN t_leagues ON t_seasons.historicalLeagueGUID = t_leagues.GUID
                                   JOIN t_franchise tf ON t_leagues.GUID = tf.leagueGUID
                          WHERE t_leagues.GUID = CAST(@leagueId AS BLOB)
                          ORDER BY ID DESC
                          LIMIT 1)
SELECT AVG(
               CASE
                   WHEN tspitch.outsPitched = 0 THEN NULL
                   ELSE (tspitch.earnedRuns * 9) / (tspitch.outsPitched / 3.0)
                   END) AS era
FROM [v_baseball_player_info] vbpi
         LEFT JOIN t_baseball_player_local_ids tbpli ON vbpi.baseballPlayerGUID = tbpli.GUID
         LEFT JOIN t_stats_players tsp ON tbpli.localID = tsp.baseballPlayerLocalID
         LEFT JOIN t_stats ts ON tsp.statsPlayerID = ts.statsPlayerID
         JOIN t_stats_pitching tspitch ON ts.aggregatorID = tspitch.aggregatorID
         LEFT JOIN t_baseball_players tbp ON tbpli.GUID = tbp.GUID
         LEFT JOIN t_season_stats tss ON ts.aggregatorID = tss.aggregatorID
         JOIN t_seasons tsea ON tss.seasonID = tsea.ID
         JOIN mostRecentSeason ON mostRecentSeason.seasonID = tsea.ID