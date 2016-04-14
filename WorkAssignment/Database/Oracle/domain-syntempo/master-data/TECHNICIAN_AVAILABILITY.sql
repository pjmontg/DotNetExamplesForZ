SET SQLBLANKLINES ON;
SET DEFINE OFF;
DECLARE
    v_start_date TECHNICIAN_AVAILABILITY.WORK_DATE%TYPE;
    v_end_date   TECHNICIAN_AVAILABILITY.WORK_DATE%TYPE;
BEGIN
        Select add_months(min(REMAIN_EARLY_START), -12) into v_start_date from activity;
        Select add_months(max(REMAIN_EARLY_START), +12) into v_end_date   from activity;
         
        dbms_output.put_line(v_start_date);        
        dbms_output.put_line(v_end_date);        
        
        Delete from TECHNICIAN_AVAILABILITY;
        dbms_output.put_line(TO_CHAR(SQL%ROWCOUNT)||' TECHNICIAN_AVAILABILITY rows deleted');
        
        Insert into TECHNICIAN_AVAILABILITY (TECHNICIAN_ID, WORK_DATE, AVAILABLE_HRS)
        select ID, workDate, round(dbms_random.value(1,8)) 
         from (
                Select (v_start_date + (LEVEL -1)) AS workDate
                 FROM DUAL connect by level <=( v_end_date-(v_start_date))
                ) days   
        cross join technician;
        dbms_output.put_line(TO_CHAR(SQL%ROWCOUNT)||' TECHNICIAN_AVAILABILITY rows inserted');
        COMMIT;         
END;
/