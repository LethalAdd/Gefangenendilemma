﻿using System;
using System.Collections.Generic;
using Gefangenendilemma.Basis;

namespace Gefangenendilemma
{
    /// <summary>
    /// Diese Klasse können Sie beliebig umschreiben, jenachdem welche Tasks sie erledigen. 
    ///Erledigte Task:
    ///S1-2: +30 (je +15) für eine eigene Strategie pro Gruppenmitglied (bei 2 Gruppenmitgliern) Dazu einfach die Strategie1-2 mit Leben füllen.
    ///B1: +15 Eine ihrer Strategie schlägt die Verrate immer Strategie (Wenn Sie die Aufgabe machen, geben Sie an, welche Strategie, wieviele Runden und die Art des Verstoßes)
    ///B2: +15 Eine ihre Strategie schlägt die Groll Strategie (Wenn Sie die Aufgabe machen, geben Sie an, welche Strategie, wieviele Runden und die Art des Verstoßes)
    ///P3: +5 Kommentieren Sie ihre komplexen Codeabschnitte und Methoden C# konform
    ///E1: +10 Ergänzung, dass auch leichte und mittlere Schwere im Programm gespielt werden können.
    ///E2: +10 Neue Funktion, dass 2 Strategien die 9 Spiele gegenander spielen und dann erst der Sieger gekürt wird.
    ///E3: +10 Neue Funktion, die ermittelt, ab welcher Rundenzahl gewinnt die eine Strategie zuverlässig.
    ///R1: +10 Sie haben im Team ein Reposity verwendet, also jedes Teammitglied hat mehr als 2 Commits gemacht.
    
    /// </summary>
    class VerwaltungProgramm
    {
        /// <summary>
        /// Diese Liste(Collection) enthält alle Gefangene/Strategien
        /// </summary>
        private static List<BasisStrategie> _strategien;
        
        static void Main(string[] args)
        {
            //bekannt machen der ganzen strategien
            _strategien = new List<BasisStrategie>();
            _strategien.Add(new GrollStrategie());
            _strategien.Add(new VerrateImmerStrategie());
            _strategien.Add(new Strategie1());
            _strategien.Add(new Strategie2());
            _strategien.Add(new Strategie3());
            
            string eingabe = null;

            do
            {
                // Begrüßung
                Console.WriteLine("Willkommen zum Gefangenendilemma");
                Console.WriteLine("0 - Verhör zwischen 2 Strategien");
                Console.WriteLine("X - Beenden");
                
                // Eingabe
                Console.WriteLine("Treffen Sie ihre Option: ");
                eingabe = Console.ReadLine();
               
                // Auswerten der Eingabe
                switch (eingabe.ToLower())
                {
                    case "0":
                        Gefangene2();
                        break;
                    case "X":
                        break;
                    default:
                        Console.WriteLine($"Eingabe {eingabe} nicht erkannt.");
                        break;
                }
            } while (!"x".Equals(eingabe?.ToLower()));
            Console.WriteLine(eingabe);
        }

        /// <summary>
        /// Fragt 2 Strategien, Länge und Schwere ab.
        /// </summary>
        static void Gefangene2()
        {
            int st1, st2;
            int schwere, runde;
            int anzahlSpiele = 9;
            
            Console.WriteLine("Willkommen zum Verhör zwischen 2 Strategien");
            for (int i = 0; i < _strategien.Count; i++)
            {
                Console.WriteLine($"{i} - {_strategien[i].Name()}");
            }
            Console.WriteLine("Wählen Sie ihre 2 Strategien:");
            st1 = VerwaltungKram.EingabeZahlMinMax("Wählen Sie die 1. Strategie", 0, _strategien.Count);
            st2 = VerwaltungKram.EingabeZahlMinMax("Wählen Sie die 2. Strategie", 0, _strategien.Count);
            runde = VerwaltungKram.EingabeZahlMinMax("Wie viele Runden sollen diese verhört werden?", 1, 101);
            schwere = VerwaltungKram.EingabeZahlMinMax("Wie schwer sind die Verstöße? (0=Leicht,1=Mittel,2=schwer)", 0, 3);
            
            Verhoer(st1, st2, anzahlSpiele, runde, schwere);
        }

        /// <summary>
        /// Startet ein Verhör zwischen der Strategie an der Position st1 und Position st2 über die Länge von runde und der Schwere schwere
        /// </summary>
        /// <param name="st1"></param>
        /// <param name="st2"></param>
        /// <param name="runde"></param>
        /// <param name="schwere"></param>
        static void Verhoer(int st1, int st2, int anzahlSpiele, int runde, int schwere)
        {
            //holt die beiden Strategien aus der Collection.
            BasisStrategie strategie1 = _strategien[st1];
            BasisStrategie strategie2 = _strategien[st2];

            //setzt Startwerte
            int reaktion1 = BasisStrategie.NochNichtVerhoert;
            int reaktion2 = BasisStrategie.NochNichtVerhoert;
            int punkte1 = 0, punkte2 = 0;

            //beide Strategien über den Start informieren (Also es wird die Startmethode aufgerufen)
            strategie1.Start(runde, schwere);
            strategie2.Start(runde, schwere);
            
            Console.WriteLine($"Verhör zwischen {strategie1.Name()} und {strategie2.Name()} für {runde} Runden.");
            
            //start
            int anzahlGewonneneSpieleStrat1 = 0;
            int anzahlGewonneneSpieleStrat2 = 0;
            int siegerStehtFestInRunde = 0;
            bool siegerStehtFest = false;

            for (int i = 0; i < anzahlSpiele; i++)
            {
                int punkteDifferenz = 0;
                for (int j = 0; j < runde; j++)
                {
                    //beide verhören
                    int aktReaktion1 = strategie1.Verhoer(reaktion2);
                    int aktReaktion2 = strategie2.Verhoer(reaktion1);

                    //punkte berechnen
                    switch (schwere)
                    {
                        case 0:
                            VerhoerLeichtPunkte(aktReaktion1, aktReaktion2, ref punkte1, ref punkte2);
                            break;
                        case 1:
                            VerhoerMittelPunkte(aktReaktion1, aktReaktion2, ref punkte1, ref punkte2);
                            break;
                        default:
                            VerhoerSchwerPunkte(aktReaktion1, aktReaktion2, ref punkte1, ref punkte2);
                            break;
                    }
                    punkteDifferenz = punkte1 - punkte2;
                    
                    if(punkteDifferenz >= 10 || punkteDifferenz <= -10 && siegerStehtFest == false)
                    {
                        siegerStehtFestInRunde = siegerStehtFestInRunde + j;
                        siegerStehtFest = true;
                    }

                    //reaktion für den nächsten durchlauf merken
                    reaktion1 = aktReaktion1;
                    reaktion2 = aktReaktion2;
                }
                if(siegerStehtFestInRunde != 0)
                {
                    if( i == 0)
                    {
                        Console.WriteLine("Sieger steht fest ab Runde: " + (siegerStehtFestInRunde).ToString());
                    }
                    else
                    {
                        Console.WriteLine("Sieger steht fest ab Runde: " + (siegerStehtFestInRunde/i).ToString());
                    }
                }
                siegerStehtFest = false;

            //ausgabe   

                if (punkte1 < punkte2)
                {
                    Console.WriteLine($"{strategie1.Name()} hat {punkte1} Punkte erhalten.");
                    Console.WriteLine($"{strategie2.Name()} hat {punkte2} Punkte erhalten.");
                    Console.WriteLine("Somit hat {0} Spiel nr {1} gewonnen.", strategie1.Name(), i+1);
                    anzahlGewonneneSpieleStrat1++;
                } 
                else
                {
                    Console.WriteLine($"{strategie1.Name()} hat {punkte1} Punkte erhalten.");
                    Console.WriteLine($"{strategie2.Name()} hat {punkte2} Punkte erhalten.");
                    Console.WriteLine("Somit hat {0} Spiel nr {1} gewonnen.", strategie2.Name(), i+1);
                    anzahlGewonneneSpieleStrat2++;
                }
                punkte1 = 0;
                punkte2 = 0;
            }
            
            if(siegerStehtFestInRunde == 0)
            {
                 Console.WriteLine("Sieger konnte nicht zuverlässig ermittelt werden");
            }
            
            else
            {
                Console.WriteLine("Sieger steht zuverlässig fest ab Runde: " + (siegerStehtFestInRunde/anzahlSpiele).ToString());
            }
            
            Console.WriteLine($"{strategie1.Name()} hat {anzahlGewonneneSpieleStrat1} Spiele gewonnen.");
            Console.WriteLine($"{strategie2.Name()} hat {anzahlGewonneneSpieleStrat2} Spiele gewonnen.");
            if (anzahlGewonneneSpieleStrat1 > anzahlGewonneneSpieleStrat2)
            {
                Console.WriteLine("Somit hat {0} gewonnen.", strategie1.Name());
            } 
            else
            {
                Console.WriteLine("Somit hat {0} gewonnen.", strategie2.Name());
            }
            
        }

        /// <summary>
        /// Berechnet für leichte Verstöße die Punkte und verwendet die 2 letzten Eingabeparameter als Rückgabe
        /// </summary>
        /// <param name="aktReaktion1"></param>
        /// <param name="aktReaktion2"></param>
        /// <param name="punkte1"></param>
        /// <param name="punkte2"></param>
        static void VerhoerLeichtPunkte(int aktReaktion1, int aktReaktion2, ref int punkte1, ref int punkte2)
        {
            if (aktReaktion1 == BasisStrategie.Kooperieren && aktReaktion2 == BasisStrategie.Kooperieren)
            {
                punkte1 += 3;
                punkte2 += 3;
                return;
            } 
            if (aktReaktion1 == BasisStrategie.Verrat && aktReaktion2 == BasisStrategie.Kooperieren)
            {
                punkte1 += 0;
                punkte2 += 9;
                return;
            }
            if (aktReaktion1 == BasisStrategie.Kooperieren && aktReaktion2 == BasisStrategie.Verrat)
            {
                punkte1 += 9;
                punkte2 += 0;
                return;
            }
            
            punkte1 += 6;
            punkte2 += 6;
        }
        /// Berechnet für mittleren Verstöße die Punkte und verwendet die 2 letzten Eingabeparameter als Rückgabe
        /// <param name="aktReaktion1"></param>
        /// <param name="aktReaktion2"></param>
        /// <param name="punkte1"></param>
        /// <param name="punkte2"></param>
        static void VerhoerMittelPunkte(int aktReaktion1, int aktReaktion2, ref int punkte1, ref int punkte2)
        {
            if (aktReaktion1 == BasisStrategie.Kooperieren && aktReaktion2 == BasisStrategie.Kooperieren)
            {
                punkte1 += 10;
                punkte2 += 10;
                return;
            } 
            if (aktReaktion1 == BasisStrategie.Verrat && aktReaktion2 == BasisStrategie.Kooperieren)
            {
                punkte1 += 0;
                punkte2 += 8;
                return;
            }
            if (aktReaktion1 == BasisStrategie.Kooperieren && aktReaktion2 == BasisStrategie.Verrat)
            {
                punkte1 += 8;
                punkte2 += 0;
                return;
            }
            
            punkte1 += 4;
            punkte2 += 4;
        }

        /// Berechnet für schwere Verstöße die Punkte und verwendet die 2 letzten Eingabeparameter als Rückgabe
        /// <param name="aktReaktion1"></param>
        /// <param name="aktReaktion2"></param>
        /// <param name="punkte1"></param>
        /// <param name="punkte2"></param>
        static void VerhoerSchwerPunkte(int aktReaktion1, int aktReaktion2, ref int punkte1, ref int punkte2)
        {
            if (aktReaktion1 == BasisStrategie.Kooperieren && aktReaktion2 == BasisStrategie.Kooperieren)
            {
                punkte1 += 4;
                punkte2 += 4;
                return;
            } 
            if (aktReaktion1 == BasisStrategie.Verrat && aktReaktion2 == BasisStrategie.Kooperieren)
            {
                punkte1 += 0;
                punkte2 += 10;
                return;
            }
            if (aktReaktion1 == BasisStrategie.Kooperieren && aktReaktion2 == BasisStrategie.Verrat)
            {
                punkte1 += 10;
                punkte2 += 0;
                return;
            }
            
            punkte1 += 8;
            punkte2 += 8;
        }
    }
}