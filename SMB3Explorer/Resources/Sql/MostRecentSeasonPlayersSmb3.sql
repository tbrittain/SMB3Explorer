﻿WITH teams AS
         (SELECT ttli.GUID AS teamGUID, tt.teamName
          FROM t_team_local_ids ttli
                   JOIN t_teams tt ON ttli.GUID = tt.GUID),
     mostRecentSeason AS (SELECT id                        AS seasonID,
                                 RANK() OVER (ORDER BY id) AS seasonNum
                          FROM t_seasons
                                   JOIN t_leagues ON t_seasons.historicalLeagueGUID = t_leagues.GUID
                          WHERE t_leagues.GUID = CAST(@leagueId AS BLOB)
                          ORDER BY ID DESC
                          LIMIT 1)
SELECT vbpi.baseballPlayerGUID,
       tsea.ID                                            AS seasonId,
       s.seasonNum,
       CASE
           WHEN tsp.[baseballPlayerLocalID] IS NULL THEN tsp.[firstName]
           ELSE vbpi.[firstName] END                      AS firstName,
       CASE
           WHEN tsp.[baseballPlayerLocalID] IS NULL THEN tsp.[lastName]
           ELSE vbpi.[lastName] END                       AS lastName,
       CASE
           WHEN tsp.[baseballPlayerLocalID] IS NULL THEN tsp.[primaryPos]
           ELSE vbpi.[primaryPosition] END                AS primaryPosition,
       CASE
           WHEN tsp.[baseballPlayerLocalID] IS NULL THEN tsp.[pitcherRole]
           ELSE vbpi.[pitcherRole] END                    AS pitcherRole,
       CAST(secondaryPosition.optionValue AS INTEGER)     AS secondaryPosition,
       currentTeam.teamName                               AS currentTeam,
       previousTeam.teamName                              AS previousTeam,
       tbp.power,
       tbp.contact,
       tbp.speed,
       tbp.fielding,
       tbp.arm,
       tbp.velocity,
       tbp.junk,
       tbp.accuracy,
       tbp.age,
       IIF(salary.salary IS NULL, 0, salary.salary) * 200 AS salaryDollars,
       CASE
           WHEN COUNT(tbpt.trait) = 0 THEN NULL
           ELSE json_group_array(json_object('traitId', tbpt.trait, 'subtypeId', tbpt.subType))
           END                                            AS traits

FROM [v_baseball_player_info] vbpi
         LEFT JOIN t_baseball_player_local_ids tbpli ON vbpi.baseballPlayerGUID = tbpli.GUID
         LEFT JOIN t_stats_players tsp ON tbpli.localID = tsp.baseballPlayerLocalID
         LEFT JOIN t_stats ts ON tsp.statsPlayerID = ts.statsPlayerID
         LEFT JOIN t_season_stats tss ON ts.aggregatorID = tss.aggregatorID
         JOIN t_baseball_players tbp ON tbpli.GUID = tbp.GUID
         LEFT JOIN t_baseball_player_traits tbpt ON tbpli.localID = tbpt.baseballPlayerLocalID

         JOIN t_seasons tsea ON tss.seasonID = tsea.ID
         JOIN mostRecentSeason s ON tsea.ID = s.seasonID
         JOIN t_league_local_ids tlli ON tsp.leagueLocalID = tlli.localID
         JOIN t_leagues tl ON tlli.GUID = tl.GUID

         LEFT JOIN t_salary salary ON vbpi.baseballPlayerGUID = salary.baseballPlayerGUID

         LEFT JOIN t_baseball_player_options secondaryPosition
                   ON tbpli.localID = secondaryPosition.baseballPlayerLocalID AND secondaryPosition.optionKey = 55

         LEFT JOIN [t_team_local_ids] tt1 ON ts.[currentTeamLocalID] = tt1.[localID]
         LEFT JOIN [t_team_local_ids] tt2
                   ON ts.[previousRecentlyPlayedTeamLocalID] = tt2.[localID]
         LEFT JOIN teams currentTeam ON tt1.GUID = currentTeam.teamGUID
         LEFT JOIN teams previousTeam ON tt2.GUID = previousTeam.teamGUID

WHERE tl.GUID = CAST(@leagueId AS BLOB)
GROUP BY vbpi.baseballPlayerGUID, currentTeam, salaryDollars
ORDER BY currentTeam IS NULL, currentTeam, salaryDollars DESC