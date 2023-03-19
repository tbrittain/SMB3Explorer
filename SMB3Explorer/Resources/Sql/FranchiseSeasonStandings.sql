WITH franchiseSeasons AS
         (SELECT ts.*
          FROM t_franchise tf
                   JOIN t_leagues tl ON tf.leagueGUID = tl.GUID
                   LEFT JOIN t_franchise_seasons tfs ON tf.GUID = tfs.franchiseGUID
                   LEFT JOIN t_seasons ts ON tfs.seasonGUID = ts.GUID
          WHERE tl.GUID = CAST(@leagueId AS BLOB)
          GROUP BY ts.ID)
SELECT ROW_NUMBER() OVER (
    ORDER BY fs.ID, vss.conferenceGUID, vss.divisionGUID, vss.gamesBack, vss.runsFor - vss.runsAgainst DESC
    )                                AS `index`,
       fs.ID                         AS seasonID,
       DENSE_RANK() OVER (ORDER BY fs.ID  ) AS seasonNum,
       vss.*,
       vss.runsFor - vss.runsAgainst AS runDifferential,
       tc.name                       AS conferenceName,
       td.name                       AS divisionName,
       tt.teamName                   AS teamName
FROM v_season_standings vss
         JOIN franchiseSeasons fs ON vss.seasonGUID = fs.GUID
         JOIN t_conferences tc ON vss.conferenceGUID = tc.GUID
         JOIN t_divisions td ON vss.divisionGUID = td.GUID
         JOIN t_teams tt ON vss.teamGUID = tt.GUID
ORDER BY fs.ID, vss.conferenceGUID, vss.divisionGUID, vss.gamesBack, runDifferential DESC