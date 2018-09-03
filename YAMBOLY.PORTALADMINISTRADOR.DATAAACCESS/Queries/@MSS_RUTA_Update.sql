-------------------------------------------------------DISABLE GEOSHAPE_D-------------------------------------------------------
--PARAM0: DbName
--PARAM1: Code
--PARAM2: Coordinates

--BEGINQUERY
UPDATE "PARAM0"."@MSS_RUTA" SET "U_COORDINATESARRAY" = 'PARAM2' WHERE "Code" = 'PARAM1';