WITH teams AS
         (SELECT ttli.GUID AS teamGUID, ttli.localID, tt.teamName
          FROM t_team_local_ids ttli
                   JOIN t_teams tt ON ttli.GUID = tt.GUID
                   JOIN t_division_teams t on tt.GUID = t.teamGUID
                   JOIN t_divisions d on t.divisionGUID = d.GUID
                   JOIN t_conferences c on d.conferenceGUID = c.GUID
                   JOIN t_leagues l on c.leagueGUID = l.GUID
          WHERE l.GUID = CAST(@leagueId AS BLOB)),
     mostRecentSeason AS (SELECT id                        AS seasonID,
                                 t_seasons.GUID            AS seasonGUID,
                                 RANK() OVER (ORDER BY id) AS seasonNum
                          FROM t_seasons
                                   JOIN t_leagues ON t_seasons.historicalLeagueGUID = t_leagues.GUID
                          WHERE t_leagues.GUID = CAST(@leagueId AS BLOB)
                          ORDER BY ID DESC
                          LIMIT 1),
     mostRecentSeasonPlayoff AS (SELECT GUID AS playoffGUID,
                                        mostRecentSeason.seasonNum,
                                        mostRecentSeason.seasonID
                                 FROM t_playoffs tp
                                          JOIN mostRecentSeason ON mostRecentSeason.seasonGUID = tp.seasonGUID),
     playoffSeries AS (SELECT mostRecentSeasonPlayoff.seasonID,
                              mostRecentSeasonPlayoff.seasonNum,
                              tps.*
                       FROM t_playoff_series tps
                                JOIN mostRecentSeasonPlayoff
                       WHERE tps.playoffGUID = mostRecentSeasonPlayoff.playoffGUID),
     playoffGames AS (SELECT playoffSeries.*,
                             RANK() OVER (ORDER BY tgr.ID) AS gameNumber,
                             tgr.*
                      FROM t_playoff_games tpg
                               JOIN playoffSeries ON tpg.playoffGUID = playoffSeries.playoffGUID
                          AND tpg.seriesNumber = playoffSeries.seriesNumber
                               JOIN t_game_results tgr on tpg.gameID = tgr.ID)
SELECT pg.seasonID,
       pg.seasonNum,
       pg.seriesNumber + 1                                  AS seriesNumber,
       pg.team1GUID,
       team1.teamName                                       AS team1Name,
       pg.team1Standing + 1                                 AS team1Standing,
       pg.team2GUID,
       team2.teamName                                       AS team2Name,
       pg.team2Standing + 1                                 AS team2Standing,
       pg.gameNumber,
       homeTeam.teamGUID                                    AS homeTeamId,
       homeTeam.teamName                                    AS homeTeamName,
       awayTeam.teamGUID                                    AS awayTeamId,
       awayTeam.teamName                                    AS awayTeamName,
       pg.homeRunsScored,
       pg.awayRunsScored,
       homePitcher.baseballPlayerGUID                       AS homePitcherId,
       homePitcher.firstName || ' ' || homePitcher.lastName AS homePitcherName,
       awayPitcher.baseballPlayerGUID                       AS awayPitcherId,
       awayPitcher.firstName || ' ' || awayPitcher.lastName AS awayPitcherName
FROM playoffGames pg
         JOIN teams team1 ON team1.teamGUID = pg.team1GUID
         JOIN teams team2 ON team2.teamGUID = pg.team2GUID

         JOIN t_team_local_ids homeTeamLocal ON pg.homeTeamLocalID = homeTeamLocal.localID
         JOIN teams homeTeam ON homeTeamLocal.GUID = homeTeam.teamGUID

         JOIN t_team_local_ids awayTeamLocal ON pg.awayTeamLocalID = awayTeamLocal.localID
         JOIN teams awayTeam ON awayTeamLocal.GUID = awayTeam.teamGUID

         LEFT JOIN t_baseball_player_local_ids homePitcherLocal ON pg.homePitcherLocalID = homePitcherLocal.localID
         LEFT JOIN v_baseball_player_info homePitcher ON homePitcherLocal.GUID = homePitcher.baseballPlayerGUID

         LEFT JOIN t_baseball_player_local_ids awayPitcherLocal ON pg.awayPitcherLocalID = awayPitcherLocal.localID
         LEFT JOIN v_baseball_player_info awayPitcher ON awayPitcherLocal.GUID = awayPitcher.baseballPlayerGUID
ORDER BY pg.seriesNumber, pg.gameNumber