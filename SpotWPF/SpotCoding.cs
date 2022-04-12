using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotWPF {
    internal static class SpotCoding {
        private static string[] cEroCats = { "d23", "d24", "d25", "d26", "d72", "d73", "d74", "d75", "z03" };
        private static string[] cImageCats = { "DivX", "WMV", "MPG", "DVD5", "HD", "ePub", "Bluray", "HD", "HD", "x264", "DVD9", "PDF", "Bmp", "Vector", "3D", "UHD" };
        private static string[] cSoundCats = { "MP3", "WMA", "WAV", "OGG", "EAC", "DTS", "AAC", "APE", "FLAC" };
        private static string[] cGamesCats = { "Win", "Mac", "Linux", "PSX", "PS2", "PSP", "XBox", "360", "GBA", "GC", "NDS", "Wii", "PS3", "WP7", "iOs", "Android", "3DS", "PS4" };
        private static string[] cAppsCats = { "Win", "Mac", "Linux", "OS2", "WP7", "Navi", "iOs", "Android" };
        private static readonly string[] cCategories = {"Image", "Film", "Sound", "Games", "Applications", "Boeken", "Series", "7", "8", "Ero" };
        private static readonly string[] cTypeImage = {"Actie", "Avontuur", "Animatie", "Cabaret", "Komedie", "Misdaad", "Documentaire", "Drama", "Familie", "Fantasie",
                                "Filmhuis", "Televisie", "Horror", "Muziek", "Musical", "Mysterie", "Romantiek", "Science Fiction", "Sport", "Kort",
                                "Thriller", "Oorlog", "Western", "Hetero", "Homo", "Lesbo", "Bi", "Anders", "Aziatisch", "Anime",
                                "Cover", "Stripboek", "Cartoon", "Jeugd", "Zakelijk", "Computer", "Hobby", "Koken", "Knutselen", "Handwerk",
                                "Gezondheid", "Historie", "Psychologie", "Dagblad", "Tijdschrift", "Wetenschap", "Vrouw", "Religie", "Roman", "Biografie",
                                "Detective", "Dieren", "52", "Reizen", "Waargebeurd", "Non-fictie", "56", "Poezie", "Sprookje", "59",
                                "60", "61", "62", "63", "64", "65", "66", "67", "68", "69",
                                "70", "71", "Bi", "Lesbo", "Homo", "Hetero", "Amateur", "Groep", "POV", "Solo",
                                "Jong", "Soft", "Fetisj", "Oud", "BBW", "SM", "Hard", "Donker", "Hentai", "Buiten"};
        private static readonly string[] cTypeSound = {"Blues", "Compilatie", "Cabaret", "Dance", "Diversen", "Hardstyle", "Wereld", "Jazz", "Jeugd", "Klassiek", 
                                "Kleinkunst", "Hollands", "New Age", "Pop", "R&B", "Hiphop", "Reggae", "Religieus", "Rock", "Soundtrack", 
                                "Anders", "Hardstyle", "Aziatisch", "Disco", "Classics", "Metal", "Country", "Dubstep", "Nederhop", "DnB", 
                                "Electro", "Folk", "Soul", "Trance", "Balkan", "Techno", "Ambient", "Latin", "Live"};
        private static readonly string[] cTypeGames = {"Actie", "Avontuur", "Strategie", "Rollenspel", "Simulatie", "Race", "Vliegen", "Shooter", "Platform", "Sport", 
                                "Jeugd", "Puzzel", "Anders", "Bordspel", "Kaarten", "Educatie", "Muziek", "Party"};
        private static readonly string[] cTypeApp = {"Audio", "Video", "Grafisch", "CD/DVD Tools", "Media spelers", "Rippers & Encoders", "Plugins", "Database tools", "Email software", "Foto", 
                                "Screensavers", "Skin software", "Drivers", "Browsers", "Download managers", "Download", "Usenet software", "RSS Readers", "FTP software", "Firewalls", 
                                "Antivirus software", "Antispyware software", "Optimalisatiesoftware", "Beveiliging", "Systeem", "Anders", "Educatief", "Kantoor", "Internet", "Communicatie", 
                                "Ontwikkel", "Spotnet"};
        private static readonly string[] cLangMovie = {"Geen ondertitels", "Nederlands ondertiteld (extern)", "Nederlands ondertiteld (ingebakken)", "Engels ondertiteld (extern)", "Engels ondertiteld (ingebakken)", "Anders", "Nederlands ondertiteld (instelbaar)", "Engels ondertiteld (instelbaar)", "8", "9", 
                                "Engels gesproken", "Nederlands gesproken", "Duits gesproken", "Frans gesproken", "Spaans gesproken"};
        private static readonly string[] cLangBook = {"0", "1", "Nederlands", "2", "Engels", "Anders", "6", "7", "8", "9",
                                "10", "11", "Duits", "Frans", "Spaans"};
        private static readonly string[] cBitrate = {"Variabel", "< 96kbit", "96kbit", "128kbit", "160kbit", "192kbit", "256kbit", "320kbit", "Lossless", "Anders"};
        private static readonly string[] cGameSource = {"ISO", "Rip", "Retail", "DLC", "Anders", "Patch", "Crack"};

        internal static bool xIsEroCat(string pCat) {
            return cEroCats.Contains(pCat);
        }

        internal static bool xIsEbookCat(string pCat) {
            return (pCat == "a05" || pCat == "z02");
        }

        internal static bool xIsSerialsCat(string pCat) {
            return (pCat == "b04" || pCat == "d11" || pCat == "z01");
        }

        internal static string xCategory(int pCat) {
            if (pCat >= 0 && pCat < cCategories.Length) {
                return cCategories[pCat];
            } else {
                return "---";
            }
        }

        internal static string xLanguage(int pMainCat, int pSubCat) {
            string lResult;
            string[] lLang;

            if (pMainCat == 5) {
                lLang = cLangBook;
            } else {
                lLang = cLangMovie;
            }
            if (pSubCat >= 0 && pSubCat < lLang.Length) {
                lResult = lLang[pSubCat];
            } else {
                lResult = "--";
            }

            return lResult;
        }

        internal static string xFormat(int pMainCat, int pSubCat) {
            string lResult;
            string[] lCats;

            switch (pMainCat) {
                default: {
                        lCats = cImageCats;
                        break;
                    }
                case 2: {
                        lCats = cSoundCats;
                        break;
                    }
                case 3: {
                        lCats = cGamesCats;
                        break;
                    }
                case 4: {
                        lCats = cAppsCats;
                        break;
                    }
            }

            if (pSubCat >= 0 && pSubCat < lCats.Length) {
                lResult = lCats[pSubCat];
            } else {
                lResult = "--";
            }

            return lResult;
        }

        internal static string xTypeImage(int pCat) {
            if (pCat >= 0 && pCat < cTypeImage.Length) {
                return cTypeImage[pCat];
            } else {
                return "---";
            }
        }

        internal static string xTypeSound(int pCat) {
            if (pCat >= 0 && pCat < cTypeSound.Length) {
                return cTypeSound[pCat];
            } else {
                return "---";
            }
        }

        internal static string xTypeGames(int pCat) {
            if (pCat >= 0 && pCat < cTypeGames.Length) {
                return cTypeGames[pCat];
            } else {
                return "---";
            }
        }

        internal static string xTypeApp(int pCat) {
            if (pCat >= 0 && pCat < cTypeApp.Length) {
                return cTypeApp[pCat];
            } else {
                return "---";
            }
        }

        internal static string xBitrate(int pCat) {
            if (pCat >= 0 && pCat < cBitrate.Length) {
                return cBitrate[pCat];
            } else {
                return "---";
            }
        }

        internal static string xGameSource(int pCat) {
            if (pCat >= 0 && pCat < cGameSource.Length) {
                return cGameSource[pCat];
            } else {
                return "---";
            }
        }
    }
}
