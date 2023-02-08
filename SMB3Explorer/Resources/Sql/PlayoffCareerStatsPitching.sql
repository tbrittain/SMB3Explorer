WITH teams AS
         (SELECT ttli.GUID AS teamGUID, tt.teamName
          FROM t_team_local_ids ttli
                   JOIN t_teams tt ON ttli.GUID = tt.GUID)
SELECT baseballPlayerGUID,
       CASE
           WHEN tsp.[baseballPlayerLocalID] IS NULL THEN tsp.[firstName]
           ELSE vbpi.[firstName] END                  AS firstName,
       CASE
           WHEN tsp.[baseballPlayerLocalID] IS NULL THEN tsp.[lastName]
           ELSE vbpi.[lastName] END                   AS lastName,
       CASE
           WHEN tsp.[baseballPlayerLocalID] IS NULL THEN tsp.[primaryPos]
           ELSE vbpi.[primaryPosition] END            AS primaryPosition,
       CASE
           WHEN tsp.[baseballPlayerLocalID] IS NULL THEN tsp.[pitcherRole]
           ELSE vbpi.[pitcherRole] END                AS pitcherRole,
       CAST(secondaryPosition.optionValue AS INTEGER) AS secondaryPosition,
       tspitch.*,
       currentTeam.teamName AS currentTeam,
       previousTeam.teamName AS previousTeam
FROM [v_baseball_player_info] vbpi
         LEFT JOIN t_baseball_player_local_ids tbpli ON vbpi.baseballPlayerGUID = tbpli.GUID
         LEFT JOIN t_stats_players tsp ON tbpli.localID = tsp.baseballPlayerLocalID
         LEFT JOIN t_stats ts ON tsp.statsPlayerID = ts.statsPlayerID
         JOIN t_stats_pitching tspitch ON ts.aggregatorID = tspitch.aggregatorID

         JOIN t_career_playoff_stats tcps ON ts.aggregatorID = tcps.aggregatorID
         JOIN t_franchise_local_ids tfli ON tcps.franchiseID = tfli.localID
         JOIN t_franchise tf ON tfli.GUID = tf.GUID

         LEFT JOIN t_baseball_player_options secondaryPosition
                   ON tbpli.localID = secondaryPosition.baseballPlayerLocalID AND secondaryPosition.optionKey = 55

         LEFT JOIN [t_team_local_ids] tt1 ON ts.[currentTeamLocalID] = tt1.[localID]
         LEFT JOIN [t_team_local_ids] tt2
                   ON ts.[previousRecentlyPlayedTeamLocalID] = tt2.[localID]
         LEFT JOIN teams currentTeam ON tt1.GUID = currentTeam.teamGUID
         LEFT JOIN teams previousTeam ON tt2.GUID = previousTeam.teamGUID

WHERE tf.GUID = CAST(@franchiseId AS BLOB)
ORDER BY totalPitches DESC