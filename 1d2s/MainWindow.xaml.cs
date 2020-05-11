using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;          // OpenFileDialog, SaveFileDialog, ColorDialog, FontDialog

namespace _1d2s
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Değişkenler
        string gelenSarki;
        string ResimYolu;
        string ResimYoluXML;
        int gelenRow;
        string[] Puan = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", };
        OpenFileDialog ofdResim;
        DataSet ds;
        #endregion

        private void BilgileriGetir()
        {
            ds = new DataSet();
            ds.ReadXml(System.AppDomain.CurrentDomain.BaseDirectory + "Movies.xml");
            dtgrdListele.ItemsSource = ds.Tables[0].DefaultView;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BilgileriGetir();

        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Çıkmak İstiyon Mu ? ", "EXIT", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (MessageBoxResult.Yes == result)
            {
                Close();
            }
        }

        private void dtgrdListele_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            mdelmntPlayer.Close();
            spPlayer.Visibility = Visibility.Collapsed;
            spPuan.Children.Clear();
            DataRowView drv = (DataRowView)dtgrdListele.SelectedItem;
            string gelenResim = (drv["Poster"]).ToString();
            string gelenTitle = (drv["Title"]).ToString();
            string gelenYear = (drv["Year"]).ToString();
            string gelenRuntime = (drv["Runtime"]).ToString();
            string gelenGenres = (drv["Genres"]).ToString();
            string gelenSummary = (drv["Summary"]).ToString();
            string gelenDirectory = (drv["Director"]).ToString();
            string gelenStars = (drv["Stars"]).ToString();
            string gelenSoundtrack = (drv["Soundtrack"]).ToString();
            int gelenRating = int.Parse((drv["Rating"]).ToString());

            imgPoster.Source = new BitmapImage(new Uri("bin/Debug" + gelenResim, UriKind.Relative));

            txbTitle.Text = gelenTitle;
            txbYear.Text = gelenYear;
            txbRuntime.Text = gelenRuntime + " " + gelenGenres;
            txbSummary.Text = gelenSummary;
            txbDirector.Text = gelenDirectory;
            txbStars.Text = gelenStars;
            btnSoundtrack.Tag = gelenSoundtrack;

            for (int i = 0; i < 10; i++)
            {
                Image imgStar = new Image()
                {
                    Width = 32,
                    Height = 32,
                    Margin = new Thickness(0, 0, 0, 3)
                };

                if (i < gelenRating)
                {
                    imgStar.Source = new BitmapImage(new Uri("img/star1.png", UriKind.Relative));
                }
                else
                {
                    imgStar.Source = new BitmapImage(new Uri("img/star2.png", UriKind.Relative));
                }
                spFilmİçerik.Visibility = Visibility.Visible;
                spPuan.Children.Add(imgStar);
            }
        }

        private void btnSoundtrack_Click(object sender, RoutedEventArgs e)
        {
            spPlayer.Visibility = Visibility.Visible;
            gelenSarki = btnSoundtrack.Tag.ToString();
            lblSoundtrack.Content = gelenSarki;
            mdelmntPlayer.Source = new Uri(gelenSarki, UriKind.Relative);
            sldrMüzikVolume.Value = 5;
            mdelmntPlayer.Volume = sldrMüzikVolume.Value;
        }

        private void imgResim_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image gelenImage = (Image)sender;
            try
            {
                ofdResim = new OpenFileDialog();
                ofdResim.Title = "Lütfen Resim Seçiniz";
                ofdResim.Filter = "(JPEG Dosyaları)|*.jpg|(PNG Dosyaları)|*.png|(Tüm Dosyalar)|*.*";
                ofdResim.FilterIndex = -1;
                ofdResim.InitialDirectory = "C:\\";

                if ((bool)ofdResim.ShowDialog())
                {
                    gelenImage.Source = new BitmapImage(new Uri(ofdResim.FileName, UriKind.Absolute));
                    ResimYolu = ("posters\\" + System.IO.Path.GetFileName(ofdResim.FileName)).ToString();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Dosya Okuma Hatası", "H A T A", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void cmbxGuncelle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            gelenRow = cmbxGuncelle.SelectedIndex;
            DataRow drv = ds.Tables[0].Rows[gelenRow];
            txtYear1.Text = drv["Year"].ToString();
            txtRuntime1.Text = drv["Runtime"].ToString();
            txtGenres1.Text = drv["Genres"].ToString();
            txtSummary1.Text = drv["Summary"].ToString();
            txtDirectors1.Text = drv["Director"].ToString();
            txtStars1.Text = drv["Stars"].ToString();
            string gelenResim = (drv["Poster"]).ToString();

            if (gelenResim.Length > 0)
            {
                imgResim1.Source = new BitmapImage(new Uri("bin/Debug" + gelenResim, UriKind.Relative));
            }
            else
            {
                imgResim1.Source = new BitmapImage(new Uri("img/add.png", UriKind.Relative));
            }

            cmbxRating1.SelectedItem = drv["Rating"].ToString();
            txtSoundtrack1.Text = drv["Soundtrack"].ToString();
        }

        private void btnKaydet_Click(object sender, RoutedEventArgs e)
        {
            string Title = txtTitle.Text;
            string Year = txtYear.Text;
            string Runtime = txtRuntime.Text;
            string Genres = txtGenres.Text;
            string Directors = txtDirectors.Text;
            string Stars = txtStars.Text;
            string Summary = txtSummary.Text;
            string Soundtrack = txtSoundtrack.Text;
            string Poster = ResimYoluXML;
            string Rating = cmbxRating.Text;

            DataRow dr = ds.Tables[0].NewRow();
            dr["Title"] = Title;
            dr["Year"] = Year;
            dr["Runtime"] = Runtime;
            dr["Genres"] = Genres;
            dr["Director"] = Directors;
            dr["Stars"] = Stars;
            dr["Summary"] = Summary;
            dr["Soundtrack"] = Soundtrack;
            dr["Poster"] = Poster;
            dr["Rating"] = Rating;

            ds.Tables[0].Rows.Add(dr);
            ds.AcceptChanges();
            ds.WriteXml(System.AppDomain.CurrentDomain.BaseDirectory + "Movies.xml");
            File.Copy(ofdResim.FileName, ResimYolu, true);
            BilgileriGetir();
        }

        private void btnGuncelle_Click(object sender, RoutedEventArgs e)
        {
            ds.Tables[0].Rows[gelenRow]["Title"] = cmbxGuncelle.Text;
            ds.Tables[0].Rows[gelenRow]["Year"] = txtYear1.Text;
            ds.Tables[0].Rows[gelenRow]["Runtime"] = txtRuntime1.Text;
            ds.Tables[0].Rows[gelenRow]["Genres"] = txtGenres1.Text;
            ds.Tables[0].Rows[gelenRow]["Director"] = txtDirectors1.Text;
            ds.Tables[0].Rows[gelenRow]["Stars"] = txtStars1.Text;
            ds.Tables[0].Rows[gelenRow]["Summary"] = txtSummary1.Text;
            ds.Tables[0].Rows[gelenRow]["Soundtrack"] = txtSoundtrack1.Text;
            ds.Tables[0].Rows[gelenRow]["Rating"] = cmbxRating1.Text;
            ds.Tables[0].Rows[gelenRow]["Poster"] = ResimYoluXML;

            ds.WriteXml(System.AppDomain.CurrentDomain.BaseDirectory + "Movies.xml");
            BilgileriGetir();
        }

        private void btnSil_Click(object sender, RoutedEventArgs e)
        {
            ds.Tables[0].Rows[cmbxSil.SelectedIndex].Delete();
            ds.WriteXml(System.AppDomain.CurrentDomain.BaseDirectory + "Movies.xml");
            BilgileriGetir();
        }

        private void Image_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            lblSoundtrack.Content = gelenSarki;
            Image gelenPlayerButton = (Image)sender;
            string gelenPlayerButonTag = gelenPlayerButton.Tag.ToString();

            switch (gelenPlayerButonTag)
            {
                case "play":
                    mdelmntPlayer.Play();
                    break;
                case "pause":
                    mdelmntPlayer.Pause();
                    break;
                case "stop":
                    mdelmntPlayer.Stop();
                    break;
                case "rewind":
                    mdelmntPlayer.Position -= TimeSpan.FromSeconds(3);
                    break;
                case "forward":
                    mdelmntPlayer.Position += TimeSpan.FromSeconds(3);
                    break;
                case "mute":
                    mdelmntPlayer.IsMuted = !mdelmntPlayer.IsMuted;
                    break;
                default:
                    MessageBox.Show("Hata");
                    break;
            }
        }
    }
}