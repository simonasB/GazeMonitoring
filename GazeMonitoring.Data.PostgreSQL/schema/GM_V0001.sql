/*
 * Gaze Monitoring Schema Deployment Script
 *
 * Author:  Simonas Baltulionis baltulionis.simonas@gmail.com
 */


\set ON_ERROR_STOP on


\connect gazemonitoring



/*
 * CREATE UTILITY FUNCTIONS
 */



 CREATE OR REPLACE FUNCTION pg_temp.exists(view_name text) RETURNS BOOLEAN AS $$
DECLARE
    view regclass;
BEGIN
    EXECUTE 'SELECT to_regclass(''' || view_name || ''')' INTO view;
    IF view IS NULL THEN
        RETURN FALSE;
    ELSE
        RETURN TRUE;
    END IF;
END;
$$ LANGUAGE 'plpgsql';


CREATE OR REPLACE FUNCTION public.get_gaze_monitoring_schema_version() RETURNS int AS $$
DECLARE
    version int;
    applied timestamptz;
BEGIN
    EXECUTE 'SELECT version, applied FROM public.gaze_monitoring_schema_version ORDER BY version DESC LIMIT 1' INTO version, applied;
    RAISE NOTICE 'Found schema version %, applied %.', version, applied;
    RETURN version;
END
$$ LANGUAGE 'plpgsql';



CREATE OR REPLACE FUNCTION public.set_gaze_monitoring_schema_version(set_version int) RETURNS VOID AS $$
DECLARE
    current_version int;
    schema_applied timestamptz;
BEGIN
    current_version := public.get_gaze_monitoring_schema_version();
    IF current_version IS NULL THEN
        RAISE NOTICE 'No schema version found; setting schema version...';
        INSERT INTO public.gaze_monitoring_schema_version VALUES (set_version, now());
    ELSEIF (current_version = set_version) THEN
        UPDATE public.gaze_monitoring_schema_version SET applied = now() WHERE public.gaze_monitoring_schema_version.version = set_version;
        RAISE NOTICE 'Schema version % reapplied; updating timestamp...', set_version;
    ELSEIF (current_version > set_version) THEN
        RAISE EXCEPTION 'Cannot set schema version % on current version %.', set_version, current_version;
    ELSEIF (current_version < set_version) THEN
        INSERT INTO public.gaze_monitoring_schema_version VALUES (set_version, now()) RETURNING applied INTO schema_applied;
        RAISE NOTICE 'Schema version % applied %.', set_version, schema_applied;
    END IF;
END;
$$ LANGUAGE 'plpgsql';



DO $$
BEGIN
    IF NOT pg_temp.exists('public.gaze_monitoring_schema_version') THEN

        RAISE NOTICE 'Schema version table does not exist; creating...';

        CREATE TABLE public.gaze_monitoring_schema_version (
            version INTEGER NOT NULL UNIQUE,
            applied TIMESTAMPTZ,
            PRIMARY KEY (version)
        );
    
        RAISE NOTICE 'Schema version table created successfully!';

    ELSEIF public.get_gaze_monitoring_schema_version() >= 1 THEN

        RAISE EXCEPTION SQLSTATE 'SKIP0' USING MESSAGE = 'Base schema previously deployed; exiting...';

    END IF;
END
$$;



CREATE SCHEMA IF NOT EXISTS gaze_monitoring;


CREATE TABLE IF NOT EXISTS gaze_monitoring.subject_info
(
	session_id uuid,
	name text,
	age integer,
	details text,
	id serial,
	session_start_timestamp timestamp,
	session_end_timestamp timestamp,
	PRIMARY KEY(id)
);

CREATE INDEX IF NOT EXISTS subject_info_sesion_idx ON gaze_monitoring.subject_info (session_id);

CREATE TABLE IF NOT EXISTS gaze_monitoring.gaze_point
(
	x float8,
	y float8,
	timestamp float8,
	id bigserial,
	session_id uuid,
	subject_info_id integer,
	sample_time timestamp NOT NULL,
	PRIMARY KEY(id)
);

ALTER TABLE gaze_monitoring.gaze_point ADD FOREIGN KEY (subject_info_id) REFERENCES gaze_monitoring.subject_info (id);

CREATE INDEX IF NOT EXISTS gaze_point_sesion_idx ON gaze_monitoring.gaze_point (session_id);

CREATE TABLE IF NOT EXISTS gaze_monitoring.saccade
(
	direction float8,
	amplitude float8,
	velocity float8,
	start_timestamp float8,
	end_timestamp float8,
	id bigserial,
	session_id uuid,
	subject_info_id integer,
	sample_time timestamp NOT NULL,
	PRIMARY KEY(id)
);

ALTER TABLE gaze_monitoring.saccade ADD FOREIGN KEY (subject_info_id) REFERENCES gaze_monitoring.subject_info (id);

CREATE INDEX IF NOT EXISTS saccade_sesion_idx ON gaze_monitoring.gaze_point (session_id);

-- g_monitor service group role

DO $$
BEGIN
    IF NOT EXISTS (
        SELECT TRUE FROM pg_group WHERE groname = 'g_monitor'
    ) THEN

        RAISE NOTICE 'Creating ''g_monitor'' group role...';

        CREATE ROLE g_monitor;
        
        GRANT CONNECT ON DATABASE gazemonitoring TO g_monitor;
        GRANT USAGE ON SCHEMA gaze_monitoring TO g_monitor;
        GRANT SELECT, UPDATE ON ALL SEQUENCES IN SCHEMA gaze_monitoring TO g_monitor;
        GRANT SELECT, INSERT ON ALL TABLES IN SCHEMA gaze_monitoring TO g_monitor;

        RAISE NOTICE 'Group role completed successfully!';

    END IF;
END
$$;


	
CREATE OR REPLACE FUNCTION gaze_monitoring.create_gaze_monitoring_partitions()
    RETURNS integer
    LANGUAGE 'plpgsql'
AS $function$

DECLARE
	_timestamp_now timestamp;
	_timestamp_from timestamp;
	_timestamp_to timestamp;
	_partition_date_from text;
	_partition_date_to text;
	_tablename text;
	_count integer;
BEGIN	
    _timestamp_now = timezone('UTC', now());
    _count = 0;
    
    FOR i IN 0..6 LOOP
    	_timestamp_from = _timestamp_now + interval '1 day' * i;
		_timestamp_to = _timestamp_from + interval '1 day';
        _partition_date_from = to_char(_timestamp_from, 'YYYYMMDD');
		_partition_date_to = to_char(_timestamp_to, 'YYYYMMDD');
        _tablename = 'gaze_point_'||_partition_date_from;
        
		PERFORM 1
		FROM   pg_catalog.pg_class c
		JOIN   pg_catalog.pg_namespace n ON n.oid = c.relnamespace
		WHERE  c.relkind = 'r'
		AND    c.relname = _tablename
		AND    n.nspname = 'gaze_monitoring';
    
    	IF NOT FOUND THEN        
        	EXECUTE format('CREATE TABLE gaze_monitoring.%I (CHECK (sample_time >= %L AND sample_time < %L)) INHERITS (gaze_monitoring.gaze_point)', _tablename, _partition_date_from, _partition_date_to);     
        	EXECUTE format('ALTER TABLE gaze_monitoring.%I OWNER TO g_monitor', _tablename);
			EXECUTE format('ALTER TABLE gaze_monitoring.%I ADD CONSTRAINT %I_pkey PRIMARY KEY (id)', _tablename, _tablename);
			EXECUTE format('ALTER TABLE gaze_monitoring.%I ADD FOREIGN KEY (subject_info_id) REFERENCES gaze_monitoring.subject_info (id)', _tablename, _tablename, _tablename);
			EXECUTE format('CREATE INDEX %I_sesion_idx ON gaze_monitoring.%I (session_id)', _tablename, _tablename);
			
            _count = _count + 1;    
		END IF;
		
		_tablename = 'saccade_'||_partition_date_from;
		PERFORM 1
		FROM   pg_catalog.pg_class c
		JOIN   pg_catalog.pg_namespace n ON n.oid = c.relnamespace
		WHERE  c.relkind = 'r'
		AND    c.relname = _tablename
		AND    n.nspname = 'gaze_monitoring';
    
    	IF NOT FOUND THEN        
        	EXECUTE format('CREATE TABLE gaze_monitoring.%I (CHECK (sample_time >= %L AND sample_time < %L)) INHERITS (gaze_monitoring.saccade)', _tablename, _partition_date_from, _partition_date_to);     
        	EXECUTE format('ALTER TABLE gaze_monitoring.%I OWNER TO g_monitor', _tablename);
			EXECUTE format('ALTER TABLE gaze_monitoring.%I ADD CONSTRAINT %I_pkey PRIMARY KEY (id)', _tablename, _tablename);
			EXECUTE format('ALTER TABLE gaze_monitoring.%I ADD FOREIGN KEY (subject_info_id) REFERENCES gaze_monitoring.subject_info (id)', _tablename, _tablename);
			EXECUTE format('CREATE INDEX %I_sesion_idx ON gaze_monitoring.%I (session_id)', _tablename, _tablename);
			
            _count = _count + 1;    
		END IF;
    END LOOP;
	RETURN _count;
END;
$function$;

-- set schema version
SELECT public.set_gaze_monitoring_schema_version(1);
