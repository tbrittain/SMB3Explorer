WITH teams AS
         (SELECT ttli.GUID AS teamGUID, ttli.localID, tt.teamName
          FROM t_team_local_ids ttli
                   JOIN t_teams tt ON ttli.GUID = tt.GUID
                   JOIN t_division_teams t on tt.GUID = t.teamGUID
                   JOIN t_divisions d on t.divisionGUID = d.GUID
                   JOIN t_conferences c on d.conferenceGUID = c.GUID
                   JOIN t_leagues l on c.leagueGUID = l.GUID
                   JOIN t_franchise tf ON l.GUID = tf.leagueGUID
          WHERE l.GUID = CAST(@leagueId AS BLOB)),
     mostRecentSeason AS (SELECT id                        AS seasonID,
                                 RANK() OVER (ORDER BY id) AS seasonNum
                          FROM t_seasons
                                   JOIN t_leagues ON t_seasons.historicalLeagueGUID = t_leagues.GUID
                                   JOIN t_franchise tf ON t_leagues.GUID = tf.leagueGUID
                          WHERE t_leagues.GUID = CAST(@leagueId AS BLOB)
                          ORDER BY ID DESC
                          LIMIT 1),
     gameResults AS (SELECT (ROW_NUMBER() OVER (PARTITION BY tsg.seasonID ORDER BY tgr.ID) - 1) /
                            ((SELECT COUNT(*) FROM teams) / 2) + 1 AS day,
                            tgr.*
                     FROM t_game_results tgr
                              JOIN t_season_games tsg on tgr.ID = tsg.gameID
                              JOIN mostRecentSeason mrs on tsg.seasonID = mrs.seasonID
                              JOIN t_team_local_ids t_local_away on tgr.awayTeamLocalID = t_local_away.localID
                              JOIN t_team_local_ids t_local_home on tgr.homeTeamLocalID = t_local_home.localID
                     ORDER BY tgr.ID),
     seasonSchedule AS (SELECT tss.seasonID,
                               mrs.seasonNum,
                               RANK() OVER (PARTITION BY tss.seasonID ORDER BY rowid) as gameNumber,
                               (ROW_NUMBER() OVER (PARTITION BY tss.seasonID ORDER BY rowid) - 1) /
                               ((SELECT COUNT(*) FROM teams) / 2) + 1                 AS day,
                               tss.homeTeamID,
                               tss.awayTeamID
                        FROM t_season_schedule tss
                                 JOIN mostRecentSeason mrs ON tss.seasonID = mrs.seasonID)
SELECT ss.seasonID,
       ss.seasonNum,
       ss.gameNumber,
       ss.day,
       ss.homeTeamID,
       homeTeams.teamName                               AS homeTeamName,
       homeTeams.teamGUID                               AS homeTeamGUID,
       ss.awayTeamID,
       awayTeams.teamName                               AS awayTeamName,
       awayTeams.teamGUID                               AS awayTeamGUID,
       gr.homeRunsScored,
       gr.awayRunsScored,
       gr.homePitcherLocalID,
       vbpi_home.baseballPlayerGUID                     AS homePitcherGUID,
       vbpi_home.firstName || ' ' || vbpi_home.lastName AS homePitcherName,
       gr.awayPitcherLocalID,
       vbpi_away.baseballPlayerGUID                     AS awayPitcherGUID,
       vbpi_away.firstName || ' ' || vbpi_away.lastName AS awayPitcherName
FROM seasonSchedule ss
         LEFT JOIN gameResults gr
                   ON gr.homeTeamLocalID = ss.homeTeamID AND gr.awayTeamLocalID = ss.awayTeamID AND ss.day = gr.day

         JOIN teams homeTeams ON ss.homeTeamID = homeTeams.localID
         JOIN teams awayTeams ON ss.awayTeamID = awayTeams.localID

         LEFT JOIN t_baseball_player_local_ids tbpli_home ON gr.homePitcherLocalID = tbpli_home.localID
         LEFT JOIN t_baseball_player_local_ids tbpli_away ON gr.awayPitcherLocalID = tbpli_away.localID

         LEFT JOIN v_baseball_player_info vbpi_home ON tbpli_home.GUID = vbpi_home.baseballPlayerGUID
         LEFT JOIN v_baseball_player_info vbpi_away ON tbpli_away.GUID = vbpi_away.baseballPlayerGUID
ORDER BY gameNumber;
