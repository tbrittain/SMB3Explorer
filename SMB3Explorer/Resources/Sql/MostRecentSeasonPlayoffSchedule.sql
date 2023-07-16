WITH teams AS
         (SELECT ttli.GUID AS teamGUID, ttli.localID, tt.teamName
          FROM t_team_local_ids ttli
                   JOIN t_teams tt ON ttli.GUID = tt.GUID
                   JOIN t_division_teams t on tt.GUID = t.teamGUID
                   JOIN t_divisions d on t.divisionGUID = d.GUID
                   JOIN t_conferences c on d.conferenceGUID = c.GUID
                   JOIN t_leagues l on c.leagueGUID = l.GUID
                   JOIN t_franchise tf ON l.GUID = tf.leagueGUID
          WHERE l.name = 'Baseball United v3'),
     mostRecentSeason AS (SELECT id                        AS seasonID,
                                 t_seasons.GUID            AS seasonGUID,
                                 RANK() OVER (ORDER BY id) AS seasonNum
                          FROM t_seasons
                                   JOIN t_leagues ON t_seasons.historicalLeagueGUID = t_leagues.GUID
                                   JOIN t_franchise tf ON t_leagues.GUID = tf.leagueGUID
                          WHERE t_leagues.name = 'Baseball United v3'
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
       pg.seriesNumber,
       pg.team1GUID,
       pg.team1Standing,
       pg.team2GUID,
       pg.team2Standing,
       pg.gameNumber,
       pg.homeTeamLocalID,
       pg.awayTeamLocalID,
       pg.homeRunsScored,
       pg.awayRunsScored,
       pg.homePitcherLocalID,
       pg.awayPitcherLocalID
FROM playoffGames pg