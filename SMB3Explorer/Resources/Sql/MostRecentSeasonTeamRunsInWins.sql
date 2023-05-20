CREATE VIEW v_franchise_most_recent_season_runs_in_wins AS
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
                               conferenceGUID,
                               divisionGUID,
                               t_team_local_ids.[GUID]        AS teamGUID,
                               SUM(gamesWon)                  AS gamesWon,
                               SUM(gamesLost)                 AS gamesLost,
                               COALESCE(SUM(runsFor), 0)      AS runsFor,
                               COALESCE(SUM(runsAgainst), 0)  AS runsAgainst,
                               COALESCE(SUM(runsInWins), 0)   AS runsInWins,
                               COALESCE(SUM(runsInLosses), 0) AS runsInLosses
                        FROM (SELECT tsg.[seasonID],
                                     tgr.[homeTeamLocalID]                                                    AS teamLocalID,
                                     CASE WHEN homeRunsScored > awayRunsScored THEN 1 ELSE 0 END              AS gamesWon,
                                     CASE WHEN homeRunsScored < awayRunsScored THEN 1 ELSE 0 END              AS gamesLost,
                                     homeRunsScored                                                           AS runsFor,
                                     awayRunsScored                                                           AS runsAgainst,
                                     CASE WHEN homeRunsScored > awayRunsScored THEN homeRunsScored ELSE 0 END AS runsInWins,
                                     CASE WHEN homeRunsScored < awayRunsScored THEN homeRunsScored ELSE 0 END AS runsInLosses
                              FROM t_game_results tgr
                                       JOIN t_season_games tsg ON tgr.[ID] = tsg.[gameID]
                                       JOIN t_seasons ts on tsg.seasonID = ts.ID
                                       JOIN mostRecentSeason mrs ON ts.ID = mrs.seasonID
                              UNION ALL
                              SELECT tsg2.[seasonID],
                                     tgr2.[awayTeamLocalID]                                                   AS teamLocalID,
                                     CASE WHEN homeRunsScored < awayRunsScored THEN 1 ELSE 0 END              AS gamesWon,
                                     CASE WHEN homeRunsScored > awayRunsScored THEN 1 ELSE 0 END              AS gamesLost,
                                     awayRunsScored                                                           AS runsFor,
                                     homeRunsScored                                                           AS runsAgainst,
                                     CASE WHEN homeRunsScored < awayRunsScored THEN awayRunsScored ELSE 0 END AS runsInWins,
                                     CASE WHEN homeRunsScored > awayRunsScored THEN awayRunsScored ELSE 0 END AS runsInLosses
                              FROM t_game_results tgr2
                                       JOIN t_season_games tsg2 ON tgr2.[ID] = tsg2.[gameID]
                                       JOIN t_seasons ts2 on tsg2.seasonID = ts2.ID
                                       JOIN mostRecentSeason mrs2 ON ts2.ID = mrs2.seasonID) t
                                 JOIN t_seasons ON t.[seasonID] = t_seasons.[ID]
                                 JOIN mostRecentSeason mrs ON t_seasons.ID = mrs.seasonID
                                 JOIN t_team_local_ids ON t.[teamLocalID] = t_team_local_ids.[localID]
                                 JOIN t_division_teams ON t_division_teams.[teamGUID] = t_team_local_ids.[GUID]
                                 JOIN t_divisions ON t_divisions.[GUID] = t_division_teams.[divisionGUID]
                                 JOIN t_conferences ON t_conferences.[GUID] = t_divisions.[conferenceGUID]
                        GROUP BY mrs.seasonNum, mrs.seasonID, conferenceGUID, divisionGUID, t_team_local_ids.[GUID])
SELECT seasonNum,
       seasonId,
       teamGUID,
       teamName,
       gamesWon,
       runsInWins,
       (runsInWins * 1.0 / gamesWon) AS avgRunsInWins
FROM currentSeasons
         JOIN t_teams tt ON teamGUID = tt.[GUID]
ORDER BY avgRunsInWins DESC
