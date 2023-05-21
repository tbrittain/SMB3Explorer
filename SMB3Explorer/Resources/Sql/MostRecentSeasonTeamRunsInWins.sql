WITH mostRecentSeason AS (SELECT id                        AS seasonID,
                                 RANK() OVER (ORDER BY id) AS seasonNum
                          FROM t_seasons
                                   JOIN t_leagues ON t_seasons.historicalLeagueGUID = t_leagues.GUID
                                   JOIN t_franchise tf ON t_leagues.GUID = tf.leagueGUID
                          WHERE t_leagues.GUID = CAST(@leagueId AS BLOB)
                          ORDER BY ID DESC
                          LIMIT 1),
     currentSeasons AS (SELECT mrs.seasonNum,
                               mrs.seasonID,
                               t_team_local_ids.[GUID]        AS teamGUID,
                               SUM(gamesWon)                  AS gamesWon,
                               COALESCE(SUM(runsFor), 0)      AS runsFor,
                               COALESCE(SUM(runsInWins), 0)   AS runsInWins
                        FROM (SELECT tsg.[seasonID],
                                     tgr.[homeTeamLocalID]                                                    AS teamLocalID,
                                     CASE WHEN homeRunsScored > awayRunsScored THEN 1 ELSE 0 END              AS gamesWon,
                                     homeRunsScored                                                           AS runsFor,
                                     CASE WHEN homeRunsScored > awayRunsScored THEN homeRunsScored ELSE 0 END AS runsInWins
                              FROM t_game_results tgr
                                       JOIN t_season_games tsg ON tgr.[ID] = tsg.[gameID]
                                       JOIN t_seasons ts on tsg.seasonID = ts.ID
                                       JOIN mostRecentSeason mrs ON ts.ID = mrs.seasonID
                              UNION ALL
                              SELECT tsg2.[seasonID],
                                     tgr2.[awayTeamLocalID]                                                   AS teamLocalID,
                                     CASE WHEN homeRunsScored < awayRunsScored THEN 1 ELSE 0 END              AS gamesWon,
                                     awayRunsScored                                                           AS runsFor,
                                     CASE WHEN homeRunsScored < awayRunsScored THEN awayRunsScored ELSE 0 END AS runsInWins
                              FROM t_game_results tgr2
                                       JOIN t_season_games tsg2 ON tgr2.[ID] = tsg2.[gameID]
                                       JOIN t_seasons ts2 on tsg2.seasonID = ts2.ID
                                       JOIN mostRecentSeason mrs2 ON ts2.ID = mrs2.seasonID) t
                                 JOIN t_seasons ON t.[seasonID] = t_seasons.[ID]
                                 JOIN mostRecentSeason mrs ON t_seasons.ID = mrs.seasonID
                                 JOIN t_team_local_ids ON t.[teamLocalID] = t_team_local_ids.[localID]
                        GROUP BY mrs.seasonNum, mrs.seasonID, t_team_local_ids.[GUID])
SELECT seasonNum,
       seasonId,
       teamGUID,
       teamName,
       gamesWon,
       runsInWins,
       (runsInWins * 1.0 / gamesWon) AS avgRunsInWins
FROM currentSeasons
         JOIN t_teams tt ON teamGUID = tt.[GUID]
