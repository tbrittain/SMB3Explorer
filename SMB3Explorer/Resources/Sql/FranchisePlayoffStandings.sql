WITH leagueSeasons AS
         (SELECT ts.*
          FROM t_seasons ts
                   JOIN t_leagues tl ON ts.historicalLeagueGUID = tl.GUID
          WHERE tl.GUID = CAST(@leagueId AS BLOB)
          GROUP BY ts.ID),
     seasonPlayoffs AS (SELECT ts.ID AS seasonID,
                               vpgwl1.playoffGUID,
                               vpgwl1.conferenceGUID,
                               vpgwl1.divisionGUID,
                               vpgwl1.teamGUID,
                               gamesWon,
                               gamesLost,
                               runsFor,
                               runsAgainst
                        FROM v_playoff_games_won_lost vpgwl1
                                 JOIN (SELECT playoffGUID,
                                              conferenceGUID,
                                              divisionGUID,
                                              MAX(gamesWon - gamesLost) AS maxGameDiff
                                       FROM v_playoff_games_won_lost vpgwl2
                                       GROUP BY playoffGUID, conferenceGUID, divisionGUID) gameDiff
                                      ON vpgwl1.playoffGUID = gameDiff.playoffGUID AND
                                         vpgwl1.conferenceGUID = gameDiff.conferenceGUID AND
                                         vpgwl1.divisionGUID = gameDiff.divisionGUID
                                 JOIN t_playoffs tp ON tp.GUID = vpgwl1.playoffGUID
                                 JOIN t_seasons ts ON ts.GUID = tp.seasonGUID)
SELECT ROW_NUMBER() OVER (
    ORDER BY ls.ID, gamesWon DESC
    )                                      AS `index`,
       ls.ID                               AS seasonID,
       DENSE_RANK() OVER (ORDER BY ls.ID ) AS seasonNum,
       vss.*,
       vss.runsFor - vss.runsAgainst       AS runDifferential,
       tc.name                             AS conferenceName,
       td.name                             AS divisionName,
       tt.teamName                         AS teamName
FROM seasonPlayoffs vss
         JOIN leagueSeasons ls ON vss.seasonID = ls.ID
         JOIN t_conferences tc ON vss.conferenceGUID = tc.GUID
         JOIN t_divisions td ON vss.divisionGUID = td.GUID
         JOIN t_teams tt ON vss.teamGUID = tt.GUID
ORDER BY ls.ID, gamesWon DESC