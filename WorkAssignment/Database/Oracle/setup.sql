--$Id: setup.sql 11684 2014-06-24 17:13:51Z ogarcia $
--
SPOOL ON
SPOOL setup.log;

-- Create Work Assignment domain model
@./domain-syntempo/schema/CompleteScript.sql;

--Seed master data for web console app
@./domain-syntempo/master-data/TECHNICIAN.sql
@./domain-syntempo/master-data/TECHNICIAN_AVAILABILITY.sql
@./domain-syntempo/master-data/LOOKUP_CUSTOM.sql
@./domain-syntempo/master-data/LOOKUP_ITEM_CUSTOM.sql -- Up to here good
@./domain-syntempo/master-data/DATASOURCE_CUSTOM.sql
@./domain-syntempo/master-data/POSTPROCESS.sql

SPOOL OFF
