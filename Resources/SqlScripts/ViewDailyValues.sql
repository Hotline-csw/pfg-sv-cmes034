-- DailyValues View for Dashboards

CREATE VIEW[cust].[ViewDailyValues]
 AS WITH cteCalendar( DateOfDay ) AS /* Idea found in www.mssqltips.com/sqlservertip/4054*/ (  
 
SELECT CONVERT( DATETIME2, DateOfDay ) FROM ( SELECT DateOfDay = DATEADD( DAY, rn - 1, '2019-01-01' )  
FROM ( SELECT TOP ( DATEDIFF( DAY, '2021-01-01', '2025-01-01' ) ) rn = ROW_NUMBER() OVER ( ORDER BY s1.Sequence )  
FROM base.Modules AS s1, base.Modules AS s2, base.Modules AS s3, base.Modules AS s4 /* Cross join*/ ORDER BY s1.Sequence ) as x ) AS y ),  
 
cteSettings( ValueState1, ValueState2, ValueState3 ) as ( SELECT ISNULL( (  
SELECT ValueFloat FROM base.ProgramSettings WHERE Identifier = 'DailyOverviewSecondsForColor01' ), 0 ) AS ValueState1, ISNULL( (  
SELECT ValueFloat FROM base.ProgramSettings WHERE Identifier = 'DailyOverviewSecondsForColor02' ), 60 ) AS ValueState2, ISNULL( (  
SELECT ValueFloat FROM base.ProgramSettings WHERE Identifier = 'DailyOverviewSecondsForColor03' ), 600 ) AS ValueState3 ),  
 
 cte1010( DateOfDay, Quantity, DateOfLastFeedback, TimeOfNoFeedback, TimeOfNoFeedbackColor ) AS ( SELECT DateOfDay, Value01Int AS Quantity, ModificationDate AS DateOfLastFeedback, DATEDIFF( SECOND, ModificationDate, getdate() ) AS TimeOfNoFeedback, CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState3 FROM cteSettings ) > 0 THEN 3 ELSE CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState2 FROM cteSettings ) > 0 THEN 2 ELSE 1 END END AS TimeOfNoFeedbackColor FROM cust.DailyValues WHERE Position = '1010' ),
 cte3010( DateOfDay, Quantity, DateOfLastFeedback, TimeOfNoFeedback, TimeOfNoFeedbackColor ) AS ( SELECT DateOfDay, Value01Int AS Quantity, ModificationDate AS DateOfLastFeedback, DATEDIFF( SECOND, ModificationDate, getdate() ) AS TimeOfNoFeedback, CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState3 FROM cteSettings ) > 0 THEN 3 ELSE CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState2 FROM cteSettings ) > 0 THEN 2 ELSE 1 END END AS TimeOfNoFeedbackColor FROM cust.DailyValues WHERE Position = '3010' ),
 cte5010( DateOfDay, Quantity, DateOfLastFeedback, TimeOfNoFeedback, TimeOfNoFeedbackColor ) AS ( SELECT DateOfDay, Value01Int AS Quantity, ModificationDate AS DateOfLastFeedback, DATEDIFF( SECOND, ModificationDate, getdate() ) AS TimeOfNoFeedback, CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState3 FROM cteSettings ) > 0 THEN 3 ELSE CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState2 FROM cteSettings ) > 0 THEN 2 ELSE 1 END END AS TimeOfNoFeedbackColor FROM cust.DailyValues WHERE Position = '5010' ),
 cte5020( DateOfDay, Quantity, DateOfLastFeedback, TimeOfNoFeedback, TimeOfNoFeedbackColor ) AS ( SELECT DateOfDay, Value01Int AS Quantity, ModificationDate AS DateOfLastFeedback, DATEDIFF( SECOND, ModificationDate, getdate() ) AS TimeOfNoFeedback, CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState3 FROM cteSettings ) > 0 THEN 3 ELSE CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState2 FROM cteSettings ) > 0 THEN 2 ELSE 1 END END AS TimeOfNoFeedbackColor FROM cust.DailyValues WHERE Position = '5020' ),
 cte5070( DateOfDay, Quantity, DateOfLastFeedback, TimeOfNoFeedback, TimeOfNoFeedbackColor ) AS ( SELECT DateOfDay, Value01Int AS Quantity, ModificationDate AS DateOfLastFeedback, DATEDIFF( SECOND, ModificationDate, getdate() ) AS TimeOfNoFeedback, CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState3 FROM cteSettings ) > 0 THEN 3 ELSE CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState2 FROM cteSettings ) > 0 THEN 2 ELSE 1 END END AS TimeOfNoFeedbackColor FROM cust.DailyValues WHERE Position = '5070' ),
 cte6010( DateOfDay, Quantity, DateOfLastFeedback, TimeOfNoFeedback, TimeOfNoFeedbackColor ) AS ( SELECT DateOfDay, Value01Int AS Quantity, ModificationDate AS DateOfLastFeedback, DATEDIFF( SECOND, ModificationDate, getdate() ) AS TimeOfNoFeedback, CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState3 FROM cteSettings ) > 0 THEN 3 ELSE CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState2 FROM cteSettings ) > 0 THEN 2 ELSE 1 END END AS TimeOfNoFeedbackColor FROM cust.DailyValues WHERE Position = '6010' ),

 
 cte1010RestPlanWert( DateOfDay, Quantity, DateOfLastFeedback, TimeOfNoFeedback, TimeOfNoFeedbackColor ) AS ( SELECT DateOfDay, Value01Int AS Quantity, ModificationDate AS DateOfLastFeedback, DATEDIFF( SECOND, ModificationDate, getdate() ) AS TimeOfNoFeedback, CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState3 FROM cteSettings ) > 0 THEN 3 ELSE CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState2 FROM cteSettings ) > 0 THEN 2 ELSE 1 END END AS TimeOfNoFeedbackColor FROM cust.DailyValues WHERE Position = '1010RestPlanWert' ),
 cte3010RestPlanWert( DateOfDay, Quantity, DateOfLastFeedback, TimeOfNoFeedback, TimeOfNoFeedbackColor ) AS ( SELECT DateOfDay, Value01Int AS Quantity, ModificationDate AS DateOfLastFeedback, DATEDIFF( SECOND, ModificationDate, getdate() ) AS TimeOfNoFeedback, CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState3 FROM cteSettings ) > 0 THEN 3 ELSE CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState2 FROM cteSettings ) > 0 THEN 2 ELSE 1 END END AS TimeOfNoFeedbackColor FROM cust.DailyValues WHERE Position = '3010RestPlanWert' ),
 cte5010RestPlanWert( DateOfDay, Quantity, DateOfLastFeedback, TimeOfNoFeedback, TimeOfNoFeedbackColor ) AS ( SELECT DateOfDay, Value01Int AS Quantity, ModificationDate AS DateOfLastFeedback, DATEDIFF( SECOND, ModificationDate, getdate() ) AS TimeOfNoFeedback, CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState3 FROM cteSettings ) > 0 THEN 3 ELSE CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState2 FROM cteSettings ) > 0 THEN 2 ELSE 1 END END AS TimeOfNoFeedbackColor FROM cust.DailyValues WHERE Position = '5010RestPlanWert' ),
 cte5020RestPlanWert( DateOfDay, Quantity, DateOfLastFeedback, TimeOfNoFeedback, TimeOfNoFeedbackColor ) AS ( SELECT DateOfDay, Value01Int AS Quantity, ModificationDate AS DateOfLastFeedback, DATEDIFF( SECOND, ModificationDate, getdate() ) AS TimeOfNoFeedback, CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState3 FROM cteSettings ) > 0 THEN 3 ELSE CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState2 FROM cteSettings ) > 0 THEN 2 ELSE 1 END END AS TimeOfNoFeedbackColor FROM cust.DailyValues WHERE Position = '5020RestPlanWert' ),
 cte5070RestPlanWert( DateOfDay, Quantity, DateOfLastFeedback, TimeOfNoFeedback, TimeOfNoFeedbackColor ) AS ( SELECT DateOfDay, Value01Int AS Quantity, ModificationDate AS DateOfLastFeedback, DATEDIFF( SECOND, ModificationDate, getdate() ) AS TimeOfNoFeedback, CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState3 FROM cteSettings ) > 0 THEN 3 ELSE CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState2 FROM cteSettings ) > 0 THEN 2 ELSE 1 END END AS TimeOfNoFeedbackColor FROM cust.DailyValues WHERE Position = '5070RestPlanWert' ),
 cte6010RestPlanWert( DateOfDay, Quantity, DateOfLastFeedback, TimeOfNoFeedback, TimeOfNoFeedbackColor ) AS ( SELECT DateOfDay, Value01Int AS Quantity, ModificationDate AS DateOfLastFeedback, DATEDIFF( SECOND, ModificationDate, getdate() ) AS TimeOfNoFeedback, CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState3 FROM cteSettings ) > 0 THEN 3 ELSE CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState2 FROM cteSettings ) > 0 THEN 2 ELSE 1 END END AS TimeOfNoFeedbackColor FROM cust.DailyValues WHERE Position = '6010RestPlanWert' ),

 cte1010Verzug( DateOfDay, Quantity, DateOfLastFeedback, TimeOfNoFeedback, TimeOfNoFeedbackColor ) AS ( SELECT DateOfDay, Value01Int AS Quantity, ModificationDate AS DateOfLastFeedback, DATEDIFF( SECOND, ModificationDate, getdate() ) AS TimeOfNoFeedback, CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState3 FROM cteSettings ) > 0 THEN 3 ELSE CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState2 FROM cteSettings ) > 0 THEN 2 ELSE 1 END END AS TimeOfNoFeedbackColor FROM cust.DailyValues WHERE Position = '1010Verzug' ),
 cte3010Verzug( DateOfDay, Quantity, DateOfLastFeedback, TimeOfNoFeedback, TimeOfNoFeedbackColor ) AS ( SELECT DateOfDay, Value01Int AS Quantity, ModificationDate AS DateOfLastFeedback, DATEDIFF( SECOND, ModificationDate, getdate() ) AS TimeOfNoFeedback, CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState3 FROM cteSettings ) > 0 THEN 3 ELSE CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState2 FROM cteSettings ) > 0 THEN 2 ELSE 1 END END AS TimeOfNoFeedbackColor FROM cust.DailyValues WHERE Position = '3010Verzug' ),
 cte5010Verzug( DateOfDay, Quantity, DateOfLastFeedback, TimeOfNoFeedback, TimeOfNoFeedbackColor ) AS ( SELECT DateOfDay, Value01Int AS Quantity, ModificationDate AS DateOfLastFeedback, DATEDIFF( SECOND, ModificationDate, getdate() ) AS TimeOfNoFeedback, CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState3 FROM cteSettings ) > 0 THEN 3 ELSE CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState2 FROM cteSettings ) > 0 THEN 2 ELSE 1 END END AS TimeOfNoFeedbackColor FROM cust.DailyValues WHERE Position = '5010Verzug' ),
 cte5020Verzug( DateOfDay, Quantity, DateOfLastFeedback, TimeOfNoFeedback, TimeOfNoFeedbackColor ) AS ( SELECT DateOfDay, Value01Int AS Quantity, ModificationDate AS DateOfLastFeedback, DATEDIFF( SECOND, ModificationDate, getdate() ) AS TimeOfNoFeedback, CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState3 FROM cteSettings ) > 0 THEN 3 ELSE CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState2 FROM cteSettings ) > 0 THEN 2 ELSE 1 END END AS TimeOfNoFeedbackColor FROM cust.DailyValues WHERE Position = '5020Verzug' ),
 cte5070Verzug( DateOfDay, Quantity, DateOfLastFeedback, TimeOfNoFeedback, TimeOfNoFeedbackColor ) AS ( SELECT DateOfDay, Value01Int AS Quantity, ModificationDate AS DateOfLastFeedback, DATEDIFF( SECOND, ModificationDate, getdate() ) AS TimeOfNoFeedback, CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState3 FROM cteSettings ) > 0 THEN 3 ELSE CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState2 FROM cteSettings ) > 0 THEN 2 ELSE 1 END END AS TimeOfNoFeedbackColor FROM cust.DailyValues WHERE Position = '5070Verzug' ),
 cte6010Verzug( DateOfDay, Quantity, DateOfLastFeedback, TimeOfNoFeedback, TimeOfNoFeedbackColor ) AS ( SELECT DateOfDay, Value01Int AS Quantity, ModificationDate AS DateOfLastFeedback, DATEDIFF( SECOND, ModificationDate, getdate() ) AS TimeOfNoFeedback, CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState3 FROM cteSettings ) > 0 THEN 3 ELSE CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState2 FROM cteSettings ) > 0 THEN 2 ELSE 1 END END AS TimeOfNoFeedbackColor FROM cust.DailyValues WHERE Position = '6010Verzug' ),

 cte1010Vorleistung( DateOfDay, Quantity, DateOfLastFeedback, TimeOfNoFeedback, TimeOfNoFeedbackColor ) AS ( SELECT DateOfDay, Value01Int AS Quantity, ModificationDate AS DateOfLastFeedback, DATEDIFF( SECOND, ModificationDate, getdate() ) AS TimeOfNoFeedback, CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState3 FROM cteSettings ) > 0 THEN 3 ELSE CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState2 FROM cteSettings ) > 0 THEN 2 ELSE 1 END END AS TimeOfNoFeedbackColor FROM cust.DailyValues WHERE Position = '1010Vorleistung' ),
 cte3010Vorleistung( DateOfDay, Quantity, DateOfLastFeedback, TimeOfNoFeedback, TimeOfNoFeedbackColor ) AS ( SELECT DateOfDay, Value01Int AS Quantity, ModificationDate AS DateOfLastFeedback, DATEDIFF( SECOND, ModificationDate, getdate() ) AS TimeOfNoFeedback, CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState3 FROM cteSettings ) > 0 THEN 3 ELSE CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState2 FROM cteSettings ) > 0 THEN 2 ELSE 1 END END AS TimeOfNoFeedbackColor FROM cust.DailyValues WHERE Position = '3010Vorleistung' ),
 cte5010Vorleistung( DateOfDay, Quantity, DateOfLastFeedback, TimeOfNoFeedback, TimeOfNoFeedbackColor ) AS ( SELECT DateOfDay, Value01Int AS Quantity, ModificationDate AS DateOfLastFeedback, DATEDIFF( SECOND, ModificationDate, getdate() ) AS TimeOfNoFeedback, CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState3 FROM cteSettings ) > 0 THEN 3 ELSE CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState2 FROM cteSettings ) > 0 THEN 2 ELSE 1 END END AS TimeOfNoFeedbackColor FROM cust.DailyValues WHERE Position = '5010Vorleistung' ),
 cte5020Vorleistung( DateOfDay, Quantity, DateOfLastFeedback, TimeOfNoFeedback, TimeOfNoFeedbackColor ) AS ( SELECT DateOfDay, Value01Int AS Quantity, ModificationDate AS DateOfLastFeedback, DATEDIFF( SECOND, ModificationDate, getdate() ) AS TimeOfNoFeedback, CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState3 FROM cteSettings ) > 0 THEN 3 ELSE CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState2 FROM cteSettings ) > 0 THEN 2 ELSE 1 END END AS TimeOfNoFeedbackColor FROM cust.DailyValues WHERE Position = '5020Vorleistung' ),
 cte5070Vorleistung( DateOfDay, Quantity, DateOfLastFeedback, TimeOfNoFeedback, TimeOfNoFeedbackColor ) AS ( SELECT DateOfDay, Value01Int AS Quantity, ModificationDate AS DateOfLastFeedback, DATEDIFF( SECOND, ModificationDate, getdate() ) AS TimeOfNoFeedback, CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState3 FROM cteSettings ) > 0 THEN 3 ELSE CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState2 FROM cteSettings ) > 0 THEN 2 ELSE 1 END END AS TimeOfNoFeedbackColor FROM cust.DailyValues WHERE Position = '5070Vorleistung' ),
 cte6010Vorleistung( DateOfDay, Quantity, DateOfLastFeedback, TimeOfNoFeedback, TimeOfNoFeedbackColor ) AS ( SELECT DateOfDay, Value01Int AS Quantity, ModificationDate AS DateOfLastFeedback, DATEDIFF( SECOND, ModificationDate, getdate() ) AS TimeOfNoFeedback, CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState3 FROM cteSettings ) > 0 THEN 3 ELSE CASE WHEN DATEDIFF( SECOND, ModificationDate, getdate() ) - ( SELECT ValueState2 FROM cteSettings ) > 0 THEN 2 ELSE 1 END END AS TimeOfNoFeedbackColor FROM cust.DailyValues WHERE Position = '6010Vorleistung' )

   
 SELECT c.DateOfDay, /* Globale Werte*/  
 ISNULL( ( SELECT TOP 1 Value01Int FROM cust.CurrentValues WHERE Position = 'ItemsNotReleased' ), 0 ) AS NotReleased,  
 ISNULL( ( SELECT TOP 1 Value01Int FROM cust.CurrentValues WHERE Position = 'ItemsReleasedAndNotOptimized' ), 0 ) AS ReleasedAndNotOptimized,  
 ISNULL( ( SELECT TOP 1 Value01Int FROM cust.CurrentValues WHERE Position = 'ItemsOptimizedAndNotProduced' ), 0 ) AS OptimizedAndNotProduced, 
   
  /*A1010*/  
 isnull( A1010.Quantity, 0) AS A1010,
 isnull( A1010RestPlanWert.Quantity, 0) AS A1010RestPlanwert,
 isnull( A1010Verzug.Quantity, 0) AS A1010Verzug,
 isnull( A1010Vorleistung.Quantity, 0) AS A1010Vorleistung,
 ISNULL( A1010.DateOfLastFeedback, getdate() - 100 ) A1010DateOfLastFeedback,  
 ISNULL( A1010.TimeOfNoFeedback, 999999 ) AS A1010TimeOfNoFeedback,  
 ISNULL( A1010.TimeOfNoFeedbackColor, 0 ) AS A1010TimeOfNoFeedbackColor,  
   
 /*A3010*/  
 isnull( A3010.Quantity, 0) AS A3010,
 isnull( A3010RestPlanWert.Quantity, 0) AS A3010RestPlanwert,
 isnull( A3010Verzug.Quantity, 0) AS A3010Verzug,
 isnull( A3010Vorleistung.Quantity, 0) AS A3010Vorleistung,
 ISNULL( A3010.DateOfLastFeedback, getdate() - 100 ) A3010DateOfLastFeedback,  
 ISNULL( A3010.TimeOfNoFeedback, 999999 ) AS A3010TimeOfNoFeedback,  
 ISNULL( A3010.TimeOfNoFeedbackColor, 0 ) AS A3010TimeOfNoFeedbackColor, 
 
 /*A5010*/  
 isnull( A5010.Quantity, 0) AS A5010,
 isnull( A5010RestPlanWert.Quantity, 0) AS A5010RestPlanwert,
 isnull( A5010Verzug.Quantity, 0) AS A5010Verzug,
 isnull( A5010Vorleistung.Quantity, 0) AS A5010Vorleistung,
 ISNULL( A5010.DateOfLastFeedback, getdate() - 100 ) A5010DateOfLastFeedback,  
 ISNULL( A5010.TimeOfNoFeedback, 999999 ) AS A5010TimeOfNoFeedback,  
 ISNULL( A5010.TimeOfNoFeedbackColor, 0 ) AS A5010TimeOfNoFeedbackColor, 

 /*A5020*/
 isnull( A5020.Quantity, 0) AS A5020,
 isnull( A5020RestPlanWert.Quantity, 0) AS A5020RestPlanwert,
 isnull( A5020Verzug.Quantity, 0) AS A5020Verzug,
 isnull( A5020Vorleistung.Quantity, 0) AS A5020Vorleistung,
 ISNULL( A5020.DateOfLastFeedback, getdate() - 200 ) A5020DateOfLastFeedback,  
 ISNULL( A5020.TimeOfNoFeedback, 999999 ) AS A5020TimeOfNoFeedback,  
 ISNULL( A5020.TimeOfNoFeedbackColor, 0 ) AS A5020TimeOfNoFeedbackColor, 
 
  /*A5070*/  
 isnull( A5070.Quantity, 0) AS A5070,
 isnull( A5070RestPlanWert.Quantity, 0) AS A5070RestPlanwert,
 isnull( A5070Verzug.Quantity, 0) AS A5070Verzug,
 isnull( A5070Vorleistung.Quantity, 0) AS A5070Vorleistung,
 ISNULL( A5070.DateOfLastFeedback, getdate() - 100 ) A5070DateOfLastFeedback,  
 ISNULL( A5070.TimeOfNoFeedback, 999999 ) AS A5070TimeOfNoFeedback,  
 ISNULL( A5070.TimeOfNoFeedbackColor, 0 ) AS A5070TimeOfNoFeedbackColor, 

 /*A6010*/  
 isnull( A6010.Quantity, 0) AS A6010,
 isnull( A6010RestPlanWert.Quantity, 0) AS A6010RestPlanwert,
 isnull( A6010Verzug.Quantity, 0) AS A6010Verzug,
 isnull( A6010Vorleistung.Quantity, 0) AS A6010Vorleistung,
 ISNULL( A6010.DateOfLastFeedback, getdate() - 200 ) A6010DateOfLastFeedback,  
 ISNULL( A6010.TimeOfNoFeedback, 999999 ) AS A6010TimeOfNoFeedback,  
 ISNULL( A6010.TimeOfNoFeedbackColor, 0 ) AS A6010TimeOfNoFeedbackColor 

 
   
 FROM cteCalendar c  
 

 Left outer join cte1010 as A1010 on c.DateOfDay = A1010.DateOfDay	/* */
 Left outer join cte1010RestPlanWert as A1010RestPlanWert on c.DateOfDay = A1010RestPlanWert.DateOfDay	/* */
 Left outer join cte1010Verzug as A1010Verzug on c.DateOfDay = A1010Verzug.DateOfDay	/* */
 Left outer join cte1010Vorleistung as A1010Vorleistung on c.DateOfDay = A1010Vorleistung.DateOfDay	/* */

 Left outer join cte3010 as A3010 on c.DateOfDay = A3010.DateOfDay	/* */
 Left outer join cte3010RestPlanWert as A3010RestPlanWert on c.DateOfDay = A3010RestPlanWert.DateOfDay	/* */
 Left outer join cte3010Verzug as A3010Verzug on c.DateOfDay = A3010Verzug.DateOfDay	/* */
 Left outer join cte3010Vorleistung as A3010Vorleistung on c.DateOfDay = A3010Vorleistung.DateOfDay	/* */

 Left outer join cte5010 as A5010 on c.DateOfDay = A5010.DateOfDay	/* */
 Left outer join cte5010RestPlanWert as A5010RestPlanWert on c.DateOfDay = A5010RestPlanWert.DateOfDay	/* */
 Left outer join cte5010Verzug as A5010Verzug on c.DateOfDay = A5010Verzug.DateOfDay	/* */
 Left outer join cte5010Vorleistung as A5010Vorleistung on c.DateOfDay = A5010Vorleistung.DateOfDay	/* */

 Left outer join cte5020 as A5020 on c.DateOfDay = A5020.DateOfDay	/* */
 Left outer join cte5020RestPlanWert as A5020RestPlanWert on c.DateOfDay = A5020RestPlanWert.DateOfDay	/* */
 Left outer join cte5020Verzug as A5020Verzug on c.DateOfDay = A5020Verzug.DateOfDay	/* */
 Left outer join cte5020Vorleistung as A5020Vorleistung on c.DateOfDay = A5020Vorleistung.DateOfDay	/* */

 Left outer join cte5070 as A5070 on c.DateOfDay = A5070.DateOfDay	/* */
 Left outer join cte5070RestPlanWert as A5070RestPlanWert on c.DateOfDay = A5070RestPlanWert.DateOfDay	/* */
 Left outer join cte5070Verzug as A5070Verzug on c.DateOfDay = A5070Verzug.DateOfDay	/* */
 Left outer join cte5070Vorleistung as A5070Vorleistung on c.DateOfDay = A5070Vorleistung.DateOfDay	/* */

 Left outer join cte6010 as A6010 on c.DateOfDay = A6010.DateOfDay	/* */
 Left outer join cte6010RestPlanWert as A6010RestPlanWert on c.DateOfDay = A6010RestPlanWert.DateOfDay	/* */
 Left outer join cte6010Verzug as A6010Verzug on c.DateOfDay = A6010Verzug.DateOfDay	/* */
 Left outer join cte6010Vorleistung as A6010Vorleistung on c.DateOfDay = A6010Vorleistung.DateOfDay	/* */
