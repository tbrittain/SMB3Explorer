WITH leagueSeasons AS
         (SELECT ts.*
          FROM t_seasons ts
                   JOIN t_leagues tl ON ts.historicalLeagueGUID = tl.GUID
          WHERE tl.GUID = CAST(@leagueId AS BLOB)
          GROUP BY ts.ID)
SELECT ROW_NUMBER() OVER (
    ORDER BY ls.ID, vss.conferenceGUID, vss.divisionGUID, vss.gamesBack, vss.runsFor - vss.runsAgainst DESC
    )                                      AS `index`,
       ls.ID                               AS seasonID,
       DENSE_RANK() OVER (ORDER BY ls.ID ) AS seasonNum,
       vss.*,
       vss.runsFor - vss.runsAgainst       AS runDifferential,
       tc.name                             AS conferenceName,
       td.name                             AS divisionName,
       tt.teamName                         AS teamName
FROM v_season_standings vss
         JOIN leagueSeasons ls ON vss.seasonGUID = ls.GUID
         JOIN t_conferences tc ON vss.conferenceGUID = tc.GUID
         JOIN t_divisions td ON vss.divisionGUID = td.GUID
         JOIN t_teams tt ON vss.teamGUID = tt.GUID
ORDER BY ls.ID, vss.conferenceGUID, vss.divisionGUID, vss.gamesBack, runDifferential DESC