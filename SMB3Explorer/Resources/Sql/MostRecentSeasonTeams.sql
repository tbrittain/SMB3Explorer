WITH teams AS
         (SELECT ttli.localID AS teamLocalID,
                 ttli.GUID    AS teamGUID,
                 tt.teamName,
                 td.GUID      AS divisionGUID,
                 td.name      AS divisionName,
                 tc.GUID      AS conferenceGUID,
                 tc.name      AS conferenceName
          FROM t_team_local_ids ttli
                   JOIN t_teams tt ON ttli.GUID = tt.GUID
                   JOIN t_division_teams tdt ON tt.GUID = tdt.teamGUID
                   JOIN t_divisions td ON tdt.divisionGUID = td.GUID
                   JOIN t_conferences tc ON td.conferenceGUID = tc.GUID),
     mostRecentSeason AS (SELECT id                        AS seasonID,
                                 t_seasons.GUID            AS seasonGUID,
                                 incomePerTick * 200       AS payrollMax,
                                 RANK() OVER (ORDER BY id) AS seasonNum
                          FROM t_seasons
                                   JOIN t_leagues ON t_seasons.historicalLeagueGUID = t_leagues.GUID
                                   JOIN t_franchise tf ON t_leagues.GUID = tf.leagueGUID
                          WHERE t_leagues.GUID = CAST(@leagueId AS BLOB)
                          ORDER BY ID DESC
                          LIMIT 1),
     mostRecentSeasonStandings AS (SELECT fs.seasonID                                                  AS seasonID,
                                          vss.teamGUID                                                 AS teamGUID,
                                          vss.gamesWon                                                 AS wins,
                                          vss.gamesLost                                                AS losses,
                                          vss.gamesBack                                                AS gamesBack,
                                          CAST(vss.gamesWon AS FLOAT) / (vss.gamesWon + vss.gamesLost) AS winPercentage,
                                          vss.runsFor - vss.runsAgainst                                AS runDifferential,
                                          vss.runsFor                                                  AS runsFor,
                                          vss.runsAgainst                                              AS runsAgainst
                                   FROM v_season_standings vss
                                            JOIN mostRecentSeason fs ON vss.seasonGUID = fs.seasonGUID
                                            JOIN t_teams tt ON vss.teamGUID = tt.GUID)
SELECT teams.*,
       tsea.ID                                                      AS seasonId,
       s.seasonNum,
       payrollMax                                                   AS budget,
       SUM(salary.salary * 200)                                     AS payroll,
       payrollMax - SUM(salary.salary * 200)                        AS surplus,
       (payrollMax - SUM(salary.salary * 200)) / tfscp.seasonLength AS surplusPerGame,
       mostRecentSeasonStandings.wins,
       mostRecentSeasonStandings.losses,
       mostRecentSeasonStandings.gamesBack,
       mostRecentSeasonStandings.winPercentage,
       mostRecentSeasonStandings.runDifferential,
       mostRecentSeasonStandings.runsFor,
       mostRecentSeasonStandings.runsAgainst,
       SUM(tbp.power)                                               AS power,
       SUM(tbp.contact)                                             AS contact,
       SUM(tbp.speed)                                               AS speed,
       SUM(tbp.fielding)                                            AS fielding,
       SUM(tbp.arm)                                                 AS arm,
       SUM(tbp.velocity)                                            AS velocity,
       SUM(tbp.junk)                                                AS junk,
       SUM(tbp.accuracy)                                            AS accuracy

FROM [v_baseball_player_info] vbpi
         LEFT JOIN t_baseball_player_local_ids tbpli ON vbpi.baseballPlayerGUID = tbpli.GUID
         LEFT JOIN t_stats_players tsp ON tbpli.localID = tsp.baseballPlayerLocalID
         LEFT JOIN t_stats ts ON tsp.statsPlayerID = ts.statsPlayerID
         LEFT JOIN t_season_stats tss ON ts.aggregatorID = tss.aggregatorID
         JOIN t_baseball_players tbp ON tbpli.GUID = tbp.GUID

         JOIN t_seasons tsea ON tss.seasonID = tsea.ID
         JOIN mostRecentSeason s ON tsea.ID = s.seasonID
         JOIN t_league_local_ids tlli ON tsp.leagueLocalID = tlli.localID
         JOIN t_leagues tl ON tlli.GUID = tl.GUID

         JOIN t_franchise tf ON tl.GUID = tf.leagueGUID
         JOIN t_franchise_season_creation_params tfscp ON tf.GUID = tfscp.franchiseGUID

         JOIN teams ON ts.currentTeamLocalID = teams.teamLocalID
         JOIN t_salary salary
              ON vbpi.baseballPlayerGUID = salary.baseballPlayerGUID

         JOIN mostRecentSeasonStandings ON mostRecentSeasonStandings.teamGUID = teams.teamGUID

WHERE tl.GUID = CAST(@leagueId AS BLOB)
  AND ts.currentTeamLocalID IS NOT NULL
GROUP BY teams.teamLocalID
ORDER BY payroll DESC