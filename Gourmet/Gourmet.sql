-- MySQL dump 10.13  Distrib 5.6.45, for Win64 (x86_64)
--
-- Host: localhost    Database: Gourmet
-- ------------------------------------------------------
-- Server version	5.6.45-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `dishes`
--

CREATE DATABASE IF NOT EXISTS `gourmeet`;

USE `gourmeet`;

DROP TABLE IF EXISTS `dishes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `dishes` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(150) CHARACTER SET utf8 DEFAULT NULL,
  `cost` int(11) DEFAULT NULL,
  `weight` int(11) DEFAULT NULL,
  `vegan_friendly` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=28 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `dishes`
--

LOCK TABLES `dishes` WRITE;
/*!40000 ALTER TABLE `dishes` DISABLE KEYS */;
INSERT INTO `dishes` VALUES (1,'Тартар из лосося с дайконом',550,300,0),(2,'Медальоны из индейки с томатами и моцареллой',580,320,0),(3,'Ризотто с шампиньонами и пармезаном',300,280,1),(4,'Лобио из красной фасоли с зеленью',200,260,1),(5,'Цезарь традиционный с курицей',320,280,0),(6,'Сырное ассорти',400,200,1),(22,'Оливье с курицей',220,200,0),(25,'Оливье вегетарианский',180,200,1),(26,'Сок томатный',80,300,1),(27,'Сок яблочный',80,300,1);
/*!40000 ALTER TABLE `dishes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `orders`
--

DROP TABLE IF EXISTS `orders`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `orders` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `id_table` int(11) DEFAULT NULL COMMENT 'tables.id',
  `id_waiter` int(11) DEFAULT NULL COMMENT 'personnel.id',
  `menu` varchar(250) DEFAULT NULL,
  `status` int(11) DEFAULT '0' COMMENT '-1 cancelled, 0 active, 1 completed',
  PRIMARY KEY (`id`),
  KEY `table_orders` (`id_table`),
  KEY `waiter_orders` (`id_waiter`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `orders`
--

LOCK TABLES `orders` WRITE;
/*!40000 ALTER TABLE `orders` DISABLE KEYS */;
INSERT INTO `orders` VALUES (1,1,6,'{\"2\":2,\"3\":3,\"4\":4,\"6\":2,\"25\":1}',1),(2,3,2,'{\"4\":2,\"6\":3,\"25\":2,\"26\":2}',0),(3,1,10,'{\"4\":2,\"6\":3,\"27\":1}',-1);
/*!40000 ALTER TABLE `orders` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `personnel`
--

DROP TABLE IF EXISTS `personnel`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `personnel` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(100) DEFAULT NULL,
  `position` enum('Официант','Шеф-повар','Повар') DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `personnel`
--

LOCK TABLES `personnel` WRITE;
/*!40000 ALTER TABLE `personnel` DISABLE KEYS */;
INSERT INTO `personnel` VALUES (1,'Воронов Конрад Алексеевич','Шеф-повар'),(2,'Антонова Алина Богдановна','Официант'),(3,'Исаков Добрыня Петрович','Официант'),(4,'Посидайло Янина Борисовна','Повар'),(5,'Шилова Инесса Львовна','Официант'),(6,'Носкова Полина Алексеевна','Официант'),(7,'Гамула Роберт Григорьевич','Официант'),(8,'Гриневская Галина Леонидовна','Шеф-повар'),(9,'Ильин Нестор Станиславович','Повар'),(10,'Шарова Юлия Станиславовна','Официант'),(11,'Бердник Денис Артёмович','Повар'),(12,'Летов Константин Сергеевич','Официант'),(13,'Щукина Анастасия Павловна','Повар'),(14,'Тихомирова Яна Семеновна','Официант');
/*!40000 ALTER TABLE `personnel` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tables`
--

DROP TABLE IF EXISTS `tables`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tables` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `capacity` int(11) DEFAULT NULL,
  `is_free` int(11) DEFAULT '1',
  `current_order` int(11) DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tables`
--

LOCK TABLES `tables` WRITE;
/*!40000 ALTER TABLE `tables` DISABLE KEYS */;
INSERT INTO `tables` VALUES (1,2,1,0),(2,4,0,0),(3,4,0,2),(4,2,1,0),(5,6,1,0),(6,6,1,0);
/*!40000 ALTER TABLE `tables` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2019-09-02 16:15:51
