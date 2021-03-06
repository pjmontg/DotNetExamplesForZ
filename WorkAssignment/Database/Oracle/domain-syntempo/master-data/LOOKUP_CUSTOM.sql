--$Id: LOOKUP_CUSTOM.sql 11599 2014-06-16 20:10:04Z ogarcia $
SET SQLBLANKLINES ON;
SET DEFINE OFF;
DECLARE
    cte_now MAPPING_FIELD.CREATED_DATE%TYPE := SYSDATE;
    cte_author MAPPING_FIELD.CREATED_BY%TYPE := 'SEED';
BEGIN
  MERGE INTO lookup o
  USING ( SELECT
    'Area' NAME
    ,'Area' TITLE
    ,cte_author CREATED_BY --cte_author CREATED_BY
    ,cte_now CREATED_DATE-- cte_now CREATED_DATE
    ,null LAST_MODIFIED_BY --cte_author LAST_MODIFIED_BY
    ,null LAST_MODIFIED_DATE-- cte_now LAST_MODIFIED_DATE
    from dual
  ) n
  ON (o.NAME = n.NAME)
 WHEN NOT MATCHED THEN
     INSERT VALUES (n.NAME, n.TITLE, n.CREATED_BY, n.CREATED_DATE, n.LAST_MODIFIED_BY, n.LAST_MODIFIED_DATE)
 WHEN MATCHED THEN
     UPDATE SET o.LAST_MODIFIED_BY = n.CREATED_BY, o.LAST_MODIFIED_DATE = n.CREATED_DATE;
  COMMIT;
END;    
/