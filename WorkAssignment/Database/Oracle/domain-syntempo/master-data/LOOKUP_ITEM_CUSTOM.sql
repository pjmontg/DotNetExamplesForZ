--$Id: LOOKUP_ITEM_CUSTOM.sql 11599 2014-06-16 20:10:04Z ogarcia $
SET SQLBLANKLINES ON;
SET DEFINE OFF;
DECLARE
    cte_now MAPPING_FIELD.CREATED_DATE%TYPE := SYSDATE;
    cte_author MAPPING_FIELD.CREATED_BY%TYPE := 'SEED';
BEGIN
	Insert into LOOKUP_ITEM
	   (LOOKUP_NAME, VALUE, KEYNAME, ORDINAL, CREATED_BY, CREATED_DATE)
	 Values
	   ('Area', 'Area 1', 'A1', 0, cte_author, cte_now);
	Insert into LOOKUP_ITEM
	   (LOOKUP_NAME, VALUE, KEYNAME, ORDINAL, CREATED_BY, CREATED_DATE)
	 Values
	   ('Area', 'Area 2', 'A2', 1, cte_author, cte_now);
	Insert into LOOKUP_ITEM
	   (LOOKUP_NAME, VALUE, KEYNAME, ORDINAL, CREATED_BY, CREATED_DATE)
	 Values
	   ('Area', 'Area 3', 'A3', 2, cte_author, cte_now);
	COMMIT;
END;    
/