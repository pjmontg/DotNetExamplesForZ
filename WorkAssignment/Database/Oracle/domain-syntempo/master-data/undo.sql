Drop table TECHNICIAN_AVAILABILITY;
Drop table TECHNICIAN_ASSIGNMENT;
Drop table TECHNICIAN;
Drop Sequence TA_SEQ;
Drop Sequence TECH_SEQ;
Delete from MAPPING_FIELD where Name = 'Area';
Delete from LOOKUP_ITEM where lookup_name = 'Area';
Delete from LOOKUP where name = 'Area';
Delete from DATASOURCE where name = 'Area';