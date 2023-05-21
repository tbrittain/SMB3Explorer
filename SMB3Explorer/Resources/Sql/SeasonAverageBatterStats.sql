WITH mostRecentSeason AS (SELECT id                        AS seasonID,
                                 RANK() OVER (ORDER BY id) AS seasonNum
                          FROM t_seasons
                                   JOIN t_leagues ON t_seasons.historicalLeagueGUID = t_leagues.GUID
                                   JOIN t_franchise tf ON t_leagues.GUID = tf.leagueGUID
                          WHERE t_leagues.GUID = CAST(@leagueId AS BLOB)
                          ORDER BY ID DESC
                          LIMIT 1)
SELECT AVG(
                       ([hits] + [baseOnBalls] + [hitByPitch]) /
                       CAST(NULLIF([atBats] + [baseOnBalls] + [hitByPitch] + [sacrificeFlies], 0) AS [REAL]) +
                       (([hits] - [doubles] - [triples] - [homeruns]) + 2 * [doubles] + 3 * [triples] +
                        4 * [homeruns]) / CAST(NULLIF([atBats], 0) AS [REAL])
           )
             AS ops,
       AVG(
                   ((0.69 * baseOnBalls) + (0.72 * hitByPitch) + (0.89 * (hits - doubles - triples - homeruns)) +
                    (1.27 * doubles) + (1.62 * triples) + (2.10 * homeruns)) /
                   (atBats + baseOnBalls + hitByPitch + sacrificeFlies)
           ) AS wOBA,
       (
               -- static value for runCS, although in fWAR this changes from season to season
               ((SUM(stolenBases) * 0.2) + (SUM(caughtStealing) * -0.384)) /
               -- no IBB to subtract from the denominator
               (SUM(hits - doubles - triples - homeruns) + SUM(baseOnBalls) + SUM(hitByPitch))
           ) AS leagueStolenBaseRuns,
    -- this does not account for conferences unfortunately, so in WAR calculation
    -- we will need to divide this by 2
    SUM(atBats + baseOnBalls + hitByPitch + sacrificeFlies) AS leaguePlateAppearances
FROM [v_baseball_player_info] vbpi
         LEFT JOIN t_baseball_player_local_ids tbpli ON vbpi.baseballPlayerGUID = tbpli.GUID
         LEFT JOIN t_stats_players tsp ON tbpli.localID = tsp.baseballPlayerLocalID
         LEFT JOIN t_stats ts ON tsp.statsPlayerID = ts.statsPlayerID
         LEFT JOIN t_stats_batting tsb ON ts.aggregatorID = tsb.aggregatorID
         LEFT JOIN t_baseball_players tbp ON tbpli.GUID = tbp.GUID
         LEFT JOIN t_season_stats tss ON ts.aggregatorID = tss.aggregatorID
         JOIN t_seasons tsea ON tss.seasonID = tsea.ID
         JOIN mostRecentSeason ON mostRecentSeason.seasonID = tsea.ID