-- phpMyAdmin SQL Dump
-- version 4.2.11
-- http://www.phpmyadmin.net
--
-- Host: 127.0.0.1
-- Generation Time: Aug 12, 2015 at 06:21 AM
-- Server version: 5.6.21
-- PHP Version: 5.6.3

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
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
`id_appo` int(255) NOT NULL,
  `id_pasien` int(50) NOT NULL,
  `id_dokter` int(50) NOT NULL,
  `tanggal_appo` datetime NOT NULL,
  `jam_appo` time NOT NULL,
  `norm_appo` varchar(50) NOT NULL,
  `namapasien_appo` varchar(255) NOT NULL,
  `namadokter_appo` varchar(255) NOT NULL,
  `status_appo` int(50) NOT NULL,
  `keterangan_appo` text NOT NULL
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tbl_appointment`
--

INSERT INTO `tbl_appointment` (`id_appo`, `id_pasien`, `id_dokter`, `tanggal_appo`, `jam_appo`, `norm_appo`, `namapasien_appo`, `namadokter_appo`, `status_appo`, `keterangan_appo`) VALUES
(8, 3, 0, '2015-08-12 00:00:00', '00:00:00', 'AA86779823', 'Hariyanto', 'Dimas', 0, 'Periksa'),
(9, 2, 0, '2015-08-12 00:00:00', '00:00:00', 'AA09876354', 'Suryanti', 'Dimas', 1, 'kjghjhglhjx jv'),
(10, 4, 0, '2015-08-12 00:00:00', '00:00:00', 'AA54635324324', 'Hari Managi', 'Angging Wahyu Wibowo', 1, 'YYYY');

-- --------------------------------------------------------

--
-- Table structure for table `tbl_karyawan`
--

CREATE TABLE IF NOT EXISTS `tbl_karyawan` (
`id_karyawan` int(50) NOT NULL,
  `nama_karyawan` varchar(255) NOT NULL,
  `jenis_karyawan` varchar(50) NOT NULL,
  `alamat_karyawan` text NOT NULL,
  `telp_karyawan` varchar(50) NOT NULL,
  `tglmasuk_karyawan` date NOT NULL,
  `keterangan_karyawan` text NOT NULL
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=latin1;

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
`id_obat` int(11) NOT NULL,
  `nama_obat` varchar(255) NOT NULL,
  `jenis_obat` varchar(255) NOT NULL,
  `hargabeli_obat` int(50) NOT NULL,
  `hargajual_obat` int(50) NOT NULL,
  `stok_obat` int(11) NOT NULL,
  `keterangan_obat` text NOT NULL
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

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
`id_pasien` int(11) NOT NULL,
  `norm_pasien` varchar(50) NOT NULL,
  `nama_pasien` varchar(255) NOT NULL,
  `alamat_pasien` text NOT NULL,
  `telp_pasien` varchar(50) NOT NULL,
  `keterangan_pasien` text NOT NULL
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tbl_pasien`
--

INSERT INTO `tbl_pasien` (`id_pasien`, `norm_pasien`, `nama_pasien`, `alamat_pasien`, `telp_pasien`, `keterangan_pasien`) VALUES
(2, 'AA09876354', 'Suryanti A', 'Jln. Bantul No 54 Sewon Bantul Yogyakarta', '653647823981', 'Sakit Gigi Ya'),
(3, 'AA86779823', 'Hariyanto', 'Jln.Parangtritis Km4 Bantul Yogyakarta', '676432432123', 'Cabut Gigi 2x'),
(4, 'AA54635324324', 'Hari Managi', 'Jln. Sleman', '32423654323', 'sfdsfdafdsf');

-- --------------------------------------------------------

--
-- Table structure for table `tbl_terapi`
--

CREATE TABLE IF NOT EXISTS `tbl_terapi` (
`id_terapi` int(11) NOT NULL,
  `nama_terapi` varchar(255) NOT NULL,
  `jenis_terapi` int(50) NOT NULL,
  `biaya_terapi` int(50) NOT NULL,
  `keterangan_terapi` text NOT NULL
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tbl_terapi`
--

INSERT INTO `tbl_terapi` (`id_terapi`, `nama_terapi`, `jenis_terapi`, `biaya_terapi`, `keterangan_terapi`) VALUES
(1, 'Cabut Gigi', 10, 100000, 'Cabut Gigi');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `tbl_appointment`
--
ALTER TABLE `tbl_appointment`
 ADD PRIMARY KEY (`id_appo`);

--
-- Indexes for table `tbl_karyawan`
--
ALTER TABLE `tbl_karyawan`
 ADD PRIMARY KEY (`id_karyawan`);

--
-- Indexes for table `tbl_obat`
--
ALTER TABLE `tbl_obat`
 ADD PRIMARY KEY (`id_obat`);

--
-- Indexes for table `tbl_pasien`
--
ALTER TABLE `tbl_pasien`
 ADD PRIMARY KEY (`id_pasien`);

--
-- Indexes for table `tbl_terapi`
--
ALTER TABLE `tbl_terapi`
 ADD PRIMARY KEY (`id_terapi`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `tbl_appointment`
--
ALTER TABLE `tbl_appointment`
MODIFY `id_appo` int(255) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=11;
--
-- AUTO_INCREMENT for table `tbl_karyawan`
--
ALTER TABLE `tbl_karyawan`
MODIFY `id_karyawan` int(50) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=17;
--
-- AUTO_INCREMENT for table `tbl_obat`
--
ALTER TABLE `tbl_obat`
MODIFY `id_obat` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=4;
--
-- AUTO_INCREMENT for table `tbl_pasien`
--
ALTER TABLE `tbl_pasien`
MODIFY `id_pasien` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=5;
--
-- AUTO_INCREMENT for table `tbl_terapi`
--
ALTER TABLE `tbl_terapi`
MODIFY `id_terapi` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=2;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
