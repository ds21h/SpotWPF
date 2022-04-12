using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotWPF {
    internal class SpotPage {
        private const string cFontSize = "18";
        private SpotExt mSpot;
        private string mHtml;

        internal SpotPage(SpotExt pSpot) {
            mSpot = pSpot;
        }

        internal async Task<string> xAsyncCreatePage() {
            string lHtml;
            string lInfo;

            await sGetHtmlBase();
            lInfo = sMakeInfo();
//            lInfo = "<br>Info<br>";
            lHtml = mHtml.Replace("[SN:TITLE]", mSpot.xTitle).Replace("[SN:DESC]", mSpot.xDescription).Replace("[SN:FONTSIZE]", cFontSize).Replace("[SN:PATH]", Global.cHomeDir).Replace("[SN:INFO]", lInfo);

            return lHtml;
        }

        private async Task sGetHtmlBase() {
            StreamReader lReader;

            try {
                lReader = new StreamReader(Global.cHomeDir + @"\" + Global.cSpotBase);
                mHtml = await lReader.ReadToEndAsync().ConfigureAwait(false);
            } catch (Exception) {
                mHtml = "";
            }
       }

        private string sMakeInfo() {
            StringBuilder lInfo;
            bool lFirst;

            lInfo = new StringBuilder();
            lInfo.Append("<TABLE BORDER=0>");
            lInfo.Append("<TR><TD><b>Categorie:&nbsp;<b></TD><TD>" + SpotCoding.xCategory(mSpot.xCategory) + "</TD></TR>");
            switch (mSpot.xCategory) {
                default: {
                        lInfo.Append("<TR><TD><b>Formaat:&nbsp;<b></TD><TD>" + mSpot.xFormat + "</TD></TR>");
                        lFirst = true;
                        foreach (var bSCat in mSpot.xSubC) {
                            if (lFirst) {
                                lInfo.Append("<TR><TD><b>Taal:&nbsp;<b></TD><TD>" + SpotCoding.xLanguage(mSpot.xCategory, bSCat) + "</TD></TR>");
                                lFirst = false;
                            } else {
                                lInfo.Append("<TR><TD><b>&nbsp;&nbsp;<b></TD><TD>" + SpotCoding.xLanguage(mSpot.xCategory, bSCat) + "</TD></TR>");
                            }
                        }
                        lFirst = true;
                        foreach (var bSCat in mSpot.xSubD) {
                            if (lFirst) {
                                lInfo.Append("<TR><TD><b>Genre:&nbsp;<b></TD><TD>" + SpotCoding.xTypeImage(bSCat) + "</TD></TR>");
                                lFirst = false;
                            } else {
                                lInfo.Append("<TR><TD><b>&nbsp;&nbsp;<b></TD><TD>" + SpotCoding.xTypeImage(bSCat) + "</TD></TR>");
                            }
                        }
                        break;
                    }
                case 2: {
                        lInfo.Append("<TR><TD><b>Formaat:&nbsp;<b></TD><TD>" + mSpot.xFormat + "</TD></TR>");
                        lFirst = true;
                        foreach (var bSCat in mSpot.xSubC) {
                            if (lFirst) {
                                lInfo.Append("<TR><TD><b>Bitrate:&nbsp;<b></TD><TD>" + SpotCoding.xBitrate(bSCat) + "</TD></TR>");
                                lFirst = false;
                            } else {
                                lInfo.Append("<TR><TD><b>&nbsp;&nbsp;<b></TD><TD>" + SpotCoding.xBitrate(bSCat) + "</TD></TR>");
                            }
                        }
                        lFirst = true;
                        foreach (var bSCat in mSpot.xSubD) {
                            if (lFirst) {
                                lInfo.Append("<TR><TD><b>Genre:&nbsp;<b></TD><TD>" + SpotCoding.xTypeSound(bSCat) + "</TD></TR>");
                                lFirst = false;
                            } else {
                                lInfo.Append("<TR><TD><b>&nbsp;&nbsp;<b></TD><TD>" + SpotCoding.xTypeSound(bSCat) + "</TD></TR>");
                            }
                        }
                        break;
                    }
                case 3: {
                        lFirst = true;
                        foreach (var bSCat in mSpot.xSubB) {
                            if (lFirst) {
                                lInfo.Append("<TR><TD><b>Formaat:&nbsp;<b></TD><TD>" + SpotCoding.xGameSource(bSCat) + "</TD></TR>");
                                lFirst = false;
                            } else {
                                lInfo.Append("<TR><TD><b>&nbsp;&nbsp;<b></TD><TD>" + SpotCoding.xGameSource(bSCat) + "</TD></TR>");
                            }
                        }
                        lInfo.Append("<TR><TD><b>Platform:&nbsp;<b></TD><TD>" + SpotCoding.xFormat(mSpot.xCategory, mSpot.xSubA) + "</TD></TR>");
                        lFirst = true;
                        foreach (var bSCat in mSpot.xSubC) {
                            if (lFirst) {
                                lInfo.Append("<TR><TD><b>Genre:&nbsp;<b></TD><TD>" + SpotCoding.xTypeGames(bSCat) + "</TD></TR>");
                                lFirst = false;
                            } else {
                                lInfo.Append("<TR><TD><b>&nbsp;&nbsp;<b></TD><TD>" + SpotCoding.xTypeGames(bSCat) + "</TD></TR>");
                            }
                        }
                        break;
                    }
                case 4: {
                        lInfo.Append("<TR><TD><b>Platform:&nbsp;<b></TD><TD>" + SpotCoding.xFormat(mSpot.xCategory, mSpot.xSubA) + "</TD></TR>");
                        lFirst = true;
                        foreach (var bSCat in mSpot.xSubB) {
                            if (lFirst) {
                                lInfo.Append("<TR><TD><b>Genre:&nbsp;<b></TD><TD>" + SpotCoding.xTypeApp(bSCat) + "</TD></TR>");
                                lFirst = false;
                            } else {
                                lInfo.Append("<TR><TD><b>&nbsp;&nbsp;<b></TD><TD>" + SpotCoding.xTypeApp(bSCat) + "</TD></TR>");
                            }
                        }
                        break;
                    }
            }
            lInfo.Append("<TR><TD><b>Omvang:&nbsp;<b></TD><TD>" + mSpot.xSizeStr + "</TD></TR>");
            lInfo.Append("<TR><TD><b>Spotter:&nbsp;<b></TD><TD>" + mSpot.xPoster + "</TD></TR>");
            lInfo.Append("<TR><TD><b>Moment:&nbsp;<b></TD><TD>" + mSpot.xCreatedLocalString + "</TD></TR>");

            //Dim sGroups As String = ""
            //Dim TransCat As String = ""
            //xTable.Append(GetCatInfo(xSpot.Category, xSpot.SubCats, xSpot.Tag, sGroups, xSpot.Poster, xSpot.User))
            //    xTable.Append("<TR><TD><b>Omvang</b></TD><TD>" & Utils.ConvertSize(xSpot.Filesize) & "</TD></TR>")
            //    xTable.Append("<TR><TD>&nbsp;</TD><TD>&nbsp;</TD></TR>")
            //If Len(xSpot.Web) > 0 Then
            //    xTable.Append("<TR><TD><b>Website</b></TD><TD><A onfocus='this.blur()' TITLE='Link openen' HREF=" & Chr(34) & "link:" & Utils.SafeHref(xSpot.Web) & Chr(34) & ">" & Utils.HtmlEncode(Utils.URLDecode(xSpot.Web)) & "</A></TD></TR>")
            //Else
            //    xTable.Append("<TR><TD><b>Website</b></TD><TD><A onfocus='this.blur()' TITLE='Link openen' HREF=" & Chr(34) & "link:" & Utils.SafeHref(MakeGoogleSearch(xSpot.Title)) & Chr(34) & ">" & Utils.HtmlEncode(Utils.URLDecode(MakeGoogleSearch(xSpot.Title))) & "</A></TD></TR>")
            //End If
            //xTable.Append(sGroups)
            //xTable.Append("</TABLE>")

            //Return xTable.ToString
            lInfo.Append("</TABLE>");
            return lInfo.ToString();
        }

        //        private string sGetCatInfo(ByVal hCat As Byte, ByVal Cats As String, ByVal zTags As String, ByRef zGroups As String, ByVal sPoster As String, ByVal zUser As UserInfo) {
        //    Dim zAdd As String
        //Dim zCat() As String
        //Dim TypeCat As Byte = 0
        //Dim sColor As String = "black"
        //Dim sColor2 As String = "blue"

        //Dim sOutTags As String = vbNullString
        //Dim OutPoster As String = vbNullString

        //Dim iCatA As New Dictionary(Of String, String)
        //Dim iCatB As New Dictionary(Of String, String)
        //Dim iCatC As New Dictionary(Of String, String)
        //Dim iCatD As New Dictionary(Of String, String)
        //Dim iCatZ As New Dictionary(Of String, String)

        //Dim CatTags As New Collection

        //zCat = Split(Cats, "|")

        //For i = 0 To UBound(zCat)
        //    If Len(zCat(i)) > 0 Then

        //        zAdd = Spotz.TranslateCat(hCat, zCat(i))

        //        If Len(zAdd) = 0 Then Continue For

        //        Select Case Microsoft.VisualBasic.Left(zCat(i), 1).ToLower

        //            Case "a"

        //                If Not iCatA.ContainsKey(zCat(i)) Then
        //                    iCatA.Add(zCat(i), zAdd)
        //                End If

        //            Case "b"

        //                If Not iCatB.ContainsKey(zCat(i)) Then
        //                    iCatB.Add(zCat(i), zAdd)
        //                End If

        //            Case "c"

        //                If Not iCatC.ContainsKey(zCat(i)) Then
        //                    iCatC.Add(zCat(i), zAdd)
        //                End If

        //            Case "d"

        //                If hCat = 9 Then

        //                    Select Case zCat(i).ToLower
        //                        Case "d75", "d74", "d73", "d72"
        //                            If Cats.Contains("d2") Then
        //                                Continue For ' Double Cat fix
        //                            End If
        //                    End Select

        //                End If

        //                If Not iCatD.ContainsKey(zCat(i)) Then
        //                    iCatD.Add(zCat(i), zAdd)
        //                End If

        //            Case "z"

        //                If Not iCatZ.ContainsKey(zCat(i)) Then

        //                    TypeCat = CByte(Val(zCat(i).ToLower.Replace("z", "")) + 1)
        //                    iCatZ.Add(zCat(i), zAdd)

        //                End If

        //        End Select
        //    End If
        //Next

        //zCat = Split(zTags, " ")

        //For i = 0 To UBound(zCat)
        //    If Len(Trim$(zCat(i))) > 0 Then
        //        CatTags.Add(zCat(i))
        //    End If
        //Next

        //Dim sApp As String

        //If CatTags.Count > 0 Then
        //    sApp = vbNullString
        //    sOutTags = "<TR><TD><b>Tag" & Utils.sIIF(CatTags.Count > 1, "s", "") & "</b></TD><TD>"
        //    For Each sG As String In CatTags
        //        If sG<> sPoster Then
        //           sOutTags += sApp & CreateClassLink("query:" & Utils.URLEncode(Utils.StripChars(sG) & "_" & "tag MATCH '" & Utils.StripChars(sG).ToLower & "'"), Utils.StripChars(sG), "category", "Zoeken") & "</TD></TR>"
        //            sApp = "<TR>&nbsp;<TD></TD><TD>"
        //        End If
        //    Next
        //    If Len(sApp) = 0 Then sOutTags = vbNullString
        //End If

        //Dim sTooltip As String = "Onbekend"

        //If zUser.ValidSignature Then
        //    sTooltip = Utils.HtmlEncode(Spotz.MakeUnique(zUser.Modulus))
        //End If

        //If Len(zUser.Organisation) > 0 Then
        //    sTooltip += vbCrLf & Utils.HtmlEncode(zUser.Organisation)
        //End If

        //Dim Ref As MainWindow = CType(Application.Current.MainWindow, MainWindow)
        //Dim SuperSpot As Boolean = zUser.ValidSignature And Ref.WhiteList.Contains(zUser.Modulus)

        //OutPoster = "<TR><TD><b>Afzender</b></TD><TD>" & CreateClassLink("menu:" & Utils.sIIF(zUser.ValidSignature, zUser.Modulus, "") & "_" & Utils.StripChars(sPoster), Utils.StripChars(sPoster), Utils.sIIF(SuperSpot, "trusted", "from"), sTooltip) & "</TD></TR>"

        //Dim sBr As String = "<TR><TD>&nbsp;</TD><TD>&nbsp;</TD></TR>"

        //zGroups += sBr & OutPoster & sOutTags

        //Dim TransCat As String = Spotz.CatDesc(hCat, TypeCat)
        //Dim sSort As String = "<TR><TD><b>Categorie&nbsp;&nbsp;&nbsp;</b></TD><TD>" & CreateClassLink("query:" & Utils.URLEncode(TransCat & "_" & "cat = " & CStr(hCat)), TransCat, "category", "Zoeken") & "</TD></TR>"

        //Dim sOutZ As String = MakeCats(hCat, "z"c, iCatZ, sColor, sColor2)
        //Dim sOutA As String = MakeCats(hCat, "a"c, iCatA, sColor, sColor2)
        //Dim sOutB As String = MakeCats(hCat, "b"c, iCatB, sColor, sColor2)
        //Dim sOutC As String = MakeCats(hCat, "c"c, iCatC, sColor, sColor2)
        //Dim sOutD As String = MakeCats(hCat, "d"c, iCatD, sColor, sColor2)

        //Select Case hCat

        //    Case 2

        //        If TypeCat > 1 Then sSort = sOutZ
        //        sSort += sOutA & sOutB & sOutC & sOutD

        //    Case 3
        //        sSort += sOutB & sOutA & sOutC

        //    Case 4
        //        sSort += sOutA & sOutB

        //    Case Else
        //        sSort += sOutA & sOutB & sOutC & sOutD

        //End Select

        //Return sSort
        //        }

        //Public Shared Function TranslateCat(ByVal hCat As Long, ByVal sCat As String, Optional ByVal bStrict As Boolean = False) As String

        //    If sCat.Length< 2 Then Return ""

        //    Select Case hCat

        //        Case 2

        //            Select Case sCat.Substring(0, 1)

        //                Case "a"

        //                    Select Case CInt(sCat.Substring(1))
        //                        Case 0 : Return "MP3"
        //                        Case 1 : Return "WMA"
        //                        Case 2 : Return "WAV"
        //                        Case 3 : Return "OGG"
        //                        Case 4 : Return "EAC"
        //                        Case 5 : Return "DTS"
        //                        Case 6 : Return "AAC"
        //                        Case 7 : Return "APE"
        //                        Case 8 : Return "FLAC"
        //                        Case Else : Return ""
        //                    End Select

        //                Case "b"

        //                    Select Case CInt(sCat.Substring(1))
        //                        Case 0 : Return "CD"
        //                        Case 1 : Return "Radio"
        //                        Case 3 : Return "DVD"
        //                        Case 5 : Return "Vinyl"
        //                        Case 2 : Return Utils.sIIF(bStrict, "", "Compilatie")
        //                        Case 4 : Return "" ''"Anders"
        //                        Case 6 : Return "Stream"
        //                        Case Else : Return ""
        //                    End Select

        //                Case "c"

        //                    Select Case CInt(sCat.Substring(1))
        //                        Case 1 : Return "< 96kbit"
        //                        Case 2 : Return "96kbit"
        //                        Case 3 : Return "128kbit"
        //                        Case 4 : Return "160kbit"
        //                        Case 5 : Return "192kbit"
        //                        Case 6 : Return "256kbit"
        //                        Case 7 : Return "320kbit"
        //                        Case 8 : Return "Lossless"
        //                        Case 0 : Return "Variabel"
        //                        Case 9 : Return "" ''"Anders"
        //                        Case Else : Return ""
        //                    End Select

        //                Case "d"
        //                    Select Case CInt(sCat.Substring(1))
        //                        Case 0 : Return "Blues"
        //                        Case 1 : Return "Compilatie"
        //                        Case 2 : Return "Cabaret"
        //                        Case 3 : Return "Dance"
        //                        Case 4 : Return "Diversen"
        //                        Case 5 : Return "Hardstyle"
        //                        Case 6 : Return "Wereld"
        //                        Case 7 : Return "Jazz"
        //                        Case 8 : Return "Jeugd"
        //                        Case 9 : Return "Klassiek"
        //                        Case 10 : Return Utils.sIIF(bStrict, "", "Kleinkunst")
        //                        Case 11 : Return "Hollands"
        //                        Case 12 : Return Utils.sIIF(bStrict, "", "New Age")
        //                        Case 13 : Return "Pop"
        //                        Case 14 : Return "RnB"
        //                        Case 15 : Return "Hiphop"
        //                        Case 16 : Return "Reggae"
        //                        Case 17 : Return "Religieus"
        //                        Case 18 : Return "Rock"
        //                        Case 19 : Return "Soundtrack"
        //                        Case 20 : Return "" ''"Anders"
        //                        Case 21 : Return Utils.sIIF(bStrict, "", "Hardstyle")
        //                        Case 22 : Return Utils.sIIF(bStrict, "", "Aziatisch")
        //                        Case 23 : Return "Disco"
        //                        Case 24 : Return "Classics"
        //                        Case 25 : Return "Metal"
        //                        Case 26 : Return "Country"
        //                        Case 27 : Return "Dubstep"
        //                        Case 28 : Return Utils.sIIF(bStrict, "", "Nederhop")
        //                        Case 29 : Return "DnB"
        //                        Case 30 : Return "Electro"
        //                        Case 31 : Return "Folk"
        //                        Case 32 : Return "Soul"
        //                        Case 33 : Return "Trance"
        //                        Case 34 : Return "Balkan"
        //                        Case 35 : Return "Techno"
        //                        Case 36 : Return "Ambient"
        //                        Case 37 : Return "Latin"
        //                        Case 38 : Return "Live"
        //                        Case Else : Return ""
        //                    End Select

        //                Case "z"

        //                    Select Case CInt(sCat.Substring(1))
        //                        Case 0 : Return "Album"
        //                        Case 1 : Return "Liveset"
        //                        Case 2 : Return "Podcast"
        //                        Case 3 : Return "Luisterboek"
        //                        Case Else : Return ""
        //                    End Select

        //                Case Else

        //                    Return ""

        //            End Select

        //        Case 3

        //            Select Case sCat.Substring(0, 1)

        //                Case "a"

        //                    Select Case CInt(sCat.Substring(1))
        //                        Case 0 : Return "Windows"
        //                        Case 1 : Return "Macintosh"
        //                        Case 2 : Return "Linux"
        //                        Case 3 : Return "Playstation"
        //                        Case 4 : Return "Playstation 2"
        //                        Case 5 : Return "PSP"
        //                        Case 6 : Return "XBox"
        //                        Case 7 : Return "XBox 360"
        //                        Case 8 : Return "Gameboy Advance"
        //                        Case 9 : Return "Gamecube"
        //                        Case 10 : Return "Nintendo DS"
        //                        Case 11 : Return "Nintendo Wii"
        //                        Case 12 : Return "Playstation 3"
        //                        Case 13 : Return "Windows Phone"
        //                        Case 14 : Return "iOs"
        //                        Case 15 : Return "Android"
        //                        Case 16 : Return Utils.sIIF(bStrict, "", "Nintendo 3DS")
        //                        Case Else : Return ""
        //                    End Select

        //                Case "b"

        //                    Select Case CInt(sCat.Substring(1))
        //                        Case 1 : Return "Rip"
        //                        Case 0 : Return Utils.sIIF(bStrict, "", "ISO")
        //                        Case 2 : Return "Retail"
        //                        Case 3 : Return "DLC"
        //                        Case 4 : Return "" ''"Anders"
        //                        Case 5 : Return "Patch"
        //                        Case 6 : Return "Crack"
        //                        Case Else : Return ""
        //                    End Select

        //                Case "c"

        //                    Select Case CInt(sCat.Substring(1))
        //                        Case 0 : Return "Actie"
        //                        Case 1 : Return "Avontuur"
        //                        Case 2 : Return "Strategie"
        //                        Case 3 : Return "Rollenspel"
        //                        Case 4 : Return "Simulatie"
        //                        Case 5 : Return "Race"
        //                        Case 6 : Return "Vliegen"
        //                        Case 7 : Return "Shooter"
        //                        Case 8 : Return "Platform"
        //                        Case 9 : Return "Sport"
        //                        Case 10 : Return "Jeugd"
        //                        Case 11 : Return "Puzzel"
        //                        Case 12 : Return "" ''"Anders"
        //                        Case 13 : Return "Bordspel"
        //                        Case 14 : Return "Kaarten"
        //                        Case 15 : Return "Educatie"
        //                        Case 16 : Return "Muziek"
        //                        Case 17 : Return "Party"
        //                        Case Else : Return ""
        //                    End Select

        //                Case Else

        //                    Return ""

        //            End Select

        //        Case 4

        //            Select Case sCat.Substring(0, 1)

        //                Case "a"

        //                    Select Case CInt(sCat.Substring(1))
        //                        Case 0 : Return "Windows"
        //                        Case 1 : Return "Macintosh"
        //                        Case 2 : Return "Linux"
        //                        Case 3 : Return "OS/2"
        //                        Case 4 : Return "Windows Phone"
        //                        Case 5 : Return "Navigatie"
        //                        Case 6 : Return "iOs"
        //                        Case 7 : Return "Android"
        //                        Case Else : Return ""
        //                    End Select

        //                Case "b"
        //                    Select Case CInt(sCat.Substring(1))
        //                        Case 0 : Return "Audio"
        //                        Case 1 : Return "Video"
        //                        Case 2 : Return "Grafisch"
        //                        Case 3 : Return Utils.sIIF(bStrict, "", "CD/DVD Tools")
        //                        Case 4 : Return Utils.sIIF(bStrict, "", "Media spelers")
        //                        Case 5 : Return Utils.sIIF(bStrict, "", "Rippers & Encoders")
        //                        Case 6 : Return Utils.sIIF(bStrict, "", "Plugins")
        //                        Case 7 : Return Utils.sIIF(bStrict, "", "Database tools")
        //                        Case 8 : Return Utils.sIIF(bStrict, "", "Email software")
        //                        Case 9 : Return "Foto"
        //                        Case 10 : Return Utils.sIIF(bStrict, "", "Screensavers")
        //                        Case 11 : Return Utils.sIIF(bStrict, "", "Skin software")
        //                        Case 12 : Return Utils.sIIF(bStrict, "", "Drivers")
        //                        Case 13 : Return Utils.sIIF(bStrict, "", "Browsers")
        //                        Case 14 : Return Utils.sIIF(bStrict, "", "Download managers")
        //                        Case 15 : Return "Download"
        //                        Case 16 : Return Utils.sIIF(bStrict, "", "Usenet software")
        //                        Case 17 : Return Utils.sIIF(bStrict, "", "RSS Readers")
        //                        Case 18 : Return Utils.sIIF(bStrict, "", "FTP software")
        //                        Case 19 : Return Utils.sIIF(bStrict, "", "Firewalls")
        //                        Case 20 : Return Utils.sIIF(bStrict, "", "Antivirus software")
        //                        Case 21 : Return Utils.sIIF(bStrict, "", "Antispyware software")
        //                        Case 22 : Return Utils.sIIF(bStrict, "", "Optimalisatiesoftware")
        //                        Case 23 : Return "Beveiliging"
        //                        Case 24 : Return "Systeem"
        //                        Case 25 : Return "" ''"Anders"
        //                        Case 26 : Return "Educatief"
        //                        Case 27 : Return "Kantoor"
        //                        Case 28 : Return "Internet"
        //                        Case 29 : Return "Communicatie"
        //                        Case 30 : Return "Ontwikkel"
        //                        Case 31 : Return "Spotnet"
        //                        Case Else : Return ""
        //                    End Select

        //                Case Else

        //                    Return ""

        //            End Select

        //        Case Else

        //            Select Case sCat.Substring(0, 1)

        //                Case "a"

        //                    Select Case CInt(sCat.Substring(1))
        //                        Case 0 : Return "DivX"
        //                        Case 1 : Return "WMV"
        //                        Case 2 : Return "MPG"
        //                        Case 3 : Return "DVD5"
        //                        Case 4 : Return Utils.sIIF(bStrict, "", "HD Overig")
        //                        Case 5 : Return "ePub"
        //                        Case 6 : Return "Bluray"
        //                        Case 7 : Return Utils.sIIF(bStrict, "", "HD-DVD")
        //                        Case 8 : Return Utils.sIIF(bStrict, "", "WMV HD")
        //                        Case 9 : Return "x264"
        //                        Case 10 : Return "DVD9"
        //                        Case Else : Return ""
        //                    End Select

        //                Case "b"

        //                    Select Case CInt(sCat.Substring(1))
        //                        Case 4 : Return Utils.sIIF(bStrict, "", "TV")
        //                        Case 1 : Return Utils.sIIF(bStrict, "", "(S)VCD")
        //                        Case 6 : Return Utils.sIIF(bStrict, "", "Satelliet")
        //                        Case 2 : Return Utils.sIIF(bStrict, "", "Promo")
        //                        Case 3 : Return "Retail"
        //                        Case 7 : Return "R5"
        //                        Case 0 : Return "Cam"
        //                        Case 8 : Return Utils.sIIF(bStrict, "", "Telecine")
        //                        Case 9 : Return "Telesync"
        //                        Case 5 : Return "" '' "Anders"
        //                        Case 10 : Return "Scan"
        //                        Case Else : Return ""
        //                    End Select

        //                Case "c"

        //                    Select Case CInt(sCat.Substring(1))
        //                        Case 0 : Return "Geen ondertitels"
        //                        Case 3 : Return "Engels ondertiteld (extern)"
        //                        Case 4 : Return Utils.sIIF(hCat<> 5, "Engels ondertiteld (ingebakken)", "Engels geschreven")
        //                        Case 7 : Return "Engels ondertiteld (instelbaar)"
        //                        Case 1 : Return "Nederlands ondertiteld (extern)"
        //                        Case 2 : Return Utils.sIIF(hCat<> 5, "Nederlands ondertiteld (ingebakken)", "Nederlands geschreven")
        //                        Case 6 : Return "Nederlands ondertiteld (instelbaar)"
        //                        Case 10 : Return "Engels gesproken"
        //                        Case 11 : Return "Nederlands gesproken"
        //                        Case 12 : Return Utils.sIIF(hCat<> 5, "Duits gesproken", "Duits geschreven")
        //                        Case 13 : Return Utils.sIIF(hCat<> 5, "Frans gesproken", "Frans geschreven")
        //                        Case 14 : Return Utils.sIIF(hCat<> 5, "Spaans gesproken", "Spaans geschreven")
        //                        Case 5 : Return ""  '' "Anders"
        //                        Case Else : Return ""
        //                    End Select

        //                Case "d"

        //                    Select Case CInt(sCat.Substring(1))

        //                        Case 0 : Return "Actie"
        //                        Case 29 : Return "Anime"
        //                        Case 2 : Return "Animatie"
        //                        Case 28 : Return "Aziatisch"
        //                        Case 1 : Return "Avontuur"
        //                        Case 3 : Return "Cabaret"
        //                        Case 32 : Return "Cartoon"
        //                        Case 6 : Return "Documentaire"
        //                        Case 7 : Return "Drama"
        //                        Case 8 : Return "Familie"
        //                        Case 9 : Return "Fantasie"
        //                        Case 10 : Return "Filmhuis"
        //                        Case 12 : Return "Horror"
        //                        Case 33 : Return "Jeugd"
        //                        Case 4 : Return "Komedie"
        //                        Case 19 : Return "Kort"
        //                        Case 5 : Return "Misdaad"
        //                        Case 13 : Return "Muziek"
        //                        Case 14 : Return "Musical"
        //                        Case 15 : Return "Mysterie"
        //                        Case 21 : Return "Oorlog"
        //                        Case 16 : Return "Romantiek"
        //                        Case 17 : Return "Science Fiction"
        //                        Case 18 : Return "Sport"
        //                        Case 11 : Return Utils.sIIF(bStrict, "", "Televisie")
        //                        Case 20 : Return "Thriller"
        //                        Case 22 : Return "Western"

        //                        Case 23 : Return "Hetero"
        //                        Case 24 : Return "Homo"
        //                        Case 25 : Return "Lesbo"
        //                        Case 26 : Return "Bi"

        //                        Case 27 : Return "" '' "Anders"

        //                        Case 30 : Return "Cover"
        //                        Case 43 : Return "Dagblad"
        //                        Case 44 : Return "Tijdschrift"
        //                        Case 31 : Return "Stripboek"

        //                        Case 34 : Return "Zakelijk"
        //                        Case 35 : Return "Computer"
        //                        Case 36 : Return "Hobby"
        //                        Case 37 : Return "Koken"
        //                        Case 38 : Return "Knutselen"
        //                        Case 39 : Return "Handwerk"
        //                        Case 40 : Return "Gezondheid"
        //                        Case 41 : Return "Historie"
        //                        Case 42 : Return "Psychologie"
        //                        Case 45 : Return "Wetenschap"
        //                        Case 46 : Return "Vrouw"
        //                        Case 47 : Return "Religie"
        //                        Case 48 : Return "Roman"
        //                        Case 49 : Return "Biografie"
        //                        Case 50 : Return "Detective"
        //                        Case 51 : Return "Dieren"
        //                        Case 52 : Return ""
        //                        Case 53 : Return "Reizen"
        //                        Case 54 : Return "Waargebeurd"
        //                        Case 55 : Return "Non-fictie"
        //                        Case 57 : Return "Poezie"
        //                        Case 58 : Return "Sprookje"

        //                        Case 75 : Return Utils.sIIF(bStrict, "", "Hetero")
        //                        Case 74 : Return Utils.sIIF(bStrict, "", "Homo")
        //                        Case 73 : Return Utils.sIIF(bStrict, "", "Lesbo")
        //                        Case 72 : Return Utils.sIIF(bStrict, "", "Bi")

        //                        Case 76 : Return "Amateur"
        //                        Case 77 : Return "Groep"
        //                        Case 78 : Return "POV"
        //                        Case 79 : Return "Solo"
        //                        Case 80 : Return "Jong"
        //                        Case 81 : Return "Soft"
        //                        Case 82 : Return "Fetisj"
        //                        Case 83 : Return "Oud"
        //                        Case 84 : Return "BBW"
        //                        Case 85 : Return "SM"
        //                        Case 86 : Return "Hard"
        //                        Case 87 : Return "Donker"
        //                        Case 88 : Return "Hentai"
        //                        Case 89 : Return "Buiten"

        //                        Case Else : Return ""

        //                    End Select

        //                Case "z"

        //                    Select Case CInt(sCat.Substring(1))
        //                        Case 0 : Return "Film"
        //                        Case 1 : Return "Serie"
        //                        Case 2 : Return "Boek"
        //                        Case 3 : Return "Erotiek"
        //                        Case Else : Return ""
        //                    End Select

        //                Case Else

        //                    Return ""

        //            End Select

        //    End Select

        //End Function

    }
}
