--$Id: TECHNICIAN.sql 9332 2013-11-07 17:40:42Z ogarcia $
SET SQLBLANKLINES ON;
SET DEFINE OFF;
DECLARE
    cte_now LOOKUP_ITEM.CREATED_DATE%TYPE := SYSDATE;
    cte_author LOOKUP_ITEM.CREATED_BY%TYPE := 'SEED';    
BEGIN
Insert into TECHNICIAN (ID, SHOP_ID, NAME)
 Values (null, 'Machine', 'George Blue');
Insert into TECHNICIAN (ID, SHOP_ID, NAME)
 Values (null, 'Machine', 'Bill Yellow');
Insert into TECHNICIAN (ID, SHOP_ID, NAME)
 Values (null, 'Machine', 'David Red');
Insert into TECHNICIAN (ID, SHOP_ID, NAME)
 Values (null, 'General Shops', 'Joe Black');
Insert into TECHNICIAN (ID, SHOP_ID, NAME)
 Values (null, 'General Shops', 'Mary White');
Insert into TECHNICIAN (ID, SHOP_ID, NAME)
 Values (null, 'General Shops', 'Robert Green'); 
COMMIT;
END;
/