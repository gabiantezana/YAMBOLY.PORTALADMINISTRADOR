-------------------------------------------------------UPDATE CRD1 - Dirección fiscal-------------------------------------------------------
--PARAM0: DbName
--PARAM1: ClientCardCode
--PARAM2: Latitud
--PARAM3: Longitud
--PARAM4: Address

--BEGINQUERY
UPDATE "PARAM0"."CRD1" SET "U_MSSM_LAT" ='PARAM2', "U_MSSM_LON" = 'PARAM3' WHERE "Address" = 'PARAM4' AND "CardCode" = 'PARAM1';