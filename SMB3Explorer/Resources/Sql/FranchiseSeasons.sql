SELECT ts.id                        AS seasonID,
       ts.historicalLeagueGUID      AS leagueGUID,
       RANK() OVER (ORDER BY ts.id) AS seasonNum
FROM t_seasons ts
         JOIN t_leagues tl ON ts.historicalLeagueGUID = tl.GUID
WHERE tl.GUID = CAST(@leagueId AS BLOB)
ORDER BY ID DESC