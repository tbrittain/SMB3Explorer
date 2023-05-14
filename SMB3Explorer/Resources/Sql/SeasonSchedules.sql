WITH teams AS
         (SELECT ttli.GUID AS teamGUID, ttli.localID, tt.teamName
          FROM t_team_local_ids ttli
                   JOIN t_teams tt ON ttli.GUID = tt.GUID
                   JOIN t_division_teams t on tt.GUID = t.teamGUID
                   JOIN t_divisions d on t.divisionGUID = d.GUID
                   JOIN t_conferences c on d.conferenceGUID = c.GUID
                   JOIN t_leagues l on c.leagueGUID = l.GUID
                   JOIN t_franchise tf ON l.GUID = tf.leagueGUID
          WHERE l.GUID = CAST(@leagueId AS BLOB)),
     seasons AS (SELECT id AS seasonID, RANK() OVER (ORDER BY id) AS seasonNum
                 FROM t_seasons
                          JOIN t_leagues ON t_seasons.historicalLeagueGUID = t_leagues.GUID
                          JOIN t_franchise tf ON t_leagues.GUID = tf.leagueGUID
                 WHERE t_leagues.GUID = CAST(@leagueId AS BLOB))
SELECT tss.seasonID,
       s.seasonNum,
       RANK() OVER (PARTITION BY tss.seasonID ORDER BY rowid) as gameNumber,
       (ROW_NUMBER() OVER (PARTITION BY tss.seasonID ORDER BY rowid) - 1) /
       ((SELECT COUNT(*) FROM teams) / 2) + 1                 AS day,
       homeTeams.teamName                                     AS homeTeam,
       awayTeams.teamName                                     AS awayTeam
FROM t_season_schedule tss
         JOIN seasons s ON tss.seasonID = s.seasonID
         JOIN teams homeTeams ON tss.homeTeamID = homeTeams.localID
         JOIN teams awayTeams ON tss.awayTeamID = awayTeams.localID
ORDER BY tss.seasonID, gameNumber;
