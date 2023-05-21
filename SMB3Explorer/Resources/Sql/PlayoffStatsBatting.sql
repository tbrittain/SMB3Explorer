WITH teams AS
         (SELECT ttli.GUID AS teamGUID, tt.teamName
          FROM t_team_local_ids ttli
                   JOIN t_teams tt ON ttli.GUID = tt.GUID),
     seasons AS (SELECT id                        AS seasonID,
                        RANK() OVER (ORDER BY id) AS seasonNum
                 FROM t_seasons
                          JOIN t_leagues ON t_seasons.historicalLeagueGUID = t_leagues.GUID
                          JOIN t_franchise tf ON t_leagues.GUID = tf.leagueGUID
                 WHERE t_leagues.GUID = CAST(@leagueId AS BLOB))
SELECT ts.aggregatorID                     AS aggregatorID,
       ts.statsPlayerID                    AS statsPlayerID,
       tbpli.GUID                          AS baseballPlayerGUIDIfKnown,
       s.seasonID,
       s.seasonNum,
       currentTeam.teamName                AS teamName,
       mostRecentTeam.teamName             AS mostRecentlyPlayedTeamName,
       previouslyRecentPlayedTeam.teamName AS previousRecentlyPlayedTeamName,
       CASE
           WHEN tsplayers.baseballPlayerLocalID IS NULL THEN tsplayers.firstName
           ELSE vbpi.firstName END         AS firstName,
       CASE
           WHEN tsplayers.baseballPlayerLocalID IS NULL THEN tsplayers.lastName
           ELSE vbpi.lastName END          AS lastName,
       CASE
           WHEN tsplayers.baseballPlayerLocalID IS NULL THEN tsplayers.primaryPos
           ELSE vbpi.primaryPosition END   AS primaryPosition,
       CASE
           WHEN tsplayers.baseballPlayerLocalID IS NOT NULL THEN CAST(secondaryPosition.optionValue AS INTEGER)
           ELSE tsplayers.secondaryPos END AS secondaryPosition,
       CASE
           WHEN tsplayers.baseballPlayerLocalID IS NULL THEN tsplayers.pitcherRole
           ELSE vbpi.pitcherRole END       AS pitcherRole,
       CASE
           WHEN tbp.GUID IS NOT NULL THEN
                   tbp.age - (MAX(seasonNum) OVER (PARTITION BY baseballPlayerGUID) - seasonNum)
           ELSE tsplayers.age - (tsplayers.retirementSeason - s.seasonNum)
           END                             AS age,
       gamesBatting,
       gamesPlayed,
       atBats,
       runs,
       hits,
       doubles,
       triples,
       homeruns,
       rbi,
       stolenBases,
       caughtStealing,
       baseOnBalls,
       strikeOuts,
       hitByPitch,
       sacrificeHits,
       sacrificeFlies,
       errors,
       passedBalls
FROM t_stats_batting tsb
         JOIN t_stats ts ON tsb.aggregatorID = ts.aggregatorID
         JOIN t_stats_players tsplayers USING (statsPlayerID)
         JOIN t_playoff_stats tps USING (aggregatorID)
         JOIN seasons s USING (seasonID)

         LEFT JOIN t_team_local_ids currentTeamLocal ON ts.currentTeamLocalID = currentTeamLocal.localID
         LEFT JOIN t_team_local_ids mostRecentTeamLocal
                   ON ts.mostRecentlyPlayedTeamLocalID = mostRecentTeamLocal.localID
         LEFT JOIN t_team_local_ids previouslyRecentPlayedTeamLocal
                   ON ts.previousRecentlyPlayedTeamLocalID = previouslyRecentPlayedTeamLocal.localID

         LEFT JOIN teams currentTeam ON currentTeamLocal.GUID = currentTeam.teamGUID
         LEFT JOIN teams mostRecentTeam ON mostRecentTeamLocal.GUID = mostRecentTeam.teamGUID
         LEFT JOIN teams previouslyRecentPlayedTeam
                   ON previouslyRecentPlayedTeamLocal.GUID = previouslyRecentPlayedTeam.teamGUID

         LEFT JOIN t_baseball_player_local_ids tbpli
                   ON tsplayers.baseballPlayerLocalID = tbpli.localID
         LEFT JOIN t_baseball_player_options secondaryPosition
                   ON tbpli.localID = secondaryPosition.baseballPlayerLocalID AND secondaryPosition.optionKey = 55
         LEFT JOIN t_baseball_players tbp on tbpli.GUID = tbp.GUID
         LEFT JOIN v_baseball_player_info vbpi ON tbpli.GUID =
                                                  vbpi.baseballPlayerGUID
ORDER BY s.seasonNum, ts.statsPlayerID;
