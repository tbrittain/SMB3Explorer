WITH teams AS
         (SELECT ttli.GUID AS teamGUID, tt.teamName
          FROM t_team_local_ids ttli
                   JOIN t_teams tt ON ttli.GUID = tt.GUID),
     mostRecentSeason AS (SELECT id                        AS seasonID,
                                 RANK() OVER (ORDER BY id) AS seasonNum
                          FROM t_seasons
                                   JOIN t_leagues ON t_seasons.historicalLeagueGUID = t_leagues.GUID
                                   JOIN t_franchise tf ON t_leagues.GUID = tf.leagueGUID
                          WHERE t_leagues.GUID = CAST(@leagueId AS BLOB)
                          ORDER BY ID DESC
                          LIMIT 1)
SELECT baseballPlayerGUID,
       tsea.ID                                        AS seasonId,
       s.seasonNum,
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
       tsb.*,
       100 * ((
                          ([hits] + [baseOnBalls] + [hitByPitch]) /
                          CAST(NULLIF([atBats] + [baseOnBalls] + [hitByPitch] + [sacrificeFlies], 0) AS [REAL]) +
                          (([hits] - [doubles] - [triples] - [homeruns]) + 2 * [doubles] + 3 * [triples] +
                           4 * [homeruns]) / CAST(NULLIF([atBats], 0) AS [REAL])
                  ) / @leagueOps)                     AS opsPlus,
       -- sortOrder is a weighted OPS+ based on number of at bats
       atBats * 100 * ((
                                   ([hits] + [baseOnBalls] + [hitByPitch]) /
                                   CAST(NULLIF([atBats] + [baseOnBalls] + [hitByPitch] + [sacrificeFlies], 0) AS [REAL]) +
                                   (([hits] - [doubles] - [triples] - [homeruns]) + 2 * [doubles] + 3 * [triples] +
                                    4 * [homeruns]) / CAST(NULLIF([atBats], 0) AS [REAL])
                           ) / @leagueOps)            AS sortOrder,
       currentTeam.teamName                           AS teamName,
       mostRecentTeam.teamName                        AS mostRecentlyPlayedTeamName,
       previouslyRecentPlayedTeam.teamName            AS previousRecentlyPlayedTeamName,
       tbp.age                                        AS age
FROM [v_baseball_player_info] vbpi
         LEFT JOIN t_baseball_player_local_ids tbpli ON vbpi.baseballPlayerGUID = tbpli.GUID
         LEFT JOIN t_stats_players tsp ON tbpli.localID = tsp.baseballPlayerLocalID
         LEFT JOIN t_stats ts ON tsp.statsPlayerID = ts.statsPlayerID
         LEFT JOIN t_stats_batting tsb ON ts.aggregatorID = tsb.aggregatorID

         LEFT JOIN t_baseball_players tbp ON tbpli.GUID = tbp.GUID

         LEFT JOIN t_season_stats tss ON ts.aggregatorID = tss.aggregatorID

         JOIN t_seasons tsea ON tss.seasonID = tsea.ID
         JOIN mostRecentSeason s ON tsea.ID = s.seasonID
         JOIN t_league_local_ids tlli ON tsp.leagueLocalID = tlli.localID
         JOIN t_leagues tl ON tlli.GUID = tl.GUID

         LEFT JOIN t_baseball_player_options secondaryPosition
                   ON tbpli.localID = secondaryPosition.baseballPlayerLocalID AND secondaryPosition.optionKey = 55

         LEFT JOIN t_team_local_ids tt1 ON ts.currentTeamLocalID = tt1.localID
         LEFT JOIN t_team_local_ids tt2 ON ts.mostRecentlyPlayedTeamLocalID = tt2.localID
         LEFT JOIN t_team_local_ids tt3 ON ts.previousRecentlyPlayedTeamLocalID = tt3.localID
         LEFT JOIN teams currentTeam ON tt1.GUID = currentTeam.teamGUID
         LEFT JOIN teams mostRecentTeam ON tt2.GUID = mostRecentTeam.teamGUID
         LEFT JOIN teams previouslyRecentPlayedTeam ON tt3.GUID = previouslyRecentPlayedTeam.teamGUID
WHERE tl.GUID = CAST(@leagueId AS BLOB)
ORDER BY sortOrder DESC