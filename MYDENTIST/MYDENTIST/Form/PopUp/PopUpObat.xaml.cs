﻿using MYDENTIST.Class;
using MYDENTIST.Class.DatabaseHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MYDENTIST.Form.PopUp
{

    public delegate void AddItemDelegateObat();

    public partial class PopUpObat : Window
    {
        public AddItemDelegateObat AddItemCallback;
        private decimal biayaBeliAngka;
        private decimal biayaJualAngka;

        private cds_MYSQLKonektor koneksi;
        private ParameterData[] param;
        private bool isEdit = false;

        public PopUpObat()
        {
            InitializeComponent();

            cmbJenis.Items.Add("Daftar");
            cmbJenis.Items.Add("Obat");

            isEdit = false;
            this.Title = "Tambah Data Obat";
            btnSimpan.Content = "Simpan";

            
        }

        public PopUpObat(string IdObat)
        {
            InitializeComponent();

            cmbJenis.Items.Add("Daftar");
            cmbJenis.Items.Add("Obat");

            isEdit = true;

            this.Title = "Ubah Data Obat";
            btnSimpan.Content = "Update";
            txtID.Text = IdObat;

            FetchEditData();

        }

        private void txtHargaBeli_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtHargaBeli.Text == string.Empty)
                txtHargaBeli.Text = "0";

            string value = txtHargaBeli.Text.Replace("Rp", "").Replace(".", "").Replace(",", "");
            //.Replace("Rp", "").Replace(",", "").TrimStart('0');

            decimal ul;
            //Check we are indeed handling a number
            if (decimal.TryParse(value, out ul))
            {
                biayaBeliAngka = ul;
                //ul /=  100;
                //Unsub the event so we don't enter a loop
                txtHargaBeli.TextChanged -= txtHargaBeli_TextChanged;
                //Format the text as currency
                txtHargaBeli.Text = string.Format(CultureInfo.CreateSpecificCulture("id-ID"), "{0:C0}", ul);
                txtHargaBeli.TextChanged += txtHargaBeli_TextChanged;
                txtHargaBeli.Select(txtHargaBeli.Text.Length, 0);
            }
            //bool goodToGo = TextisValid(txtBiaya.Text);

            //if (!goodToGo)
            //{
            //    txtBiaya.Text = "Rp0";
            //    txtBiaya.Select(txtBiaya.Text.Length, 0);
            //}

            //Console.WriteLine(ul);
        }

        private void txtHargaJual_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtHargaJual.Text == string.Empty)
                txtHargaJual.Text = "0";

            string value = txtHargaJual.Text.Replace("Rp", "").Replace(".", "").Replace(",", "");
            //.Replace("Rp", "").Replace(",", "").TrimStart('0');

            decimal ul;
            //Check we are indeed handling a number
            if (decimal.TryParse(value, out ul))
            {
                biayaJualAngka = ul;
                //ul /=  100;
                //Unsub the event so we don't enter a loop
                txtHargaJual.TextChanged -= txtHargaJual_TextChanged;
                //Format the text as currency
                txtHargaJual.Text = string.Format(CultureInfo.CreateSpecificCulture("id-ID"), "{0:C0}", ul);
                txtHargaJual.TextChanged += txtHargaJual_TextChanged;
                txtHargaJual.Select(txtHargaJual.Text.Length, 0);
            }
            //bool goodToGo = TextisValid(txtBiaya.Text);

            //if (!goodToGo)
            //{
            //    txtBiaya.Text = "Rp0";
            //    txtBiaya.Select(txtBiaya.Text.Length, 0);
            //}

            //Console.WriteLine(ul);
        }

        private void btnBatal_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSimpan_Click(object sender, RoutedEventArgs e)
        {
            if (txtNama.Text != string.Empty && cmbJenis.SelectedIndex != -1)
            {
                try
                {
                  
                    if (!isEdit) SimpanNew();
                    else EditUpdate();
                    
                }
                catch (Exception ex)
                {
                
                    MessageBox.Show("Terjadi kesalahan!", "Informasi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Mohon data field dengan tanda * diisi !", "Informasi", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            
        }


        void FetchEditData()
        {
            try
            {
                koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);
                DataTable Datatable = koneksi.GetDataTable("SELECT * FROM mydentist.tbl_obat WHERE mydentist.tbl_obat.id_obat =" + txtID.Text, null);
                foreach (DataRow row in Datatable.Rows)
                {
                    //MessageBox.Show(row["nama_karyawan"].ToString());
                    txtNama.Text = row["nama_obat"].ToString();
                    cmbJenis.SelectedItem = row["jenis_obat"].ToString();
                    txtHargaBeli.Text = row["hargabeli_obat"].ToString();
                    txtHargaJual.Text = row["hargajual_obat"].ToString();
                    txtStok.Text = row["stok_obat"].ToString();
                    txtKeterangan.Text = row["keterangan_obat"].ToString();
                }

                koneksi.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        void SimpanNew()
        {
            koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);
            param = new ParameterData[] { new ParameterData("nama_obat", txtNama.Text),
                                          new ParameterData("jenis_obat",  cmbJenis.SelectedItem),
                                          new ParameterData("hargabeli_obat", biayaBeliAngka),
                                          new ParameterData("hargajual_obat", biayaJualAngka),
                                          new ParameterData("stok_obat", txtStok.Text),
                                          new ParameterData("keterangan_obat", txtKeterangan.Text)};

            koneksi.InsertRow(SettingHelper.database, "tbl_obat", true, param);

            //@Bahar : Penting ketika melakukan fungsi InsertRow, kalau tidak dicommit data gk akan masuk ke database
            koneksi.Commit(true);

            //@Bahar : melaksanakan fungsi delegate
            AddItemCallback();

            koneksi.Dispose();

            this.Close();

            MessageBox.Show("Data obat berhasil ditambah", "Informasi", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        void EditUpdate()
        {
            koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);
            param = new ParameterData[] { new ParameterData("nama_obat", txtNama.Text),
                                          new ParameterData("jenis_obat",  cmbJenis.SelectedItem),
                                          new ParameterData("hargabeli_obat", biayaBeliAngka),
                                          new ParameterData("hargajual_obat", biayaJualAngka),
                                          new ParameterData("stok_obat", txtStok.Text),
                                          new ParameterData("keterangan_obat", txtKeterangan.Text)};

            koneksi.UpdateRow(SettingHelper.database, "tbl_obat", "id_obat=" + txtID.Text, 0, param);
            koneksi.Commit(true);

            AddItemCallback();
            koneksi.Dispose();

            this.Close();
            MessageBox.Show("Data obat berhasil diubah", "Informasi", MessageBoxButton.OK, MessageBoxImage.Information);

            

        }

        private void txtStok_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtStok.Text == string.Empty)
                txtStok.Text = "0";
        }

    }
}
