WITH teams AS
         (SELECT ttli.GUID AS teamGUID, tt.teamName
          FROM t_team_local_ids ttli
                   JOIN t_teams tt ON ttli.GUID = tt.GUID)
SELECT ts.[aggregatorID]                                           AS [aggregatorID],
       ts.[statsPlayerID]                                          AS [statsPlayerID],
       tbpli.[GUID]                                                AS [baseballPlayerGUIDIfKnown],
       [currentTeam].teamName                                      AS [currentTeamName],
       [mostRecentTeam].teamName                                   AS [mostRecentTeamName],
       [secondMostRecentTeam].teamName                             AS [secondMostRecentTeamName],
       CASE
           WHEN tsp.[baseballPlayerLocalID] IS NULL THEN tsp.[firstName]
           ELSE vbpi.[firstName] END                               AS firstName,
       CASE
           WHEN tsp.[baseballPlayerLocalID] IS NULL THEN tsp.[lastName]
           ELSE vbpi.[lastName] END                                AS lastName,

       tsp.retirementSeason,
       tsp.age,

       CASE
           WHEN tsp.[baseballPlayerLocalID] IS NULL THEN tsp.[primaryPos]
           ELSE vbpi.[primaryPosition] END                         AS primaryPosition,

       CASE
           WHEN tsp.[baseballPlayerLocalID] IS NULL THEN tsp.[secondaryPos]
           ELSE CAST(secondaryPosition.optionValue AS INTEGER) END AS secondaryPosition,

       CASE
           WHEN tsp.[baseballPlayerLocalID] IS NULL THEN tsp.[pitcherRole]
           ELSE vbpi.[pitcherRole] END                             AS pitcherRole,
       [gamesPlayed],
       [gamesBatting],
       [atBats],
       [runs],
       [hits],
       [doubles],
       [triples],
       [homeruns],
       [rbi],
       [stolenBases],
       [caughtStealing],
       [baseOnBalls],
       [strikeOuts],
       [hitByPitch],
       [sacrificeHits],
       [sacrificeFlies],
       [errors],
       [passedBalls]
FROM [t_stats_batting] tsbat
         JOIN [t_stats] ts ON tsbat.[aggregatorID] = ts.[aggregatorID]
         JOIN [t_stats_players] tsp USING ([statsPlayerID])

         LEFT JOIN [t_team_local_ids] t1 ON ts.[currentTeamLocalID] = t1.[localID]
         LEFT JOIN [t_team_local_ids] t2
                   ON ts.[mostRecentlyPlayedTeamLocalID] = t2.[localID]
         LEFT JOIN [t_team_local_ids] t3
                   ON ts.[previousRecentlyPlayedTeamLocalID] = t3.[localID]

         LEFT JOIN [teams] currentTeam ON currentTeam.teamGUID = t1.GUID
         LEFT JOIN [teams] mostRecentTeam ON mostRecentTeam.teamGUID = t2.GUID
         LEFT JOIN [teams] secondMostRecentTeam ON secondMostRecentTeam.teamGUID = t3.GUID

         LEFT JOIN [t_baseball_player_local_ids] tbpli
                   ON tsp.[baseballPlayerLocalID] = tbpli.[localID]
         LEFT JOIN [v_baseball_player_info] vbpi ON tbpli.[GUID] =
                                                    vbpi.[baseballPlayerGUID]

         LEFT JOIN t_baseball_player_options secondaryPosition
                   ON tbpli.localID = secondaryPosition.baseballPlayerLocalID AND secondaryPosition.optionKey = 55

         JOIN t_league_local_ids tlli ON tsp.leagueLocalID = tlli.localID
         JOIN t_leagues tl ON tlli.GUID = tl.GUID
         LEFT JOIN t_season_stats tss ON ts.aggregatorID = tss.aggregatorID
         JOIN t_career_playoff_stats tcss ON ts.aggregatorID = tcss.aggregatorID
WHERE tl.GUID = CAST(@leagueId AS BLOB)
ORDER BY gamesPlayed DESC