-- phpMyAdmin SQL Dump
-- version 3.5.2.2
-- http://www.phpmyadmin.net
--
-- Host: 127.0.0.1
-- Generation Time: Aug 11, 2015 at 04:37 PM
-- Server version: 5.5.27
-- PHP Version: 5.4.7

SET SQL_MODE="NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Database: `mydentist`
--

-- --------------------------------------------------------

--
-- Table structure for table `tbl_appointment`
--

CREATE TABLE IF NOT EXISTS `tbl_appointment` (
  `id_appo` int(255) NOT NULL AUTO_INCREMENT,
  `tanggal_appo` datetime NOT NULL,
  `jam_appo` time NOT NULL,
  `norm_appo` varchar(50) NOT NULL,
  `namapasien_appo` varchar(255) NOT NULL,
  `namadokter_appo` varchar(255) NOT NULL,
  `status_appo` int(50) NOT NULL,
  `keterangan_appo` text NOT NULL,
  PRIMARY KEY (`id_appo`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=2 ;

--
-- Dumping data for table `tbl_appointment`
--

INSERT INTO `tbl_appointment` (`id_appo`, `tanggal_appo`, `jam_appo`, `norm_appo`, `namapasien_appo`, `namadokter_appo`, `status_appo`, `keterangan_appo`) VALUES
(1, '2015-08-21 00:00:00', '01:02:03', 'AA09876354', 'Suryanti', 'Angging Wahyu Wibowo', 0, '');

-- --------------------------------------------------------

--
-- Table structure for table `tbl_karyawan`
--

CREATE TABLE IF NOT EXISTS `tbl_karyawan` (
  `id_karyawan` int(50) NOT NULL AUTO_INCREMENT,
  `nama_karyawan` varchar(255) NOT NULL,
  `jenis_karyawan` varchar(50) NOT NULL,
  `alamat_karyawan` text NOT NULL,
  `telp_karyawan` varchar(50) NOT NULL,
  `tglmasuk_karyawan` date NOT NULL,
  `keterangan_karyawan` text NOT NULL,
  PRIMARY KEY (`id_karyawan`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=17 ;

--
-- Dumping data for table `tbl_karyawan`
--

INSERT INTO `tbl_karyawan` (`id_karyawan`, `nama_karyawan`, `jenis_karyawan`, `alamat_karyawan`, `telp_karyawan`, `tglmasuk_karyawan`, `keterangan_karyawan`) VALUES
(14, 'Angging Wahyu Wibowo', 'Dokter', 'Sorowajan RT10 Panggungharjo Sewon Bantul Yogyakarta', '085702363359', '2015-01-18', 'Ini adalah percobaan'),
(16, 'Dimas', 'Dokter', 'Sorowajan RT10 Panggungharjo Sewon Bantul Yogyakarta', '0857774534', '2014-01-09', 'Sukses');

-- --------------------------------------------------------

--
-- Table structure for table `tbl_obat`
--

CREATE TABLE IF NOT EXISTS `tbl_obat` (
  `id_obat` int(11) NOT NULL AUTO_INCREMENT,
  `nama_obat` varchar(255) NOT NULL,
  `jenis_obat` varchar(255) NOT NULL,
  `hargabeli_obat` int(50) NOT NULL,
  `hargajual_obat` int(50) NOT NULL,
  `stok_obat` int(11) NOT NULL,
  `keterangan_obat` text NOT NULL,
  PRIMARY KEY (`id_obat`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=4 ;

--
-- Dumping data for table `tbl_obat`
--

INSERT INTO `tbl_obat` (`id_obat`, `nama_obat`, `jenis_obat`, `hargabeli_obat`, `hargajual_obat`, `stok_obat`, `keterangan_obat`) VALUES
(2, 'Combantrin', 'Obat', 5000, 5500, 25, 'Obat Cacing'),
(3, 'Sulfatilamit', 'Obat', 20000, 23000, 10, 'Obat sunat');

-- --------------------------------------------------------

--
-- Table structure for table `tbl_pasien`
--

CREATE TABLE IF NOT EXISTS `tbl_pasien` (
  `id_pasien` int(11) NOT NULL AUTO_INCREMENT,
  `norm_pasien` varchar(50) NOT NULL,
  `nama_pasien` varchar(255) NOT NULL,
  `alamat_pasien` text NOT NULL,
  `telp_pasien` varchar(50) NOT NULL,
  `keterangan_pasien` text NOT NULL,
  PRIMARY KEY (`id_pasien`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=4 ;

--
-- Dumping data for table `tbl_pasien`
--

INSERT INTO `tbl_pasien` (`id_pasien`, `norm_pasien`, `nama_pasien`, `alamat_pasien`, `telp_pasien`, `keterangan_pasien`) VALUES
(2, 'AA09876354', 'Suryanti', 'Jln. Bantul No 54 Sewon Bantul Yogyakarta', '653647823981', 'Sakit Gigi'),
(3, 'AA86779823', 'Hariyanto', 'Jln.Parangtritis Km4 Bantul Yogyakarta', '676432432123', 'Cabut Gigi 2x');

-- --------------------------------------------------------

--
-- Table structure for table `tbl_terapi`
--

CREATE TABLE IF NOT EXISTS `tbl_terapi` (
  `id_terapi` int(11) NOT NULL AUTO_INCREMENT,
  `nama_terapi` varchar(255) NOT NULL,
  `jenis_terapi` int(50) NOT NULL,
  `biaya_terapi` int(50) NOT NULL,
  `keterangan_terapi` text NOT NULL,
  PRIMARY KEY (`id_terapi`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=2 ;

--
-- Dumping data for table `tbl_terapi`
--

INSERT INTO `tbl_terapi` (`id_terapi`, `nama_terapi`, `jenis_terapi`, `biaya_terapi`, `keterangan_terapi`) VALUES
(1, 'Cabut Gigi', 10, 100000, 'Cabut Gigi');

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
