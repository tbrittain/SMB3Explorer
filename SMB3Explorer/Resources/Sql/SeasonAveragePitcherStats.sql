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
WHERE tsea.ID = @seasonId