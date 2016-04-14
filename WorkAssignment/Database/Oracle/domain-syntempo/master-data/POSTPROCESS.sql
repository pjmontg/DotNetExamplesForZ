-- Massage data so it matches proof of concept.
--$Id: POSTPROCESS.sql 11684 2014-06-24 17:13:51Z ogarcia $
ALTER TRIGGER AIUD_ACTIVITY DISABLE;
/
SET DEFINE OFF;

	BEGIN
	Update activity set 
	area = 'A1', -- area
	custom_attr_string_8 = 'General Shops', -- shop
	CUSTOM_ATTR_STRING_21 = '60030', --wo_num
	CUSTOM_ATTR_STRING_22 = 'Bearing Maintenance',  --wo desc
	CUSTOM_ATTR_STRING_23 = 'E124-21', -- equip num
	CUSTOM_ATTR_STRING_24 = 'Hydro pump', -- equip desc
	CUSTOM_ATTR_STRING_25 = 'Code1',  --compliance code
	CUSTOM_ATTR_BOOL_5 = round(dbms_random.value(0,1)) ,  --overtime required
	CUSTOM_ATTR_INT_5 = round(dbms_random.value(1,4)) ,  --# of People
	start_date = trunc(sysdate) -- start date 
	  where act_num like '60030%'; -- '%553938%'
	
	COMMIT;
END;
/
	ALTER TRIGGER AIUD_ACTIVITY ENABLE;
/	
