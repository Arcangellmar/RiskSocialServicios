-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Versión del servidor:         10.5.18-MariaDB - mariadb.org binary distribution
-- SO del servidor:              Win64
-- HeidiSQL Versión:             11.3.0.6295
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- Volcando estructura de base de datos para risk_social
DROP DATABASE IF EXISTS `risk_social`;
CREATE DATABASE IF NOT EXISTS `risk_social` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci */;
USE `risk_social`;

-- Volcando estructura para procedimiento risk_social.sp_acciones_listar
DROP PROCEDURE IF EXISTS `sp_acciones_listar`;
DELIMITER //
CREATE PROCEDURE `sp_acciones_listar`(
	IN `PARAM_IN_ID_PROYECTO` INT
)
BEGIN

	SELECT
		A.VC_NOMBRE_ACCION,
		A.VC_DESCRIPCION,
		A.VC_ESTADO,
		U.VC_NOMBRE,
		UA.VC_NOMBRE AS USUARIO_ASIGNADO
	FROM ta_acciones A
	LEFT JOIN ta_usuarios U
		ON A.IN_ID_USUARIO_CREADOR = U.IN_ID_USUARIO
	LEFT JOIN ta_usuarios UA
		ON A.IN_ID_USUARIO_RESPONSABLE = UA.IN_ID_USUARIO
	WHERE A.IN_ID_PROYECTO = PARAM_IN_ID_PROYECTO;

END//
DELIMITER ;

-- Volcando estructura para procedimiento risk_social.sp_accion_actualizar_responsable
DROP PROCEDURE IF EXISTS `sp_accion_actualizar_responsable`;
DELIMITER //
CREATE PROCEDURE `sp_accion_actualizar_responsable`(
	IN `PARAM_IN_ID_USUARIO_RESPONSABLE` INT,
	IN `PARAM_IN_ID_ACCION_MITIGAR` INT
)
BEGIN

	UPDATE ta_acciones
	SET IN_ID_USUARIO_RESPONSABLE = PARAM_IN_ID_USUARIO_RESPONSABLE
	WHERE IN_ID_ACCION_MITIGAR = PARAM_IN_ID_ACCION_MITIGAR;

	SELECT 1 AS RES;
	
END//
DELIMITER ;

-- Volcando estructura para procedimiento risk_social.sp_accion_crear
DROP PROCEDURE IF EXISTS `sp_accion_crear`;
DELIMITER //
CREATE PROCEDURE `sp_accion_crear`(
	IN `PARAM_VC_NOMBRE_ACCION` VARCHAR(256),
	IN `PARAM_VC_DESCRIPCION` VARCHAR(256),
	IN `PARAM_IN_ID_USUARIO_CREADOR` INT,
	IN `PARAM_IN_ID_PROYECTO` INT,
	IN `PARAM_VC_ESTADO` VARCHAR(128),
	IN `PARAM_IN_ID_USUARIO_ASIGNADO` INT
)
BEGIN

	INSERT INTO ta_acciones (
		VC_NOMBRE_ACCION,
		VC_DESCRIPCION,
		IN_ID_USUARIO_CREADOR,
		IN_ID_PROYECTO,
		CH_SITUACION_REGISTRO,
		VC_ESTADO,
		IN_ID_USUARIO_RESPONSABLE
	)
	VALUES (
		PARAM_VC_NOMBRE_ACCION,
		PARAM_VC_DESCRIPCION,
		PARAM_IN_ID_USUARIO_CREADOR,
		PARAM_IN_ID_PROYECTO,
		"A",
		"En proceso",
		PARAM_IN_ID_USUARIO_ASIGNADO
	);
	
	SELECT MAX(IN_ID_ACCION_MITIGAR) AS RES FROM ta_acciones;

END//
DELIMITER ;

-- Volcando estructura para procedimiento risk_social.sp_comentarios_listar
DROP PROCEDURE IF EXISTS `sp_comentarios_listar`;
DELIMITER //
CREATE PROCEDURE `sp_comentarios_listar`(
	IN `PARAM_IN_ID_REPORTE` INT,
	IN `PARAM_IN_ID_RIESGO` INT,
	IN `PARAM_IN_ID_ACCION` INT,
	IN `PARAM_IN_ID_PROBLEMA` INT
)
BEGIN

	SELECT
		C.VC_COMENTARIO,
		U.VC_NOMBRE
	FROM ta_comentarios C
	JOIN ta_usuarios U
		ON C.IN_ID_USUARIO = U.IN_ID_USUARIO
	WHERE C.CH_SITUACION_REGISTRO = "A"
		AND (PARAM_IN_ID_REPORTE IS NULL OR C.IN_ID_REPORTE = PARAM_IN_ID_REPORTE)
		AND (PARAM_IN_ID_RIESGO IS NULL OR C.IN_ID_RIESGO = PARAM_IN_ID_RIESGO)
		AND (PARAM_IN_ID_ACCION IS NULL OR C.IN_ID_ACCION = PARAM_IN_ID_ACCION)
		AND (PARAM_IN_ID_PROBLEMA IS NULL OR C.IN_ID_PROBLEMA = PARAM_IN_ID_PROBLEMA);

END//
DELIMITER ;

-- Volcando estructura para procedimiento risk_social.sp_comentario_crear
DROP PROCEDURE IF EXISTS `sp_comentario_crear`;
DELIMITER //
CREATE PROCEDURE `sp_comentario_crear`(
	IN `PARAM_IN_ID_USUARIO` INT,
	IN `PARAM_VC_COMENTARIO` VARCHAR(1024),
	IN `PARAM_IN_ID_REPORTE` INT,
	IN `PARAM_IN_ID_RIESGO` INT,
	IN `PARAM_IN_ID_ACCION` INT,
	IN `PARAM_IN_ID_PROBLEMA` INT
)
BEGIN

	INSERT INTO ta_comentarios (
		IN_ID_USUARIO,
		VC_COMENTARIO,
		IN_ID_REPORTE,
		IN_ID_RIESGO,
		IN_ID_ACCION,
		IN_ID_PROBLEMA,
		CH_SITUACION_REGISTRO
	)
	VALUES (
		PARAM_IN_ID_USUARIO,
		PARAM_VC_COMENTARIO,
		PARAM_IN_ID_REPORTE,
		PARAM_IN_ID_RIESGO,
		PARAM_IN_ID_ACCION,
		PARAM_IN_ID_PROBLEMA,
		"A"
	);
	
	SELECT MAX(IN_ID_COMENTARIO) AS RES FROM ta_comentarios;

END//
DELIMITER ;

-- Volcando estructura para procedimiento risk_social.sp_comentario_eliminar
DROP PROCEDURE IF EXISTS `sp_comentario_eliminar`;
DELIMITER //
CREATE PROCEDURE `sp_comentario_eliminar`(
	IN `PARAM_ID_COMENTARIO` INT
)
BEGIN

	IF EXISTS (SELECT 1 FROM ta_comentarios WHERE IN_ID_COMENTARIO = PARAM_ID_COMENTARIO) THEN
	
		DELETE FROM ta_comentarios
		WHERE IN_ID_COMENTARIO = PARAM_ID_COMENTARIO;
		
		SELECT 1 AS RES;
		
	ELSE
	
		SELECT 0 AS RES;
	
	END IF;
	
END//
DELIMITER ;

-- Volcando estructura para procedimiento risk_social.sp_matriz
DROP PROCEDURE IF EXISTS `sp_matriz`;
DELIMITER //
CREATE PROCEDURE `sp_matriz`(
	IN `PARAM_IN_ID_PROYECTO` INT
)
BEGIN

	SELECT
		SUM(CASE WHEN R.DO_PROBABILIDAD = 5 AND R.DO_IMPACTO = 1 THEN 1 ELSE 0 END) AS MUY_POSIBLE_MUY_BAJO,
		SUM(CASE WHEN R.DO_PROBABILIDAD = 4 AND R.DO_IMPACTO = 1 THEN 1 ELSE 0 END) AS POSIBLE_MUY_BAJO,
		SUM(CASE WHEN R.DO_PROBABILIDAD = 3 AND R.DO_IMPACTO = 1 THEN 1 ELSE 0 END) AS OCASIONAL_MUY_BAJO,
		SUM(CASE WHEN R.DO_PROBABILIDAD = 2 AND R.DO_IMPACTO = 1 THEN 1 ELSE 0 END) AS PROBABLE_MUY_BAJO,
		SUM(CASE WHEN R.DO_PROBABILIDAD = 1 AND R.DO_IMPACTO = 1 THEN 1 ELSE 0 END) AS IMPROBABLE_MUY_BAJO,
		
		SUM(CASE WHEN R.DO_PROBABILIDAD = 5 AND R.DO_IMPACTO = 2 THEN 1 ELSE 0 END) AS MUY_POSIBLE_BAJO,
		SUM(CASE WHEN R.DO_PROBABILIDAD = 4 AND R.DO_IMPACTO = 2 THEN 1 ELSE 0 END) AS POSIBLE_BAJO,
		SUM(CASE WHEN R.DO_PROBABILIDAD = 3 AND R.DO_IMPACTO = 2 THEN 1 ELSE 0 END) AS OCASIONAL_BAJO,
		SUM(CASE WHEN R.DO_PROBABILIDAD = 2 AND R.DO_IMPACTO = 2 THEN 1 ELSE 0 END) AS PROBABLE_BAJO,
		SUM(CASE WHEN R.DO_PROBABILIDAD = 1 AND R.DO_IMPACTO = 2 THEN 1 ELSE 0 END) AS IMPROBABLE_BAJO,
		
		SUM(CASE WHEN R.DO_PROBABILIDAD = 5 AND R.DO_IMPACTO = 3 THEN 1 ELSE 0 END) AS MUY_POSIBLE_MEDIO,
		SUM(CASE WHEN R.DO_PROBABILIDAD = 4 AND R.DO_IMPACTO = 3 THEN 1 ELSE 0 END) AS POSIBLE_MEDIO,
		SUM(CASE WHEN R.DO_PROBABILIDAD = 3 AND R.DO_IMPACTO = 3 THEN 1 ELSE 0 END) AS OCASIONAL_MEDIO,
		SUM(CASE WHEN R.DO_PROBABILIDAD = 2 AND R.DO_IMPACTO = 3 THEN 1 ELSE 0 END) AS PROBABLE_MEDIO,
		SUM(CASE WHEN R.DO_PROBABILIDAD = 1 AND R.DO_IMPACTO = 3 THEN 1 ELSE 0 END) AS IMPROBABLE_MEDIO,
		
		SUM(CASE WHEN R.DO_PROBABILIDAD = 5 AND R.DO_IMPACTO = 4 THEN 1 ELSE 0 END) AS MUY_POSIBLE_ALTO,
		SUM(CASE WHEN R.DO_PROBABILIDAD = 4 AND R.DO_IMPACTO = 4 THEN 1 ELSE 0 END) AS POSIBLE_ALTO,
		SUM(CASE WHEN R.DO_PROBABILIDAD = 3 AND R.DO_IMPACTO = 4 THEN 1 ELSE 0 END) AS OCASIONAL_ALTO,
		SUM(CASE WHEN R.DO_PROBABILIDAD = 2 AND R.DO_IMPACTO = 4 THEN 1 ELSE 0 END) AS PROBABLE_ALTO,
		SUM(CASE WHEN R.DO_PROBABILIDAD = 1 AND R.DO_IMPACTO = 4 THEN 1 ELSE 0 END) AS IMPROBABLE_ALTO,
		
		SUM(CASE WHEN R.DO_PROBABILIDAD = 5 AND R.DO_IMPACTO = 5 THEN 1 ELSE 0 END) AS MUY_POSIBLE_CRITICO,
		SUM(CASE WHEN R.DO_PROBABILIDAD = 4 AND R.DO_IMPACTO = 5 THEN 1 ELSE 0 END) AS POSIBLE_CRITICO,
		SUM(CASE WHEN R.DO_PROBABILIDAD = 3 AND R.DO_IMPACTO = 5 THEN 1 ELSE 0 END) AS OCASIONAL_CRITICO,
		SUM(CASE WHEN R.DO_PROBABILIDAD = 2 AND R.DO_IMPACTO = 5 THEN 1 ELSE 0 END) AS PROBABLE_CRITICO,
		SUM(CASE WHEN R.DO_PROBABILIDAD = 1 AND R.DO_IMPACTO = 5 THEN 1 ELSE 0 END) AS IMPROBABLE_CRITICO
	FROM ta_proyectos P
	JOIN ta_riesgo R
		ON P.ID_PROYECTO = R.IN_ID_PROYECTO
	WHERE P.ID_PROYECTO = PARAM_IN_ID_PROYECTO;

	SELECT
		R.IN_ID_RIESGO,
		R.VC_NOMBRE_RIESGO,
		R.DO_PROBABILIDAD,
		R.DO_IMPACTO,
		R.VC_PRIODIDAD,
		R.VC_CRITICIDAD,
		R.VC_ESTADO,
		A.VC_USUARIO
	FROM ta_riesgo R
	LEFT JOIN ta_usuarios A
		ON R.IN_ID_USUARIO_ASIGNADO = A.IN_ID_USUARIO
	WHERE R.IN_ID_PROYECTO = PARAM_IN_ID_PROYECTO;

END//
DELIMITER ;

-- Volcando estructura para procedimiento risk_social.sp_problemas_listar
DROP PROCEDURE IF EXISTS `sp_problemas_listar`;
DELIMITER //
CREATE PROCEDURE `sp_problemas_listar`(
	IN `PARAM_IN_ID_PROYECTO` INT
)
BEGIN

	SELECT
		P.VC_NOMBRE_PROBLEMA,
		P.VC_DESCRIPCION_PROBLEMA,
		P.VC_PRIORIDAD,
		P.VC_CRITICIDAD,
		P.VC_ESTADO,
		U.VC_NOMBRE,
		R.VC_NOMBRE_RIESGO
	FROM ta_problemas P
	JOIN ta_usuarios U
		ON P.IN_ID_USUARIO_ASIGNADO = U.IN_ID_USUARIO
	JOIN ta_riesgo R
		ON R.IN_ID_RIESGO = P.IN_ID_RIESGO
	WHERE
		R.IN_ID_PROYECTO = PARAM_IN_ID_PROYECTO;

END//
DELIMITER ;

-- Volcando estructura para procedimiento risk_social.sp_problema_crear
DROP PROCEDURE IF EXISTS `sp_problema_crear`;
DELIMITER //
CREATE PROCEDURE `sp_problema_crear`(
	IN `PARAM_VC_NOMBRE_PROBLEMA` VARCHAR(256),
	IN `PARAM_VC_DESCRIPCION_PROBLEMA` VARCHAR(256),
	IN `PARAM_VC_PRIODIDAD` VARCHAR(256),
	IN `PARAM_VC_CRITICIDAD` VARCHAR(256),
	IN `PARAM_IN_ID_USUARIO_ASIGNADO` INT,
	IN `PARAM_IN_ID_RIESGO` INT
)
BEGIN

	INSERT INTO ta_problemas (
		VC_NOMBRE_PROBLEMA,
		VC_DESCRIPCION_PROBLEMA,
		VC_PRIORIDAD,
		VC_CRITICIDAD,
		IN_ID_USUARIO_ASIGNADO,
		VC_ESTADO,
		IN_ID_RIESGO,
		CH_SITUACION_REGISTRO
	)
	VALUES (
		PARAM_VC_NOMBRE_PROBLEMA,
		PARAM_VC_DESCRIPCION_PROBLEMA,
		PARAM_VC_PRIODIDAD,
		PARAM_VC_CRITICIDAD,
		PARAM_IN_ID_USUARIO_ASIGNADO,
		"Inicio",
		PARAM_IN_ID_RIESGO,
		"A"
	);
	
	SELECT MAX(IN_ID_PROBLEMA) AS RES FROM ta_problemas;

END//
DELIMITER ;

-- Volcando estructura para procedimiento risk_social.sp_proyecto_crear
DROP PROCEDURE IF EXISTS `sp_proyecto_crear`;
DELIMITER //
CREATE PROCEDURE `sp_proyecto_crear`(
	IN `PARAM_VC_NOMBRE_PROYECTO` VARCHAR(256),
	IN `PARAM_VC_DESCRIPCION_PROYECTO` VARCHAR(1024),
	IN `PARAM_IN_USUARIO_RESPONSABLE` INT,
	IN `PARAM_DT_FECHA_INICIO` DATETIME,
	IN `PARAM_DT_FECHA_FIN` DATETIME
)
BEGIN

	INSERT INTO ta_proyectos (
		VC_NOMBRE_PROYECTO,
		VC_DESCRIPCION_PROYECTO,
		IN_USUARIO_RESPONSABLE,
		DT_FECHA_INICIO,
		DT_FECHA_FIN,
		VC_ESTADO,
		CH_SITUACION_REGISTRO
	)
	VALUES (
		PARAM_VC_NOMBRE_PROYECTO,
		PARAM_VC_DESCRIPCION_PROYECTO,
		PARAM_IN_USUARIO_RESPONSABLE,
		PARAM_DT_FECHA_INICIO,
		PARAM_DT_FECHA_FIN,
		"Inicio",
		"A"
	);
	
	SELECT MAX(ID_PROYECTO) AS RES FROM ta_proyectos;

END//
DELIMITER ;

-- Volcando estructura para procedimiento risk_social.sp_proyecto_seguimiento
DROP PROCEDURE IF EXISTS `sp_proyecto_seguimiento`;
DELIMITER //
CREATE PROCEDURE `sp_proyecto_seguimiento`(
	IN `PARAM_ID_PROYECTO` INT
)
BEGIN

	SELECT 
		R.VC_NOMBRE_ROL,
		A.VC_USUARIO
	FROM ta_proyectos_usuarios PA
	LEFT JOIN ta_usuarios A
		ON PA.IN_ID_USUARIO = A.IN_ID_USUARIO
	LEFT JOIN ta_rol R
		ON PA.IN_ID_ROL = R.IN_ID_ROL
	WHERE PA.IN_ID_PROYECTO = PARAM_ID_PROYECTO;

	SELECT
		R.VC_NOMBRE_RIESGO,
		R.DO_PROBABILIDAD,
		R.DO_IMPACTO,
		R.VC_PRIODIDAD,
		R.VC_CRITICIDAD,
		R.VC_ESTADO,
		A.VC_USUARIO
	FROM ta_riesgo R
	LEFT JOIN ta_usuarios A
		ON R.IN_ID_USUARIO_ASIGNADO = A.IN_ID_USUARIO
	WHERE R.IN_ID_PROYECTO = PARAM_ID_PROYECTO;

	SELECT
		R.VC_CONTENIDO_REPORTE,
		A.VC_USUARIO
	FROM ta_reporte R
	LEFT JOIN ta_usuarios A
		ON R.IN_USUARIO_CREADOR = A.IN_ID_USUARIO
	WHERE R.IN_ID_PROYECTO = PARAM_ID_PROYECTO;

	SELECT
		U.VC_USUARIO,
		A.VC_NOMBRE_ACCION,
		A.VC_DESCRIPCION
	FROM ta_acciones A
	LEFT JOIN ta_usuarios U
		ON A.IN_ID_USUARIO_RESPONSABLE = U.IN_ID_USUARIO
	WHERE A.IN_ID_PROYECTO = PARAM_ID_PROYECTO;
	
END//
DELIMITER ;

-- Volcando estructura para procedimiento risk_social.sp_proyecto_usuario_crear
DROP PROCEDURE IF EXISTS `sp_proyecto_usuario_crear`;
DELIMITER //
CREATE PROCEDURE `sp_proyecto_usuario_crear`(
	IN `PARAM_IN_ID_USUARIO` INT,
	IN `PARAM_IN_ID_PROYECTO` INT,
	IN `PARAM_IN_ID_ROL` INT
)
BEGIN

	INSERT INTO ta_proyectos_usuarios (
		IN_ID_USUARIO,
		IN_ID_PROYECTO,
		IN_ID_ROL,
		CH_SITUACION_REGISTRO
	)
	VALUES (
		PARAM_IN_ID_USUARIO,
		PARAM_IN_ID_PROYECTO,
		PARAM_IN_ID_ROL,
		"A"
	);

	SELECT 1 AS RES;

END//
DELIMITER ;

-- Volcando estructura para procedimiento risk_social.sp_reporte_crear
DROP PROCEDURE IF EXISTS `sp_reporte_crear`;
DELIMITER //
CREATE PROCEDURE `sp_reporte_crear`(
	IN `PARAM_VC_CONTENIDO_REPORTE` VARCHAR(256),
	IN `PARAM_IN_USUARIO_CREADOR` INT,
	IN `PARAM_IN_ID_PROYECTO` INT
)
BEGIN

	INSERT INTO ta_reporte (
		VC_CONTENIDO_REPORTE,
		IN_USUARIO_CREADOR,
		DT_FECHA_CREACION,
		IN_ID_PROYECTO,
		CH_SITUACION_REGISTRO
	)
	VALUES (
		PARAM_VC_CONTENIDO_REPORTE,
		PARAM_IN_USUARIO_CREADOR,
		NOW(),
		PARAM_IN_ID_PROYECTO,
		"A"
	);
	
	SELECT MAX(IN_ID_REPORTE) AS RES FROM ta_reporte;

END//
DELIMITER ;

-- Volcando estructura para procedimiento risk_social.sp_riesgos_listar
DROP PROCEDURE IF EXISTS `sp_riesgos_listar`;
DELIMITER //
CREATE PROCEDURE `sp_riesgos_listar`(
	IN `PARAM_IN_ID_PROYECTO` INT
)
BEGIN

	SELECT
		R.IN_ID_RIESGO,
		R.VC_NOMBRE_RIESGO,
		R.DO_PROBABILIDAD,
		R.DO_IMPACTO,
		R.VC_PRIODIDAD,
		R.VC_CRITICIDAD,
		R.VC_ESTADO,
		A.VC_USUARIO
	FROM ta_riesgo R
	LEFT JOIN ta_usuarios A
		ON R.IN_ID_USUARIO_ASIGNADO = A.IN_ID_USUARIO
	WHERE R.IN_ID_PROYECTO = PARAM_IN_ID_PROYECTO;

END//
DELIMITER ;

-- Volcando estructura para procedimiento risk_social.sp_riesgo_crear
DROP PROCEDURE IF EXISTS `sp_riesgo_crear`;
DELIMITER //
CREATE PROCEDURE `sp_riesgo_crear`(
	IN `PARAM_VC_NOMBRE_RIESGO` VARCHAR(256),
	IN `PARAM_VC_DESCRIPCION_RIESGO` VARCHAR(256),
	IN `PARAM_DO_PROBABILIDAD` DOUBLE,
	IN `PARAM_DO_IMPACTO` DOUBLE,
	IN `PARAM_VC_PRIODIDAD` VARCHAR(256),
	IN `PARAM_VC_CRITICIDAD` VARCHAR(256),
	IN `PARAM_IN_ID_USUARIO_ASIGNADO` INT,
	IN `PARAM_IN_ID_PROYECTO` INT
)
BEGIN

	INSERT INTO ta_riesgo (
		VC_NOMBRE_RIESGO,
		VC_DESCRIPCION_RIESGO,
		DO_PROBABILIDAD,
		DO_IMPACTO,
		VC_PRIODIDAD,
		VC_CRITICIDAD,
		IN_ID_USUARIO_ASIGNADO,
		VC_ESTADO,
		IN_ID_PROYECTO,
		CH_SITUACION_REGISTRO
	)
	VALUES (
		PARAM_VC_NOMBRE_RIESGO,
		PARAM_VC_DESCRIPCION_RIESGO,
		PARAM_DO_PROBABILIDAD,
		PARAM_DO_IMPACTO,
		PARAM_VC_PRIODIDAD,
		PARAM_VC_CRITICIDAD,
		PARAM_IN_ID_USUARIO_ASIGNADO,
		"Inicio",
		PARAM_IN_ID_PROYECTO,
		"A"
	);
	
	SELECT MAX(IN_ID_RIESGO) AS RES FROM ta_riesgo;

END//
DELIMITER ;

-- Volcando estructura para procedimiento risk_social.sp_rol_cambiar
DROP PROCEDURE IF EXISTS `sp_rol_cambiar`;
DELIMITER //
CREATE PROCEDURE `sp_rol_cambiar`(
	IN `PARAM_IN_ID_USUARIO` INT,
	IN `PARAM_IN_ID_PROYECTO` INT,
	IN `PARAM_IN_ID_ROL` INT
)
BEGIN

	IF EXISTS (SELECT 1 FROM ta_proyectos_usuarios WHERE IN_ID_USUARIO = PARAM_IN_ID_USUARIO
		 	AND IN_ID_PROYECTO = PARAM_IN_ID_PROYECTO) THEN
	
		 UPDATE ta_proyectos_usuarios
		 SET IN_ID_ROL = PARAM_IN_ID_ROL
		 WHERE
		 	IN_ID_USUARIO = PARAM_IN_ID_USUARIO
		 	AND IN_ID_PROYECTO = PARAM_IN_ID_PROYECTO;
		 
	ELSE
	
		 INSERT INTO ta_proyectos_usuarios (
			IN_ID_USUARIO,
			IN_ID_PROYECTO,
			IN_ID_ROL,
			CH_SITUACION_REGISTRO
		)
		VALUES (
			PARAM_IN_ID_USUARIO,
			PARAM_IN_ID_PROYECTO,
			PARAM_IN_ID_ROL,
			"A"
		);
		 
	END IF;

	SELECT 1 AS RES;

END//
DELIMITER ;

-- Volcando estructura para procedimiento risk_social.sp_usuario_crear
DROP PROCEDURE IF EXISTS `sp_usuario_crear`;
DELIMITER //
CREATE PROCEDURE `sp_usuario_crear`(
	IN `PARAM_VC_USUARIO` VARCHAR(256),
	IN `PARAM_VC_PASS` VARCHAR(256),
	IN `PARAM_VC_NOMBRE` VARCHAR(256),
	IN `PARAM_VC_CORREO` VARCHAR(256)
)
BEGIN

	INSERT INTO ta_usuarios (
		VC_USUARIO,
		VC_PASS,
		VC_NOMBRE,
		VC_CORREO,
		CH_SITUACION_REGISTRO
	)
	VALUES (
		PARAM_VC_USUARIO,
		PARAM_VC_PASS,
		PARAM_VC_NOMBRE,
		PARAM_VC_CORREO,
		"A"
	);
	
	SELECT 1 AS RES;

END//
DELIMITER ;

-- Volcando estructura para procedimiento risk_social.sp_usuario_listar
DROP PROCEDURE IF EXISTS `sp_usuario_listar`;
DELIMITER //
CREATE PROCEDURE `sp_usuario_listar`(
	IN `PARAM_IN_ID_PROYECTO` INT
)
BEGIN

	SELECT DISTINCT
		U.IN_ID_USUARIO,
		U.VC_USUARIO
	FROM ta_usuarios U
	JOIN ta_proyectos_usuarios PU
		ON U.IN_ID_USUARIO = PU.IN_ID_USUARIO
	WHERE
		PARAM_IN_ID_PROYECTO IS NULL OR PU.IN_ID_PROYECTO = PARAM_IN_ID_PROYECTO;
	
END//
DELIMITER ;

-- Volcando estructura para procedimiento risk_social.sp_usuario_listar_proyectos
DROP PROCEDURE IF EXISTS `sp_usuario_listar_proyectos`;
DELIMITER //
CREATE PROCEDURE `sp_usuario_listar_proyectos`(
	IN `PARAM_IN_ID_USUARIO` INT
)
BEGIN

	SELECT
		P.ID_PROYECTO,
		P.VC_NOMBRE_PROYECTO,
		P.VC_DESCRIPCION_PROYECTO,
		P.DT_FECHA_INICIO,
		P.DT_FECHA_FIN,
		P.VC_ESTADO,
		R.VC_NOMBRE_ROL
	FROM ta_proyectos P
	JOIN ta_proyectos_usuarios PU
		ON P.ID_PROYECTO = PU.IN_ID_PROYECTO
	JOIN ta_rol R
		ON R.IN_ID_ROL = PU.IN_ID_ROL
	WHERE
		P.CH_SITUACION_REGISTRO = "A"
		AND PU.CH_SITUACION_REGISTRO = "A"
		AND PU.IN_ID_USUARIO = PARAM_IN_ID_USUARIO;

END//
DELIMITER ;

-- Volcando estructura para procedimiento risk_social.sp_usuario_login
DROP PROCEDURE IF EXISTS `sp_usuario_login`;
DELIMITER //
CREATE PROCEDURE `sp_usuario_login`(
	IN `PARAM_VC_CORREO` VARCHAR(1024)
)
BEGIN

	SELECT
		IN_ID_USUARIO,
		VC_USUARIO,
		VC_PASS,
		VC_NOMBRE,
		VC_CORREO,
		CH_SITUACION_REGISTRO
	FROM ta_usuarios
	WHERE
		VC_CORREO = PARAM_VC_CORREO
		AND CH_SITUACION_REGISTRO = "A";

END//
DELIMITER ;

-- Volcando estructura para tabla risk_social.ta_acciones
DROP TABLE IF EXISTS `ta_acciones`;
CREATE TABLE IF NOT EXISTS `ta_acciones` (
  `IN_ID_ACCION_MITIGAR` int(11) NOT NULL AUTO_INCREMENT,
  `VC_NOMBRE_ACCION` varchar(1024) DEFAULT NULL,
  `VC_DESCRIPCION` varchar(1024) DEFAULT NULL,
  `IN_ID_USUARIO_CREADOR` int(11) DEFAULT NULL,
  `IN_ID_PROYECTO` int(11) DEFAULT NULL,
  `IN_ID_USUARIO_RESPONSABLE` int(11) DEFAULT NULL,
  `VC_ESTADO` varchar(128) DEFAULT NULL,
  `CH_SITUACION_REGISTRO` char(1) DEFAULT NULL,
  PRIMARY KEY (`IN_ID_ACCION_MITIGAR`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Volcando datos para la tabla risk_social.ta_acciones: ~13 rows (aproximadamente)
DELETE FROM `ta_acciones`;
/*!40000 ALTER TABLE `ta_acciones` DISABLE KEYS */;
INSERT INTO `ta_acciones` (`IN_ID_ACCION_MITIGAR`, `VC_NOMBRE_ACCION`, `VC_DESCRIPCION`, `IN_ID_USUARIO_CREADOR`, `IN_ID_PROYECTO`, `IN_ID_USUARIO_RESPONSABLE`, `VC_ESTADO`, `CH_SITUACION_REGISTRO`) VALUES
	(1, 'Búsqueda de múltiples fuentes de financiamiento y desarrollo', 'Búsqueda de múltiples fuentes de financiamiento y desarrollo', 17, 24, 17, 'En proceso', 'A'),
	(2, 'Desarrollo de un plan de contingencia', 'Desarrollo de un plan de contingencia para abordar posibles interrupciones en la entrega de suministros esenciales.', 17, 24, 17, 'En proceso', 'A'),
	(3, 'Establecimiento de alternativas tecnológicas', 'Establecimiento de alternativas tecnológicas', 17, 24, 17, 'En proceso', 'A'),
	(4, 'Creación de una red ampliada de voluntarios', 'Creación de una red ampliada de voluntarios', 17, 24, 17, 'En proceso', 'A'),
	(5, 'Implementación de medidas de seguridad mejoradas ', 'Implementación de medidas de seguridad mejoradas ', 17, 24, 17, 'En proceso', 'A'),
	(6, 'Mantenimiento de un inventario adicional', 'Mantenimiento de un inventario adicional', 17, 24, 17, 'En proceso', 'A'),
	(7, 'Exploración de proveedores alternativos y estrategias de entrega', 'Exploración de proveedores alternativos y estrategias de entrega', 17, 24, 17, 'En proceso', 'A'),
	(8, 'Colaboración con múltiples proveedores de servicios', 'Colaboración con múltiples proveedores de servicios', 17, 24, 17, 'En proceso', 'A'),
	(13, 'Accion 1', 'Descripcion 1', 17, 26, 15, 'En proceso', 'A'),
	(14, 'Accion 1', 'Descripcion', 17, 24, 15, 'En proceso', 'A'),
	(15, 'Limpieza de datos basura', 'Se procede con la limpieza de datos basura', 17, 24, 18, 'En proceso', 'A'),
	(16, 'Accion test', 'test', 17, 24, 15, 'En proceso', 'A'),
	(17, 'Accion test', 'test', 17, 24, 15, 'En proceso', 'A');
/*!40000 ALTER TABLE `ta_acciones` ENABLE KEYS */;

-- Volcando estructura para tabla risk_social.ta_comentarios
DROP TABLE IF EXISTS `ta_comentarios`;
CREATE TABLE IF NOT EXISTS `ta_comentarios` (
  `IN_ID_COMENTARIO` int(11) NOT NULL AUTO_INCREMENT,
  `IN_ID_USUARIO` int(11) DEFAULT NULL,
  `VC_COMENTARIO` varchar(1024) DEFAULT NULL,
  `IN_ID_REPORTE` int(11) DEFAULT NULL,
  `IN_ID_RIESGO` int(11) DEFAULT NULL,
  `IN_ID_ACCION` int(11) DEFAULT NULL,
  `IN_ID_PROBLEMA` int(11) DEFAULT NULL,
  `CH_SITUACION_REGISTRO` char(1) DEFAULT NULL,
  PRIMARY KEY (`IN_ID_COMENTARIO`)
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Volcando datos para la tabla risk_social.ta_comentarios: ~7 rows (aproximadamente)
DELETE FROM `ta_comentarios`;
/*!40000 ALTER TABLE `ta_comentarios` DISABLE KEYS */;
INSERT INTO `ta_comentarios` (`IN_ID_COMENTARIO`, `IN_ID_USUARIO`, `VC_COMENTARIO`, `IN_ID_REPORTE`, `IN_ID_RIESGO`, `IN_ID_ACCION`, `IN_ID_PROBLEMA`, `CH_SITUACION_REGISTRO`) VALUES
	(1, 17, 'El listado de riesgos está desactualizado', 1, 21, NULL, NULL, 'A'),
	(2, 17, 'Los detalles del proyecto fueron actualizados', 1, 21, NULL, NULL, 'A'),
	(3, 17, 'El primer riesgo contempla datos erroneos', 1, 21, NULL, NULL, 'A'),
	(4, 17, 'Los detalles del proyecto fueron actualizados', NULL, 21, NULL, NULL, 'A'),
	(5, 18, 'La probabilidad fue actualizada', NULL, 21, NULL, NULL, 'A'),
	(6, 17, 'Se actualizó el nombre del riesgo', NULL, 21, NULL, NULL, 'A'),
	(18, 17, 'comentario prueba ultimo', NULL, 21, NULL, NULL, 'A'),
	(22, 17, 'Comentario Test', NULL, 21, NULL, NULL, 'A');
/*!40000 ALTER TABLE `ta_comentarios` ENABLE KEYS */;

-- Volcando estructura para tabla risk_social.ta_problemas
DROP TABLE IF EXISTS `ta_problemas`;
CREATE TABLE IF NOT EXISTS `ta_problemas` (
  `IN_ID_PROBLEMA` int(11) NOT NULL AUTO_INCREMENT,
  `VC_NOMBRE_PROBLEMA` varchar(1024) DEFAULT NULL,
  `VC_DESCRIPCION_PROBLEMA` varchar(1024) DEFAULT NULL,
  `VC_PRIORIDAD` varchar(50) DEFAULT NULL,
  `VC_CRITICIDAD` varchar(50) DEFAULT NULL,
  `IN_ID_USUARIO_ASIGNADO` int(11) DEFAULT NULL,
  `IN_ID_RIESGO` int(11) DEFAULT NULL,
  `VC_ESTADO` varchar(256) DEFAULT NULL,
  `CH_SITUACION_REGISTRO` char(1) DEFAULT NULL,
  PRIMARY KEY (`IN_ID_PROBLEMA`)
) ENGINE=InnoDB AUTO_INCREMENT=33 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Volcando datos para la tabla risk_social.ta_problemas: ~23 rows (aproximadamente)
DELETE FROM `ta_problemas`;
/*!40000 ALTER TABLE `ta_problemas` DISABLE KEYS */;
INSERT INTO `ta_problemas` (`IN_ID_PROBLEMA`, `VC_NOMBRE_PROBLEMA`, `VC_DESCRIPCION_PROBLEMA`, `VC_PRIORIDAD`, `VC_CRITICIDAD`, `IN_ID_USUARIO_ASIGNADO`, `IN_ID_RIESGO`, `VC_ESTADO`, `CH_SITUACION_REGISTRO`) VALUES
	(8, ' Interrupción en la entrega de alimentos debido a problemas logísticos con el proveedor.', ' Interrupción en la entrega de alimentos debido a problemas logísticos con el proveedor.', 'Alta', 'Alto', 16, 12, 'Inicio', 'A'),
	(9, 'Falla en la plataforma', 'Falla en la plataforma virtual durante una sesión educativa, afectando la participación de los estudiantes.', 'Alta', 'Alta', 1, 4, 'Inicio', 'A'),
	(10, 'Ausencia inesperada de voluntarios para un evento recreativo en el hogar de ancianos.', 'Ausencia inesperada de voluntarios para un evento recreativo en el hogar de ancianos.', 'Baja', 'Alta', 1, 4, 'Inicio', 'A'),
	(11, 'Robo de herramientas y equipos utilizados en proyectos de jardinería comunitaria.', 'Robo de herramientas y equipos utilizados en proyectos de jardinería comunitaria.', 'Media', 'Alto', 16, 12, 'Inicio', 'A'),
	(13, ' Pérdida de parte del material donado para talleres de costura, comprometiendo la continuidad de las actividades.', ' Pérdida de parte del material donado para talleres de costura, comprometiendo la continuidad de las actividades.', 'Media', 'Muy alto', 15, 11, 'Inicio', 'A'),
	(14, 'Cancelación de una charla motivacional programada debido a problemas de transporte para el ponente.', 'Cancelación de una charla motivacional programada debido a problemas de transporte para el ponente.', 'Media', 'Alto', 17, 5, 'Inicio', 'A'),
	(15, 'Acto vandálico daña áreas verdes recientemente revitalizadas por el proyecto.', 'Acto vandálico daña áreas verdes recientemente revitalizadas por el proyecto.', 'Baja', 'Media', 17, NULL, 'Inicio', 'A'),
	(16, 'Materiales tardíos', 'Retraso en la adquisición de materiales adaptados para participantes con discapacidades.', 'Baja', 'Media', 17, NULL, 'Inicio', 'A'),
	(17, 'Incidencia test', 'Incidencia test', 'Media', 'Insignificante', 17, 1, 'Inicio', 'A'),
	(18, 'Retrasos en el cronograma', 'La falta de fondos puede resultar en la imposibilidad de completar las tareas según lo planeado. Esto puede llevar a retrasos en la finalización del proyecto, lo que a su vez podría causar inconvenientes y frustración entre las partes interesadas.', 'Media', 'Medio', 19, 22, 'Inicio', 'A'),
	(19, 'Falta de Recursos Financieros', 'Limitaciones presupuestarias que afectan la implementación y continuidad del proyecto.', 'Media', 'Media', 17, 24, 'Inicio', 'A'),
	(20, 'Falta de Recursos Financieros', 'Limitaciones presupuestarias que afectan la implementación y continuidad del proyecto.', 'Media', 'Media', 17, 24, 'Inicio', 'A'),
	(21, 'Falta de Recursos Financieros', 'Limitaciones presupuestarias que afectan la implementación y continuidad del proyecto.', 'Media', 'Media', 17, 21, 'Inicio', 'A'),
	(22, 'Falta de Recursos Financieros', 'Limitaciones presupuestarias que afectan la implementación y continuidad del proyecto.', 'Media', 'Media', 20, 21, 'Inicio', 'A'),
	(23, 'Falta de Participación Comunitaria', 'Resistencia o falta de participación activa por parte de la comunidad local.', 'Media', 'Media', 17, 21, 'Inicio', 'A'),
	(24, 'Falta de Participación Comunitaria', 'Resistencia o falta de participación activa por parte de la comunidad local.', 'Media', 'Media', 17, 21, 'Inicio', 'A'),
	(25, 'Falta de Participación Comunitaria', 'Resistencia o falta de participación activa por parte de la comunidad local.', 'Media', 'Media', 17, 21, 'Inicio', 'A'),
	(26, 'Falta de Participación Comunitaria', 'Resistencia o falta de participación activa por parte de la comunidad local.', 'Media', 'Media', 20, 21, 'Inicio', 'A'),
	(27, 'Riesgos ambientales que podrían afectar negativamente la ejecución y sostenibilidad del proyecto.', 'Riesgos ambientales que podrían afectar negativamente la ejecución y sostenibilidad del proyecto.', 'Media', 'Media', 17, 21, 'Inicio', 'A'),
	(28, 'Riesgos ambientales que podrían afectar negativamente la ejecución y sostenibilidad del proyecto.', 'Riesgos ambientales que podrían afectar negativamente la ejecución y sostenibilidad del proyecto.', 'Media', 'Media', 17, 21, 'Inicio', 'A'),
	(29, 'Riesgos ambientales que podrían afectar negativamente la ejecución y sostenibilidad del proyecto.', 'Riesgos ambientales que podrían afectar negativamente la ejecución y sostenibilidad del proyecto.', 'Media', 'Media', 17, 21, 'Inicio', 'A'),
	(30, 'Riesgos ambientales que podrían afectar negativamente la ejecución y sostenibilidad del proyecto.', 'Riesgos ambientales que podrían afectar negativamente la ejecución y sostenibilidad del proyecto.', 'Media', 'Media', 20, 21, 'Inicio', 'A'),
	(31, 'Sostenibilidad a Largo Plazo', 'Desafíos para garantizar la continuidad y sostenibilidad del proyecto después de su finalización inicial.', 'Media', 'Medio', 18, 31, 'Inicio', 'A'),
	(32, 'Incidencia test', 'test', 'Baja', 'Insignificante', 17, 12, 'Inicio', 'A');
/*!40000 ALTER TABLE `ta_problemas` ENABLE KEYS */;

-- Volcando estructura para tabla risk_social.ta_proyectos
DROP TABLE IF EXISTS `ta_proyectos`;
CREATE TABLE IF NOT EXISTS `ta_proyectos` (
  `ID_PROYECTO` int(11) NOT NULL AUTO_INCREMENT,
  `VC_NOMBRE_PROYECTO` varchar(256) DEFAULT NULL,
  `VC_DESCRIPCION_PROYECTO` varchar(1025) DEFAULT NULL,
  `IN_USUARIO_RESPONSABLE` int(11) DEFAULT NULL,
  `DT_FECHA_INICIO` datetime DEFAULT NULL,
  `DT_FECHA_FIN` datetime DEFAULT NULL,
  `VC_ESTADO` varchar(256) DEFAULT NULL,
  `CH_SITUACION_REGISTRO` char(1) DEFAULT NULL,
  PRIMARY KEY (`ID_PROYECTO`)
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Volcando datos para la tabla risk_social.ta_proyectos: ~9 rows (aproximadamente)
DELETE FROM `ta_proyectos`;
/*!40000 ALTER TABLE `ta_proyectos` DISABLE KEYS */;
INSERT INTO `ta_proyectos` (`ID_PROYECTO`, `VC_NOMBRE_PROYECTO`, `VC_DESCRIPCION_PROYECTO`, `IN_USUARIO_RESPONSABLE`, `DT_FECHA_INICIO`, `DT_FECHA_FIN`, `VC_ESTADO`, `CH_SITUACION_REGISTRO`) VALUES
	(22, 'Vínculos de Esperanza', 'Uniendo esfuerzos para brindar ayuda humanitaria y apoyo comunitario en momentos de necesidad.', 16, '0001-12-14 00:00:00', '0141-12-14 00:00:00', 'Inicio', 'A'),
	(23, 'Raíces de Bienestar', 'Iluminando vecindarios con actividades recreativas, culturales y educativas para fortalecer la cohesión social.', 16, '2023-03-12 00:00:00', '2023-04-04 00:00:00', 'Inicio', 'A'),
	(24, 'Proyecto Charlie', 'Un esfuerzo colaborativo para mejorar la calidad de vida en comunidades locales mediante la implementación de iniciativas centradas en la educación, el bienestar social y el desarrollo sostenible.', 17, '2023-12-12 00:00:00', '2023-12-17 00:00:00', 'Inicio', 'A'),
	(25, 'Compromiso Ciudadano', 'Construyendo conexiones entre empleadores y aspirantes a empleo para impulsar la empleabilidad en comunidades desfavorecidas.', 17, '2023-12-12 00:00:00', '2023-12-20 00:00:00', 'Inicio', 'A'),
	(26, 'Amanecer Social', 'Fomentando la seguridad alimentaria a través de huertos comunitarios y programas de distribución de alimentos.', 17, '2023-12-12 00:00:00', '2023-12-30 00:00:00', 'Inicio', 'A'),
	(27, 'Pulso Comunitario', 'Fomentando habilidades artesanales y emprendimiento a través de programas de tejido y costura para empoderar a mujeres en situación vulnerable.', 18, '2023-12-01 00:00:00', '2014-12-01 00:00:00', 'Inicio', 'A'),
	(28, 'Recuperación del parque "El Pinochito"', 'Proyecto realizado con el voluntariado y la Municipalidad de Lima', 19, '2021-05-01 00:00:00', '2022-02-27 00:00:00', 'Inicio', 'A'),
	(29, 'Proyecto Ciudad Blanca', 'Proyecto enfocado a impulsar la participación de mujeres en el área STEM', 20, '2023-10-29 00:00:00', '2023-12-31 00:00:00', 'Inicio', 'A'),
	(30, 'Proyecto test ', 'Test', 17, '2023-12-02 00:00:00', '2023-12-12 00:00:00', 'Inicio', 'A');
/*!40000 ALTER TABLE `ta_proyectos` ENABLE KEYS */;

-- Volcando estructura para tabla risk_social.ta_proyectos_usuarios
DROP TABLE IF EXISTS `ta_proyectos_usuarios`;
CREATE TABLE IF NOT EXISTS `ta_proyectos_usuarios` (
  `IN_ID_PROYECTO_USUARIO` int(11) NOT NULL AUTO_INCREMENT,
  `IN_ID_USUARIO` int(11) DEFAULT NULL,
  `IN_ID_PROYECTO` int(11) DEFAULT NULL,
  `IN_ID_ROL` int(11) DEFAULT NULL,
  `CH_SITUACION_REGISTRO` char(1) DEFAULT NULL,
  PRIMARY KEY (`IN_ID_PROYECTO_USUARIO`)
) ENGINE=InnoDB AUTO_INCREMENT=34 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Volcando datos para la tabla risk_social.ta_proyectos_usuarios: ~14 rows (aproximadamente)
DELETE FROM `ta_proyectos_usuarios`;
/*!40000 ALTER TABLE `ta_proyectos_usuarios` DISABLE KEYS */;
INSERT INTO `ta_proyectos_usuarios` (`IN_ID_PROYECTO_USUARIO`, `IN_ID_USUARIO`, `IN_ID_PROYECTO`, `IN_ID_ROL`, `CH_SITUACION_REGISTRO`) VALUES
	(18, 16, 22, 1, 'A'),
	(19, 16, 23, 1, 'A'),
	(20, 17, 24, 2, 'A'),
	(21, 17, 25, 1, 'A'),
	(22, 17, 26, 1, 'A'),
	(23, 15, 24, 2, 'A'),
	(24, 16, 24, 1, 'A'),
	(25, 18, 27, 1, 'A'),
	(26, 1, 1, 1, 'A'),
	(27, 18, 25, 2, 'A'),
	(28, 15, 26, 3, 'A'),
	(29, 16, 25, 1, 'A'),
	(30, 18, 24, 2, 'A'),
	(31, 19, 28, 1, 'A'),
	(32, 20, 29, 1, 'A'),
	(33, 17, 30, 1, 'A');
/*!40000 ALTER TABLE `ta_proyectos_usuarios` ENABLE KEYS */;

-- Volcando estructura para tabla risk_social.ta_reporte
DROP TABLE IF EXISTS `ta_reporte`;
CREATE TABLE IF NOT EXISTS `ta_reporte` (
  `IN_ID_REPORTE` int(11) NOT NULL AUTO_INCREMENT,
  `VC_CONTENIDO_REPORTE` varchar(1024) DEFAULT NULL,
  `IN_USUARIO_CREADOR` int(11) DEFAULT NULL,
  `DT_FECHA_CREACION` datetime DEFAULT NULL,
  `IN_ID_PROYECTO` int(11) DEFAULT NULL,
  `CH_SITUACION_REGISTRO` char(1) DEFAULT NULL,
  PRIMARY KEY (`IN_ID_REPORTE`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Volcando datos para la tabla risk_social.ta_reporte: ~2 rows (aproximadamente)
DELETE FROM `ta_reporte`;
/*!40000 ALTER TABLE `ta_reporte` DISABLE KEYS */;
INSERT INTO `ta_reporte` (`IN_ID_REPORTE`, `VC_CONTENIDO_REPORTE`, `IN_USUARIO_CREADOR`, `DT_FECHA_CREACION`, `IN_ID_PROYECTO`, `CH_SITUACION_REGISTRO`) VALUES
	(1, 'Reporte 2022 VI', 17, '2023-11-01 17:13:42', 24, 'A'),
	(2, 'Reporte 2023 III', 17, '2023-12-02 13:00:00', 24, 'A');
/*!40000 ALTER TABLE `ta_reporte` ENABLE KEYS */;

-- Volcando estructura para tabla risk_social.ta_riesgo
DROP TABLE IF EXISTS `ta_riesgo`;
CREATE TABLE IF NOT EXISTS `ta_riesgo` (
  `IN_ID_RIESGO` int(11) NOT NULL AUTO_INCREMENT,
  `VC_NOMBRE_RIESGO` varchar(1024) DEFAULT NULL,
  `VC_DESCRIPCION_RIESGO` varchar(1024) DEFAULT NULL,
  `DO_PROBABILIDAD` double DEFAULT NULL,
  `DO_IMPACTO` double DEFAULT NULL,
  `VC_PRIODIDAD` varchar(50) DEFAULT NULL,
  `VC_CRITICIDAD` varchar(50) DEFAULT NULL,
  `IN_ID_USUARIO_ASIGNADO` int(11) DEFAULT NULL,
  `VC_ESTADO` varchar(256) DEFAULT NULL,
  `IN_ID_PROYECTO` int(11) DEFAULT NULL,
  `CH_SITUACION_REGISTRO` char(1) DEFAULT NULL,
  PRIMARY KEY (`IN_ID_RIESGO`)
) ENGINE=InnoDB AUTO_INCREMENT=40 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Volcando datos para la tabla risk_social.ta_riesgo: ~26 rows (aproximadamente)
DELETE FROM `ta_riesgo`;
/*!40000 ALTER TABLE `ta_riesgo` DISABLE KEYS */;
INSERT INTO `ta_riesgo` (`IN_ID_RIESGO`, `VC_NOMBRE_RIESGO`, `VC_DESCRIPCION_RIESGO`, `DO_PROBABILIDAD`, `DO_IMPACTO`, `VC_PRIODIDAD`, `VC_CRITICIDAD`, `IN_ID_USUARIO_ASIGNADO`, `VC_ESTADO`, `IN_ID_PROYECTO`, `CH_SITUACION_REGISTRO`) VALUES
	(1, 'Falta de Financiamiento', 'El riesgo de no contar con los recursos financieros necesarios para ejecutar y mantener el proyecto.', 0, 1, 'Baja', 'Alta', 17, 'Inicio', 24, 'A'),
	(12, 'Baja Sostenibilidad', 'La amenaza de que el proyecto no sea sostenible a largo plazo, ya sea debido a la falta de apoyo continuo o a la dependencia de recursos temporales.', 1, 1, 'Baja', 'Bajo', 17, 'Inicio', 24, 'A'),
	(13, 'Conflictos Internos', 'Riesgo de enfrentamientos o discordias dentro del equipo de proyecto, lo que podría afectar la cohesión y la eficacia.', 3, 3, 'Media', 'Medio', 17, 'Inicio', 24, 'A'),
	(14, 'Resistencia Cultural', 'La posibilidad de que las prácticas culturales existentes obstaculicen la implementación eficaz del proyecto.', 3, 1, 'Baja', 'Medio', 15, 'Inicio', 24, 'A'),
	(18, 'Impacto Ambiental No Previsto', 'Posibilidad de que las actividades del proyecto tengan consecuencias no deseadas para el medio ambiente local.', 1, 1, 'Baja', 'Bajo', 17, 'Inicio', 24, 'A'),
	(19, 'Financiamiento económico', 'Fondos insuficientes', 4, 5, 'Alta', 'Alta', 17, 'Inicio', 24, 'A'),
	(20, 'Escasez de recursos', 'Falta de dinero', 4, 5, 'Alta', 'Alta', 17, 'Inicio', 24, 'A'),
	(21, 'Riesgo de robo', 'Puede que nos roben', 3, 4, 'Media', 'Bajo', 18, 'Inicio', 27, 'A'),
	(22, 'Presupuesto insuficiente', 'Uno de los mayores riesgos es no contar con suficientes recursos financieros para llevar a cabo el proyecto. Esto podría resultar en retrasos, la implementación parcial del plan o la baja calidad de las mejoras.', 3, 5, 'Alta', 'Crítico', 19, 'Inicio', 28, 'A'),
	(23, 'Riesgo test', 'test', 2, 1, 'Baja', 'Bajo', 15, 'Inicio', 24, 'A'),
	(24, 'Riesgo de no asegurar fondos suficientes para la implementación y sostenibilidad del proyecto.', 'Riesgo de no asegurar fondos suficientes para la implementación y sostenibilidad del proyecto.', 4, 5, 'Alta', 'Alta', 17, 'Inicio', 25, 'A'),
	(25, 'El riesgo de no contar con los recursos financieros necesarios para ejecutar y mantener el proyecto.', 'El riesgo de no contar con los recursos financieros necesarios para ejecutar y mantener el proyecto.', 4, 5, 'Alta', 'Alta', 17, 'Inicio', 25, 'A'),
	(26, 'Posibles cambios en políticas gubernamentales o regulaciones', 'Posibles cambios en políticas gubernamentales o regulaciones que podrían impactar negativamente en la viabilidad del proyecto.', 4, 5, 'Alta', 'Alta', 17, 'Inicio', 26, 'A'),
	(27, 'Posibles cambios en políticas gubernamentales o regulaciones', 'Posibles cambios en políticas gubernamentales o regulaciones que podrían impactar negativamente en la viabilidad del proyecto.', 4, 5, 'Alta', 'Alta', 20, 'Inicio', 29, 'A'),
	(28, 'Riesgo de malentendidos, falta de comunicación efectiva ', 'Riesgo de malentendidos, falta de comunicación efectiva ', 4, 5, 'Alta', 'Alta', 17, 'Inicio', 25, 'A'),
	(29, 'Riesgo de malentendidos, falta de comunicación efectiva ', 'Riesgo de malentendidos, falta de comunicación efectiva ', 4, 5, 'Alta', 'Alta', 17, 'Inicio', 26, 'A'),
	(30, 'Riesgo de malentendidos, falta de comunicación efectiva ', 'Riesgo de malentendidos, falta de comunicación efectiva ', 4, 5, 'Alta', 'Alta', 20, 'Inicio', 29, 'A'),
	(31, 'Posibilidad de que las actividades del proyecto tengan consecuencias', 'Posibilidad de que las actividades del proyecto tengan consecuencias', 4, 5, 'Alta', 'Alta', 17, 'Inicio', 25, 'A'),
	(32, 'Posibilidad de que las actividades del proyecto tengan consecuencias', 'Posibilidad de que las actividades del proyecto tengan consecuencias', 4, 5, 'Alta', 'Alta', 17, 'Inicio', 26, 'A'),
	(33, 'Posibilidad de que las actividades del proyecto tengan consecuencias', 'Posibilidad de que las actividades del proyecto tengan consecuencias', 4, 5, 'Alta', 'Alta', 20, 'Inicio', 29, 'A'),
	(34, 'factores económicos y sociales externos', 'factores económicos y sociales externos', 4, 5, 'Alta', 'Alta', 17, 'Inicio', 25, 'A'),
	(35, 'factores económicos y sociales externos', 'factores económicos y sociales externos', 4, 5, 'Alta', 'Alta', 17, 'Inicio', 26, 'A'),
	(36, 'factores económicos y sociales externos', 'factores económicos y sociales externos', 4, 5, 'Alta', 'Alta', 20, 'Inicio', 29, 'A'),
	(37, 'Resistencia al Cambio', 'Resistencia por parte de la comunidad o de las partes interesadas a adoptar nuevas prácticas o enfoques', 5, 2, 'Media', 'Bajo', 16, 'Inicio', 25, 'A'),
	(38, 'Falta de Evaluación y Monitoreo', 'Insuficiente seguimiento y evaluación del progreso del proyecto, lo que dificulta la identificación temprana de problemas.', 2, 4, 'Media', 'Medio', 18, 'Inicio', 25, 'A'),
	(39, 'Riesgo test', 'test', 1, 1, 'Baja', 'Insignificante', 15, 'Inicio', 24, 'A');
/*!40000 ALTER TABLE `ta_riesgo` ENABLE KEYS */;

-- Volcando estructura para tabla risk_social.ta_rol
DROP TABLE IF EXISTS `ta_rol`;
CREATE TABLE IF NOT EXISTS `ta_rol` (
  `IN_ID_ROL` int(11) NOT NULL AUTO_INCREMENT,
  `VC_NOMBRE_ROL` varchar(1025) DEFAULT NULL,
  `CH_SITUACION_REGISTRO` char(1) DEFAULT NULL,
  PRIMARY KEY (`IN_ID_ROL`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Volcando datos para la tabla risk_social.ta_rol: ~4 rows (aproximadamente)
DELETE FROM `ta_rol`;
/*!40000 ALTER TABLE `ta_rol` DISABLE KEYS */;
INSERT INTO `ta_rol` (`IN_ID_ROL`, `VC_NOMBRE_ROL`, `CH_SITUACION_REGISTRO`) VALUES
	(1, 'Administrador', 'A'),
	(2, 'Miembro', 'A'),
	(3, 'Evaluador', 'A'),
	(4, 'Lider', 'A');
/*!40000 ALTER TABLE `ta_rol` ENABLE KEYS */;

-- Volcando estructura para tabla risk_social.ta_usuarios
DROP TABLE IF EXISTS `ta_usuarios`;
CREATE TABLE IF NOT EXISTS `ta_usuarios` (
  `IN_ID_USUARIO` int(11) NOT NULL AUTO_INCREMENT,
  `VC_USUARIO` varchar(256) DEFAULT NULL,
  `VC_PASS` varchar(256) DEFAULT NULL,
  `VC_NOMBRE` varchar(256) DEFAULT NULL,
  `VC_CORREO` varchar(256) DEFAULT NULL,
  `CH_SITUACION_REGISTRO` char(1) DEFAULT NULL,
  PRIMARY KEY (`IN_ID_USUARIO`),
  UNIQUE KEY `VC_CORREO` (`VC_CORREO`)
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Volcando datos para la tabla risk_social.ta_usuarios: ~7 rows (aproximadamente)
DELETE FROM `ta_usuarios`;
/*!40000 ALTER TABLE `ta_usuarios` DISABLE KEYS */;
INSERT INTO `ta_usuarios` (`IN_ID_USUARIO`, `VC_USUARIO`, `VC_PASS`, `VC_NOMBRE`, `VC_CORREO`, `CH_SITUACION_REGISTRO`) VALUES
	(15, 'nick.paredes', 'AQICAHjqSOhM76AJQFXwUBzD4HwxER2OXHyoK2Z/+QNwGTEDzQHnVpz2Gy1D7YaOxeNVj/H8AAAAYzBhBgkqhkiG9w0BBwagVDBSAgEAME0GCSqGSIb3DQEHATAeBglghkgBZQMEAS4wEQQMc445lp9r6fPXeQxZAgEQgCBsFOnWpM0u8m8Br2OwvmFRrtQvGiv6rbogXmU+TTS2wg==', 'Nick Paredes', 'nick.paredes@unmsm.edu.pe', 'A'),
	(16, 'carlos.villena', 'AQICAHjqSOhM76AJQFXwUBzD4HwxER2OXHyoK2Z/+QNwGTEDzQHDc61B1zDtciBUfN1m83lJAAAAYjBgBgkqhkiG9w0BBwagUzBRAgEAMEwGCSqGSIb3DQEHATAeBglghkgBZQMEAS4wEQQMtmMLiqsg5Kwh4eziAgEQgB+nJCzh2dd0SGJaj0NUc/Qf3tH9AOco/SJXHL50suqU', 'Carlos Villena', 'carlos.villena@unmsm.edu.pe', 'A'),
	(17, 'charlie', 'AQICAHjqSOhM76AJQFXwUBzD4HwxER2OXHyoK2Z/+QNwGTEDzQEvycCylwT+tqkGVaSM5fI7AAAAZTBjBgkqhkiG9w0BBwagVjBUAgEAME8GCSqGSIb3DQEHATAeBglghkgBZQMEAS4wEQQMQvAFsFBhfqvwSk69AgEQgCLU6fodRzc1esWbGnJgtDk+TKQfkdohmZSgNVtORxppgRXV', 'Admin Proyecto Charlie', 'charlie@unmsm.edu.pe', 'A'),
	(18, 'admin', 'AQICAHjqSOhM76AJQFXwUBzD4HwxER2OXHyoK2Z/+QNwGTEDzQFx5ukRdR4rjZiL4ALEbiEFAAAAYzBhBgkqhkiG9w0BBwagVDBSAgEAME0GCSqGSIb3DQEHATAeBglghkgBZQMEAS4wEQQMR4qnwWsGM2Ps/qR4AgEQgCDmzJgOwUpNuoEn4CYPjyRmDaJ5buNaBfwAZYlJHBvbNw==', 'Admin General', 'admin@admin.com', 'A'),
	(19, 'jhennyfer.zarate', 'AQICAHjqSOhM76AJQFXwUBzD4HwxER2OXHyoK2Z/+QNwGTEDzQGK8KEESQ3UGPoYO5udGll3AAAAbzBtBgkqhkiG9w0BBwagYDBeAgEAMFkGCSqGSIb3DQEHATAeBglghkgBZQMEAS4wEQQM6XCV/nFlbdhTV09tAgEQgCx94ACQdeabi5KxDrjTmJILXOK4KfD/Zx5QeomGEJidmVzUO5RAcVIX5l9WcQ==', 'Jhennyfer Zarate', 'jhennyferzarate21@gmail.com', 'A'),
	(20, 'nicole.tumi', 'AQICAHjqSOhM76AJQFXwUBzD4HwxER2OXHyoK2Z/+QNwGTEDzQEBseY+2nALeVxoua3mUTN4AAAAaDBmBgkqhkiG9w0BBwagWTBXAgEAMFIGCSqGSIb3DQEHATAeBglghkgBZQMEAS4wEQQM3u0mzgl/L4+HnscOAgEQgCWkByTzKyilpJHPJdaM7h+J55ThA+yQxP02cKI4Z5aKVetEhawH', 'Nicole Tumi', 'nicole.tumin@unmsm.edu.pe', 'A'),
	(21, 'nick', 'AQICAHjqSOhM76AJQFXwUBzD4HwxER2OXHyoK2Z/+QNwGTEDzQGnPBMmIMDyhULxwrcbt+A1AAAAYjBgBgkqhkiG9w0BBwagUzBRAgEAMEwGCSqGSIb3DQEHATAeBglghkgBZQMEAS4wEQQM3GvlssRl/2a1Tk/mAgEQgB/HGpKJzzqVErpP+xTzLarbqqTUUJQyb7D45gA/+KvJ', 'Nick', ' nick.carranza@unmsm.edu.pe', 'A');
/*!40000 ALTER TABLE `ta_usuarios` ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
