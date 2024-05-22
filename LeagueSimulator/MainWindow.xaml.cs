using LeagueClassLibrary.DataAccess;
using LeagueClassLibrary.Entities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace LeagueSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Match currentMatch;
        public MainWindow()
        {
            InitializeComponent();
            string[] positions = { "sup", "mid", "bot", "jung", "top" };
            ComboBoxPositions.ItemsSource = positions;
        }

        private void LaadChampionDataButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = "Enkel csv bestanden (*.csv)|*.csv"
            };
            if(ofd.ShowDialog() == true)
            {
                try
                {
                    ChampionData.LoadCSV(ofd.FileName);
                    CheckBoxLaadChamionData.IsChecked = true;
                    EnableTabsEnDataGridAlsDataGeladen();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message,"Fout bestand",MessageBoxButton.OK);
                    CheckBoxLaadChamionData.IsChecked = false;
                }
            }
        }

        private void LaadAbilityDataButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = "Enkel csv bestanden (*.csv)|*.csv"
            };
            if (ofd.ShowDialog() == true)
            {
                try
                {
                    AbilityData.LoadCSV(ofd.FileName);
                    CheckBoxLaadAbilityData.IsChecked = true;
                    MatchData.InitializeDataTableMatches();
                    EnableTabsEnDataGridAlsDataGeladen();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Fout bestand", MessageBoxButton.OK);
                    CheckBoxLaadAbilityData.IsChecked = false;
                }
            }
        }
        private void EnableTabsEnDataGridAlsDataGeladen()
        {
            if (CheckBoxLaadChamionData.IsChecked == true &&
                CheckBoxLaadAbilityData.IsChecked == true)
            {
                TabItemSimuleerMatch.IsEnabled = true;
                TabItemOverzichtMatches.IsEnabled = true;

                DataGridChampions.ItemsSource = ChampionData.GetDataViewChampions();
            }
        }

        private void ComboBoxPositions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(CheckBoxLaadChamionData.IsChecked == true && CheckBoxLaadAbilityData.IsChecked == true)
            {
                string position = ComboBoxPositions.SelectedItem.ToString();
                DataGridChampions.ItemsSource = ChampionData.GetDataViewChampionsByPosition(position);
            }
        }

        private void BestToWorstButton_Click(object sender, RoutedEventArgs e)
        {
            DataGridChampions.ItemsSource = ChampionData.GetDataViewChampionsBestToWorst();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            DataGridChampions.ItemsSource = ChampionData.GetDataViewChampions();
            TextBlockChampionTitle.Text = "Name and Title";
            ImageChampion.Source = null;
        }

        private void DataGridChampions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridChampions.SelectedItem != null)
            {
                DataRowView row = DataGridChampions.SelectedItem as DataRowView;
                string name = row.Row.Field<string>("ChampionName");
                string title = row.Row.Field<string>("ChampionTitle");
                string imagePath = "images/" + row.Row.Field<string>("ChampionIcon");
                TextBlockChampionTitle.Text = $"{name} {title}";
                ImageChampion.Source = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
            }

        }

        private void LaadChampion(int indexChampion, int team)
        {
            if(currentMatch != null)
            {
                List<Champion> teamChampion;
                if(team == 1)
                {
                    teamChampion = currentMatch.Team1Champions;
                }
                else
                {
                    teamChampion = currentMatch.Team2Champions;
                }
                if (indexChampion < teamChampion.Count())
                {
                    ImageBanner.Source = new BitmapImage(new Uri("images/" + teamChampion[indexChampion].BannerSource, UriKind.RelativeOrAbsolute));
                    TextBlockChampion.Text = teamChampion[indexChampion].ToString();
                    TextBlockClass.Text = teamChampion[indexChampion].Class;
                    TextBlockCost.Text = teamChampion[indexChampion].GetCost();
                    List<Ability> abilities = AbilityData.GetAbilitiesByChampionName(teamChampion[indexChampion].Name);
                    ListBoxChampionAbilities.Items.Clear();
                    foreach (Ability ability in abilities)
                    {
                        ListBoxChampionAbilities.Items.Add(ability);
                    }
                }
            }
        }
        private void LaadChampionIcons()
        {
            if(currentMatch != null)
            {
                int index = 0;
                foreach(Image champIcon in GridTeam1.Children)
                {
                    if(index >= currentMatch.Team1Champions.Count())
                    {
                        champIcon.Source = new BitmapImage(new Uri("images/icons/empty_icon.png", UriKind.RelativeOrAbsolute));
                    }
                    else
                    {
                        champIcon.Source = new BitmapImage(new Uri("images/" + currentMatch.Team1Champions[index].IconSource, UriKind.RelativeOrAbsolute));
                        index++;
                    }
                }

                int index2 = 0;
                foreach (Image champIcon in GridTeam2.Children)
                {
                    if (index2 >= currentMatch.Team2Champions.Count())
                    {
                        champIcon.Source = new BitmapImage(new Uri("images/icons/empty_icon.png", UriKind.RelativeOrAbsolute));
                    }
                    else
                    {
                        champIcon.Source = new BitmapImage(new Uri("images/" + currentMatch.Team2Champions[index2].IconSource, UriKind.RelativeOrAbsolute));
                        index2++;
                    }
                }
            }
        }

        private void ImageIconChampion1Team1_MouseEnter(object sender, MouseEventArgs e)
        {
            LaadChampion(0, 1);
        }

        private void ImageIconChampion2Team1_MouseEnter(object sender, MouseEventArgs e)
        {
            LaadChampion(1, 1);
        }

        private void ImageIconChampion3Team1_MouseEnter(object sender, MouseEventArgs e)
        {
            LaadChampion(2, 1);
        }

        private void ImageIconChampion4Team1_MouseEnter(object sender, MouseEventArgs e)
        {
            LaadChampion(3, 1);
        }

        private void ImageIconChampion5Team1_MouseEnter(object sender, MouseEventArgs e)
        {
            LaadChampion(4, 1);
        }

        private void ImageIconChampion1Team2_MouseEnter(object sender, MouseEventArgs e)
        {
            LaadChampion(0, 2);
        }

        private void ImageIconChampion2Team2_MouseEnter(object sender, MouseEventArgs e)
        {
            LaadChampion(1, 2);
        }

        private void ImageIconChampion3Team2_MouseEnter(object sender, MouseEventArgs e)
        {
            LaadChampion(2, 2);
        }

        private void ImageIconChampion4Team2_MouseEnter(object sender, MouseEventArgs e)
        {
            LaadChampion(3,2);
        }

        private void ImageIconChampion5Team2_MouseEnter(object sender, MouseEventArgs e)
        {
            LaadChampion(4, 2);
        }

        private void Genereer5v5Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(PasswordBoxMatchCode.Password))
            {
                if (MatchData.IsUniqueCode(PasswordBoxMatchCode.Password))
                {
                    SummonersRift summonersRift = new SummonersRift(PasswordBoxMatchCode.Password);
                    currentMatch = summonersRift;
                    currentMatch.GenereerTeams();
                    LaadChampionIcons();
                }
                else
                {
                    MessageBox.Show("Deze code is reeds gebruikt. Probeer opnieuw", "Code niet uniek", MessageBoxButton.OK);
                }

            }
            else
            {
                MessageBox.Show("geef een code mee.", "Geen code", MessageBoxButton.OK);
            }
        }

        private void Genereer3v3Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(PasswordBoxMatchCode.Password))
            {
                if (MatchData.IsUniqueCode(PasswordBoxMatchCode.Password))
                {
                    TwistedTreeline twistedTreeline = new TwistedTreeline(PasswordBoxMatchCode.Password);
                    currentMatch = twistedTreeline;
                    currentMatch.GenereerTeams();
                    LaadChampionIcons();
                }
                else
                {
                    MessageBox.Show("Deze code is reeds gebruikt. Probeer opnieuw", "Code niet uniek", MessageBoxButton.OK);
                }

            }
            else
            {
                MessageBox.Show("geef een code mee.", "Geen code", MessageBoxButton.OK);
            }
        }

        private void ExportToXMLButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Enkel xml bestanden toegelaten (*.xml)|*.xml";
            if(sfd.ShowDialog() == true)
            {
                try
                {
                    MatchData.ExportToXML(sfd.FileName);
                    MessageBox.Show("Bestand is succesvol geexporteerd!", "XML export", MessageBoxButton.OK);
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
        }

        private void BeslisWinnaarButton_Click(object sender, RoutedEventArgs e)
        {
            if(currentMatch != null)
            {
                currentMatch.DecideWinner();
                MatchData.AddFinishedMatch(currentMatch);
                DataGridMatches.ItemsSource = MatchData.GetDataViewMatches();
                ClearSimulatieTab();
            }
            else
            {
                MessageBox.Show("Genereer een teams eerst!");
            }
        }

        private void ClearSimulatieTab()
        {
            currentMatch = null;
            TextBlockChampion.Text = "";
            TextBlockClass.Text = "";
            TextBlockCost.Text = "";
            ListBoxChampionAbilities.Items.Clear();
            foreach(Image champIcon in GridTeam1.Children)
            {
                champIcon.Source = new BitmapImage(new Uri("images/icons/empty_icon.png", UriKind.RelativeOrAbsolute));
            }
            foreach (Image champIcon in GridTeam2.Children)
            {
                champIcon.Source = new BitmapImage(new Uri("images/icons/empty_icon.png", UriKind.RelativeOrAbsolute));
            }
        }
    }
}
