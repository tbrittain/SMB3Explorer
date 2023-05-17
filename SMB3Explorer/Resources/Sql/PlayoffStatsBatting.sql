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
SELECT ts.aggregatorID                   AS aggregatorID,
       ts.statsPlayerID                  AS statsPlayerID,
       tbpli.GUID                        AS baseballPlayerGUIDIfKnown,
       s.seasonID,
       s.seasonNum,
       currentTeamLocal.GUID             AS teamGUID,
       mostRecentTeamLocal.GUID          AS mostRecentlyPlayedTeamGUID,
       previouslyRecentTeam.GUID         AS previousRecentlyPlayedTeamGUID,
       CASE
           WHEN tsp.baseballPlayerLocalID IS NULL THEN tsp.firstName
           ELSE vbpi.firstName END       AS firstName,
       CASE
           WHEN tsp.baseballPlayerLocalID IS NULL THEN tsp.lastName
           ELSE vbpi.lastName END        AS lastName,
       CASE
           WHEN tsp.baseballPlayerLocalID IS NULL THEN tsp.primaryPos
           ELSE vbpi.primaryPosition END AS primaryPosition,
       CASE
           WHEN tsp.baseballPlayerLocalID IS NULL THEN tsp.pitcherRole
           ELSE vbpi.pitcherRole END     AS pitcherRole,
       gamesBatting,
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
         JOIN t_stats_players tsp USING (statsPlayerID)
         JOIN t_playoff_stats tps USING (aggregatorID)
         JOIN seasons s USING (seasonID)

         LEFT JOIN t_team_local_ids currentTeamLocal ON ts.currentTeamLocalID = currentTeamLocal.localID
         LEFT JOIN t_team_local_ids mostRecentTeamLocal
                   ON ts.mostRecentlyPlayedTeamLocalID = mostRecentTeamLocal.localID
         LEFT JOIN t_team_local_ids previouslyRecentTeam
                   ON ts.previousRecentlyPlayedTeamLocalID = previouslyRecentTeam.localID

         LEFT JOIN t_baseball_player_local_ids tbpli
                   ON tsp.baseballPlayerLocalID = tbpli.localID
         LEFT JOIN v_baseball_player_info vbpi ON tbpli.GUID =
                                                  vbpi.baseballPlayerGUID
WHERE 1 = CASE
              WHEN :seasonId IS NOT NULL THEN CASE
                                                  WHEN s.seasonID = :seasonId THEN 1
                                                  ELSE 0 END
              ELSE 1 END
ORDER BY s.seasonNum, ts.statsPlayerID;
